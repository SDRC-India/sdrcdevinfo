using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Data;
using SDMXObjectModel;
using DevInfo.Lib.DI_LibDAL;
using SDMXObjectModel.Data.Generic;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using SDMXObjectModel.Message;
using SDMXObjectModel.Structure;
using SDMXObjectModel.Common;
using System.IO;


namespace DevInfo.Lib.DI_LibSDMX
{
    internal class GenericTimeSeriesDataUtility : BaseDataUtility
    {
        #region "Properties"

        #region "Private"

        #endregion "Private"

        #region "Public"

        #endregion "Public"

        #endregion "Properties"

        #region "Constructors"

        #region "Private"

        #endregion "Private"

        #region "Public"

        internal GenericTimeSeriesDataUtility(DIConnection DIConnection, DIQueries DIQueries, string agencyId)
            : base(DIConnection, DIQueries, agencyId)
        {
        }

        #endregion "Public"

        #endregion "Constructors"

        #region "Methods"

        #region "Private"

        private SDMXObjectModel.Message.GenericTimeSeriesDataHeaderType Get_Appropriate_Header()
        {
            SDMXObjectModel.Message.GenericTimeSeriesDataHeaderType RetVal;
            SenderType Sender;
            PartyType Receiver;

            Sender = new SenderType(Constants.Header.SenderId, Constants.Header.SenderName, Constants.DefaultLanguage, new SDMXObjectModel.Message.ContactType(Constants.Header.Sender, Constants.Header.SenderDepartment, Constants.Header.SenderRole, Constants.DefaultLanguage));
            Sender.Contact[0].Items = new string[] { Constants.Header.SenderTelephone, Constants.Header.SenderEmail, Constants.Header.SenderFax };
            Sender.Contact[0].ItemsElementName = new SDMXObjectModel.Message.ContactChoiceType[] { SDMXObjectModel.Message.ContactChoiceType.Telephone, SDMXObjectModel.Message.ContactChoiceType.Email, SDMXObjectModel.Message.ContactChoiceType.Fax };

            Receiver = new PartyType(Constants.Header.ReceiverId, Constants.Header.ReceiverName, Constants.DefaultLanguage, new SDMXObjectModel.Message.ContactType(Constants.Header.Receiver, Constants.Header.ReceiverDepartment, Constants.Header.ReceiverRole, Constants.DefaultLanguage));
            Receiver.Contact[0].Items = new string[] { Constants.Header.ReceiverTelephone, Constants.Header.ReceiverEmail, Constants.Header.ReceiverFax };
            Receiver.Contact[0].ItemsElementName = new SDMXObjectModel.Message.ContactChoiceType[] { SDMXObjectModel.Message.ContactChoiceType.Telephone, SDMXObjectModel.Message.ContactChoiceType.Email, SDMXObjectModel.Message.ContactChoiceType.Fax };

            RetVal = new GenericTimeSeriesDataHeaderType(Constants.Header.Id, true, DateTime.Now, Sender, Receiver);

            return RetVal;
        }

        #endregion "Private"

        #region "Public"

