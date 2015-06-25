using System;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using System.Collections.Generic;
using SDMXObjectModel.Structure;
using SDMXObjectModel.Message.Footer;
using SDMXObjectModel.Query;
using SDMXObjectModel.Common;
using SDMXObjectModel.Registry;
using SDMXObjectModel.Data.StructureSpecific;
using SDMXObjectModel.Metadata.Generic;

namespace SDMXObjectModel.Message
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("Structure", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class StructureType
    {

        private StructureHeaderType headerField;

        private StructuresType structuresField;

        private List<FooterMessageType> footerField;

        public StructureType()
        {
            this.footerField = new List<FooterMessageType>();
            this.structuresField = new StructuresType();
            this.headerField = new StructureHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public StructureHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public StructuresType Structures
        {
            get
            {
                return this.structuresField;
            }
            set
            {
                this.structuresField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message/footer", Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Message", IsNullable = false)]
        public List<FooterMessageType> Footer
        {
            get
            {
                return this.footerField;
            }
            set
            {
                this.footerField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = true)]
    public class StructureHeaderType
    {
        #region "Variables"

        #region "Private"
        
        private string idField;

        private bool testField;

        private string preparedField;

        private SenderType senderField;

        private List<PartyType> receiverField;

        private List<TextType> nameField;

        private List<TextType> sourceField;

        #endregion "Private"

        #region "Public"

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public bool Test
        {
            get
            {
                return this.testField;
            }
            set
            {
                this.testField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Prepared
        {
            get
            {
                return this.preparedField;
            }
            set
            {
                this.preparedField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public SenderType Sender
        {
            get
            {
                return this.senderField;
            }
            set
            {
                this.senderField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Receiver", Order = 4)]
        public List<PartyType> Receiver
        {
            get
            {
                return this.receiverField;
            }
            set
            {
                this.receiverField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", Order = 5)]
        public List<TextType> Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Source", Order = 6)]
        public List<TextType> Source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }
        #endregion "Public"

        #endregion "Variables"

        #region "Constructors"

        public StructureHeaderType()
            : this(string.Empty, false, DateTime.Now, null, null)
        {
        }

        public StructureHeaderType(string id, bool test, DateTime prepared, SenderType sender, PartyType receiver)
        {
            this.idField = id;
            this.testField = test;
            this.preparedField = prepared.ToString(Constants.DateFormat) + Constants.DateTimeSeparator + prepared.ToString(Constants.TimeFormat);
            this.senderField = sender;

            if (receiver != null)
            {
                this.receiverField = new List<PartyType>();
                this.receiverField.Add(receiver);
            }

            this.sourceField = new List<TextType>();
            this.nameField = new List<TextType>();
        }

        #endregion "Constructors"
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = true)]
    public class SenderType : PartyType
    {
        #region "Variables"

        #region "Private"

        private string timezoneField;

        #endregion "Private"

        #region "Public"

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Timezone
        {
            get
            {
                return this.timezoneField;
            }
            set
            {
                this.timezoneField = value;
            }
        }

        #endregion "Public"

        #endregion "Variables"

        #region "Constructors"

        public SenderType()
            : this(string.Empty, string.Empty, string.Empty, null)
        {
        }

        public SenderType(string id, string name, string language, ContactType contact)
            : base(id, name, language, contact)
        {
        }

        #endregion "Constructors"
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(SenderType))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = true)]
    public class PartyType
    {
        #region "Variables"

        #region "Private"

        private List<TextType> nameField;

        private List<ContactType> contactField;

        private string idField;

        #endregion "Private"

        #region "Public"

        [System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", Order = 0)]
        public List<TextType> Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Contact", Order = 1)]
        public List<ContactType> Contact
        {
            get
            {
                return this.contactField;
            }
            set
            {
                this.contactField = value;
            }
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

        #endregion "Public"

        #endregion "Variables"

        #region "Constructors"

        public PartyType()
            : this(string.Empty, string.Empty, string.Empty, null)
        {
        }

        public PartyType(string id, string name, string language, ContactType contact)
        {
            this.id = id;

            if (!string.IsNullOrEmpty(name))
            {
                this.nameField = new List<TextType>();
                this.nameField.Add(new TextType(language, name));
            }

            if (contact != null)
            {
                this.contactField = new List<ContactType>();
                this.contactField.Add(contact);
            }
        }

        #endregion "Constructors"
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ConstraintQueryType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("ConstraintQuery", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class ConstraintQueryType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Query.ConstraintQueryType queryField;

        public ConstraintQueryType()
        {
            this.queryField = new SDMXObjectModel.Query.ConstraintQueryType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Query.ConstraintQueryType Query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = true)]
    public class BasicHeaderType
    {
        #region "Variables"

        #region "Private"

        private string idField;

        private bool testField;

        private string preparedField;

        private SenderType senderField;

        private PartyType receiverField;

        #endregion "Private"

        #region "Public"

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public bool Test
        {
            get
            {
                return this.testField;
            }
            set
            {
                this.testField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Prepared
        {
            get
            {
                return this.preparedField;
            }
            set
            {
                this.preparedField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public SenderType Sender
        {
            get
            {
                return this.senderField;
            }
            set
            {
                this.senderField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public PartyType Receiver
        {
            get
            {
                return this.receiverField;
            }
            set
            {
                this.receiverField = value;
            }
        }

        #endregion "Public"

        #endregion "Variables"

        #region "Constructors"

        public BasicHeaderType()
            : this(string.Empty, false, DateTime.Now, null, null)
        {
        }

        public BasicHeaderType(string id, bool test, DateTime prepared, SenderType sender, PartyType receiver)
        {
            this.idField = id;
            this.testField = test;
            this.preparedField = prepared.ToString(Constants.DateFormat) + Constants.DateTimeSeparator + prepared.ToString(Constants.TimeFormat);
            this.senderField = sender;
            this.receiverField = receiver;
        }

        #endregion "Constructors"
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ProvisionAgreementQueryType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("ProvisionAgreementQuery", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class ProvisionAgreementQueryType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Query.ProvisionAgreementQueryType queryField;

        public ProvisionAgreementQueryType()
        {
            this.queryField = new SDMXObjectModel.Query.ProvisionAgreementQueryType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Query.ProvisionAgreementQueryType Query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "CategorisationQueryType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("CategorisationQuery", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class CategorisationQueryType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Query.CategorisationQueryType queryField;

        public CategorisationQueryType()
        {
            this.queryField = new SDMXObjectModel.Query.CategorisationQueryType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Query.CategorisationQueryType Query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ProcessQueryType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("ProcessQuery", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class ProcessQueryType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Query.ProcessQueryType queryField;

        public ProcessQueryType()
        {
            this.queryField = new SDMXObjectModel.Query.ProcessQueryType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Query.ProcessQueryType Query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "StructureSetQueryType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("StructureSetQuery", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class StructureSetQueryType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Query.StructureSetQueryType queryField;

        public StructureSetQueryType()
        {
            this.queryField = new SDMXObjectModel.Query.StructureSetQueryType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Query.StructureSetQueryType Query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ReportingTaxonomyQueryType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("ReportingTaxonomyQuery", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class ReportingTaxonomyQueryType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Query.ReportingTaxonomyQueryType queryField;

        public ReportingTaxonomyQueryType()
        {
            this.queryField = new SDMXObjectModel.Query.ReportingTaxonomyQueryType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Query.ReportingTaxonomyQueryType Query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "OrganisationSchemeQueryType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("OrganisationSchemeQuery", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class OrganisationSchemeQueryType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Query.OrganisationSchemeQueryType queryField;

        public OrganisationSchemeQueryType()
        {
            this.queryField = new SDMXObjectModel.Query.OrganisationSchemeQueryType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Query.OrganisationSchemeQueryType Query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "HierarchicalCodelistQueryType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("HierarchicalCodelistQuery", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class HierarchicalCodelistQueryType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Query.HierarchicalCodelistQueryType queryField;

        public HierarchicalCodelistQueryType()
        {
            this.queryField = new SDMXObjectModel.Query.HierarchicalCodelistQueryType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Query.HierarchicalCodelistQueryType Query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "CodelistQueryType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("CodelistQuery", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class CodelistQueryType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Query.CodelistQueryType queryField;

        public CodelistQueryType()
        {
            this.queryField = new SDMXObjectModel.Query.CodelistQueryType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Query.CodelistQueryType Query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "ConceptSchemeQueryType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("ConceptSchemeQuery", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class ConceptSchemeQueryType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Query.ConceptSchemeQueryType queryField;

        public ConceptSchemeQueryType()
        {
            this.queryField = new SDMXObjectModel.Query.ConceptSchemeQueryType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Query.ConceptSchemeQueryType Query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "CategorySchemeQueryType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("CategorySchemeQuery", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class CategorySchemeQueryType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Query.CategorySchemeQueryType queryField;

        public CategorySchemeQueryType()
        {
            this.queryField = new SDMXObjectModel.Query.CategorySchemeQueryType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Query.CategorySchemeQueryType Query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "MetadataStructureQueryType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("MetadataStructureQuery", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class MetadataStructureQueryType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Query.MetadataStructureQueryType queryField;

        public MetadataStructureQueryType()
        {
            this.queryField = new SDMXObjectModel.Query.MetadataStructureQueryType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Query.MetadataStructureQueryType Query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "DataStructureQueryType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("DataStructureQuery", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class DataStructureQueryType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Query.DataStructureQueryType queryField;

        public DataStructureQueryType()
        {
            this.queryField = new SDMXObjectModel.Query.DataStructureQueryType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Query.DataStructureQueryType Query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "MetadataflowQueryType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("MetadataflowQuery", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class MetadataflowQueryType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Query.MetadataflowQueryType queryField;

        public MetadataflowQueryType()
        {
            this.queryField = new SDMXObjectModel.Query.MetadataflowQueryType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Query.MetadataflowQueryType Query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "DataflowQueryType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("DataflowQuery", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class DataflowQueryType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Query.DataflowQueryType queryField;

        public DataflowQueryType()
        {
            this.queryField = new SDMXObjectModel.Query.DataflowQueryType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Query.DataflowQueryType Query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("StructuresQuery", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class StructuresQueryType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Query.StructuresQueryType queryField;

        public StructuresQueryType()
        {
            this.queryField = new SDMXObjectModel.Query.StructuresQueryType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Query.StructuresQueryType Query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("MetadataSchemaQuery", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class MetadataSchemaQueryType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Query.MetadataSchemaQueryType queryField;

        public MetadataSchemaQueryType()
        {
            this.queryField = new SDMXObjectModel.Query.MetadataSchemaQueryType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Query.MetadataSchemaQueryType Query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("DataSchemaQuery", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class DataSchemaQueryType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Query.DataSchemaQueryType queryField;

        public DataSchemaQueryType()
        {
            this.queryField = new SDMXObjectModel.Query.DataSchemaQueryType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Query.DataSchemaQueryType Query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("GenericMetadataQuery", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class MetadataQueryType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Query.MetadataQueryType queryField;

        public MetadataQueryType()
        {
            this.queryField = new SDMXObjectModel.Query.MetadataQueryType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Query.MetadataQueryType Query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("StructureSpecificTimeSeriesDataQuery", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class StructureSpecificTimeSeriesDataQueryType
    {

        private BasicHeaderType headerField;

        private TimeSeriesDataQueryType queryField;

        public StructureSpecificTimeSeriesDataQueryType()
        {
            this.queryField = new TimeSeriesDataQueryType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public TimeSeriesDataQueryType Query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "GenericTimeSeriesDataQueryType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("GenericTimeSeriesDataQuery", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class GenericTimeSeriesDataQueryType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Query.GenericTimeSeriesDataQueryType queryField;

        public GenericTimeSeriesDataQueryType()
        {
            this.queryField = new SDMXObjectModel.Query.GenericTimeSeriesDataQueryType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Query.GenericTimeSeriesDataQueryType Query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "GenericDataQueryType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("GenericDataQuery", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class GenericDataQueryType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Query.GenericDataQueryType queryField;

        public GenericDataQueryType()
        {
            this.queryField = new SDMXObjectModel.Query.GenericDataQueryType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Query.GenericDataQueryType Query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("StructureSpecificDataQuery", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class DataQueryType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Query.DataQueryType queryField;

        public DataQueryType()
        {
            this.queryField = new SDMXObjectModel.Query.DataQueryType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Query.DataQueryType Query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "NotifyRegistryEventType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("NotifyRegistryEvent", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class NotifyRegistryEventType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Registry.NotifyRegistryEventType notifyRegistryEventField;

        public NotifyRegistryEventType()
        {
            this.notifyRegistryEventField = new SDMXObjectModel.Registry.NotifyRegistryEventType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Registry.NotifyRegistryEventType NotifyRegistryEvent
        {
            get
            {
                return this.notifyRegistryEventField;
            }
            set
            {
                this.notifyRegistryEventField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "QuerySubscriptionResponseType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("QuerySubscriptionResponse", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class QuerySubscriptionResponseType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Registry.QuerySubscriptionResponseType querySubscriptionResponseField;

        private List<FooterMessageType> footerField;

        public QuerySubscriptionResponseType()
        {
            this.footerField = new List<FooterMessageType>();
            this.querySubscriptionResponseField = new SDMXObjectModel.Registry.QuerySubscriptionResponseType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Registry.QuerySubscriptionResponseType QuerySubscriptionResponse
        {
            get
            {
                return this.querySubscriptionResponseField;
            }
            set
            {
                this.querySubscriptionResponseField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message/footer", Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Message", IsNullable = false)]
        public List<FooterMessageType> Footer
        {
            get
            {
                return this.footerField;
            }
            set
            {
                this.footerField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "QuerySubscriptionRequestType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("QuerySubscriptionRequest", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class QuerySubscriptionRequestType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Registry.QuerySubscriptionRequestType querySubscriptionRequestField;

        public QuerySubscriptionRequestType()
        {
            this.querySubscriptionRequestField = new SDMXObjectModel.Registry.QuerySubscriptionRequestType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Registry.QuerySubscriptionRequestType QuerySubscriptionRequest
        {
            get
            {
                return this.querySubscriptionRequestField;
            }
            set
            {
                this.querySubscriptionRequestField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "SubmitSubscriptionsResponseType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("SubmitSubscriptionsResponse", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class SubmitSubscriptionsResponseType
    {

        private BasicHeaderType headerField;

        private List<SubscriptionStatusType> submitSubscriptionsResponseField;

        private List<FooterMessageType> footerField;

        public SubmitSubscriptionsResponseType()
        {
            this.footerField = new List<FooterMessageType>();
            this.submitSubscriptionsResponseField = new List<SubscriptionStatusType>();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("SubscriptionStatus", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = false)]
        public List<SubscriptionStatusType> SubmitSubscriptionsResponse
        {
            get
            {
                return this.submitSubscriptionsResponseField;
            }
            set
            {
                this.submitSubscriptionsResponseField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message/footer", Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Message", IsNullable = false)]
        public List<FooterMessageType> Footer
        {
            get
            {
                return this.footerField;
            }
            set
            {
                this.footerField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "SubmitStructureResponseType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("SubmitStructureResponse", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class SubmitStructureResponseType
    {
        #region "Variables"

        #region "Private"

        private BasicHeaderType headerField;

        private List<SubmissionResultType> submitStructureResponseField;

        private List<FooterMessageType> footerField;

        #endregion "Private"

        #region "Public"

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("SubmissionResult", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = false)]
        public List<SubmissionResultType> SubmitStructureResponse
        {
            get
            {
                return this.submitStructureResponseField;
            }
            set
            {
                this.submitStructureResponseField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message/footer", Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Message", IsNullable = false)]
        public List<FooterMessageType> Footer
        {
            get
            {
                return this.footerField;
            }
            set
            {
                this.footerField = value;
            }
        }

        #endregion "Public"

        #endregion "Variables"

        #region "Constructors"

        public SubmitStructureResponseType():this(null, null, null)
        {
            this.footerField = new List<FooterMessageType>();
            this.submitStructureResponseField = new List<SubmissionResultType>();
            this.headerField = new BasicHeaderType();
        }

        public SubmitStructureResponseType(BasicHeaderType basicHeader, SubmissionResultType submissionResult, FooterMessageType footerMessage)
        {
            this.headerField = basicHeader;

            if (submissionResult != null)
            {
                this.submitStructureResponseField = new List<SubmissionResultType>();
                this.submitStructureResponseField.Add(submissionResult);
            }

            if (footerMessage != null)
            {
                this.footerField = new List<FooterMessageType>();
                this.footerField.Add(footerMessage);
            }
        }

        #endregion "Constructors"
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "SubmitStructureRequestType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("SubmitStructureRequest", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class SubmitStructureRequestType
    {
        #region "Variables"

        #region "Private"

        private BasicHeaderType headerField;

        private SDMXObjectModel.Registry.SubmitStructureRequestType submitStructureRequestField;

        #endregion "Private"

        #region "Public"

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Registry.SubmitStructureRequestType SubmitStructureRequest
        {
            get
            {
                return this.submitStructureRequestField;
            }
            set
            {
                this.submitStructureRequestField = value;
            }
        }

        #endregion "Public"

        #endregion "Variables"

        #region "Constructors"

        public SubmitStructureRequestType()
        {
            this.submitStructureRequestField = new SDMXObjectModel.Registry.SubmitStructureRequestType();
            this.headerField = new BasicHeaderType();
        }

        #endregion "Constructors"
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "QueryRegistrationResponseType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("QueryRegistrationResponse", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class QueryRegistrationResponseType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Registry.QueryRegistrationResponseType queryRegistrationResponseField;

        private List<FooterMessageType> footerField;

        public QueryRegistrationResponseType()
        {
            this.footerField = new List<FooterMessageType>();
            this.queryRegistrationResponseField = new SDMXObjectModel.Registry.QueryRegistrationResponseType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Registry.QueryRegistrationResponseType QueryRegistrationResponse
        {
            get
            {
                return this.queryRegistrationResponseField;
            }
            set
            {
                this.queryRegistrationResponseField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message/footer", Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Message", IsNullable = false)]
        public List<FooterMessageType> Footer
        {
            get
            {
                return this.footerField;
            }
            set
            {
                this.footerField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "QueryRegistrationRequestType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("QueryRegistrationRequest", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class QueryRegistrationRequestType
    {

        private BasicHeaderType headerField;

        private SDMXObjectModel.Registry.QueryRegistrationRequestType queryRegistrationRequestField;

        public QueryRegistrationRequestType()
        {
            this.queryRegistrationRequestField = new SDMXObjectModel.Registry.QueryRegistrationRequestType();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public SDMXObjectModel.Registry.QueryRegistrationRequestType QueryRegistrationRequest
        {
            get
            {
                return this.queryRegistrationRequestField;
            }
            set
            {
                this.queryRegistrationRequestField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "SubmitRegistrationsResponseType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("SubmitRegistrationsResponse", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class SubmitRegistrationsResponseType
    {

        private BasicHeaderType headerField;

        private List<RegistrationStatusType> submitRegistrationsResponseField;

        private List<FooterMessageType> footerField;

        public SubmitRegistrationsResponseType()
        {
            this.footerField = new List<FooterMessageType>();
            this.submitRegistrationsResponseField = new List<RegistrationStatusType>();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("RegistrationStatus", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = false)]
        public List<RegistrationStatusType> SubmitRegistrationsResponse
        {
            get
            {
                return this.submitRegistrationsResponseField;
            }
            set
            {
                this.submitRegistrationsResponseField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message/footer", Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Message", IsNullable = false)]
        public List<FooterMessageType> Footer
        {
            get
            {
                return this.footerField;
            }
            set
            {
                this.footerField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "SubmitRegistrationsRequestType", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("SubmitRegistrationsRequest", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class SubmitRegistrationsRequestType
    {

        private BasicHeaderType headerField;

        private List<RegistrationRequestType> submitRegistrationsRequestField;

        public SubmitRegistrationsRequestType()
        {
            this.submitRegistrationsRequestField = new List<RegistrationRequestType>();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Order = 1)]
        [System.Xml.Serialization.XmlArrayItemAttribute("RegistrationRequest", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/registry", IsNullable = false)]
        public List<RegistrationRequestType> SubmitRegistrationsRequest
        {
            get
            {
                return this.submitRegistrationsRequestField;
            }
            set
            {
                this.submitRegistrationsRequestField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("RegistryInterface", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class RegistryInterfaceType
    {

        private BasicHeaderType headerField;

        private object itemField;

        private List<FooterMessageType> footerField;

        public RegistryInterfaceType()
        {
            this.footerField = new List<FooterMessageType>();
            this.headerField = new BasicHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BasicHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("NotifyRegistryEvent", typeof(SDMXObjectModel.Registry.NotifyRegistryEventType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("QueryRegistrationRequest", typeof(SDMXObjectModel.Registry.QueryRegistrationRequestType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("QueryRegistrationResponse", typeof(SDMXObjectModel.Registry.QueryRegistrationResponseType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("QuerySubscriptionRequest", typeof(SDMXObjectModel.Registry.QuerySubscriptionRequestType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("QuerySubscriptionResponse", typeof(SDMXObjectModel.Registry.QuerySubscriptionResponseType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("SubmitRegistrationsRequest", typeof(SDMXObjectModel.Registry.SubmitRegistrationsRequestType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("SubmitRegistrationsResponse", typeof(SDMXObjectModel.Registry.SubmitRegistrationsResponseType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("SubmitStructureRequest", typeof(SDMXObjectModel.Registry.SubmitStructureRequestType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("SubmitStructureResponse", typeof(SDMXObjectModel.Registry.SubmitStructureResponseType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("SubmitSubscriptionsRequest", typeof(SDMXObjectModel.Registry.SubmitSubscriptionsRequestType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("SubmitSubscriptionsResponse", typeof(SDMXObjectModel.Registry.SubmitSubscriptionsResponseType), Order = 1)]
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

        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message/footer", Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Message", IsNullable = false)]
        public List<FooterMessageType> Footer
        {
            get
            {
                return this.footerField;
            }
            set
            {
                this.footerField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = true)]
    public class StructureSpecificMetadataHeaderType
    {

        private string idField;

        private bool testField;

        private string preparedField;

        private SenderType senderField;

        private List<PartyType> receiverField;

        private List<TextType> nameField;

        private List<StructureSpecificMetadataStructureType> structureField;

        private DataProviderReferenceType dataProviderField;

        private ActionType dataSetActionField;

        private bool dataSetActionFieldSpecified;

        private List<string> dataSetIDField;

        private System.DateTime extractedField;

        private bool extractedFieldSpecified;

        private List<TextType> sourceField;

        public StructureSpecificMetadataHeaderType()
        {
            this.sourceField = new List<TextType>();
            this.dataSetIDField = new List<string>();
            this.structureField = new List<StructureSpecificMetadataStructureType>();
            this.nameField = new List<TextType>();
            this.receiverField = new List<PartyType>();
            this.senderField = new SenderType();
            this.testField = false;
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public bool Test
        {
            get
            {
                return this.testField;
            }
            set
            {
                this.testField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Prepared
        {
            get
            {
                return this.preparedField;
            }
            set
            {
                this.preparedField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public SenderType Sender
        {
            get
            {
                return this.senderField;
            }
            set
            {
                this.senderField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Receiver", Order = 4)]
        public List<PartyType> Receiver
        {
            get
            {
                return this.receiverField;
            }
            set
            {
                this.receiverField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", Order = 5)]
        public List<TextType> Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Structure", Order = 6)]
        public List<StructureSpecificMetadataStructureType> Structure
        {
            get
            {
                return this.structureField;
            }
            set
            {
                this.structureField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public DataProviderReferenceType DataProvider
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public ActionType DataSetAction
        {
            get
            {
                return this.dataSetActionField;
            }
            set
            {
                this.dataSetActionField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DataSetActionSpecified
        {
            get
            {
                return this.dataSetActionFieldSpecified;
            }
            set
            {
                this.dataSetActionFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("DataSetID", Order = 9)]
        public List<string> DataSetID
        {
            get
            {
                return this.dataSetIDField;
            }
            set
            {
                this.dataSetIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public System.DateTime Extracted
        {
            get
            {
                return this.extractedField;
            }
            set
            {
                this.extractedField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExtractedSpecified
        {
            get
            {
                return this.extractedFieldSpecified;
            }
            set
            {
                this.extractedFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Source", Order = 11)]
        public List<TextType> Source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("StructureSpecificMetadata", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class StructureSpecificMetadataType
    {

        private StructureSpecificMetadataHeaderType headerField;

        private List<SDMXObjectModel.Metadata.StructureSpecific.MetadataSetType> metadataSetField;

        private List<FooterMessageType> footerField;

        public StructureSpecificMetadataType()
        {
            this.footerField = new List<FooterMessageType>();
            this.metadataSetField = new List<SDMXObjectModel.Metadata.StructureSpecific.MetadataSetType>();
            this.headerField = new StructureSpecificMetadataHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public StructureSpecificMetadataHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("MetadataSet", Order = 1)]
        public List<SDMXObjectModel.Metadata.StructureSpecific.MetadataSetType> MetadataSet
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

        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message/footer", Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Message", IsNullable = false)]
        public List<FooterMessageType> Footer
        {
            get
            {
                return this.footerField;
            }
            set
            {
                this.footerField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = true)]
    public class GenericMetadataHeaderType
    {
        #region "Variables"

        #region "Private"

        private string idField;

        private bool testField;

        private string preparedField;

        private SenderType senderField;

        private List<PartyType> receiverField;

        private List<TextType> nameField;

        private List<GenericMetadataStructureType> structureField;

        private DataProviderReferenceType dataProviderField;

        private ActionType dataSetActionField;

        private bool dataSetActionFieldSpecified;

        private List<string> dataSetIDField;

        private System.DateTime extractedField;

        private bool extractedFieldSpecified;

        private List<TextType> sourceField;

        #endregion "Private"

        #region "Public"

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public bool Test
        {
            get
            {
                return this.testField;
            }
            set
            {
                this.testField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Prepared
        {
            get
            {
                return this.preparedField;
            }
            set
            {
                this.preparedField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public SenderType Sender
        {
            get
            {
                return this.senderField;
            }
            set
            {
                this.senderField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Receiver", Order = 4)]
        public List<PartyType> Receiver
        {
            get
            {
                return this.receiverField;
            }
            set
            {
                this.receiverField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", Order = 5)]
        public List<TextType> Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Structure", Order = 6)]
        public List<GenericMetadataStructureType> Structure
        {
            get
            {
                return this.structureField;
            }
            set
            {
                this.structureField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public DataProviderReferenceType DataProvider
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public ActionType DataSetAction
        {
            get
            {
                return this.dataSetActionField;
            }
            set
            {
                this.dataSetActionField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DataSetActionSpecified
        {
            get
            {
                return this.dataSetActionFieldSpecified;
            }
            set
            {
                this.dataSetActionFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("DataSetID", Order = 9)]
        public List<string> DataSetID
        {
            get
            {
                return this.dataSetIDField;
            }
            set
            {
                this.dataSetIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public System.DateTime Extracted
        {
            get
            {
                return this.extractedField;
            }
            set
            {
                this.extractedField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExtractedSpecified
        {
            get
            {
                return this.extractedFieldSpecified;
            }
            set
            {
                this.extractedFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Source", Order = 11)]
        public List<TextType> Source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }

        #endregion "Public"

        #endregion "Variables"

        #region "Constructors"

        public GenericMetadataHeaderType()
            : this(string.Empty, false, DateTime.Now, null, null)
        {
        }

        public GenericMetadataHeaderType(string id, bool test, DateTime prepared, SenderType sender, PartyType receiver)
        {
            this.idField = id;
            this.testField = test;
            this.preparedField = prepared.ToString(Constants.DateFormat) + Constants.DateTimeSeparator + prepared.ToString(Constants.TimeFormat);
            this.senderField = sender;

            if (receiver != null)
            {
                this.receiverField = new List<PartyType>();
                this.receiverField.Add(receiver);
            }
        }

        #endregion "Constructors"
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("GenericMetadata", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class GenericMetadataType
    {

        private GenericMetadataHeaderType headerField;

        private List<MetadataSetType> metadataSetField;

        private List<FooterMessageType> footerField;

        public GenericMetadataType()
        {
            this.footerField = new List<FooterMessageType>();
            this.metadataSetField = new List<MetadataSetType>();
            this.headerField = new GenericMetadataHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public GenericMetadataHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("MetadataSet", Order = 1)]
        public List<MetadataSetType> MetadataSet
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

        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message/footer", Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Message", IsNullable = false)]
        public List<FooterMessageType> Footer
        {
            get
            {
                return this.footerField;
            }
            set
            {
                this.footerField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = true)]
    public class StructureSpecificTimeSeriesDataHeaderType
    {
        #region "Variables"

        #region "Private"

        private string idField;

        private bool testField;

        private string preparedField;

        private SenderType senderField;

        private List<PartyType> receiverField;

        private List<TextType> nameField;

        private List<StructureSpecificDataTimeSeriesStructureType> structureField;

        private DataProviderReferenceType dataProviderField;

        private ActionType dataSetActionField;

        private bool dataSetActionFieldSpecified;

        private List<string> dataSetIDField;

        private System.DateTime extractedField;

        private bool extractedFieldSpecified;

        private string reportingBeginField;

        private string reportingEndField;

        private System.DateTime embargoDateField;

        private bool embargoDateFieldSpecified;

        private List<TextType> sourceField;

        #endregion "Private"

        #region "Public"

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public bool Test
        {
            get
            {
                return this.testField;
            }
            set
            {
                this.testField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Prepared
        {
            get
            {
                return this.preparedField;
            }
            set
            {
                this.preparedField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public SenderType Sender
        {
            get
            {
                return this.senderField;
            }
            set
            {
                this.senderField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Receiver", Order = 4)]
        public List<PartyType> Receiver
        {
            get
            {
                return this.receiverField;
            }
            set
            {
                this.receiverField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", Order = 5)]
        public List<TextType> Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Structure", Order = 6)]
        public List<StructureSpecificDataTimeSeriesStructureType> Structure
        {
            get
            {
                return this.structureField;
            }
            set
            {
                this.structureField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public DataProviderReferenceType DataProvider
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public ActionType DataSetAction
        {
            get
            {
                return this.dataSetActionField;
            }
            set
            {
                this.dataSetActionField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DataSetActionSpecified
        {
            get
            {
                return this.dataSetActionFieldSpecified;
            }
            set
            {
                this.dataSetActionFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("DataSetID", Order = 9)]
        public List<string> DataSetID
        {
            get
            {
                return this.dataSetIDField;
            }
            set
            {
                this.dataSetIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public System.DateTime Extracted
        {
            get
            {
                return this.extractedField;
            }
            set
            {
                this.extractedField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExtractedSpecified
        {
            get
            {
                return this.extractedFieldSpecified;
            }
            set
            {
                this.extractedFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public string ReportingBegin
        {
            get
            {
                return this.reportingBeginField;
            }
            set
            {
                this.reportingBeginField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public string ReportingEnd
        {
            get
            {
                return this.reportingEndField;
            }
            set
            {
                this.reportingEndField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public System.DateTime EmbargoDate
        {
            get
            {
                return this.embargoDateField;
            }
            set
            {
                this.embargoDateField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EmbargoDateSpecified
        {
            get
            {
                return this.embargoDateFieldSpecified;
            }
            set
            {
                this.embargoDateFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Source", Order = 14)]
        public List<TextType> Source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }

        #endregion "Public"

        #endregion "Varaibles"

        #region "Constructors"

        public StructureSpecificTimeSeriesDataHeaderType()
            : this(string.Empty, false, DateTime.Now, null, null)
        {
        }

        public StructureSpecificTimeSeriesDataHeaderType(string id, bool test, DateTime prepared, SenderType sender, PartyType receiver)
        {
            this.idField = id;
            this.testField = test;
            this.preparedField = prepared.ToString(Constants.DateFormat) + Constants.DateTimeSeparator + prepared.ToString(Constants.TimeFormat);
            this.senderField = sender;

            if (receiver != null)
            {
                this.receiverField = new List<PartyType>();
                this.receiverField.Add(receiver);
            }

            this.sourceField = new List<TextType>();
            this.dataSetIDField = new List<string>();
            this.structureField = new List<StructureSpecificDataTimeSeriesStructureType>();
            this.nameField = new List<TextType>();
        }

        #endregion "Constructors"
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("StructureSpecificTimeSeriesData", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class StructureSpecificTimeSeriesDataType
    {

        private StructureSpecificTimeSeriesDataHeaderType headerField;

        private List<SDMXObjectModel.Data.StructureSpecific.TimeSeriesDataSetType> dataSetField;

        private List<FooterMessageType> footerField;

        public StructureSpecificTimeSeriesDataType()
        {
            this.footerField = new List<FooterMessageType>();
            this.dataSetField = new List<SDMXObjectModel.Data.StructureSpecific.TimeSeriesDataSetType>();
            this.headerField = new StructureSpecificTimeSeriesDataHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public StructureSpecificTimeSeriesDataHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("DataSet", Order = 1)]
        public List<SDMXObjectModel.Data.StructureSpecific.TimeSeriesDataSetType> DataSet
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

        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message/footer", Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Message", IsNullable = false)]
        public List<FooterMessageType> Footer
        {
            get
            {
                return this.footerField;
            }
            set
            {
                this.footerField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = true)]
    public class StructureSpecificDataHeaderType
    {
        #region "Variables"

        #region "Private"

        private string idField;

        private bool testField;

        private string preparedField;

        private SenderType senderField;

        private List<PartyType> receiverField;

        private List<TextType> nameField;

        private List<StructureSpecificDataStructureType> structureField;

        private DataProviderReferenceType dataProviderField;

        private ActionType dataSetActionField;

        private bool dataSetActionFieldSpecified;

        private List<string> dataSetIDField;

        private System.DateTime extractedField;

        private bool extractedFieldSpecified;

        private string reportingBeginField;

        private string reportingEndField;

        private System.DateTime embargoDateField;

        private bool embargoDateFieldSpecified;

        private List<TextType> sourceField;

        #endregion "Private"

        #region "Public"
        
        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public bool Test
        {
            get
            {
                return this.testField;
            }
            set
            {
                this.testField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Prepared
        {
            get
            {
                return this.preparedField;
            }
            set
            {
                this.preparedField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public SenderType Sender
        {
            get
            {
                return this.senderField;
            }
            set
            {
                this.senderField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Receiver", Order = 4)]
        public List<PartyType> Receiver
        {
            get
            {
                return this.receiverField;
            }
            set
            {
                this.receiverField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", Order = 5)]
        public List<TextType> Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Structure", Order = 6)]
        public List<StructureSpecificDataStructureType> Structure
        {
            get
            {
                return this.structureField;
            }
            set
            {
                this.structureField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public DataProviderReferenceType DataProvider
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public ActionType DataSetAction
        {
            get
            {
                return this.dataSetActionField;
            }
            set
            {
                this.dataSetActionField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DataSetActionSpecified
        {
            get
            {
                return this.dataSetActionFieldSpecified;
            }
            set
            {
                this.dataSetActionFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("DataSetID", Order = 9)]
        public List<string> DataSetID
        {
            get
            {
                return this.dataSetIDField;
            }
            set
            {
                this.dataSetIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public System.DateTime Extracted
        {
            get
            {
                return this.extractedField;
            }
            set
            {
                this.extractedField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExtractedSpecified
        {
            get
            {
                return this.extractedFieldSpecified;
            }
            set
            {
                this.extractedFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public string ReportingBegin
        {
            get
            {
                return this.reportingBeginField;
            }
            set
            {
                this.reportingBeginField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public string ReportingEnd
        {
            get
            {
                return this.reportingEndField;
            }
            set
            {
                this.reportingEndField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public System.DateTime EmbargoDate
        {
            get
            {
                return this.embargoDateField;
            }
            set
            {
                this.embargoDateField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EmbargoDateSpecified
        {
            get
            {
                return this.embargoDateFieldSpecified;
            }
            set
            {
                this.embargoDateFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Source", Order = 14)]
        public List<TextType> Source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }

        #endregion "Public"

        #endregion "Variables"

        #region "Constructors"

        public StructureSpecificDataHeaderType()
            : this(string.Empty, false, DateTime.Now, null, null)
        {
        }

        public StructureSpecificDataHeaderType(string id, bool test, DateTime prepared, SenderType sender, PartyType receiver)
        {
            this.idField = id;
            this.testField = test;
            this.preparedField = prepared.ToString(Constants.DateFormat) + Constants.DateTimeSeparator + prepared.ToString(Constants.TimeFormat);
            this.senderField = sender;

            if (receiver != null)
            {
                this.receiverField = new List<PartyType>();
                this.receiverField.Add(receiver);
            }

            this.sourceField = new List<TextType>();
            this.dataSetIDField = new List<string>();
            this.structureField = new List<StructureSpecificDataStructureType>();
            this.nameField = new List<TextType>();
        }

        #endregion "Constructors"
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("StructureSpecificData", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class StructureSpecificDataType
    {

        private StructureSpecificDataHeaderType headerField;

        private List<SDMXObjectModel.Data.StructureSpecific.DataSetType> dataSetField;

        private List<FooterMessageType> footerField;

        public StructureSpecificDataType()
        {
            this.footerField = new List<FooterMessageType>();
            this.dataSetField = new List<SDMXObjectModel.Data.StructureSpecific.DataSetType>();
            this.headerField = new StructureSpecificDataHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public StructureSpecificDataHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("DataSet", Order = 1)]
        public List<SDMXObjectModel.Data.StructureSpecific.DataSetType> DataSet
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

        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message/footer", Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Message", IsNullable = false)]
        public List<FooterMessageType> Footer
        {
            get
            {
                return this.footerField;
            }
            set
            {
                this.footerField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = true)]
    public class GenericTimeSeriesDataHeaderType
    {
        #region "Variables"

        #region "Private"

        private string idField;

        private bool testField;

        private string preparedField;

        private SenderType senderField;

        private List<PartyType> receiverField;

        private List<TextType> nameField;

        private GenericTimeSeriesDataStructureType structureField;

        private DataProviderReferenceType dataProviderField;

        private ActionType dataSetActionField;

        private bool dataSetActionFieldSpecified;

        private List<string> dataSetIDField;

        private System.DateTime extractedField;

        private bool extractedFieldSpecified;

        private string reportingBeginField;

        private string reportingEndField;

        private System.DateTime embargoDateField;

        private bool embargoDateFieldSpecified;

        private List<TextType> sourceField;

        #endregion "Private"

        #region "Public"

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public bool Test
        {
            get
            {
                return this.testField;
            }
            set
            {
                this.testField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Prepared
        {
            get
            {
                return this.preparedField;
            }
            set
            {
                this.preparedField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public SenderType Sender
        {
            get
            {
                return this.senderField;
            }
            set
            {
                this.senderField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Receiver", Order = 4)]
        public List<PartyType> Receiver
        {
            get
            {
                return this.receiverField;
            }
            set
            {
                this.receiverField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", Order = 5)]
        public List<TextType> Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 6)]
        public GenericTimeSeriesDataStructureType Structure
        {
            get
            {
                return this.structureField;
            }
            set
            {
                this.structureField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public DataProviderReferenceType DataProvider
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public ActionType DataSetAction
        {
            get
            {
                return this.dataSetActionField;
            }
            set
            {
                this.dataSetActionField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DataSetActionSpecified
        {
            get
            {
                return this.dataSetActionFieldSpecified;
            }
            set
            {
                this.dataSetActionFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("DataSetID", Order = 9)]
        public List<string> DataSetID
        {
            get
            {
                return this.dataSetIDField;
            }
            set
            {
                this.dataSetIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public System.DateTime Extracted
        {
            get
            {
                return this.extractedField;
            }
            set
            {
                this.extractedField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExtractedSpecified
        {
            get
            {
                return this.extractedFieldSpecified;
            }
            set
            {
                this.extractedFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public string ReportingBegin
        {
            get
            {
                return this.reportingBeginField;
            }
            set
            {
                this.reportingBeginField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public string ReportingEnd
        {
            get
            {
                return this.reportingEndField;
            }
            set
            {
                this.reportingEndField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public System.DateTime EmbargoDate
        {
            get
            {
                return this.embargoDateField;
            }
            set
            {
                this.embargoDateField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EmbargoDateSpecified
        {
            get
            {
                return this.embargoDateFieldSpecified;
            }
            set
            {
                this.embargoDateFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Source", Order = 14)]
        public List<TextType> Source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }

        #endregion "Public"

        #endregion "Variables"

        #region "Constructors"

        public GenericTimeSeriesDataHeaderType()
            : this(string.Empty, false, DateTime.Now, null, null)
        {
        }

        public GenericTimeSeriesDataHeaderType(string id, bool test, DateTime prepared, SenderType sender, PartyType receiver)
        {
            this.idField = id;
            this.testField = test;
            this.preparedField = prepared.ToString(Constants.DateFormat) + Constants.DateTimeSeparator + prepared.ToString(Constants.TimeFormat);
            this.senderField = sender;

            if (receiver != null)
            {
                this.receiverField = new List<PartyType>();
                this.receiverField.Add(receiver);
            }

            this.sourceField = new List<TextType>();
            this.dataSetIDField = new List<string>();
            this.structureField = new GenericTimeSeriesDataStructureType();
            this.nameField = new List<TextType>();
        }

        #endregion "Constructors"
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("GenericTimeSeriesData", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class GenericTimeSeriesDataType
    {

        private GenericTimeSeriesDataHeaderType headerField;

        private List<SDMXObjectModel.Data.Generic.TimeSeriesDataSetType> dataSetField;

        private List<FooterMessageType> footerField;

        public GenericTimeSeriesDataType()
        {
            this.footerField = new List<FooterMessageType>();
            this.dataSetField = new List<SDMXObjectModel.Data.Generic.TimeSeriesDataSetType>();
            this.headerField = new GenericTimeSeriesDataHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public GenericTimeSeriesDataHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("DataSet", Order = 1)]
        public List<SDMXObjectModel.Data.Generic.TimeSeriesDataSetType> DataSet
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

        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message/footer", Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Message", IsNullable = false)]
        public List<FooterMessageType> Footer
        {
            get
            {
                return this.footerField;
            }
            set
            {
                this.footerField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = true)]
    public class GenericDataHeaderType
    {
        #region "Variables"

        #region "Private"

        private string idField;

        private bool testField;

        private string preparedField;

        private SenderType senderField;

        private List<PartyType> receiverField;

        private List<TextType> nameField;

        private List<GenericDataStructureType> structureField;

        private DataProviderReferenceType dataProviderField;

        private ActionType dataSetActionField;

        private bool dataSetActionFieldSpecified;

        private List<string> dataSetIDField;

        private System.DateTime extractedField;

        private bool extractedFieldSpecified;

        private string reportingBeginField;

        private string reportingEndField;

        private System.DateTime embargoDateField;

        private bool embargoDateFieldSpecified;

        private List<TextType> sourceField;

        #endregion "Private"

        #region "Public"

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public bool Test
        {
            get
            {
                return this.testField;
            }
            set
            {
                this.testField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Prepared
        {
            get
            {
                return this.preparedField;
            }
            set
            {
                this.preparedField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public SenderType Sender
        {
            get
            {
                return this.senderField;
            }
            set
            {
                this.senderField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Receiver", Order = 4)]
        public List<PartyType> Receiver
        {
            get
            {
                return this.receiverField;
            }
            set
            {
                this.receiverField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", Order = 5)]
        public List<TextType> Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Structure", Order = 6)]
        public List<GenericDataStructureType> Structure
        {
            get
            {
                return this.structureField;
            }
            set
            {
                this.structureField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public DataProviderReferenceType DataProvider
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public ActionType DataSetAction
        {
            get
            {
                return this.dataSetActionField;
            }
            set
            {
                this.dataSetActionField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DataSetActionSpecified
        {
            get
            {
                return this.dataSetActionFieldSpecified;
            }
            set
            {
                this.dataSetActionFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("DataSetID", Order = 9)]
        public List<string> DataSetID
        {
            get
            {
                return this.dataSetIDField;
            }
            set
            {
                this.dataSetIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public System.DateTime Extracted
        {
            get
            {
                return this.extractedField;
            }
            set
            {
                this.extractedField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExtractedSpecified
        {
            get
            {
                return this.extractedFieldSpecified;
            }
            set
            {
                this.extractedFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public string ReportingBegin
        {
            get
            {
                return this.reportingBeginField;
            }
            set
            {
                this.reportingBeginField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public string ReportingEnd
        {
            get
            {
                return this.reportingEndField;
            }
            set
            {
                this.reportingEndField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public System.DateTime EmbargoDate
        {
            get
            {
                return this.embargoDateField;
            }
            set
            {
                this.embargoDateField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EmbargoDateSpecified
        {
            get
            {
                return this.embargoDateFieldSpecified;
            }
            set
            {
                this.embargoDateFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Source", Order = 14)]
        public List<TextType> Source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }

        #endregion "Public"

        #endregion "Variables"

        #region "Constructors"

        public GenericDataHeaderType()
            : this(string.Empty, false, DateTime.Now, null, null)
        {
        }

        public GenericDataHeaderType(string id, bool test, DateTime prepared, SenderType sender, PartyType receiver)
        {
            this.idField = id;
            this.testField = test;
            this.preparedField = prepared.ToString(Constants.DateFormat) + Constants.DateTimeSeparator + prepared.ToString(Constants.TimeFormat);
            this.senderField = sender;

            if (receiver != null)
            {
                this.receiverField = new List<PartyType>();
                this.receiverField.Add(receiver);
            }

            this.sourceField = new List<TextType>();
            this.dataSetIDField = new List<string>();
            this.structureField = new List<GenericDataStructureType>();
            this.nameField = new List<TextType>();
        }

        #endregion "Constructors"
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("GenericData", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class GenericDataType
    {

        private GenericDataHeaderType headerField;

        private List<SDMXObjectModel.Data.Generic.DataSetType> dataSetField;

        private List<FooterMessageType> footerField;

        public GenericDataType()
        {
            this.footerField = new List<FooterMessageType>();
            this.dataSetField = new List<SDMXObjectModel.Data.Generic.DataSetType>();
            this.headerField = new GenericDataHeaderType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public GenericDataHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("DataSet", Order = 1)]
        public List<SDMXObjectModel.Data.Generic.DataSetType> DataSet
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

        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message/footer", Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Message", IsNullable = false)]
        public List<FooterMessageType> Footer
        {
            get
            {
                return this.footerField;
            }
            set
            {
                this.footerField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = true)]
    public abstract class BaseHeaderType
    {

        private string idField;

        private bool testField;

        private string preparedField;

        private SenderType senderField;

        private List<PartyType> receiverField;

        private List<TextType> nameField;

        private List<PayloadStructureType> structureField;

        private DataProviderReferenceType dataProviderField;

        private ActionType dataSetActionField;

        private bool dataSetActionFieldSpecified;

        private List<string> dataSetIDField;

        private System.DateTime extractedField;

        private bool extractedFieldSpecified;

        private string reportingBeginField;

        private string reportingEndField;

        private System.DateTime embargoDateField;

        private bool embargoDateFieldSpecified;

        private List<TextType> sourceField;

        public BaseHeaderType()
        {
            this.sourceField = new List<TextType>();
            this.dataSetIDField = new List<string>();
            this.structureField = new List<PayloadStructureType>();
            this.nameField = new List<TextType>();
            this.receiverField = new List<PartyType>();
            this.senderField = new SenderType();
            this.testField = false;
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public bool Test
        {
            get
            {
                return this.testField;
            }
            set
            {
                this.testField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Prepared
        {
            get
            {
                return this.preparedField;
            }
            set
            {
                this.preparedField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public SenderType Sender
        {
            get
            {
                return this.senderField;
            }
            set
            {
                this.senderField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Receiver", Order = 4)]
        public List<PartyType> Receiver
        {
            get
            {
                return this.receiverField;
            }
            set
            {
                this.receiverField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", Order = 5)]
        public List<TextType> Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Structure", Order = 6)]
        public List<PayloadStructureType> Structure
        {
            get
            {
                return this.structureField;
            }
            set
            {
                this.structureField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 7)]
        public DataProviderReferenceType DataProvider
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 8)]
        public ActionType DataSetAction
        {
            get
            {
                return this.dataSetActionField;
            }
            set
            {
                this.dataSetActionField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DataSetActionSpecified
        {
            get
            {
                return this.dataSetActionFieldSpecified;
            }
            set
            {
                this.dataSetActionFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("DataSetID", Order = 9)]
        public List<string> DataSetID
        {
            get
            {
                return this.dataSetIDField;
            }
            set
            {
                this.dataSetIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 10)]
        public System.DateTime Extracted
        {
            get
            {
                return this.extractedField;
            }
            set
            {
                this.extractedField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ExtractedSpecified
        {
            get
            {
                return this.extractedFieldSpecified;
            }
            set
            {
                this.extractedFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 11)]
        public string ReportingBegin
        {
            get
            {
                return this.reportingBeginField;
            }
            set
            {
                this.reportingBeginField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 12)]
        public string ReportingEnd
        {
            get
            {
                return this.reportingEndField;
            }
            set
            {
                this.reportingEndField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 13)]
        public System.DateTime EmbargoDate
        {
            get
            {
                return this.embargoDateField;
            }
            set
            {
                this.embargoDateField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EmbargoDateSpecified
        {
            get
            {
                return this.embargoDateFieldSpecified;
            }
            set
            {
                this.embargoDateFieldSpecified = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Source", Order = 14)]
        public List<TextType> Source
        {
            get
            {
                return this.sourceField;
            }
            set
            {
                this.sourceField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = true)]
    public abstract class MessageType
    {

        private BaseHeaderType headerField;

        private List<System.Xml.XmlElement> anyField;

        private List<FooterMessageType> footerField;

        public MessageType()
        {
            this.footerField = new List<FooterMessageType>();
            this.anyField = new List<System.Xml.XmlElement>();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public BaseHeaderType Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        [System.Xml.Serialization.XmlAnyElementAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", Order = 1)]
        public List<System.Xml.XmlElement> Any
        {
            get
            {
                return this.anyField;
            }
            set
            {
                this.anyField = value;
            }
        }

        [System.Xml.Serialization.XmlArrayAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message/footer", Order = 2)]
        [System.Xml.Serialization.XmlArrayItemAttribute("Message", IsNullable = false)]
        public List<FooterMessageType> Footer
        {
            get
            {
                return this.footerField;
            }
            set
            {
                this.footerField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = true)]
    public class ContactType
    {
        #region "Variables"

        #region "Private"

        private List<TextType> nameField;

        private List<TextType> departmentField;

        private List<TextType> roleField;

        private string[] itemsField;

        private ContactChoiceType[] itemsElementNameField;

        #endregion "Private"

        #region "Public"

        [System.Xml.Serialization.XmlElementAttribute("Name", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/common", Order = 0)]
        public List<TextType> Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Department", Order = 1)]
        public List<TextType> Department
        {
            get
            {
                return this.departmentField;
            }
            set
            {
                this.departmentField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Role", Order = 2)]
        public List<TextType> Role
        {
            get
            {
                return this.roleField;
            }
            set
            {
                this.roleField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("Email", typeof(string), Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("Fax", typeof(string), Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("Telephone", typeof(string), Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("URI", typeof(string), DataType = "anyURI", Order = 3)]
        [System.Xml.Serialization.XmlElementAttribute("X400", typeof(string), Order = 3)]
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

        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName", Order = 4)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ContactChoiceType[] ItemsElementName
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

        #endregion "Public"

        #endregion "Variables"

        #region "Constructors"

        public ContactType()
            : this(string.Empty, string.Empty, string.Empty, string.Empty)
        {
        }

        public ContactType(string name, string department, string role, string language)
        {
            if (!string.IsNullOrEmpty(name))
            {
                this.nameField = new List<TextType>();
                this.nameField.Add(new TextType(language, name));
            }

            if (!string.IsNullOrEmpty(department))
            {
                this.departmentField = new List<TextType>();
                this.departmentField.Add(new TextType(language, department));
            }

            if (!string.IsNullOrEmpty(role))
            {
                this.roleField = new List<TextType>();
                this.roleField.Add(new TextType(language, role));
            }
        }

        #endregion "Constructors"
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IncludeInSchema = false)]
    public enum ContactChoiceType
    {

        /// <remarks/>
        Email,

        /// <remarks/>
        Fax,

        /// <remarks/>
        Telephone,

        /// <remarks/>
        URI,

        /// <remarks/>
        X400,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message")]
    [System.Xml.Serialization.XmlRootAttribute("Error", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message", IsNullable = false)]
    public class ErrorType
    {

        private List<CodedStatusMessageType> errorMessageField;

        public ErrorType()
        {
            this.errorMessageField = new List<CodedStatusMessageType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("ErrorMessage", Order = 0)]
        public List<CodedStatusMessageType> ErrorMessage
        {
            get
            {
                return this.errorMessageField;
            }
            set
            {
                this.errorMessageField = value;
            }
        }
    }
}
