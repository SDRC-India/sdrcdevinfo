using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using SDMXObjectModel.Structure;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using SDMXObjectModel.Data.StructureSpecific;
using SDMXObjectModel.Common;
using SDMXObjectModel;

namespace DevInfo.Lib.DI_LibSDMX
{
    internal class DSDValidateUtility : BaseValidateUtility
    {
        internal DSDValidateUtility()
            : base()
        {
        }

        # region internal Methods

        internal override Dictionary<string, string> ValidateDSDAgainstDevInfoDSD(string dsdFileNameWPath, string devinfodsdFileNameWPath)
        {
            Dictionary<string, string> RetVal;
            XmlReader Reader;
            XmlDocument Document;
            XmlReaderSettings ReaderSettings;

            RetVal = new Dictionary<string,string>();
            Reader = null;
            Document = new XmlDocument();
            ReaderSettings = new XmlReaderSettings();
            try
            {
                // 1. Loading the SDMX-ML data file in an XmlDocument object - an exception implies invalid XML structure.
                Document.Load(dsdFileNameWPath);

                // 2. Loading the SDMX schemas for validation of SDMX-ML data file.
                Load_SDMX_Schemas(ReaderSettings.Schemas);

                // 3. Setting the validation flags.
                ReaderSettings.ValidationType = ValidationType.Schema;
                ReaderSettings.ValidationFlags = XmlSchemaValidationFlags.AllowXmlAttributes | XmlSchemaValidationFlags.None;

                // 4. Creating an XmlReader object to read and validate the SDMX-ML data file against the schemas loaded.
                Reader = XmlReader.Create(new StringReader(Document.InnerXml), ReaderSettings);

                // 5. Reading and validating the SDMX-ML data file line by line.
                while (Reader.Read())
                {
                }


                RetVal=Check_DimensionsAttributesMeasure_Exist_In_MasterDSD(dsdFileNameWPath, devinfodsdFileNameWPath);
            }
            catch (Exception ex)
            {
                // An XmlSchemaValidationException while reading the SDMX-ML data file before the dataSet tag implies invalid SDMX structure.
                if (ex is System.Xml.Schema.XmlSchemaValidationException)
                {
                    if (!(Reader.NodeType == XmlNodeType.Element && Reader.LocalName == SDMXObjectModel.Constants.Elements.DataSet))
                    {
                        RetVal.Add(DSDValidationStatus.SDMX_Invalid.ToString(), ex.Message.ToString());
                    }
                }
                // An XML exception implies invalid xml structure.
                else if ((ex.InnerException is System.Xml.XmlException) || (ex is System.Xml.XmlException))
                {
                    RetVal.Add(DSDValidationStatus.Xml_Invalid.ToString(), ex.Message.ToString());
                }
                // All other exceptions e.g System.IO exceptions are thrown to be handled by using code.
                else
                {
                    throw ex;
                }
            }

            return RetVal;
        }

        internal override Dictionary<string, string> ValidateDSD(string dsdFileNameWPath)
        {
            Dictionary<string, string> RetVal;
            XmlReader Reader;
            XmlDocument Document;
            XmlReaderSettings ReaderSettings;

            RetVal = new Dictionary<string, string>();
            Reader = null;
            Document = new XmlDocument();
            ReaderSettings = new XmlReaderSettings();
            try
            {
                // 1. Loading the SDMX-ML data file in an XmlDocument object - an exception implies invalid XML structure.
                Document.Load(dsdFileNameWPath);

                // 2. Loading the SDMX schemas for validation of SDMX-ML data file.
                Load_SDMX_Schemas(ReaderSettings.Schemas);

                // 3. Setting the validation flags.
                ReaderSettings.ValidationType = ValidationType.Schema;
                ReaderSettings.ValidationFlags = XmlSchemaValidationFlags.AllowXmlAttributes | XmlSchemaValidationFlags.None;

                // 4. Creating an XmlReader object to read and validate the SDMX-ML data file against the schemas loaded.
                Reader = XmlReader.Create(new StringReader(Document.InnerXml), ReaderSettings);

                // 5. Reading and validating the SDMX-ML data file line by line.
                while (Reader.Read())
                {
                }
            }
            catch (Exception ex)
            {
                // An XmlSchemaValidationException while reading the SDMX-ML data file before the dataSet tag implies invalid SDMX structure.
                if (ex is System.Xml.Schema.XmlSchemaValidationException)
                {
                    if (!(Reader.NodeType == XmlNodeType.Element && Reader.LocalName == SDMXObjectModel.Constants.Elements.DataSet))
                    {
                        RetVal.Add(DSDValidationStatus.SDMX_Invalid.ToString(), ex.Message.ToString());
                    }
                }
                // An XML exception implies invalid xml structure.
                else if ((ex.InnerException is System.Xml.XmlException) || (ex is System.Xml.XmlException))
                {
                    RetVal.Add(DSDValidationStatus.Xml_Invalid.ToString(), ex.Message.ToString());
                }
                // All other exceptions e.g System.IO exceptions are thrown to be handled by using code.
                else
                {
                    throw ex;
                }
            }

            return RetVal;
        }

