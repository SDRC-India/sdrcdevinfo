// ***********************Copy Right Notice*****************************
// 
// **********************************************************************
// Program Name:									       
// Developed By: DG6
// Creation date: 2007-05-31							
// Program Comments: Stores mapping information 
// **********************************************************************
// **********************Change history*********************************
// No.	Mod: Date	        Mod: By	       Change Description		        
// c1   2007.11.30          DG6             Added new types : footnote and denominator
//											          
// **********************************************************************
using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.SerializeClasses;
using DevInfo.Lib.DI_LibBAL.LogFiles;

namespace DevInfo.Lib.DI_LibBAL.Controls.MappingControlsBAL
{

    /// <summary>
    /// Implements the Mapping information
    /// </summary>
    public class Mapping
    {

        #region "-- private --"

        #region "-- New/Dispose --"


        #endregion

        #region "-- Methods --"


        #endregion

        #endregion

        #region "-- Public / Friend --"

        #region "-- Variables and Properties --"
        private string _IndicatorName = string.Empty;
        /// <summary>
        /// Gets or Sets Indicator name
        /// </summary>
        public string IndicatorName
        {
            get
            {
                return this._IndicatorName;
            }
            set
            {
                this._IndicatorName = value;
            }
        }

        private string _IndicatorGID = string.Empty;
        /// <summary>
        /// Gets or Sets Indicator GID
        /// </summary>
        public string IndicatorGID
        {
            get
            {

                return this._IndicatorGID;
            }
            set
            {
                this._IndicatorGID = value;
            }
        }

        private string _UnitGID = string.Empty;
        /// <summary>
        /// Gets or Sets Unit GID
        /// </summary>
        public string UnitGID
        {
            get
            {
                return this._UnitGID;
            }
            set
            {
                this._UnitGID = value;
            }
        }

        private string _UnitName = string.Empty;
        /// <summary>
        /// Gets or Sets Unit name
        /// </summary>
        public string UnitName
        {
            get
            {
                return this._UnitName;
            }
            set
            {
                this._UnitName = value;
            }
        }


        private string _SubgroupVal = string.Empty;
        /// <summary>
        /// Gets or Sets Subgroupval
        /// </summary>
        public string SubgroupVal
        {
            get
            {
                return this._SubgroupVal;
            }
            set
            {
                _SubgroupVal = value;
            }
        }


        private string _SubgroupValGID = string.Empty;
        /// <summary>
        /// Gets or Sets Subgroupval GID
        /// </summary>
        public string SubgroupValGID
        {
            get
            {
                return this._SubgroupValGID;
            }
            set
            {
                _SubgroupValGID = value;
            }
        }


        private string _Timeperiod = string.Empty;
        /// <summary>
        /// Gets or Sets Timeperiod. Depends on TimeperiodType property
        /// </summary>
        public string Timeperiod
        {
            get
            {
                return this._Timeperiod;
            }
            set
            {
                this._Timeperiod = value;
            }
        }

        private DIMappingType _TimeperiodType = DIMappingType.Value;
        /// <summary>
        /// Gets or Sets Timeperiod value type ( Reference or Value)
        /// </summary>
        public DIMappingType TimeperiodType
        {
            get
            {
                return this._TimeperiodType;
            }
            set
            {
                this._TimeperiodType = value;
            }
        }


        private string _Area = string.Empty;
        /// <summary>
        /// Gets or Sets Area name
        /// </summary>
        public string Area
        {
            get
            {
                return this._Area;
            }
            set
            {
                this._Area = value;
            }
        }

        private string _AreaID = string.Empty;
        /// <summary>
        /// Gets or Sets Area ID
        /// </summary>
        public string AreaID
        {
            get
            {
                return this._AreaID;
            }
            set
            {
                this._AreaID = value;
            }
        }

        private DIMappingType _AreaType = DIMappingType.Value;
        /// <summary>
        /// Gets or Sets Area value type ( Reference or Value)
        /// </summary>
        public DIMappingType AreaType
        {
            get
            {
                return this._AreaType;
            }
            set
            {
                this._AreaType = value;
            }
        }


