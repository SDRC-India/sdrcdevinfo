using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using SDMXObjectModel;
using System.Data;
using SDMXObjectModel.Message;
using SDMXObjectModel.Query;
using SDMXObjectModel.Structure;
using DevInfo.Lib.DI_LibDAL;
using SDMXObjectModel.Data.StructureSpecific;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.IO;
using SDMXObjectModel.Common;

namespace DevInfo.Lib.DI_LibSDMX
{
    internal class StructureSpecificTimeSeriesDataUtility : BaseDataUtility
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

        internal StructureSpecificTimeSeriesDataUtility(DIConnection DIConnection, DIQueries DIQueries, string agencyId)
            : base(DIConnection, DIQueries, agencyId)
        {
        }

        #endregion "Public"

        #endregion "Constructors"

        #region "Methods"

        #region "Private"

        private SDMXObjectModel.Message.StructureSpecificTimeSeriesDataHeaderType Get_Appropriate_Header()
        {
            SDMXObjectModel.Message.StructureSpecificTimeSeriesDataHeaderType RetVal;
            SenderType Sender;
            PartyType Receiver;

            Sender = new SenderType(Constants.Header.SenderId, Constants.Header.SenderName, Constants.DefaultLanguage, new SDMXObjectModel.Message.ContactType(Constants.Header.Sender, Constants.Header.SenderDepartment, Constants.Header.SenderRole, Constants.DefaultLanguage));
            Sender.Contact[0].Items = new string[] { Constants.Header.SenderTelephone, Constants.Header.SenderEmail, Constants.Header.SenderFax };
            Sender.Contact[0].ItemsElementName = new SDMXObjectModel.Message.ContactChoiceType[] { SDMXObjectModel.Message.ContactChoiceType.Telephone, SDMXObjectModel.Message.ContactChoiceType.Email, SDMXObjectModel.Message.ContactChoiceType.Fax };

            Receiver = new PartyType(Constants.Header.ReceiverId, Constants.Header.ReceiverName, Constants.DefaultLanguage, new SDMXObjectModel.Message.ContactType(Constants.Header.Receiver, Constants.Header.ReceiverDepartment, Constants.Header.ReceiverRole, Constants.DefaultLanguage));
            Receiver.Contact[0].Items = new string[] { Constants.Header.ReceiverTelephone, Constants.Header.ReceiverEmail, Constants.Header.ReceiverFax };
            Receiver.Contact[0].ItemsElementName = new SDMXObjectModel.Message.ContactChoiceType[] { SDMXObjectModel.Message.ContactChoiceType.Telephone, SDMXObjectModel.Message.ContactChoiceType.Email, SDMXObjectModel.Message.ContactChoiceType.Fax };

            RetVal = new StructureSpecificTimeSeriesDataHeaderType(Constants.Header.Id, true, DateTime.Now, Sender, Receiver);

            return RetVal;
        }
               
        #endregion "Private"

        #region "Public"

