using System;
using System.Collections.Generic;
using System.Text;
using SDMXObjectModel.Message;
using System.Xml;
using System.Xml.Schema;
using SDMXObjectModel;
using System.IO;
using SDMXObjectModel.Common;
using SDMXObjectModel.Structure;

namespace DevInfo.Lib.DI_LibSDMX
{
    internal class MetadataValidateUtility : BaseValidateUtility
    {
        internal MetadataValidateUtility()
            : base()
        {
        }

        # region internal Methods

        internal override Dictionary<string, string> ValidateMetadataReportAgainstMSD(string MetadataFileNameWPath, string completeFileNameWPath, string MFDId)
        {
            Dictionary<string, string> RetVal;
            GenericMetadataType Metadata;

            //1. Loading Metadata object from MetadataFile on successful validation against SDMX schemas.
            RetVal = Validate_And_Load_Metadatafile(MetadataFileNameWPath, out Metadata);
            if (RetVal.Keys.Count == 0 && Metadata != null && !string.IsNullOrEmpty(completeFileNameWPath))
            {
                //2. Validating the loaded Metadata object against the msd file. 
                RetVal = Validate_MSD_Compliance(Metadata, completeFileNameWPath, MFDId);
            }

            return RetVal;
        }

        # endregion internal Methods


        # region Private Methods

        private Dictionary<string, string> Validate_And_Load_Metadatafile(string MetadataFileNameWPath, out GenericMetadataType Metadata)
        {
            Dictionary<string, string> RetVal;
            XmlDocument Document;

            RetVal = new Dictionary<string, string>();
            Metadata = null;
            Document = null;

            try
            {
                // 1. Validating the Metadata file:
                RetVal = Validate_XML(MetadataFileNameWPath, out Document);

                if (RetVal.Keys.Count == 0)
                {
                    RetVal = Validate_SDMX(Document.InnerXml);
                }

                // 2. Loading the Metadata file in the GenericMetadataType object on successful validation against SDMX schemas.

                if (RetVal.Keys.Count == 0)
                {
                    Metadata = (GenericMetadataType)(Deserializer.LoadFromText(typeof(GenericMetadataType), Document.InnerXml));
                }

            }
            catch (Exception ex)
            {
                // An exception while loading the Metadata file in a GenericMetadataType object implies invalid SDMX structure.
                if (ex is System.InvalidOperationException)
                {
                    RetVal.Add(MetadataValidationStatus.SDMX_Invalid.ToString(), ex.InnerException.Message.ToString());
                }
                // All other exceptions e.g System.IO exceptions are thrown to be handled by using code.
                else
                {
                    throw ex;
                }
            }

            return RetVal;

        }

        private Dictionary<string, string> Validate_SDMX(string XmlContent)
        {
            Dictionary<string, string> RetVal;
            XmlReader Reader;
            XmlReaderSettings ReaderSettings;

            RetVal = new Dictionary<string, string>();
            Reader = null;
            ReaderSettings = new XmlReaderSettings();
            try
            {

                // 2. Loading the SDMX schemas for validation of Metadata file.
                Load_SDMX_Schemas(ReaderSettings.Schemas);

                // 3. Setting the validation flags.
                ReaderSettings.ValidationType = ValidationType.Schema;
                ReaderSettings.ValidationFlags = XmlSchemaValidationFlags.AllowXmlAttributes | XmlSchemaValidationFlags.None;

                // 4. Creating an XmlReader object to read and validate the Metadata file against the schemas loaded.
                Reader = XmlReader.Create(new StringReader(XmlContent), ReaderSettings);

                // 5. Reading and validating the Metadata file line by line.
                while (Reader.Read())
                {
                }

            }
            catch (Exception ex)
            {
                // An XmlSchemaValidationException while reading the Metadata file implies invalid SDMX structure.
                if (ex is System.Xml.Schema.XmlSchemaValidationException)
                {
                    RetVal.Add(MetadataValidationStatus.SDMX_Invalid.ToString(), ex.Message.ToString());
                }
                // All other exceptions e.g System.IO exceptions are thrown to be handled by using code.
                else
                {
                    throw ex;
                }
            }

            return RetVal;
        }