        private string _Source = string.Empty;
        /// <summary>
        /// Gets or Sets Timeperiod. Depends on SourceType property
        /// </summary>
        public string Source
        {
            get { return _Source; }
            set { _Source = value; }
        }

        private DIMappingType _SourceType = DIMappingType.Value;
        /// <summary>
        /// Gets or Sets Source value type ( Reference or Value)
        /// </summary>
        public DIMappingType SourceType
        {
            get
            {
                return this._SourceType;
            }
            set
            {
                this._SourceType = value;
            }
        }

        //-- ***Start of Change no: c1

        private string _Footnote = string.Empty;
        /// <summary>
        /// Gets or Sets Footnote. Depends on Footnote type
        /// </summary>
        public string Footnote
        {
            get { return this._Footnote; }
            set { this._Footnote = value; }
        }

        private DIMappingType _FootnoteType = DIMappingType.Value;
        /// <summary>
        /// Gets or sets footnote type 
        /// </summary>
        public DIMappingType FootnoteType
        {
            get { return this._FootnoteType; }
            set { this._FootnoteType = value; }
        }

        private string _Denominator = string.Empty;
        /// <summary>
        /// Gets or Sets Denominator. Depends on denominator type
        /// </summary>
        public string Denominator
        {
            get { return _Denominator; }
            set { _Denominator = value; }
        }

        private int _DefaultDecimalValue = 0;
        /// <summary>
        /// Gets or Sets Default Decimal Place.
        /// </summary>
        public int DefaultDecimalValue
        {
            get { return this._DefaultDecimalValue; }
            set { this._DefaultDecimalValue = value; }
        }

        private DIMappingType _DenominatorType = DIMappingType.Value;
        /// <summary>
        /// Gets or Sets denominator type.
        /// </summary>
        public DIMappingType DenominatorType
        {
            get { return this._DenominatorType; }
            set { this._DenominatorType = value; }
        }


        private SerializableDictionary<string, DI6SubgroupInfo> _Subgroups = new SerializableDictionary<string, DI6SubgroupInfo>();
        /// <summary>
        /// Gets or sets subgroup(Dimension values).Key is GID and value is DI6SubgroupInfo
        /// </summary>
        public SerializableDictionary<string, DI6SubgroupInfo> Subgroups
        {
            get { return this._Subgroups; }
            set { this._Subgroups = value; }
        }


        //-- ***End of Change no: c1

        #endregion

        #region "-- New/Dispose --"

