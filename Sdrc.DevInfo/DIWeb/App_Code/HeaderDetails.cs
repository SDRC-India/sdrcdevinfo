using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using DevInfo.Lib.DI_LibSDMX;
using SDMXObjectModel;
using SDMXObjectModel.Registry;
using SDMXObjectModel.Common;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Runtime.Serialization.Formatters.Soap;
using System.Net;
using System.Net.Mail;
using SDMXObjectModel.Message;
using System.Text;
using DevInfo.Lib.DI_LibDATA;
using System.Web.Services.Protocols;
using SDMXObjectModel.Query;
using System.Xml;
using System.Configuration;
using DevInfo.Lib.DI_LibSDMX;


/// <summary>
/// Get and header Deatils based on DbNid
/// </summary>
public class HeaderDetails
{
    #region "-- Private --"

    #region "-- Variables / Properties --"

    //Header File Name
    internal static string HeaderFileName = Constants.FileName.HeaderFileName;
    #endregion

    #region "-- Methods --"
    private int Get_AssociatedDB_NId(string dSDDBId)
    {
        int RetVal;
        string DBXMLFileName;
        XmlDocument DBXMLDocument;
        XmlNodeList DBList;

        RetVal = -1;
        DBXMLFileName = string.Empty;
        DBXMLDocument = null;
        DBList = null;

        DBXMLFileName = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.DBConnectionsFile]);
        DBXMLDocument = new XmlDocument();
        DBXMLDocument.Load(DBXMLFileName);
        DBList = DBXMLDocument.GetElementsByTagName(Constants.XmlFile.Db.Tags.Database);

        foreach (XmlNode DB in DBList)
        {
            if (DB.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.Id].Value == dSDDBId)
            {
                if (DB.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.AssosciatedDb] != null)
                {
                    RetVal = Convert.ToInt32(DB.Attributes[Constants.XmlFile.Db.Tags.DatabaseAttributes.AssosciatedDb].Value);
                    break;
                }
            }
        }

        return RetVal;
    }

    /// <summary>
    /// Reads Devinfo header file and return object
    /// </summary>
    /// <param name="dbNId">Nid Of DevInfo Database</param>
    /// <returns>Object containing header</returns>
    private HeaderDetailsTemplate GetDIHeaderStructure(string dbNId)
    {
        HeaderDetailsTemplate RetVal;
        string AssociatedDbNId, SourceHeaderFileNameWPath;
        StructureType ObjStructure;
        AssociatedDbNId = string.Empty;
        SourceHeaderFileNameWPath = string.Empty;
        ContactType SenderContact = new ContactType();
        ContactType RecieverContact = new ContactType();

        try
        {
            RetVal = new HeaderDetailsTemplate();
            ObjStructure = new StructureType();
            SourceHeaderFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + dbNId + "\\" + Constants.FolderName.SDMX.sdmx + HeaderFileName);

            if (File.Exists(SourceHeaderFileNameWPath))
            {
                ObjStructure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), SourceHeaderFileNameWPath);

                RetVal.HeaderId = !string.IsNullOrEmpty(ObjStructure.Header.ID) ? ObjStructure.Header.ID : string.Empty;

                SenderContact = ObjStructure.Header.Sender.Contact[0];
                RetVal.SenderId = !string.IsNullOrEmpty(ObjStructure.Header.Sender.id.ToString()) ? ObjStructure.Header.Sender.id.ToString() : string.Empty;
                RetVal.SenderName = !string.IsNullOrEmpty(ObjStructure.Header.Sender.Name[0].Value.ToString()) ? ObjStructure.Header.Sender.Name[0].Value.ToString() : string.Empty;
                RetVal.LanguageCode = !string.IsNullOrEmpty(ObjStructure.Header.Sender.Name[0].lang.ToString()) ? ObjStructure.Header.Sender.Name[0].lang.ToString() : string.Empty;
                RetVal.SenderDepartment = !string.IsNullOrEmpty(SenderContact.Department[0].Value) ? SenderContact.Department[0].Value.ToString() : string.Empty;
                RetVal.SenderRole = !string.IsNullOrEmpty(SenderContact.Role[0].Value) ? SenderContact.Role[0].Value.ToString() : string.Empty;
                RetVal.SenderContactName = !string.IsNullOrEmpty(SenderContact.Name[0].Value.ToString()) ? SenderContact.Name[0].Value.ToString() : string.Empty;
                RetVal.SenderTelephone = !string.IsNullOrEmpty(SenderContact.Items[0].ToString()) ? SenderContact.Items[0].ToString() : string.Empty;
                RetVal.SenderEmail = !string.IsNullOrEmpty(SenderContact.Items[1]) ? SenderContact.Items[1].ToString() : string.Empty;
                RetVal.SenderFax = !string.IsNullOrEmpty(SenderContact.Items[2].ToString()) ? SenderContact.Items[2].ToString() : string.Empty;

                RecieverContact = ObjStructure.Header.Receiver[0].Contact[0];

                RetVal.RecieverId = !string.IsNullOrEmpty(ObjStructure.Header.Receiver[0].id.ToString()) ? ObjStructure.Header.Receiver[0].id.ToString() : string.Empty;
                RetVal.RecieverName = !string.IsNullOrEmpty(ObjStructure.Header.Receiver[0].Name[0].Value.ToString()) ? ObjStructure.Header.Receiver[0].Name[0].Value.ToString() : string.Empty;
                RetVal.RecieverDepartment = !string.IsNullOrEmpty(RecieverContact.Department[0].Value.ToString()) ? RecieverContact.Department[0].Value.ToString() : string.Empty;
                RetVal.RecieverRole = !string.IsNullOrEmpty(RecieverContact.Role[0].Value.ToString()) ? RecieverContact.Role[0].Value.ToString() : string.Empty;
                RetVal.RecieverContactName = !string.IsNullOrEmpty(RecieverContact.Name[0].Value.ToString()) ? RecieverContact.Name[0].Value.ToString() : string.Empty;
                RetVal.RecieverTelephone = !string.IsNullOrEmpty(RecieverContact.Items[0].ToString()) ? RecieverContact.Items[0].ToString() : string.Empty;
                RetVal.RecieverEmail = !string.IsNullOrEmpty(RecieverContact.Items[1].ToString()) ? RecieverContact.Items[1].ToString() : string.Empty;
                RetVal.RecieverFax = !string.IsNullOrEmpty(RecieverContact.Items[2].ToString()) ? RecieverContact.Items[2].ToString() : string.Empty;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            RetVal = null;
            throw ex;
        }
        return RetVal;
    }
    /// <summary>
    /// Reads UNSD header file and return object
    /// </summary>
    /// <param name="dbNId">Nid Of unsd Database</param>
    /// <returns>Object containing header</returns>
    private HeaderDetailsTemplate GetUNSDHeaderStructure(string dbNId)
    {
        HeaderDetailsTemplate RetVal = null;
        string AssociatedDbNId, SourceHeaderFileNameWPath;
        SDMXApi_2_0.Message.StructureType ObjStructure;
        AssociatedDbNId = string.Empty;
        SourceHeaderFileNameWPath = string.Empty;
        RetVal = new HeaderDetailsTemplate();
        SDMXApi_2_0.Message.ContactType SenderContact;
        SDMXApi_2_0.Message.ContactType RecieverContact;
        try
        {
            SourceHeaderFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + dbNId + "\\" + Constants.FolderName.SDMX.sdmx + HeaderFileName);
            if (File.Exists(SourceHeaderFileNameWPath))
            {
                ObjStructure = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), SourceHeaderFileNameWPath);

                RetVal.HeaderId = ObjStructure.Header.ID;
                RetVal.Prepared = !string.IsNullOrEmpty(ObjStructure.Header.Prepared.ToString()) ? ObjStructure.Header.Prepared.ToString() : string.Empty;
                RetVal.HeaderDsdName = !string.IsNullOrEmpty(ObjStructure.Header.Name[0].Value.ToString()) ? ObjStructure.Header.Name[0].Value.ToString() : string.Empty;
                RetVal.SenderId = !string.IsNullOrEmpty(ObjStructure.Header.Sender[0].id.ToString()) ? ObjStructure.Header.Sender[0].id.ToString() : string.Empty;
                RetVal.SenderName = !string.IsNullOrEmpty(ObjStructure.Header.Sender[0].Name[0].Value.ToString()) ? ObjStructure.Header.Sender[0].Name[0].Value.ToString() : string.Empty;
                RetVal.LanguageCode = !string.IsNullOrEmpty(ObjStructure.Header.Sender[0].Name[0].lang.ToString()) ? ObjStructure.Header.Sender[0].Name[0].lang.ToString() : string.Empty;
                SenderContact = ObjStructure.Header.Sender[0].Contact[0];

                RetVal.SenderDepartment = !string.IsNullOrEmpty(SenderContact.Department[0].Value.ToString()) ? SenderContact.Department[0].Value.ToString() : string.Empty;
                RetVal.SenderRole = !string.IsNullOrEmpty(SenderContact.Role[0].Value.ToString()) ? SenderContact.Role[0].Value.ToString() : string.Empty;
                RetVal.SenderContactName = !string.IsNullOrEmpty(SenderContact.Name[0].Value.ToString()) ? SenderContact.Name[0].Value.ToString() : string.Empty;
                if (SenderContact.Items.Length > 2)
                {
                    RetVal.SenderTelephone = !string.IsNullOrEmpty(SenderContact.Items[0].ToString()) ? SenderContact.Items[0].ToString() : string.Empty;
                    RetVal.SenderFax = !string.IsNullOrEmpty(SenderContact.Items[1].ToString()) ? SenderContact.Items[1].ToString() : string.Empty;
                    RetVal.SenderEmail = !string.IsNullOrEmpty(SenderContact.Items[2].ToString()) ? SenderContact.Items[2].ToString() : string.Empty;
                }
                else if(SenderContact.Items.Length >1)
                {
                    RetVal.SenderTelephone = !string.IsNullOrEmpty(SenderContact.Items[0].ToString()) ? SenderContact.Items[0].ToString() : string.Empty;
                    RetVal.SenderEmail = !string.IsNullOrEmpty(SenderContact.Items[2].ToString()) ? SenderContact.Items[2].ToString() : string.Empty;
                }
                else
                {
                    RetVal.SenderTelephone = !string.IsNullOrEmpty(SenderContact.Items[0].ToString()) ? SenderContact.Items[0].ToString() : string.Empty;
                }
                RecieverContact = ObjStructure.Header.Receiver[0].Contact[0];

                RetVal.RecieverId = !string.IsNullOrEmpty(ObjStructure.Header.Receiver[0].id.ToString()) ? ObjStructure.Header.Receiver[0].id.ToString() : string.Empty;
                RetVal.RecieverName = !string.IsNullOrEmpty(ObjStructure.Header.Receiver[0].Name[0].Value) ? ObjStructure.Header.Receiver[0].Name[0].Value.ToString() : string.Empty;
                RetVal.RecieverDepartment = !string.IsNullOrEmpty(RecieverContact.Department[0].Value) ? RecieverContact.Department[0].Value.ToString() : string.Empty;
                RetVal.RecieverRole = !string.IsNullOrEmpty(RecieverContact.Role[0].Value) ? RecieverContact.Role[0].Value.ToString() : string.Empty;
                RetVal.RecieverContactName = !string.IsNullOrEmpty(RecieverContact.Name[0].Value) ? RecieverContact.Name[0].Value.ToString() : string.Empty;
                if (RecieverContact.Items.Length > 2)
                {
                    RetVal.RecieverTelephone = !string.IsNullOrEmpty(RecieverContact.Items[0].ToString()) ? RecieverContact.Items[0].ToString() : string.Empty;
                    RetVal.RecieverEmail = !string.IsNullOrEmpty(RecieverContact.Items[1].ToString()) ? RecieverContact.Items[1].ToString() : string.Empty;
                    RetVal.RecieverFax = !string.IsNullOrEmpty(RecieverContact.Items[2].ToString()) ? RecieverContact.Items[2].ToString() : string.Empty;
                }
                else if (RecieverContact.Items.Length >1)
                {
                    RetVal.RecieverTelephone = !string.IsNullOrEmpty(RecieverContact.Items[0].ToString()) ? RecieverContact.Items[0].ToString() : string.Empty;
                    RetVal.RecieverEmail = !string.IsNullOrEmpty(RecieverContact.Items[1].ToString()) ? RecieverContact.Items[1].ToString() : string.Empty;
                }
                else
                {
                    RetVal.RecieverTelephone = !string.IsNullOrEmpty(RecieverContact.Items[0].ToString()) ? RecieverContact.Items[0].ToString() : string.Empty;
                }
            }
        }
        catch (Exception Ex)
        {
            Global.CreateExceptionString(Ex, null);
            RetVal = null;
            throw Ex;
        }
        return RetVal;
    }
    /// <summary>
    /// Save Values to devinfo header file
    /// </summary>
    /// <param name="dbNId">Nid of devinfo database</param>
    /// <param name="objHeaderDetail">Object containg header details to be saved</param>
    /// <returns>true if header file updated success fully, else return false</returns>
    private bool SaveDIHeaderStructure(string dbNId, SDMXObjectModel.Message.StructureType objStructure)
    {
        bool RetVal = false;
        string SourceHeaderFileNameWPath;
        SourceHeaderFileNameWPath = string.Empty;
        XmlDocument ObjHeaderXml;
        try
        {
            ObjHeaderXml = new XmlDocument();
            // Get path of header xml file
            SourceHeaderFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + dbNId + "\\" + Constants.FolderName.SDMX.sdmx + HeaderFileName);
            // Check if file exists
            if (File.Exists(SourceHeaderFileNameWPath))
            { // Searilize structure element to xml document
                ObjHeaderXml = SDMXObjectModel.Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.StructureType), objStructure);
                // Save searilizes xml as header xml
                ObjHeaderXml.Save(SourceHeaderFileNameWPath);
                RetVal = true;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            RetVal = false;
            throw ex;
        }
        return RetVal;
    }
    /// <summary>
    /// Save Values to unsd header file by dbnid
    /// </summary>
    /// <param name="dbNId">Nid of unsd database</param>
    /// <param name="objHeaderDetail">Object containg header details to be saved</param>
    /// <returns>true if header file updated success fully, else return false</returns>
    private bool SaveUNSDHeaderStructure(string dbNId, SDMXApi_2_0.Message.StructureType objStructure)
    {
        bool RetVal = false;
        string SourceHeaderFileNameWPath;
        XmlDocument ObjHeaderXml;
        SourceHeaderFileNameWPath = string.Empty;
        try
        {
            // Get Path of source header file
            SourceHeaderFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + dbNId + "\\" + Constants.FolderName.SDMX.sdmx + HeaderFileName);
            //Check if file exists
            if (File.Exists(SourceHeaderFileNameWPath))
            {
                ObjHeaderXml = new XmlDocument();
                // Searilize structure element to xml document
                ObjHeaderXml = SDMXApi_2_0.Serializer.SerializeToXmlDocument(typeof(SDMXApi_2_0.Message.StructureType), objStructure);
                // Save searilizes xml as header xml
                ObjHeaderXml.Save(SourceHeaderFileNameWPath);
                // Retuen true ifoperation completed succesfully
                RetVal = true;
            }
        }
        catch (Exception Ex)
        {
            Global.CreateExceptionString(Ex, null);
            RetVal = false;
            throw Ex;
        }
        return RetVal;
    }

    /// <summary>
    /// Sets header details to sdmx structure header object and return object
    /// </summary>
    /// <param name="ObjHeaderDet">Class containg fields for header detail</param>
    /// <returns>Structure header object with fields initlized</returns>
    private SDMXApi_2_0.Message.HeaderType GetSdmxMessageHeaderStructure(HeaderDetailsTemplate ObjHeaderDet)
    {
        SDMXApi_2_0.Message.HeaderType ObjHeader = null;
        SDMXApi_2_0.Message.PartyType SenderParty;
        SDMXApi_2_0.Message.PartyType RecieverParty;
        SDMXApi_2_0.Message.ContactType SenderContact;
        SDMXApi_2_0.Message.ContactType ReceiverContact;
        string LanguageCode = string.Empty;
        try
        {
            SenderParty = new SDMXApi_2_0.Message.PartyType();
            RecieverParty = new SDMXApi_2_0.Message.PartyType();
            SenderParty.Contact = new List<SDMXApi_2_0.Message.ContactType>();
            RecieverParty.Contact = new List<SDMXApi_2_0.Message.ContactType>();
            LanguageCode = ObjHeaderDet.LanguageCode;

            #region "--Set sender fields value--"
            // Set sender Id
            SenderParty.id = ObjHeaderDet.SenderId;
            // Set sender name 
            SenderParty.Name = SetHeaderFilds(LanguageCode, ObjHeaderDet.SenderName);
            // Set sender contact Name 
            SenderContact = new SDMXApi_2_0.Message.ContactType();
            SenderContact.Name = SetHeaderFilds(LanguageCode, ObjHeaderDet.SenderContactName);
            // Set reciever contact role
            SenderContact.Role = SetHeaderFilds(LanguageCode, ObjHeaderDet.SenderRole);
            // Set sender contact department
            SenderContact.Department = SetHeaderFilds(LanguageCode, ObjHeaderDet.SenderDepartment);

            // Set value of sender item fileds, telephone,emailid and fax   
            SenderContact.Items = new string[] { ObjHeaderDet.SenderTelephone, ObjHeaderDet.SenderEmail, ObjHeaderDet.SenderFax };
            SenderContact.ItemsElementName = new SDMXApi_2_0.Message.ContactChoiceType[] { SDMXApi_2_0.Message.ContactChoiceType.Telephone, SDMXApi_2_0.Message.ContactChoiceType.Email, SDMXApi_2_0.Message.ContactChoiceType.Fax };

            SenderParty.Contact.Add(SenderContact);
            #endregion

            #region "--set Reciever fields value--"
            // Set Reciever Id
            RecieverParty.id = ObjHeaderDet.RecieverId;
            // Set reciever name
            RecieverParty.Name = SetHeaderFilds(LanguageCode, ObjHeaderDet.RecieverName);
            ReceiverContact = new SDMXApi_2_0.Message.ContactType();
            // Set reciever contact name
            ReceiverContact.Name = SetHeaderFilds(LanguageCode, ObjHeaderDet.RecieverContactName);
            // Set reciever contact role
            ReceiverContact.Role = SetHeaderFilds(LanguageCode, ObjHeaderDet.RecieverRole);
            // Set reciever contact department
            ReceiverContact.Department = SetHeaderFilds(LanguageCode, ObjHeaderDet.RecieverDepartment);
            // Set value of sender item fileds, telephone,emailid and fax
            ReceiverContact.Items = new string[] { ObjHeaderDet.RecieverTelephone, ObjHeaderDet.RecieverEmail, ObjHeaderDet.RecieverFax };
            ReceiverContact.ItemsElementName = new SDMXApi_2_0.Message.ContactChoiceType[] { SDMXApi_2_0.Message.ContactChoiceType.Telephone, SDMXApi_2_0.Message.ContactChoiceType.Email, SDMXApi_2_0.Message.ContactChoiceType.Fax };

            RecieverParty.Contact.Add(ReceiverContact);
            #endregion

            //Init line header object
            ObjHeader = new SDMXApi_2_0.Message.HeaderType();
            // Add SenderParty object to headers sender property
            ObjHeader.Sender.Add(SenderParty);
            // Add RecieverParty object to headers reciever property
            ObjHeader.Receiver.Add(RecieverParty);
            // Set id of header
            ObjHeader.ID = ObjHeaderDet.HeaderId;
            ObjHeader.Name = SetHeaderFilds(LanguageCode, ObjHeaderDet.HeaderDsdName);
            ObjHeader.Prepared = ObjHeaderDet.Prepared;
        }
        catch (Exception Ex)
        {
            throw Ex;
        }

        return ObjHeader;
    }

    /// <summary>
    /// This method creates object for header elements from input paramaters  
    /// </summary>
    /// <param name="Lang">language of element</param>
    /// <param name="FiledValue">value of element</param>
    /// <returns>list containing TextType object</returns>
    private List<SDMXApi_2_0.Common.TextType> SetHeaderFilds(string Lang, string FiledValue)
    {
        SDMXApi_2_0.Common.TextType ObjTextType;
        List<SDMXApi_2_0.Common.TextType> ObjListTextType;
        ObjTextType = new SDMXApi_2_0.Common.TextType();
        ObjListTextType = new List<SDMXApi_2_0.Common.TextType>();

        ObjTextType.lang = Lang;
        ObjTextType.Value = FiledValue;
        ObjListTextType.Add(ObjTextType);
        return ObjListTextType;
    }

    /// <summary>
    /// Sets header details to di structure header object and return object
    /// </summary>
    /// <param name="ObjHeaderDet">Class containg fields for header detail</param>
    /// <returns>Structure header object with fields initlized</returns>
    private SDMXObjectModel.Message.StructureHeaderType GetDiMessageHeaderStructure(HeaderDetailsTemplate ObjHeaderDet)
    {
        SDMXObjectModel.Message.StructureHeaderType ObjStructureHeader = null;
        SenderType Sender;
        PartyType Receiver;
        Sender = new SenderType(ObjHeaderDet.SenderId, ObjHeaderDet.SenderName, ObjHeaderDet.LanguageCode, new SDMXObjectModel.Message.ContactType(ObjHeaderDet.SenderContactName, ObjHeaderDet.SenderDepartment, ObjHeaderDet.SenderRole, ObjHeaderDet.LanguageCode));
        Sender.Contact[0].Items = new string[] { ObjHeaderDet.SenderTelephone, ObjHeaderDet.SenderEmail, ObjHeaderDet.SenderFax };
        Sender.Contact[0].ItemsElementName = new SDMXObjectModel.Message.ContactChoiceType[] { SDMXObjectModel.Message.ContactChoiceType.Telephone, SDMXObjectModel.Message.ContactChoiceType.Email, SDMXObjectModel.Message.ContactChoiceType.Fax };

        Receiver = new PartyType(ObjHeaderDet.RecieverId, ObjHeaderDet.RecieverName, ObjHeaderDet.LanguageCode, new SDMXObjectModel.Message.ContactType(ObjHeaderDet.RecieverContactName, ObjHeaderDet.RecieverDepartment, ObjHeaderDet.RecieverRole, ObjHeaderDet.LanguageCode));
        Receiver.Contact[0].Items = new string[] { ObjHeaderDet.RecieverTelephone, ObjHeaderDet.RecieverEmail, ObjHeaderDet.RecieverFax };
        Receiver.Contact[0].ItemsElementName = new SDMXObjectModel.Message.ContactChoiceType[] { SDMXObjectModel.Message.ContactChoiceType.Telephone, SDMXObjectModel.Message.ContactChoiceType.Email, SDMXObjectModel.Message.ContactChoiceType.Fax };

        ObjStructureHeader = new StructureHeaderType(ObjHeaderDet.HeaderId, true, DateTime.Now, Sender, Receiver);
        return ObjStructureHeader;
    }

    #endregion

    #endregion
    #region "-- Public --"

    #region "-- Variables / Properties --"

    public class HeaderDetailsTemplate
    {
        public string HeaderId { get; set; }
        public string HeaderDsdName { get; set; }
        public string LanguageCode { get; set; }

        private string _SenderId = "NA";
        /// <summary>
        /// Property to set the SenderId ID
        /// </summary>
        public string SenderId
        {
            get { return _SenderId; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _SenderId = value;
                }
            }
        }

        private string _SenderName = "NA";
        public string SenderName
        {
            get { return _SenderName; }
            set { if (!string.IsNullOrEmpty(value)) { _SenderName = value; } }
        }

        private string _SenderContactName = "NA";
        /// <summary>
        /// Property to set the SenderContactName
        /// </summary>
        public string SenderContactName
        {
            get { return _SenderContactName; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _SenderContactName = value;
                }
            }
        }

        private string _SenderDepartment = "NA";
        /// <summary>
        /// Property to set the SenderDepartment
        /// </summary>
        public string SenderDepartment
        {
            get { return _SenderDepartment; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _SenderDepartment = value;
                }
            }
        }

        private string _SenderRole = "NA";
        /// <summary>
        /// Property to set the SenderRole
        /// </summary>
        public string SenderRole
        {
            get { return _SenderRole; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _SenderRole = value;
                }
            }
        }

        private string _SenderFax = "NA";
        /// <summary>
        /// Property to set the SenderFax
        /// </summary>
        public string SenderFax
        {
            get { return _SenderFax; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _SenderFax = value;
                }
            }
        }

        private string _SenderTelephone = "NA";
        /// <summary>
        /// Property to set the SenderTelephone
        /// </summary>
        public string SenderTelephone
        {
            get { return _SenderTelephone; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _SenderTelephone = value;
                }
            }
        }

        private string _SenderEmail = "NA";
        /// <summary>
        /// Property to set the SenderFax
        /// </summary>
        public string SenderEmail
        {
            get { return _SenderEmail; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _SenderEmail = value;
                }
            }
        }

        private string _RecieverId = "NA";
        /// <summary>
        /// Property to set the RecieverId ID
        /// </summary>
        public string RecieverId
        {
            get { return _RecieverId; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _RecieverId = value;
                }
            }
        }

        private string _RecieverName = "NA";
        public string RecieverName
        {
            get { return _RecieverName; }
            set { if (!string.IsNullOrEmpty(value)) { _RecieverName = value; } }
        }

        private string _RecieverContactName = "NA";
        /// <summary>
        /// Property to set the RecieverContactName
        /// </summary>
        public string RecieverContactName
        {
            get { return _RecieverContactName; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _RecieverContactName = value;
                }
            }
        }

        private string _RecieverDepartment = "NA";
        /// <summary>
        /// Property to set the RecieverDepartment
        /// </summary>
        public string RecieverDepartment
        {
            get { return _RecieverDepartment; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _RecieverDepartment = value;
                }
            }
        }

        private string _RecieverRole = "NA";
        /// <summary>
        /// Property to set the RecieverRole
        /// </summary>
        public string RecieverRole
        {
            get { return _RecieverRole; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _RecieverRole = value;
                }
            }
        }

        private string _RecieverFax = "NA";
        /// <summary>
        /// Property to set the RecieverFax
        /// </summary>
        public string RecieverFax
        {
            get { return _RecieverFax; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _RecieverFax = value;
                }
            }
        }

        private string _RecieverTelephone = "NA";
        /// <summary>
        /// Property to set the RecieverTelephone
        /// </summary>
        public string RecieverTelephone
        {
            get { return _RecieverTelephone; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _RecieverTelephone = value;
                }
            }
        }

        private string _RecieverEmail = "NA";
        /// <summary>
        /// Property to set the RecieverFax
        /// </summary>
        public string RecieverEmail
        {
            get { return _RecieverEmail; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _RecieverEmail = value;
                }
            }
        }



        private string _Prepared = "NA";
        /// <summary>
        /// Property to set the RecieverFax
        /// </summary>
        public string Prepared
        {
            get { return _Prepared; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    _Prepared = value;
                }
            }
        }
    }

    #endregion

    #region "-- Methods --"

    /// <summary>
    /// Generate and save header.xml file based passed input parameters 
    /// </summary>
    /// <param name="requestParam">input parameters for generating header Xml file</param>
    /// <returns>True if header file created successfuly, else return false</returns>
    public bool SaveHeaderDetails(string requestParam)
    {
        Boolean RetVal = false;
        string[] Params;
        string DbNid = string.Empty;
        bool IsDIDatabase = false;
        HeaderDetailsTemplate ObjHeaderDet;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DbNid = Params[0];
            ObjHeaderDet = new HeaderDetailsTemplate();
            ObjHeaderDet.LanguageCode = Params[1];
            IsDIDatabase = Convert.ToBoolean(Params[2]);
            ObjHeaderDet.HeaderId = Params[3];
            ObjHeaderDet.HeaderDsdName = Params[4];
            ObjHeaderDet.SenderId = Params[5];
            ObjHeaderDet.RecieverId = Params[6];
            ObjHeaderDet.SenderName = Params[7];
            ObjHeaderDet.SenderContactName = Params[8];
            ObjHeaderDet.SenderDepartment = Params[9];
            ObjHeaderDet.SenderRole = Params[10];
            ObjHeaderDet.SenderFax = Params[11];
            ObjHeaderDet.SenderTelephone = Params[12];
            ObjHeaderDet.SenderEmail = Params[13];
            ObjHeaderDet.RecieverName = Params[14];
            ObjHeaderDet.RecieverContactName = Params[15];
            ObjHeaderDet.RecieverDepartment = Params[16];
            ObjHeaderDet.RecieverRole = Params[17];
            ObjHeaderDet.RecieverFax = Params[18];
            ObjHeaderDet.RecieverTelephone = Params[19];
            ObjHeaderDet.RecieverEmail = Params[20];
            ObjHeaderDet.Prepared = Params[22];

            // Check if to save header for di adaptation--2.1
            if (IsDIDatabase)
            {
                // Create StructureType object of  SDMXObjectModel--2.1
                SDMXObjectModel.Message.StructureType ObjStructure = new StructureType();
                // Set header field of structure object, by calling method  
                ObjStructure.Header = GetDiMessageHeaderStructure(ObjHeaderDet);
                ObjStructure.Footer = null;
                // Call method to save header xml
                RetVal = SaveDIHeaderStructure(DbNid, ObjStructure);
             
            }
            else// if to save header for unsd--2.0
            {
                // create StructureType object of  SDMXApi_2_0
                SDMXApi_2_0.Message.StructureType ObjSdxmStructure = new SDMXApi_2_0.Message.StructureType();
                // set header field of structure object
                ObjSdxmStructure.Header = GetSdmxMessageHeaderStructure(ObjHeaderDet);
                ObjSdxmStructure.Concepts = null;
                ObjSdxmStructure.KeyFamilies = null;
                ObjSdxmStructure.CodeLists = null;
                ObjSdxmStructure.MetadataStructureDefinitions = null;
                ObjSdxmStructure.OrganisationSchemes = null;
                ObjSdxmStructure.HierarchicalCodelists = null;
                ObjSdxmStructure.Metadataflows = null;
                ObjSdxmStructure.Processes = null;
                ObjSdxmStructure.ReportingTaxonomies = null;
                ObjSdxmStructure.StructureSets = null;
                ObjSdxmStructure.CategorySchemes = null;
                ObjSdxmStructure.Dataflows = null;
                // Call method to save header xml
                RetVal = SaveUNSDHeaderStructure(DbNid, ObjSdxmStructure);
            }
        }
        catch (Exception Ex)
        {
            Global.CreateExceptionString(Ex, null);
            RetVal = false;
        }
        return RetVal;
    }

    /// <summary>
    /// Read Header Xml file based in database NId,
    /// Create HeaderDeatails class object and assign property based on xml file values,
    /// Searialize object to json string using JavaScriptSerializer and return 
    /// </summary>
    /// <param name="requestParam">Containg information to retreve xml file</param>
    /// <returns>string containing header information</returns>
    public string GetHeaderDetail(string requestParam)
    {
        HeaderDetailsTemplate ObjHeaderDetail;
        bool IsDIDatabase = false;
        string DbNid = string.Empty;
        string[] Params;
        string RetVal = string.Empty;
        System.Web.Script.Serialization.JavaScriptSerializer jSearializer = null;
        try
        {
            ObjHeaderDetail = new HeaderDetailsTemplate();
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            DbNid = Params[0];
            IsDIDatabase = Convert.ToBoolean(Params[1]);
            // Check if to getheader for Di Adaptations or UNSD
            if (IsDIDatabase)
            {
                ObjHeaderDetail = GetDIHeaderStructure(DbNid);
            }
            else
            {
                ObjHeaderDetail = GetUNSDHeaderStructure(DbNid);
            }
            // Initlize javascript seailizer
            jSearializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            // Searilize Header deatail and return
            RetVal = jSearializer.Serialize(ObjHeaderDetail);
        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }

    #endregion

    #endregion

}