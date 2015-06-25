using System;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using System.Collections.Generic;

namespace SDMXObjectModel.Schema
{
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class schema : openAttrs
    {

        private List<openAttrs> itemsField;

        private List<openAttrs> items1Field;

        private string targetNamespaceField;

        private string versionField;

        private string finalDefaultField;

        private string blockDefaultField;

        private formChoice attributeFormDefaultField;

        private formChoice elementFormDefaultField;

        private string idField;

        private string langField;

        public schema()
        {
            this.items1Field = new List<openAttrs>();
            this.itemsField = new List<openAttrs>();
            this.finalDefaultField = "";
            this.blockDefaultField = "";
            this.attributeFormDefaultField = formChoice.unqualified;
            this.elementFormDefaultField = formChoice.unqualified;
        }

        [System.Xml.Serialization.XmlElementAttribute("annotation", typeof(annotation), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("import", typeof(import), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("include", typeof(include), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("redefine", typeof(redefine), Order = 0)]
        public List<openAttrs> Items
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

        [System.Xml.Serialization.XmlElementAttribute("annotation", typeof(annotation), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("attribute", typeof(topLevelAttribute), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("attributeGroup", typeof(namedAttributeGroup), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("complexType", typeof(topLevelComplexType), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("element", typeof(topLevelElement), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("group", typeof(namedGroup), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("notation", typeof(notation), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("simpleType", typeof(topLevelSimpleType), Order = 1)]
        public List<openAttrs> Items1
        {
            get
            {
                return this.items1Field;
            }
            set
            {
                this.items1Field = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string targetNamespace
        {
            get
            {
                return this.targetNamespaceField;
            }
            set
            {
                this.targetNamespaceField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "token")]
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string finalDefault
        {
            get
            {
                return this.finalDefaultField;
            }
            set
            {
                this.finalDefaultField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("")]
        public string blockDefault
        {
            get
            {
                return this.blockDefaultField;
            }
            set
            {
                this.blockDefaultField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(formChoice.unqualified)]
        public formChoice attributeFormDefault
        {
            get
            {
                return this.attributeFormDefaultField;
            }
            set
            {
                this.attributeFormDefaultField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(formChoice.unqualified)]
        public formChoice elementFormDefault
        {
            get
            {
                return this.elementFormDefaultField;
            }
            set
            {
                this.elementFormDefaultField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class annotation : openAttrs
    {

        private List<object> itemsField;

        private string idField;

        public annotation()
        {
            this.itemsField = new List<object>();
        }

        [System.Xml.Serialization.XmlElementAttribute("appinfo", typeof(appinfo), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("documentation", typeof(documentation), Order = 0)]
        public List<object> Items
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

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class appinfo
    {

        private List<System.Xml.XmlNode> anyField;

        private string sourceField;

        private List<System.Xml.XmlAttribute> anyAttrField;

        public appinfo()
        {
            this.anyAttrField = new List<System.Xml.XmlAttribute>();
            this.anyField = new List<System.Xml.XmlNode>();
        }

        [System.Xml.Serialization.XmlTextAttribute()]
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
        public List<System.Xml.XmlNode> Any
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

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string source
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

        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public List<System.Xml.XmlAttribute> AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(annotated))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(extensionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(simpleExtensionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(attributeGroup))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(namedAttributeGroup))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(attributeGroupRef))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(wildcard))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(keybase))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(element))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(localElement))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(narrowMaxMin))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(topLevelElement))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(group))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(explicitGroup))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(all))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(simpleExplicitGroup))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(realGroup))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(groupRef))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(namedGroup))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(restrictionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(simpleRestrictionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(complexRestrictionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(complexType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(topLevelComplexType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(localComplexType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(facet))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(numFacet))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(noFixedFacet))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(simpleType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(topLevelSimpleType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(localSimpleType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(attribute))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(topLevelAttribute))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = true)]
    public class openAttrs
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(extensionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(simpleExtensionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(attributeGroup))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(namedAttributeGroup))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(attributeGroupRef))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(wildcard))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(keybase))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(element))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(localElement))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(narrowMaxMin))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(topLevelElement))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(group))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(explicitGroup))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(all))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(simpleExplicitGroup))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(realGroup))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(groupRef))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(namedGroup))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(restrictionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(simpleRestrictionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(complexRestrictionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(complexType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(topLevelComplexType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(localComplexType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(facet))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(numFacet))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(noFixedFacet))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(simpleType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(topLevelSimpleType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(localSimpleType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(attribute))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(topLevelAttribute))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = true)]
    public class annotated : openAttrs
    {

        private annotation annotationField;

        private string idField;

        public annotated()
        {
            this.annotationField = new annotation();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public annotation annotation
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

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
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
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(simpleExtensionType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = true)]
    public class extensionType : annotated
    {

        private groupRef groupField;

        private all allField;

        private explicitGroup choiceField;

        private explicitGroup sequenceField;

        private List<annotated> itemsField;

        private wildcard anyAttributeField;

        private System.Xml.XmlQualifiedName baseField;

        public extensionType()
        {
            this.anyAttributeField = new wildcard();
            this.itemsField = new List<annotated>();
            this.sequenceField = new explicitGroup();
            this.choiceField = new explicitGroup();
            this.allField = new all();
            this.groupField = new groupRef();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public groupRef group
        {
            get
            {
                return this.groupField;
            }
            set
            {
                this.groupField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public all all
        {
            get
            {
                return this.allField;
            }
            set
            {
                this.allField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 2)]
        public explicitGroup choice
        {
            get
            {
                return this.choiceField;
            }
            set
            {
                this.choiceField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public explicitGroup sequence
        {
            get
            {
                return this.sequenceField;
            }
            set
            {
                this.sequenceField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("attribute", typeof(attribute), Order = 4)]
        [System.Xml.Serialization.XmlElementAttribute("attributeGroup", typeof(attributeGroupRef), Order = 4)]
        public List<annotated> Items
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 5)]
        public wildcard anyAttribute
        {
            get
            {
                return this.anyAttributeField;
            }
            set
            {
                this.anyAttributeField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.Xml.XmlQualifiedName @base
        {
            get
            {
                return this.baseField;
            }
            set
            {
                this.baseField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = true)]
    public class groupRef : realGroup
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(groupRef))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(namedGroup))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = true)]
    public class realGroup : group
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(explicitGroup))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(all))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(simpleExplicitGroup))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(realGroup))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(groupRef))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(namedGroup))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = true)]
    public abstract class group : annotated
    {

        private annotated[] itemsField;

        private groupChoiceType[] itemsElementNameField;

        private string nameField;

        private System.Xml.XmlQualifiedName refField;

        private string minOccursField;

        private string maxOccursField;

        public group()
        {
            //this.itemsElementNameField = new List<groupChoiceType>();
            //this.itemsField = new List<annotated>();
            this.minOccursField = "1";
            this.maxOccursField = "1";
        }

        [System.Xml.Serialization.XmlElementAttribute("all", typeof(all), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("any", typeof(any), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("choice", typeof(explicitGroup), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("element", typeof(localElement), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("group", typeof(groupRef), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("sequence", typeof(explicitGroup), Order = 0)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public annotated[] Items
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
        public groupChoiceType[] ItemsElementName
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

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NCName")]
        public string name
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.Xml.XmlQualifiedName @ref
        {
            get
            {
                return this.refField;
            }
            set
            {
                this.refField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "nonNegativeInteger")]
        [System.ComponentModel.DefaultValueAttribute("1")]
        public string minOccurs
        {
            get
            {
                return this.minOccursField;
            }
            set
            {
                this.minOccursField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("1")]
        public string maxOccurs
        {
            get
            {
                return this.maxOccursField;
            }
            set
            {
                this.maxOccursField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class all : explicitGroup
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(all))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(simpleExplicitGroup))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute("choice", Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class explicitGroup : group
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = true)]
    public class simpleExplicitGroup : explicitGroup
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class any : wildcard
    {

        private string minOccursField;

        private string maxOccursField;

        public any()
        {
            this.minOccursField = "1";
            this.maxOccursField = "1";
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "nonNegativeInteger")]
        [System.ComponentModel.DefaultValueAttribute("1")]
        public string minOccurs
        {
            get
            {
                return this.minOccursField;
            }
            set
            {
                this.minOccursField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("1")]
        public string maxOccurs
        {
            get
            {
                return this.maxOccursField;
            }
            set
            {
                this.maxOccursField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute("anyAttribute", Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class wildcard : annotated
    {

        private string namespaceField;

        private wildcardProcessContents processContentsField;

        public wildcard()
        {
            this.namespaceField = "##any";
            this.processContentsField = wildcardProcessContents.strict;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("##any")]
        public string @namespace
        {
            get
            {
                return this.namespaceField;
            }
            set
            {
                this.namespaceField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(wildcardProcessContents.strict)]
        public wildcardProcessContents processContents
        {
            get
            {
                return this.processContentsField;
            }
            set
            {
                this.processContentsField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2001/XMLSchema")]
    public enum wildcardProcessContents
    {

        /// <remarks/>
        skip,

        /// <remarks/>
        lax,

        /// <remarks/>
        strict,
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(narrowMaxMin))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = true)]
    public class localElement : element
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(localElement))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(narrowMaxMin))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(topLevelElement))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = true)]
    public abstract class element : annotated
    {

        private annotated itemField;

        private keybase[] itemsField;

        private elementChoiceType[] itemsElementNameField;

        private string nameField;

        private System.Xml.XmlQualifiedName refField;

        private System.Xml.XmlQualifiedName typeField;

        private System.Xml.XmlQualifiedName substitutionGroupField;

        private string minOccursField;

        private string maxOccursField;

        private string defaultField;

        private string fixedField;

        private bool nillableField;

        private bool abstractField;

        private string finalField;

        private string blockField;

        private formChoice formField;

        private bool formFieldSpecified;

        public element()
        {
            //this.itemsElementNameField = new List<elementChoiceType>();
            //this.itemsField = new List<keybase>();
            this.itemField = new annotated();
            this.minOccursField = "1";
            this.maxOccursField = "1";
            this.nillableField = false;
            this.abstractField = false;
        }

        [System.Xml.Serialization.XmlElementAttribute("complexType", typeof(localComplexType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("simpleType", typeof(localSimpleType), Order = 0)]
        public annotated Item
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

        [System.Xml.Serialization.XmlElementAttribute("key", typeof(keybase), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("keyref", typeof(keyref), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("unique", typeof(keybase), Order = 1)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public keybase[] Items
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

        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName", Order = 2)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public elementChoiceType[] ItemsElementName
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

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NCName")]
        public string name
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.Xml.XmlQualifiedName @ref
        {
            get
            {
                return this.refField;
            }
            set
            {
                this.refField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.Xml.XmlQualifiedName type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.Xml.XmlQualifiedName substitutionGroup
        {
            get
            {
                return this.substitutionGroupField;
            }
            set
            {
                this.substitutionGroupField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "nonNegativeInteger")]
        [System.ComponentModel.DefaultValueAttribute("1")]
        public string minOccurs
        {
            get
            {
                return this.minOccursField;
            }
            set
            {
                this.minOccursField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute("1")]
        public string maxOccurs
        {
            get
            {
                return this.maxOccursField;
            }
            set
            {
                this.maxOccursField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @default
        {
            get
            {
                return this.defaultField;
            }
            set
            {
                this.defaultField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @fixed
        {
            get
            {
                return this.fixedField;
            }
            set
            {
                this.fixedField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool nillable
        {
            get
            {
                return this.nillableField;
            }
            set
            {
                this.nillableField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool @abstract
        {
            get
            {
                return this.abstractField;
            }
            set
            {
                this.abstractField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string final
        {
            get
            {
                return this.finalField;
            }
            set
            {
                this.finalField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string block
        {
            get
            {
                return this.blockField;
            }
            set
            {
                this.blockField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public formChoice form
        {
            get
            {
                return this.formField;
            }
            set
            {
                this.formField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool formSpecified
        {
            get
            {
                return this.formFieldSpecified;
            }
            set
            {
                this.formFieldSpecified = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = true)]
    public class localComplexType : complexType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(topLevelComplexType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(localComplexType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = true)]
    public abstract class complexType : annotated
    {

        private annotated[] itemsField;

        private complexChoiceType[] itemsElementNameField;

        private string nameField;

        private bool mixedField;

        private bool abstractField;

        private string finalField;

        private string blockField;

        public complexType()
        {
            //this.itemsElementNameField = new List<complexChoiceType>();
            //this.itemsField = new List<annotated>();
            this.mixedField = false;
            this.abstractField = false;
        }

        [System.Xml.Serialization.XmlElementAttribute("all", typeof(all), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("anyAttribute", typeof(wildcard), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("attribute", typeof(attribute), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("attributeGroup", typeof(attributeGroupRef), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("choice", typeof(explicitGroup), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("complexContent", typeof(complexContent), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("group", typeof(groupRef), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("sequence", typeof(explicitGroup), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("simpleContent", typeof(simpleContent), Order = 0)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public annotated[] Items
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
        public complexChoiceType[] ItemsElementName
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

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NCName")]
        public string name
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool mixed
        {
            get
            {
                return this.mixedField;
            }
            set
            {
                this.mixedField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool @abstract
        {
            get
            {
                return this.abstractField;
            }
            set
            {
                this.abstractField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string final
        {
            get
            {
                return this.finalField;
            }
            set
            {
                this.finalField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string block
        {
            get
            {
                return this.blockField;
            }
            set
            {
                this.blockField = value;
            }
        }
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(topLevelAttribute))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = true)]
    public class attribute : annotated
    {

        private localSimpleType simpleTypeField;

        private string nameField;

        private System.Xml.XmlQualifiedName refField;

        private System.Xml.XmlQualifiedName typeField;

        private attributeUse useField;

        private string defaultField;

        private string fixedField;

        private formChoice formField;

        private bool formFieldSpecified;

        public attribute()
        {
            this.simpleTypeField = new localSimpleType();
            this.useField = attributeUse.optional;
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public localSimpleType simpleType
        {
            get
            {
                return this.simpleTypeField;
            }
            set
            {
                this.simpleTypeField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NCName")]
        public string name
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.Xml.XmlQualifiedName @ref
        {
            get
            {
                return this.refField;
            }
            set
            {
                this.refField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.Xml.XmlQualifiedName type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(attributeUse.optional)]
        public attributeUse use
        {
            get
            {
                return this.useField;
            }
            set
            {
                this.useField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @default
        {
            get
            {
                return this.defaultField;
            }
            set
            {
                this.defaultField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @fixed
        {
            get
            {
                return this.fixedField;
            }
            set
            {
                this.fixedField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public formChoice form
        {
            get
            {
                return this.formField;
            }
            set
            {
                this.formField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool formSpecified
        {
            get
            {
                return this.formFieldSpecified;
            }
            set
            {
                this.formFieldSpecified = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = true)]
    public class localSimpleType : simpleType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(topLevelSimpleType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(localSimpleType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = true)]
    public abstract class simpleType : annotated
    {

        private annotated itemField;

        private string finalField;

        private string nameField;

        public simpleType()
        {
            this.itemField = new annotated();
        }

        [System.Xml.Serialization.XmlElementAttribute("list", typeof(list), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("restriction", typeof(restriction), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("union", typeof(union), Order = 0)]
        public annotated Item
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string final
        {
            get
            {
                return this.finalField;
            }
            set
            {
                this.finalField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NCName")]
        public string name
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class list : annotated
    {

        private localSimpleType simpleTypeField;

        private System.Xml.XmlQualifiedName itemTypeField;

        public list()
        {
            this.simpleTypeField = new localSimpleType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public localSimpleType simpleType
        {
            get
            {
                return this.simpleTypeField;
            }
            set
            {
                this.simpleTypeField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.Xml.XmlQualifiedName itemType
        {
            get
            {
                return this.itemTypeField;
            }
            set
            {
                this.itemTypeField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class restriction : annotated
    {

        private localSimpleType simpleTypeField;

        private facet[] itemsField;

        private restrictionChoiceType[] itemsElementNameField;

        private System.Xml.XmlQualifiedName baseField;

        public restriction()
        {
            //this.itemsElementNameField = new List<restrictionChoiceType>();
            //this.itemsField = new List<facet>();
            this.simpleTypeField = new localSimpleType();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public localSimpleType simpleType
        {
            get
            {
                return this.simpleTypeField;
            }
            set
            {
                this.simpleTypeField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("enumeration", typeof(noFixedFacet), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("fractionDigits", typeof(numFacet), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("length", typeof(numFacet), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("maxExclusive", typeof(facet), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("maxInclusive", typeof(facet), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("maxLength", typeof(numFacet), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("minExclusive", typeof(facet), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("minInclusive", typeof(facet), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("minLength", typeof(numFacet), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("pattern", typeof(pattern), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("totalDigits", typeof(totalDigits), Order = 1)]
        [System.Xml.Serialization.XmlElementAttribute("whiteSpace", typeof(whiteSpace), Order = 1)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public facet[] Items
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

        [System.Xml.Serialization.XmlElementAttribute("ItemsElementName", Order = 2)]
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public restrictionChoiceType[] ItemsElementName
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.Xml.XmlQualifiedName @base
        {
            get
            {
                return this.baseField;
            }
            set
            {
                this.baseField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute("enumeration", Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class noFixedFacet : facet
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(numFacet))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(noFixedFacet))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute("minExclusive", Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class facet : annotated
    {

        private string valueField;

        private bool fixedField;

        public facet()
        {
            this.fixedField = false;
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        [System.ComponentModel.DefaultValueAttribute(false)]
        public bool @fixed
        {
            get
            {
                return this.fixedField;
            }
            set
            {
                this.fixedField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute("fractionDigits", Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class numFacet : facet
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class pattern : noFixedFacet
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class totalDigits : numFacet
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class whiteSpace : facet
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IncludeInSchema = false)]
    public enum restrictionChoiceType
    {

        /// <remarks/>
        enumeration,

        /// <remarks/>
        fractionDigits,

        /// <remarks/>
        length,

        /// <remarks/>
        maxExclusive,

        /// <remarks/>
        maxInclusive,

        /// <remarks/>
        maxLength,

        /// <remarks/>
        minExclusive,

        /// <remarks/>
        minInclusive,

        /// <remarks/>
        minLength,

        /// <remarks/>
        pattern,

        /// <remarks/>
        totalDigits,

        /// <remarks/>
        whiteSpace,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class union : annotated
    {

        private List<localSimpleType> simpleTypeField;

        private List<System.Xml.XmlQualifiedName> memberTypesField;

        public union()
        {
            this.memberTypesField = new List<System.Xml.XmlQualifiedName>();
            this.simpleTypeField = new List<localSimpleType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("simpleType", Order = 0)]
        public List<localSimpleType> simpleType
        {
            get
            {
                return this.simpleTypeField;
            }
            set
            {
                this.simpleTypeField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public List<System.Xml.XmlQualifiedName> memberTypes
        {
            get
            {
                return this.memberTypesField;
            }
            set
            {
                this.memberTypesField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute("simpleType", Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class topLevelSimpleType : simpleType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2001/XMLSchema")]
    public enum attributeUse
    {

        /// <remarks/>
        prohibited,

        /// <remarks/>
        optional,

        /// <remarks/>
        required,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    public enum formChoice
    {

        /// <remarks/>
        qualified,

        /// <remarks/>
        unqualified,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute("attribute", Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class topLevelAttribute : attribute
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = true)]
    public class attributeGroupRef : attributeGroup
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(namedAttributeGroup))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(attributeGroupRef))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = true)]
    public abstract class attributeGroup : annotated
    {

        private List<annotated> itemsField;

        private wildcard anyAttributeField;

        private string nameField;

        private System.Xml.XmlQualifiedName refField;

        public attributeGroup()
        {
            this.anyAttributeField = new wildcard();
            this.itemsField = new List<annotated>();
        }

        [System.Xml.Serialization.XmlElementAttribute("attribute", typeof(attribute), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("attributeGroup", typeof(attributeGroupRef), Order = 0)]
        public List<annotated> Items
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

        [System.Xml.Serialization.XmlElementAttribute(Order = 1)]
        public wildcard anyAttribute
        {
            get
            {
                return this.anyAttributeField;
            }
            set
            {
                this.anyAttributeField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NCName")]
        public string name
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.Xml.XmlQualifiedName @ref
        {
            get
            {
                return this.refField;
            }
            set
            {
                this.refField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute("attributeGroup", Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class namedAttributeGroup : attributeGroup
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class complexContent : annotated
    {

        private annotated itemField;

        private bool mixedField;

        private bool mixedFieldSpecified;

        public complexContent()
        {
            this.itemField = new annotated();
        }

        [System.Xml.Serialization.XmlElementAttribute("extension", typeof(extensionType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("restriction", typeof(complexRestrictionType), Order = 0)]
        public annotated Item
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

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool mixed
        {
            get
            {
                return this.mixedField;
            }
            set
            {
                this.mixedField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool mixedSpecified
        {
            get
            {
                return this.mixedFieldSpecified;
            }
            set
            {
                this.mixedFieldSpecified = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = true)]
    public class complexRestrictionType : restrictionType
    {
    }

    [System.Xml.Serialization.XmlIncludeAttribute(typeof(simpleRestrictionType))]
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(complexRestrictionType))]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = true)]
    public class restrictionType : annotated
    {

        private annotated[] itemsField;

        private restrictionTypeChoiceType[] itemsElementNameField;

        private List<annotated> items1Field;

        private wildcard anyAttributeField;

        private System.Xml.XmlQualifiedName baseField;

        public restrictionType()
        {
            this.anyAttributeField = new wildcard();
            this.items1Field = new List<annotated>();
            //this.itemsElementNameField = new List<restrictionTypeChoiceType>();
            //this.itemsField = new List<annotated>();
        }

        [System.Xml.Serialization.XmlElementAttribute("all", typeof(all), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("choice", typeof(explicitGroup), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("enumeration", typeof(noFixedFacet), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("fractionDigits", typeof(numFacet), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("group", typeof(groupRef), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("length", typeof(numFacet), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("maxExclusive", typeof(facet), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("maxInclusive", typeof(facet), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("maxLength", typeof(numFacet), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("minExclusive", typeof(facet), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("minInclusive", typeof(facet), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("minLength", typeof(numFacet), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("pattern", typeof(pattern), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("sequence", typeof(explicitGroup), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("simpleType", typeof(localSimpleType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("totalDigits", typeof(totalDigits), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("whiteSpace", typeof(whiteSpace), Order = 0)]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemsElementName")]
        public annotated[] Items
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
        public restrictionTypeChoiceType[] ItemsElementName
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

        [System.Xml.Serialization.XmlElementAttribute("attribute", typeof(attribute), Order = 2)]
        [System.Xml.Serialization.XmlElementAttribute("attributeGroup", typeof(attributeGroupRef), Order = 2)]
        public List<annotated> Items1
        {
            get
            {
                return this.items1Field;
            }
            set
            {
                this.items1Field = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 3)]
        public wildcard anyAttribute
        {
            get
            {
                return this.anyAttributeField;
            }
            set
            {
                this.anyAttributeField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.Xml.XmlQualifiedName @base
        {
            get
            {
                return this.baseField;
            }
            set
            {
                this.baseField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IncludeInSchema = false)]
    public enum restrictionTypeChoiceType
    {

        /// <remarks/>
        all,

        /// <remarks/>
        choice,

        /// <remarks/>
        enumeration,

        /// <remarks/>
        fractionDigits,

        /// <remarks/>
        group,

        /// <remarks/>
        length,

        /// <remarks/>
        maxExclusive,

        /// <remarks/>
        maxInclusive,

        /// <remarks/>
        maxLength,

        /// <remarks/>
        minExclusive,

        /// <remarks/>
        minInclusive,

        /// <remarks/>
        minLength,

        /// <remarks/>
        pattern,

        /// <remarks/>
        sequence,

        /// <remarks/>
        simpleType,

        /// <remarks/>
        totalDigits,

        /// <remarks/>
        whiteSpace,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = true)]
    public class simpleRestrictionType : restrictionType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class simpleContent : annotated
    {

        private annotated itemField;

        public simpleContent()
        {
            this.itemField = new annotated();
        }

        [System.Xml.Serialization.XmlElementAttribute("extension", typeof(simpleExtensionType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("restriction", typeof(simpleRestrictionType), Order = 0)]
        public annotated Item
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = true)]
    public class simpleExtensionType : extensionType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IncludeInSchema = false)]
    public enum complexChoiceType
    {

        /// <remarks/>
        all,

        /// <remarks/>
        anyAttribute,

        /// <remarks/>
        attribute,

        /// <remarks/>
        attributeGroup,

        /// <remarks/>
        choice,

        /// <remarks/>
        complexContent,

        /// <remarks/>
        group,

        /// <remarks/>
        sequence,

        /// <remarks/>
        simpleContent,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute("complexType", Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class topLevelComplexType : complexType
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute("unique", Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class keybase : annotated
    {

        private selector selectorField;

        private List<field> fieldField;

        private string nameField;

        public keybase()
        {
            this.fieldField = new List<field>();
            this.selectorField = new selector();
        }

        [System.Xml.Serialization.XmlElementAttribute(Order = 0)]
        public selector selector
        {
            get
            {
                return this.selectorField;
            }
            set
            {
                this.selectorField = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute("field", Order = 1)]
        public List<field> field
        {
            get
            {
                return this.fieldField;
            }
            set
            {
                this.fieldField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NCName")]
        public string name
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class selector : annotated
    {

        private string xpathField;

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "token")]
        public string xpath
        {
            get
            {
                return this.xpathField;
            }
            set
            {
                this.xpathField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class field : annotated
    {

        private string xpathField;

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "token")]
        public string xpath
        {
            get
            {
                return this.xpathField;
            }
            set
            {
                this.xpathField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class keyref : keybase
    {

        private System.Xml.XmlQualifiedName referField;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public System.Xml.XmlQualifiedName refer
        {
            get
            {
                return this.referField;
            }
            set
            {
                this.referField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IncludeInSchema = false)]
    public enum elementChoiceType
    {

        /// <remarks/>
        key,

        /// <remarks/>
        keyref,

        /// <remarks/>
        unique,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute("element", Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class topLevelElement : element
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = true)]
    public class narrowMaxMin : localElement
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IncludeInSchema = false)]
    public enum groupChoiceType
    {

        /// <remarks/>
        all,

        /// <remarks/>
        any,

        /// <remarks/>
        choice,

        /// <remarks/>
        element,

        /// <remarks/>
        group,

        /// <remarks/>
        sequence,
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute("group", Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class namedGroup : realGroup
    {
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class documentation
    {

        private List<System.Xml.XmlNode> anyField;

        private string sourceField;

        private string langField;

        private List<System.Xml.XmlAttribute> anyAttrField;

        public documentation()
        {
            this.anyAttrField = new List<System.Xml.XmlAttribute>();
            this.anyField = new List<System.Xml.XmlNode>();
        }

        [System.Xml.Serialization.XmlTextAttribute()]
        [System.Xml.Serialization.XmlAnyElementAttribute(Order = 0)]
        public List<System.Xml.XmlNode> Any
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

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string source
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

        [System.Xml.Serialization.XmlAnyAttributeAttribute()]
        public List<System.Xml.XmlAttribute> AnyAttr
        {
            get
            {
                return this.anyAttrField;
            }
            set
            {
                this.anyAttrField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class import : annotated
    {

        private string namespaceField;

        private string schemaLocationField;

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string @namespace
        {
            get
            {
                return this.namespaceField;
            }
            set
            {
                this.namespaceField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string schemaLocation
        {
            get
            {
                return this.schemaLocationField;
            }
            set
            {
                this.schemaLocationField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class include : annotated
    {

        private string schemaLocationField;

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string schemaLocation
        {
            get
            {
                return this.schemaLocationField;
            }
            set
            {
                this.schemaLocationField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class redefine : openAttrs
    {

        private List<openAttrs> itemsField;

        private string schemaLocationField;

        private string idField;

        public redefine()
        {
            this.itemsField = new List<openAttrs>();
        }

        [System.Xml.Serialization.XmlElementAttribute("annotation", typeof(annotation), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("attributeGroup", typeof(namedAttributeGroup), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("complexType", typeof(topLevelComplexType), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("group", typeof(namedGroup), Order = 0)]
        [System.Xml.Serialization.XmlElementAttribute("simpleType", typeof(topLevelSimpleType), Order = 0)]
        public List<openAttrs> Items
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

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string schemaLocation
        {
            get
            {
                return this.schemaLocationField;
            }
            set
            {
                this.schemaLocationField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "ID")]
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
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.w3.org/2001/XMLSchema")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.w3.org/2001/XMLSchema", IsNullable = false)]
    public class notation : annotated
    {

        private string nameField;

        private string publicField;

        private string systemField;

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "NCName")]
        public string name
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

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "token")]
        public string @public
        {
            get
            {
                return this.publicField;
            }
            set
            {
                this.publicField = value;
            }
        }

        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "anyURI")]
        public string system
        {
            get
            {
                return this.systemField;
            }
            set
            {
                this.systemField = value;
            }
        }
    }
}
