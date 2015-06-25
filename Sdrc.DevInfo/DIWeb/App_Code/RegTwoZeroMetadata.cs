using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Data;
using System.IO;
using SDMXApi_2_0.GenericMetadata;
using SDMXApi_2_0.Structure;
using System.Text.RegularExpressions;
using System.Xml;
using System.Configuration;
public static class RegTwoZeroMetadata
{
    internal static bool Generate_MetadataReport(string SummaryFileName, string CodelistMappingFileName, string MetadataMappingFileName, string IndicatorNIds, string TargetAreaId, DIConnection DIConnection, DIQueries DIQueries, string OutputFolder, out string ErrorMessage, out List<string> GeneratedMetadataFiles, string HeaderfilePath, string xmlMetaFilePath)
    {
        bool RetVal;
        SDMXApi_2_0.Message.StructureType Summary;
        SDMXApi_2_0.Message.GenericMetadataType GenericMetadata;
        string MetadataSetName, MetadataStructureRef, MetadataStructureAgencyRef, ReportRef, TargetRef;
        string IndicatorNId, IndicatorGId,AreaId;
        Dictionary<string, string> DictIndicatorMapping, DictMetadataMapping,DictAreaMapping;
        DataTable DtIndicator,DtArea;
        ReportedAttributeType ReportedAttribute;
        DateTime CurrentTime;
        ErrorMessage = string.Empty;
        RetVal = false;
        CurrentTime = DateTime.Now;
        GeneratedMetadataFiles = new List<string>();

        XmlDocument UploadedHeaderXml = new XmlDocument();
        string AppSettingFile = string.Empty;
        XmlDocument XmlDoc;
        SDMXApi_2_0.Message.StructureType UploadedDSDStructure = new SDMXApi_2_0.Message.StructureType();
        SDMXApi_2_0.Message.HeaderType Header = new SDMXApi_2_0.Message.HeaderType();
        DataSet ds = new DataSet();
        Callback objCallBack = new Callback();
        AppSettingFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.AppSettingFile]);
        if (File.Exists(HeaderfilePath))
        {
            UploadedHeaderXml.Load(HeaderfilePath);
            UploadedDSDStructure = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromXmlDocument(typeof(SDMXApi_2_0.Message.StructureType), UploadedHeaderXml);
            Header = UploadedDSDStructure.Header;
        }
        if (File.Exists(xmlMetaFilePath))
        {
            ds.ReadXml(xmlMetaFilePath);

        }
        try
        {
            Summary = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromFile(typeof(SDMXApi_2_0.Message.StructureType), SummaryFileName);
            MetadataSetName = "CountryData Metadata";

            if (Summary.MetadataStructureDefinitions.Count > 0)
            {
                MetadataStructureRef = Summary.MetadataStructureDefinitions[0].id;
                MetadataStructureAgencyRef = Summary.MetadataStructureDefinitions[0].agencyID;
                ReportRef = Summary.MetadataStructureDefinitions[0].ReportStructure[0].id;
                TargetRef = Summary.MetadataStructureDefinitions[0].ReportStructure[0].target;

                DtIndicator = DIConnection.ExecuteDataTable(DIQueries.Indicators.GetIndicator(FilterFieldType.NId, IndicatorNIds, FieldSelection.Light));
                DtIndicator = DtIndicator.DefaultView.ToTable(true, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId,
                              DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId);

                DtArea = DIConnection.ExecuteDataTable(DIQueries.Area.GetAreaByAreaLevel(Global.registryAreaLevel));
                DtArea = DtArea.DefaultView.ToTable(true, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaID,
                            DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaGId);

                DictIndicatorMapping = RegTwoZeroFunctionality.Get_Indicator_Mapping_Dict(CodelistMappingFileName, string.Empty);
                DictAreaMapping = RegTwoZeroFunctionality.Get_Area_Mapping_Dict(CodelistMappingFileName);
                foreach (DataRow DrArea in DtArea.Rows)
                {
                    AreaId = DrArea[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaID].ToString();
                    if (DictAreaMapping.ContainsKey(AreaId))
                    {
                        if (DictAreaMapping.Count == 1)
                        {
                            TargetAreaId = DictAreaMapping[AreaId].ToString();//DrArea[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaID].ToString();
                        }
                    }
                }

                if (TargetAreaId != Global.registryMSDAreaId)
                {
                    XmlDoc = new XmlDocument();
                    XmlDoc.Load(AppSettingFile);
                    objCallBack.SaveAppSettingValue(XmlDoc, Constants.AppSettingKeys.registryMSDAreaId, TargetAreaId);
                    XmlDoc.Save(AppSettingFile);
                }


                if (File.Exists(MetadataMappingFileName))
                {
                    DictMetadataMapping = Get_Metadata_Mapping_Dict(MetadataMappingFileName);
                    foreach (DataRow DrIndicator in DtIndicator.Rows)
                    {
                        IndicatorNId = DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId].ToString();
                        IndicatorGId = DrIndicator[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId].ToString();


                        if (DictIndicatorMapping.ContainsKey(IndicatorGId))
                        {
                            if (File.Exists(xmlMetaFilePath))
                            {
                                foreach (DataRow DSRow in ds.Tables["Data"].Select("Ind=" + IndicatorNId))
                                {

                                    IndicatorNId = DSRow["Ind"].ToString();

                                }
                            }
                            GenericMetadata = new SDMXApi_2_0.Message.GenericMetadataType();
                            // GenericMetadata.Header = RegTwoZeroFunctionality.Get_Appropriate_Header();
                            if (!File.Exists(HeaderfilePath))
                            {
                                GenericMetadata.Header = RegTwoZeroFunctionality.Get_Appropriate_Header();
                            }
                            else
                            {
                                GenericMetadata.Header = Header;
                            }

                            GenericMetadata.MetadataSet = new SDMXApi_2_0.GenericMetadata.MetadataSetType();
                            GenericMetadata.MetadataSet.Annotations = null;

                            GenericMetadata.MetadataSet.Name = new List<SDMXApi_2_0.Common.TextType>();
                            GenericMetadata.MetadataSet.Name.Add(new SDMXApi_2_0.Common.TextType());
                            GenericMetadata.MetadataSet.Name[0].Value = MetadataSetName;

                            GenericMetadata.MetadataSet.MetadataStructureRef = MetadataStructureRef;
                            GenericMetadata.MetadataSet.MetadataStructureAgencyRef = MetadataStructureAgencyRef;
                            GenericMetadata.MetadataSet.ReportRef = ReportRef;

                            GenericMetadata.MetadataSet.AttributeValueSet = new List<SDMXApi_2_0.GenericMetadata.AttributeValueSetType>();
                            GenericMetadata.MetadataSet.AttributeValueSet.Add(new SDMXApi_2_0.GenericMetadata.AttributeValueSetType());
                            GenericMetadata.MetadataSet.AttributeValueSet[0].TargetRef = TargetRef;

                            GenericMetadata.MetadataSet.AttributeValueSet[0].TargetValues = new List<SDMXApi_2_0.GenericMetadata.ComponentValueType>();

                            GenericMetadata.MetadataSet.AttributeValueSet[0].TargetValues.Add(new SDMXApi_2_0.GenericMetadata.ComponentValueType());
                            GenericMetadata.MetadataSet.AttributeValueSet[0].TargetValues[0].@object = SDMXApi_2_0.GenericMetadata.ObjectIDType.Dimension;
                            GenericMetadata.MetadataSet.AttributeValueSet[0].TargetValues[0].component = "SERIES";
                            GenericMetadata.MetadataSet.AttributeValueSet[0].TargetValues[0].Value = DictIndicatorMapping[IndicatorGId].ToString();

                            GenericMetadata.MetadataSet.AttributeValueSet[0].TargetValues.Add(new SDMXApi_2_0.GenericMetadata.ComponentValueType());
                            GenericMetadata.MetadataSet.AttributeValueSet[0].TargetValues[1].@object = SDMXApi_2_0.GenericMetadata.ObjectIDType.Dimension;
                            GenericMetadata.MetadataSet.AttributeValueSet[0].TargetValues[1].component = "REF_AREA";
                            GenericMetadata.MetadataSet.AttributeValueSet[0].TargetValues[1].Value = TargetAreaId;

                            if (Summary.MetadataStructureDefinitions[0].ReportStructure[0].MetadataAttribute != null &&
                                Summary.MetadataStructureDefinitions[0].ReportStructure[0].MetadataAttribute.Count > 0)
                            {
                                GenericMetadata.MetadataSet.AttributeValueSet[0].ReportedAttribute = new List<ReportedAttributeType>();

                                foreach (MetadataAttributeType MetadataAttribute in Summary.MetadataStructureDefinitions[0].ReportStructure[0].MetadataAttribute)
                                {
                                    ReportedAttribute = new ReportedAttributeType();
                                    ReportedAttribute.Annotations = null;
                                    ReportedAttribute.conceptID = MetadataAttribute.conceptRef;
                                    Fill_Reported_Attribute_Value(ReportedAttribute, IndicatorNId, DictMetadataMapping, DIConnection, DIQueries);
                                    Fill_Reported_Attribute_ChildAttributes(ReportedAttribute, MetadataAttribute, IndicatorNId, DictMetadataMapping, DIConnection, DIQueries);
                                    GenericMetadata.MetadataSet.AttributeValueSet[0].ReportedAttribute.Add(ReportedAttribute);
                                }
                            }

                            //SDMXApi_2_0.Serializer.SerializeToFile(typeof(SDMXApi_2_0.Message.GenericMetadataType), GenericMetadata, Path.Combine(OutputFolder, DictIndicatorMapping[IndicatorGId].ToString() + "_" + CurrentTime.ToString("yyyy-MM-dd HHmmss") + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension));
                            //GeneratedMetadataFiles.Add(Convert.ToString(DictIndicatorMapping[IndicatorGId].ToString() + "_" + CurrentTime.ToString("yyyy-MM-dd HHmmss")));

                            SDMXApi_2_0.Serializer.SerializeToFile(typeof(SDMXApi_2_0.Message.GenericMetadataType), GenericMetadata, Path.Combine(OutputFolder, DictIndicatorMapping[IndicatorGId].ToString() + "_DIMD_" + IndicatorGId + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension));
                            GeneratedMetadataFiles.Add(Convert.ToString(DictIndicatorMapping[IndicatorGId].ToString() + "_DIMD_" + IndicatorGId));
                            RetVal = true;
                        }
                    }
                }
                else
                {
                    RetVal = false;
                    ErrorMessage = "MNF";
                    return RetVal;

                }

            }
            else
            {
                RetVal = false;
                ErrorMessage = "ANF";
                return RetVal;
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

    private static void Fill_Reported_Attribute_ChildAttributes(ReportedAttributeType ReportedAttribute, MetadataAttributeType MetadataAttribute, string IndicatorNId, Dictionary<string, string> DictMetadataMapping, DIConnection DIConnection, DIQueries DIQueries)
    {
        ReportedAttributeType ChilReportedAttribute;

        if (MetadataAttribute.MetadataAttribute != null && MetadataAttribute.MetadataAttribute.Count > 0)
        {
            ReportedAttribute.ReportedAttribute = new List<ReportedAttributeType>();

            foreach (MetadataAttributeType ChildMetadataAttribute in MetadataAttribute.MetadataAttribute)
            {
                ChilReportedAttribute = new ReportedAttributeType();
                ChilReportedAttribute.Annotations = null;
                ChilReportedAttribute.conceptID = ChildMetadataAttribute.conceptRef;
                Fill_Reported_Attribute_Value(ChilReportedAttribute, IndicatorNId, DictMetadataMapping, DIConnection, DIQueries);
                Fill_Reported_Attribute_ChildAttributes(ChilReportedAttribute, ChildMetadataAttribute, IndicatorNId, DictMetadataMapping, DIConnection, DIQueries);
                ReportedAttribute.ReportedAttribute.Add(ChilReportedAttribute);
            }
        }
    }

    private static void Fill_Reported_Attribute_Value(ReportedAttributeType ReportedAttribute, string IndicatorNId, Dictionary<string, string> DictMetadataMapping, DIConnection DIConnection, DIQueries DIQueries)
    {
        string language, Query;
        string CategoryNId;
        DataTable DtTable;
        SDMXApi_2_0.Common.TextType LanguageSpecificValue;

        if (DictMetadataMapping.ContainsKey(ReportedAttribute.conceptID))
        {
            ReportedAttribute.Value = new List<SDMXApi_2_0.Common.TextType>();

            foreach (DataRow LanguageRow in DIConnection.DILanguages(DIQueries.DataPrefix).Rows)
            {
                language = LanguageRow[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Language.LanguageCode].ToString();
                Query = "SELECT CategoryNId FROM UT_Metadata_Category_" + language + " WHERE CategoryGId = '" + DictMetadataMapping[ReportedAttribute.conceptID].ToString() + "' AND CategoryType = 'I'";
                DtTable = DIConnection.ExecuteDataTable(Regex.Replace(Query, "UT_", DIConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));

                if (DtTable != null && DtTable.Rows.Count > 0)
                {
                    CategoryNId = DtTable.Rows[0]["CategoryNId"].ToString();
                    Query = "SELECT Metadata FROM UT_MetadataReport_" + language + " WHERE Target_NId = " + IndicatorNId + " AND Category_NId = " + CategoryNId;
                    DtTable = DIConnection.ExecuteDataTable(Regex.Replace(Query, "UT_", DIConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));

                    if (DtTable != null && DtTable.Rows.Count > 0)
                    {
                        LanguageSpecificValue = new SDMXApi_2_0.Common.TextType();
                        LanguageSpecificValue.lang = language;
                        LanguageSpecificValue.Value = DtTable.Rows[0]["Metadata"].ToString();

                        ReportedAttribute.Value.Add(LanguageSpecificValue);
                    }
                }
            }
        }
    }

    private static Dictionary<string, string> Get_Metadata_Mapping_Dict(string MetadataMappingFileName)
    {
        Dictionary<string, string> RetVal;
        SDMXObjectModel.Message.StructureType Structure;
        SDMXObjectModel.Structure.ConceptSchemeMapType ConceptSchemeMap;

        RetVal = new Dictionary<string, string>();
        Structure = null;
        ConceptSchemeMap = null;

        try
        {

            Structure = (SDMXObjectModel.Message.StructureType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), MetadataMappingFileName);

            if (Structure != null && Structure.Structures != null && Structure.Structures.StructureSets != null && Structure.Structures.StructureSets.Count > 0 && Structure.Structures.StructureSets[0].id == DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.id)
            {
                if (Structure.Structures.StructureSets[0].Items != null && Structure.Structures.StructureSets[0].Items.Count > 0)
                {
                    ConceptSchemeMap = (SDMXObjectModel.Structure.ConceptSchemeMapType)Structure.Structures.StructureSets[0].Items[0];

                    if (ConceptSchemeMap.Items != null && ConceptSchemeMap.Items.Count > 0 &&
                        ConceptSchemeMap.id == DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.ConceptSchemeMap.MetadataMap.id)
                    {
                        foreach (SDMXObjectModel.Structure.ItemAssociationType ConceptMap in ConceptSchemeMap.Items)
                        {
                            RetVal.Add(((SDMXObjectModel.Common.LocalConceptRefType)((SDMXObjectModel.Common.LocalConceptReferenceType)ConceptMap.Target).Items[0]).id, ((SDMXObjectModel.Common.LocalConceptRefType)((SDMXObjectModel.Common.LocalConceptReferenceType)ConceptMap.Source).Items[0]).id);
                        }
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
}