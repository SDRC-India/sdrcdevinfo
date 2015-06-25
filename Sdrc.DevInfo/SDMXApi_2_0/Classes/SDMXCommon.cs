using System;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using System.Collections.Generic;

namespace SDMXApi_2_0.Common
{
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = true)]
    public class ConstraintType
    {

        private string constraintIDField;

        private List<CubeRegionType> cubeRegionField;

        private MetadataConceptSetType metadataConceptSetField;

        private List<KeySetType> keySetField;

        private ReleaseCalendarType releaseCalendarField;

        private ReferencePeriodType referencePeriodField;

        private ConstraintTypeType constraintType1Field;

        public ConstraintType()
        {
            this.referencePeriodField = new ReferencePeriodType();
            this.releaseCalendarField = new ReleaseCalendarType();
            this.keySetField = new List<KeySetType>();
            this.metadataConceptSetField = new MetadataConceptSetType();
            this.cubeRegionField = new List<CubeRegionType>();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string ConstraintID
        {
            get
            {
                return this.constraintIDField;
            }
            set
            {
                this.constraintIDField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("CubeRegion", Order = 1)]
        public List<CubeRegionType> CubeRegion
        {
            get
            {
                return this.cubeRegionField;
            }
            set
            {
                this.cubeRegionField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public MetadataConceptSetType MetadataConceptSet
        {
            get
            {
                return this.metadataConceptSetField;
            }
            set
            {
                this.metadataConceptSetField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("KeySet", Order = 3)]
        public List<KeySetType> KeySet
        {
            get
            {
                return this.keySetField;
            }
            set
            {
                this.keySetField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 4)]
        public ReleaseCalendarType ReleaseCalendar
        {
            get
            {
                return this.releaseCalendarField;
            }
            set
            {
                this.releaseCalendarField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public ReferencePeriodType ReferencePeriod
        {
            get
            {
                return this.referencePeriodField;
            }
            set
            {
                this.referencePeriodField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute("ConstraintType")]
        public ConstraintTypeType ConstraintType1
        {
            get
            {
                return this.constraintType1Field;
            }
            set
            {
                this.constraintType1Field = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = true)]
    public class CubeRegionType
    {

        private List<MemberType> memberField;

        private bool isIncludedField;

        public CubeRegionType()
        {
            this.memberField = new List<MemberType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Member", Order = 0)]
        public List<MemberType> Member
        {
            get
            {
                return this.memberField;
            }
            set
            {
                this.memberField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isIncluded
        {
            get
            {
                return this.isIncludedField;
            }
            set
            {
                this.isIncludedField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = true)]
    public class MemberType
    {

        private string componentRefField;

        private List<MemberValueType> memberValueField;

        private bool isIncludedField;

        public MemberType()
        {
            this.memberValueField = new List<MemberValueType>();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string ComponentRef
        {
            get
            {
                return this.componentRefField;
            }
            set
            {
                this.componentRefField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("MemberValue", Order = 1)]
        public List<MemberValueType> MemberValue
        {
            get
            {
                return this.memberValueField;
            }
            set
            {
                this.memberValueField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isIncluded
        {
            get
            {
                return this.isIncludedField;
            }
            set
            {
                this.isIncludedField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = true)]
    public class MemberValueType
    {

        private string valueField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = true)]
    public class ReferencePeriodType
    {

        private System.DateTime startTimeField;

        private System.DateTime endTimeField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime startTime
        {
            get
            {
                return this.startTimeField;
            }
            set
            {
                this.startTimeField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.DateTime endTime
        {
            get
            {
                return this.endTimeField;
            }
            set
            {
                this.endTimeField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = true)]
    public class ReleaseCalendarType
    {

        private string periodicityField;

        private string offsetField;

        private string toleranceField;

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string Periodicity
        {
            get
            {
                return this.periodicityField;
            }
            set
            {
                this.periodicityField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public string Offset
        {
            get
            {
                return this.offsetField;
            }
            set
            {
                this.offsetField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public string Tolerance
        {
            get
            {
                return this.toleranceField;
            }
            set
            {
                this.toleranceField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = true)]
    public class KeyType
    {

        private string[] itemsField;

        private KeyChoiceType[] itemsElementNameField;

        public KeyType()
        {
            //this.itemsElementNameField = new List<KeyChoiceType>();
            //this.itemsField = new List<string>();
        }

        [System.Xml.Serialization.XmlElementAttribute("ComponentRef", typeof(string), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("Value", typeof(string), Order = 0)]
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
        public KeyChoiceType[] ItemsElementName
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IncludeInSchema = false)]
    public enum KeyChoiceType
    {

        /// <remarks/>
        ComponentRef,

        /// <remarks/>
        Value,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = true)]
    public class KeySetType
    {

        private KeyType keyField;

        private bool isIncludedField;

        public KeySetType()
        {
            this.keyField = new KeyType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public KeyType Key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isIncluded
        {
            get
            {
                return this.isIncludedField;
            }
            set
            {
                this.isIncludedField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = true)]
    public class MetadataConceptSetType
    {

        private List<MemberType> memberField;

        private bool isIncludedField;

        public MetadataConceptSetType()
        {
            this.memberField = new List<MemberType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Member", Order = 0)]
        public List<MemberType> Member
        {
            get
            {
                return this.memberField;
            }
            set
            {
                this.memberField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool isIncluded
        {
            get
            {
                return this.isIncludedField;
            }
            set
            {
                this.isIncludedField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common")]
    public enum ConstraintTypeType
    {

        /// <remarks/>
        Content,

        /// <remarks/>
        Attachment,
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = true)]
    public class TextType
    {

        private string langField;

        private string valueField;

        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "http://www.w3.org/XML/1998/namespace")]
        public string lang
        {
            get
            {
                return this.langField;
            }
            set
            {
                this.langField = value;
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = true)]
    public class AnnotationType
    {

        private string annotationTitleField;

        private string annotationType1Field;

        private string annotationURLField;

        private List<TextType> annotationTextField;

        public AnnotationType()
        {
            this.annotationTextField = new List<TextType>();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public string AnnotationTitle
        {
            get
            {
                return this.annotationTitleField;
            }
            set
            {
                this.annotationTitleField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("AnnotationType", Order = 1)]
        public string AnnotationType1
        {
            get
            {
                return this.annotationType1Field;
            }
            set
            {
                this.annotationType1Field = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(DataType = "anyURI", Order = 2)]
        public string AnnotationURL
        {
            get
            {
                return this.annotationURLField;
            }
            set
            {
                this.annotationURLField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("AnnotationText", Order = 3)]
        public List<TextType> AnnotationText
        {
            get
            {
                return this.annotationTextField;
            }
            set
            {
                this.annotationTextField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common", IsNullable = true)]
    public class AnnotationsType
    {

        private List<AnnotationType> annotationField;

        public AnnotationsType()
        {
            this.annotationField = new List<AnnotationType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Annotation", Order = 0)]
        public List<AnnotationType> Annotation
        {
            get
            {
                return this.annotationField;
            }
            set
            {
                this.annotationField = value;
            }
        }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "2.0.50727.4927")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.SDMX.org/resources/SDMXML/schemas/v2_0/common")]
    public enum ActionType
    {

        /// <remarks/>
        Append,

        /// <remarks/>
        Replace,

        /// <remarks/>
        Delete,

        /// <remarks/>
        Information,
    }

}
