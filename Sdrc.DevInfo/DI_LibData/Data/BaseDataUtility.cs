using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Data;
using SDMXObjectModel.Query;
using System.Text.RegularExpressions;

namespace DevInfo.Lib.DI_LibDATA
{
    internal class BaseDataUtility
    {
        #region "Properties"

        #region "Private"

        private DIConnection _diConnection;

        private DIQueries _diQueries;

        private List<DataTable> _dtDatas;

        private string _langauge;

        private bool _isMRD;

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

        internal List<DataTable> DtDatas
        {
            get
            {
                return this._dtDatas;
            }
            set
            {
                this._dtDatas = value;
            }
        }

        internal string Language
        {
            get
            {
                return this._langauge;
            }
            set
            {
                this._langauge = value;
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

        #endregion "Public"

        #endregion "Properties"

        #region "Constructors"

        #region "Private"

        #endregion "Private"

        #region "Public"

        internal BaseDataUtility(DIConnection DIConnection, DIQueries DIQueries)
        {
            this._diConnection = DIConnection;
            this._diQueries = DIQueries;
        }

        #endregion "Public"

        #endregion "Constructors"

        #region "Methods"

        #region "Private"

        private string Get_SubgroupVal_GId(Dictionary<string, string> DictSubgroupBreakup, DataTable DtSubgroupBreakup)
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
                WhereClause += DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId + Constants.EqualsTo + Constants.Apostophe + IndicatorGId + Constants.Apostophe;

                WhereClause += Constants.AND;

                WhereClause += DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitGId + Constants.EqualsTo + Constants.Apostophe + UnitGId + Constants.Apostophe;

                WhereClause += Constants.AND;

                WhereClause += DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId + Constants.EqualsTo + Constants.Apostophe + SubgroupValGId + Constants.Apostophe;

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

        private List<string> Get_AreaNIds_From_Ids(List<string> AreaIds, string language)
        {
            List<string> RetVal;
            string AreaNId;
            DIQueries DIQueriesLanguage;
            DataTable DtArea;

            RetVal = new List<string>();
            AreaNId = string.Empty;
            DIQueriesLanguage = null;
            DtArea = null;

            try
            {
                DIQueriesLanguage = new DIQueries(this._diQueries.DataPrefix, language);
                DtArea = this._diConnection.ExecuteDataTable(DIQueriesLanguage.Area.GetArea(FilterFieldType.None, string.Empty));
                RetVal.Add(Constants.MinusOne);

                foreach (DataRow DrArea in DtArea.Rows)
                {
                    if (AreaIds != null && AreaIds.Count > 0)
                    {
                        if (AreaIds.Contains(DrArea[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaID].ToString().Trim()))
                        {
                            RetVal.Add(DrArea[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaNId].ToString().Trim());
                        }
                    }
                    else
                    {
                        RetVal.Add(DrArea[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaNId].ToString().Trim());
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

        private List<string> Get_SourceNIds_From_Texts(List<string> SourceTexts, string Language)
        {
            List<string> RetVal;
            string SourceNId;
            DIQueries DIQueriesLanguage;
            DataTable DtSource;

            RetVal = new List<string>();
            SourceNId = string.Empty;
            DIQueriesLanguage = null;
            DtSource = null;

            try
            {
                DIQueriesLanguage = new DIQueries(this._diQueries.DataPrefix, Language);
                DtSource = this._diConnection.ExecuteDataTable(DIQueriesLanguage.IndicatorClassification.GetIC(FilterFieldType.None, string.Empty, ICType.Source, FieldSelection.Light));
                RetVal.Add(Constants.MinusOne);

                foreach (DataRow DrSource in DtSource.Rows)
                {
                    if (SourceTexts != null && SourceTexts.Count > 0)
                    {
                        if (SourceTexts.Contains(DrSource[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICName].ToString().Trim()))
                        {
                            RetVal.Add(DrSource[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICNId].ToString().Trim());
                        }
                    }
                    else
                    {
                        RetVal.Add(DrSource[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICNId].ToString().Trim());
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

        private List<string> Get_TimePeriodNIds_From_Text(List<string> TimePeriods)
        {
            List<string> RetVal;
            string TimePeriodNId;
            DataTable DtTimePeriod;

            RetVal = new List<string>();
            TimePeriodNId = string.Empty;
            DtTimePeriod = null;

            try
            {
                DtTimePeriod = this._diConnection.ExecuteDataTable(this._diQueries.Timeperiod.GetTimePeriod(FilterFieldType.None, string.Empty));
                RetVal.Add(Constants.MinusOne);

                foreach (DataRow DrTimePeriod in DtTimePeriod.Rows)
                {
                    if (TimePeriods != null && TimePeriods.Count > 0)
                    {
                        if (TimePeriods.Contains(DrTimePeriod[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriod].ToString().Trim()))
                        {
                            RetVal.Add(DrTimePeriod[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriodNId].ToString().Trim());
                        }
                    }
                    else
                    {
                        RetVal.Add(DrTimePeriod[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriodNId].ToString().Trim());
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

        private DataTable Get_Language_Specific_Datatable(string language, string IUSNIds, string AreaNIds, string TimePeriodNIds, string SourceNIds)
        {
            DataTable RetVal;
            string Query;

            RetVal = null;
            Query = string.Empty;

            try
            {
                Query = "SELECT Data.Data_Value, Data.Data_Denominator, Indicator.Indicator_GId, Indicator.Indicator_Name, Unit.Unit_GId, Unit.Unit_name, SGV.Subgroup_Val_GId, SGV.Subgroup_Val, TP.TimePeriod, Area.Area_ID, Area.Area_Name, Source.IC_Name, Footnote.FootNote FROM";

                Query += " ";
                Query += "(SELECT Data_Value, Data_Denominator, Indicator_NId, Unit_NId, Subgroup_Val_NId, TimePeriod_NId, Area_NId, Source_NId, FootNote_NId FROM UT_Data WHERE 1 = 1";

                if (!string.IsNullOrEmpty(IUSNIds))
                {
                    Query += " ";
                    Query += "AND IUSNId IN";
                    Query += " ";
                    Query += "(" + IUSNIds + ")";
                }

                if (!string.IsNullOrEmpty(AreaNIds))
                {
                    Query += " ";
                    Query += "AND Area_NID IN";
                    Query += " ";
                    Query += "(" + AreaNIds + ")";
                }

                if (!string.IsNullOrEmpty(SourceNIds))
                {
                    Query += " ";
                    Query += "AND Source_NID IN";
                    Query += " ";
                    Query += "(" + SourceNIds + ")";
                }

                if (this.IsMRD == false)
                {
                    if (!string.IsNullOrEmpty(TimePeriodNIds))
                    {
                        Query += " ";
                        Query += "AND TimePeriod_NId IN";
                        Query += " ";
                        Query += "(" + TimePeriodNIds + ")";
                    }
                }
                else
                {
                    Query += " ";
                    Query += "AND isMRD = 1";
                }

                Query += ")";
                Query += " ";
                Query += "Data JOIN";

                Query += " ";
                Query += "(SELECT Indicator_NId, Indicator_GId, Indicator_Name FROM UT_Indicator_" + language + ") Indicator ON Data.Indicator_NId = Indicator.Indicator_NId JOIN";

                Query += " ";
                Query += "(SELECT Unit_NId, Unit_GId, Unit_Name FROM UT_Unit_" + language + ") Unit ON Data.Unit_NId = Unit.Unit_NId JOIN";

                Query += " ";
                Query += "(SELECT Subgroup_Val_NId, Subgroup_Val_GId, Subgroup_Val FROM UT_Subgroup_Vals_" + language + ") SGV ON Data.Subgroup_Val_NId = SGV.Subgroup_Val_NId JOIN";

                Query += " ";
                Query += "(SELECT TimePeriod_NId, TimePeriod FROM UT_TimePeriod) TP ON Data.TimePeriod_NId = TP.TimePeriod_NId JOIN";

                Query += " ";
                Query += "(SELECT Area_NId, Area_ID, Area_Name FROM UT_Area_" + language + ") Area ON Data.Area_NId = Area.Area_NId JOIN";

                Query += " ";
                Query += "(SELECT IC_NId, IC_Name FROM UT_Indicator_Classifications_" + language + ") Source ON Data.Source_NId = Source.IC_NId JOIN";

                Query += " ";
                Query += "(SELECT FootNote_NId, FootNote FROM UT_Footnote_" + language + ") Footnote ON Data.FootNote_NId = Footnote.FootNote_NId";

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

        #endregion "Private"

        #region "Public"

        internal virtual string Get_Data(XmlDocument query)
        {
            return string.Empty;
        }

        internal void Retrieve_DataTable_From_Query(DataParametersAndType DataWhere)
        {
            string IndicatorGId, UnitGId, SubgroupValGId, AreaId, Source, TimePeriodText, Language, IUSNId;
            List<string> IUSNIds, AreaIds, AreaNIds, SourceTexts, SourceNIds, TimePeriods, TimePeriodNIds;
            Dictionary<string, string> DictSubgroupBreakup;
            DataTable DtSubgroupBreakup, DtIUS, DtLanguageSpecificTable;
            DataColumn DcLanguage;

            IndicatorGId = string.Empty;
            UnitGId = string.Empty;
            SubgroupValGId = string.Empty;
            AreaId = string.Empty;
            Source = string.Empty;
            TimePeriodText = string.Empty;
            Language = string.Empty;
            IUSNId = string.Empty;

            IUSNIds = new List<string>();
            AreaIds = new List<string>();
            AreaNIds = new List<string>();
            SourceTexts = new List<string>();
            SourceNIds = new List<string>();
            TimePeriods = new List<string>();
            TimePeriodNIds = new List<string>();
            DictSubgroupBreakup = new Dictionary<string, string>();

            DtSubgroupBreakup = this._diConnection.ExecuteDataTable(this._diQueries.SubgroupValSubgroup.GetSubgroupValsWithSubgroups());
            DtSubgroupBreakup = DtSubgroupBreakup.DefaultView.ToTable(true, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupTypes.SubgroupTypeGID, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Subgroup.SubgroupGId);

            DtIUS = this._diConnection.ExecuteDataTable(this._diQueries.IUS.GetIUS(FilterFieldType.None, string.Empty, FieldSelection.Light));
            DtIUS = DtIUS.DefaultView.ToTable(true, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator_Unit_Subgroup.IUSNId, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitGId, DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId);
            DtLanguageSpecificTable = null;
            DcLanguage = null;

            foreach (DataParametersOrType ORItem in DataWhere.Or)
            {
                if (ORItem.DimensionValue.Count > 0)
                {
                    foreach (DimensionValueType Dimension in ORItem.DimensionValue)
                    {
                        switch (Dimension.ID)
                        {
                            case Constants.Xml.AREA:
                                AreaId = ((SimpleValueType)Dimension.Items[0]).Value;
                                AreaIds.Add(AreaId);
                                break;
                            case Constants.Xml.SOURCE:
                                Source = ((SimpleValueType)Dimension.Items[0]).Value;
                                SourceTexts.Add(Source);
                                break;
                            case Constants.Xml.TIME_PERIOD:
                                TimePeriodText = ((SimpleValueType)Dimension.Items[0]).Value;
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
                        case Constants.Xml.FOOTNOTES:
                            this._langauge = ((QueryTextType)ORItem.AttributeValue[0].Items[0]).lang;
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
                                case Constants.Xml.INDICATOR:
                                    IndicatorGId = ((SimpleValueType)Dimension.Items[0]).Value;
                                    break;
                                case Constants.Xml.UNIT:
                                    UnitGId = ((SimpleValueType)Dimension.Items[0]).Value;
                                    break;
                                default:
                                    DictSubgroupBreakup.Add(Dimension.ID, ((SimpleValueType)Dimension.Items[0]).Value);
                                    break;
                            }
                        }

                        SubgroupValGId = this.Get_SubgroupVal_GId(DictSubgroupBreakup, DtSubgroupBreakup);
                        IUSNId = this.Get_IUSNId_From_GIds(IndicatorGId, UnitGId, SubgroupValGId, DtIUS);
                        IUSNIds.Add(IUSNId);
                    }
                }
            }

            if (this.IsMRD == false)
            {
                TimePeriodNIds = this.Get_TimePeriodNIds_From_Text(TimePeriods);
            }

            this._dtDatas = new List<DataTable>();
            if (string.IsNullOrEmpty(this._langauge))
            {
                foreach (DataRow LanguageRow in this._diConnection.DILanguages(this._diQueries.DataPrefix).Rows)
                {
                    Language = LanguageRow[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Language.LanguageCode].ToString();
                    AreaNIds = this.Get_AreaNIds_From_Ids(AreaIds, Language);
                    SourceNIds = this.Get_SourceNIds_From_Texts(SourceTexts, Language);
                    DtLanguageSpecificTable = this.Get_Language_Specific_Datatable(Language, String.Join(Constants.Comma, IUSNIds.ToArray()), String.Join(Constants.Comma, AreaNIds.ToArray()), String.Join(Constants.Comma, TimePeriodNIds.ToArray()), String.Join(Constants.Comma, SourceNIds.ToArray()));

                    DcLanguage = new DataColumn("Language", typeof(System.String));
                    DcLanguage.DefaultValue = Language;
                    DtLanguageSpecificTable.Columns.Add(DcLanguage);

                    this._dtDatas.Add(DtLanguageSpecificTable);
                }
            }
            else
            {
                if (this._diConnection.IsValidDILanguage(this._diQueries.DataPrefix, this._langauge))
                {
                    Language = this._langauge;
                }
                else
                {
                    Language = this._diQueries.LanguageCode.Substring(1);
                }

                AreaNIds = this.Get_AreaNIds_From_Ids(AreaIds, Language);
                SourceNIds = this.Get_SourceNIds_From_Texts(SourceTexts, Language);
                DtLanguageSpecificTable = this.Get_Language_Specific_Datatable(Language, String.Join(Constants.Comma, IUSNIds.ToArray()), String.Join(Constants.Comma, AreaNIds.ToArray()), String.Join(Constants.Comma, TimePeriodNIds.ToArray()), String.Join(Constants.Comma, SourceNIds.ToArray()));

                DcLanguage = new DataColumn("Language", typeof(System.String));
                DcLanguage.DefaultValue = Language;
                DtLanguageSpecificTable.Columns.Add(DcLanguage);

                this._dtDatas.Add(DtLanguageSpecificTable);
            }
        }

        #endregion "Public"

        #endregion "Methods"
    }
}