        internal override XmlDocument Get_Data(XmlDocument query)
        {
            XmlDocument RetVal;
            SDMXObjectModel.Message.GenericTimeSeriesDataQueryType GenericTimeSeriesDataQuery;
            GenericTimeSeriesDataType GenericTimeSeriesData;
            TimeSeriesDataSetType Dataset;
            SeriesType Series;
            ObsType Obs;
            Dictionary<string, string> DictSubgroupBreakup;
            DataTable DtSubgroup, DtSubgroupTypes, DtData, DtDistinctIUSA;
            string IndicatorGId, UnitGId, SubgroupValGId, AreaID, Source;
            DataRow[] IUSARows;

            RetVal = null;
            DtData = null;

            try
            {
                try
                {
                    GenericTimeSeriesDataQuery = (SDMXObjectModel.Message.GenericTimeSeriesDataQueryType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.GenericTimeSeriesDataQueryType), query);
                }
                catch
                {
                    throw new Exception(Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                }

                this.Parse_Query(GenericTimeSeriesDataQuery.Query.DataWhere, GenericTimeSeriesDataQuery.Query.ReturnDetails);

                DtSubgroup = this.DIConnection.ExecuteDataTable(this.DIQueries.SubgroupValSubgroup.GetSubgroupValsWithSubgroups());
                DtSubgroupTypes = this.DIConnection.ExecuteDataTable(this.DIQueries.SubgroupTypes.GetSubgroupTypes(FilterFieldType.None, string.Empty));
                if (this.Languages != null && this.Languages.Count > 0)
                {
                    GenericTimeSeriesData = new GenericTimeSeriesDataType();
                    GenericTimeSeriesData.DataSet = new List<TimeSeriesDataSetType>();

                    GenericTimeSeriesData.Header = this.Get_Appropriate_Header();
                    GenericTimeSeriesData.Header.Structure = new SDMXObjectModel.Common.GenericTimeSeriesDataStructureType();

                    GenericTimeSeriesData.Header.Structure.dimensionAtObservation = Constants.Concept.TIME_PERIOD.Id;
                    GenericTimeSeriesData.Header.Structure.structureID = Constants.DSD.Id;
                    GenericTimeSeriesData.Header.Structure.@namespace = null;

                    GenericTimeSeriesData.Header.Structure.Item = new DataStructureReferenceType();
                    ((DataStructureReferenceType)GenericTimeSeriesData.Header.Structure.Item).Items.Add(new DataStructureRefType());
                    ((DataStructureRefType)((DataStructureReferenceType)GenericTimeSeriesData.Header.Structure.Item).Items[0]).id = Constants.DSD.Id;
                    ((DataStructureRefType)((DataStructureReferenceType)GenericTimeSeriesData.Header.Structure.Item).Items[0]).agencyID = this.AgencyId;
                    ((DataStructureRefType)((DataStructureReferenceType)GenericTimeSeriesData.Header.Structure.Item).Items[0]).version = Constants.DSD.Version;
                    GenericTimeSeriesData.Footer = null;

                    foreach (string language in this.Languages)
                    {
                        Dataset = new TimeSeriesDataSetType();
                        GenericTimeSeriesData.DataSet.Add(Dataset);
                        Dataset.structureRef = Constants.DSD.Id;
                        Dataset.DataProvider = null;
                        Dataset.Annotations = null;
                        Dataset.Attributes = null;
                        Dataset.Items = new List<SDMXObjectModel.Common.AnnotableType>();

                        this.Set_Area_NIds(language);
                        this.Set_Source_NIds(language);

                        DtDistinctIUSA = this.Get_Distinct_IUSA_Table(language);

                        if (this.DataReturnDetailType == DataReturnDetailTypes.Full)
                        {
                            DtData = this.Get_Language_Specific_Data_Table(language);
                        }

                        foreach (DataRow DrDistinctIUSA in DtDistinctIUSA.Rows)
                        {
                            IndicatorGId = DrDistinctIUSA[Indicator.IndicatorGId].ToString();
                            UnitGId = DrDistinctIUSA[Unit.UnitGId].ToString();
                            SubgroupValGId = DrDistinctIUSA[SubgroupVals.SubgroupValGId].ToString();
                            AreaID = DrDistinctIUSA[Area.AreaID].ToString();
                            Source = DrDistinctIUSA[IndicatorClassifications.ICName].ToString();

                            Series = new SeriesType();
                            Series.Annotations = null;
                            Series.Attributes = null;
                            Series.Obs = new List<ObsType>();

                            Series.SeriesKey.Add(new ComponentValueType(Constants.Concept.INDICATOR.Id, IndicatorGId));
                            Series.SeriesKey.Add(new ComponentValueType(Constants.Concept.UNIT.Id, UnitGId));

                            DictSubgroupBreakup = this.Get_Subgroup_Breakup(SubgroupValGId, DtSubgroup, DtSubgroupTypes);
                            foreach (string Key in DictSubgroupBreakup.Keys)
                            {
                                Series.SeriesKey.Add(new ComponentValueType(Key, DictSubgroupBreakup[Key]));
                            }

                            Series.SeriesKey.Add(new ComponentValueType(Constants.Concept.AREA.Id, AreaID));
                            Series.SeriesKey.Add(new ComponentValueType(Constants.Concept.SOURCE.Id, Source));
                            Dataset.Items.Add(Series);

                            if (this.DataReturnDetailType == DataReturnDetailTypes.Full)
                            {
                                IUSARows = DtData.Select(Indicator.IndicatorGId + Constants.EqualsTo + Constants.Apostophe + IndicatorGId + Constants.Apostophe + Constants.AND + Unit.UnitGId + Constants.EqualsTo + Constants.Apostophe + UnitGId + Constants.Apostophe + Constants.AND + SubgroupVals.SubgroupValGId + Constants.EqualsTo + Constants.Apostophe + SubgroupValGId + Constants.Apostophe + Constants.AND + Area.AreaID + Constants.EqualsTo + Constants.Apostophe + AreaID.Replace("'", "''") + Constants.Apostophe + Constants.AND + IndicatorClassifications.ICName + Constants.EqualsTo + Constants.Apostophe + Source.Replace("'", "''") + Constants.Apostophe);

                                foreach (DataRow DrIUSA in IUSARows)
                                {
                                    Obs = new ObsType();
                                    Obs.Annotations = null;

                                    Obs.ObsDimension.id = Constants.Concept.TIME_PERIOD.Id;
                                    Obs.ObsDimension.value = DrIUSA[Timeperiods.TimePeriod].ToString();

                                    Obs.ObsValue.id = Constants.Concept.OBS_VALUE.Id;
                                    Obs.ObsValue.value = DrIUSA[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.DataValue].ToString();

                                    Obs.Attributes.Add(new ComponentValueType(Constants.Concept.CONFIDENCE_INTERVAL_LOWER.Id, DrIUSA["ConfidenceIntervalLower"].ToString()));
                                    Obs.Attributes.Add(new ComponentValueType(Constants.Concept.CONFIDENCE_INTERVAL_UPPER.Id, DrIUSA["ConfidenceIntervalUpper"].ToString()));
                                    Obs.Attributes.Add(new ComponentValueType(Constants.Concept.NATURE.Id, DrIUSA["Nature"].ToString()));
                                    Obs.Attributes.Add(new ComponentValueType(Constants.Concept.PERIODICITY.Id, DrIUSA["Periodicity"].ToString()));
                                    Obs.Attributes.Add(new ComponentValueType(Constants.Concept.DENOMINATOR.Id, DrIUSA[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.DataDenominator].ToString()));
                                    Obs.Attributes.Add(new ComponentValueType(Constants.Concept.FOOTNOTES.Id, DrIUSA[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.FootNotes.FootNote].ToString()));

                                    Series.Obs.Add(Obs);
                                }
                            }
                        }
                    }

                    if (GenericTimeSeriesData != null && GenericTimeSeriesData.DataSet != null && GenericTimeSeriesData.DataSet.Count > 0 && GenericTimeSeriesData.DataSet[0].Items != null && GenericTimeSeriesData.DataSet[0].Items.Count > 0)
                    {
                        RetVal = Serializer.SerializeToXmlDocument(typeof(GenericTimeSeriesDataType), GenericTimeSeriesData);
                    }
                    else
                    {
                        throw new Exception(Constants.SDMXWebServices.Exceptions.NoResults.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        internal override bool Generate_Data(XmlDocument query, string outputFolder, out int fileCount, out List<string> GeneratedFiles, SDMXObjectModel.Message.StructureHeaderType Header)
        {
            bool RetVal;
            string FileName;
            SDMXObjectModel.Message.GenericTimeSeriesDataQueryType GenericTimeSeriesDataQuery;
            GenericTimeSeriesDataType GenericTimeSeriesData;
            TimeSeriesDataSetType Dataset;
            SeriesType Series;
            ObsType Obs;
            Dictionary<string, string> DictSubgroupBreakup;
            DataTable DtSubgroup, DtSubgroupTypes, DtData, DtDistinctIUS, DtDistinctIUSA;
            string IndicatorGId, UnitGId, SubgroupValGId, AreaID, Source;
            DataRow[] IUSRows, IUSARows;
            fileCount = 0;
            DateTime CurrentTime = DateTime.Now;
            GeneratedFiles = new List<string>();
            try
            {
                GenericTimeSeriesDataQuery = (SDMXObjectModel.Message.GenericTimeSeriesDataQueryType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.GenericTimeSeriesDataQueryType), query);

                this.Parse_Query(GenericTimeSeriesDataQuery.Query.DataWhere, GenericTimeSeriesDataQuery.Query.ReturnDetails);
                this.Create_Folder_Structure(outputFolder);

                DtSubgroup = this.DIConnection.ExecuteDataTable(this.DIQueries.SubgroupValSubgroup.GetSubgroupValsWithSubgroups());
                DtSubgroupTypes = this.DIConnection.ExecuteDataTable(this.DIQueries.SubgroupTypes.GetSubgroupTypes(FilterFieldType.None, string.Empty));
                FileName = string.Empty;

                if (this.Languages != null && this.Languages.Count > 0)
                {
                    foreach (string language in this.Languages)
                    {
                        this.Set_Area_NIds(language);
                        this.Set_Source_NIds(language);

                        DtDistinctIUS = this.Get_Distinct_IUS_Table(language);
                        DtDistinctIUSA = this.Get_Distinct_IUSA_Table(language);
                        DtData = this.Get_Language_Specific_Data_Table(language);

                        foreach (DataRow DrDistinctIUS in DtDistinctIUS.Rows)
                        {
                            fileCount = fileCount + 1;
                            IndicatorGId = DrDistinctIUS[Indicator.IndicatorGId].ToString();
                            UnitGId = DrDistinctIUS[Unit.UnitGId].ToString();
                            SubgroupValGId = DrDistinctIUS[SubgroupVals.SubgroupValGId].ToString();


                            FileName = Path.Combine(Path.Combine(outputFolder, language), IndicatorGId + Constants.Underscore + UnitGId + Constants.Underscore + SubgroupValGId + "_" + language + "_" + CurrentTime.ToString("yyyy-MM-dd HHmmss") + Constants.XmlExtension);

                            GenericTimeSeriesData = new GenericTimeSeriesDataType();
                            GenericTimeSeriesData.DataSet = new List<TimeSeriesDataSetType>();

                           
                            if (Header == null)
                            {
                                GenericTimeSeriesData.Header = this.Get_Appropriate_Header();
                            }
                            else
                            {
                                GenericTimeSeriesData.Header.ID = Header.ID;
                                GenericTimeSeriesData.Header.Name = Header.Name;
                                GenericTimeSeriesData.Header.Prepared = Header.Prepared;
                                GenericTimeSeriesData.Header.Receiver = Header.Receiver;
                                GenericTimeSeriesData.Header.Sender = Header.Sender;
                                GenericTimeSeriesData.Header.Test = Header.Test;
                            }
                            GenericTimeSeriesData.Header.Structure = new GenericTimeSeriesDataStructureType();

                            GenericTimeSeriesData.Header.Structure.dimensionAtObservation = Constants.Concept.TIME_PERIOD.Id;
                            GenericTimeSeriesData.Header.Structure.structureID = Constants.DSD.Id;
                            GenericTimeSeriesData.Header.Structure.@namespace = null;

                            GenericTimeSeriesData.Header.Structure.Item = new DataStructureReferenceType();
                            ((DataStructureReferenceType)GenericTimeSeriesData.Header.Structure.Item).Items.Add(new DataStructureRefType());
                            ((DataStructureRefType)((DataStructureReferenceType)GenericTimeSeriesData.Header.Structure.Item).Items[0]).id = Constants.DSD.Id;
                            ((DataStructureRefType)((DataStructureReferenceType)GenericTimeSeriesData.Header.Structure.Item).Items[0]).agencyID = this.AgencyId;
                            ((DataStructureRefType)((DataStructureReferenceType)GenericTimeSeriesData.Header.Structure.Item).Items[0]).version = Constants.DSD.Version;
                            GenericTimeSeriesData.Footer = null;

                            Dataset = new TimeSeriesDataSetType();
                            GenericTimeSeriesData.DataSet.Add(Dataset);
                            Dataset.structureRef = Constants.DSD.Id;
                            Dataset.DataProvider = null;
                            Dataset.Annotations = null;
                            Dataset.Attributes = null;
                            Dataset.Items = new List<SDMXObjectModel.Common.AnnotableType>();

                            IUSRows = DtDistinctIUSA.Select(Indicator.IndicatorGId + Constants.EqualsTo + Constants.Apostophe + IndicatorGId + Constants.Apostophe + Constants.AND + Unit.UnitGId + Constants.EqualsTo + Constants.Apostophe + UnitGId + Constants.Apostophe + Constants.AND + SubgroupVals.SubgroupValGId + Constants.EqualsTo + Constants.Apostophe + SubgroupValGId + Constants.Apostophe);

                            foreach (DataRow DrIUS in IUSRows)
                            {
                                AreaID = DrIUS[Area.AreaID].ToString();
                                Source = DrIUS[IndicatorClassifications.ICName].ToString();

                                Series = new SeriesType();
                                Series.Annotations = null;
                                Series.Attributes = null;
                                Series.Obs = new List<ObsType>();

                                Series.SeriesKey.Add(new ComponentValueType(Constants.Concept.INDICATOR.Id, IndicatorGId));
                                Series.SeriesKey.Add(new ComponentValueType(Constants.Concept.UNIT.Id, UnitGId));

                                DictSubgroupBreakup = this.Get_Subgroup_Breakup(SubgroupValGId, DtSubgroup, DtSubgroupTypes);
                                foreach (string Key in DictSubgroupBreakup.Keys)
                                {
                                    Series.SeriesKey.Add(new ComponentValueType(Key, DictSubgroupBreakup[Key]));
                                }

                                Series.SeriesKey.Add(new ComponentValueType(Constants.Concept.AREA.Id, AreaID));
                                Series.SeriesKey.Add(new ComponentValueType(Constants.Concept.SOURCE.Id, Source));
                                Dataset.Items.Add(Series);

                                IUSARows = DtData.Select(Indicator.IndicatorGId + Constants.EqualsTo + Constants.Apostophe + IndicatorGId + Constants.Apostophe + Constants.AND + Unit.UnitGId + Constants.EqualsTo + Constants.Apostophe + UnitGId + Constants.Apostophe + Constants.AND + SubgroupVals.SubgroupValGId + Constants.EqualsTo + Constants.Apostophe + SubgroupValGId + Constants.Apostophe + Constants.AND + Area.AreaID + Constants.EqualsTo + Constants.Apostophe + AreaID.Replace("'", "''") + Constants.Apostophe + Constants.AND + IndicatorClassifications.ICName + Constants.EqualsTo + Constants.Apostophe + Source.Replace("'", "''") + Constants.Apostophe);

                                foreach (DataRow DrIUSA in IUSARows)
                                {
                                    Obs = new ObsType();
                                    Obs.Annotations = null;

                                    Obs.ObsDimension.id = Constants.Concept.TIME_PERIOD.Id;
                                    Obs.ObsDimension.value = DrIUSA[Timeperiods.TimePeriod].ToString();

                                    Obs.ObsValue.id = Constants.Concept.OBS_VALUE.Id;
                                    Obs.ObsValue.value = DrIUSA[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.DataValue].ToString();

                                    Obs.Attributes.Add(new ComponentValueType(Constants.Concept.CONFIDENCE_INTERVAL_LOWER.Id, DrIUSA["ConfidenceIntervalLower"].ToString()));
                                    Obs.Attributes.Add(new ComponentValueType(Constants.Concept.CONFIDENCE_INTERVAL_UPPER.Id, DrIUSA["ConfidenceIntervalUpper"].ToString()));
                                    Obs.Attributes.Add(new ComponentValueType(Constants.Concept.NATURE.Id, DrIUSA["Nature"].ToString()));
                                    Obs.Attributes.Add(new ComponentValueType(Constants.Concept.PERIODICITY.Id, DrIUSA["Periodicity"].ToString()));
                                    Obs.Attributes.Add(new ComponentValueType(Constants.Concept.DENOMINATOR.Id, DrIUSA[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.DataDenominator].ToString()));
                                    Obs.Attributes.Add(new ComponentValueType(Constants.Concept.FOOTNOTES.Id, DrIUSA[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.FootNotes.FootNote].ToString()));

                                    Series.Obs.Add(Obs);
                                }
                            }
                            Serializer.SerializeToFile(typeof(GenericTimeSeriesDataType), GenericTimeSeriesData, FileName);
                            GeneratedFiles.Add(FileName);
                        }
                    }
                }

                RetVal = true;
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

        internal override bool Generate_Data(XmlDocument query, string outputFolder)
        {
            bool RetVal;
            string FileName;
            SDMXObjectModel.Message.GenericTimeSeriesDataQueryType GenericTimeSeriesDataQuery;
            GenericTimeSeriesDataType GenericTimeSeriesData;
            TimeSeriesDataSetType Dataset;
            SeriesType Series;
            ObsType Obs;
            Dictionary<string, string> DictSubgroupBreakup;
            DataTable DtSubgroup, DtSubgroupTypes, DtData, DtDistinctIUS, DtDistinctIUSA;
            string IndicatorGId, UnitGId, SubgroupValGId, AreaID, Source;
            DataRow[] IUSRows, IUSARows;
         
            DateTime CurrentTime = DateTime.Now;
          
            try
            {
                GenericTimeSeriesDataQuery = (SDMXObjectModel.Message.GenericTimeSeriesDataQueryType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.GenericTimeSeriesDataQueryType), query);

                this.Parse_Query(GenericTimeSeriesDataQuery.Query.DataWhere, GenericTimeSeriesDataQuery.Query.ReturnDetails);
                this.Create_Folder_Structure(outputFolder);

                DtSubgroup = this.DIConnection.ExecuteDataTable(this.DIQueries.SubgroupValSubgroup.GetSubgroupValsWithSubgroups());
                DtSubgroupTypes = this.DIConnection.ExecuteDataTable(this.DIQueries.SubgroupTypes.GetSubgroupTypes(FilterFieldType.None, string.Empty));
                FileName = string.Empty;

                if (this.Languages != null && this.Languages.Count > 0)
                {
                    foreach (string language in this.Languages)
                    {
                        this.Set_Area_NIds(language);
                        this.Set_Source_NIds(language);

                        DtDistinctIUS = this.Get_Distinct_IUS_Table(language);
                        DtDistinctIUSA = this.Get_Distinct_IUSA_Table(language);
                        DtData = this.Get_Language_Specific_Data_Table(language);

                        foreach (DataRow DrDistinctIUS in DtDistinctIUS.Rows)
                        {
                           
                            IndicatorGId = DrDistinctIUS[Indicator.IndicatorGId].ToString();
                            UnitGId = DrDistinctIUS[Unit.UnitGId].ToString();
                            SubgroupValGId = DrDistinctIUS[SubgroupVals.SubgroupValGId].ToString();


                            FileName = Path.Combine(Path.Combine(outputFolder, language), IndicatorGId + Constants.Underscore + UnitGId + Constants.Underscore + SubgroupValGId + "_" + language + "_" + CurrentTime.ToString("yyyy-MM-dd HHmmss") + Constants.XmlExtension);

                            GenericTimeSeriesData = new GenericTimeSeriesDataType();
                            GenericTimeSeriesData.DataSet = new List<TimeSeriesDataSetType>();

                            GenericTimeSeriesData.Header = this.Get_Appropriate_Header();
                            GenericTimeSeriesData.Header.Structure = new GenericTimeSeriesDataStructureType();

                            GenericTimeSeriesData.Header.Structure.dimensionAtObservation = Constants.Concept.TIME_PERIOD.Id;
                            GenericTimeSeriesData.Header.Structure.structureID = Constants.DSD.Id;
                            GenericTimeSeriesData.Header.Structure.@namespace = null;

                            GenericTimeSeriesData.Header.Structure.Item = new DataStructureReferenceType();
                            ((DataStructureReferenceType)GenericTimeSeriesData.Header.Structure.Item).Items.Add(new DataStructureRefType());
                            ((DataStructureRefType)((DataStructureReferenceType)GenericTimeSeriesData.Header.Structure.Item).Items[0]).id = Constants.DSD.Id;
                            ((DataStructureRefType)((DataStructureReferenceType)GenericTimeSeriesData.Header.Structure.Item).Items[0]).agencyID = this.AgencyId;
                            ((DataStructureRefType)((DataStructureReferenceType)GenericTimeSeriesData.Header.Structure.Item).Items[0]).version = Constants.DSD.Version;
                            GenericTimeSeriesData.Footer = null;

                            Dataset = new TimeSeriesDataSetType();
                            GenericTimeSeriesData.DataSet.Add(Dataset);
                            Dataset.structureRef = Constants.DSD.Id;
                            Dataset.DataProvider = null;
                            Dataset.Annotations = null;
                            Dataset.Attributes = null;
                            Dataset.Items = new List<SDMXObjectModel.Common.AnnotableType>();

                            IUSRows = DtDistinctIUSA.Select(Indicator.IndicatorGId + Constants.EqualsTo + Constants.Apostophe + IndicatorGId + Constants.Apostophe + Constants.AND + Unit.UnitGId + Constants.EqualsTo + Constants.Apostophe + UnitGId + Constants.Apostophe + Constants.AND + SubgroupVals.SubgroupValGId + Constants.EqualsTo + Constants.Apostophe + SubgroupValGId + Constants.Apostophe);

                            foreach (DataRow DrIUS in IUSRows)
                            {
                                AreaID = DrIUS[Area.AreaID].ToString();
                                Source = DrIUS[IndicatorClassifications.ICName].ToString();

                                Series = new SeriesType();
                                Series.Annotations = null;
                                Series.Attributes = null;
                                Series.Obs = new List<ObsType>();

                                Series.SeriesKey.Add(new ComponentValueType(Constants.Concept.INDICATOR.Id, IndicatorGId));
                                Series.SeriesKey.Add(new ComponentValueType(Constants.Concept.UNIT.Id, UnitGId));

                                DictSubgroupBreakup = this.Get_Subgroup_Breakup(SubgroupValGId, DtSubgroup, DtSubgroupTypes);
                                foreach (string Key in DictSubgroupBreakup.Keys)
                                {
                                    Series.SeriesKey.Add(new ComponentValueType(Key, DictSubgroupBreakup[Key]));
                                }

                                Series.SeriesKey.Add(new ComponentValueType(Constants.Concept.AREA.Id, AreaID));
                                Series.SeriesKey.Add(new ComponentValueType(Constants.Concept.SOURCE.Id, Source));
                                Dataset.Items.Add(Series);

                                IUSARows = DtData.Select(Indicator.IndicatorGId + Constants.EqualsTo + Constants.Apostophe + IndicatorGId + Constants.Apostophe + Constants.AND + Unit.UnitGId + Constants.EqualsTo + Constants.Apostophe + UnitGId + Constants.Apostophe + Constants.AND + SubgroupVals.SubgroupValGId + Constants.EqualsTo + Constants.Apostophe + SubgroupValGId + Constants.Apostophe + Constants.AND + Area.AreaID + Constants.EqualsTo + Constants.Apostophe + AreaID.Replace("'", "''") + Constants.Apostophe + Constants.AND + IndicatorClassifications.ICName + Constants.EqualsTo + Constants.Apostophe + Source.Replace("'", "''") + Constants.Apostophe);

                                foreach (DataRow DrIUSA in IUSARows)
                                {
                                    Obs = new ObsType();
                                    Obs.Annotations = null;

                                    Obs.ObsDimension.id = Constants.Concept.TIME_PERIOD.Id;
                                    Obs.ObsDimension.value = DrIUSA[Timeperiods.TimePeriod].ToString();

                                    Obs.ObsValue.id = Constants.Concept.OBS_VALUE.Id;
                                    Obs.ObsValue.value = DrIUSA[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.DataValue].ToString();

                                    Obs.Attributes.Add(new ComponentValueType(Constants.Concept.CONFIDENCE_INTERVAL_LOWER.Id, DrIUSA["ConfidenceIntervalLower"].ToString()));
                                    Obs.Attributes.Add(new ComponentValueType(Constants.Concept.CONFIDENCE_INTERVAL_UPPER.Id, DrIUSA["ConfidenceIntervalUpper"].ToString()));
                                    Obs.Attributes.Add(new ComponentValueType(Constants.Concept.NATURE.Id, DrIUSA["Nature"].ToString()));
                                    Obs.Attributes.Add(new ComponentValueType(Constants.Concept.PERIODICITY.Id, DrIUSA["Periodicity"].ToString()));
                                    Obs.Attributes.Add(new ComponentValueType(Constants.Concept.DENOMINATOR.Id, DrIUSA[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.DataDenominator].ToString()));
                                    Obs.Attributes.Add(new ComponentValueType(Constants.Concept.FOOTNOTES.Id, DrIUSA[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.FootNotes.FootNote].ToString()));

                                    Series.Obs.Add(Obs);
                                }
                            }
                            Serializer.SerializeToFile(typeof(GenericTimeSeriesDataType), GenericTimeSeriesData, FileName);
                          
                        }
                    }
                }

                RetVal = true;
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

        #endregion "Public"

        #endregion "Methods"
    }
}
