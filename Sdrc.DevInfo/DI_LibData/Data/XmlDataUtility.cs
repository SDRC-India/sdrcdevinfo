using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using DevInfo.Lib.DI_LibDAL;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.IO;
using SDMXObjectModel;

namespace DevInfo.Lib.DI_LibDATA
{
    internal class XmlDataUtility : BaseDataUtility
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

        internal XmlDataUtility(DIConnection DIConnection, DIQueries DIQueries)
            : base(DIConnection, DIQueries)
        {
        }

        #endregion "Public"

        #endregion "Constructors"

        #region "Methods"

        #region "Private"

        #endregion "Private"

        #region "Public"

        internal override string Get_Data(XmlDocument query)
        {
            string RetVal;
            SDMXObjectModel.Message.StructureSpecificTimeSeriesDataQueryType StructureSpecificTimeSeriesDataQuery;
            XmlDocument Document;
            XmlDeclaration Declaration;
            XmlElement Root, Data, Observation, Element;

            RetVal = string.Empty;
            Document = new XmlDocument();

            try
            {
                StructureSpecificTimeSeriesDataQuery = (SDMXObjectModel.Message.StructureSpecificTimeSeriesDataQueryType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureSpecificTimeSeriesDataQueryType), query);

                this.Retrieve_DataTable_From_Query(StructureSpecificTimeSeriesDataQuery.Query.DataWhere);

                Document = new XmlDocument();
                Declaration = Document.CreateXmlDeclaration(Constants.Xml.version, Constants.Xml.encoding, null);
                Document.AppendChild(Declaration);

                Root = Document.CreateElement(Constants.Xml.Root);
                Document.AppendChild(Root);

                foreach (DataTable DtData in this.DtDatas)
                {
                    if (DtData.Rows.Count > 0)
                    {
                        Data = Document.CreateElement(Constants.Xml.Data);
                        Data.SetAttribute(Constants.Xml.Xml_Lang, DtData.Rows[0]["Language"].ToString());
                        Root.AppendChild(Data);

                        foreach (DataRow DrData in DtData.Rows)
                        {
                            Observation = Document.CreateElement(Constants.Xml.Observation);
                            Data.AppendChild(Observation);

                            Element = Document.CreateElement(Constants.Xml.INDICATOR);
                            Element.SetAttribute(Constants.Xml.id, DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId].ToString());
                            Element.AppendChild(Document.CreateTextNode(DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorName].ToString()));
                            Observation.AppendChild(Element);

                            Element = Document.CreateElement(Constants.Xml.UNIT);
                            Element.SetAttribute(Constants.Xml.id, DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitGId].ToString());
                            Element.AppendChild(Document.CreateTextNode(DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitName].ToString()));
                            Observation.AppendChild(Element);

                            Element = Document.CreateElement(Constants.Xml.SUBGROUP);
                            Element.SetAttribute(Constants.Xml.id, DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId].ToString());
                            Element.AppendChild(Document.CreateTextNode(DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupVal].ToString()));
                            Observation.AppendChild(Element);

                            Element = Document.CreateElement(Constants.Xml.AREA);
                            Element.SetAttribute(Constants.Xml.id, DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaID].ToString());
                            Element.AppendChild(Document.CreateTextNode(DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaName].ToString()));
                            Observation.AppendChild(Element);

                            Element = Document.CreateElement(Constants.Xml.SOURCE);
                            Element.AppendChild(Document.CreateTextNode(DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICName].ToString()));
                            Observation.AppendChild(Element);

                            Element = Document.CreateElement(Constants.Xml.DENOMINATOR);
                            Element.AppendChild(Document.CreateTextNode(DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.DataDenominator].ToString()));
                            Observation.AppendChild(Element);

                            Element = Document.CreateElement(Constants.Xml.FOOTNOTES);
                            Element.AppendChild(Document.CreateTextNode(DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.FootNotes.FootNote].ToString()));
                            Observation.AppendChild(Element);

                            Element = Document.CreateElement(Constants.Xml.TIME_PERIOD);
                            Element.AppendChild(Document.CreateTextNode(DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriod].ToString()));
                            Observation.AppendChild(Element);

                            Element = Document.CreateElement(Constants.Xml.OBS_VALUE);
                            Element.AppendChild(Document.CreateTextNode(DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.DataValue].ToString()));
                            Observation.AppendChild(Element);
                        }
                    }
                }

                RetVal = Document.InnerXml;
            }
            catch (Exception ex)
            {
                Document = null;
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