        # endregion internal Methods


        # region Private Methods

        /// <summary>
        /// Load_SDMX_Schemas method loads SDMX schemas in a XmlSchemaSet object.
        /// </summary>
        /// <param name="Schemas">XmlSchemaSet object into which the SDMX schemas have to be loaded</param>
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


        private Dictionary<string, string> Check_DimensionsAttributesMeasure_Exist_In_MasterDSD(string dsdFileNameWPath, string devinfodsdFileNameWPath)
        {
            Dictionary<string, string> RetVal;
            List<string> UserDSDDimensionList, UserDSDAttributeList,UserDSDMeasureList;
            List<string> MasterDSDDimensionList, MasterDSDAttributeList,MasterDSDMeasureList;
            XmlDocument UserDSDXml,MasterDevInfoDSDXml;



            RetVal = new Dictionary<string,string>();

            UserDSDXml = new XmlDocument();
            MasterDevInfoDSDXml = new XmlDocument();

            SDMXObjectModel.Message.StructureType UserDSD = new SDMXObjectModel.Message.StructureType();
            SDMXObjectModel.Message.StructureType MasterDevInfoDSD = new SDMXObjectModel.Message.StructureType();
            SDMXObjectModel.Structure.DataStructureComponentsType UserDSComponents = new DataStructureComponentsType();
            SDMXObjectModel.Structure.DataStructureComponentsType MasterDSComponents = new DataStructureComponentsType();


            UserDSDXml.Load(dsdFileNameWPath);
            UserDSD = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), UserDSDXml);

