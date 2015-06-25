using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using SDMXObjectModel.Message;
using SDMXObjectModel.Structure;
using SDMXObjectModel;
using System.Xml;
using System.Xml.Schema;
using System.IO;
using SDMXObjectModel.Data.StructureSpecific;
using SDMXObjectModel.Common;

namespace DevInfo.Lib.DI_LibSDMX
{
    internal class SDMXMLValidateUtility : BaseValidateUtility
    {

        internal SDMXMLValidateUtility()
            : base()
        {
        }

        # region internal Methods

        internal override Dictionary<string, string> ValidateSdmxMlAgainstDSD(string dataFileNameWPath, string completeFileNameWPath)
        {
            Dictionary<string, string> RetVal;
            StructureSpecificTimeSeriesDataType Data;

            //1. Loading Data object from datafile on successful validation against SDMX schemas.
            RetVal = Validate_And_Load_Datafile(dataFileNameWPath, out Data);
            if (RetVal.Keys.Count == 0 && Data != null && !string.IsNullOrEmpty(completeFileNameWPath))
            {
                //2. Validating the loaded CompactData object against the dsd file. 
                RetVal = Validate_DSD_Compliance(Data, completeFileNameWPath);
            }



            return RetVal;
        }

        internal override Dictionary<string, string> ValidateSdmxML(string dataFileNameWPath)
        {
            Dictionary<string, string> RetVal;
            StructureSpecificTimeSeriesDataType Data;

            //1. Loading Data object from datafile on successful validation against SDMX schemas.
            RetVal = Validate_And_Load_Datafile(dataFileNameWPath, out Data);
           
            return RetVal;
        }

        # endregion internal Methods


        # region Private Methods

        /// <summary>
        /// Validate_And_Load_Datafile method:
        /// 1. Validates the SDMX-ML data file for:
        ///     1.1. Well formed xml.
        ///     1.2. The SDMX-ML data file's compliance to SDMX schemas.
        /// 2. Loads a StructureSpecificData object after successful valdations of SDMX-ML data file.
        /// </summary>
        /// <param name="dataFileNameWPath">The SDMX-ML file name with path to be validated.</param>
        /// <param name="Data">The StructureSpecificData object to be loaded if the SDMX-ML data file is found valid for the above-mentioned two 
        /// validations.</param>
        /// <returns>SDMXValidationStatus enum value.</returns>
        private Dictionary<string, string> Validate_And_Load_Datafile(string dataFileNameWPath, out StructureSpecificTimeSeriesDataType Data)
        {
            Dictionary<string, string> RetVal;
            XmlDocument Document;

            RetVal = new Dictionary<string, string>();
            Data = null;
            Document = null;

            try
            {
                // 1. Validating the SDMX-ML data file:
                RetVal = Validate_XML(dataFileNameWPath, out Document);

                if (RetVal.Keys.Count == 0)
                {
                    RetVal = Validate_SDMX(Document.InnerXml);
                }

                // 2. Loading the SDMX-ML data file in the CompactData object on successful validation against SDMX schemas.

                if (RetVal.Keys.Count == 0)
                {
                    Data = (StructureSpecificTimeSeriesDataType)(Deserializer.LoadFromXmlDocument(typeof(StructureSpecificTimeSeriesDataType), Document));
                }

            }
            catch (Exception ex)
            {
                // An exception while loading the SDMX-ML data file in a CompactData object implies invalid SDMX structure.
                if (ex is System.InvalidOperationException)
                {
                    RetVal.Add(SDMXValidationStatus.SDMX_Invalid.ToString(), ex.InnerException.Message.ToString());
                }
                // All other exceptions e.g System.IO exceptions are thrown to be handled by using code.
                else
                {
                    throw ex;
                }
            }

            return RetVal;

        }


        // Validation Error Count
        private Dictionary<string, string> SDMXValidationError = null;



        /// <summary>
        /// Validate_XML_And_SDMX_Header method:
        /// 1. Validates the SDMX-ML data file for well formed xml.
        /// 2. Validates the SDMX-ML data file's "header" element's compliance to SDMX schemas.
        /// </summary>
        /// <param name="dataFileNameWPath">The SDMX-ML file name with path to be validated.</param>
        /// <returns>SDMXValidationStatus enum value.</returns>
        /// <remarks>
        /// Only the "header" element in SDMX-ML data file is validated against SDMX schemas.The rest of datafile i.e DataSet element onwards, is 
        /// validated against the DSD in Validate_DSD_Compliance method. 
        /// Future Scope: Complete SDMX-ML data file will be validated against the SDMX schemas and the DSD schema in one go and the 
        /// Validate_DSD_Compliance method done away with. This is pending the completion of DSD schema creation logic.
        /// </remarks>
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

