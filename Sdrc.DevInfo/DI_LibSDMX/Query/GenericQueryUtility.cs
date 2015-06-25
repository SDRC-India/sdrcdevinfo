using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using SDMXObjectModel.Message;
using SDMXObjectModel.Structure;
using SDMXObjectModel;
using System.Data;
using DevInfo.Lib.DI_LibDAL;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using SDMXObjectModel.Data.Generic;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using SDMXObjectModel.Query;
using SDMXObjectModel.Common;

namespace DevInfo.Lib.DI_LibSDMX
{
    internal class GenericQueryUtility : BaseQueryUtility
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

        internal GenericQueryUtility(Dictionary<string, string> dictUserSelections, DataReturnDetailTypes dataReturnDetailType, string agencyId, DIConnection DIConnection, DIQueries DIQueries)
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
            SDMXObjectModel.Message.GenericDataQueryType GenericDataQuery;
            DataParametersOrType ORItem;
            DataParametersAndType ANDItem;
            DimensionValueType DimensionValue;
            DataTable DtSubgroupBreakup;
            Dictionary<string, string> DictSubgroupBreakup;
            string[] SplittedUserSelectionKeyValues;

            RetVal = null;
            ORItem = null;
            ANDItem = null;
            GenericDataQuery = new SDMXObjectModel.Message.GenericDataQueryType();
            GenericDataQuery.Header = Get_Appropriate_Header();
            GenericDataQuery.Query = new SDMXObjectModel.Query.GenericDataQueryType();
            GenericDataQuery.Query.ReturnDetails = new DataReturnDetailsType();
            GenericDataQuery.Query.ReturnDetails.detail = Enum.GetName(typeof(DataReturnDetailTypes), this.DataReturnDetailType).ToString();
            GenericDataQuery.Query.DataWhere = new DataParametersAndType();
            GenericDataQuery.Query.DataWhere.DataStructure = this.Get_DataStructure_Reference();
            GenericDataQuery.Query.DataWhere.Or = new List<DataParametersOrType>();

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

                    GenericDataQuery.Query.DataWhere.Or.Add(ORItem);
                }
            }
            RetVal = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.GenericDataQueryType), GenericDataQuery);

            return RetVal;
        }

        #endregion "Public"

        #endregion "Methods"
    }
}