        public Mapping()
        {
            // do nothing
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="indicator"></param>
        /// <param name="indicatorGID"></param>
        /// <param name="unit"></param>
        /// <param name="unitGID"></param>
        /// <param name="SubgroupVal"></param>
        /// <param name="SubgroupValGID"></param>
        /// <param name="areaName"></param>
        /// <param name="areaId"></param>
        /// <param name="timeperiod"></param>
        /// <param name="source"></param>
        /// <param name="footnote"></param>
        /// <param name="denominator"></param>
        public Mapping(string indicator, string indicatorGID, string unit, string unitGID, string SubgroupVal, string SubgroupValGID,
            string areaName, string areaId, string timeperiod, string source,
            string footnote, string denominator)
        {
            this._IndicatorName = indicator;
            this._IndicatorGID = IndicatorGID;
            this._UnitName = unit;
            this._UnitGID = UnitGID;
            this._SubgroupVal = SubgroupVal;
            this._SubgroupValGID = SubgroupValGID;
            this._Timeperiod = timeperiod;
            this._AreaType = DIMappingType.Value;
            this._Area = areaName;
            this._AreaID = areaId;
            this._Source = source;

            //set default type for source and timeperiod to Value
            this._SourceType = DIMappingType.Value;
            this._TimeperiodType = DIMappingType.Value;

            //-- ***Start of Change no: c1
            this._Footnote = footnote;
            this._Denominator = denominator;

            //set default type for source and timeperiod to Value
            this._FootnoteType = DIMappingType.Value;
            this._DenominatorType = DIMappingType.Value;

            //-- ***End of Change no: c1


        }

        #endregion


        #region "-- methods --"

        /// <summary>
        /// Returns the exact copy of the current mapping object
        /// </summary>
        /// <returns></returns>
        public Mapping Copy()
        {
            Mapping RetVal = new Mapping();

            RetVal.Area = this._Area;
            RetVal.AreaID = this.AreaID;
            RetVal.AreaType = this.AreaType;
            RetVal.Denominator = this.Denominator;
            RetVal.DenominatorType = this.DenominatorType;
            RetVal.Footnote = this.Footnote;
            RetVal.FootnoteType = this.FootnoteType;
            RetVal.IndicatorGID = this.IndicatorGID;
            RetVal.IndicatorName = this.IndicatorName;
            RetVal.Source = this.Source;
            RetVal.SourceType = this.SourceType;
            RetVal.Subgroups = this.Subgroups;
            RetVal.SubgroupVal = this.SubgroupVal;
            RetVal.SubgroupValGID = this.SubgroupValGID;
            RetVal.Timeperiod = this.Timeperiod;
            RetVal.TimeperiodType = this.TimeperiodType;
            RetVal.UnitGID = this.UnitGID;
            RetVal.UnitName = this.UnitName;
            RetVal.DefaultDecimalValue = this.DefaultDecimalValue;

            return RetVal;
        }

        /// <summary>
        /// Replace the empty values with the values available in sourceMapping 
        /// </summary>
        /// <param name="sourceMapping"></param>
        public void ReplaceEmptyValues(Mapping sourceMapping)
        {
            if (sourceMapping != null)
            {

                if (string.IsNullOrEmpty(this.Area))
                {
                    this.Area = sourceMapping.Area;
                    this.AreaID = sourceMapping.AreaID;
                    this.AreaType = sourceMapping.AreaType;
                }

                if (string.IsNullOrEmpty(this.Denominator))
                {
                    this.Denominator = sourceMapping.Denominator;
                    this.DenominatorType = sourceMapping.DenominatorType;
                }

                if (string.IsNullOrEmpty(this.Footnote))
                {
                    this.Footnote = sourceMapping.Footnote;
                    this.FootnoteType = sourceMapping.FootnoteType;
                }

                if (string.IsNullOrEmpty(this.IndicatorName))
                {
                    this.IndicatorName = sourceMapping.IndicatorName;
                    this.IndicatorGID = sourceMapping.IndicatorGID;
                }

                if (string.IsNullOrEmpty(this.Source))
                {
                    this.Source = sourceMapping.Source;
                    this.SourceType = sourceMapping.SourceType;
                }

                if (this.Subgroups == null || this.Subgroups.Count == 0)
                {
                    this.Subgroups = sourceMapping.Subgroups;
                }

                if (string.IsNullOrEmpty(this.SubgroupVal))
                {
                    if (this.Subgroups == null || this.Subgroups.Count == 0)
                    {
                        this.SubgroupVal = sourceMapping.SubgroupVal;
                        this.SubgroupValGID = sourceMapping.SubgroupValGID;
                    }
                }

                if (string.IsNullOrEmpty(this.Timeperiod))
                {
                    this.Timeperiod = sourceMapping.Timeperiod;
                    this.TimeperiodType = sourceMapping.TimeperiodType;
                }

                if (string.IsNullOrEmpty(this.UnitName))
                {
                    this.UnitName = sourceMapping.UnitName;
                    this.UnitGID = sourceMapping.UnitGID;
                }

                if (string.IsNullOrEmpty(this.Denominator))
                {
                    this.Denominator = sourceMapping.Denominator;
                    this.DenominatorType = sourceMapping.DenominatorType;
                }

                this.DefaultDecimalValue = sourceMapping.DefaultDecimalValue;
            }
        }
        
        /// <summary>
        /// Clears mapped values
        /// </summary>
        public void ClearMappedValues()
        {
            this._Area = string.Empty;
            this._AreaID = string.Empty;
            this.AreaType = DIMappingType.Value;

            this._Denominator = string.Empty;
            this.DenominatorType = DIMappingType.Value;

            this._Footnote = string.Empty;
            this.FootnoteType = DIMappingType.Value;

            this._IndicatorGID = string.Empty;
            this._IndicatorName = string.Empty;

            this._Source = string.Empty;
            this.SourceType = DIMappingType.Value;

            this._Subgroups.Clear();
            this._SubgroupVal = string.Empty;
            this._SubgroupValGID = string.Empty;

            this._Timeperiod = string.Empty;
            this.TimeperiodType = DIMappingType.Value;

            this._UnitGID = string.Empty;
            this._UnitName = string.Empty;
            this.DefaultDecimalValue = 0;
        }

        /// <summary>
        /// Imports mapped values into database and returns unmatched elements info
        /// </summary>
        /// <param name="mappedValues"></param>
        /// <param name="dataValue"></param>
        /// <param name="denominator"></param>
        /// <returns></returns>
        public UnMatchedElementInfo ImportMappedValuesIntoDB(DIConnection dbConnection, DIQueries dbQueries, string dataValue, string denominator)
        {
            UnMatchedElementInfo RetVal = new UnMatchedElementInfo();

            int IndicatorNId=-1;
            int UnitNId=-1;
            int SGNId=-1;
            int IUSNId = -1;

            int AreaNId = -1;
            int SourceNId = -1;
            int TimeperiodNId = -1;
            int DataNId = -1;

            Dictionary<string, int> IUSNIds = new Dictionary<string, int>();
            Dictionary<string, int> AreaNIds = new Dictionary<string, int>();
            Dictionary<string, int> SourceNIds = new Dictionary<string, int>();
            Dictionary<string, int> TimeperiodNIds = new Dictionary<string, int>();

            IndicatorBuilder IndicatorBuilderObj;
            UnitBuilder UnitBuilderObj;
            DI6SubgroupValBuilder SGValBuilderObj;
            IUSBuilder IUSBuilderObj;

            AreaBuilder AreaBuilderObj;
            SourceBuilder SourceBuilderObj;
            TimeperiodBuilder TimeperiodBuilderObj;
            DIDatabase TargetDatabase;

            try
            {
                IndicatorBuilderObj = new IndicatorBuilder(dbConnection, dbQueries);
                UnitBuilderObj = new UnitBuilder(dbConnection, dbQueries);
                SGValBuilderObj = new DI6SubgroupValBuilder(dbConnection, dbQueries);
                IUSBuilderObj = new IUSBuilder(dbConnection, dbQueries);

                AreaBuilderObj = new AreaBuilder(dbConnection, dbQueries);
                SourceBuilderObj = new SourceBuilder(dbConnection, dbQueries);
                TimeperiodBuilderObj = new TimeperiodBuilder(dbConnection, dbQueries);
                TargetDatabase = new DIDatabase(dbConnection, dbQueries);

                // import value
                if (this.IsVaildMappedValues())
                {
                    // check IUSNId in IUSNIDs list
                    if (IUSNIds.ContainsKey(this._IndicatorGID + this._UnitGID + this._SubgroupValGID))
                    {
                        IUSNId = IUSNIds[this._IndicatorGID + this._UnitGID + this._SubgroupValGID];
                    }

                    else
                    {
// indicator Nid
                        IndicatorNId=IndicatorBuilderObj.GetIndicatorNid(this._IndicatorGID,this._IndicatorName);

                        // UnitNId
                        UnitNId =UnitBuilderObj.GetUnitNid(this._UnitGID,this._UnitName);

                        // SG Val NID
                        SGNId=SGValBuilderObj.GetSubgroupValNid(this._SubgroupValGID,this._SubgroupVal);

                        //IUSNID
                        IUSNId = IUSBuilderObj.GetIUSNid(IndicatorNId, UnitNId, SGNId);

                        // if IUSNID <=0 then create the given IUS combinator into database
                        if (IUSNId <= 0)
                        {
                            IUSNId = IUSBuilderObj.InsertIUSIntoDB(this._IndicatorGID, this._UnitGID, this._SubgroupValGID);
                        }

                        IUSNIds.Add((this._IndicatorGID + this._UnitGID + this._SubgroupValGID), IUSNId);
                    }


                    if (IUSNId > 0)
                    {
                        // get AREANID
                        if (AreaNIds.ContainsKey(this._AreaID))
                        {
                            AreaNId = AreaNIds[this._AreaID];
                        }
                        else
                        {
                            AreaNId = AreaBuilderObj.GetAreaNidByAreaID(this._AreaID);
                            AreaNIds.Add(this._AreaID, AreaNId);
                        }

                        // get sourcenid
                        if (SourceNIds.ContainsKey(this._Source))
                        {
                            SourceNId = SourceNIds[this._Source];
                        }
                        else
                        {
                            SourceNId = SourceBuilderObj.CheckNCreateSource(this._Source);
                            SourceNIds.Add(this._Source, SourceNId);
                        }

                        // get timeperiodNId
                        if (TimeperiodNIds.ContainsKey(this._Timeperiod))
                        {
                            TimeperiodNId = TimeperiodNIds[this._Timeperiod];
                        }
                        else
                        {
                            TimeperiodNId = TimeperiodBuilderObj.CheckNCreateTimeperiod(this._Timeperiod);
                            TimeperiodNIds.Add(this._Timeperiod, TimeperiodNId);
                        }

                        if (AreaNId > 0 && IUSNId > 0 && SourceNId > 0 && TimeperiodNId > 0 && !string.IsNullOrEmpty(dataValue))
                        {
                            DataNId = TargetDatabase.CheckNCreateData(AreaNId, IUSNId, SourceNId, TimeperiodNId, dataValue);
                        }
                        

                        // denominator value
                        if (!string.IsNullOrEmpty(denominator) && DataNId > 0)
                        {
                            try
                            {
                                TargetDatabase.UpdateDenominatorValue(DataNId, Convert.ToInt32(Convert.ToDecimal( denominator)));
                            }
                            catch (Exception ex)
                            {
                                throw new ApplicationException(ex.ToString());
                            }
                        }
                    }

                }

                // for log file
                if (!string.IsNullOrEmpty(dataValue))
                {
                    if (AreaNId <= 0 || IUSNId <= 0 || SourceNId <= 0 || TimeperiodNId <= 0)
                    {
                        // for log file
                        if (AreaNId <= 0)
                        {
                            RetVal.Areas.Add(this._AreaID, this._Area);
                        }
                        if (IndicatorNId <= 0)
                        {
                            RetVal.Indicators.Add(this._IndicatorGID, this._IndicatorName);
                        }
                        if (UnitNId <= 0)
                        {
                            RetVal.Units.Add(this._UnitGID, this._UnitName);
                        }
                        if (SGNId <= 0)
                        {
                            RetVal.Subgroups.Add(this._SubgroupValGID, this._SubgroupVal);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        /// <summary>
        /// Returns true if mapped values are valid otherwise false
        /// </summary>
        /// <param name="mappedValues"></param>
        /// <returns></returns>
        public  bool IsVaildMappedValues()
        {
            bool RetVal = true;

                if (string.IsNullOrEmpty(this.AreaID))
                {
                    RetVal = false;
                }

                if (string.IsNullOrEmpty(this.IndicatorGID) || string.IsNullOrEmpty(this.UnitGID) || string.IsNullOrEmpty(this.SubgroupValGID))
                {
                    RetVal = false;
                }

                if (string.IsNullOrEmpty(this.Timeperiod))
                {
                    RetVal = false;
                }

                if (string.IsNullOrEmpty(this.Source))
                {
                    RetVal = false;
                }           

            return RetVal;
        }

        #endregion

        #endregion

    }


}