            MasterDevInfoDSDXml.Load(devinfodsdFileNameWPath);
            MasterDevInfoDSD = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureType), MasterDevInfoDSDXml);

            UserDSComponents = (SDMXObjectModel.Structure.DataStructureComponentsType)(UserDSD.Structures.DataStructures[0].Item);
            MasterDSComponents = (SDMXObjectModel.Structure.DataStructureComponentsType)(MasterDevInfoDSD.Structures.DataStructures[0].Item);

            UserDSDDimensionList = BindDimensionList(UserDSComponents);
            UserDSDAttributeList = BindAttributeList(UserDSComponents);
            UserDSDMeasureList = BindMeasureList(UserDSComponents);

            RetVal=Check_MandatoryDimensions_Exist_In_UserDSD(UserDSDDimensionList, UserDSDMeasureList);
            if (RetVal.Keys.Count == 0)
            {
                MasterDSDDimensionList = BindDimensionList(MasterDSComponents);
                MasterDSDAttributeList = BindAttributeList(MasterDSComponents);
                MasterDSDMeasureList = BindMeasureList(MasterDSComponents);

                //Comparison of Dimensions

                foreach (string Dimension in UserDSDDimensionList)
                {
                    if (!MasterDSDDimensionList.Contains(Dimension))
                    {
                        RetVal.Add(DSDValidationStatus.DSD_Dimensions_Invalid.ToString(), Constants.ValidationMessages.Invalid_Dimension + Dimension);
                        break;
                    }

                }

                //Comparison of Attributes

                foreach (string Attribute in UserDSDAttributeList)
                {
                    if (!MasterDSDAttributeList.Contains(Attribute))
                    {
                        RetVal.Add(DSDValidationStatus.DSD_Attribute_Invalid.ToString(), Constants.ValidationMessages.Invalid_Attribute + Attribute);
                        break;
                    }

                }

                //Comparison of Measure

                foreach (string Measure in UserDSDMeasureList)
                {
                    if (!MasterDSDMeasureList.Contains(Measure))
                    {
                        RetVal.Add(DSDValidationStatus.DSD_Primary_Measure_Invalid.ToString(), Constants.ValidationMessages.Invalid_Primary_Measure + Measure);
                        break;
                    }

                }
            
            }

            return RetVal;

        }

        private Dictionary<string, string> Check_MandatoryDimensions_Exist_In_UserDSD(List<string> UserDSDDimensionList, List<string> UserDSDMeasureList)
        {
            Dictionary<string, string> RetVal;
            RetVal = new Dictionary<string,string>();

            if (!UserDSDDimensionList.Contains(Constants.Concept.INDICATOR.Id))
            {
                RetVal.Add(DSDValidationStatus.DSD_MandatoryDimensions_Invalid.ToString(), Constants.ValidationMessages.Non_Existing_Mandatory_Dimension + Constants.Concept.INDICATOR.Id);
            }

            if (!UserDSDDimensionList.Contains(Constants.Concept.UNIT.Id))
            {
                RetVal.Add(DSDValidationStatus.DSD_MandatoryDimensions_Invalid.ToString(), Constants.ValidationMessages.Non_Existing_Mandatory_Dimension + Constants.Concept.UNIT.Id);
            }

            if (!UserDSDDimensionList.Contains(Constants.Concept.AREA.Id))
            {
                RetVal.Add(DSDValidationStatus.DSD_MandatoryDimensions_Invalid.ToString(), Constants.ValidationMessages.Non_Existing_Mandatory_Dimension + Constants.Concept.AREA.Id);
            }

            if (!UserDSDDimensionList.Contains(Constants.Concept.SOURCE.Id))
            {
                RetVal.Add(DSDValidationStatus.DSD_MandatoryDimensions_Invalid.ToString(), Constants.ValidationMessages.Non_Existing_Mandatory_Dimension + Constants.Concept.SOURCE.Id);
            }


            if (!UserDSDDimensionList.Contains(Constants.Concept.TIME_PERIOD.Id))
            {
                RetVal.Add(DSDValidationStatus.DSD_MandatoryDimensions_Invalid.ToString(), Constants.ValidationMessages.Non_Existing_Mandatory_Dimension + Constants.Concept.TIME_PERIOD.Id);
            }
            if (!UserDSDMeasureList.Contains(Constants.Concept.OBS_VALUE.Id))
            {
                RetVal.Add(DSDValidationStatus.DSD_MandatoryDimensions_Invalid.ToString(), Constants.ValidationMessages.Non_Existing_Mandatory_Dimension + Constants.Concept.OBS_VALUE.Id);
            }
          
            return RetVal;
        }

        private List<string> BindDimensionList(DataStructureComponentsType DSComponents)
        {
            List<string> RetVal;
            int i;
            SDMXObjectModel.Structure.DimensionType Dimension;
            SDMXObjectModel.Structure.TimeDimensionType TimeDimension;
            SDMXObjectModel.Common.ConceptReferenceType ConceptIdentity;

            RetVal = new List<string>();
            for (i = 0; i < DSComponents.Items[0].Items.Count; i++)
            {
                if (DSComponents.Items[0].Items[i] is SDMXObjectModel.Structure.DimensionType)
                {
                    Dimension = (SDMXObjectModel.Structure.DimensionType)(DSComponents.Items[0].Items[i]);
                    ConceptIdentity = Dimension.ConceptIdentity;
                    RetVal.Add(((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).id.ToString());
                }
                else
                {

                    TimeDimension = (SDMXObjectModel.Structure.TimeDimensionType)(DSComponents.Items[0].Items[i]);
                    ConceptIdentity = TimeDimension.ConceptIdentity;
                    RetVal.Add(((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).id.ToString());
                }

            }

            return RetVal;
        
        }

        private List<string> BindAttributeList(DataStructureComponentsType DSComponents)
        {
            List<string> RetVal;
            int i;
            SDMXObjectModel.Structure.AttributeType Attribute; 
            SDMXObjectModel.Common.ConceptReferenceType ConceptIdentity;

            RetVal = new List<string>();
            for (i = 0; i < DSComponents.Items[1].Items.Count; i++)
            {
                if (DSComponents.Items[1].Items[i] is SDMXObjectModel.Structure.AttributeType)
                {
                    Attribute = (SDMXObjectModel.Structure.AttributeType)(DSComponents.Items[1].Items[i]);
                    ConceptIdentity = Attribute.ConceptIdentity;
                    RetVal.Add(((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).id.ToString());
                }

            }

            return RetVal;

        }

        private List<string> BindMeasureList(DataStructureComponentsType DSComponents)
        {
            List<string> RetVal;
            int i;
            SDMXObjectModel.Structure.PrimaryMeasureType PrimaryMeasure;
            SDMXObjectModel.Common.ConceptReferenceType ConceptIdentity;

            RetVal = new List<string>();
            for (i = 0; i < DSComponents.Items[2].Items.Count; i++)
            {
                if (DSComponents.Items[2].Items[i] is SDMXObjectModel.Structure.PrimaryMeasureType)
                {
                    PrimaryMeasure = (SDMXObjectModel.Structure.PrimaryMeasureType)(DSComponents.Items[2].Items[i]);
                    ConceptIdentity = PrimaryMeasure.ConceptIdentity;
                    RetVal.Add(((SDMXObjectModel.Common.ConceptRefType)(ConceptIdentity.Items[0])).id.ToString());
                }

            }

            return RetVal;

        }

        # endregion Private Methods
    }
}
