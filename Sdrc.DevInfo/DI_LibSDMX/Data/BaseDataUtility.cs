using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Data;
using SDMXObjectModel.Query;
using SDMXObjectModel.Common;
using System.IO;
using System.Text.RegularExpressions;

namespace DevInfo.Lib.DI_LibSDMX
{
    internal class BaseDataUtility
    {
        #region "Properties"

        #region "Private"

        private DIConnection _diConnection;

        private DIQueries _diQueries;

        private List<string> _languages;

        private string _iusNIds;

        private List<string> _areaIds;

        private string _areaNIds;

        private List<string> _sourceTexts;

        private string _sourceNIds;

        private string _timeperiodNIds;

        private string _agencyID;

        private bool _isMRD;

        private DataReturnDetailTypes _dataReturnDetailType;

        #endregion "Private"

        #region "Public"

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

        internal List<string> Languages
        {
            get
            {
                return this._languages;
            }
            set
            {
                this._languages = value;
            }
        }

        internal string IUSNIds
        {
            get
            {
                return this._iusNIds;
            }
            set
            {
                this._iusNIds = value;
            }
        }

        internal List<string> AreaIds
        {
            get
            {
                return this._areaIds;
            }
            set
            {
                this._areaIds = value;
            }
        }

        internal string AreaNIds
        {
            get
            {
                return this._areaNIds;
            }
            set
            {
                this._areaNIds = value;
            }
        }

        internal List<string> SourceTexts
        {
            get
            {
                return this._sourceTexts;
            }
            set
            {
                this._sourceTexts = value;
            }
        }

        internal string SourceNIds
        {
            get
            {
                return this._sourceNIds;
            }
            set
            {
                this._sourceNIds = value;
            }
        }

        internal string TimePeriodNIds
        {
            get
            {
                return this._timeperiodNIds;
            }
            set
            {
                this._timeperiodNIds = value;
            }
        }

        internal string AgencyId
        {
            get
            {
                return this._agencyID;
            }
            set
            {
                this._agencyID = value;
            }
        }

        internal bool IsMRD
        {
            get
            {
                return this._isMRD;
            }
            set
            {
                this._isMRD = value;
            }
        }

        internal DataReturnDetailTypes DataReturnDetailType
        {
            get
            {
                return this._dataReturnDetailType;
            }
            set
            {
                this._dataReturnDetailType = value;
            }
        }

        #endregion "Public"

        #endregion "Properties"

        #region "Constructors"

        #region "Private"

        #endregion "Private"

        #region "Public"

        internal BaseDataUtility(DIConnection DIConnection, DIQueries DIQueries, string agencyId)
        {
            this._diConnection = DIConnection;
            this._diQueries = DIQueries;
            this._agencyID = agencyId;
        }

        #endregion "Public"

        #endregion "Constructors"

        #region "Methods"

        #region "Private"

        private void Create_Directory_If_Not_Exists(string RetVal)
        {
            if (!Directory.Exists(RetVal))
            {
                Directory.CreateDirectory(RetVal);
            }
        }

