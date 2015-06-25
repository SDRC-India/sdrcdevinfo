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
    internal class SubscriptionUtility : ArtefactUtility
    {
        #region "--Properties--"

        #region "--Private--"

        private string _id;

        private string _userId;

        private UserTypes _userType;

        private List<bool> _isSOAPMailIds;

        private List<string> _notificationMailIds;

        private List<bool> _isSOAPHTTPs;

        private List<string> _notificationHTTPs;

        private string _subscriberAssignedId;

        private DateTime _startDate;

        private DateTime _endDate;

        private string _eventSelector;

        private Dictionary<string, string> _dictCategories;

        private string _mfdId;

        #endregion "--Private--"

        #region "--Public--"

        public string Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        public string UserId
        {
            get
            {
                return this._userId;
            }
            set
            {
                this._userId = value;
            }
        }

        public UserTypes UserType
        {
            get
            {
                return this._userType;
            }
            set
            {
                this._userType = value;
            }
        }

        public List<bool> IsSOAPMailIds
        {
            get
            {
                return this._isSOAPMailIds;
            }
            set
            {
                this._isSOAPMailIds = value;
            }
        }

        public List<string> NotificationMailIds
        {
            get
            {
                return this._notificationMailIds;
            }
            set
            {
                this._notificationMailIds = value;
            }
        }

        public List<bool> IsSOAPHTTPs
        {
            get
            {
                return this._isSOAPHTTPs;
            }
            set
            {
                this._isSOAPHTTPs = value;
            }
        }

        public List<string> NotificationHTTPs
        {
            get
            {
                return this._notificationHTTPs;
            }
            set
            {
                this._notificationHTTPs = value;
            }
        }

        public string SubscriberAssignedId
        {
            get
            {
                return this._subscriberAssignedId;
            }
            set
            {
                this._subscriberAssignedId = value;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return this._startDate;
            }
            set
            {
                this._startDate = value;
            }
        }

        public DateTime EndDate
        {
            get
            {
                return this._endDate;
            }
            set
            {
                this._endDate = value;
            }
        }

        public Dictionary<string, string> DictCategories
        {
            get
            {
                return this._dictCategories;
            }
            set
            {
                this._dictCategories = value;
            }
        }

        #endregion "--Public--"

        #endregion "--Properties--""

        #region "--Constructors--"

        #region "--Private--"

        #endregion "--Private--"

        #region "--Public--"

        internal SubscriptionUtility(string id, string userId, UserTypes userType, List<bool> isSOAPMailIds, List<string> notificationMailIds, List<bool> isSOAPHTTPs, List<string> notificationHTTPs, string subscriberAssignedId, DateTime startDate, DateTime endDate, string EventSelector, Dictionary<string, string> dictCategories,string MFDId, string agencyId, Header header, string outputFolder)
            : base(agencyId, string.Empty, header, outputFolder)
        {
            this._id = id;
            this._userId = userId;
            this._userType = userType;
            this._isSOAPMailIds = isSOAPMailIds;
            this._notificationMailIds = notificationMailIds;
            this._isSOAPHTTPs = isSOAPHTTPs;
            this._notificationHTTPs = notificationHTTPs;
            this._subscriberAssignedId = subscriberAssignedId;
            this._startDate = startDate;
            this._endDate = endDate;
            this._eventSelector = EventSelector;
            this._dictCategories = dictCategories;
            this._mfdId = MFDId;
        }

        #endregion "--Public--"

        #endregion "--Constructors--""

        #region "--Methods--"

        #region "--Private--"

        private ArtefactInfo Prepare_ArtefactInfo_From_SubscriptionRequest(SDMXObjectModel.Registry.SubscriptionRequestType SubscriptionRequest, string FileName)
        {
            ArtefactInfo RetVal;
            SDMXObjectModel.Message.RegistryInterfaceType RegistryInterface;
            XmlDocument XmlContent;

            RetVal = null;
            XmlContent = null;

            try
            {
                RegistryInterface = this.Get_RegistryInterface_Object(SubscriptionRequest);
                XmlContent = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.RegistryInterfaceType), RegistryInterface);
                RetVal = new ArtefactInfo(SubscriptionRequest.Subscription.RegistryURN, string.Empty, string.Empty, string.Empty, ArtefactTypes.Subscription, FileName, XmlContent);
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

        private SDMXObjectModel.Message.RegistryInterfaceType Get_RegistryInterface_Object(SDMXObjectModel.Registry.SubscriptionRequestType SubscriptionRequestType)
        {
            SDMXObjectModel.Message.RegistryInterfaceType RetVal;

            RetVal = new SDMXObjectModel.Message.RegistryInterfaceType();
            RetVal.Header = this.Get_Appropriate_Header();

            RetVal.Item = new SDMXObjectModel.Registry.SubmitSubscriptionsRequestType();
            ((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)RetVal.Item).SubscriptionRequest = new List<SDMXObjectModel.Registry.SubscriptionRequestType>();
            ((SDMXObjectModel.Registry.SubmitSubscriptionsRequestType)RetVal.Item).SubscriptionRequest.Add(SubscriptionRequestType);

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
            SDMXObjectModel.Registry.SubscriptionRequestType SubscriptionRequest;
            int counter = 0;

            RetVal = null;

            try
            {
                SubscriptionRequest = new SDMXObjectModel.Registry.SubscriptionRequestType();

                SubscriptionRequest.Subscription = new SDMXObjectModel.Registry.SubscriptionType();
                SubscriptionRequest.Subscription.Organisation = new OrganisationReferenceType();

                if (this._userType == UserTypes.Consumer)
                {
                    SubscriptionRequest.Subscription.Organisation.Items.Add(new DataConsumerRefType(this._userId, Constants.DataConsumerScheme.AgencyId, Constants.DataConsumerScheme.Id, Constants.DataConsumerScheme.Version));
                }
                else
                {
                    SubscriptionRequest.Subscription.Organisation.Items.Add(new DataProviderRefType(this._userId, Constants.DataProviderScheme.AgencyId, Constants.DataProviderScheme.Id, Constants.DataProviderScheme.Version));
                }

                SubscriptionRequest.Subscription.RegistryURN = this._id;

                SubscriptionRequest.Subscription.NotificationMailTo = new List<SDMXObjectModel.Registry.NotificationURLType>();
                foreach (string notificationMailId in this._notificationMailIds)
                {
                    SubscriptionRequest.Subscription.NotificationMailTo.Add(new SDMXObjectModel.Registry.NotificationURLType(this._isSOAPMailIds[counter], this._notificationMailIds[counter]));
                    counter++;
                }

                
                SubscriptionRequest.Subscription.NotificationHTTP = new List<SDMXObjectModel.Registry.NotificationURLType>();

                counter = 0;
                foreach (string notificationHTTP in this._notificationHTTPs)
                {
                    SubscriptionRequest.Subscription.NotificationHTTP.Add(new SDMXObjectModel.Registry.NotificationURLType(this._isSOAPHTTPs[counter], this._notificationHTTPs[counter]));
                    counter++;
                }

                SubscriptionRequest.Subscription.SubscriberAssignedID = this._subscriberAssignedId;

                SubscriptionRequest.Subscription.ValidityPeriod = new SDMXObjectModel.Registry.ValidityPeriodType();
                SubscriptionRequest.Subscription.ValidityPeriod.StartDate = this._startDate;
                SubscriptionRequest.Subscription.ValidityPeriod.EndDate = this._endDate;
                
                SubscriptionRequest.Subscription.EventSelector = new List<object>();

                if (this._eventSelector == "Data Registration")
                {
                    SubscriptionRequest.Subscription.EventSelector.Add(new DataRegistrationEventsType());
                    ((DataRegistrationEventsType)SubscriptionRequest.Subscription.EventSelector[0]).Items = new CategoryReferenceType[this._dictCategories.Keys.Count];
                    ((DataRegistrationEventsType)SubscriptionRequest.Subscription.EventSelector[0]).ItemsElementName = new SDMXObjectModel.Registry.DataRegistrationEventsChoiceType[this._dictCategories.Keys.Count];

                    counter = 0;
                    foreach (string categoryId in this._dictCategories.Keys)
                    {
                        ((DataRegistrationEventsType)SubscriptionRequest.Subscription.EventSelector[0]).Items[counter] = new CategoryReferenceType();
                        ((DataRegistrationEventsType)SubscriptionRequest.Subscription.EventSelector[0]).ItemsElementName[counter] = SDMXObjectModel.Registry.DataRegistrationEventsChoiceType.Category;

                        ((CategoryReferenceType)((DataRegistrationEventsType)SubscriptionRequest.Subscription.EventSelector[0]).Items[counter]).Items.Add(new CategoryRefType(categoryId, this.AgencyId, this._dictCategories[categoryId], Constants.CategoryScheme.Sector.Version));

                        counter++;
                    }
                }
                else if (this._eventSelector == "Metadata Registration")
                {
                    SubscriptionRequest.Subscription.EventSelector.Add(new MetadataRegistrationEventsType());
                    ((MetadataRegistrationEventsType)SubscriptionRequest.Subscription.EventSelector[0]).Items = new MaintainableEventType[1];

                    ((MetadataRegistrationEventsType)SubscriptionRequest.Subscription.EventSelector[0]).Items[0] = new MaintainableEventType();
                    ((MetadataRegistrationEventsType)SubscriptionRequest.Subscription.EventSelector[0]).ItemsElementName = new MetadataRegistrationEventsChoiceType[1];
                    ((MetadataRegistrationEventsType)SubscriptionRequest.Subscription.EventSelector[0]).ItemsElementName[0] = SDMXObjectModel.Registry.MetadataRegistrationEventsChoiceType.MetadataflowReference;
                    ((MaintainableEventType)((MetadataRegistrationEventsType)SubscriptionRequest.Subscription.EventSelector[0]).Items[0]).Item = new MaintainableQueryType();
                    ((MaintainableQueryType)(((MaintainableEventType)((MetadataRegistrationEventsType)SubscriptionRequest.Subscription.EventSelector[0]).Items[0]).Item)).id = this._mfdId;
                    ((MaintainableQueryType)(((MaintainableEventType)((MetadataRegistrationEventsType)SubscriptionRequest.Subscription.EventSelector[0]).Items[0]).Item)).agencyID = this.AgencyId;
                    ((MaintainableQueryType)(((MaintainableEventType)((MetadataRegistrationEventsType)SubscriptionRequest.Subscription.EventSelector[0]).Items[0]).Item)).version = Constants.MFD.Area.Version;
                }
                else if (this._eventSelector == "Structural Metadata Registration")
                {
                    SubscriptionRequest.Subscription.EventSelector.Add(new StructuralRepositoryEventsType());
                    ((StructuralRepositoryEventsType)SubscriptionRequest.Subscription.EventSelector[0]).AgencyID = new List<string>();
                    ((StructuralRepositoryEventsType)SubscriptionRequest.Subscription.EventSelector[0]).AgencyID.Add(this.AgencyId);
                    ((StructuralRepositoryEventsType)SubscriptionRequest.Subscription.EventSelector[0]).Items = new EmptyType[1];
                    ((StructuralRepositoryEventsType)SubscriptionRequest.Subscription.EventSelector[0]).Items[0] = new EmptyType();
                    ((StructuralRepositoryEventsType)SubscriptionRequest.Subscription.EventSelector[0]).ItemsElementName = new StructuralRepositoryEventsChoiceType[1];
                    ((StructuralRepositoryEventsType)SubscriptionRequest.Subscription.EventSelector[0]).ItemsElementName[0] = SDMXObjectModel.Registry.StructuralRepositoryEventsChoiceType.AllEvents;
                }

                // Preparing Artefact and saving
                Artefact = this.Prepare_ArtefactInfo_From_SubscriptionRequest(SubscriptionRequest, this._id + Constants.XmlExtension);
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
