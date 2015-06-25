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

namespace DevInfo.Lib.DI_LibSDMX
{
    internal class RegistrationUtility : ArtefactUtility
    {
        #region "--Properties--"

        #region "--Private--"

        private string _id;

        private string _paId;

        private string _dataURL;

        private bool _isREST;

        private string _wadlURL;

        private bool _isSOAP;

        private string _wsdlURL;

        private string _fileURL;

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

        public string PaId
        {
            get
            {
                return this._paId;
            }
            set
            {
                this._paId = value;
            }
        }

        public string DataURL
        {
            get
            {
                return this._dataURL;
            }
            set
            {
                this._dataURL = value;
            }
        }

        public bool IsREST
        {
            get
            {
                return this._isREST;
            }
            set
            {
                this._isREST = value;
            }
        }

        public string WadlURL
        {
            get
            {
                return this._wadlURL;
            }
            set
            {
                this._wadlURL = value;
            }
        }

        public bool IsSOAP
        {
            get
            {
                return this._isSOAP;
            }
            set
            {
                this._isSOAP = value;
            }
        }

        public string WsdlURL
        {
            get
            {
                return this._wsdlURL;
            }
            set
            {
                this._wsdlURL = value;
            }
        }

        public string FileURL
        {
            get
            {
                return this._fileURL;
            }
            set
            {
                this._fileURL = value;
            }
        }

        #endregion "--Public--"

        #endregion "--Properties--""

        #region "--Constructors--"

        #region "--Private--"

        #endregion "--Private--"

        #region "--Public--"

        internal RegistrationUtility(string id, string paId, string dataURL, bool isREST, string wadlURL, bool isSOAP, string wsdlURL, string fileURL, string agencyId, Header header, string outputFolder)
            : base(agencyId, string.Empty, header, outputFolder)
        {
            this._id = id;
            this._paId = paId;
            this._dataURL = dataURL;
            this._isREST = isREST;
            this._wadlURL = wadlURL;
            this._isSOAP = isSOAP;
            this._wsdlURL = wsdlURL;
            this._fileURL = fileURL;
        }

        #endregion "--Public--"

        #endregion "--Constructors--""

        #region "--Methods--"

        #region "--Private--"

        private ArtefactInfo Prepare_ArtefactInfo_From_RegistrationRequest(SDMXObjectModel.Registry.RegistrationRequestType RegistrationRequest, string FileName)
        {
            ArtefactInfo RetVal;
            SDMXObjectModel.Message.RegistryInterfaceType RegistryInterface;
            XmlDocument XmlContent;

            RetVal = null;
            XmlContent = null;

            try
            {
                RegistryInterface = this.Get_RegistryInterface_Object(RegistrationRequest);
                XmlContent = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.RegistryInterfaceType), RegistryInterface);
                RetVal = new ArtefactInfo(RegistrationRequest.Registration.id, string.Empty, string.Empty, string.Empty, ArtefactTypes.Registration, FileName, XmlContent);
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

        private SDMXObjectModel.Message.RegistryInterfaceType Get_RegistryInterface_Object(SDMXObjectModel.Registry.RegistrationRequestType RegistrationRequest)
        {
            SDMXObjectModel.Message.RegistryInterfaceType RetVal;

            RetVal = new SDMXObjectModel.Message.RegistryInterfaceType();
            RetVal.Header = this.Get_Appropriate_Header();

            RetVal.Item = new SDMXObjectModel.Registry.SubmitRegistrationsRequestType();
            ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RetVal.Item).RegistrationRequest = new List<SDMXObjectModel.Registry.RegistrationRequestType>();
            ((SDMXObjectModel.Registry.SubmitRegistrationsRequestType)RetVal.Item).RegistrationRequest.Add(RegistrationRequest);

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
            SDMXObjectModel.Registry.RegistrationRequestType RegistrationRequest;

            RetVal = null;

            try
            {
                RegistrationRequest = new SDMXObjectModel.Registry.RegistrationRequestType();
                RegistrationRequest.Registration = new SDMXObjectModel.Registry.RegistrationType(this._id);
                RegistrationRequest.Registration.indexTimeSeries = true;

                RegistrationRequest.Registration.ProvisionAgreement = new ProvisionAgreementReferenceType();
                RegistrationRequest.Registration.ProvisionAgreement.Items.Add(new ProvisionAgreementRefType(this._paId, this.AgencyId, Constants.PA.Version));

                RegistrationRequest.Registration.Datasource = new List<object>();
                RegistrationRequest.Registration.Datasource.Add(new SDMXObjectModel.Registry.QueryableDataSourceType(this._dataURL, this._isREST, this._wadlURL, this._isSOAP, this._wsdlURL));
                RegistrationRequest.Registration.Datasource.Add(this._fileURL);

                // Preparing Artefact and saving
                Artefact = this.Prepare_ArtefactInfo_From_RegistrationRequest(RegistrationRequest, this._id + Constants.XmlExtension);
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
