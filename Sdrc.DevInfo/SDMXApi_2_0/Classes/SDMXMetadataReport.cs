using System;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using System.Collections.Generic;
using SDMXApi_2_0.Common;

namespace SDMXApi_2_0.MetadataReport
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/metadatareport")]
    [System.Xml.Serialization.XmlRootAttribute("MetadataSet", Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/metadatareport", IsNullable = false)]
    public class MetadataSetType
    {

        private string metadataStructureRefField;

        private string metadataStructureAgencyRefField;

        private string versionField;

        private string metadataStructureURIField;

        private string datasetIDField;

        private string dataProviderSchemeAgencyIdField;

        private string dataProviderSchemeIdField;

        private string dataProviderIDField;

        private string dataflowAgencyIDField;

        private string dataflowIDField;

        private ActionType actionField;

        private bool actionFieldSpecified;

        private string reportingBeginDateField;

        private string reportingEndDateField;

        private string validFromDateField;

        private string validToDateField;

        private string publicationYearField;

        private string publicationPeriodField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string MetadataStructureRef
        {
            get
            {
                return this.metadataStructureRefField;
            }
            set
            {
                this.metadataStructureRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string MetadataStructureAgencyRef
        {
            get
            {
                return this.metadataStructureAgencyRefField;
            }
            set
            {
                this.metadataStructureAgencyRefField = value;
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

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string MetadataStructureURI
        {
            get
            {
                return this.metadataStructureURIField;
            }
            set
            {
                this.metadataStructureURIField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string datasetID
        {
            get
            {
                return this.datasetIDField;
            }
            set
            {
                this.datasetIDField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataProviderSchemeAgencyId
        {
            get
            {
                return this.dataProviderSchemeAgencyIdField;
            }
            set
            {
                this.dataProviderSchemeAgencyIdField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataProviderSchemeId
        {
            get
            {
                return this.dataProviderSchemeIdField;
            }
            set
            {
                this.dataProviderSchemeIdField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataProviderID
        {
            get
            {
                return this.dataProviderIDField;
            }
            set
            {
                this.dataProviderIDField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataflowAgencyID
        {
            get
            {
                return this.dataflowAgencyIDField;
            }
            set
            {
                this.dataflowAgencyIDField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string dataflowID
        {
            get
            {
                return this.dataflowIDField;
            }
            set
            {
                this.dataflowIDField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ActionType action
        {
            get
            {
                return this.actionField;
            }
            set
            {
                this.actionField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool actionSpecified
        {
            get
            {
                return this.actionFieldSpecified;
            }
            set
            {
                this.actionFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string reportingBeginDate
        {
            get
            {
                return this.reportingBeginDateField;
            }
            set
            {
                this.reportingBeginDateField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string reportingEndDate
        {
            get
            {
                return this.reportingEndDateField;
            }
            set
            {
                this.reportingEndDateField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validFromDate
        {
            get
            {
                return this.validFromDateField;
            }
            set
            {
                this.validFromDateField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string validToDate
        {
            get
            {
                return this.validToDateField;
            }
            set
            {
                this.validToDateField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "gYear")]
        public string publicationYear
        {
            get
            {
                return this.publicationYearField;
            }
            set
            {
                this.publicationYearField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string publicationPeriod
        {
            get
            {
                return this.publicationPeriodField;
            }
            set
            {
                this.publicationPeriodField = value;
            }
        }
    }

}
