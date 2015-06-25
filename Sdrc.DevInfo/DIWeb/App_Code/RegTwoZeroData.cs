using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Data;
using System.IO;
using SDMXApi_2_0.Message;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using SDMXLibrary = DevInfo.Lib.DI_LibSDMX;
using System.Xml;
using DevInfo.Lib.DI_LibBAL.Controls.TimeperiodBAL;
using System.Text.RegularExpressions;

public static class RegTwoZeroData
{

    /// <summary>
    /// Generate_Data for Country DSD SDMX-ML v2.0
    /// </summary>
    /// <param name="DictMapping">Key - DevInfo GId; Value - UNSD GID</param>
    /// <param name="OutputFolder">SDMX-ML folder path</param>
    /// <param name="DIConnection"></param>
    /// <param name="DIQueries"></param>
    /// <param name="fileCount">Return Count  of SDMX-ML files generated for all language</param>
    /// <param name="AreaId"></param>
    /// <param name="DBOrDSDDBId">dbnid of devinfo database / country dsd</param>
    /// <param name="GeneratedFiles">list unique SDMX-ML file generated (single file name for multiple language)</param>
    /// <param name="HeaderfilePath"></param>
    /// <param name="xml">In case of optimize database - user selection through databuplish xml</param>
    /// <param name="DuplicateKey">Return knowldege of Duplicate key for indicator and time</param>
    /// <param name="dtSelections">In case of Register - user selection in form of datatable [ind, Area]</param>
    /// <param name="ErrorLogs">Return indicator@@Timeperiod}} </param>
    /// <param name="GeneratedIndicatorCountryGIDS">Return list of Country GUID for which files have been generated</param>
    /// <returns></returns>
    