        internal override XmlDocument Get_Data(XmlDocument query)
        {
            XmlDocument RetVal;
            SDMXObjectModel.Message.StructureSpecificTimeSeriesDataQueryType StructureSpecificTimeSeriesDataQuery;
            StructureSpecificTimeSeriesDataType StructureSpecificTimeSeriesData;
            TimeSeriesDataSetType Dataset;
            SeriesType Series;
            ObsType Obs;
            Dictionary<string, string> DictSeriesAttributes, DictObsAttributes, DictSubgroupBreakup;
            DataTable DtSubgroup, DtSubgroupTypes, DtData, DtDistinctIUSA;
            string IndicatorGId, UnitGId, SubgroupValGId, AreaID, Source;
            DataRow[] IUSARows;

            RetVal = null;
            DtData = null;

            try
            {
                try
                {
                    StructureSpecificTimeSeriesDataQuery = (SDMXObjectModel.Message.StructureSpecificTimeSeriesDataQueryType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureSpecificTimeSeriesDataQueryType), query);
                }
                catch
                {
                    throw new Exception(Constants.SDMXWebServices.Exceptions.InvalidSyntax.Message);
                }

                this.Parse_Query(StructureSpecificTimeSeriesDataQuery.Query.DataWhere, StructureSpecificTimeSeriesDataQuery.Query.ReturnDetails);

                DtSubgroup = this.DIConnection.ExecuteDataTable(this.DIQueries.SubgroupValSubgroup.GetSubgroupValsWithSubgroups());
                DtSubgroupTypes = this.DIConnection.ExecuteDataTable(this.DIQueries.SubgroupTypes.GetSubgroupTypes(FilterFieldType.None, string.Empty));

                if (this.Languages != null && this.Languages.Count > 0)
                {
                    StructureSpecificTimeSeriesData = new StructureSpecificTimeSeriesDataType();
                    StructureSpecificTimeSeriesData.DataSet = new List<TimeSeriesDataSetType>();

                    StructureSpecificTimeSeriesData.Header = this.Get_Appropriate_Header();
                    StructureSpecificTimeSeriesData.Header.Structure = new List<SDMXObjectModel.Common.StructureSpecificDataTimeSeriesStructureType>();
                    StructureSpecificTimeSeriesData.Header.Structure.Add(new SDMXObjectModel.Common.StructureSpecificDataTimeSeriesStructureType());

                    StructureSpecificTimeSeriesData.Header.Structure[0].dimensionAtObservation = Constants.Concept.TIME_PERIOD.Id;
                    StructureSpecificTimeSeriesData.Header.Structure[0].structureID = Constants.DSD.Id;
                    StructureSpecificTimeSeriesData.Header.Structure[0].@namespace = Constants.defaultNamespace;

                    StructureSpecificTimeSeriesData.Header.Structure[0].Item = new DataStructureReferenceType();
                    ((DataStructureReferenceType)StructureSpecificTimeSeriesData.Header.Structure[0].Item).Items.Add(new DataStructureRefType());
                    ((DataStructureRefType)((DataStructureReferenceType)StructureSpecificTimeSeriesData.Header.Structure[0].Item).Items[0]).id = Constants.DSD.Id;
                    ((DataStructureRefType)((DataStructureReferenceType)StructureSpecificTimeSeriesData.Header.Structure[0].Item).Items[0]).agencyID = this.AgencyId;
                    ((DataStructureRefType)((DataStructureReferenceType)StructureSpecificTimeSeriesData.Header.Structure[0].Item).Items[0]).version = Constants.DSD.Version;
                    StructureSpecificTimeSeriesData.Footer = null;

                    foreach (string language in this.Languages)
                    {
                        Dataset = new TimeSeriesDataSetType();
                        StructureSpecificTimeSeriesData.DataSet.Add(Dataset);
                        Dataset.structureRef = Constants.DSD.Id;
                        Dataset.DataProvider = null;
                        Dataset.Annotations = null;
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

                            DictSeriesAttributes = new Dictionary<string, string>();

                            DictSeriesAttributes.Add(Constants.Concept.INDICATOR.Id, IndicatorGId);
                            DictSeriesAttributes.Add(Constants.Concept.UNIT.Id, UnitGId);

                            DictSubgroupBreakup = this.Get_Subgroup_Breakup(SubgroupValGId, DtSubgroup, DtSubgroupTypes);
                            foreach (string Key in DictSubgroupBreakup.Keys)
                            {
                                DictSeriesAttributes.Add(Key, DictSubgroupBreakup[Key]);
                            }

                            DictSeriesAttributes.Add(Constants.Concept.AREA.Id, AreaID);
                            DictSeriesAttributes.Add(Constants.Concept.SOURCE.Id, Source);

                            Series = new SeriesType(DictSeriesAttributes, null);
                            Dataset.Items.Add(Series);
                            Series.Annotations = null;
                            Series.Obs = new List<ObsType>();

                            if (this.DataReturnDetailType == DataReturnDetailTypes.Full)
                            {
                                IUSARows = DtData.Select(Indicator.IndicatorGId + Constants.EqualsTo + Constants.Apostophe + IndicatorGId + Constants.Apostophe + Constants.AND + Unit.UnitGId + Constants.EqualsTo + Constants.Apostophe + UnitGId + Constants.Apostophe + Constants.AND + SubgroupVals.SubgroupValGId + Constants.EqualsTo + Constants.Apostophe + SubgroupValGId + Constants.Apostophe + Constants.AND + Area.AreaID + Constants.EqualsTo + Constants.Apostophe + AreaID.Replace("'", "''") + Constants.Apostophe + Constants.AND + IndicatorClassifications.ICName + Constants.EqualsTo + Constants.Apostophe + Source.Replace("'", "''") + Constants.Apostophe);

                                foreach (DataRow DrIUSA in IUSARows)
                                {
                                    DictObsAttributes = new Dictionary<string, string>();

                                    DictObsAttributes.Add(Constants.Concept.OBS_VALUE.Id, DrIUSA[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.DataValue].ToString());
                                    DictObsAttributes.Add(Constants.Concept.TIME_PERIOD.Id, DrIUSA[Timeperiods.TimePeriod].ToString());

                                    DictObsAttributes.Add(Constants.Concept.CONFIDENCE_INTERVAL_LOWER.Id, DrIUSA["ConfidenceIntervalLower"].ToString());
                                    DictObsAttributes.Add(Constants.Concept.CONFIDENCE_INTERVAL_UPPER.Id, DrIUSA["ConfidenceIntervalUpper"].ToString());
                                    DictObsAttributes.Add(Constants.Concept.NATURE.Id, DrIUSA["Nature"].ToString());
                                    DictObsAttributes.Add(Constants.Concept.PERIODICITY.Id, DrIUSA["Periodicity"].ToString());
                                    DictObsAttributes.Add(Constants.Concept.DENOMINATOR.Id, DrIUSA[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.DataDenominator].ToString());
                                    DictObsAttributes.Add(Constants.Concept.FOOTNOTES.Id, DrIUSA[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.FootNotes.FootNote].ToString());
                                    Obs = new ObsType(DictObsAttributes);
                                    Series.Obs.Add(Obs);
                                    Obs.Annotations = null;
                                }
                            }
                        }
                    }

                    if (StructureSpecificTimeSeriesData != null && StructureSpecificTimeSeriesData.DataSet != null && StructureSpecificTimeSeriesData.DataSet.Count > 0 && StructureSpecificTimeSeriesData.DataSet[0].Items != null && StructureSpecificTimeSeriesData.DataSet[0].Items.Count > 0)
                    {
                        RetVal = Serializer.SerializeToXmlDocument(typeof(StructureSpecificTimeSeriesDataType), StructureSpecificTimeSeriesData);
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
            fileCount = 0;
            SDMXObjectModel.Message.StructureSpecificTimeSeriesDataQueryType StructureSpecificTimeSeriesDataQuery;
            StructureSpecificTimeSeriesDataType StructureSpecificTimeSeriesData;
            TimeSeriesDataSetType Dataset;
            SeriesType Series;
            ObsType Obs;
            Dictionary<string, string> DictSeriesAttributes, DictObsAttributes, DictSubgroupBreakup;
            DataTable DtSubgroup, DtSubgroupTypes, DtData, DtDistinctIUS, DtDistinctIUSA;
            string IndicatorGId, UnitGId, SubgroupValGId, AreaID, Source;
            DataRow[] IUSRows, IUSARows;
            GeneratedFiles = new List<string>();
            DateTime CurrentTime = DateTime.Now;
            try
            {
                StructureSpecificTimeSeriesDataQuery = (SDMXObjectModel.Message.StructureSpecificTimeSeriesDataQueryType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureSpecificTimeSeriesDataQueryType), query);

                this.Parse_Query(StructureSpecificTimeSeriesDataQuery.Query.DataWhere, StructureSpecificTimeSeriesDataQuery.Query.ReturnDetails);
                this.Create_Folder_Structure(outputFolder);

                DtSubgroup = this.DIConnection.ExecuteDataTable(this.DIQueries.SubgroupValSubgroup.GetSubgroupValsWithSubgroups());
                DtSubgroupTypes = this.DIConnection.ExecuteDataTable(this.DIQueries.SubgroupTypes.GetSubgroupTypes(FilterFieldType.None, string.Empty));
                FileName = string.Empty;

                SDMXUtility.Raise_Initilize_Process_Event("Generating SDMX files...", this.Languages.Count, this.Languages.Count);

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

                            StructureSpecificTimeSeriesData = new StructureSpecificTimeSeriesDataType();
                            StructureSpecificTimeSeriesData.DataSet = new List<TimeSeriesDataSetType>();

                            //StructureSpecificTimeSeriesData.Header = this.Get_Appropriate_Header();

                            if (Header == null)
                            {
                                StructureSpecificTimeSeriesData.Header = this.Get_Appropriate_Header();
                            }
                            else
                            {
                                StructureSpecificTimeSeriesData.Header.ID = Header.ID;
                                StructureSpecificTimeSeriesData.Header.Name = Header.Name;
                                StructureSpecificTimeSeriesData.Header.Prepared = Header.Prepared;
                                StructureSpecificTimeSeriesData.Header.Receiver = Header.Receiver;
                                StructureSpecificTimeSeriesData.Header.Sender = Header.Sender;
                                StructureSpecificTimeSeriesData.Header.Test = Header.Test;
                            }
                            StructureSpecificTimeSeriesData.Header.Structure = new List<SDMXObjectModel.Common.StructureSpecificDataTimeSeriesStructureType>();
                            StructureSpecificTimeSeriesData.Header.Structure.Add(new SDMXObjectModel.Common.StructureSpecificDataTimeSeriesStructureType());

                            StructureSpecificTimeSeriesData.Header.Structure[0].dimensionAtObservation = Constants.Concept.TIME_PERIOD.Id;
                            StructureSpecificTimeSeriesData.Header.Structure[0].structureID = Constants.DSD.Id;
                            StructureSpecificTimeSeriesData.Header.Structure[0].@namespace = Constants.defaultNamespace;

                            StructureSpecificTimeSeriesData.Header.Structure[0].Item = new DataStructureReferenceType();
                            ((DataStructureReferenceType)StructureSpecificTimeSeriesData.Header.Structure[0].Item).Items.Add(new DataStructureRefType());
                            ((DataStructureRefType)((DataStructureReferenceType)StructureSpecificTimeSeriesData.Header.Structure[0].Item).Items[0]).id = Constants.DSD.Id;
                            ((DataStructureRefType)((DataStructureReferenceType)StructureSpecificTimeSeriesData.Header.Structure[0].Item).Items[0]).agencyID = this.AgencyId;
                            ((DataStructureRefType)((DataStructureReferenceType)StructureSpecificTimeSeriesData.Header.Structure[0].Item).Items[0]).version = Constants.DSD.Version;
                            StructureSpecificTimeSeriesData.Footer = null;

                            Dataset = new TimeSeriesDataSetType();
                            StructureSpecificTimeSeriesData.DataSet.Add(Dataset);
                            Dataset.structureRef = Constants.DSD.Id;
                            Dataset.DataProvider = null;
                            Dataset.Annotations = null;
                            Dataset.Items = new List<SDMXObjectModel.Common.AnnotableType>();

                            IUSRows = DtDistinctIUSA.Select(Indicator.IndicatorGId + Constants.EqualsTo + Constants.Apostophe + IndicatorGId + Constants.Apostophe + Constants.AND + Unit.UnitGId + Constants.EqualsTo + Constants.Apostophe + UnitGId + Constants.Apostophe + Constants.AND + SubgroupVals.SubgroupValGId + Constants.EqualsTo + Constants.Apostophe + SubgroupValGId + Constants.Apostophe);

                            foreach (DataRow DrIUS in IUSRows)
                            {
                                AreaID = DrIUS[Area.AreaID].ToString();
                                Source = DrIUS[IndicatorClassifications.ICName].ToString();

                                DictSeriesAttributes = new Dictionary<string, string>();

                                DictSeriesAttributes.Add(Constants.Concept.INDICATOR.Id, IndicatorGId);
                                DictSeriesAttributes.Add(Constants.Concept.UNIT.Id, UnitGId);

                                DictSubgroupBreakup = this.Get_Subgroup_Breakup(SubgroupValGId, DtSubgroup, DtSubgroupTypes);
                                foreach (string Key in DictSubgroupBreakup.Keys)
                                {
                                    DictSeriesAttributes.Add(Key, DictSubgroupBreakup[Key]);
                                }

                                DictSeriesAttributes.Add(Constants.Concept.AREA.Id, AreaID);
                                DictSeriesAttributes.Add(Constants.Concept.SOURCE.Id, Source);

                                Series = new SeriesType(DictSeriesAttributes, null);
                                Dataset.Items.Add(Series);
                                Series.Annotations = null;
                                Series.Obs = new List<ObsType>();

                                IUSARows = DtData.Select(Indicator.IndicatorGId + Constants.EqualsTo + Constants.Apostophe + IndicatorGId + Constants.Apostophe + Constants.AND + Unit.UnitGId + Constants.EqualsTo + Constants.Apostophe + UnitGId + Constants.Apostophe + Constants.AND + SubgroupVals.SubgroupValGId + Constants.EqualsTo + Constants.Apostophe + SubgroupValGId + Constants.Apostophe + Constants.AND + Area.AreaID + Constants.EqualsTo + Constants.Apostophe + AreaID.Replace("'", "''") + Constants.Apostophe + Constants.AND + IndicatorClassifications.ICName + Constants.EqualsTo + Constants.Apostophe + Source.Replace("'", "''") + Constants.Apostophe);

                                foreach (DataRow DrIUSA in IUSARows)
                                {
                                    DictObsAttributes = new Dictionary<string, string>();

                                    DictObsAttributes.Add(Constants.Concept.OBS_VALUE.Id, DrIUSA[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.DataValue].ToString());
                                    DictObsAttributes.Add(Constants.Concept.TIME_PERIOD.Id, DrIUSA[Timeperiods.TimePeriod].ToString());

                                    DictObsAttributes.Add(Constants.Concept.CONFIDENCE_INTERVAL_LOWER.Id, DrIUSA["ConfidenceIntervalLower"].ToString());
                                    DictObsAttributes.Add(Constants.Concept.CONFIDENCE_INTERVAL_UPPER.Id, DrIUSA["ConfidenceIntervalUpper"].ToString());
                                    DictObsAttributes.Add(Constants.Concept.NATURE.Id, DrIUSA["Nature"].ToString());
                                    DictObsAttributes.Add(Constants.Concept.PERIODICITY.Id, DrIUSA["Periodicity"].ToString());
                                    DictObsAttributes.Add(Constants.Concept.DENOMINATOR.Id, DrIUSA[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.DataDenominator].ToString());
                                    DictObsAttributes.Add(Constants.Concept.FOOTNOTES.Id, DrIUSA[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.FootNotes.FootNote].ToString());
                                    Obs = new ObsType(DictObsAttributes);
                                    Series.Obs.Add(Obs);
                                    Obs.Annotations = null;
                                }
                            }

                            Serializer.SerializeToFile(typeof(StructureSpecificTimeSeriesDataType), StructureSpecificTimeSeriesData, FileName);
                            GeneratedFiles.Add(FileName);
                            //-- Raise event for generated file
                            SDMXUtility.Raise_Notify_File_Name_Event(FileName);
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
           
            SDMXObjectModel.Message.StructureSpecificTimeSeriesDataQueryType StructureSpecificTimeSeriesDataQuery;
            StructureSpecificTimeSeriesDataType StructureSpecificTimeSeriesData;
            TimeSeriesDataSetType Dataset;
            SeriesType Series;
            ObsType Obs;
            Dictionary<string, string> DictSeriesAttributes, DictObsAttributes, DictSubgroupBreakup;
            DataTable DtSubgroup, DtSubgroupTypes, DtData, DtDistinctIUS, DtDistinctIUSA;
            string IndicatorGId, UnitGId, SubgroupValGId, AreaID, Source;
            DataRow[] IUSRows, IUSARows;
         
            DateTime CurrentTime = DateTime.Now;
            try
            {
                StructureSpecificTimeSeriesDataQuery = (SDMXObjectModel.Message.StructureSpecificTimeSeriesDataQueryType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureSpecificTimeSeriesDataQueryType), query);

                this.Parse_Query(StructureSpecificTimeSeriesDataQuery.Query.DataWhere, StructureSpecificTimeSeriesDataQuery.Query.ReturnDetails);
                this.Create_Folder_Structure(outputFolder);

                DtSubgroup = this.DIConnection.ExecuteDataTable(this.DIQueries.SubgroupValSubgroup.GetSubgroupValsWithSubgroups());
                DtSubgroupTypes = this.DIConnection.ExecuteDataTable(this.DIQueries.SubgroupTypes.GetSubgroupTypes(FilterFieldType.None, string.Empty));
                FileName = string.Empty;

                SDMXUtility.Raise_Initilize_Process_Event("Generating SDMX files...", this.Languages.Count, this.Languages.Count);

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

                            StructureSpecificTimeSeriesData = new StructureSpecificTimeSeriesDataType();
                            StructureSpecificTimeSeriesData.DataSet = new List<TimeSeriesDataSetType>();

                            StructureSpecificTimeSeriesData.Header = this.Get_Appropriate_Header();
                            StructureSpecificTimeSeriesData.Header.Structure = new List<SDMXObjectModel.Common.StructureSpecificDataTimeSeriesStructureType>();
                            StructureSpecificTimeSeriesData.Header.Structure.Add(new SDMXObjectModel.Common.StructureSpecificDataTimeSeriesStructureType());

                            StructureSpecificTimeSeriesData.Header.Structure[0].dimensionAtObservation = Constants.Concept.TIME_PERIOD.Id;
                            StructureSpecificTimeSeriesData.Header.Structure[0].structureID = Constants.DSD.Id;
                            StructureSpecificTimeSeriesData.Header.Structure[0].@namespace = Constants.defaultNamespace;

                            StructureSpecificTimeSeriesData.Header.Structure[0].Item = new DataStructureReferenceType();
                            ((DataStructureReferenceType)StructureSpecificTimeSeriesData.Header.Structure[0].Item).Items.Add(new DataStructureRefType());
                            ((DataStructureRefType)((DataStructureReferenceType)StructureSpecificTimeSeriesData.Header.Structure[0].Item).Items[0]).id = Constants.DSD.Id;
                            ((DataStructureRefType)((DataStructureReferenceType)StructureSpecificTimeSeriesData.Header.Structure[0].Item).Items[0]).agencyID = this.AgencyId;
                            ((DataStructureRefType)((DataStructureReferenceType)StructureSpecificTimeSeriesData.Header.Structure[0].Item).Items[0]).version = Constants.DSD.Version;
                            StructureSpecificTimeSeriesData.Footer = null;

                            Dataset = new TimeSeriesDataSetType();
                            StructureSpecificTimeSeriesData.DataSet.Add(Dataset);
                            Dataset.structureRef = Constants.DSD.Id;
                            Dataset.DataProvider = null;
                            Dataset.Annotations = null;
                            Dataset.Items = new List<SDMXObjectModel.Common.AnnotableType>();

                            IUSRows = DtDistinctIUSA.Select(Indicator.IndicatorGId + Constants.EqualsTo + Constants.Apostophe + IndicatorGId + Constants.Apostophe + Constants.AND + Unit.UnitGId + Constants.EqualsTo + Constants.Apostophe + UnitGId + Constants.Apostophe + Constants.AND + SubgroupVals.SubgroupValGId + Constants.EqualsTo + Constants.Apostophe + SubgroupValGId + Constants.Apostophe);

                            foreach (DataRow DrIUS in IUSRows)
                            {
                                AreaID = DrIUS[Area.AreaID].ToString();
                                Source = DrIUS[IndicatorClassifications.ICName].ToString();

                                DictSeriesAttributes = new Dictionary<string, string>();

                                DictSeriesAttributes.Add(Constants.Concept.INDICATOR.Id, IndicatorGId);
                                DictSeriesAttributes.Add(Constants.Concept.UNIT.Id, UnitGId);

                                DictSubgroupBreakup = this.Get_Subgroup_Breakup(SubgroupValGId, DtSubgroup, DtSubgroupTypes);
                                foreach (string Key in DictSubgroupBreakup.Keys)
                                {
                                    DictSeriesAttributes.Add(Key, DictSubgroupBreakup[Key]);
                                }

                                DictSeriesAttributes.Add(Constants.Concept.AREA.Id, AreaID);
                                DictSeriesAttributes.Add(Constants.Concept.SOURCE.Id, Source);

                                Series = new SeriesType(DictSeriesAttributes, null);
                                Dataset.Items.Add(Series);
                                Series.Annotations = null;
                                Series.Obs = new List<ObsType>();

                                IUSARows = DtData.Select(Indicator.IndicatorGId + Constants.EqualsTo + Constants.Apostophe + IndicatorGId + Constants.Apostophe + Constants.AND + Unit.UnitGId + Constants.EqualsTo + Constants.Apostophe + UnitGId + Constants.Apostophe + Constants.AND + SubgroupVals.SubgroupValGId + Constants.EqualsTo + Constants.Apostophe + SubgroupValGId + Constants.Apostophe + Constants.AND + Area.AreaID + Constants.EqualsTo + Constants.Apostophe + AreaID.Replace("'", "''") + Constants.Apostophe + Constants.AND + IndicatorClassifications.ICName + Constants.EqualsTo + Constants.Apostophe + Source.Replace("'", "''") + Constants.Apostophe);

                                foreach (DataRow DrIUSA in IUSARows)
                                {
                                    DictObsAttributes = new Dictionary<string, string>();

                                    DictObsAttributes.Add(Constants.Concept.OBS_VALUE.Id, DrIUSA[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.DataValue].ToString());
                                    DictObsAttributes.Add(Constants.Concept.TIME_PERIOD.Id, DrIUSA[Timeperiods.TimePeriod].ToString());

                                    DictObsAttributes.Add(Constants.Concept.CONFIDENCE_INTERVAL_LOWER.Id, DrIUSA["ConfidenceIntervalLower"].ToString());
                                    DictObsAttributes.Add(Constants.Concept.CONFIDENCE_INTERVAL_UPPER.Id, DrIUSA["ConfidenceIntervalUpper"].ToString());
                                    DictObsAttributes.Add(Constants.Concept.NATURE.Id, DrIUSA["Nature"].ToString());
                                    DictObsAttributes.Add(Constants.Concept.PERIODICITY.Id, DrIUSA["Periodicity"].ToString());
                                    DictObsAttributes.Add(Constants.Concept.DENOMINATOR.Id, DrIUSA[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.DataDenominator].ToString());
                                    DictObsAttributes.Add(Constants.Concept.FOOTNOTES.Id, DrIUSA[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.FootNotes.FootNote].ToString());
                                    Obs = new ObsType(DictObsAttributes);
                                    Series.Obs.Add(Obs);
                                    Obs.Annotations = null;
                                }
                            }

                            Serializer.SerializeToFile(typeof(StructureSpecificTimeSeriesDataType), StructureSpecificTimeSeriesData, FileName);
                         
                            //-- Raise event for generated file
                            SDMXUtility.Raise_Notify_File_Name_Event(FileName);
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