        private Dictionary<string, string> Validate_XML(string dataFileNameWPath, out XmlDocument Document)
        {
            Dictionary<string, string> RetVal;

            RetVal = new Dictionary<string, string>();
            Document = new XmlDocument();

            try
            {
                // 1. Loading the Metadata file in an XmlDocument object - an exception implies invalid XML structure.
                Document.Load(dataFileNameWPath);
            }
            catch (Exception ex)
            {
                // An XML exception implies invalid xml structure.
                if ((ex.InnerException is System.Xml.XmlException) || (ex is System.Xml.XmlException))
                {
                    RetVal.Add(MetadataValidationStatus.Xml_Invalid.ToString(), ex.Message.ToString());
                }
                // All other exceptions e.g System.IO exceptions are thrown to be handled by using code.
                else
                {
                    throw ex;
                }
            }

            return RetVal;
        }

        private void Load_SDMX_Schemas(XmlSchemaSet Schemas)
        {
            Schemas.Add(null, XmlReader.Create(new StringReader(SDMXObjectModel.Schemas.SDMXCommon)));
            Schemas.Add(null, XmlReader.Create(new StringReader(SDMXObjectModel.Schemas.SDMXDataGeneric)));
            Schemas.Add(null, XmlReader.Create(new StringReader(SDMXObjectModel.Schemas.SDMXDataStructureSpecific)));
            Schemas.Add(null, XmlReader.Create(new StringReader(SDMXObjectModel.Schemas.SDMXMessage)));
            Schemas.Add(null, XmlReader.Create(new StringReader(SDMXObjectModel.Schemas.SDMXMessageFooter)));
            Schemas.Add(null, XmlReader.Create(new StringReader(SDMXObjectModel.Schemas.SDMXMetadataGeneric)));
            Schemas.Add(null, XmlReader.Create(new StringReader(SDMXObjectModel.Schemas.SDMXMetadataStructureSpecific)));
            Schemas.Add(null, XmlReader.Create(new StringReader(SDMXObjectModel.Schemas.SDMXQuery)));
            Schemas.Add(null, XmlReader.Create(new StringReader(SDMXObjectModel.Schemas.SDMXRegistry)));
            Schemas.Add(null, XmlReader.Create(new StringReader(SDMXObjectModel.Schemas.SDMXStructure)));
            Schemas.Add(null, XmlReader.Create(new StringReader(SDMXObjectModel.Schemas.xml)));
            Schemas.Add(null, XmlReader.Create(new StringReader(SDMXObjectModel.Schemas.XMLSchema)));

        }