    internal static bool Generate_Data(Dictionary<string, string> DictMapping, string OutputFolder, DIConnection DIConnection, DIQueries DIQueries, string AreaId, string DBOrDSDDBId, string HeaderfilePath, string xml, DataTable dtSelections, out int fileCount, out List<string> GeneratedFiles, out string ErrorLogs, out string DuplicateKey)
    {
        bool RetVal;
        string Language;
        string IndicatorGId, UnitGId, SGVGId, AreaID;
        string AreaNIds, TPNIds, SourceNIds;
        string SeriesValue, Unit, Age, Sex, Location, Frequency, SourceType, Nature, UnitMult, TimePeriod, ObsVal, TimeDetail, SourceDetail, Footnotes, IndicatorName;
        ErrorLogs = string.Empty;
        List<string> ProcessedIUSA;
        DataTable DtData;
        DataRow[] IUSARows;
        DataRow[] CheckDuplicateKeys;
        XmlDocument AttributesDoc;
        System.Xml.XmlAttribute Attribute;
        CompactDataType CompactData;
        SDMXApi_2_0.CompactData.SeriesType Series;
        SDMXApi_2_0.CompactData.ObsType Obs;
        Dictionary<string, string> DictAreaMapping;
        DateTime CurrentTime;
        RetVal = true;
        fileCount = 0;
        Language = string.Empty;
        IndicatorGId = string.Empty;
        UnitGId = string.Empty;
        SGVGId = string.Empty;
        AreaID = string.Empty;

        SeriesValue = string.Empty;
        Unit = string.Empty;
        Age = string.Empty;
        Sex = string.Empty;
        Location = string.Empty;
        Frequency = string.Empty;
        SourceType = string.Empty;
        Nature = string.Empty;
        UnitMult = string.Empty;
        TimePeriod = string.Empty;
        ObsVal = string.Empty;
        TimeDetail = string.Empty;
        SourceDetail = string.Empty;
        Footnotes = string.Empty;
        int RowCount = 0;
        ProcessedIUSA = new List<string>();
        DtData = null;
        IUSARows = null;
        CheckDuplicateKeys = null;
        AttributesDoc = null;
        Attribute = null;

        CompactData = null;
        Series = null;
        Obs = null;
        DictAreaMapping = null;
        CurrentTime = DateTime.Now;
        IndicatorName = string.Empty;
        XmlDocument UploadedHeaderXml = new XmlDocument();
        DataSet ds = new DataSet();
        SDMXApi_2_0.Message.StructureType UploadedDSDStructure = new SDMXApi_2_0.Message.StructureType();
        SDMXApi_2_0.Message.HeaderType Header = new SDMXApi_2_0.Message.HeaderType();
        DuplicateKey = string.Empty;
        DataTable dtIndicator = new DataTable();
       if (File.Exists(HeaderfilePath))
        {
            UploadedHeaderXml.Load(HeaderfilePath);
            UploadedDSDStructure = (SDMXApi_2_0.Message.StructureType)SDMXApi_2_0.Deserializer.LoadFromXmlDocument(typeof(SDMXApi_2_0.Message.StructureType), UploadedHeaderXml);
            Header = UploadedDSDStructure.Header;
        }
        GeneratedFiles = new List<string>();
        if (dtSelections != null && dtSelections.Rows.Count > 0)
        {
            // register tab
            ds.Tables.Add(dtSelections);
        }
        else
        {
            // Admin 
            if (File.Exists(xml))
            {
                ds.ReadXml(xml);
            }
        }
        string IndicatorNId = string.Empty;

        // for each language in database 
        foreach (DataRow LanguageRow in DIConnection.DILanguages(DIQueries.DataPrefix).Rows)
        {
            //
            Language = LanguageRow[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Language.LanguageCode].ToString();

            int dsRowCount = ds.Tables["Data"].Rows.Count;
            RowCount = 0;

            // consider only those indiactors which are selected 
            foreach (DataRow DSRow in ds.Tables["Data"].Select("selectedState=true"))
            {

                if (RowCount > dsRowCount)
                {
                    break;
                }
                IndicatorNId = DSRow["Ind"].ToString();
                AreaNIds = DSRow["areas"].ToString();
                TPNIds = DSRow["timeperiods"].ToString();
                SourceNIds = DSRow["source"].ToString();



                // Get data based on Indiactor and associated fileters
                DtData = Get_Language_Specific_Data_Table(Language, IndicatorNId, AreaNIds, TPNIds, SourceNIds, DIConnection);
                dtIndicator = DIConnection.ExecuteDataTable(DIQueries.Indicators.GetIndicator(FilterFieldType.NId, IndicatorNId, FieldSelection.Heavy));
                foreach (DataRow drIndicator in dtIndicator.Rows)
                {
                    IndicatorName = drIndicator[Indicator.IndicatorName].ToString();

                }
                ProcessedIUSA = new List<string>();
                DictAreaMapping = RegTwoZeroFunctionality.Get_Area_Mapping_Dict(Convert.ToInt32(DBOrDSDDBId));

                if ((DtData != null) && (DtData.Rows.Count > 0))
                {
                    fileCount += 1;
                    CompactData = new CompactDataType();
                    if (!File.Exists(HeaderfilePath))
                    {
                        CompactData.Header = RegTwoZeroFunctionality.Get_Appropriate_Header();
                    }
                    else
                    {
                        CompactData.Header = Header;

                    }

                    CompactData.DataSet = new SDMXApi_2_0.CompactData.DataSetType();
                    CompactData.DataSet.ListSeries = new List<SDMXApi_2_0.CompactData.SeriesType>();

                    // for each data value for indicator
                    foreach (DataRow DrData in DtData.Rows)
                    {
                        IndicatorGId = DrData[Indicator.IndicatorGId].ToString();
                        UnitGId = DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitGId].ToString();
                        SGVGId = DrData[SubgroupVals.SubgroupValGId].ToString();
                        if (DictAreaMapping.ContainsKey(DrData[Area.AreaID].ToString()))
                        {
                            AreaId = DictAreaMapping[DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaID].ToString()];
                        }
                        else
                        {
                            AreaId = DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaID].ToString();
                        }
                        //  AreaID = DrData[Area.AreaID].ToString();
                        AreaID = AreaId;
                        if (string.IsNullOrEmpty(AreaID) == true)
                        {
                            AreaID = Global.registryMSDAreaId;
                        }
                        if (!DictMapping.ContainsKey(IndicatorGId + "@__@" + UnitGId + "@__@" + SGVGId))
                        {
                            continue;
                        }

                        // Replace DI GUIDs with Coutry data GIDs based on mapping 
                        SeriesValue = DictMapping[IndicatorGId + "@__@" + UnitGId + "@__@" + SGVGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[0];
                        Unit = DictMapping[IndicatorGId + "@__@" + UnitGId + "@__@" + SGVGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[1];
                        Age = DictMapping[IndicatorGId + "@__@" + UnitGId + "@__@" + SGVGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[2];
                        Sex = DictMapping[IndicatorGId + "@__@" + UnitGId + "@__@" + SGVGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[3];
                        Location = DictMapping[IndicatorGId + "@__@" + UnitGId + "@__@" + SGVGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[4];
                        Frequency = DictMapping[IndicatorGId + "@__@" + UnitGId + "@__@" + SGVGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[5];
                        SourceType = DictMapping[IndicatorGId + "@__@" + UnitGId + "@__@" + SGVGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[6];
                        Nature = DictMapping[IndicatorGId + "@__@" + UnitGId + "@__@" + SGVGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[7];
                        UnitMult = DictMapping[IndicatorGId + "@__@" + UnitGId + "@__@" + SGVGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[8];

                        // If series item already exists
                        if (ProcessedIUSA.Contains(IndicatorGId + "@__@" + UnitGId + "@__@" + SGVGId + "@__@" + AreaID))
                        {
                            continue;
                        }
                        else
                        {
                            ProcessedIUSA.Add(IndicatorGId + "@__@" + UnitGId + "@__@" + SGVGId + "@__@" + AreaID);
                            Series = new SDMXApi_2_0.CompactData.SeriesType();
                            Series.AnyAttr = new List<XmlAttribute>();
                            AttributesDoc = new XmlDocument();

                            Attribute = AttributesDoc.CreateAttribute(Constants.UNSD.Concept.Indicator.Id);
                            Attribute.Value = SeriesValue;
                            Series.AnyAttr.Add(Attribute);

                            Attribute = AttributesDoc.CreateAttribute(Constants.UNSD.Concept.Unit.Id);
                            Attribute.Value = Unit;
                            Series.AnyAttr.Add(Attribute);

                            Attribute = AttributesDoc.CreateAttribute(Constants.UNSD.Concept.Age.Id);
                            Attribute.Value = Age;
                            Series.AnyAttr.Add(Attribute);

                            Attribute = AttributesDoc.CreateAttribute(Constants.UNSD.Concept.Sex.Id);
                            Attribute.Value = Sex;
                            Series.AnyAttr.Add(Attribute);

                            Attribute = AttributesDoc.CreateAttribute(Constants.UNSD.Concept.Location.Id);
                            Attribute.Value = Location;
                            Series.AnyAttr.Add(Attribute);

                            Attribute = AttributesDoc.CreateAttribute(Constants.UNSD.Concept.Frequency.Id);
                            Attribute.Value = Frequency;
                            Series.AnyAttr.Add(Attribute);

                            Attribute = AttributesDoc.CreateAttribute(Constants.UNSD.Concept.SourceType.Id);
                            Attribute.Value = SourceType;
                            Series.AnyAttr.Add(Attribute);

                            Attribute = AttributesDoc.CreateAttribute(Constants.UNSD.Concept.Area.Id);
                            Attribute.Value = AreaID;
                            Series.AnyAttr.Add(Attribute);

                            IUSARows = DtData.Select(Indicator.IndicatorGId + SDMXLibrary.Constants.EqualsTo + SDMXLibrary.Constants.Apostophe + IndicatorGId + SDMXLibrary.Constants.Apostophe + SDMXLibrary.Constants.AND + DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitGId + SDMXLibrary.Constants.EqualsTo + SDMXLibrary.Constants.Apostophe + UnitGId + SDMXLibrary.Constants.Apostophe + SDMXLibrary.Constants.AND + SubgroupVals.SubgroupValGId + SDMXLibrary.Constants.EqualsTo + SDMXLibrary.Constants.Apostophe + SGVGId + SDMXLibrary.Constants.Apostophe + SDMXLibrary.Constants.AND + Area.AreaID + SDMXLibrary.Constants.EqualsTo + SDMXLibrary.Constants.Apostophe + DrData[Area.AreaID].ToString() + SDMXLibrary.Constants.Apostophe);

                            Series.ListObs = new List<SDMXApi_2_0.CompactData.ObsType>();

                            foreach (DataRow IUSARow in IUSARows)
                            {
                                TimePeriod = Get_Time_Period_Start_Year(Convert.ToString(IUSARow[Timeperiods.TimePeriod]));
                                CheckDuplicateKeys = DtData.Select(Indicator.IndicatorGId + SDMXLibrary.Constants.EqualsTo + SDMXLibrary.Constants.Apostophe + IndicatorGId + SDMXLibrary.Constants.Apostophe + SDMXLibrary.Constants.AND + DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitGId + SDMXLibrary.Constants.EqualsTo + SDMXLibrary.Constants.Apostophe + UnitGId + SDMXLibrary.Constants.Apostophe + SDMXLibrary.Constants.AND + SubgroupVals.SubgroupValGId + SDMXLibrary.Constants.EqualsTo + SDMXLibrary.Constants.Apostophe + SGVGId + SDMXLibrary.Constants.Apostophe + SDMXLibrary.Constants.AND + Area.AreaID + SDMXLibrary.Constants.EqualsTo + SDMXLibrary.Constants.Apostophe + DrData[Area.AreaID].ToString() + SDMXLibrary.Constants.Apostophe + SDMXLibrary.Constants.AND + Timeperiods.TimePeriod + SDMXLibrary.Constants.EqualsTo + SDMXLibrary.Constants.Apostophe + TimePeriod + SDMXLibrary.Constants.Apostophe);

                                ObsVal = IUSARow[Data.DataValue].ToString();

                                TimeDetail = IUSARow[Timeperiods.TimePeriod].ToString();
                                SourceDetail = IUSARow[IndicatorClassifications.ICName].ToString();
                                Footnotes = IUSARow[FootNotes.FootNote].ToString();

                                Obs = new SDMXApi_2_0.CompactData.ObsType();
                                Obs.AnyAttr = new List<XmlAttribute>();
                                AttributesDoc = new XmlDocument();

                                Attribute = AttributesDoc.CreateAttribute(Constants.UNSD.Concept.Nature.Id);
                                Attribute.Value = Nature;
                                Obs.AnyAttr.Add(Attribute);

                                Attribute = AttributesDoc.CreateAttribute(Constants.UNSD.Concept.UnitMult.Id);
                                Attribute.Value = UnitMult;
                                Obs.AnyAttr.Add(Attribute);

                                Attribute = AttributesDoc.CreateAttribute(Constants.UNSD.Concept.TimePeriod.Id);
                                Attribute.Value = TimePeriod;
                                Obs.AnyAttr.Add(Attribute);

                                Attribute = AttributesDoc.CreateAttribute(Constants.UNSD.Concept.ObsVal.Id);
                                Attribute.Value = ObsVal;
                                Obs.AnyAttr.Add(Attribute);

                                Attribute = AttributesDoc.CreateAttribute(Constants.UNSD.Concept.TimeDetail.Id);
                                Attribute.Value = TimeDetail;
                                Obs.AnyAttr.Add(Attribute);

                                Attribute = AttributesDoc.CreateAttribute(Constants.UNSD.Concept.SourceDetail.Id);
                                Attribute.Value = SourceDetail;
                                Obs.AnyAttr.Add(Attribute);

                                Attribute = AttributesDoc.CreateAttribute(Constants.UNSD.Concept.Footnotes.Id);
                                Attribute.Value = Footnotes;
                                Obs.AnyAttr.Add(Attribute);

                                Series.ListObs.Add(Obs);

                            }

                            CompactData.DataSet.ListSeries.Add(Series);
                        }
                    }

                    if (CheckDuplicateKeys.Count() <= 1)
                    {
                        SDMXApi_2_0.Serializer.SerializeToFile(typeof(CompactDataType), CompactData, Path.Combine(Path.Combine(OutputFolder, Language), Convert.ToString(SeriesValue + "_DI_" + IndicatorGId) + DevInfo.Lib.DI_LibSDMX.Constants.XmlExtension));
                        GeneratedFiles.Add(Convert.ToString(SeriesValue + "_DI_" + IndicatorGId));
                    
                    }
                    else
                    {

                        fileCount -= 1;
                        if (string.IsNullOrEmpty(DuplicateKey))
                        {
                            DuplicateKey += "DK" + Constants.Delimiters.ParamDelimiter + "INDICATOR: " + IndicatorName + "__@@@@__" + "TIMEPERIOD: " + TimePeriod + Constants.Delimiters.ParamDelimiter;
                        }
                        else
                        {
                            DuplicateKey += "DK" + Constants.Delimiters.ParamDelimiter + "INDICATOR: " + IndicatorName + "__@@@@__" + "TIMEPERIOD: " + TimePeriod + Constants.Delimiters.ParamDelimiter;
                        }


                        XLSLogGenerator.WriteCSVLogForMailStatus("Error found while publishing data", IndicatorName, "File not published for the given IndicatorGId and Timeperiod", DuplicateKey);

                    }
                }
                else
                {
                    // to find indicator gid 
                    DtData = Get_Language_Specific_Data_Table(Language, IndicatorNId, string.Empty, string.Empty, string.Empty, DIConnection);
                  
                    if (string.IsNullOrEmpty(IndicatorName))
                    {
                        ErrorLogs += "NDF" + Constants.Delimiters.ParamDelimiter + IndicatorName + Constants.Delimiters.ParamDelimiter;
                        XLSLogGenerator.WriteCSVLogForMailStatus("No data found", IndicatorName, "File not published", IndicatorName);
                    }
                    else
                    {
                        if (ErrorLogs.Contains(IndicatorName))
                        {
                            continue;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(ErrorLogs))
                            {
                                ErrorLogs += "NDF" + Constants.Delimiters.ParamDelimiter + IndicatorName + Constants.Delimiters.ParamDelimiter;
                            }
                            else
                            {
                                ErrorLogs += "NDF" + Constants.Delimiters.ParamDelimiter + IndicatorName + Constants.Delimiters.ParamDelimiter;
                            }

                            XLSLogGenerator.WriteCSVLogForMailStatus("No data found for ", IndicatorName, "File not published for the given IndicatorGId", IndicatorName);
                        }
                    }
                }

                RowCount += 1;
            }
        }

        RetVal = true;
   
        return RetVal;

    }

    internal static DataTable Get_Language_Specific_Data_Table(string language, string IndicatorNId, string AreaNIds, string TimePeriodNIds, string SourceNIds, DIConnection DIConnection)
    {
        DataTable RetVal;
        string Query;

        RetVal = null;
        Query = string.Empty;

        try
        {
            Query = "SELECT Data.Data_Value, Data.Data_Denominator, [Indicator].Indicator_GId, Unit.Unit_GId, SGV.Subgroup_Val_GId,TP.TimePeriod, Area.Area_ID, Source.IC_Name, Source.Nature, Footnote.FootNote FROM";

            Query += " ";
            Query += "(SELECT Data_Value, Data_Denominator, Indicator_NId, Unit_NId, Subgroup_Val_NId, TimePeriod_NId, Area_NId, Source_NId, FootNote_NId FROM UT_Data WHERE 1 = 1";

            if (!string.IsNullOrEmpty(IndicatorNId))
            {
                Query += "AND Indicator_NId = " + IndicatorNId;
            }

            if (!string.IsNullOrEmpty(AreaNIds))
            {
                Query += " ";
                Query += "AND Area_NID IN";
                Query += " ";
                Query += "(" + AreaNIds + ")";
            }

            if (!string.IsNullOrEmpty(TimePeriodNIds))
            {
                Query += " ";
                Query += "AND TimePeriod_NId IN";
                Query += " ";
                Query += "(" + TimePeriodNIds + ")";
            }

            if (!string.IsNullOrEmpty(SourceNIds))
            {
                Query += " ";
                Query += "AND Source_NId IN";
                Query += " ";
                Query += "(" + SourceNIds + ")";
            }

            Query += ")";
            Query += " ";
            Query += " AS Data,";

            Query += " ";
            Query += "(SELECT Indicator_NId, Indicator_GId FROM UT_Indicator_" + language + ")AS [Indicator],";

            Query += " ";
            Query += "(SELECT Unit_NId, Unit_GId FROM UT_Unit_" + language + ") AS Unit ,";

            Query += " ";
            Query += "(SELECT Subgroup_Val_NId, Subgroup_Val_GId FROM UT_Subgroup_Vals_" + language + ") AS SGV ,";

            Query += " ";
            Query += "(SELECT TimePeriod_NId, TimePeriod FROM UT_TimePeriod)AS TP, ";

            Query += " ";
            Query += "(SELECT Area_NId, Area_ID FROM UT_Area_" + language + " WHERE Area_Level = " + Global.registryAreaLevel + " ) AS Area, ";

            Query += " ";
            Query += "(SELECT IC_NId, IC_Name, Nature FROM UT_Indicator_Classifications_" + language + ") AS Source,";

            Query += " ";
            Query += "(SELECT FootNote_NId, FootNote FROM UT_Footnote_" + language + ") Footnote ";

            Query += " WHERE Data.Unit_NId = Unit.Unit_NId AND  Data.FootNote_NId = Footnote.FootNote_NId ";
            Query += " AND Data.TimePeriod_NId = TP.TimePeriod_NId ";
            Query += " AND Data.Area_NId = Area.Area_NId AND Data.Indicator_NId = [Indicator].Indicator_NId ";
            Query += " AND Data.Source_NId = Source.IC_NId AND Data.Subgroup_Val_NId = SGV.Subgroup_Val_NId";
            //Query += " group by TP.TimePeriod Having  Count(Source.IC_Name) > 1";

            RetVal = DIConnection.ExecuteDataTable(Regex.Replace(Query, "UT_", DIConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));
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

    internal static string Get_Time_Period_Start_Year(string timePeriod)
    {
        string RetVal;
        DateTime StartDate, EndDate;

        StartDate = new DateTime();
        EndDate = new DateTime();
        TimePeriodFacade.SetStartDateEndDate(timePeriod, ref StartDate, ref EndDate);
        RetVal = StartDate.Year.ToString();

        return RetVal;
    }
}