                // 2. Loading the SDMX schemas for validation of SDMX-ML data file.
                Load_SDMX_Schemas(ReaderSettings.Schemas);

                // 3. Setting the validation flags.
                ReaderSettings.ValidationType = ValidationType.Schema;
                ReaderSettings.ValidationFlags = XmlSchemaValidationFlags.AllowXmlAttributes | XmlSchemaValidationFlags.None;

                // 4. Creating an XmlReader object to read and validate the SDMX-ML data file against the schemas loaded.
                Reader = XmlReader.Create(new StringReader(XmlContent), ReaderSettings);

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
                        RetVal.Add(SDMXValidationStatus.SDMX_Invalid.ToString(), ex.Message.ToString());
                    }
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
                // 1. Loading the SDMX-ML data file in an XmlDocument object - an exception implies invalid XML structure.
                Document.Load(dataFileNameWPath);
            }
            catch (Exception ex)
            {
                // An XML exception implies invalid xml structure.
                if ((ex.InnerException is System.Xml.XmlException) || (ex is System.Xml.XmlException))
                {
                    RetVal.Add(SDMXValidationStatus.Xml_Invalid.ToString(), ex.Message.ToString());
                }
                // All other exceptions e.g System.IO exceptions are thrown to be handled by using code.
                else
                {
                    throw ex;
                }
            }

            return RetVal;
        }

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

        /// <summary>
        /// Validate_DSD_Compliance method validates a StructureSpecificData object against a DSD(data structure definition) file.
        /// </summary>
        /// <param name="Data">The StructureSpecificData object to be validated.</param>
        /// <param name="completeFileNameWPath">The DSD(data structure definition) file name with path to be used for validation.</param>
        /// <returns>SDMXValidationStatus enum value.</returns>
        private Dictionary<string, string> Validate_DSD_Compliance(StructureSpecificTimeSeriesDataType Data, string completeFileNameWPath)
        {
            Dictionary<string, string> RetVal;


            SDMXObjectModel.Message.StructureType Structure;
            SDMXObjectModel.Data.StructureSpecific.SeriesType Series;
            List<string> ListIndicator, ListUnit, ListArea, ListSource, ListDimensions, ListAttributes;
            Dictionary<string, List<string>> dictSubgroupBreakUp;
            int Counter = 0;

            RetVal = new Dictionary<string, string>();

            try
            {
                // 1. Loading the DSD file in a SDMXApi's Structure object.

                Structure = (SDMXObjectModel.Message.StructureType)(Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), completeFileNameWPath));

                // 2. Initialising various lists e.g. Indicators, Units etc. from the Structure object. These lists represent the codelists 
                // contained in Structure object and are later used to validate Series and Obs objects contained in StructureSpecificData object.



                Load_Lists_From_Codelists(Structure, out ListIndicator, out ListUnit, out ListArea,out ListSource, out dictSubgroupBreakUp);

                Load_ListOfDimensionsAtSeriesLevel(Structure, out ListDimensions);

                Load_ListOfAttributesAtObsLevel(Structure, out ListAttributes);

                while (RetVal.Keys.Count == 0 && Counter < Data.DataSet[0].Items.Count)
                {
                    // 3. Extracting a Series object from the StructureSpecificData's DataSet object.
                    Series = ((SeriesType)(Data.DataSet[0].Items[Counter]));

                    // 4. Validating a Series object at a time against the various lists.
                    RetVal = Validate_Series(Series, ListIndicator, ListUnit, ListArea, ListSource, dictSubgroupBreakUp, ListDimensions, ListAttributes);
                    Counter++;


                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RetVal;
        }

        /// <summary>
        /// Validate_Series method validates a Series object against the various lists initialised from the Structure object.
        /// </summary>
        /// <param name="Series">Series object to be validated against various lists.</param>
        /// <param name="ListIndicator">List of valid indicators.</param>
        /// <param name="ListUnit">List of valid units.</param>
        /// <param name="ListArea">List of valid reference areas.</param>
        /// <param name="dictSubgroupBreakUp">Dictionary of valid subgroup values.</param>
        /// <param name="ListDimensions">List of valid dimensions at series level.</param>
        /// <param name="ListAttributesMeasureAndTimeDim">List of valid Attribute,Measure and Time Dimension at Observation level.</param>
        /// <returns>SDMXValidationStatus enum value.</returns>
        private Dictionary<string, string> Validate_Series(SeriesType Series, List<string> ListIndicator, List<string> ListUnit, List<string> ListArea, List<string> ListSource, Dictionary<string, List<string>> dictSubgroupBreakUp, List<string> ListDimensions, List<string> ListAttributes)
        {
            Dictionary<string, string> RetVal;
            ObsType Obs;
            int Counter = 0;
            int ObsCounter = 0;

            RetVal = new Dictionary<string, string>();
            //RetVal.Add(SDMXValidationStatus.Valid.ToString(), string.Empty);


            while (RetVal.Keys.Count == 0 && Counter < Series.AnyAttr.Count)
            {
                if (!ListDimensions.Contains(Series.AnyAttr[Counter].Name))
                {
                    //RetVal = SDMXValidationStatus.Dimension_Invalid;
                    RetVal.Add(SDMXValidationStatus.Dimension_Invalid.ToString(), Constants.ValidationMessages.Invalid_Dimension + Series.AnyAttr[Counter].Name);
                }
                else
                {

                    switch (Series.AnyAttr[Counter].Name)
                    {
                        case Constants.Concept.INDICATOR.Id:
                            if (!ListIndicator.Contains(Series.AnyAttr[Counter].Value))
                            {
                                //RetVal = SDMXValidationStatus.Code_Invalid;
                                RetVal.Add(SDMXValidationStatus.Code_Invalid.ToString(), Constants.ValidationMessages.Invalid_Code + Series.AnyAttr[Counter].Value);
                            }

                            break;
                        case Constants.Concept.UNIT.Id:
                            if (!ListUnit.Contains(Series.AnyAttr[Counter].Value))
                            {
                                RetVal.Add(SDMXValidationStatus.Code_Invalid.ToString(), Constants.ValidationMessages.Invalid_Code + Series.AnyAttr[Counter].Value);
                            }

                            break;
                        case Constants.Concept.AREA.Id:
                            if (!ListArea.Contains(Series.AnyAttr[Counter].Value))
                            {
                                RetVal.Add(SDMXValidationStatus.Code_Invalid.ToString(), Constants.ValidationMessages.Invalid_Code + Series.AnyAttr[Counter].Value);
                            }

                            break;
                        case Constants.Concept.SOURCE.Id:
                            //if (!ListSource.Contains(Series.AnyAttr[Counter].Value))
                            //{
                            //    RetVal.Add(SDMXValidationStatus.Code_Invalid.ToString(), Constants.ValidationMessages.Invalid_Code + Series.AnyAttr[Counter].Value);
                            //}

                            break;
                        default:
                            if ((Series.AnyAttr[Counter].Value != "NA") && (!dictSubgroupBreakUp[Series.AnyAttr[Counter].Name].Contains(Series.AnyAttr[Counter].Value)))
                            {
                                RetVal.Add(SDMXValidationStatus.Code_Invalid.ToString(), Constants.ValidationMessages.Invalid_Code + Series.AnyAttr[Counter].Value);
                            }
                            break;
                    }
                    ObsCounter = 0;

                    while (RetVal.Keys.Count == 0 && ObsCounter < Series.Obs.Count)
                    {

                        // 4. Validating a Obs object at a time against the list of Atributes.
                        RetVal = Validate_Obs(Series.Obs[ObsCounter], ListAttributes);
                        ObsCounter++;


                    }



                    Counter++;

                }
            }

            return RetVal;
        }

        private Dictionary<string, string> Validate_Obs(ObsType Obs, List<string> ListAttributes)
        {
            Dictionary<string, string> RetVal;
            int Counter = 0;

            RetVal = new Dictionary<string, string>();

            while (RetVal.Keys.Count == 0 && Counter < Obs.AnyAttr.Count)
            {
                if (!ListAttributes.Contains(Obs.AnyAttr[Counter].Name))
                {
                    RetVal.Add(SDMXValidationStatus.Dimension_Invalid.ToString(), Constants.ValidationMessages.Invalid_Dimension + Obs.AnyAttr[Counter].Name);
                }

                else
                {
                    Counter++;
                }
            }

            return RetVal;
        }


        /// <summary>
        /// Load_Lists_From_Codelists method initialises various lists from a SDMXApi's Structure object. 
        /// </summary>
        /// <param name="Structure">SDMXApi's Structure object to be used for initialisation of lists.</param>
        /// <param name="ListIndicator">List of valid indicators.</param>
        /// <param name="ListUnit">List of valid units.</param>
        /// <param name="ListArea">List of valid reference areas.</param>
        /// <remarks>These lists represent the codelists contained in Structure object and are later used to validate Series and Obs objects 
        /// contained in StructureSpecificData object.</remarks>
        private void Load_Lists_From_Codelists(SDMXObjectModel.Message.StructureType Structure, out List<string> ListIndicator, out List<string> ListUnit,
                                               out List<string> ListArea, out List<string> ListSource, out Dictionary<string, List<string>> dictSubgroupBreakUp)
        {
            ListIndicator = new List<string>();
            ListUnit = new List<string>();
            ListArea = new List<string>();
            ListSource = new List<string>();
            dictSubgroupBreakUp = new Dictionary<string, List<string>>();

            // 1. Initialising all Lists and dictionary of Subgroup Breakups.
            foreach (CodelistType Codelist in Structure.Structures.Codelists)
            {
                switch (Codelist.id)
                {
                    case Constants.CodeList.Indicator.Id:
                        ListIndicator = Get_CodeValues_From_Codelist(Codelist);
                        break;
                    case Constants.CodeList.Unit.Id:
                        ListUnit = Get_CodeValues_From_Codelist(Codelist);
                        break;
                    case Constants.CodeList.Area.Id:
                        ListArea = Get_CodeValues_From_Codelist(Codelist);
                        break;
                    //case Constants.CodeList.Source.Id:
                    //    ListSource = Get_CodeValues_From_Codelist(Codelist);
                    //    break;
                    case Constants.CodeList.SubgroupVal.Id:
                        break;
                    case Constants.CodeList.SubgroupType.Id:
                        break;
                    case Constants.CodeList.IUS.Id:
                        break;
                    default:
                        dictSubgroupBreakUp.Add(Codelist.id.Replace(Constants.CodelistPrefix, string.Empty), Get_CodeValues_From_Codelist(Codelist));
                        break;
                }
            }

        }

        private void Load_ListOfDimensionsAtSeriesLevel(SDMXObjectModel.Message.StructureType Structure, out List<string> ListDimensions)
        {

            ListDimensions = new List<string>();
            int i;
            DimensionType Dimension;

            SDMXObjectModel.Structure.DataStructureComponentsType DSComponents = new DataStructureComponentsType();
            DSComponents = (SDMXObjectModel.Structure.DataStructureComponentsType)(Structure.Structures.DataStructures[0].Item);



            for (i = 0; i < DSComponents.Items[0].Items.Count; i++)
            {
                if (DSComponents.Items[0].Items[i] is SDMXObjectModel.Structure.DimensionType)
                {
                    Dimension = (SDMXObjectModel.Structure.DimensionType)(DSComponents.Items[0].Items[i]);
                    ListDimensions.Add(((ConceptRefType)(Dimension.ConceptIdentity.Items[0])).id);
                }
            }


        }

        private void Load_ListOfAttributesAtObsLevel(SDMXObjectModel.Message.StructureType Structure, out List<string> ListAttributes)
        {

            ListAttributes = new List<string>();
            int i;
            DimensionType Dimension;
            TimeDimensionType TimeDimension;
            AttributeType Attribute;
            ConceptReferenceType ConceptIdentity;
            PrimaryMeasureType PrimaryMeasure;


            SDMXObjectModel.Structure.DataStructureComponentsType DSComponents = new DataStructureComponentsType();
            DSComponents = (SDMXObjectModel.Structure.DataStructureComponentsType)(Structure.Structures.DataStructures[0].Item);

            for (i = 0; i < DSComponents.Items[1].Items.Count; i++)
            {
                Attribute = (SDMXObjectModel.Structure.AttributeType)(DSComponents.Items[1].Items[i]);
                ConceptIdentity = Attribute.ConceptIdentity;
                ListAttributes.Add(((ConceptRefType)(ConceptIdentity.Items[0])).id);
            }

        }

        /// <summary>
        /// Get_CodeValues_From_Codelist method poulates a List from the "ID" attributes of all codes contained in Codelist object.
        /// </summary>
        /// <param name="CodeList">Codelist object to populate the List<string></param>
        /// <returns>List<string> populated from the Codelist object.</returns>
        private List<string> Get_CodeValues_From_Codelist(CodelistType Codelist)
        {
            List<string> RetVal;

            RetVal = new List<string>();
            foreach (CodeType Code in (Codelist.Items))
            {
                RetVal.Add(Code.id);
            }

            return RetVal;
        }


        # endregion Private Methods
    }
}
