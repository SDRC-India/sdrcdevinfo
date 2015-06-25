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
using SDMXObjectModel.Query;


namespace DevInfo.Lib.DI_LibSDMX
{
    internal class GenericTimeSeriesQueryUtility : BaseQueryUtility
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

        internal GenericTimeSeriesQueryUtility(Dictionary<string, string> dictUserSelections, DataReturnDetailTypes dataReturnDetailType, string agencyId, DIConnection DIConnection, DIQueries DIQueries)
            : base(dictUserSelections, dataReturnDetailType, agencyId, DIConnection, DIQueries)
        {
        }

        #endregion "Public"

        #endregion "Constructors"

        #region "Methods"

        #region "Private"

        #endregion "Private"

        #region "Public"

        internal override XmlDocument Get_Query()
        {
            XmlDocument RetVal;
            SDMXObjectModel.Message.GenericTimeSeriesDataQueryType GenericTimeSeriesDataQuery;
            DataParametersOrType ORItem;
            DataParametersAndType ANDItem;
            DimensionValueType DimensionValue;
            DataTable DtSubgroupBreakup;
            Dictionary<string, string> DictSubgroupBreakup;
            string[] SplittedUserSelectionKeyValues;

            RetVal = null;
            ORItem = null;
            ANDItem = null;
            GenericTimeSeriesDataQuery = new SDMXObjectModel.Message.GenericTimeSeriesDataQueryType();
            GenericTimeSeriesDataQuery.Header = Get_Appropriate_Header();
            GenericTimeSeriesDataQuery.Query = new SDMXObjectModel.Query.GenericTimeSeriesDataQueryType();
            GenericTimeSeriesDataQuery.Query.ReturnDetails = new DataReturnDetailsType();
            GenericTimeSeriesDataQuery.Query.ReturnDetails.detail = Enum.GetName(typeof(DataReturnDetailTypes), this.DataReturnDetailType).ToString();
            GenericTimeSeriesDataQuery.Query.DataWhere = new DataParametersAndType();
            GenericTimeSeriesDataQuery.Query.DataWhere.DataStructure = this.Get_DataStructure_Reference();
            GenericTimeSeriesDataQuery.Query.DataWhere.Or = new List<DataParametersOrType>();

            if (this.DictUserSelections != null)
            {
                DtSubgroupBreakup = this.DIConnection.ExecuteDataTable(this.DIQueries.SubgroupValSubgroup.GetSubgroupValsWithSubgroups());
                DictSubgroupBreakup = new Dictionary<string, string>();

                foreach (string Key in this.DictUserSelections.Keys)
                {
                    switch (Key)
                    {
                        case Constants.Concept.INDICATOR.Id:
                            if (this.DictUserSelections[Key] != null && !string.IsNullOrEmpty(this.DictUserSelections[Key].ToString()))
                            {
                                ORItem = new DataParametersOrType();
                                ORItem.And = new List<DataParametersAndType>();
                                SplittedUserSelectionKeyValues = this.DictUserSelections[Key].Split(new string[] { Constants.Comma }, StringSplitOptions.None);
                                for (int i = 0; i < SplittedUserSelectionKeyValues.Length; i++)
                                {
                                    ANDItem = new DataParametersAndType();
                                    ANDItem.DimensionValue = new List<DimensionValueType>();

                                    DimensionValue = new DimensionValueType();
                                    DimensionValue.ID = Constants.Concept.INDICATOR.Id;
                                    DimensionValue.Items = new List<object>();
                                    DimensionValue.Items.Add(new SDMXObjectModel.Query.SimpleValueType());
                                    ((SDMXObjectModel.Query.SimpleValueType)DimensionValue.Items[0]).Value = SplittedUserSelectionKeyValues[i].Split(new string[] { Constants.AtTheRate }, StringSplitOptions.None)[0];
                                    ANDItem.DimensionValue.Add(DimensionValue);

                                    DimensionValue = new DimensionValueType();
                                    DimensionValue.ID = Constants.Concept.UNIT.Id;
                                    DimensionValue.Items = new List<object>();
                                    DimensionValue.Items.Add(new SDMXObjectModel.Query.SimpleValueType());
                                    ((SDMXObjectModel.Query.SimpleValueType)DimensionValue.Items[0]).Value = SplittedUserSelectionKeyValues[i].Split(new string[] { Constants.AtTheRate }, StringSplitOptions.None)[1];
                                    ANDItem.DimensionValue.Add(DimensionValue);

                                    DictSubgroupBreakup = this.Get_Subgroup_Breakup(SplittedUserSelectionKeyValues[i].Split(new string[] { Constants.AtTheRate }, StringSplitOptions.None)[2], DtSubgroupBreakup);

                                    foreach (string SubgroupTypeGId in DictSubgroupBreakup.Keys)
                                    {
                                        DimensionValue = new DimensionValueType();
                                        DimensionValue.ID = SubgroupTypeGId;
                                        DimensionValue.Items = new List<object>();
                                        DimensionValue.Items.Add(new SDMXObjectModel.Query.SimpleValueType());
                                        ((SDMXObjectModel.Query.SimpleValueType)DimensionValue.Items[0]).Value = DictSubgroupBreakup[SubgroupTypeGId].ToString();
                                        ANDItem.DimensionValue.Add(DimensionValue);
                                    }

                                    ORItem.And.Add(ANDItem);
                                }
                            }
                            break;
                        case Constants.Concept.SOURCE.Id:
                        case Constants.Concept.AREA.Id:
                        case Constants.Concept.TIME_PERIOD.Id:
                            if (this.DictUserSelections[Key] != null && !string.IsNullOrEmpty(this.DictUserSelections[Key].ToString()))
                            {
                                ORItem = new DataParametersOrType();
                                ORItem.DimensionValue = new List<DimensionValueType>();
                                SplittedUserSelectionKeyValues = this.DictUserSelections[Key].Split(new string[] { Constants.Comma }, StringSplitOptions.None);
                                for (int i = 0; i < SplittedUserSelectionKeyValues.Length; i++)
                                {
                                    DimensionValue = new DimensionValueType();
                                    DimensionValue.ID = Key;
                                    DimensionValue.Items = new List<object>();
                                    DimensionValue.Items.Add(new SDMXObjectModel.Query.SimpleValueType());
                                    ((SDMXObjectModel.Query.SimpleValueType)DimensionValue.Items[0]).Value = SplittedUserSelectionKeyValues[i];
                                    ORItem.DimensionValue.Add(DimensionValue);
                                }
                            }
                            break;
                        case Constants.Concept.FOOTNOTES.Id:
                            ORItem = new DataParametersOrType();
                            ORItem.AttributeValue = new List<AttributeValueType>();
                            ORItem.AttributeValue.Add(new AttributeValueType());
                            ORItem.AttributeValue[0].ID = Key;
                            ORItem.AttributeValue[0].Items = new List<object>();
                            ORItem.AttributeValue[0].Items.Add(new SDMXObjectModel.Query.QueryTextType());
                            ((QueryTextType)ORItem.AttributeValue[0].Items[0]).lang = this.DictUserSelections[Key];
                            break;
                        default:
                            break;
                    }

                    GenericTimeSeriesDataQuery.Query.DataWhere.Or.Add(ORItem);
                }
            }
            RetVal = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.GenericTimeSeriesDataQueryType), GenericTimeSeriesDataQuery);

            return RetVal;
        }

        #endregion "Public"

        #endregion "Methods"
    }
}
