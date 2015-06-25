using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using SDMXObjectModel.Structure;
using SDMXObjectModel.Common;
using System.Xml;
using SDMXObjectModel;
using SDMXObjectModel.Message;
using SDMXObjectModel.Data.Generic;

namespace DevInfo.Lib.DI_LibSDMX
{
    internal class ConstraintUtility : ArtefactUtility
    {
         #region "--Properties--"

        #region "--Private--"

        private XmlDocument _sdmxMLFileXMLDocument;

        private string _registrationId;

        private string _simpleDataSourceUrl;

        #endregion "--Private--"

        #region "--Public--"

        public XmlDocument SdmxMLFileXMLDocument
        {
            get
            {
                return this._sdmxMLFileXMLDocument;
            }
            set
            {
                this._sdmxMLFileXMLDocument = value ;
            }
        }

        public string RegistrationId
        {
            get
            {
                return this._registrationId;
            }
            set
            {
                this._registrationId = value;
            }
        }

        public string SimpleDataSourceUrl
        {
            get
            {
                return this._simpleDataSourceUrl;
            }
            set
            {
                this._simpleDataSourceUrl = value;
            }
        }

        #endregion "--Public--"

        #endregion "--Properties--""

        #region "--Constructors--"

        #region "--Private--"

        #endregion "--Private--"

        #region "--Public--"

        internal ConstraintUtility(XmlDocument sdmxMLFileXMLDocument, string registrationId, string simpleDataSourceUrl, string agencyId, Header header, string outputFolder)
            : base(agencyId, string.Empty, header, outputFolder)
        {
            this._sdmxMLFileXMLDocument = sdmxMLFileXMLDocument;
            this._registrationId = registrationId;
            this._simpleDataSourceUrl = simpleDataSourceUrl;
        }

        #endregion "--Public--"

        #endregion "--Constructors--""

        #region "--Methods--"

        #region "--Private--"

        private ArtefactInfo Prepare_ArtefactInfo_From_ConstraintCreation(SDMXObjectModel.Structure.ContentConstraintType Constraint, string FileName)
        {
            ArtefactInfo RetVal;
            SDMXObjectModel.Message.StructureType Structure;
            XmlDocument XmlContent;

            RetVal = null;
            XmlContent = null;

            try
            {
                Structure = this.Get_ConstraintType_Object(Constraint);
                XmlContent = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.StructureType), Structure);
                RetVal = new ArtefactInfo(Constraint.id, Constraint.agencyID, Constraint.version, string.Empty, ArtefactTypes.Constraint, FileName, XmlContent);
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

        private SDMXObjectModel.Message.StructureType Get_ConstraintType_Object(SDMXObjectModel.Structure.ContentConstraintType Constraint)
        {
            SDMXObjectModel.Message.StructureType RetVal;

            RetVal = new SDMXObjectModel.Message.StructureType();
            RetVal.Header = this.Get_Appropriate_Header();
            RetVal.Structures.Constraints = new List<ConstraintType>();
            RetVal.Structures.Constraints.Add(Constraint);
            RetVal.Footer = null;
            return RetVal;
        }

        public new SDMXObjectModel.Message.StructureHeaderType Get_Appropriate_Header()
        {
            SDMXObjectModel.Message.StructureHeaderType RetVal;
            SenderType Sender;
            PartyType Receiver;

            if (this.Header == null)
            {
                Sender = new SenderType(Constants.Header.SenderId, Constants.Header.SenderName, Constants.DefaultLanguage, new SDMXObjectModel.Message.ContactType(Constants.Header.Sender, Constants.Header.SenderDepartment, Constants.Header.SenderRole, Constants.DefaultLanguage));
                Sender.Contact[0].Items = new string[] { Constants.Header.SenderTelephone, Constants.Header.SenderEmail, Constants.Header.SenderFax };
                Sender.Contact[0].ItemsElementName = new SDMXObjectModel.Message.ContactChoiceType[] { SDMXObjectModel.Message.ContactChoiceType.Telephone, SDMXObjectModel.Message.ContactChoiceType.Email, SDMXObjectModel.Message.ContactChoiceType.Fax };

                Receiver = new PartyType(Constants.Header.ReceiverId, Constants.Header.ReceiverName, Constants.DefaultLanguage, new SDMXObjectModel.Message.ContactType(Constants.Header.Receiver, Constants.Header.ReceiverDepartment, Constants.Header.ReceiverRole, Constants.DefaultLanguage));
                Receiver.Contact[0].Items = new string[] { Constants.Header.ReceiverTelephone, Constants.Header.ReceiverEmail, Constants.Header.ReceiverFax };
                Receiver.Contact[0].ItemsElementName = new SDMXObjectModel.Message.ContactChoiceType[] { SDMXObjectModel.Message.ContactChoiceType.Telephone, SDMXObjectModel.Message.ContactChoiceType.Email, SDMXObjectModel.Message.ContactChoiceType.Fax };

                RetVal = new StructureHeaderType(Constants.Header.Id, true, DateTime.Now, Sender, Receiver);
            }
            else
            {
                Sender = new SenderType(this.Header.Sender.ID, this.Header.Sender.Name, Constants.DefaultLanguage, new SDMXObjectModel.Message.ContactType(this.Header.Sender.Contact.Name, this.Header.Sender.Contact.Department, this.Header.Sender.Contact.Role, Constants.DefaultLanguage));
                Sender.Contact[0].Items = new string[] { this.Header.Sender.Contact.Telephone, this.Header.Sender.Contact.Email, this.Header.Sender.Contact.Fax };
                Sender.Contact[0].ItemsElementName = new SDMXObjectModel.Message.ContactChoiceType[] { SDMXObjectModel.Message.ContactChoiceType.Telephone, SDMXObjectModel.Message.ContactChoiceType.Email, SDMXObjectModel.Message.ContactChoiceType.Fax };

                Receiver = new PartyType(this.Header.Receiver.ID, this.Header.Receiver.Name, Constants.DefaultLanguage, new SDMXObjectModel.Message.ContactType(this.Header.Receiver.Contact.Name, this.Header.Receiver.Contact.Department, this.Header.Receiver.Contact.Role, Constants.DefaultLanguage));
                Receiver.Contact[0].Items = new string[] { this.Header.Receiver.Contact.Telephone, this.Header.Receiver.Contact.Email, this.Header.Receiver.Contact.Fax };
                Receiver.Contact[0].ItemsElementName = new SDMXObjectModel.Message.ContactChoiceType[] { SDMXObjectModel.Message.ContactChoiceType.Telephone, SDMXObjectModel.Message.ContactChoiceType.Email, SDMXObjectModel.Message.ContactChoiceType.Fax };

                RetVal = new StructureHeaderType(this.Header.ID, true, DateTime.Now, Sender, Receiver);
            }

            return RetVal;
        }

