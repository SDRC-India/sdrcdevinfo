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
using SDMXObjectModel.Common;
using System.IO;
using SDMXObjectModel.Metadata.Generic;
using System.Text.RegularExpressions;

namespace DevInfo.Lib.DI_LibSDMX
{
    internal class MetadataReportUtility
    {
        #region "Properties"

        #region "Private"

        private string _agencyId;

        private string _language;

        private bool _multiLanguageHandlingRequired;

        private Header _header;

        private DIConnection _diConnection;

        private DIQueries _diQueries;

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

        internal bool MultiLanguageHandlingRequired
        {
            get
            {
                return this._multiLanguageHandlingRequired;
            }
            set
            {
                this._multiLanguageHandlingRequired = value;
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

        internal MetadataReportUtility(string agencyId, string language, Header header, DIConnection DIConnection, DIQueries DIQueries)
        {
            this._agencyId = agencyId;
            this._header = header;
            this._diConnection = DIConnection;
            this._diQueries = DIQueries;

            if (string.IsNullOrEmpty(language))
            {
                this._language = this._diQueries.LanguageCode.Substring(1);
                this._multiLanguageHandlingRequired = true;
            }
            else
            {
                if (this._diConnection.IsValidDILanguage(this._diQueries.DataPrefix, language))
                {
                    this._language = language;
                    this._multiLanguageHandlingRequired = false;
                }
                else
                {
                    this._language = this._diQueries.LanguageCode.Substring(1);
                    this._multiLanguageHandlingRequired = false;
                }
            }
        }

        #endregion "Public"

        #endregion "Constructors"

        #region "Methods"

        #region "Private"

        public SDMXObjectModel.Message.GenericMetadataHeaderType Get_Appropriate_Header(MetadataTypes metadataType)
        {
            SDMXObjectModel.Message.GenericMetadataHeaderType RetVal;
            SenderType Sender;
            PartyType Receiver;

            if (this._header == null)
            {
                Sender = new SenderType(Constants.Header.SenderId, Constants.Header.SenderName, Constants.DefaultLanguage, new SDMXObjectModel.Message.ContactType(Constants.Header.Sender, Constants.Header.SenderDepartment, Constants.Header.SenderRole, Constants.DefaultLanguage));
                Sender.Contact[0].Items = new string[] { Constants.Header.SenderTelephone, Constants.Header.SenderEmail, Constants.Header.SenderFax };
                Sender.Contact[0].ItemsElementName = new SDMXObjectModel.Message.ContactChoiceType[] { SDMXObjectModel.Message.ContactChoiceType.Telephone, SDMXObjectModel.Message.ContactChoiceType.Email, SDMXObjectModel.Message.ContactChoiceType.Fax };

                Receiver = new PartyType(Constants.Header.ReceiverId, Constants.Header.ReceiverName, Constants.DefaultLanguage, new SDMXObjectModel.Message.ContactType(Constants.Header.Receiver, Constants.Header.ReceiverDepartment, Constants.Header.ReceiverRole, Constants.DefaultLanguage));
                Receiver.Contact[0].Items = new string[] { Constants.Header.ReceiverTelephone, Constants.Header.ReceiverEmail, Constants.Header.ReceiverFax };
                Receiver.Contact[0].ItemsElementName = new SDMXObjectModel.Message.ContactChoiceType[] { SDMXObjectModel.Message.ContactChoiceType.Telephone, SDMXObjectModel.Message.ContactChoiceType.Email, SDMXObjectModel.Message.ContactChoiceType.Fax };

                RetVal = new GenericMetadataHeaderType(Constants.Header.Id, true, DateTime.Now, Sender, Receiver);
            }
            else
            {
                Sender = new SenderType(this._header.Sender.ID, this._header.Sender.Name, Constants.DefaultLanguage, new SDMXObjectModel.Message.ContactType(this._header.Sender.Contact.Name, this._header.Sender.Contact.Department, this._header.Sender.Contact.Role, Constants.DefaultLanguage));
                Sender.Contact[0].Items = new string[] { this._header.Sender.Contact.Telephone, this._header.Sender.Contact.Email, this._header.Sender.Contact.Fax };
                Sender.Contact[0].ItemsElementName = new SDMXObjectModel.Message.ContactChoiceType[] { SDMXObjectModel.Message.ContactChoiceType.Telephone, SDMXObjectModel.Message.ContactChoiceType.Email, SDMXObjectModel.Message.ContactChoiceType.Fax };

                Receiver = new PartyType(this._header.Receiver.ID, this._header.Receiver.Name, Constants.DefaultLanguage, new SDMXObjectModel.Message.ContactType(this._header.Receiver.Contact.Name, this._header.Receiver.Contact.Department, this._header.Receiver.Contact.Role, Constants.DefaultLanguage));
                Receiver.Contact[0].Items = new string[] { this._header.Receiver.Contact.Telephone, this._header.Receiver.Contact.Email, this._header.Receiver.Contact.Fax };
                Receiver.Contact[0].ItemsElementName = new SDMXObjectModel.Message.ContactChoiceType[] { SDMXObjectModel.Message.ContactChoiceType.Telephone, SDMXObjectModel.Message.ContactChoiceType.Email, SDMXObjectModel.Message.ContactChoiceType.Fax };

                RetVal = new GenericMetadataHeaderType(this._header.ID, true, DateTime.Now, Sender, Receiver);
            }

            RetVal.Structure = new List<GenericMetadataStructureType>();
            RetVal.Structure.Add(new GenericMetadataStructureType());

            switch (metadataType)
            {
                case MetadataTypes.Area:
                    RetVal.Structure[0].structureID = Constants.MSD.Area.Id;
                    RetVal.Structure[0].Item = new MetadataStructureReferenceType();
                    ((MetadataStructureReferenceType)RetVal.Structure[0].Item).Items = new List<object>();
                    ((MetadataStructureReferenceType)RetVal.Structure[0].Item).Items.Add(new MetadataStructureRefType(Constants.MSD.Area.Id, this._agencyId, Constants.MSD.Area.Version));
                    break;
                case MetadataTypes.Indicator:
                    RetVal.Structure[0].structureID = Constants.MSD.Indicator.Id;
                    RetVal.Structure[0].Item = new MetadataStructureReferenceType();
                    ((MetadataStructureReferenceType)RetVal.Structure[0].Item).Items = new List<object>();
                    ((MetadataStructureReferenceType)RetVal.Structure[0].Item).Items.Add(new MetadataStructureRefType(Constants.MSD.Indicator.Id, this._agencyId, Constants.MSD.Indicator.Version));
                    break;
                case MetadataTypes.Source:
                    RetVal.Structure[0].structureID = Constants.MSD.Source.Id;
                    RetVal.Structure[0].Item = new MetadataStructureReferenceType();
                    ((MetadataStructureReferenceType)RetVal.Structure[0].Item).Items = new List<object>();
                    ((MetadataStructureReferenceType)RetVal.Structure[0].Item).Items.Add(new MetadataStructureRefType(Constants.MSD.Source.Id, this._agencyId, Constants.MSD.Source.Version));
                    break;
                case MetadataTypes.Layer:
                    RetVal.Structure[0].structureID = Constants.MSD.Area.Id;
                    RetVal.Structure[0].Item = new MetadataStructureReferenceType();
                    ((MetadataStructureReferenceType)RetVal.Structure[0].Item).Items = new List<object>();
                    ((MetadataStructureReferenceType)RetVal.Structure[0].Item).Items.Add(new MetadataStructureRefType(Constants.MSD.Area.Id, this._agencyId, Constants.MSD.Area.Version));
                    break;
                default:
                    break;
            }

            ((MetadataStructureRefType)((MetadataStructureReferenceType)RetVal.Structure[0].Item).Items[0]).local = false;
            ((MetadataStructureRefType)((MetadataStructureReferenceType)RetVal.Structure[0].Item).Items[0]).localSpecified = true;

            ((MetadataStructureRefType)((MetadataStructureReferenceType)RetVal.Structure[0].Item).Items[0]).@class = ObjectTypeCodelistType.MetadataStructure;
            ((MetadataStructureRefType)((MetadataStructureReferenceType)RetVal.Structure[0].Item).Items[0]).classSpecified = true;

            ((MetadataStructureRefType)((MetadataStructureReferenceType)RetVal.Structure[0].Item).Items[0]).package = PackageTypeCodelistType.metadatastructure;
            ((MetadataStructureRefType)((MetadataStructureReferenceType)RetVal.Structure[0].Item).Items[0]).packageSpecified = true;

            return RetVal;
        }

        private GenericMetadataType Retrieve_Metadata_From_Database(MetadataTypes metadataType, string TargetObjectId,SDMXObjectModel.Message.StructureHeaderType Header)
        {
            GenericMetadataType RetVal;
            string StructureRef, ReportStructureId, MetadataTargetId, IdentifiableObjectTargetId, MaintenableParentId, MaintenableParentVersion;
            ObjectTypeCodelistType TargetObjectType;
            PackageTypeCodelistType TargetPackageType;

            RetVal = null;
            StructureRef = string.Empty;
            ReportStructureId = string.Empty;
            MetadataTargetId = string.Empty;
            IdentifiableObjectTargetId = string.Empty;
            MaintenableParentId = string.Empty;
            MaintenableParentVersion = string.Empty;
            TargetObjectType = ObjectTypeCodelistType.Code;
            TargetPackageType = PackageTypeCodelistType.codelist;

            try
            {
                switch (metadataType)
                {
                    case MetadataTypes.Area:
                        StructureRef = Constants.MSD.Area.Id;
                        ReportStructureId = Constants.MSD.Area.ReportStructureId;
                        MetadataTargetId = Constants.MSD.Area.MetadataTargetId;
                        IdentifiableObjectTargetId = Constants.MSD.Area.IdentifiableObjectTargetId;
                        MaintenableParentId = Constants.CodeList.Area.Id;
                        MaintenableParentVersion = Constants.CodeList.Area.Version;
                        TargetObjectType = ObjectTypeCodelistType.Code;
                        TargetPackageType = PackageTypeCodelistType.codelist;
                        break;
                    case MetadataTypes.Indicator:
                        StructureRef = Constants.MSD.Indicator.Id;
                        ReportStructureId = Constants.MSD.Indicator.ReportStructureId;
                        MetadataTargetId = Constants.MSD.Indicator.MetadataTargetId;
                        IdentifiableObjectTargetId = Constants.MSD.Indicator.IdentifiableObjectTargetId;
                        MaintenableParentId = Constants.CodeList.Indicator.Id;
                        MaintenableParentVersion = Constants.CodeList.Indicator.Version;
                        TargetObjectType = ObjectTypeCodelistType.Code;
                        TargetPackageType = PackageTypeCodelistType.codelist;
                        break;
                    case MetadataTypes.Source:
                        StructureRef = Constants.MSD.Source.Id;
                        ReportStructureId = Constants.MSD.Source.ReportStructureId;
                        MetadataTargetId = Constants.MSD.Source.MetadataTargetId;
                        IdentifiableObjectTargetId = Constants.MSD.Source.IdentifiableObjectTargetId;
                        MaintenableParentId = Constants.CategoryScheme.Source.Id;
                        MaintenableParentVersion = Constants.CategoryScheme.Source.Version;
                        TargetObjectType = ObjectTypeCodelistType.Category;
                        TargetPackageType = PackageTypeCodelistType.categoryscheme;
                        break;
                    case MetadataTypes.Layer:
                        StructureRef = Constants.MSD.Area.Id;
                        ReportStructureId = Constants.MSD.Area.ReportStructureId;
                        MetadataTargetId = Constants.MSD.Area.MetadataTargetId;
                        IdentifiableObjectTargetId = Constants.MSD.Area.IdentifiableObjectTargetId;
                        MaintenableParentId = Constants.CodeList.Area.Id;
                        MaintenableParentVersion = Constants.CodeList.Area.Version;
                        TargetObjectType = ObjectTypeCodelistType.Code;
                        TargetPackageType = PackageTypeCodelistType.codelist;
                        break;
                    default:
                        break;
                }

                RetVal = new GenericMetadataType();
                if (Header != null)
                {
                    RetVal.Header.ID = Header.ID;
                    RetVal.Header.Name = Header.Name;
                    RetVal.Header.Receiver = Header.Receiver;
                    RetVal.Header.Sender = Header.Sender;
                    RetVal.Header.Test = Header.Test;
                    RetVal.Header.Source = Header.Source;
                    RetVal.Header.Prepared = Header.Prepared;

                }
                else
                {
                    RetVal.Header = this.Get_Appropriate_Header(metadataType);
                }
                RetVal.Footer = null;

                RetVal.MetadataSet = new List<SDMXObjectModel.Metadata.Generic.MetadataSetType>();
                RetVal.MetadataSet.Add(new SDMXObjectModel.Metadata.Generic.MetadataSetType());
                RetVal.MetadataSet[0].structureRef = StructureRef;
                RetVal.MetadataSet[0].Annotations = null;
                RetVal.MetadataSet[0].DataProvider = null;

                RetVal.MetadataSet[0].Report = new List<SDMXObjectModel.Metadata.Generic.ReportType>();
                RetVal.MetadataSet[0].Report.Add(new SDMXObjectModel.Metadata.Generic.ReportType());
                RetVal.MetadataSet[0].Report[0].id = ReportStructureId;
                RetVal.MetadataSet[0].Report[0].Annotations = null;

                RetVal.MetadataSet[0].Report[0].Target = new SDMXObjectModel.Metadata.Generic.TargetType();
                RetVal.MetadataSet[0].Report[0].Target.id = MetadataTargetId;

                RetVal.MetadataSet[0].Report[0].Target.ReferenceValue = new List<SDMXObjectModel.Metadata.Generic.ReferenceValueType>();
                RetVal.MetadataSet[0].Report[0].Target.ReferenceValue.Add(new SDMXObjectModel.Metadata.Generic.ReferenceValueType());
                RetVal.MetadataSet[0].Report[0].Target.ReferenceValue[0].id = IdentifiableObjectTargetId;

                RetVal.MetadataSet[0].Report[0].Target.ReferenceValue[0].Item = new ObjectReferenceType();
                ((ObjectReferenceType)RetVal.MetadataSet[0].Report[0].Target.ReferenceValue[0].Item).Items = new List<object>();
                ((ObjectReferenceType)RetVal.MetadataSet[0].Report[0].Target.ReferenceValue[0].Item).Items.Add(new ObjectRefType(TargetObjectId, this._agencyId, null, MaintenableParentId, MaintenableParentVersion, TargetObjectType, true, TargetPackageType, true));

                RetVal.MetadataSet[0].Report[0].AttributeSet = this.Get_AttributeSet(metadataType);

                if (RetVal.MetadataSet[0].Report[0].AttributeSet != null && RetVal.MetadataSet[0].Report[0].AttributeSet.ReportedAttribute != null && RetVal.MetadataSet[0].Report[0].AttributeSet.ReportedAttribute.Count > 0)
                {
                    this.Fill_AttributeSet_Values(RetVal.MetadataSet[0].Report[0].AttributeSet, TargetObjectId, metadataType);
                }
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

        private AttributeSetType Get_AttributeSet(MetadataTypes metadataTypes)
        {
            AttributeSetType RetVal;
            DataTable DtMetadataCategory;
            string Query;

            RetVal = null;
            DtMetadataCategory = null;
            Query = string.Empty;

            try
            {
                switch (metadataTypes)
                {
                    case MetadataTypes.Area:
                        Query = "SELECT * FROM UT_Metadata_Category_" + this._language + " WHERE CategoryType = 'A' ORDER BY CategoryOrder ASC;";
                        break;
                    case MetadataTypes.Indicator:
                        Query = "SELECT * FROM UT_Metadata_Category_" + this._language + " WHERE CategoryType = 'I' ORDER BY CategoryOrder ASC;";
                        break;
                    case MetadataTypes.Source:
                        Query = "SELECT * FROM UT_Metadata_Category_" + this._language + " WHERE CategoryType = 'S' ORDER BY CategoryOrder ASC;";
                        break;
                    case MetadataTypes.Layer:
                        Query = "SELECT * FROM UT_Metadata_Category_" + this._language + " WHERE CategoryType = 'A' ORDER BY CategoryOrder ASC;";
                        break;
                    default:
                        break;
                }

                DtMetadataCategory = this._diConnection.ExecuteDataTable(Regex.Replace(Query, "UT_", this._diConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));

                if (DtMetadataCategory != null && DtMetadataCategory.Rows.Count > 0)
                {
                    RetVal = new AttributeSetType();
                    this.Fill_AttributeSet_Structure(RetVal, DtMetadataCategory);
                }
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

        private void Fill_AttributeSet_Structure(AttributeSetType AttributeSet, DataTable DtMetadataCategory)
        {
            ReportedAttributeType ReportedAttribute;
            DataRow[] ParentRows;
            string CategoryNId, CategoryGId;

            AttributeSet.ReportedAttribute = new List<ReportedAttributeType>();
            ParentRows = DtMetadataCategory.Select("ParentCategoryNId = -1", "CategoryOrder ASC");

            foreach (DataRow ParentRow in ParentRows)
            {
                CategoryNId = ParentRow["CategoryNId"].ToString();
                CategoryGId = ParentRow["CategoryGId"].ToString();

                ReportedAttribute = new ReportedAttributeType();
                ReportedAttribute.id = CategoryGId;
                ReportedAttribute.Annotations = null;

                this.Add_Children_Attributes(ReportedAttribute, CategoryNId, DtMetadataCategory);
                AttributeSet.ReportedAttribute.Add(ReportedAttribute);
            }
        }

        private void Add_Children_Attributes(ReportedAttributeType ReportedAttribute, string CategoryNId, DataTable DtMetadataCategory)
        {
            ReportedAttributeType ChildReportedAttribute;
            DataRow[] ChildRows;
            string ChildCategoryNId, ChildCategoryGId;

            ChildRows = DtMetadataCategory.Select("ParentCategoryNId = " + CategoryNId, "CategoryOrder ASC");

            if (ChildRows.Length > 0)
            {
                ReportedAttribute.AttributeSet = new AttributeSetType();
                ReportedAttribute.AttributeSet.ReportedAttribute = new List<ReportedAttributeType>();

                foreach (DataRow ChildRow in ChildRows)
                {
                    ChildCategoryNId = ChildRow["CategoryNId"].ToString();
                    ChildCategoryGId = ChildRow["CategoryGId"].ToString();

                    ChildReportedAttribute = new ReportedAttributeType();
                    ChildReportedAttribute.id = ChildCategoryGId;
                    ChildReportedAttribute.Annotations = null;

                    this.Add_Children_Attributes(ChildReportedAttribute, ChildCategoryNId, DtMetadataCategory);
                    ReportedAttribute.AttributeSet.ReportedAttribute.Add(ChildReportedAttribute);
                }
            }
            else
            {
                ReportedAttribute.AttributeSet = null;
            }
        }

        private string Get_TargetNId(string TargetObjectId, MetadataTypes metadataTypes, string Language)
        {
            string RetVal;
            string Query, AreaNId;
            DataTable DtTable;

            RetVal = string.Empty;
            Query = string.Empty;
            AreaNId = string.Empty;
            DtTable = null;

            try
            {
                switch (metadataTypes)
                {
                    case MetadataTypes.Area:
                        Query = "SELECT Area_NId FROM UT_Area_" + Language + " WHERE Area_ID = '" + TargetObjectId + "'";
                        DtTable = this._diConnection.ExecuteDataTable(Regex.Replace(Query, "UT_", this._diConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));

                        if (DtTable != null && DtTable.Rows.Count > 0)
                        {
                            AreaNId = DtTable.Rows[0]["Area_NId"].ToString();
                            Query = "SELECT Layer_NId FROM UT_Area_Map WHERE Area_NId = " + AreaNId.ToString();
                            DtTable = this._diConnection.ExecuteDataTable(Regex.Replace(Query, "UT_", this._diConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));

                            if (DtTable != null && DtTable.Rows.Count > 0)
                            {
                                RetVal = DtTable.Rows[0]["Layer_NId"].ToString();
                            }
                        }
                        break;
                    case MetadataTypes.Indicator:
                        Query = "SELECT Indicator_NId FROM UT_Indicator_" + Language + " WHERE Indicator_GId = '" + TargetObjectId + "';";
                        DtTable = this._diConnection.ExecuteDataTable(Regex.Replace(Query, "UT_", this._diConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));

                        if (DtTable != null && DtTable.Rows.Count > 0)
                        {
                            RetVal = DtTable.Rows[0]["Indicator_NId"].ToString();
                        }
                        break;
                    case MetadataTypes.Source:
                        Query = "SELECT IC_NId FROM UT_Indicator_Classifications_" + Language + " WHERE IC_GId = '" + TargetObjectId + "' AND IC_Type = 'SR';";
                        DtTable = this._diConnection.ExecuteDataTable(Regex.Replace(Query, "UT_", this._diConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));

                        if (DtTable != null && DtTable.Rows.Count > 0)
                        {
                            RetVal = DtTable.Rows[0]["IC_NId"].ToString();
                        }
                        break;
                    case MetadataTypes.Layer:
                        RetVal = TargetObjectId;
                        break;
                    default:
                        break;
                }
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

        private void Fill_AttributeSet_Values(AttributeSetType AttributeSet, string TargetObjectId, MetadataTypes metadataType)
        {
            string Language;

            Language = string.Empty;

            try
            {
                this.Fill_AttributeSet_Language_Specific_Values(AttributeSet, TargetObjectId, metadataType, this._language);

                if (this._multiLanguageHandlingRequired == true)
                {
                    foreach (DataRow LanguageRow in this.DIConnection.DILanguages(this.DIQueries.DataPrefix).Rows)
                    {
                        Language = LanguageRow[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Language.LanguageCode].ToString();

                        if (Language != this.DIQueries.LanguageCode.Substring(1))
                        {
                            this.Fill_AttributeSet_Language_Specific_Values(AttributeSet, TargetObjectId, metadataType, Language);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        private void Fill_AttributeSet_Language_Specific_Values(AttributeSetType AttributeSet, string TargetObjectId, MetadataTypes metadataType, string Language)
        {
            string Query;
            string TargetNId;
            
            List<string> CategoryGIds, CategoryNIds;
            Dictionary<string, string> DictCategoryNIdGId, DictCategoryGIdValue,DictCategoryPresentational;
            DataTable DtMetadataCategory, DtMetadataReport;

            Query = string.Empty;
            TargetNId = string.Empty;
            CategoryGIds = null;
            CategoryNIds = null;
            DictCategoryNIdGId = null;
            DictCategoryGIdValue = null;
            DtMetadataCategory = null;
            DtMetadataReport = null;

            try
            {
                TargetNId = this.Get_TargetNId(TargetObjectId, metadataType, Language);

                if (!string.IsNullOrEmpty(TargetNId))
                {
                    CategoryGIds = new List<string>();
                    CategoryNIds = new List<string>();
                    DictCategoryNIdGId = new Dictionary<string, string>();
                    DictCategoryGIdValue = new Dictionary<string, string>();
                    DictCategoryPresentational = new Dictionary<string, string>();
                    foreach (ReportedAttributeType ReportedAttribute in AttributeSet.ReportedAttribute)
                    {
                        CategoryGIds.Add(ReportedAttribute.id);
                        CategoryGIds.AddRange(this.Get_ChildAttribute_CategoryGIds(ReportedAttribute));
                    }

                    Query = "SELECT * FROM UT_Metadata_Category_" + Language;

                    if (CategoryGIds.Count > 0)
                    {
                        Query += " WHERE CategoryGId IN ('" + string.Join("','", CategoryGIds.ToArray()) + "');";
                    }

                    DtMetadataCategory = this._diConnection.ExecuteDataTable(Regex.Replace(Query, "UT_", this._diConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));

                    foreach (DataRow DrMetadataCategory in DtMetadataCategory.Rows)
                    {
                        CategoryNIds.Add(DrMetadataCategory["CategoryNId"].ToString());
                        DictCategoryNIdGId.Add(DrMetadataCategory["CategoryNId"].ToString(), DrMetadataCategory["CategoryGId"].ToString());
                        if (DrMetadataCategory["IsPresentational"].ToString() == "True")
                        {
                            DictCategoryPresentational.Add(DrMetadataCategory["CategoryNId"].ToString(), DrMetadataCategory["CategoryGId"].ToString());
                        }
                    }

                    Query = "SELECT * FROM UT_MetadataReport_" + Language + " WHERE Target_NId = " + TargetNId;

                    if (CategoryNIds.Count > 0)
                    {
                       Query += " AND Category_NId IN (" + string.Join(",", CategoryNIds.ToArray()) + ");";//commented to add those rows which doesnt have metadata for metadata category
                    }

                    DtMetadataReport = this._diConnection.ExecuteDataTable(Regex.Replace(Query, "UT_", this._diConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));

                    foreach (DataRow DrMetadataReport in DtMetadataReport.Rows)
                    {
                        
                          DictCategoryGIdValue.Add(DictCategoryNIdGId[DrMetadataReport["Category_NId"].ToString()], DrMetadataReport["Metadata"].ToString());
                          
                    }

                    foreach (ReportedAttributeType ReportedAttribute in AttributeSet.ReportedAttribute)
                    {
                        if (ReportedAttribute.Items == null)
                        {
                            ReportedAttribute.Items = new List<object>();
                        }
                        else if (ReportedAttribute.Items.Count == 0)
                        {
                            ReportedAttribute.Items = new List<object>();
                        }

                        if (DictCategoryGIdValue.ContainsKey(ReportedAttribute.id))
                        {
                            ReportedAttribute.Items.Add(new TextType(Language, DictCategoryGIdValue[ReportedAttribute.id]));
                        }
                        else if (DictCategoryPresentational.ContainsValue(ReportedAttribute.id)==false && DictCategoryGIdValue.ContainsKey(ReportedAttribute.id) == false && DictCategoryPresentational.Count>0)
                        {
                            ReportedAttribute.Items.Add(new TextType(Language, string.Empty));//to add empty tag where metadata category doesnt have any description or report 
                        }


                        this.Fill_Children_Attribute_Values(ReportedAttribute, DictCategoryGIdValue, Language);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        private void Fill_Children_Attribute_Values(ReportedAttributeType ReportedAttribute, Dictionary<string, string> DictCategoryGIdValue, string Language)
        {
            if (ReportedAttribute.AttributeSet != null && ReportedAttribute.AttributeSet.ReportedAttribute != null && ReportedAttribute.AttributeSet.ReportedAttribute.Count > 0)
            {
                foreach (ReportedAttributeType ChildReportedAttribute in ReportedAttribute.AttributeSet.ReportedAttribute)
                {
                    if (ChildReportedAttribute.Items == null)
                    {
                        ChildReportedAttribute.Items = new List<object>();
                    }
                    else if (ChildReportedAttribute.Items.Count == 0)
                    {
                        ChildReportedAttribute.Items = new List<object>();
                    }

                    if (DictCategoryGIdValue.ContainsKey(ChildReportedAttribute.id))
                    {
                        ChildReportedAttribute.Items.Add(new TextType(Language, DictCategoryGIdValue[ChildReportedAttribute.id]));
                    }

                    this.Fill_Children_Attribute_Values(ChildReportedAttribute, DictCategoryGIdValue, Language);
                }
            }
        }

        private List<string> Get_ChildAttribute_CategoryGIds(ReportedAttributeType ReportedAttribute)
        {
            List<string> RetVal;

            RetVal = new List<string>();

            if (ReportedAttribute.AttributeSet != null && ReportedAttribute.AttributeSet.ReportedAttribute != null && ReportedAttribute.AttributeSet.ReportedAttribute.Count > 0)
            {
                foreach (ReportedAttributeType ChildReportedAttribute in ReportedAttribute.AttributeSet.ReportedAttribute)
                {
                    RetVal.Add(ChildReportedAttribute.id);
                    RetVal.AddRange(this.Get_ChildAttribute_CategoryGIds(ChildReportedAttribute));
                }
            }
            else
            {
                RetVal = new List<string>();
            }

            return RetVal;
        }

        #endregion "Private"

        #region "Public"

        internal XmlDocument Get_MetadataReport(XmlDocument query)
        {
            XmlDocument RetVal;
            SDMXObjectModel.Message.MetadataQueryType MetadataQuery;
            SDMXObjectModel.Message.GenericMetadataType Metadata;
            string TargetObjectId, TargetObjectParentId;

            RetVal = null;
            MetadataQuery = null;
            Metadata = null;
            TargetObjectId = string.Empty;
            TargetObjectParentId = string.Empty;

            try
            {
                try
                {
                    MetadataQuery = (SDMXObjectModel.Message.MetadataQueryType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.MetadataQueryType), query);
                }
                catch
                {
                    throw new Exception(Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                }

                if (MetadataQuery.Query.MetadataParameters.AttachedObject.Count > 0)
                {
                    if (MetadataQuery.Query.MetadataParameters.AttachedObject[0].Items.Count > 0)
                    {
                        TargetObjectId = ((SDMXObjectModel.Common.ObjectRefType)MetadataQuery.Query.MetadataParameters.AttachedObject[0].Items[0]).id;
                        TargetObjectParentId = ((SDMXObjectModel.Common.ObjectRefType)MetadataQuery.Query.MetadataParameters.AttachedObject[0].Items[0]).maintainableParentID;
                        this._agencyId = ((SDMXObjectModel.Common.ObjectRefType)MetadataQuery.Query.MetadataParameters.AttachedObject[0].Items[0]).agencyID;
                    }
                    else
                    {
                        throw new Exception(Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                    }
                }
                else
                {
                    throw new Exception(Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                }

                if (!string.IsNullOrEmpty(TargetObjectId) && !string.IsNullOrEmpty(TargetObjectParentId))
                {
                    switch (TargetObjectParentId)
                    {
                        case Constants.CodeList.Area.Id:
                            Metadata = this.Retrieve_Metadata_From_Database(MetadataTypes.Area, TargetObjectId,null);
                            break;
                        case Constants.CodeList.Indicator.Id:
                            Metadata = this.Retrieve_Metadata_From_Database(MetadataTypes.Indicator, TargetObjectId,null);
                            break;
                        case Constants.CategoryScheme.Source.Id:
                            Metadata = this.Retrieve_Metadata_From_Database(MetadataTypes.Source, TargetObjectId,null);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    throw new Exception(Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                }

                if (Metadata != null)
                {
                    RetVal = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.GenericMetadataType), Metadata);
                }
                else
                {
                    throw new Exception(Constants.SDMXWebServices.Exceptions.NoResults.Message);
                }
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

        internal XmlDocument Get_MetadataReport(string TargetObjectId, MetadataTypes metadataType)
        {
            XmlDocument RetVal;
            SDMXObjectModel.Message.GenericMetadataType Metadata;

            RetVal = null;
            Metadata = null;

            try
            {
                if (!string.IsNullOrEmpty(TargetObjectId))
                {
                    Metadata = this.Retrieve_Metadata_From_Database(metadataType, TargetObjectId,null);
                }

                if (Metadata != null)
                {
                    RetVal = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.GenericMetadataType), Metadata);
                }
                else
                {
                    RetVal = new XmlDocument();
                }
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

        internal bool Generate_MetadataReport(MetadataTypes metadataType, string filterNIds, string outputFolder, out List<string> GeneratedMetadataFiles, SDMXObjectModel.Message.StructureHeaderType Header)
        {
            bool RetVal;
            string Query, ColumnName, TargetObjectId, FileName;
            GenericMetadataType Metadata;
            DataTable DtTable;

            RetVal = false;
            Query = string.Empty;
            ColumnName = string.Empty;
            TargetObjectId = string.Empty;
            FileName = string.Empty;
            Metadata = null;
            DtTable = null;
            GeneratedMetadataFiles = new List<string>();
            DateTime CurrentTime = DateTime.Now;
            try
            {
                switch (metadataType)
                {
                    case MetadataTypes.Area:
                        Query = "SELECT Area_ID FROM UT_Area_" + this._language;
                        
                        if (!string.IsNullOrEmpty(filterNIds))
                        {
                            Query += " WHERE Area_NId IN (" + filterNIds + ");";
                        }

                        ColumnName = "Area_ID";
                        break;
                    case MetadataTypes.Indicator:
                        Query = "SELECT Indicator_GId FROM UT_Indicator_" + this._language;

                        if (!string.IsNullOrEmpty(filterNIds))
                        {
                            Query += " WHERE Indicator_NId IN (" + filterNIds + ");";
                        }

                        ColumnName = "Indicator_GId";
                        break;
                    case MetadataTypes.Source:
                        Query = "SELECT IC_GId FROM UT_Indicator_Classifications_" + this._language + " WHERE IC_Type = 'SR'";

                        if (!string.IsNullOrEmpty(filterNIds))
                        {
                            Query += " AND IC_NId IN (" + filterNIds + ");";
                        }

                        ColumnName = "IC_GId";
                        break;
                    default:
                        break;
                }

                DtTable = this._diConnection.ExecuteDataTable(Regex.Replace(Query, "UT_", this._diConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));

                foreach (DataRow DrTable in DtTable.Rows)
                {
                    TargetObjectId = DrTable[ColumnName].ToString();
                    Metadata = this.Retrieve_Metadata_From_Database(metadataType, TargetObjectId, Header);

                    if (Metadata != null)
                    {
                        FileName = Path.Combine(outputFolder, TargetObjectId + "_" + CurrentTime.ToString("yyyy-MM-dd HHmmss") + Constants.XmlExtension);
                        Serializer.SerializeToFile(typeof(SDMXObjectModel.Message.GenericMetadataType), Metadata, FileName);
                        GeneratedMetadataFiles.Add(FileName);
                    }
                }

                RetVal = true;
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


        internal bool Generate_MetadataReport(MetadataTypes metadataType, string filterNIds, string outputFolder)
        {
            bool RetVal;
            string Query, ColumnName, TargetObjectId, FileName;
            GenericMetadataType Metadata;
            DataTable DtTable;

            RetVal = false;
            Query = string.Empty;
            ColumnName = string.Empty;
            TargetObjectId = string.Empty;
            FileName = string.Empty;
            Metadata = null;
            DtTable = null;
         
            try
            {
                switch (metadataType)
                {
                    case MetadataTypes.Area:
                        Query = "SELECT Area_ID FROM UT_Area_" + this._language;

                        if (!string.IsNullOrEmpty(filterNIds))
                        {
                            Query += " WHERE Area_NId IN (" + filterNIds + ");";
                        }

                        ColumnName = "Area_ID";
                        break;
                    case MetadataTypes.Indicator:
                        Query = "SELECT Indicator_GId FROM UT_Indicator_" + this._language;

                        if (!string.IsNullOrEmpty(filterNIds))
                        {
                            Query += " WHERE Indicator_NId IN (" + filterNIds + ");";
                        }

                        ColumnName = "Indicator_GId";
                        break;
                    case MetadataTypes.Source:
                        Query = "SELECT IC_GId FROM UT_Indicator_Classifications_" + this._language + " WHERE IC_Type = 'SR'";

                        if (!string.IsNullOrEmpty(filterNIds))
                        {
                            Query += " AND IC_NId IN (" + filterNIds + ");";
                        }

                        ColumnName = "IC_GId";
                        break;
                    default:
                        break;
                }

                DtTable = this._diConnection.ExecuteDataTable(Regex.Replace(Query, "UT_", this._diConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));

                foreach (DataRow DrTable in DtTable.Rows)
                {
                    TargetObjectId = DrTable[ColumnName].ToString();
                    Metadata = this.Retrieve_Metadata_From_Database(metadataType, TargetObjectId,null);

                    if (Metadata != null)
                    {
                        FileName = Path.Combine(outputFolder, TargetObjectId + Constants.XmlExtension);
                        Serializer.SerializeToFile(typeof(SDMXObjectModel.Message.GenericMetadataType), Metadata, FileName);
                    }
                }

                RetVal = true;
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
