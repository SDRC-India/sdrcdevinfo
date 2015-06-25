using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using SDMXObjectModel.Message;
using SDMXObjectModel.Structure;
using SDMXObjectModel;
using System.Data;
using DevInfo.Lib.DI_LibDAL;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using SDMXObjectModel.Data.Generic;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using SDMXObjectModel.Query;

namespace DevInfo.Lib.DI_LibSDMX
{
    internal class MetadataQueryUtility
    {
        #region "Properties"

        #region "Private"

        private MetadataTypes _metadataType;

        private string _codeId;

        private string _agencyId;

        #endregion "Private"

        #region "Public"

        internal MetadataTypes MetadataType
        {
            get
            {
                return this._metadataType;
            }
            set
            {
                this._metadataType = value;
            }
        }

        internal string CodeId
        {
            get
            {
                return this._codeId;
            }
            set
            {
                this._codeId = value;
            }
        }

        internal string AgencyId
        {
            get
            {
                return this._agencyId;
            }
            set
            {
                this._agencyId = value;
            }
        }

        #endregion "Public"

        #endregion "Properties"

        #region "Constructors"

        #region "Private"

        #endregion "Private"

        #region "Public"

        internal MetadataQueryUtility(MetadataTypes metadataType, string codeId, string agencyId)
        {
            this._metadataType = metadataType;
            this._codeId = codeId;
            this._agencyId = agencyId;
        }

        #endregion "Public"

        #endregion "Constructors"

        #region "Methods"

        #region "Private"

        private SDMXObjectModel.Message.BasicHeaderType Get_Appropriate_Header()
        {
            SDMXObjectModel.Message.BasicHeaderType RetVal;
            SenderType Sender;
            PartyType Receiver;

            Sender = new SenderType(Constants.Header.SenderId, Constants.Header.SenderName, Constants.DefaultLanguage, new SDMXObjectModel.Message.ContactType(Constants.Header.Sender, Constants.Header.SenderDepartment, Constants.Header.SenderRole, Constants.DefaultLanguage));
            Sender.Contact[0].Items = new string[] { Constants.Header.SenderTelephone, Constants.Header.SenderEmail, Constants.Header.SenderFax };
            Sender.Contact[0].ItemsElementName = new SDMXObjectModel.Message.ContactChoiceType[] { SDMXObjectModel.Message.ContactChoiceType.Telephone, SDMXObjectModel.Message.ContactChoiceType.Email, SDMXObjectModel.Message.ContactChoiceType.Fax };

            Receiver = new PartyType(Constants.Header.ReceiverId, Constants.Header.ReceiverName, Constants.DefaultLanguage, new SDMXObjectModel.Message.ContactType(Constants.Header.Receiver, Constants.Header.ReceiverDepartment, Constants.Header.ReceiverRole, Constants.DefaultLanguage));
            Receiver.Contact[0].Items = new string[] { Constants.Header.ReceiverTelephone, Constants.Header.ReceiverEmail, Constants.Header.ReceiverFax };
            Receiver.Contact[0].ItemsElementName = new SDMXObjectModel.Message.ContactChoiceType[] { SDMXObjectModel.Message.ContactChoiceType.Telephone, SDMXObjectModel.Message.ContactChoiceType.Email, SDMXObjectModel.Message.ContactChoiceType.Fax };

            RetVal = new BasicHeaderType(Constants.Header.Id, true, DateTime.Now, Sender, Receiver);

            return RetVal;
        }

        #endregion "Private"

        #region "Public"

        internal XmlDocument Get_Query()
        {
            XmlDocument RetVal;
            SDMXObjectModel.Message.MetadataQueryType MetadataQuery;

            RetVal = null;
            MetadataQuery = null;

            try
            {
                MetadataQuery = new SDMXObjectModel.Message.MetadataQueryType();

                MetadataQuery.Header = this.Get_Appropriate_Header();
                MetadataQuery.Query = new SDMXObjectModel.Query.MetadataQueryType();

                MetadataQuery.Query.ReturnDetails = new MetadataReturnDetailsType();

                MetadataQuery.Query.MetadataParameters = new MetadataParametersAndType();

                MetadataQuery.Query.MetadataParameters.AttachedObject = new List<SDMXObjectModel.Common.ObjectReferenceType>();
                MetadataQuery.Query.MetadataParameters.AttachedObject.Add(new SDMXObjectModel.Common.ObjectReferenceType());

                MetadataQuery.Query.MetadataParameters.AttachedObject[0].Items = new List<object>();
                switch(this._metadataType)
                {
                    case MetadataTypes.Area:
                        MetadataQuery.Query.MetadataParameters.AttachedObject[0].Items.Add(new SDMXObjectModel.Common.ObjectRefType(this.CodeId, this.AgencyId, null, Constants.CodeList.Area.Id, Constants.CodeList.Area.Version, SDMXObjectModel.Common.ObjectTypeCodelistType.Code, true, SDMXObjectModel.Common.PackageTypeCodelistType.codelist, true));
                        break;
                    case MetadataTypes.Indicator:
                        MetadataQuery.Query.MetadataParameters.AttachedObject[0].Items.Add(new SDMXObjectModel.Common.ObjectRefType(this.CodeId, this.AgencyId, null, Constants.CodeList.Indicator.Id, Constants.CodeList.Indicator.Version, SDMXObjectModel.Common.ObjectTypeCodelistType.Code, true, SDMXObjectModel.Common.PackageTypeCodelistType.codelist, true));
                        break;
                    case MetadataTypes.Source:
                        MetadataQuery.Query.MetadataParameters.AttachedObject[0].Items.Add(new SDMXObjectModel.Common.ObjectRefType(this.CodeId, this.AgencyId, null, Constants.CategoryScheme.Source.Id, Constants.CategoryScheme.Source.Version, SDMXObjectModel.Common.ObjectTypeCodelistType.Category, true, SDMXObjectModel.Common.PackageTypeCodelistType.categoryscheme, true));
                        break;
                    default:
                        break;
                }

                RetVal = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.MetadataQueryType), MetadataQuery);
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

        #endregion "Public"

        #endregion "Methods"
    }
}