        #endregion "--Private--"

        #region "--Public--"

        public override List<ArtefactInfo> Generate_Artefact()
        {
            List<ArtefactInfo> RetVal;
            ArtefactInfo Artefact;
            SDMXObjectModel.Structure.ContentConstraintType ContentConstraint;
            SDMXObjectModel.Message.RegistryInterfaceType RegistryInterface;
            SDMXObjectModel.Registry.RegistrationType Registration;
            StructureSpecificTimeSeriesDataType StructureSpecificTimeSeriesData;
            DataKeySetType DataKeySet;
            DataKeyValueType DataKeyValue;
            int KeyIndex;
            string SimpleDataFileUrl = string.Empty;
            RetVal = null;

            try
            {

                StructureSpecificTimeSeriesData = (SDMXObjectModel.Message.StructureSpecificTimeSeriesDataType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureSpecificTimeSeriesDataType), this._sdmxMLFileXMLDocument);

                ContentConstraint = new SDMXObjectModel.Structure.ContentConstraintType();
                ContentConstraint.id = Constants.Constraint.Prefix + this._registrationId;
                ContentConstraint.Name.Add(new TextType(null, Constants.Constraint.Name + this._registrationId));
                ContentConstraint.agencyID = this.AgencyId;
                ContentConstraint.version = Constants.Constraint.Version;
                ContentConstraint.Description.Add(new TextType(null, Constants.Constraint.Description));
                ContentConstraint.Annotations = null;
                ContentConstraint.ReleaseCalendar = null;
                ContentConstraint.ConstraintAttachment = new ContentConstraintAttachmentType();
               
                if (!String.IsNullOrEmpty(this._simpleDataSourceUrl))
                {
                    ContentConstraint.ConstraintAttachment.Items = new object[1];
                    ContentConstraint.ConstraintAttachment.Items[0] = this._simpleDataSourceUrl;
                    ContentConstraint.ConstraintAttachment.ItemsElementName = new ConstraintAttachmentChoiceType[] { ConstraintAttachmentChoiceType.SimpleDataSource };
                }
                
                DataKeySet = new DataKeySetType();
                DataKeySet.isIncluded = true;
                ContentConstraint.Items.Add(DataKeySet);
                KeyIndex = 0;
                foreach (SDMXObjectModel.Data.StructureSpecific.SeriesType Series in StructureSpecificTimeSeriesData.DataSet[0].Items)
                {
                    ((DataKeySetType)(ContentConstraint.Items[0])).Key.Add(new DataKeyType());
                    foreach (XmlAttribute SeriesAttribute in Series.AnyAttr)
                    {
                        
                        DataKeyValue = new DataKeyValueType();
                        DataKeyValue.id = SeriesAttribute.Name;
                        DataKeyValue.Items.Add(new SimpleKeyValueType());
                        ((SimpleKeyValueType)(DataKeyValue.Items[0])).Value = SeriesAttribute.Value;
                        ((DataKeySetType)(ContentConstraint.Items[0])).Key[KeyIndex].KeyValue.Add(DataKeyValue);
                    }
                    KeyIndex = KeyIndex + 1;
                }
                              
                // Preparing Artefact and saving
                Artefact = this.Prepare_ArtefactInfo_From_ConstraintCreation(ContentConstraint, ContentConstraint.id + Constants.XmlExtension);
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
