using System;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;
using System.Collections.Generic;
using SDMXObjectModel.Common;

namespace SDMXObjectModel.Message.Footer
{
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message/footer")]
    [System.Xml.Serialization.XmlRootAttribute("Footer", Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message/footer", IsNullable = false)]
    public class FooterType
    {

        private List<FooterMessageType> messageField;

        public FooterType()
        {
            this.messageField = new List<FooterMessageType>();
        }

        [System.Xml.Serialization.XmlElementAttribute("Message", Order = 0)]
        public List<FooterMessageType> Message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message/footer")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message/footer", IsNullable = true)]
    public class FooterMessageType : CodedStatusMessageType
    {

        private SeverityCodeType severityField;

        private bool severityFieldSpecified;

        [System.Xml.Serialization.XmlAttributeAttribute()]
        public SeverityCodeType severity
        {
            get
            {
                return this.severityField;
            }
            set
            {
                this.severityField = value;
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool severitySpecified
        {
            get
            {
                return this.severityFieldSpecified;
            }
            set
            {
                this.severityFieldSpecified = value;
            }
        }
    }

    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message/footer")]
    public enum SeverityCodeType
    {

        /// <remarks/>
        Error,

        /// <remarks/>
        Warning,

        /// <remarks/>
        Information,
    }
}
