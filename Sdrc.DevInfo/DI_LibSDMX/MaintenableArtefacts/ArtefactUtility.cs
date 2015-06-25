using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Data;
using SDMXObjectModel.Query;
using System.IO;
using SDMXObjectModel.Message;
using SDMXObjectModel.Structure;

namespace DevInfo.Lib.DI_LibSDMX
{
    internal abstract class ArtefactUtility
    {

        #region "Properties"

        #region "Private"

        private string _agencyId;

        private string _language;

        private Header _header;

        private string _outputFolder;

        #endregion "Private"

        #region "Public"

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

        internal string Language
        {
            get
            {
                return this._language;
            }
            set
            {
                this._language = value;
            }
        }

        internal Header Header
        {
            get
            {
                return this._header;
            }
            set
            {
                this._header = value;
            }
        }

        internal string OutputFolder
        {
            get
            {
                return this._outputFolder;
            }
            set
            {
                this._outputFolder = value;
            }
        }

        #endregion "Public"

        #endregion "Properties"

        #region "Constructors"

        #region "Private"

        #endregion "Private"

        #region "Public"

        internal ArtefactUtility(string agencyId, string language, Header header, string outputFolder)
        {
            this._agencyId = agencyId;
            this._language = language;
            this._header = header;
            this._outputFolder = outputFolder;
        }

        #endregion "Public"

        #endregion "Constructors"

        #region "Methods"

        #region "Private"

        #endregion "Private"

        #region "Public"

        public abstract List<ArtefactInfo> Generate_Artefact();

        public bool Save_Artefacts(List<ArtefactInfo> Artefacts)
        {
            bool RetVal;

            RetVal = true;

            try
            {
                if (Artefacts != null)
                {
                    foreach (ArtefactInfo Artefact in Artefacts)
                    {
                        if (Artefact != null)
                        {
                            Artefact.Content.Save(Path.Combine(OutputFolder, Artefact.FileName));
                            
                            //-- Raise event for generated new file
                            SDMXUtility.Raise_Notify_File_Name_Event(Artefact.FileName);

                            //Artefact.Content = null;                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                RetVal = false;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        public void Add_ArtefactInfo_To_List(ref List<ArtefactInfo> Artefacts, ArtefactInfo Artefact)
        {
            if (Artefacts == null)
            {
                Artefacts = new List<ArtefactInfo>();
            }

            if (Artefact != null)
            {
                Artefacts.Add(Artefact);
            }
        }

        public virtual SDMXObjectModel.Message.StructureHeaderType Get_Appropriate_Header()
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

        public string[] SplitString(string valueString, string delimiter)
        {
            string[] RetVal;
            int Index = 0;
            string Value;
            List<string> SplittedList = new List<string>();

            while (true)
            {
                Index = valueString.IndexOf(delimiter);
                if (Index == -1)
                {
                    if (!string.IsNullOrEmpty(valueString))
                    {
                        SplittedList.Add(valueString);
                    }
                    break;
                }
                else
                {
                    Value = valueString.Substring(0, Index);
                    valueString = valueString.Substring(Index + delimiter.Length);
                    SplittedList.Add(Value);

                }
            }

            RetVal = SplittedList.ToArray();

            return RetVal;
        }

        #endregion "Public"

        #endregion "Methods"
    }
}
