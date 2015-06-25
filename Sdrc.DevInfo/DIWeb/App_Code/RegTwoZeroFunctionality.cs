using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;
using DevInfo.Lib.DI_LibSDMX;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.Data;
using SDMXApi_2_0.Message;
using SDMXApi_2_0.Structure;
using System.Xml;
using System.Xml.Schema;
using SDMXApi_2_0.CompactData;
using SDMXLibrary = DevInfo.Lib.DI_LibSDMX;
using SDMXApi_2_0;
using SDMXApi_2_0.GenericMetadata;
using SDMXApi_2_0.Common;
using SDMXTwoOneStrucutre = SDMXObjectModel.Structure;
using SDMXTwoOneCommon = SDMXObjectModel.Common;
using DevInfo.Lib.DI_LibDAL.Queries;

public static class RegTwoZeroFunctionality
{
    #region "--Methods--"

    #region "--Private--"

    #region "--Validation Data--"

    private static Dictionary<string, string> Validate_And_Load_Datafile(string dataFileNameWPath, out CompactDataType Data)
    {
        Dictionary<string, string> RetVal;
        XmlDocument Document;

        RetVal = new Dictionary<string, string>();
        Data = null;
        Document = new XmlDocument();

        try
        {
            // 1. Validating the SDMX-ML data file for:
            //   1.1. Well formed xml.
            //   1.2. The SDMX-ML data file's compliance to SDMX schemas.
            RetVal = Validate_XML_And_SDMX_Header(dataFileNameWPath);

            // 2. Loading the SDMX-ML data file in the CompactData object on successful validation against SDMX schemas.
            if (RetVal.Keys.Count == 0)
            {
                Document.Load(dataFileNameWPath);
                Data = ((CompactDataType)(SDMXApi_2_0.Deserializer.LoadFromXmlDocument(typeof(CompactDataType), Document)));
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            // An exception while loading the SDMX-ML data file in a CompactData object implies invalid SDMX structure.
            if (ex.InnerException is System.InvalidOperationException)
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

    private static Dictionary<string, string> Validate_XML_And_SDMX_Header(string dataFileNameWPath)
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
            Document.Load(dataFileNameWPath);

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
            Global.CreateExceptionString(ex, null);
            // An XmlSchemaValidationException while reading the SDMX-ML data file before the dataSet tag implies invalid SDMX structure.
            if (ex is System.Xml.Schema.XmlSchemaValidationException)
            {
                if (!(Reader.NodeType == XmlNodeType.Element && Reader.LocalName == SDMXApi_v2_1.SDMX.Constants.Elements.DataSet))
                {
                    RetVal.Add(SDMXValidationStatus.SDMX_Invalid.ToString(), ex.Message.ToString());
                }
            }
            // An XML exception implies invalid xml structure.
            else if (ex is System.Xml.XmlException)
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

    private static Dictionary<string, string> Validate_DSD_Compliance(CompactDataType Data, string dsdFileNameWPath)
    {
        Dictionary<string, string> RetVal;
        StructureType Structure;
        SDMXApi_2_0.CompactData.SeriesType Series;
        List<string> ListFrequency, ListIndicator, ListUnit, ListLocation, ListAge, ListSex, ListArea, ListSourceType, ListNature, ListUnitMult;
        int Counter = 0;

        RetVal = new Dictionary<string, string>();
        try
        {
            // 1. Loading the DSD file in a SDMXApi's Structure object.
            Structure = ((StructureType)(SDMXApi_2_0.Deserializer.LoadFromFile(typeof(StructureType), dsdFileNameWPath)));

            // 2. Initialising various lists e.g. Indicators, Units etc. from the Structure object. These lists represent the codelists 
            // contained in Structure object and are later used to validate Series and Obs objects contained in CompactData object.
            Load_Lists_From_Codelists(Structure, out ListFrequency, out ListIndicator, out ListUnit, out ListLocation, out ListAge, out ListSex,
                                      out ListArea, out ListSourceType, out ListNature, out ListUnitMult);

            while (RetVal.Keys.Count == 0 && Counter < Data.DataSet.ListSeries.Count)
            {
                // 3. Extracting a Series object from the CompactData's DataSet object.
                Series = Data.DataSet.ListSeries[Counter];

                // 4. Validating a Series object at a time against the various lists.
                RetVal = Validate_Series(Series, ListFrequency, ListIndicator, ListUnit, ListLocation, ListAge, ListSex, ListArea,
                                             ListSourceType, ListNature, ListUnitMult);
                Counter++;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }

        return RetVal;
    }

    private static Dictionary<string, string> Validate_Series(SDMXApi_2_0.CompactData.SeriesType Series, List<string> ListFrequency, List<string> ListIndicator, List<string> ListUnit,
                                                 List<string> ListLocation, List<string> ListAge, List<string> ListSex, List<string> ListArea,
                                                 List<string> ListSourceType, List<string> ListNature, List<string> ListUnitMult)
    {
        Dictionary<string, string> RetVal;
        SDMXApi_2_0.CompactData.ObsType Obs;
        int Counter = 0;
        int AttributeCounter = 0;
        string SeriesFrequency = string.Empty;
        string SeriesSeries = string.Empty;
        string SeriesUnit = string.Empty;
        string SeriesLocation = string.Empty;
        string SeriesAge = string.Empty;
        string SeriesSex = string.Empty;
        string SeriesArea = string.Empty;
        string SeriesSourceType = string.Empty;

        RetVal = new Dictionary<string, string>();

        while (AttributeCounter < Series.AnyAttr.Count)
        {

            if (!((Series.AnyAttr[AttributeCounter].Name == Constants.UNSD.Concept.Frequency.Id) || (Series.AnyAttr[AttributeCounter].Name == Constants.UNSD.Concept.Indicator.Id) || (Series.AnyAttr[AttributeCounter].Name == Constants.UNSD.Concept.Unit.Id) || (Series.AnyAttr[AttributeCounter].Name == Constants.UNSD.Concept.Location.Id) || (Series.AnyAttr[AttributeCounter].Name == Constants.UNSD.Concept.Age.Id) || (Series.AnyAttr[AttributeCounter].Name == Constants.UNSD.Concept.Sex.Id) || (Series.AnyAttr[AttributeCounter].Name == Constants.UNSD.Concept.Area.Id) || (Series.AnyAttr[AttributeCounter].Name == Constants.UNSD.Concept.SourceType.Id)))
            {
                RetVal.Add(SDMXValidationStatus.Dimension_Invalid.ToString(), DevInfo.Lib.DI_LibSDMX.Constants.ValidationMessages.Invalid_Dimension + Series.AnyAttr[AttributeCounter].Name);
                break;
            }
            else
            {
                switch (Series.AnyAttr[AttributeCounter].Name)
                {
                    case Constants.UNSD.Concept.Frequency.Id:
                        SeriesFrequency = Series.AnyAttr[AttributeCounter].Value;
                        break;
                    case Constants.UNSD.Concept.Indicator.Id:
                        SeriesSeries = Series.AnyAttr[AttributeCounter].Value;
                        break;
                    case Constants.UNSD.Concept.Unit.Id:
                        SeriesUnit = Series.AnyAttr[AttributeCounter].Value;
                        break;
                    case Constants.UNSD.Concept.Location.Id:
                        SeriesLocation = Series.AnyAttr[AttributeCounter].Value;
                        break;
                    case Constants.UNSD.Concept.Age.Id:
                        SeriesAge = Series.AnyAttr[AttributeCounter].Value;
                        break;
                    case Constants.UNSD.Concept.Sex.Id:
                        SeriesSex = Series.AnyAttr[AttributeCounter].Value;
                        break;
                    case Constants.UNSD.Concept.Area.Id:
                        SeriesArea = Series.AnyAttr[AttributeCounter].Value;
                        break;
                    case Constants.UNSD.Concept.SourceType.Id:
                        SeriesSourceType = Series.AnyAttr[AttributeCounter].Value;
                        break;
                    default:
                        break;
                }

            }

            AttributeCounter = AttributeCounter + 1;
        }
        if (RetVal.Keys.Count == 0)
        {
            if (!ListFrequency.Contains(SeriesFrequency))
            {
                RetVal.Add(SDMXValidationStatus.Code_Invalid.ToString(), DevInfo.Lib.DI_LibSDMX.Constants.ValidationMessages.Invalid_Code + SeriesFrequency);
            }
            else
            {
                if (!ListIndicator.Contains(SeriesSeries))
                {
                    RetVal.Add(SDMXValidationStatus.Code_Invalid.ToString(), DevInfo.Lib.DI_LibSDMX.Constants.ValidationMessages.Invalid_Code + SeriesSeries);
                }
                else
                {

                    if (!ListUnit.Contains(SeriesUnit))
                    {
                        RetVal.Add(SDMXValidationStatus.Code_Invalid.ToString(), DevInfo.Lib.DI_LibSDMX.Constants.ValidationMessages.Invalid_Code + SeriesUnit);
                    }
                    else
                    {

                        if (!string.IsNullOrEmpty(SeriesLocation) && !ListLocation.Contains(SeriesLocation))
                        {
                            RetVal.Add(SDMXValidationStatus.Code_Invalid.ToString(), DevInfo.Lib.DI_LibSDMX.Constants.ValidationMessages.Invalid_Code + SeriesLocation);
                        }
                        else
                        {

                            if (!string.IsNullOrEmpty(SeriesAge) && !ListAge.Contains(SeriesAge))
                            {
                                RetVal.Add(SDMXValidationStatus.Code_Invalid.ToString(), DevInfo.Lib.DI_LibSDMX.Constants.ValidationMessages.Invalid_Code + SeriesAge);
                            }
                            else
                            {

                                if (!string.IsNullOrEmpty(SeriesSex) && !ListSex.Contains(SeriesSex))
                                {
                                    RetVal.Add(SDMXValidationStatus.Code_Invalid.ToString(), DevInfo.Lib.DI_LibSDMX.Constants.ValidationMessages.Invalid_Code + SeriesSex);
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(SeriesArea) && !ListArea.Contains(SeriesArea))
                                    {
                                        RetVal.Add(SDMXValidationStatus.Code_Invalid.ToString(), DevInfo.Lib.DI_LibSDMX.Constants.ValidationMessages.Invalid_Code + SeriesArea);
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(SeriesSourceType) && !ListSourceType.Contains(SeriesSourceType))
                                        {
                                            RetVal.Add(SDMXValidationStatus.Code_Invalid.ToString(), DevInfo.Lib.DI_LibSDMX.Constants.ValidationMessages.Invalid_Code + SeriesSourceType);
                                        }
                                        else
                                        {
                                            Counter = 0;
                                            while (RetVal.Keys.Count == 0 && Counter < Series.ListObs.Count)
                                            {
                                                // 1. Extracting an Obs object from the Series' object.
                                                Obs = Series.ListObs[Counter];

                                                // 2. Validating an Obs object at a time against the various lists.
                                                RetVal = Validate_Obs(Obs, ListNature, ListUnitMult);
                                                Counter++;
                                            }
                                        }

                                    }

                                }
                            }
                        }
                    }
                }

            }
        }

        return RetVal;
    }

    private static Dictionary<string, string> Validate_Obs(SDMXApi_2_0.CompactData.ObsType Obs, List<string> ListNature, List<string> ListUnitMult)
    {
        Dictionary<string, string> RetVal;
        string ObsNature = string.Empty;
        string ObsUnitMult = string.Empty;
        int AttributeCounter = 0;

        RetVal = new Dictionary<string, string>();
        while (AttributeCounter < Obs.AnyAttr.Count)
        {
            if (!((Obs.AnyAttr[AttributeCounter].Name == Constants.UNSD.Concept.Nature.Id) || (Obs.AnyAttr[AttributeCounter].Name == Constants.UNSD.Concept.UnitMult.Id) || (Obs.AnyAttr[AttributeCounter].Name == Constants.UNSD.Concept.TimePeriod.Id) || (Obs.AnyAttr[AttributeCounter].Name == Constants.UNSD.Concept.ObsVal.Id) || (Obs.AnyAttr[AttributeCounter].Name == Constants.UNSD.Concept.TimeDetail.Id) || (Obs.AnyAttr[AttributeCounter].Name == Constants.UNSD.Concept.SourceDetail.Id) || (Obs.AnyAttr[AttributeCounter].Name == Constants.UNSD.Concept.Footnotes.Id)))
            {
                RetVal.Add(SDMXValidationStatus.Dimension_Invalid.ToString(), DevInfo.Lib.DI_LibSDMX.Constants.ValidationMessages.Invalid_Dimension + Obs.AnyAttr[AttributeCounter].Name);
                break;
            }
            else
            {
                switch (Obs.AnyAttr[AttributeCounter].Name)
                {
                    case Constants.UNSD.Concept.Nature.Id:
                        ObsNature = Obs.AnyAttr[AttributeCounter].Value;
                        break;
                    case Constants.UNSD.Concept.UnitMult.Id:
                        ObsUnitMult = Obs.AnyAttr[AttributeCounter].Value;
                        break;
                    default:
                        break;
                }
            }

            AttributeCounter = AttributeCounter + 1;
        }
        if (RetVal.Keys.Count == 0)
        {
            if (!ListNature.Contains(ObsNature))
            {
                RetVal.Add(SDMXValidationStatus.Code_Invalid.ToString(), DevInfo.Lib.DI_LibSDMX.Constants.ValidationMessages.Invalid_Code + ObsNature);
            }
        }

        if (RetVal.Keys.Count == 0)
        {
            if (!ListUnitMult.Contains(ObsUnitMult))
            {
                RetVal.Add(SDMXValidationStatus.Code_Invalid.ToString(), DevInfo.Lib.DI_LibSDMX.Constants.ValidationMessages.Invalid_Code + ObsUnitMult);
            }
        }


        return RetVal;
    }

    private static void Load_Lists_From_Codelists(StructureType Structure, out List<string> ListFrequency, out List<string> ListIndicator,
                                           out List<string> ListUnit, out List<string> ListLocation, out List<string> ListAge,
                                           out List<string> ListSex, out List<string> ListArea, out List<string> ListSourceType,
                                           out List<string> ListNature, out List<string> ListUnitMult)
    {
        ListFrequency = new List<string>();
        ListIndicator = new List<string>();
        ListUnit = new List<string>();
        ListLocation = new List<string>();
        ListAge = new List<string>();
        ListSex = new List<string>();
        ListArea = new List<string>();
        ListSourceType = new List<string>();
        ListNature = new List<string>();
        ListUnitMult = new List<string>();
        string IndicatorCodelistId, UnitCodelistId, AreaCodelistId, AgeCodelistId, SexCodelistId, LocationCodelistId, NatureCodelistId, FreqCodelistId, SourceTypeCodelistId, UnitMultCodelistId;
        IndicatorCodelistId = string.Empty;
        UnitCodelistId = string.Empty;
        AreaCodelistId = string.Empty;
        AgeCodelistId = string.Empty;
        SexCodelistId = string.Empty;
        LocationCodelistId = string.Empty;
        NatureCodelistId = string.Empty;
        FreqCodelistId = string.Empty;
        SourceTypeCodelistId = string.Empty;
        UnitMultCodelistId = string.Empty;
        foreach (SDMXApi_2_0.Structure.DimensionType Dimensions in Structure.KeyFamilies[0].Components.Dimension)
        {
            if (Dimensions.conceptRef == Constants.UNSD.Concept.Indicator.Id)
            {
                IndicatorCodelistId = Dimensions.codelist;
            }
            if (Dimensions.conceptRef == Constants.UNSD.Concept.Unit.Id)
            {
                UnitCodelistId = Dimensions.codelist;
            }
            if (Dimensions.conceptRef == Constants.UNSD.Concept.Area.Id)
            {
                AreaCodelistId = Dimensions.codelist;
            }
            if (Dimensions.conceptRef == Constants.UNSD.Concept.Age.Id)
            {
                AgeCodelistId = Dimensions.codelist;
            }
            if (Dimensions.conceptRef == Constants.UNSD.Concept.Sex.Id)
            {
                SexCodelistId = Dimensions.codelist;
            }
            if (Dimensions.conceptRef == Constants.UNSD.Concept.Location.Id)
            {
                LocationCodelistId = Dimensions.codelist;
            }
            if (Dimensions.conceptRef == Constants.UNSD.Concept.Frequency.Id)
            {
                FreqCodelistId = Dimensions.codelist;
            }

            if (Dimensions.conceptRef == Constants.UNSD.Concept.SourceType.Id)
            {
                SourceTypeCodelistId = Dimensions.codelist;
            }

        }
        foreach (SDMXApi_2_0.Structure.AttributeType Attributes in Structure.KeyFamilies[0].Components.Attribute)
        {
            if (Attributes.conceptRef == Constants.UNSD.Concept.Nature.Id)
            {
                NatureCodelistId = Attributes.codelist;
            }
            if (Attributes.conceptRef == Constants.UNSD.Concept.UnitMult.Id)
            {
                UnitMultCodelistId = Attributes.codelist;
            }
        }




        // 1. Initialising all Lists except sources.
        foreach (CodeListType CodeList in Structure.CodeLists)
        {
            if (CodeList.id == FreqCodelistId)
            {
                ListFrequency = Get_CodeValues_From_CodeList(CodeList);
              
            }
            if (CodeList.id == IndicatorCodelistId)
            {
                ListIndicator = Get_CodeValues_From_CodeList(CodeList);
               
            }
            if (CodeList.id == UnitCodelistId)
            {
                ListUnit = Get_CodeValues_From_CodeList(CodeList);
               
            }
            if (CodeList.id == AgeCodelistId)
            {
                ListAge = Get_CodeValues_From_CodeList(CodeList);
             
            }
            if (CodeList.id == SexCodelistId)
            {
                ListSex = Get_CodeValues_From_CodeList(CodeList);
               
            }
            if (CodeList.id == LocationCodelistId)
            {
                ListLocation = Get_CodeValues_From_CodeList(CodeList);
              
            }
            if (CodeList.id == AreaCodelistId)
            {
                ListArea = Get_CodeValues_From_CodeList(CodeList);
               
            }
            if (CodeList.id == SourceTypeCodelistId)
            {
                ListSourceType = Get_CodeValues_From_CodeList(CodeList);
            }
            if (CodeList.id == NatureCodelistId)
            {
                ListNature = Get_CodeValues_From_CodeList(CodeList);
             
            }
            if (CodeList.id == UnitMultCodelistId)
            {
                ListUnitMult = Get_CodeValues_From_CodeList(CodeList);
               
            }
            //switch (CodeList.id)
            //{
            //    case Constants.UNSD.CodeList.Frequency.Id:
            //        ListFrequency = Get_CodeValues_From_CodeList(CodeList);
            //        break;
            //    case Constants.UNSD.CodeList.Indicator.Id:
            //        ListIndicator = Get_CodeValues_From_CodeList(CodeList);
            //        break;
            //    case Constants.UNSD.CodeList.Unit.Id:
            //        ListUnit = Get_CodeValues_From_CodeList(CodeList);
            //        break;
            //    case Constants.UNSD.CodeList.Location.Id:
            //        ListLocation = Get_CodeValues_From_CodeList(CodeList);
            //        break;
            //    case Constants.UNSD.CodeList.Age.Id:
            //        ListAge = Get_CodeValues_From_CodeList(CodeList);
            //        break;
            //    case Constants.UNSD.CodeList.Sex.Id:
            //        ListSex = Get_CodeValues_From_CodeList(CodeList);
            //        break;
            //    case Constants.UNSD.CodeList.Area.Id:
            //        ListArea = Get_CodeValues_From_CodeList(CodeList);
            //        break;
            //    case Constants.UNSD.CodeList.SourceType.Id:
            //        ListSourceType = Get_CodeValues_From_CodeList(CodeList);
            //        break;
            //    case Constants.UNSD.CodeList.Nature.Id:
            //        ListNature = Get_CodeValues_From_CodeList(CodeList);
            //        break;
            //    case Constants.UNSD.CodeList.UnitMult.Id:
            //        ListUnitMult = Get_CodeValues_From_CodeList(CodeList);
            //        break;
            //    default:
            //        break;
            //     }
        }

    }

    private static List<string> Get_CodeValues_From_CodeList(CodeListType CodeList)
    {
        List<string> RetVal;

        RetVal = new List<string>();
        foreach (CodeType Code in CodeList.Code)
        {
            RetVal.Add(Code.value);
        }

        return RetVal;
    }

    #endregion "--Validation Data--"

    #region "--Validation Metadata--"

    private static Dictionary<string, string> Validate_And_Load_Metadatafile(string metadataFileNameWPath, out GenericMetadataType Metadata)
    {
        Dictionary<string, string> RetVal;
        XmlDocument Document;

        RetVal = new Dictionary<string, string>();
        Metadata = null;
        Document = null;

        try
        {
            RetVal = Validate_XML(metadataFileNameWPath, out Document);

            if (RetVal.Keys.Count == 0)
            {
                RetVal = Validate_SDMX(Document.InnerXml);
            }

            if (RetVal.Keys.Count == 0)
            {
                Metadata = (GenericMetadataType)(Deserializer.LoadFromText(typeof(GenericMetadataType), Document.InnerXml));
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            if (ex is System.InvalidOperationException)
            {
                RetVal.Add(MetadataValidationStatus.SDMX_Invalid.ToString(), ex.InnerException.Message.ToString());
            }
            else
            {
                throw ex;
            }
        }

        return RetVal;
    }

    private static Dictionary<string, string> Validate_XML(string metadataFileNameWPath, out XmlDocument Document)
    {
        Dictionary<string, string> RetVal;

        RetVal = new Dictionary<string, string>();
        Document = new XmlDocument();

        try
        {
            Document.Load(metadataFileNameWPath);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            if ((ex.InnerException is System.Xml.XmlException) || (ex is System.Xml.XmlException))
            {
                RetVal.Add(MetadataValidationStatus.Xml_Invalid.ToString(), ex.Message.ToString());
            }
            else
            {
                throw ex;
            }
        }

        return RetVal;
    }

    private static Dictionary<string, string> Validate_SDMX(string XmlContent)
    {
        Dictionary<string, string> RetVal;
        XmlReader Reader;
        XmlReaderSettings ReaderSettings;

        RetVal = new Dictionary<string, string>();
        Reader = null;
        ReaderSettings = new XmlReaderSettings();
        try
        {
            Load_SDMX_Schemas(ReaderSettings.Schemas);

            ReaderSettings.ValidationType = ValidationType.Schema;
            ReaderSettings.ValidationFlags = XmlSchemaValidationFlags.AllowXmlAttributes | XmlSchemaValidationFlags.None;

            Reader = XmlReader.Create(new StringReader(XmlContent), ReaderSettings);

            while (Reader.Read())
            {
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            if (ex is System.Xml.Schema.XmlSchemaValidationException)
            {
                RetVal.Add(MetadataValidationStatus.SDMX_Invalid.ToString(), ex.Message.ToString());
            }
            else
            {
                throw ex;
            }
        }

        return RetVal;
    }

    private static Dictionary<string, string> Validate_MSD_Compliance(GenericMetadataType Metadata, string completeFileNameWPath, string TargetAreaId)
    {
        Dictionary<string, string> RetVal;
        SDMXApi_2_0.Message.StructureType Complete;
        RetVal = new Dictionary<string, string>();

        try
        {
            Complete = (SDMXApi_2_0.Message.StructureType)(Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), completeFileNameWPath));

            RetVal = Referred_MSD_Validation(Metadata, Complete);

            if (RetVal.Keys.Count == 0)
            {
                RetVal = Referred_ReportRef_Validation(Metadata, Complete);

                if (RetVal.Keys.Count == 0)
                {
                    RetVal = Referred_Target_Validation(Metadata, Complete, TargetAreaId);

                    if (RetVal.Keys.Count == 0)
                    {
                        RetVal = Report_Structure_Validation(Metadata, Complete);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        return RetVal;

    }

    private static Dictionary<string, string> Referred_MSD_Validation(GenericMetadataType GenericMetadata, StructureType Complete)
    {
        Dictionary<string, string> RetVal;
        RetVal = new Dictionary<string, string>();

        try
        {
            if (Complete.MetadataStructureDefinitions[0].id != GenericMetadata.MetadataSet.MetadataStructureRef ||
                Complete.MetadataStructureDefinitions[0].agencyID != GenericMetadata.MetadataSet.MetadataStructureAgencyRef)
            {
                RetVal.Add(MetadataValidationStatus.Referred_MSD_Invalid.ToString(), DevInfo.Lib.DI_LibSDMX.Constants.ValidationMessages.Invalid_Referred_MSD_Id + GenericMetadata.MetadataSet.MetadataStructureRef + " AgencyId = " + GenericMetadata.MetadataSet.MetadataStructureAgencyRef);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }

        return RetVal;
    }

    private static Dictionary<string, string> Referred_ReportRef_Validation(GenericMetadataType GenericMetadata, StructureType Complete)
    {
        Dictionary<string, string> RetVal;
        RetVal = new Dictionary<string, string>();

        try
        {
            if (Complete.MetadataStructureDefinitions[0].ReportStructure[0].id != GenericMetadata.MetadataSet.ReportRef)
            {
                RetVal.Add(MetadataValidationStatus.Metadata_Report_Invalid.ToString(), DevInfo.Lib.DI_LibSDMX.Constants.ValidationMessages.Invalid_Metadata_Report + GenericMetadata.MetadataSet.ReportRef);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }

        return RetVal;
    }

    private static Dictionary<string, string> Referred_Target_Validation(GenericMetadataType GenericMetadata, StructureType Complete, string TargetAreaId)
    {
        Dictionary<string, string> RetVal;
        List<string> IndicatorIDs;
        RetVal = new Dictionary<string, string>();

        try
        {
            IndicatorIDs = Get_Indicator_GIds(Complete);

            if (GenericMetadata.MetadataSet.AttributeValueSet != null && GenericMetadata.MetadataSet.AttributeValueSet.Count > 0)
            {
                if (Complete.MetadataStructureDefinitions[0].ReportStructure[0].target != GenericMetadata.MetadataSet.AttributeValueSet[0].TargetRef)
                {
                    RetVal.Add(MetadataValidationStatus.Metadata_Target_Invalid.ToString(), DevInfo.Lib.DI_LibSDMX.Constants.ValidationMessages.Invalid_Metadata_Target + GenericMetadata.MetadataSet.AttributeValueSet[0].TargetRef);
                }

                if (GenericMetadata.MetadataSet.AttributeValueSet[0].TargetValues != null && GenericMetadata.MetadataSet.AttributeValueSet[0].TargetValues.Count > 1)
                {
                    if (GenericMetadata.MetadataSet.AttributeValueSet[0].TargetValues[0].@object != SDMXApi_2_0.GenericMetadata.ObjectIDType.Dimension ||
                    GenericMetadata.MetadataSet.AttributeValueSet[0].TargetValues[0].component != "SERIES" ||
                    !IndicatorIDs.Contains(GenericMetadata.MetadataSet.AttributeValueSet[0].TargetValues[0].Value) ||
                    GenericMetadata.MetadataSet.AttributeValueSet[0].TargetValues[1].@object != SDMXApi_2_0.GenericMetadata.ObjectIDType.Dimension ||
                    GenericMetadata.MetadataSet.AttributeValueSet[0].TargetValues[1].component != "REF_AREA" ||
                    GenericMetadata.MetadataSet.AttributeValueSet[0].TargetValues[1].Value != TargetAreaId)
                    {
                        RetVal.Add(MetadataValidationStatus.Metadata_Target_Object_Reference_Invalid.ToString(), DevInfo.Lib.DI_LibSDMX.Constants.ValidationMessages.Invalid_Metadata_Target_Object_Reference);
                    }
                }
                else
                {
                    RetVal.Add(MetadataValidationStatus.Metadata_Target_Object_Reference_Invalid.ToString(), DevInfo.Lib.DI_LibSDMX.Constants.ValidationMessages.Invalid_Metadata_Target_Object_Reference);
                }
            }
            else
            {
                RetVal.Add(MetadataValidationStatus.Metadata_Report_Structure_Invalid.ToString(), DevInfo.Lib.DI_LibSDMX.Constants.ValidationMessages.Invalid_Report_Structure);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }

        return RetVal;
    }

    private static Dictionary<string, string> Report_Structure_Validation(GenericMetadataType GenericMetadata, StructureType Complete)
    {
        Dictionary<string, string> RetVal;
        RetVal = new Dictionary<string, string>();

        try
        {
            if (Complete.MetadataStructureDefinitions[0].ReportStructure[0].MetadataAttribute.Count == GenericMetadata.MetadataSet.AttributeValueSet[0].ReportedAttribute.Count)
            {
                for (int i = 0; i < Complete.MetadataStructureDefinitions[0].ReportStructure[0].MetadataAttribute.Count; i++)
                {
                    if (RetVal.Keys.Count == 0)
                    {
                        if (Complete.MetadataStructureDefinitions[0].ReportStructure[0].MetadataAttribute[i].conceptRef != GenericMetadata.MetadataSet.AttributeValueSet[0].ReportedAttribute[i].conceptID)
                        {
                            RetVal.Add(MetadataValidationStatus.Metadata_Report_Structure_Invalid.ToString(), DevInfo.Lib.DI_LibSDMX.Constants.ValidationMessages.Invalid_Metadata_Reported_Attribute + GenericMetadata.MetadataSet.AttributeValueSet[0].ReportedAttribute[i].conceptID);
                        }
                        else
                        {
                            RetVal = ReportedAttribute_Validation(Complete.MetadataStructureDefinitions[0].ReportStructure[0].MetadataAttribute[i], GenericMetadata.MetadataSet.AttributeValueSet[0].ReportedAttribute[i]);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                RetVal.Add(MetadataValidationStatus.Metadata_Report_Structure_Invalid.ToString(), DevInfo.Lib.DI_LibSDMX.Constants.ValidationMessages.Invalid_Report_Structure);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }

        return RetVal;
    }

    private static Dictionary<string, string> ReportedAttribute_Validation(MetadataAttributeType MetadataAttribute, ReportedAttributeType ReportedAttribute)
    {
        Dictionary<string, string> RetVal;
        RetVal = new Dictionary<string, string>();

        try
        {
            if (MetadataAttribute.MetadataAttribute.Count == ReportedAttribute.ReportedAttribute.Count)
            {
                for (int i = 0; i < MetadataAttribute.MetadataAttribute.Count; i++)
                {
                    if (RetVal.Keys.Count == 0)
                    {
                        if (MetadataAttribute.MetadataAttribute[i].conceptRef != ReportedAttribute.ReportedAttribute[i].conceptID)
                        {
                            RetVal.Add(MetadataValidationStatus.Metadata_Report_Structure_Invalid.ToString(), DevInfo.Lib.DI_LibSDMX.Constants.ValidationMessages.Invalid_Metadata_Reported_Attribute + ReportedAttribute.ReportedAttribute[i].conceptID);
                        }
                        else
                        {
                            RetVal = ReportedAttribute_Validation(MetadataAttribute.MetadataAttribute[i], ReportedAttribute.ReportedAttribute[i]);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                RetVal.Add(MetadataValidationStatus.Metadata_Report_Structure_Invalid.ToString(), DevInfo.Lib.DI_LibSDMX.Constants.ValidationMessages.Invalid_Report_Structure);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }

        return RetVal;
    }

    #endregion "--Validation Metadata--"

    private static void Load_SDMX_Schemas(XmlSchemaSet Schemas)
    {
        Schemas.Add(null, XmlReader.Create(new StringReader(SDMXApi_2_0.Schemas.xml)));
        Schemas.Add(null, XmlReader.Create(new StringReader(SDMXApi_2_0.Schemas.SDMXCommon)));
        Schemas.Add(null, XmlReader.Create(new StringReader(SDMXApi_2_0.Schemas.SDMXMessage)));
        Schemas.Add(null, XmlReader.Create(new StringReader(SDMXApi_2_0.Schemas.SDMXStructure)));
        Schemas.Add(null, XmlReader.Create(new StringReader(SDMXApi_2_0.Schemas.SDMXGenericData)));
        Schemas.Add(null, XmlReader.Create(new StringReader(SDMXApi_2_0.Schemas.SDMXUtilityData)));
        Schemas.Add(null, XmlReader.Create(new StringReader(SDMXApi_2_0.Schemas.SDMXCompactData)));
        Schemas.Add(null, XmlReader.Create(new StringReader(SDMXApi_2_0.Schemas.SDMXCrossSectionalData)));
        Schemas.Add(null, XmlReader.Create(new StringReader(SDMXApi_2_0.Schemas.SDMXGenericMetadata)));
        Schemas.Add(null, XmlReader.Create(new StringReader(SDMXApi_2_0.Schemas.SDMXMetadataReport)));
        Schemas.Add(null, XmlReader.Create(new StringReader(SDMXApi_2_0.Schemas.SDMXRegistry)));
        Schemas.Add(null, XmlReader.Create(new StringReader(SDMXApi_2_0.Schemas.SDMXQuery)));
    }

    private static List<string> Get_Indicator_GIds(StructureType Complete)
    {
        List<string> RetVal;

        RetVal = new List<string>();

        foreach (CodeListType Codelist in Complete.CodeLists)
        {
            if (Codelist.id == "CL_SERIES_COUNTRY_DATA")
            {
                foreach (CodeType Code in Codelist.Code)
                {
                    RetVal.Add(Code.value);
                }
            }
        }

        return RetVal;
    }

    private static bool IsIndicatorIUSMapped(string SourceIndicatorGId, string IUSMappingFileWPath)
    {
        bool RetVal;
        XmlDocument IUSMapping;

        RetVal = false;
        IUSMapping = new XmlDocument();

        try
        {
            if (File.Exists(IUSMappingFileWPath))
            {
                IUSMapping.Load(IUSMappingFileWPath);

                foreach (XmlNode Mapping in IUSMapping.GetElementsByTagName("Mapping"))
                {
                    if (Mapping.Attributes["IUS"].Value.Split(new string[] { "@__@" }, StringSplitOptions.None)[0] == SourceIndicatorGId)
                    {
                        RetVal = true;
                        break;
                    }
                }
            }
            else
            {
                RetVal = true;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
        }

        return RetVal;
    }

    #endregion "--Private--"

    #region "--Public--"

    public static Dictionary<string, string> Validate_SDMXML_File_For_Version_2_0(string dataFileNameWPath, string dsdFileNameWPath)
    {
        Dictionary<string, string> RetVal;
        CompactDataType Data;

        //1. Loading Data object from datafile on successful validation against SDMX schemas.
        RetVal = Validate_And_Load_Datafile(dataFileNameWPath, out Data);

        if (RetVal.Keys.Count == 0 && Data != null && !string.IsNullOrEmpty(dsdFileNameWPath))
        {
            //2. Validating the loaded CompactData object against the dsd file. 
            RetVal = Validate_DSD_Compliance(Data, dsdFileNameWPath);
        }
        if (RetVal.Keys.Count == 0)
        {
            RetVal.Add(SDMXValidationStatus.Valid.ToString(), string.Empty);
        }
        return RetVal;
    }

    public static Dictionary<string, string> Validate_MetadataReport_For_Version_2_0(string metadataFileNameWPath, string completeFileNameWPath, string TargetAreaId)
    {
        Dictionary<string, string> RetVal;
        GenericMetadataType Metadata;

        RetVal = Validate_And_Load_Metadatafile(metadataFileNameWPath, out Metadata);

        if (RetVal.Keys.Count == 0 && Metadata != null && !string.IsNullOrEmpty(completeFileNameWPath))
        {
            RetVal = Validate_MSD_Compliance(Metadata, completeFileNameWPath, TargetAreaId);
        }

        if (RetVal.Keys.Count == 0)
        {
            RetVal.Add(MetadataValidationStatus.Valid.ToString(), string.Empty);
        }

        return RetVal;
    }

    internal static SDMXApi_2_0.Message.HeaderType Get_Appropriate_Header()
    {
        SDMXApi_2_0.Message.HeaderType RetVal;
        SDMXApi_2_0.Common.TextType ObjectValue;
        SDMXApi_2_0.Message.PartyType SenderParty;
        SDMXApi_2_0.Message.PartyType ReceiverParty;
        SDMXApi_2_0.Message.ContactType SenderContact;
        SDMXApi_2_0.Message.ContactType ReceiverContact;

        RetVal = new SDMXApi_2_0.Message.HeaderType();
        RetVal.ID = DevInfo.Lib.DI_LibSDMX.Constants.Header.Id;
        RetVal.Name = new List<SDMXApi_2_0.Common.TextType>();
        RetVal.Test = true;
        ObjectValue = new SDMXApi_2_0.Common.TextType();
        ObjectValue.lang = "en";
        ObjectValue.Value = "test";
        RetVal.Name.Add(ObjectValue);
        RetVal.Prepared = DateTime.Now.ToString(SDMXLibrary.Constants.DateFormat) + SDMXObjectModel.Constants.DateTimeSeparator + DateTime.Now.ToString(SDMXLibrary.Constants.TimeFormat);

        // Sender 
        RetVal.Sender = new List<SDMXApi_2_0.Message.PartyType>();
        SenderParty = new SDMXApi_2_0.Message.PartyType();
        SenderParty.id = DevInfo.Lib.DI_LibSDMX.Constants.Header.SenderId;
        SenderParty.Name = new List<SDMXApi_2_0.Common.TextType>();
        ObjectValue = new SDMXApi_2_0.Common.TextType();
        ObjectValue.lang = "en";
        ObjectValue.Value = DevInfo.Lib.DI_LibSDMX.Constants.Header.SenderName;
        SenderParty.Name.Add(ObjectValue);
        // Sender Contact
        SenderParty.Contact = new List<SDMXApi_2_0.Message.ContactType>();
        SenderContact = new SDMXApi_2_0.Message.ContactType();
        SenderContact.Name = new List<SDMXApi_2_0.Common.TextType>();
        ObjectValue = new SDMXApi_2_0.Common.TextType();
        ObjectValue.lang = "en";
        ObjectValue.Value = DevInfo.Lib.DI_LibSDMX.Constants.Header.SenderName;
        SenderContact.Name.Add(ObjectValue);
        SenderContact.Department = new List<SDMXApi_2_0.Common.TextType>();
        ObjectValue = new SDMXApi_2_0.Common.TextType();
        ObjectValue.lang = "en";
        ObjectValue.Value = DevInfo.Lib.DI_LibSDMX.Constants.Header.SenderDepartment;
        SenderContact.Department.Add(ObjectValue);
        SenderParty.Contact.Add(SenderContact);

        RetVal.Sender.Add(SenderParty);

        //Receiver

        RetVal.Receiver = new List<SDMXApi_2_0.Message.PartyType>();
        ReceiverParty = new SDMXApi_2_0.Message.PartyType();

        ReceiverParty.id = DevInfo.Lib.DI_LibSDMX.Constants.Header.ReceiverId;
        ReceiverParty.Name = new List<SDMXApi_2_0.Common.TextType>();
        ObjectValue = new SDMXApi_2_0.Common.TextType();
        ObjectValue.lang = "en";
        ObjectValue.Value = DevInfo.Lib.DI_LibSDMX.Constants.Header.ReceiverName;
        ReceiverParty.Name.Add(ObjectValue);
        // Sender Contact
        ReceiverParty.Contact = new List<SDMXApi_2_0.Message.ContactType>();
        ReceiverContact = new SDMXApi_2_0.Message.ContactType();
        ReceiverContact.Name = new List<SDMXApi_2_0.Common.TextType>();
        ObjectValue = new SDMXApi_2_0.Common.TextType();
        ObjectValue.lang = "en";
        ObjectValue.Value = DevInfo.Lib.DI_LibSDMX.Constants.Header.ReceiverName;
        ReceiverContact.Name.Add(ObjectValue);
        ReceiverContact.Department = new List<SDMXApi_2_0.Common.TextType>();
        ObjectValue = new SDMXApi_2_0.Common.TextType();
        ObjectValue.lang = "en";
        ObjectValue.Value = DevInfo.Lib.DI_LibSDMX.Constants.Header.ReceiverDepartment;
        ReceiverContact.Department.Add(ObjectValue);
        ReceiverParty.Contact.Add(ReceiverContact);

        RetVal.Receiver.Add(ReceiverParty);



        return RetVal;
    }


    #region "--Indicator--"
    internal static DataTable GetIndicatorTable(int DBNId, string IndicatorNIds)
    {
        DataTable RetVal;
        DIConnection DIConnection;
        DIQueries DIQueries;

        RetVal = null;
        DIConnection = null;
        DIQueries = null;

        try
        {
            DIConnection = Global.GetDbConnection(DBNId);
            DIQueries = new DIQueries(DIConnection.DIDataSetDefault(), DIConnection.DILanguageCodeDefault(DIConnection.DIDataSetDefault()));

            if (!string.IsNullOrEmpty(IndicatorNIds))
            {
                RetVal = DIConnection.ExecuteDataTable(DIQueries.Indicators.GetIndicator(FilterFieldType.NId, IndicatorNIds, FieldSelection.Light));
            }
            else
            {
                RetVal = DIConnection.ExecuteDataTable(DIQueries.Indicators.GetIndicator(FilterFieldType.None, string.Empty, FieldSelection.Light));
            }
            
            RetVal = RetVal.DefaultView.ToTable(true, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId,
                            DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorName);
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

    internal static Dictionary<string, string> Get_Indicator_GIds(int DBOrDSDDBId)
    {
        Dictionary<string, string> RetVal;
        string CompleteFileWPath;

        RetVal = new Dictionary<string, string>();
        CompleteFileWPath = string.Empty;

        try
        {
            CompleteFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId.ToString() + "\\sdmx\\Complete.xml");
            RetVal = Get_Indicator_GIds(CompleteFileWPath);
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

    internal static Dictionary<string, string> Get_Indicator_GIds(string CompleteFileWPath)
    {
        Dictionary<string, string> RetVal;
        SDMXApi_2_0.Message.StructureType Structure;
        string IndicatorCodelistId = string.Empty;
        RetVal = new Dictionary<string, string>();
        Structure = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), CompleteFileWPath);
        foreach (SDMXApi_2_0.Structure.DimensionType Dimensions in Structure.KeyFamilies[0].Components.Dimension)
        {
            if (Dimensions.conceptRef == Constants.UNSD.Concept.Indicator.Id)
            {
                IndicatorCodelistId = Dimensions.codelist;
            }
        }
        foreach (SDMXApi_2_0.Structure.CodeListType Codelist in Structure.CodeLists)
        {
            if (Codelist.id == IndicatorCodelistId)//"CL_SERIES_COUNTRY_DATA"
            {
                foreach (SDMXApi_2_0.Structure.CodeType Code in Codelist.Code)
                {
                    RetVal.Add(Code.value, Code.Description[0].Value);
                }
            }
        }

        return RetVal;
    }

    internal static Dictionary<string, string> Get_Indicator_Mapping_Dict(int DBOrDSDDBId)
    {
        Dictionary<string, string> RetVal;
        string CodelistMappingFileWPath, IUSMappingFileWPath;

        RetVal = new Dictionary<string, string>();
        CodelistMappingFileWPath = string.Empty;
        IUSMappingFileWPath = string.Empty;

        try
        {
            CodelistMappingFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId.ToString() + "\\" + Constants.FolderName.SDMX.Mapping + DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.FileName);
            IUSMappingFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId.ToString() + "\\" + Constants.FolderName.SDMX.Mapping + "IUSMapping.xml");
            RetVal = Get_Indicator_Mapping_Dict(CodelistMappingFileWPath, IUSMappingFileWPath);
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

    internal static Dictionary<string, string> Get_Indicator_Mapping_Dict(string CodelistMappingFileName, string IUSMappingFileWPath)
    {
        Dictionary<string, string> RetVal;
        SDMXObjectModel.Message.StructureType Structure;

        RetVal = new Dictionary<string, string>();
        Structure = null;

        try
        {
            if (File.Exists(CodelistMappingFileName))
            {
                Structure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), CodelistMappingFileName);

                foreach (SDMXObjectModel.Structure.CodelistMapType CodelistMap in Structure.Structures.StructureSets[0].Items)
                {
                    if (CodelistMap.id == DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Indicator.id)
                    {
                        foreach (SDMXObjectModel.Structure.CodeMapType CodeMap in CodelistMap.Items)
                        {
                            if (IsIndicatorIUSMapped(((SDMXObjectModel.Common.LocalCodeRefType)(CodeMap.Source.Items[0])).id, IUSMappingFileWPath))
                            {
                                RetVal.Add(((SDMXObjectModel.Common.LocalCodeRefType)(CodeMap.Source.Items[0])).id, ((SDMXObjectModel.Common.LocalCodeRefType)(CodeMap.Target.Items[0])).id);
                            }
                        }

                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
        }

        return RetVal;
    }
    #endregion "--Indicator--"



    #region "--Area--"
   
    internal static Dictionary<string, string> Get_Area_Ids(int DBOrDSDDBId)
    {
        Dictionary<string, string> RetVal;
        string CompleteFileWPath;

        RetVal = new Dictionary<string, string>();
        CompleteFileWPath = string.Empty;

        try
        {
            CompleteFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId.ToString() + "\\sdmx\\Complete.xml");
            RetVal = Get_Area_Ids(CompleteFileWPath);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
        }

        return RetVal;
    }

    internal static Dictionary<string, string> Get_Area_Ids(string CompleteFileWPath)
    {
        Dictionary<string, string> RetVal;
        SDMXApi_2_0.Message.StructureType Structure;

        RetVal = new Dictionary<string, string>();
        Structure = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), CompleteFileWPath);

        foreach (SDMXApi_2_0.Structure.CodeListType Codelist in Structure.CodeLists)
        {
            if (Codelist.id == "CL_REF_AREA_MDG")
            {
                foreach (SDMXApi_2_0.Structure.CodeType Code in Codelist.Code)
                {
                    RetVal.Add(Code.value, Code.Description[0].Value);
                }
            }
        }

        return RetVal;
    }

    internal static Dictionary<string, string> Get_Area_Mapping_Dict(int DBOrDSDDBId)
    {
        Dictionary<string, string> RetVal;
        string CodelistMappingFileWPath, IUSMappingFileWPath;

        RetVal = new Dictionary<string, string>();
        CodelistMappingFileWPath = string.Empty;
        IUSMappingFileWPath = string.Empty;

        try
        {
            CodelistMappingFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBOrDSDDBId.ToString() + "\\" + Constants.FolderName.SDMX.Mapping + DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.FileName);

            RetVal = Get_Area_Mapping_Dict(CodelistMappingFileWPath);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
        }

        return RetVal;
    }

    internal static Dictionary<string, string> Get_Area_Mapping_Dict(string CodelistMappingFileName)
    {
        Dictionary<string, string> RetVal;
        SDMXObjectModel.Message.StructureType Structure;

        RetVal = new Dictionary<string, string>();
        Structure = null;

        try
        {
            if (File.Exists(CodelistMappingFileName))
            {
                Structure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), CodelistMappingFileName);

                foreach (SDMXObjectModel.Structure.CodelistMapType CodelistMap in Structure.Structures.StructureSets[0].Items)
                {
                    if (CodelistMap.id == DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Area.id)
                    {
                        foreach (SDMXObjectModel.Structure.CodeMapType CodeMap in CodelistMap.Items)
                        {
                           
                                RetVal.Add(((SDMXObjectModel.Common.LocalCodeRefType)(CodeMap.Source.Items[0])).id, ((SDMXObjectModel.Common.LocalCodeRefType)(CodeMap.Target.Items[0])).id);
                           
                        }

                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
        }

        return RetVal;
    }
    #endregion "--Area--"



    #endregion "--Public--"

    #endregion "--Methods--"
}