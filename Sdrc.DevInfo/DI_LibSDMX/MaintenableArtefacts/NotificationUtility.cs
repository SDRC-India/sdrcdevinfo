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
using SDMXObjectModel.Registry;

namespace DevInfo.Lib.DI_LibSDMX
{
    internal class NotificationUtility : ArtefactUtility
    {
        #region "--Properties--"

        #region "--Private--"

        private DateTime _eventTime;

        private string _registrationId;

        private string _subscriptionURN;

        private ActionType _action;

        private RegistrationType _registration;

        private List<ArtefactRef> _artefacts;

        private bool _registrationNotificationFlag;

        #endregion "--Private--"

        #region "--Public--"

        public DateTime EventTime
        {
            get
            {
                return this._eventTime;
            }
            set
            {
                this._eventTime = value;
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

        public string SubscriptionURN
        {
            get
            {
                return this._subscriptionURN;
            }
            set
            {
                this._subscriptionURN = value;
            }
        }

        public ActionType Action
        {
            get
            {
                return this._action;
            }
            set
            {
                this._action = value;
            }
        }

        public RegistrationType Registration
        {
            get
            {
                return this._registration;
            }
            set
            {
                this._registration = value;
            }
        }

        public List<ArtefactRef> Aretfacts
        {
            get
            {
                return this._artefacts;
            }
            set
            {
                this._artefacts = value;
            }
        }

        public bool RegistrationNotificationFlag
        {
            get
            {
                return this._registrationNotificationFlag;
            }
            set
            {
                this._registrationNotificationFlag = value;
            }
        }

        #endregion "--Public--"

        #endregion "--Properties--""

        #region "--Constructors--"

        #region "--Private--"

        #endregion "--Private--"

        #region "--Public--"

        internal NotificationUtility(DateTime eventTime, string registrationId, string subscriptionURN, ActionType action, RegistrationType registration, Header header)
            : base(string.Empty, string.Empty, header, string.Empty)
        {
            this._eventTime = eventTime;
            this._registrationId = registrationId;
            this._subscriptionURN = subscriptionURN;
            this._action = action;
            this._registration = registration;

            this._registrationNotificationFlag = true;
        }

        internal NotificationUtility(DateTime eventTime, List<ArtefactRef> artefacts, string subscriptionURN, ActionType action, Header header)
            : base(string.Empty, string.Empty, header, string.Empty)
        {
            this._eventTime = eventTime;
            this._artefacts = artefacts;
            this._subscriptionURN = subscriptionURN;
            this._action = action;

            this._registrationNotificationFlag = false;
        }

        #endregion "--Public--"

        #endregion "--Constructors--""

        #region "--Methods--"

        #region "--Private--"

        private ArtefactInfo Prepare_ArtefactInfo_From_NotifyRegistryEvent(SDMXObjectModel.Registry.NotifyRegistryEventType NotifyRegistryEvent)
        {
            ArtefactInfo RetVal;
            SDMXObjectModel.Message.RegistryInterfaceType RegistryInterface;
            XmlDocument XmlContent;

            RetVal = null;
            XmlContent = null;

            try
            {
                RegistryInterface = this.Get_RegistryInterface_Object(NotifyRegistryEvent);
                XmlContent = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.RegistryInterfaceType), RegistryInterface);
                RetVal = new ArtefactInfo(string.Empty, string.Empty, string.Empty, string.Empty, ArtefactTypes.None, string.Empty, XmlContent);
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

        private SDMXObjectModel.Message.RegistryInterfaceType Get_RegistryInterface_Object(SDMXObjectModel.Registry.NotifyRegistryEventType NotifyRegistryEvent)
        {
            SDMXObjectModel.Message.RegistryInterfaceType RetVal;

            RetVal = new SDMXObjectModel.Message.RegistryInterfaceType();
            RetVal.Header = this.Get_Appropriate_Header();
            RetVal.Item = NotifyRegistryEvent;
            RetVal.Footer = null;

            return RetVal;
        }

        public new SDMXObjectModel.Message.BasicHeaderType Get_Appropriate_Header()
        {
            SDMXObjectModel.Message.BasicHeaderType RetVal;
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

                RetVal = new BasicHeaderType(Constants.Header.Id, true, DateTime.Now, Sender, Receiver);
            }
            else
            {
                Sender = new SenderType(this.Header.Sender.ID, this.Header.Sender.Name, Constants.DefaultLanguage, new SDMXObjectModel.Message.ContactType(this.Header.Sender.Contact.Name, this.Header.Sender.Contact.Department, this.Header.Sender.Contact.Role, Constants.DefaultLanguage));
                Sender.Contact[0].Items = new string[] { this.Header.Sender.Contact.Telephone, this.Header.Sender.Contact.Email, this.Header.Sender.Contact.Fax };
                Sender.Contact[0].ItemsElementName = new SDMXObjectModel.Message.ContactChoiceType[] { SDMXObjectModel.Message.ContactChoiceType.Telephone, SDMXObjectModel.Message.ContactChoiceType.Email, SDMXObjectModel.Message.ContactChoiceType.Fax };

                Receiver = new PartyType(this.Header.Receiver.ID, this.Header.Receiver.Name, Constants.DefaultLanguage, new SDMXObjectModel.Message.ContactType(this.Header.Receiver.Contact.Name, this.Header.Receiver.Contact.Department, this.Header.Receiver.Contact.Role, Constants.DefaultLanguage));
                Receiver.Contact[0].Items = new string[] { this.Header.Receiver.Contact.Telephone, this.Header.Receiver.Contact.Email, this.Header.Receiver.Contact.Fax };
                Receiver.Contact[0].ItemsElementName = new SDMXObjectModel.Message.ContactChoiceType[] { SDMXObjectModel.Message.ContactChoiceType.Telephone, SDMXObjectModel.Message.ContactChoiceType.Email, SDMXObjectModel.Message.ContactChoiceType.Fax };

                RetVal = new BasicHeaderType(this.Header.ID, true, DateTime.Now, Sender, Receiver);
            }

            return RetVal;
        }

        #endregion "--Private--"

        #region "--Public--"

        public override List<ArtefactInfo> Generate_Artefact()
        {
            List<ArtefactInfo> RetVal;
            ArtefactInfo Artefact;
            SDMXObjectModel.Registry.NotifyRegistryEventType NotifyRegistryEvent;

            RetVal = null;

            try
            {
                NotifyRegistryEvent = new SDMXObjectModel.Registry.NotifyRegistryEventType();

                NotifyRegistryEvent.EventTime = this._eventTime;
                NotifyRegistryEvent.SubscriptionURN = this._subscriptionURN;
                NotifyRegistryEvent.EventAction = this._action;

                if (this._registrationNotificationFlag == true)
                {
                    NotifyRegistryEvent.Item = this._registrationId;
                    NotifyRegistryEvent.ItemElementName = NotifyRegistryEventChoiceType.RegistrationID;

                    NotifyRegistryEvent.Item1 = new RegistrationEventType();
                    ((RegistrationEventType)NotifyRegistryEvent.Item1).Registration = this._registration;
                }
                else
                {
                    NotifyRegistryEvent.Item = string.Empty;
                    NotifyRegistryEvent.ItemElementName = NotifyRegistryEventChoiceType.ObjectURN;

                    NotifyRegistryEvent.Item1 = new StructuralEventType();
                    ((StructuralEventType)NotifyRegistryEvent.Item1).Item = new StructuresType();

                    foreach (ArtefactRef ArtefactRef in this._artefacts)
                    {
                        if (ArtefactRef.ArtefactType == ArtefactTypes.DSD)
                        {
                            if (((StructuralEventType)NotifyRegistryEvent.Item1).Item.DataStructures == null)
                            {
                                ((StructuralEventType)NotifyRegistryEvent.Item1).Item.DataStructures = new List<SDMXObjectModel.Structure.DataStructureType>();
                            }

                            ((StructuralEventType)NotifyRegistryEvent.Item1).Item.DataStructures.Add(new SDMXObjectModel.Structure.DataStructureType(ArtefactRef.Id, ArtefactRef.AgencyId, ArtefactRef.Version, ArtefactRef.Name, ArtefactRef.Description, ArtefactRef.Language, null));
                        }

                        if (ArtefactRef.ArtefactType == ArtefactTypes.MSD)
                        {
                            if (((StructuralEventType)NotifyRegistryEvent.Item1).Item.MetadataStructures == null)
                            {
                                ((StructuralEventType)NotifyRegistryEvent.Item1).Item.MetadataStructures = new List<SDMXObjectModel.Structure.MetadataStructureType>();
                            }

                            ((StructuralEventType)NotifyRegistryEvent.Item1).Item.MetadataStructures.Add(new SDMXObjectModel.Structure.MetadataStructureType(ArtefactRef.Id, ArtefactRef.AgencyId, ArtefactRef.Version, ArtefactRef.Name, ArtefactRef.Description, ArtefactRef.Language, null));
                        }
                    }
                }
                
                // Preparing Artefact and saving
                Artefact = this.Prepare_ArtefactInfo_From_NotifyRegistryEvent(NotifyRegistryEvent);
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
