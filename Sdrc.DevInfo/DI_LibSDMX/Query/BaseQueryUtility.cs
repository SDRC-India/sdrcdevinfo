using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Data;
using SDMXObjectModel.Query;
using SDMXObjectModel.Message;
using SDMXObjectModel.Common;

namespace DevInfo.Lib.DI_LibSDMX
{
    internal class BaseQueryUtility
    {
        #region "Properties"

        #region "Private"

        private Dictionary<string, string> _dictUserSelections;

        private DataReturnDetailTypes _dataReturnDetailType;

        private string _agencyId;

        private DIConnection _diConnection;

        private DIQueries _diQueries;

        #endregion "Private"

        #region "Public"

        internal Dictionary<string, string> DictUserSelections
        {
            get
            {
                return this._dictUserSelections;
            }
            set
            {
                this._dictUserSelections = value;
            }
        }

        internal DataReturnDetailTypes DataReturnDetailType
        {
            get
            {
                return this._dataReturnDetailType;
            }
            set
            {
                this._dataReturnDetailType = value;
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

        internal DIConnection DIConnection
        {
            get
            {
                return this._diConnection;
            }
            set
            {
                this._diConnection = value;
            }
        }

        internal DIQueries DIQueries
        {
            get
            {
                return this._diQueries;
            }
            set
            {
                this._diQueries = value;
            }
        }

        #endregion "Public"

        #endregion "Properties"

        #region "Constructors"

        #region "Private"

        #endregion "Private"

        #region "Public"

        internal BaseQueryUtility(Dictionary<string, string> dictUserSelections, DataReturnDetailTypes dataReturnDetailType, string agencyId, DIConnection DIConnection, DIQueries DIQueries)
        {
            this._dictUserSelections = dictUserSelections;
            this._dataReturnDetailType = dataReturnDetailType;
            this._agencyId = agencyId;
            this._diConnection = DIConnection;
            this._diQueries = DIQueries;
        }

        #endregion "Public"

        #endregion "Constructors"

        #region "Methods"

        #region "Private"

        #endregion "Private"

        #region "Public"

        internal virtual XmlDocument Get_Query()
        {
            return new XmlDocument();
        }

        internal SDMXObjectModel.Message.BasicHeaderType Get_Appropriate_Header()
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

        internal Dictionary<string, string> Get_Subgroup_Breakup(string SubgroupValGId, DataTable DtSubgroupBreakup)
        {
            Dictionary<string, string> DictSubgroupBreakup = new Dictionary<string, string>();

            foreach (DataRow DrSubgroupBreakup in DtSubgroupBreakup.Select(DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId + "='" + SubgroupValGId + "'"))
            {
                DictSubgroupBreakup.Add(DrSubgroupBreakup[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupTypes.SubgroupTypeGID].ToString(), DrSubgroupBreakup[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Subgroup.SubgroupGId].ToString());
            }

            return DictSubgroupBreakup;
        }

        internal List<DataStructureReferenceType> Get_DataStructure_Reference()
        {
            List<DataStructureReferenceType> RetVal;

            RetVal = new List<SDMXObjectModel.Common.DataStructureReferenceType>();
            RetVal.Add(new SDMXObjectModel.Common.DataStructureReferenceType());
            RetVal[0].Items = new List<object>();
            RetVal[0].Items.Add(new DataStructureRefType(Constants.DSD.Id, this.AgencyId, Constants.DSD.Version));

            return RetVal;
        }
        #endregion "Public"

        #endregion "Methods"
    }
}