        protected string Get_SubgroupVal_GId(Dictionary<string, string> DictSubgroupBreakup, DataTable DtSubgroupBreakup)
        {
            string RetVal;
            bool IsSetFlag;
            string SubgroupValGId, SubgroupTypeGId, SubgroupGId;
            List<string> ProcessedSubgroupValGIds;
            DataRow[] SubgroupValRows;

            RetVal = string.Empty;
            IsSetFlag = false;
            SubgroupValGId = string.Empty;
            SubgroupTypeGId = string.Empty;
            SubgroupGId = string.Empty;
            ProcessedSubgroupValGIds = new List<string>();
            SubgroupValRows = null;

            try
            {
                foreach (DataRow DrSubgroupBreakup in DtSubgroupBreakup.Rows)
                {
                    SubgroupValGId = DrSubgroupBreakup[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId].ToString();

                    if (!ProcessedSubgroupValGIds.Contains(SubgroupValGId))
                    {
                        ProcessedSubgroupValGIds.Add(SubgroupValGId);

                        SubgroupValRows = DtSubgroupBreakup.Select(DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId + Constants.EqualsTo + Constants.Apostophe + SubgroupValGId + Constants.Apostophe);

                        if (SubgroupValRows.Length == DictSubgroupBreakup.Count)
                        {
                            foreach (DataRow DrSubgroupVal in SubgroupValRows)
                            {
                                SubgroupTypeGId = DrSubgroupVal[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupTypes.SubgroupTypeGID].ToString();
                                SubgroupGId = DrSubgroupVal[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Subgroup.SubgroupGId].ToString();

                                if (DictSubgroupBreakup.ContainsKey(SubgroupTypeGId) && DictSubgroupBreakup[SubgroupTypeGId] == SubgroupGId)
                                {
                                    IsSetFlag = true;
                                }
                                else
                                {
                                    IsSetFlag = false;
                                    break;
                                }
                            }

                            if (IsSetFlag == true)
                            {
                                RetVal = SubgroupValGId;
                                break;
                            }
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

            return RetVal;
        }

        private string Get_IUSNId_From_GIds(string IndicatorGId, string UnitGId, string SubgroupValGId, DataTable DtIUS)
        {
            string RetVal;
            string WhereClause;
            DataRow[] IUSRows;

            RetVal = string.Empty;
            WhereClause = string.Empty;
            IUSRows = null;

            try
            {
                WhereClause += DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId + DevInfo.Lib.DI_LibSDMX.Constants.EqualsTo + DevInfo.Lib.DI_LibSDMX.Constants.Apostophe + IndicatorGId + DevInfo.Lib.DI_LibSDMX.Constants.Apostophe;

                WhereClause += Constants.AND;

                WhereClause += DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitGId + DevInfo.Lib.DI_LibSDMX.Constants.EqualsTo + DevInfo.Lib.DI_LibSDMX.Constants.Apostophe + UnitGId + DevInfo.Lib.DI_LibSDMX.Constants.Apostophe;

                WhereClause += Constants.AND;

                WhereClause += DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId + DevInfo.Lib.DI_LibSDMX.Constants.EqualsTo + DevInfo.Lib.DI_LibSDMX.Constants.Apostophe + SubgroupValGId + DevInfo.Lib.DI_LibSDMX.Constants.Apostophe;

                IUSRows = DtIUS.Select(WhereClause);

                if (IUSRows.Length == 1)
                {
                    RetVal = IUSRows[0][DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator_Unit_Subgroup.IUSNId].ToString();
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

        private string Get_TimePeriodNIds_From_Text(List<string> TimePeriods)
        {
            string RetVal;
            List<string> TimePeriodNIds;
            string TimePeriodNId;
            DataTable DtTimePeriod;

            RetVal = string.Empty;
            TimePeriodNIds = new List<string>();
            TimePeriodNId = string.Empty;
            DtTimePeriod = null;

            try
            {
                DtTimePeriod = this._diConnection.ExecuteDataTable(this._diQueries.Timeperiod.GetTimePeriod(FilterFieldType.None, string.Empty));
                TimePeriodNIds.Add(Constants.MinusOne);

                foreach (DataRow DrTimePeriod in DtTimePeriod.Rows)
                {
                    if (TimePeriods != null && TimePeriods.Count > 0)
                    {
                        if (TimePeriods.Contains(DrTimePeriod[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriod].ToString().Trim()))
                        {
                            TimePeriodNIds.Add(DrTimePeriod[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriodNId].ToString().Trim());
                        }
                    }
                    else
                    {
                        TimePeriodNIds.Add(DrTimePeriod[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriodNId].ToString().Trim());
                    }
                }

                if (TimePeriodNIds != null && TimePeriodNIds.Count > 0)
                {
                    RetVal = String.Join(Constants.Comma, TimePeriodNIds.ToArray());
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

        #endregion "Private"

        #region "Public"

        internal virtual XmlDocument Get_Data(XmlDocument query)
        {
            return new XmlDocument();
        }

        internal virtual bool Generate_Data(XmlDocument query, string outputFolder, out int fileCount, out List<string> GeneratedFiles, SDMXObjectModel.Message.StructureHeaderType Header)
        {
            fileCount = 0;
            GeneratedFiles = new List<string>();
            return true;
        }

        internal virtual bool Generate_Data(XmlDocument query, string outputFolder)
        {
              return true;
        }
        internal void Parse_Query(DataParametersAndType DataWhere, DataReturnDetailsType ReturnDetails)
        {
            string IndicatorGId, UnitGId, SubgroupValGId, AreaId, SourceText, TimePeriodText, Language, IUSNId;
            List<string> IUSNIds, AreaIds, SourceTexts, TimePeriods;
            Dictionary<string, string> DictSubgroupBreakup;
            DataTable DtSubgroupBreakup, DtIUS;

            IndicatorGId = string.Empty;
            UnitGId = string.Empty;
            SubgroupValGId = string.Empty;
            AreaId = string.Empty;
            SourceText = string.Empty;
            TimePeriodText = string.Empty;
            Language = string.Empty;
            IUSNId = string.Empty;

            IUSNIds = new List<string>();
            AreaIds = new List<string>();
            SourceTexts = new List<string>();
            TimePeriods = new List<string>();
            DictSubgroupBreakup = new Dictionary<string, string>();

            DtSubgroupBreakup = this._diConnection.ExecuteDataTable(this._diQueries.SubgroupValSubgroup.GetSubgroupValsWithSubgroups());
            DtSubgroupBreakup = DtSubgroupBreakup.DefaultView.ToTable(true, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupTypes.SubgroupTypeGID, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Subgroup.SubgroupGId);

            DtIUS = this._diConnection.ExecuteDataTable(this._diQueries.IUS.GetIUS(FilterFieldType.None, string.Empty, FieldSelection.Light));
            DtIUS = DtIUS.DefaultView.ToTable(true, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator_Unit_Subgroup.IUSNId, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitGId, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId);

            if ((DataReturnDetailTypes)Enum.Parse(typeof(DataReturnDetailTypes), ReturnDetails.detail) == DataReturnDetailTypes.Full)
            {
                if (DataWhere != null)
                {
                    foreach (DataParametersOrType ORItem in DataWhere.Or)
                    {
                        if (ORItem.DimensionValue.Count > 0)
                        {
                            foreach (DimensionValueType Dimension in ORItem.DimensionValue)
                            {
                                switch (Dimension.ID)
                                {
                                    case Constants.Concept.AREA.Id:
                                        AreaId = ((SDMXObjectModel.Query.SimpleValueType)Dimension.Items[0]).Value;
                                        AreaIds.Add(AreaId);
                                        break;
                                    case Constants.Concept.SOURCE.Id:
                                        SourceText = ((SDMXObjectModel.Query.SimpleValueType)Dimension.Items[0]).Value;
                                        SourceTexts.Add(SourceText);
                                        break;
                                    case Constants.Concept.TIME_PERIOD.Id:
                                        TimePeriodText = ((SDMXObjectModel.Query.SimpleValueType)Dimension.Items[0]).Value;
                                        if (this._isMRD == false)
                                        {
                                            if (TimePeriodText == Constants.MRD)
                                            {
                                                this._isMRD = true;
                                                TimePeriods = null;
                                            }
                                            else
                                            {
                                                TimePeriods.Add(TimePeriodText);
                                            }
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        else if (ORItem.AttributeValue.Count > 0)
                        {
                            switch (ORItem.AttributeValue[0].ID)
                            {
                                case Constants.Concept.FOOTNOTES.Id:
                                    Language = ((QueryTextType)ORItem.AttributeValue[0].Items[0]).lang;
                                    break;
                            }
                        }
                        else if (ORItem.And.Count > 0)
                        {
                            IUSNIds.Add(Constants.MinusOne);

                            foreach (DataParametersAndType ANDItem in ORItem.And)
                            {
                                DictSubgroupBreakup = new Dictionary<string, string>();

                                foreach (DimensionValueType Dimension in ANDItem.DimensionValue)
                                {
                                    switch (Dimension.ID)
                                    {
                                        case Constants.Concept.INDICATOR.Id:
                                            IndicatorGId = ((SDMXObjectModel.Query.SimpleValueType)Dimension.Items[0]).Value;
                                            break;
                                        case Constants.Concept.UNIT.Id:
                                            UnitGId = ((SDMXObjectModel.Query.SimpleValueType)Dimension.Items[0]).Value;
                                            break;
                                        default:
                                            DictSubgroupBreakup.Add(Dimension.ID, ((SDMXObjectModel.Query.SimpleValueType)Dimension.Items[0]).Value);
                                            break;
                                    }
                                }

                                SubgroupValGId = this.Get_SubgroupVal_GId(DictSubgroupBreakup, DtSubgroupBreakup);
                                IUSNId = this.Get_IUSNId_From_GIds(IndicatorGId, UnitGId, SubgroupValGId, DtIUS);
                                IUSNIds.Add(IUSNId);
                            }
                        }
                    }

                    if (DataWhere.DataStructure != null && DataWhere.DataStructure.Count > 0 && DataWhere.DataStructure[0].Items != null &&
                        DataWhere.DataStructure[0].Items.Count > 0)
                    {
                        this._agencyID = ((DataStructureRefType)DataWhere.DataStructure[0].Items[0]).agencyID;
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
            }
            else if ((DataReturnDetailTypes)Enum.Parse(typeof(DataReturnDetailTypes), ReturnDetails.detail) == DataReturnDetailTypes.SeriesKeyOnly)
            {
                this._agencyID = "agency";
            }

            this._iusNIds = String.Join(Constants.Comma, IUSNIds.ToArray());
            this._areaIds = AreaIds;
            this._sourceTexts = SourceTexts;

            if (this.IsMRD == false)
            {
                this._timeperiodNIds = this.Get_TimePeriodNIds_From_Text(TimePeriods);
            }

            this._languages = new List<string>();

            if (string.IsNullOrEmpty(Language))
            {
                foreach (DataRow LanguageRow in this._diConnection.DILanguages(this._diQueries.DataPrefix).Rows)
                {
                    this._languages.Add(LanguageRow[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Language.LanguageCode].ToString());
                }
            }
            else
            {
                if (this._diConnection.IsValidDILanguage(this._diQueries.DataPrefix, Language))
                {
                    this._languages.Add(Language);
                }
                else
                {
                    this._languages.Add(this._diQueries.LanguageCode.Substring(1));
                }
            }

            this._dataReturnDetailType = (DataReturnDetailTypes)Enum.Parse(typeof(DataReturnDetailTypes), ReturnDetails.detail);
        }

        internal void Set_Area_NIds(string language)
        {
            List<string> AreaNIds;
            DIQueries DIQueriesLanguage;
            DataTable DtArea;

            AreaNIds = new List<string>();
            DIQueriesLanguage = null;
            DtArea = null;

            try
            {
                DIQueriesLanguage = new DIQueries(this._diQueries.DataPrefix, language);
                DtArea = this._diConnection.ExecuteDataTable(DIQueriesLanguage.Area.GetArea(FilterFieldType.None, string.Empty));
                AreaNIds.Add(Constants.MinusOne);

                foreach (DataRow DrArea in DtArea.Rows)
                {
                    if (this._areaIds != null && this._areaIds.Count > 0)
                    {
                        if (this._areaIds.Contains(DrArea[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaID].ToString().Trim()))
                        {
                            AreaNIds.Add(DrArea[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaNId].ToString().Trim());
                        }
                    }
                    else
                    {
                        AreaNIds.Add(DrArea[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaNId].ToString().Trim());
                    }
                }

                if (AreaNIds != null && AreaNIds.Count > 0)
                {
                    this._areaNIds = String.Join(Constants.Comma, AreaNIds.ToArray());
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

        internal void Set_Source_NIds(string language)
        {
            List<string> SourceNIds;
            DIQueries DIQueriesLanguage;
            DataTable DtSource;

            SourceNIds = new List<string>();
            DIQueriesLanguage = null;
            DtSource = null;

            try
            {
                DIQueriesLanguage = new DIQueries(this._diQueries.DataPrefix, language);
                DtSource = this._diConnection.ExecuteDataTable(DIQueriesLanguage.IndicatorClassification.GetIC(FilterFieldType.None, string.Empty, ICType.Source, FieldSelection.Light));
                SourceNIds.Add(Constants.MinusOne);

                foreach (DataRow DrSource in DtSource.Rows)
                {
                    if (this._sourceTexts != null && this._sourceTexts.Count > 0)
                    {
                        if (this._sourceTexts.Contains(DrSource[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICName].ToString().Trim()))
                        {
                            SourceNIds.Add(DrSource[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICNId].ToString().Trim());
                        }
                    }
                    else
                    {
                        SourceNIds.Add(DrSource[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICNId].ToString().Trim());
                    }
                }

                if (SourceNIds != null && SourceNIds.Count > 0)
                {
                    this._sourceNIds = String.Join(Constants.Comma, SourceNIds.ToArray());
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

        internal DataTable Get_Distinct_IUS_Table(string language)
        {
            DataTable RetVal;
            string Query;

            RetVal = null;
            Query = string.Empty;

            try
            {
                Query = "SELECT DISTINCT [Indicator].Indicator_GId, Unit.Unit_GId, SGV.Subgroup_Val_GId FROM";

                Query += " ";
                Query += "(SELECT Indicator_NId, Unit_NId, Subgroup_Val_NId FROM UT_Data WHERE 1 = 1";

                if (!string.IsNullOrEmpty(this._iusNIds))
                {
                    Query += " ";
                    Query += "AND IUSNId IN";
                    Query += " ";
                    Query += "(" + this._iusNIds + ")";
                }

                if (!string.IsNullOrEmpty(this._areaNIds))
                {
                    Query += " ";
                    Query += "AND Area_NID IN";
                    Query += " ";
                    Query += "(" + this._areaNIds + ")";
                }

                if (!string.IsNullOrEmpty(this._sourceNIds))
                {
                    Query += " ";
                    Query += "AND Source_NID IN";
                    Query += " ";
                    Query += "(" + this._sourceNIds + ")";
                }

                if (this.IsMRD == false)
                {
                    if (!string.IsNullOrEmpty(this._timeperiodNIds))
                    {
                        Query += " ";
                        Query += "AND TimePeriod_NId IN";
                        Query += " ";
                        Query += "(" + this._timeperiodNIds + ")";
                    }
                }
                else
                {
                    Query += " ";
                    Query += "AND isMRD = 1";
                }

                Query += ")";
                Query += " ";
                Query += " AS Data,";

                Query += " ";
                Query += "(SELECT Indicator_NId, Indicator_GId FROM UT_Indicator_" + language + ") AS [Indicator],";

                Query += "(SELECT Unit_NId, Unit_GId FROM UT_Unit_" + language + ") AS Unit,";

                Query += " ";
                Query += "(SELECT Subgroup_Val_NId, Subgroup_Val_GId FROM UT_Subgroup_Vals_" + language + ")AS SGV "
                + " WHERE (Data.Subgroup_Val_NId = SGV.Subgroup_Val_NId AND Data.Unit_NId = Unit.Unit_NId) AND Data.Indicator_NId = [Indicator].Indicator_NId ";


                RetVal = this._diConnection.ExecuteDataTable(Regex.Replace(Query, "UT_", this._diConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));
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

        internal DataTable Get_Distinct_IUSA_Table(string language)
        {
            DataTable RetVal;
            string Query;

            RetVal = null;
            Query = string.Empty;

            try
            {
                Query = "SELECT DISTINCT [Indicator].Indicator_GId, Unit.Unit_GId, SGV.Subgroup_Val_GId, Area.Area_ID, Source.IC_Name FROM";

                Query += " ";
                Query += "(SELECT Indicator_NId, Unit_NId, Subgroup_Val_NId, Area_NId, Source_NId FROM UT_Data WHERE 1 = 1";

                if (!string.IsNullOrEmpty(this._iusNIds))
                {
                    Query += " ";
                    Query += "AND IUSNId IN";
                    Query += " ";
                    Query += "(" + this._iusNIds + ")";
                }

                if (!string.IsNullOrEmpty(this._areaNIds))
                {
                    Query += " ";
                    Query += "AND Area_NID IN";
                    Query += " ";
                    Query += "(" + this._areaNIds + ")";
                }

                if (!string.IsNullOrEmpty(this._sourceNIds))
                {
                    Query += " ";
                    Query += "AND Source_NID IN";
                    Query += " ";
                    Query += "(" + this._sourceNIds + ")";
                }

                if (this.IsMRD == false)
                {
                    if (!string.IsNullOrEmpty(this._timeperiodNIds))
                    {
                        Query += " ";
                        Query += "AND TimePeriod_NId IN";
                        Query += " ";
                        Query += "(" + this._timeperiodNIds + ")";
                    }
                }
                else
                {
                    Query += " ";
                    Query += "AND isMRD = 1";
                }

                Query += ")";
                Query += " ";
                Query += "Data ,";

                Query += " ";
                Query += "(SELECT Indicator_NId, Indicator_GId FROM UT_Indicator_" + language + ") [Indicator] ,";

                Query += " ";
                Query += "(SELECT Unit_NId, Unit_GId FROM UT_Unit_" + language + ") Unit ,";

                Query += " ";
                Query += "(SELECT Subgroup_Val_NId, Subgroup_Val_GId FROM UT_Subgroup_Vals_" + language + ") SGV ,";

                Query += " ";
                Query += "(SELECT Area_NId, Area_ID FROM UT_Area_" + language + ") Area ,";

                Query += " ";
                Query += "(SELECT IC_NId, IC_Name, Nature FROM UT_Indicator_Classifications_" + language + ") AS Source ";

                Query += " WHERE Data.Indicator_NId = [Indicator].Indicator_NId AND Data.Subgroup_Val_NId = SGV.Subgroup_Val_NId AND  Data.Area_NId = Area.Area_NId AND Data.Unit_NId = Unit.Unit_NId AND Data.Source_NId = Source.IC_NId";

                RetVal = this._diConnection.ExecuteDataTable(Regex.Replace(Query, "UT_", this._diConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));
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

        internal DataTable Get_Language_Specific_Data_Table(string language)
        {
            DataTable RetVal;
            string Query;

            RetVal = null;
            Query = string.Empty;

            try
            {
                Query = "SELECT Data.Data_Value, Data.Data_Denominator, Data.ConfidenceIntervalUpper, Data.ConfidenceIntervalLower, [Indicator].Indicator_GId, Unit.Unit_GId, SGV.Subgroup_Val_GId,TP.TimePeriod, TP.Periodicity, Area.Area_ID, Source.IC_Name, Source.Nature, Footnote.FootNote FROM";

                Query += " ";
                Query += "(SELECT Data_Value, Data_Denominator, ConfidenceIntervalUpper, ConfidenceIntervalLower, Indicator_NId, Unit_NId, Subgroup_Val_NId, TimePeriod_NId, Area_NId, Source_NId, FootNote_NId FROM UT_Data WHERE 1 = 1";

                if (!string.IsNullOrEmpty(this._iusNIds))
                {
                    Query += " ";
                    Query += "AND IUSNId IN";
                    Query += " ";
                    Query += "(" + this._iusNIds + ")";
                }

                if (!string.IsNullOrEmpty(this._areaNIds))
                {
                    Query += " ";
                    Query += "AND Area_NID IN";
                    Query += " ";
                    Query += "(" + this._areaNIds + ")";
                }

                if (!string.IsNullOrEmpty(this._sourceNIds))
                {
                    Query += " ";
                    Query += "AND Source_NID IN";
                    Query += " ";
                    Query += "(" + this._sourceNIds + ")";
                }

                if (this.IsMRD == false)
                {
                    if (!string.IsNullOrEmpty(this._timeperiodNIds))
                    {
                        Query += " ";
                        Query += "AND TimePeriod_NId IN";
                        Query += " ";
                        Query += "(" + this._timeperiodNIds + ")";
                    }
                }
                else
                {
                    Query += " ";
                    Query += "AND isMRD = 1";
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
                Query += "(SELECT TimePeriod_NId, TimePeriod, Periodicity FROM UT_TimePeriod)AS TP, ";

                Query += " ";
                Query += "(SELECT Area_NId, Area_ID FROM UT_Area_" + language + ") AS Area, ";

                Query += " ";
                Query += "(SELECT IC_NId, IC_Name, Nature FROM UT_Indicator_Classifications_" + language + ") AS Source,";

                Query += " ";
                Query += "(SELECT FootNote_NId, FootNote FROM UT_Footnote_" + language + ") Footnote ";

                Query += " WHERE Data.Unit_NId = Unit.Unit_NId AND  Data.FootNote_NId = Footnote.FootNote_NId ";
                Query += " AND Data.TimePeriod_NId = TP.TimePeriod_NId ";
                Query += " AND Data.Area_NId = Area.Area_NId AND Data.Indicator_NId = [Indicator].Indicator_NId ";
                Query += " AND Data.Source_NId = Source.IC_NId AND Data.Subgroup_Val_NId = SGV.Subgroup_Val_NId";

                RetVal = this._diConnection.ExecuteDataTable(Regex.Replace(Query, "UT_", this._diConnection.DIDataSetDefault(), RegexOptions.IgnoreCase));
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

        internal Dictionary<string, string> Get_Subgroup_Breakup(string SubgroupValGId, DataTable dtSubgroup, DataTable dtSubgroupTypes)
        {
            Dictionary<string, string> DictSubgroupBreakup = new Dictionary<string, string>();

            foreach (DataRow DrSubgroup in dtSubgroup.Select(DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId + "='" + SubgroupValGId + "'"))
            {
                DictSubgroupBreakup.Add(DrSubgroup[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupTypes.SubgroupTypeGID].ToString(), DrSubgroup[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Subgroup.SubgroupGId].ToString());
            }

            foreach (DataRow DrSubgroupTypes in dtSubgroupTypes.Rows)
            {
                if (!DictSubgroupBreakup.ContainsKey(DrSubgroupTypes[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupTypes.SubgroupTypeGID].ToString()))
                {
                    DictSubgroupBreakup.Add(DrSubgroupTypes[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupTypes.SubgroupTypeGID].ToString(), "NA");
                }
            }

            return DictSubgroupBreakup;
        }

        internal void Create_Folder_Structure(string outputFolder)
        {
            this.Create_Directory_If_Not_Exists(outputFolder);

            if (this._languages != null && this._languages.Count > 0)
            {
                foreach(string language in this._languages)
                {
                    this.Create_Directory_If_Not_Exists(Path.Combine(outputFolder, language));
                }
            }
        }

        #endregion "Public"

        #endregion "Methods"
    }
}
