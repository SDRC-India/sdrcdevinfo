using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using DevInfo.Lib.DI_LibDAL;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Web;
using System.IO;
using System.Web.Script.Serialization;
using SDMXObjectModel;
 
namespace DevInfo.Lib.DI_LibDATA
{
    internal class JsonDataUtility : BaseDataUtility
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

        internal JsonDataUtility(DIConnection DIConnection, DIQueries DIQueries)
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
            Root Root;
            Data Data;
            Observation Observation;
            JavaScriptSerializer Serializer;

            RetVal = string.Empty;
            Serializer = new JavaScriptSerializer();
            Serializer.MaxJsonLength = 2147483647;

            try
            {
                StructureSpecificTimeSeriesDataQuery = (SDMXObjectModel.Message.StructureSpecificTimeSeriesDataQueryType)Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.StructureSpecificTimeSeriesDataQueryType), query);

                this.Retrieve_DataTable_From_Query(StructureSpecificTimeSeriesDataQuery.Query.DataWhere);
                 
                Root = new Root();
                Root.Data = new List<Data>();

                foreach (DataTable DtData in this.DtDatas)
                {
                    if (DtData.Rows.Count > 0)
                    {
                        Data = new Data();
                        Root.Data.Add(Data);

                        Data.lang = DtData.Rows[0]["Language"].ToString();
                        Data.Observation = new List<Observation>();

                        foreach (DataRow DrData in DtData.Rows)
                        {
                            Observation = new Observation();
                            Data.Observation.Add(Observation);

                            Observation.INDICATOR = new INDICATOR(DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGId].ToString(), DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorName].ToString());
                            Observation.UNIT = new UNIT(DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitGId].ToString(), DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitName].ToString());
                            Observation.SUBGROUP = new SUBGROUP(DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId].ToString(), DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupVal].ToString());
                            Observation.AREA = new AREA(DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaID].ToString(), DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Area.AreaName].ToString());
                            Observation.SOURCE = DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.IndicatorClassifications.ICName].ToString();
                            Observation.DENOMINATOR = DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.DataDenominator].ToString();
                            Observation.FOOTNOTES = DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.FootNotes.FootNote].ToString();
                            Observation.TIME_PERIOD = DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Timeperiods.TimePeriod].ToString();
                            Observation.OBS_VALUE = DrData[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.DataValue].ToString();
                        }
                    }
                }

                RetVal = Serializer.Serialize(Root);
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

        #endregion "Public"

        #endregion "Methods"
    }

    public class Root
    {
        public List<Data> Data;
    }

    public class Data
    {
        public string lang;

        public List<Observation> Observation;
    }

    public class Observation
    {
        public INDICATOR INDICATOR;
        public UNIT UNIT;
        public SUBGROUP SUBGROUP;
        public AREA AREA;
        public string SOURCE;
        public string DENOMINATOR;
        public string FOOTNOTES;
        public string TIME_PERIOD;
        public string OBS_VALUE;
    }

    public class INDICATOR
    {
        public string id;
        public string value;

        public INDICATOR(string _id, string _value)
        {
            this.id = _id;
            this.value = _value;
        }
    }

    public class UNIT
    {
        public string id;
        public string value;

        public UNIT(string _id, string _value)
        {
            this.id = _id;
            this.value = _value;
        }
    }

    public class SUBGROUP
    {
        public string id;
        public string value;

        public SUBGROUP(string _id, string _value)
        {
            this.id = _id;
            this.value = _value;
        }
    }

    public class AREA
    {
        public string id;
        public string value;

        public AREA(string _id, string _value)
        {
            this.id = _id;
            this.value = _value;
        }
    }
}