        private Dictionary<string, string> Validate_MSD_Compliance(GenericMetadataType Metadata, string completeFileNameWPath, string MFDId)
        {
            Dictionary<string, string> RetVal;
            SDMXObjectModel.Message.StructureType CompleteStructure;
            int i;
            RetVal = new Dictionary<string, string>();

            try
            {
                // 1. Loading the Complete file in a SDMXApi's CompleteStructure object.

                CompleteStructure = (SDMXObjectModel.Message.StructureType)(Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), completeFileNameWPath));
                if(!(string.IsNullOrEmpty(MFDId)))
                {
                     RetVal = MFD_Validation(Metadata, CompleteStructure, MFDId);
                }
                if (RetVal.Keys.Count == 0)
                {
                    RetVal = Referred_MSD_Validation(Metadata, CompleteStructure);
                }
                if (RetVal.Keys.Count == 0)
                {
                  
                    for (i = 0; i < CompleteStructure.Structures.MetadataStructures.Count; i++)
                    {
                        if (((MetadataStructureRefType)((MetadataStructureReferenceType)(Metadata.Header.Structure[0].Item)).Items[0]).id == CompleteStructure.Structures.MetadataStructures[i].id)
                        {
                            RetVal = Target_Validation(Metadata, CompleteStructure.Structures.MetadataStructures[i]);
                            if (RetVal.Keys.Count == 0)
                            {
                                RetVal = Report_Structure_Validation(Metadata, CompleteStructure.Structures.MetadataStructures[i]);
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
          
        }

        private Dictionary<string, string> MFD_Validation(GenericMetadataType Metadata, SDMXObjectModel.Message.StructureType CompleteStructure, string MFDId)
        {
            Dictionary<string, string> RetVal;
            int i;
            string MSDIdForMFD;
            RetVal = new Dictionary<string, string>();
            MSDIdForMFD = string.Empty;
          
            try
            {
                for (i = 0; i < CompleteStructure.Structures.Metadataflows.Count; i++)
                {
                    if (MFDId == CompleteStructure.Structures.Metadataflows[i].id)
                    {
                        MSDIdForMFD = ((MetadataStructureRefType)(CompleteStructure.Structures.Metadataflows[i].Structure.Items[0])).id;
                        break;
                    }
                }
                if (((MetadataStructureRefType)((MetadataStructureReferenceType)(Metadata.Header.Structure[0].Item)).Items[0]).id != MSDIdForMFD)
                {
                    RetVal.Add(MetadataValidationStatus.MFD_Selection_Invalid.ToString(), Constants.ValidationMessages.Invalid_MFD_Selected);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RetVal;
        }

        private Dictionary<string, string> Referred_MSD_Validation(GenericMetadataType Metadata, SDMXObjectModel.Message.StructureType CompleteStructure)
        {
            Dictionary<string, string> RetVal;
            List<string> ListOfMSDIds;
            RetVal = new Dictionary<string, string>();
            ListOfMSDIds=new List<string>();
            try
            {
                foreach(SDMXObjectModel.Structure.MetadataStructureType MSD in  CompleteStructure.Structures.MetadataStructures)
                {
                    ListOfMSDIds.Add(MSD.id);
                }
                if (!(ListOfMSDIds.Contains(((MetadataStructureRefType)((MetadataStructureReferenceType)(Metadata.Header.Structure[0].Item)).Items[0]).id)))
                {
                    RetVal.Add(MetadataValidationStatus.Referred_MSD_Invalid.ToString(), Constants.ValidationMessages.Invalid_Referred_MSD_Id + ((MetadataStructureRefType)((MetadataStructureReferenceType)(Metadata.Header.Structure[0].Item)).Items[0]).id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RetVal;
        }

        private Dictionary<string, string> Target_Validation(GenericMetadataType Metadata, SDMXObjectModel.Structure.MetadataStructureType MSD)
        {
            Dictionary<string, string> RetVal;
            SDMXObjectModel.Structure.IdentifiableObjectTargetType IdentifiableObjectTarget;
            SDMXObjectModel.Structure.IdentifiableObjectRepresentationType LocalRepresentation;
            SDMXObjectModel.Common.ItemSchemeReferenceType Enumeration;
            SDMXObjectModel.Structure.MetadataTargetType MetadataTarget;
            SDMXObjectModel.Structure.ReportStructureType ReportStructure;
            SDMXObjectModel.Common.ObjectRefType ObjectRef;

            RetVal = new Dictionary<string, string>();
            try
            {
                MetadataTarget = new MetadataTargetType();
                MetadataTarget = (MetadataTargetType)(((SDMXObjectModel.Structure.MetadataStructureComponentsType)MSD.Item).Items[0]);
                ReportStructure = new SDMXObjectModel.Structure.ReportStructureType();
                ReportStructure = (SDMXObjectModel.Structure.ReportStructureType)(((SDMXObjectModel.Structure.MetadataStructureComponentsType)MSD.Item).Items[1]);
                IdentifiableObjectTarget = ((SDMXObjectModel.Structure.IdentifiableObjectTargetType)(MetadataTarget.Items[0]));
                LocalRepresentation = ((SDMXObjectModel.Structure.IdentifiableObjectRepresentationType)(IdentifiableObjectTarget.LocalRepresentation));
                Enumeration = ((SDMXObjectModel.Common.ItemSchemeReferenceType)(LocalRepresentation.Items[0]));


                if (Metadata.MetadataSet[0].Report[0].id == ReportStructure.id)
                {
                    if (Metadata.MetadataSet[0].Report[0].Target.id == MetadataTarget.id)
                    {
                        ObjectRef = new ObjectRefType();
                        ObjectRef=((SDMXObjectModel.Common.ObjectRefType)((SDMXObjectModel.Common.ObjectReferenceType)(Metadata.MetadataSet[0].Report[0].Target.ReferenceValue[0].Item)).Items[0]);
                        if (((ObjectRef.agencyID == ((SDMXObjectModel.Common.ItemSchemeRefType)Enumeration.Items[0]).agencyID) && (ObjectRef.maintainableParentID == ((SDMXObjectModel.Common.ItemSchemeRefType)Enumeration.Items[0]).id) && (ObjectRef.maintainableParentVersion == ((SDMXObjectModel.Common.ItemSchemeRefType)Enumeration.Items[0]).version))==false)
                        {
                            RetVal.Add(MetadataValidationStatus.Metadata_Target_Object_Reference_Invalid.ToString(), Constants.ValidationMessages.Invalid_Metadata_Target_Object_Reference);
                        }
                    }
                    else
                    {
                        RetVal.Add(MetadataValidationStatus.Metadata_Target_Invalid.ToString(), Constants.ValidationMessages.Invalid_Metadata_Target + Metadata.MetadataSet[0].Report[0].Target.id);
                    }
                }
                else
                {
                    RetVal.Add(MetadataValidationStatus.Metadata_Report_Invalid.ToString(), Constants.ValidationMessages.Invalid_Metadata_Report + Metadata.MetadataSet[0].Report[0].id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RetVal;
        }

        private Dictionary<string, string> Report_Structure_Validation(GenericMetadataType Metadata, SDMXObjectModel.Structure.MetadataStructureType MSD)
        {
            Dictionary<string, string> RetVal;
            List<string> ListOfReportedAttributes;
            SDMXObjectModel.Structure.ReportStructureType ReportStructure;
            SDMXObjectModel.Structure.MetadataAttributeType MetadataAttribute;
            int i;

            RetVal = new Dictionary<string, string>();
            ListOfReportedAttributes = new List<string>();
            try
            {
                ReportStructure=(SDMXObjectModel.Structure.ReportStructureType)(((SDMXObjectModel.Structure.MetadataStructureComponentsType)MSD.Item).Items[1]);
                for (i = 0; i < ReportStructure.Items.Count; i++)
                {
                    MetadataAttribute = ((SDMXObjectModel.Structure.MetadataAttributeType)(ReportStructure.Items[i]));
                    ListOfReportedAttributes.Add(MetadataAttribute.id);
                }
                for (i = 0; i < Metadata.MetadataSet[0].Report[0].AttributeSet.ReportedAttribute.Count; i++)
                {
                    if (!(ListOfReportedAttributes.Contains(Metadata.MetadataSet[0].Report[0].AttributeSet.ReportedAttribute[i].id)))
                    {
                        RetVal.Add(MetadataValidationStatus.Metadata_Report_Structure_Invalid.ToString(), Constants.ValidationMessages.Invalid_Metadata_Reported_Attribute + Metadata.MetadataSet[0].Report[0].AttributeSet.ReportedAttribute[i].id);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RetVal;
        }

        # endregion Private Methods
    }
}

