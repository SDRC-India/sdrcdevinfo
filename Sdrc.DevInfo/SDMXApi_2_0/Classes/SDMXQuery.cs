using System;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using System.Collections.Generic;

namespace SDMXApi_2_0.Query
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    [System.Xml.Serialization.XmlRootAttribute("Query", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IsNullable = false)]
    public class QueryType
    {

        private List<DataWhereType> dataWhereField;

        private List<MetadataWhereType> metadataWhereField;

        private List<KeyFamilyWhereType> keyFamilyWhereField;

        private List<MetadataStructureWhereType> metadataStructureWhereField;

        private List<CodelistWhereType> codelistWhereField;

        private List<ConceptWhereType> conceptWhereField;

        private List<AgencyWhereType> agencyWhereField;

        private List<DataProviderWhereType> dataProviderWhereField;

        private List<HierarchicalCodelistWhereType> hierarchicalCodelistWhereField;

        private List<ReportingTaxonomyWhereType> reportingTaxonomyWhereField;

        private List<DataflowWhereType> dataflowWhereField;

        private List<MetadataflowWhereType> metadataflowWhereField;

        private List<StructureSetWhereType> structureSetWhereField;

        private List<ProcessWhereType> processWhereField;

        private List<OrganisationSchemeWhereType> organisationSchemeWhereField;

        private List<ConceptSchemeWhereType> conceptSchemeWhereField;

        private List<CategorySchemeWhereType> categorySchemeWhereField;

        private string defaultLimitField;

        public QueryType()
        {
            this.categorySchemeWhereField = new List<CategorySchemeWhereType>();
            this.conceptSchemeWhereField = new List<ConceptSchemeWhereType>();
            this.organisationSchemeWhereField = new List<OrganisationSchemeWhereType>();
            this.processWhereField = new List<ProcessWhereType>();
            this.structureSetWhereField = new List<StructureSetWhereType>();
            this.metadataflowWhereField = new List<MetadataflowWhereType>();
            this.dataflowWhereField = new List<DataflowWhereType>();
            this.reportingTaxonomyWhereField = new List<ReportingTaxonomyWhereType>();
            this.hierarchicalCodelistWhereField = new List<HierarchicalCodelistWhereType>();
            this.dataProviderWhereField = new List<DataProviderWhereType>();
            this.agencyWhereField = new List<AgencyWhereType>();
            this.conceptWhereField = new List<ConceptWhereType>();
            this.codelistWhereField = new List<CodelistWhereType>();
            this.metadataStructureWhereField = new List<MetadataStructureWhereType>();
            this.keyFamilyWhereField = new List<KeyFamilyWhereType>();
            this.metadataWhereField = new List<MetadataWhereType>();
            this.dataWhereField = new List<DataWhereType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("DataWhere", Order = 0)]
        public List<DataWhereType> DataWhere
        {
            get
            {
                return this.dataWhereField;
            }
            set
            {
                this.dataWhereField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("MetadataWhere", Order = 1)]
        public List<MetadataWhereType> MetadataWhere
        {
            get
            {
                return this.metadataWhereField;
            }
            set
            {
                this.metadataWhereField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("KeyFamilyWhere", Order = 2)]
        public List<KeyFamilyWhereType> KeyFamilyWhere
        {
            get
            {
                return this.keyFamilyWhereField;
            }
            set
            {
                this.keyFamilyWhereField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("MetadataStructureWhere", Order = 3)]
        public List<MetadataStructureWhereType> MetadataStructureWhere
        {
            get
            {
                return this.metadataStructureWhereField;
            }
            set
            {
                this.metadataStructureWhereField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("CodelistWhere", Order = 4)]
        public List<CodelistWhereType> CodelistWhere
        {
            get
            {
                return this.codelistWhereField;
            }
            set
            {
                this.codelistWhereField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ConceptWhere", Order = 5)]
        public List<ConceptWhereType> ConceptWhere
        {
            get
            {
                return this.conceptWhereField;
            }
            set
            {
                this.conceptWhereField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("AgencyWhere", Order = 6)]
        public List<AgencyWhereType> AgencyWhere
        {
            get
            {
                return this.agencyWhereField;
            }
            set
            {
                this.agencyWhereField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("DataProviderWhere", Order = 7)]
        public List<DataProviderWhereType> DataProviderWhere
        {
            get
            {
                return this.dataProviderWhereField;
            }
            set
            {
                this.dataProviderWhereField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("HierarchicalCodelistWhere", Order = 8)]
        public List<HierarchicalCodelistWhereType> HierarchicalCodelistWhere
        {
            get
            {
                return this.hierarchicalCodelistWhereField;
            }
            set
            {
                this.hierarchicalCodelistWhereField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ReportingTaxonomyWhere", Order = 9)]
        public List<ReportingTaxonomyWhereType> ReportingTaxonomyWhere
        {
            get
            {
                return this.reportingTaxonomyWhereField;
            }
            set
            {
                this.reportingTaxonomyWhereField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("DataflowWhere", Order = 10)]
        public List<DataflowWhereType> DataflowWhere
        {
            get
            {
                return this.dataflowWhereField;
            }
            set
            {
                this.dataflowWhereField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("MetadataflowWhere", Order = 11)]
        public List<MetadataflowWhereType> MetadataflowWhere
        {
            get
            {
                return this.metadataflowWhereField;
            }
            set
            {
                this.metadataflowWhereField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("StructureSetWhere", Order = 12)]
        public List<StructureSetWhereType> StructureSetWhere
        {
            get
            {
                return this.structureSetWhereField;
            }
            set
            {
                this.structureSetWhereField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ProcessWhere", Order = 13)]
        public List<ProcessWhereType> ProcessWhere
        {
            get
            {
                return this.processWhereField;
            }
            set
            {
                this.processWhereField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("OrganisationSchemeWhere", Order = 14)]
        public List<OrganisationSchemeWhereType> OrganisationSchemeWhere
        {
            get
            {
                return this.organisationSchemeWhereField;
            }
            set
            {
                this.organisationSchemeWhereField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ConceptSchemeWhere", Order = 15)]
        public List<ConceptSchemeWhereType> ConceptSchemeWhere
        {
            get
            {
                return this.conceptSchemeWhereField;
            }
            set
            {
                this.conceptSchemeWhereField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("CategorySchemeWhere", Order = 16)]
        public List<CategorySchemeWhereType> CategorySchemeWhere
        {
            get
            {
                return this.categorySchemeWhereField;
            }
            set
            {
                this.categorySchemeWhereField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string defaultLimit
        {
            get
            {
                return this.defaultLimitField;
            }
            set
            {
                this.defaultLimitField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IsNullable = true)]
    public class DataWhereType
    {

        private object itemField;

        private DataWhereChoiceType itemElementNameField;

        [System.Xml.Serialization.XmlElementAttribute("And", typeof(AndType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Attribute", typeof(AttributeType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Category", typeof(CategoryType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Codelist", typeof(CodelistType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Concept", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DataProvider", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DataSet", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Dataflow", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Dimension", typeof(DimensionType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("KeyFamily", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Or", typeof(OrType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Time", typeof(TimeType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Version", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public DataWhereChoiceType ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IsNullable = true)]
    public class AndType
    {

        private List<string> dataSetField;

        private List<string> metadataSetField;

        private List<string> keyFamilyField;

        private List<string> metadataStructureField;

        private List<DimensionType> dimensionField;

        private List<StructureComponentType> structureComponentField;

        private List<AttributeType> attributeField;

        private List<CodelistType> codelistField;

        private List<TimeType> timeField;

        private List<CategoryType> categoryField;

        private List<string> conceptField;

        private List<string> agencyIDField;

        private List<string> dataProviderField;

        private List<string> dataflowField;

        private List<string> metadataflowField;

        private List<string> versionField;

        private List<OrType> orField;

        private List<AndType> andField;

        public AndType()
        {
            this.andField = new List<AndType>();
            this.orField = new List<OrType>();
            this.versionField = new List<string>();
            this.metadataflowField = new List<string>();
            this.dataflowField = new List<string>();
            this.dataProviderField = new List<string>();
            this.agencyIDField = new List<string>();
            this.conceptField = new List<string>();
            this.categoryField = new List<CategoryType>();
            this.timeField = new List<TimeType>();
            this.codelistField = new List<CodelistType>();
            this.attributeField = new List<AttributeType>();
            this.structureComponentField = new List<StructureComponentType>();
            this.dimensionField = new List<DimensionType>();
            this.metadataStructureField = new List<string>();
            this.keyFamilyField = new List<string>();
            this.metadataSetField = new List<string>();
            this.dataSetField = new List<string>();
        }

        [System.Xml.Serialization.XmlElementAttribute("DataSet", Order = 0)]
        public List<string> DataSet
        {
            get
            {
                return this.dataSetField;
            }
            set
            {
                this.dataSetField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("MetadataSet", Order = 1)]
        public List<string> MetadataSet
        {
            get
            {
                return this.metadataSetField;
            }
            set
            {
                this.metadataSetField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("KeyFamily", Order = 2)]
        public List<string> KeyFamily
        {
            get
            {
                return this.keyFamilyField;
            }
            set
            {
                this.keyFamilyField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("MetadataStructure", Order = 3)]
        public List<string> MetadataStructure
        {
            get
            {
                return this.metadataStructureField;
            }
            set
            {
                this.metadataStructureField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Dimension", Order = 4)]
        public List<DimensionType> Dimension
        {
            get
            {
                return this.dimensionField;
            }
            set
            {
                this.dimensionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("StructureComponent", Order = 5)]
        public List<StructureComponentType> StructureComponent
        {
            get
            {
                return this.structureComponentField;
            }
            set
            {
                this.structureComponentField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Attribute", Order = 6)]
        public List<AttributeType> Attribute
        {
            get
            {
                return this.attributeField;
            }
            set
            {
                this.attributeField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Codelist", Order = 7)]
        public List<CodelistType> Codelist
        {
            get
            {
                return this.codelistField;
            }
            set
            {
                this.codelistField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Time", Order = 8)]
        public List<TimeType> Time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Category", Order = 9)]
        public List<CategoryType> Category
        {
            get
            {
                return this.categoryField;
            }
            set
            {
                this.categoryField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Concept", Order = 10)]
        public List<string> Concept
        {
            get
            {
                return this.conceptField;
            }
            set
            {
                this.conceptField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("AgencyID", Order = 11)]
        public List<string> AgencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("DataProvider", Order = 12)]
        public List<string> DataProvider
        {
            get
            {
                return this.dataProviderField;
            }
            set
            {
                this.dataProviderField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Dataflow", Order = 13)]
        public List<string> Dataflow
        {
            get
            {
                return this.dataflowField;
            }
            set
            {
                this.dataflowField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Metadataflow", Order = 14)]
        public List<string> Metadataflow
        {
            get
            {
                return this.metadataflowField;
            }
            set
            {
                this.metadataflowField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Version", Order = 15)]
        public List<string> Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Or", Order = 16)]
        public List<OrType> Or
        {
            get
            {
                return this.orField;
            }
            set
            {
                this.orField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("And", Order = 17)]
        public List<AndType> And
        {
            get
            {
                return this.andField;
            }
            set
            {
                this.andField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IsNullable = true)]
    public class DimensionType
    {

        private string idField;

        private string valueField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IsNullable = true)]
    public class CategorySchemeWhereType
    {

        private string agencyIDField;

        private string idField;

        private string versionField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string AgencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IsNullable = true)]
    public class ConceptSchemeWhereType
    {

        private string agencyIDField;

        private string idField;

        private string versionField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string AgencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IsNullable = true)]
    public class OrganisationSchemeWhereType
    {

        private string agencyIDField;

        private string idField;

        private string versionField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string AgencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IsNullable = true)]
    public class ProcessWhereType
    {

        private string agencyIDField;

        private string idField;

        private string versionField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string AgencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IsNullable = true)]
    public class StructureSetWhereType
    {

        private string agencyIDField;

        private string idField;

        private string versionField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string AgencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IsNullable = true)]
    public class MetadataflowWhereType
    {

        private string agencyIDField;

        private string idField;

        private string versionField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string AgencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IsNullable = true)]
    public class DataflowWhereType
    {

        private string agencyIDField;

        private string idField;

        private string versionField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string AgencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IsNullable = true)]
    public class ReportingTaxonomyWhereType
    {

        private string agencyIDField;

        private string idField;

        private string versionField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string AgencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IsNullable = true)]
    public class HierarchicalCodelistWhereType
    {

        private string agencyIDField;

        private string idField;

        private string versionField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string AgencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IsNullable = true)]
    public class DataProviderWhereType
    {

        private object[] itemsField;

        private DataProviderWhereChoiceType[] itemsElementNameField;

        public DataProviderWhereType()
        {
            //this.itemsElementNameField = new List<DataProviderWhereChoiceType>();
            //this.itemsField = new List<object>();
        }

        [System.Xml.Serialization.XmlElementAttribute("AgencyID", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("And", typeof(AndType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Category", typeof(CategoryType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Codelist", typeof(CodelistType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Concept", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DataSet", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("KeyFamily", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataSet", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataStructure", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Or", typeof(OrType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("StructureSet", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public object[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName", Order = 1)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public DataProviderWhereChoiceType[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IsNullable = true)]
    public class CategoryType
    {

        private string idField;

        private string agencyIDField;

        private string categorySchemeField;

        private string versionField;

        private string valueField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string agencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CategoryScheme
        {
            get
            {
                return this.categorySchemeField;
            }
            set
            {
                this.categorySchemeField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IsNullable = true)]
    public class CodelistType
    {

        private string idField;

        private string valueField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IsNullable = true)]
    public class OrType
    {

        private List<string> dataSetField;

        private List<string> metadataSetField;

        private List<string> keyFamilyField;

        private List<string> metadataStructureField;

        private List<DimensionType> dimensionField;

        private List<StructureComponentType> structureComponentField;

        private List<AttributeType> attributeField;

        private List<CodelistType> codelistField;

        private List<TimeType> timeField;

        private List<CategoryType> categoryField;

        private List<string> conceptField;

        private List<string> agencyIDField;

        private List<string> dataProviderField;

        private List<string> dataflowField;

        private List<string> metadataflowField;

        private List<string> versionField;

        private List<OrType> orField;

        private List<AndType> andField;

        public OrType()
        {
            this.andField = new List<AndType>();
            this.orField = new List<OrType>();
            this.versionField = new List<string>();
            this.metadataflowField = new List<string>();
            this.dataflowField = new List<string>();
            this.dataProviderField = new List<string>();
            this.agencyIDField = new List<string>();
            this.conceptField = new List<string>();
            this.categoryField = new List<CategoryType>();
            this.timeField = new List<TimeType>();
            this.codelistField = new List<CodelistType>();
            this.attributeField = new List<AttributeType>();
            this.structureComponentField = new List<StructureComponentType>();
            this.dimensionField = new List<DimensionType>();
            this.metadataStructureField = new List<string>();
            this.keyFamilyField = new List<string>();
            this.metadataSetField = new List<string>();
            this.dataSetField = new List<string>();
        }

        [System.Xml.Serialization.XmlElementAttribute("DataSet", Order = 0)]
        public List<string> DataSet
        {
            get
            {
                return this.dataSetField;
            }
            set
            {
                this.dataSetField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("MetadataSet", Order = 1)]
        public List<string> MetadataSet
        {
            get
            {
                return this.metadataSetField;
            }
            set
            {
                this.metadataSetField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("KeyFamily", Order = 2)]
        public List<string> KeyFamily
        {
            get
            {
                return this.keyFamilyField;
            }
            set
            {
                this.keyFamilyField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("MetadataStructure", Order = 3)]
        public List<string> MetadataStructure
        {
            get
            {
                return this.metadataStructureField;
            }
            set
            {
                this.metadataStructureField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Dimension", Order = 4)]
        public List<DimensionType> Dimension
        {
            get
            {
                return this.dimensionField;
            }
            set
            {
                this.dimensionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("StructureComponent", Order = 5)]
        public List<StructureComponentType> StructureComponent
        {
            get
            {
                return this.structureComponentField;
            }
            set
            {
                this.structureComponentField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Attribute", Order = 6)]
        public List<AttributeType> Attribute
        {
            get
            {
                return this.attributeField;
            }
            set
            {
                this.attributeField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Codelist", Order = 7)]
        public List<CodelistType> Codelist
        {
            get
            {
                return this.codelistField;
            }
            set
            {
                this.codelistField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Time", Order = 8)]
        public List<TimeType> Time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Category", Order = 9)]
        public List<CategoryType> Category
        {
            get
            {
                return this.categoryField;
            }
            set
            {
                this.categoryField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Concept", Order = 10)]
        public List<string> Concept
        {
            get
            {
                return this.conceptField;
            }
            set
            {
                this.conceptField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("AgencyID", Order = 11)]
        public List<string> AgencyID
        {
            get
            {
                return this.agencyIDField;
            }
            set
            {
                this.agencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("DataProvider", Order = 12)]
        public List<string> DataProvider
        {
            get
            {
                return this.dataProviderField;
            }
            set
            {
                this.dataProviderField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Dataflow", Order = 13)]
        public List<string> Dataflow
        {
            get
            {
                return this.dataflowField;
            }
            set
            {
                this.dataflowField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Metadataflow", Order = 14)]
        public List<string> Metadataflow
        {
            get
            {
                return this.metadataflowField;
            }
            set
            {
                this.metadataflowField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Version", Order = 15)]
        public List<string> Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Or", Order = 16)]
        public List<OrType> Or
        {
            get
            {
                return this.orField;
            }
            set
            {
                this.orField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("And", Order = 17)]
        public List<AndType> And
        {
            get
            {
                return this.andField;
            }
            set
            {
                this.andField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IsNullable = true)]
    public class StructureComponentType
    {

        private string idField;

        private string valueField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IsNullable = true)]
    public class AttributeType
    {

        private string idField;

        private AttachmentLevelType attachmentLevelField;

        private string valueField;

        public AttributeType()
        {
            this.attachmentLevelField = AttachmentLevelType.Any;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(AttachmentLevelType.Any)]
        public AttachmentLevelType attachmentLevel
        {
            get
            {
                return this.attachmentLevelField;
            }
            set
            {
                this.attachmentLevelField = value;
            }
        }

        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    public enum AttachmentLevelType
    {

        /// <remarks/>
        DataSet,

        /// <remarks/>
        Group,

        /// <remarks/>
        Series,

        /// <remarks/>
        Observation,

        /// <remarks/>
        Any,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IsNullable = true)]
    public class TimeType
    {

        private string[] itemsField;

        private TimeChoiceType[] itemsElementNameField;

        public TimeType()
        {
            //this.itemsElementNameField = new List<TimeChoiceType>();
            //this.itemsField = new List<string>();
        }

        [System.Xml.Serialization.XmlElementAttribute("EndTime", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("StartTime", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Time", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public string[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName", Order = 1)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public TimeChoiceType[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IncludeInSchema = false)]
    public enum TimeChoiceType
    {

        /// <remarks/>
        EndTime,

        /// <remarks/>
        StartTime,

        /// <remarks/>
        Time,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IncludeInSchema = false)]
    public enum DataProviderWhereChoiceType
    {

        /// <remarks/>
        AgencyID,

        /// <remarks/>
        And,

        /// <remarks/>
        Category,

        /// <remarks/>
        Codelist,

        /// <remarks/>
        Concept,

        /// <remarks/>
        DataSet,

        /// <remarks/>
        KeyFamily,

        /// <remarks/>
        MetadataSet,

        /// <remarks/>
        MetadataStructure,

        /// <remarks/>
        Or,

        /// <remarks/>
        StructureSet,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IsNullable = true)]
    public class AgencyWhereType
    {

        private object[] itemsField;

        private AgencyWhereChoiceType[] itemsElementNameField;

        public AgencyWhereType()
        {
            //this.itemsElementNameField = new List<AgencyWhereChoiceType>();
            //this.itemsField = new List<object>();
        }

        [System.Xml.Serialization.XmlElementAttribute("AgencyID", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("And", typeof(AndType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Category", typeof(CategoryType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Codelist", typeof(CodelistType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Concept", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("KeyFamily", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataStructure", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Or", typeof(OrType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("StructureSet", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public object[] Items
        {
            get
            {
                return this.itemsField;
            }
            set
            {
                this.itemsField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName", Order = 1)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public AgencyWhereChoiceType[] ItemsElementName
        {
            get
            {
                return this.itemsElementNameField;
            }
            set
            {
                this.itemsElementNameField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IncludeInSchema = false)]
    public enum AgencyWhereChoiceType
    {

        /// <remarks/>
        AgencyID,

        /// <remarks/>
        And,

        /// <remarks/>
        Category,

        /// <remarks/>
        Codelist,

        /// <remarks/>
        Concept,

        /// <remarks/>
        KeyFamily,

        /// <remarks/>
        MetadataStructure,

        /// <remarks/>
        Or,

        /// <remarks/>
        StructureSet,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IsNullable = true)]
    public class ConceptWhereType
    {

        private object itemField;

        private ConceptWhereChoiceType itemElementNameField;

        [System.Xml.Serialization.XmlElementAttribute("AgencyID", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("And", typeof(AndType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Concept", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Or", typeof(OrType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Version", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ConceptWhereChoiceType ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IncludeInSchema = false)]
    public enum ConceptWhereChoiceType
    {

        /// <remarks/>
        AgencyID,

        /// <remarks/>
        And,

        /// <remarks/>
        Concept,

        /// <remarks/>
        Or,

        /// <remarks/>
        Version,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IsNullable = true)]
    public class CodelistWhereType
    {

        private object itemField;

        private CodelistWhereChoiceType itemElementNameField;

        [System.Xml.Serialization.XmlElementAttribute("AgencyID", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("And", typeof(AndType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Codelist", typeof(CodelistType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Or", typeof(OrType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Version", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public CodelistWhereChoiceType ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IncludeInSchema = false)]
    public enum CodelistWhereChoiceType
    {

        /// <remarks/>
        AgencyID,

        /// <remarks/>
        And,

        /// <remarks/>
        Codelist,

        /// <remarks/>
        Or,

        /// <remarks/>
        Version,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IsNullable = true)]
    public class MetadataStructureWhereType
    {

        private object itemField;

        private MetadataStructureWhereChoiceType itemElementNameField;

        [System.Xml.Serialization.XmlElementAttribute("AgencyID", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("And", typeof(AndType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Attribute", typeof(AttributeType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Category", typeof(CategoryType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Codelist", typeof(CodelistType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Concept", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Dimension", typeof(DimensionType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("KeyFamily", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataStructure", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Or", typeof(OrType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("StructureComponent", typeof(StructureComponentType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("StructureSet", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Version", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public MetadataStructureWhereChoiceType ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IncludeInSchema = false)]
    public enum MetadataStructureWhereChoiceType
    {

        /// <remarks/>
        AgencyID,

        /// <remarks/>
        And,

        /// <remarks/>
        Attribute,

        /// <remarks/>
        Category,

        /// <remarks/>
        Codelist,

        /// <remarks/>
        Concept,

        /// <remarks/>
        Dimension,

        /// <remarks/>
        KeyFamily,

        /// <remarks/>
        MetadataStructure,

        /// <remarks/>
        Or,

        /// <remarks/>
        StructureComponent,

        /// <remarks/>
        StructureSet,

        /// <remarks/>
        Version,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IsNullable = true)]
    public class KeyFamilyWhereType
    {

        private object itemField;

        private KeyFamilyWhereChoiceType itemElementNameField;

        [System.Xml.Serialization.XmlElementAttribute("AgencyID", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("And", typeof(AndType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Attribute", typeof(AttributeType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Category", typeof(CategoryType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Codelist", typeof(CodelistType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Concept", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Dimension", typeof(DimensionType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("KeyFamily", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Or", typeof(OrType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Version", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public KeyFamilyWhereChoiceType ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IncludeInSchema = false)]
    public enum KeyFamilyWhereChoiceType
    {

        /// <remarks/>
        AgencyID,

        /// <remarks/>
        And,

        /// <remarks/>
        Attribute,

        /// <remarks/>
        Category,

        /// <remarks/>
        Codelist,

        /// <remarks/>
        Concept,

        /// <remarks/>
        Dimension,

        /// <remarks/>
        KeyFamily,

        /// <remarks/>
        Or,

        /// <remarks/>
        Version,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IsNullable = true)]
    public class MetadataWhereType
    {

        private object itemField;

        private MetadataWhereChoiceType itemElementNameField;

        [System.Xml.Serialization.XmlElementAttribute("And", typeof(AndType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Attribute", typeof(AttributeType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Category", typeof(CategoryType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Codelist", typeof(CodelistType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Concept", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("DataProvider", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataSet", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("MetadataStructure", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Metadataflow", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Or", typeof(OrType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("StructureComponent", typeof(StructureComponentType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Time", typeof(TimeType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Version", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public object Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public MetadataWhereChoiceType ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IncludeInSchema = false)]
    public enum MetadataWhereChoiceType
    {

        /// <remarks/>
        And,

        /// <remarks/>
        Attribute,

        /// <remarks/>
        Category,

        /// <remarks/>
        Codelist,

        /// <remarks/>
        Concept,

        /// <remarks/>
        DataProvider,

        /// <remarks/>
        MetadataSet,

        /// <remarks/>
        MetadataStructure,

        /// <remarks/>
        Metadataflow,

        /// <remarks/>
        Or,

        /// <remarks/>
        StructureComponent,

        /// <remarks/>
        Time,

        /// <remarks/>
        Version,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/query", IncludeInSchema = false)]
    public enum DataWhereChoiceType
    {

        /// <remarks/>
        And,

        /// <remarks/>
        Attribute,

        /// <remarks/>
        Category,

        /// <remarks/>
        Codelist,

        /// <remarks/>
        Concept,

        /// <remarks/>
        DataProvider,

        /// <remarks/>
        DataSet,

        /// <remarks/>
        Dataflow,

        /// <remarks/>
        Dimension,

        /// <remarks/>
        KeyFamily,

        /// <remarks/>
        Or,

        /// <remarks/>
        Time,

        /// <remarks/>
        Version,
    }
    
}
