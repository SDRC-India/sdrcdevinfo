using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.Connection;


namespace DevInfo.Lib.DI_LibDAL.UserSelection
{
    /// <summary>
    /// Represents data view filters.
    /// This class supports the DevInfo Framework infrastructure and is not intended to be used directly from your code
    /// </summary>
    /// <remarks>
    /// Only used in DI application 
    /// To be exposed as property of UserSelection and to be initialized internally
    /// </remarks>
    public class DataViewFilters : ICloneable
    {

        #region " -- Private / Internal -- "

        #region " -- Constructor -- "
        /// <summary>
        /// Parameterless constructor
        /// </summary>
        /// <remarks>
        /// To be exposed as property of UserSelection and to be initialized internally
        /// </remarks>
        public DataViewFilters()
        {
        }
        #endregion

        #endregion

        #region " -- Public -- "

        #region " -- Properties -- "

        #region " -- Source Filter --"

        private string _DeletedSourceNIds = string.Empty;
        /// <summary>
        /// Comma delimited SourceNIds of deleted sources. 
        /// </summary>
        /// <remarks>
        /// <para>Set during the Filteration process by Sources on the DataView page only.</para>
        /// <para>Will store "'IUSNId1_SourceNId1',IUSNId1_SourceNId'" if the property ShowSourceByIUS = true</para>
        /// <para>Will store "SourceNId1,SourceNId2" if the property ShowSourceByIUS = false</para>
        /// </remarks>
        public string DeletedSourceNIds
        {
            get { return _DeletedSourceNIds; }
            set { _DeletedSourceNIds = value; }
        }

        private string _DeletedSourceNames = string.Empty;
        /// <summary>
        /// Comma delimited SourceNames of deleted sources. 
        /// </summary>
        /// <remarks>
        /// <para>To achieve interoperability of userselection among devinfo databases</para>
        /// <para>Set during the Filteration process by Sources on the DataView page only.</para>
        /// <para>Will store IndicatorGId_UnitGId_SubgroupGId_SourceName if the property ShowSourceByIUS = true</para>
        /// <para>Will store SourceName if the property ShowSourceByIUS = false</para>
        /// </remarks>
        public string DeletedSourceNames
        {
            get { return _DeletedSourceNames; }
            set { _DeletedSourceNames = value; }
        }


        private Boolean _ShowSourceByIUS = true;
        /// <summary>
        /// Display Sources by Indicator, Unit and Subgroup or only Sources
        /// </summary>
        /// <remarks>
        /// <para>True if Sources selected on the Source Filter page are by Indicator, Unit and Subgroup.</para>
        /// <para>True if Sources selected on the Source Filter page are by Source only.</para>
        /// <para>Generally used on the Source Filter page for Data View</para>
        /// </remarks>
        public Boolean ShowSourceByIUS
        {
            get { return _ShowSourceByIUS; }
            set { _ShowSourceByIUS = value; }
        }

        #endregion

        #region " -- Unit Filter --"

        private string _DeletedUnitNIds = string.Empty;
        /// <summary>
        /// comma delimited IndicatorNID_UnitNId / UnitNIds for deleted units. 
        /// </summary>
        /// <remarks>
        /// <para>Set during the Filteration process by Units on the DataView page only.</para>
        /// <para>Will store comma delimited IndicatorNID_UnitNId if the property ShowUnitByIndicator = true</para>
        /// <para>Will store comma delimited UnitNId if the property ShowUnitByIndicator = false</para>
        /// </remarks>
        public string DeletedUnitNIds
        {
            get { return _DeletedUnitNIds; }
            set { _DeletedUnitNIds = value; }
        }

        private string _DeletedUnitGIds = string.Empty;
        /// <summary>
        /// comma delimited IndicatorGId_UnitGId / UnitGIds for deleted units. 
        /// </summary>
        /// <remarks>
        /// <para>To achieve interoperability of userselection among devinfo databases</para>
        /// <para>Set during the Filteration process by Units on the DataView page only.</para>
        /// <para>Will store comma delimited IndicatorGId_UnitGId if the property ShowUnitByIndicator = true</para>
        /// <para>Will store comma delimited UnitGId if the property ShowUnitByIndicator = false</para>
        /// </remarks>
        public string DeletedUnitGIds
        {
            get { return _DeletedUnitGIds; }
            set { _DeletedUnitGIds = value; }
        }

        private Boolean _ShowUnitByIndicator = true;
        /// <summary>
        /// Display Units by Indicator or only Units
        /// </summary>
        /// <remarks>
        /// <para>True if Units selected on the Unit Filter page are by Indicator.</para>
        /// <para>True if Units selected on the Unit Filter page are by Unit only.</para>
        /// <para>Generally used on the Unit Filter page for Data View</para>
        /// </remarks>
        public Boolean ShowUnitByIndicator
        {
            get { return _ShowUnitByIndicator; }
            set { _ShowUnitByIndicator = value; }
        }

        #endregion

        #region " -- Subgroup Filter --"

        private string _DeletedSubgroupNIds = string.Empty;
        /// <summary>
        /// Deleted comma delimited SubgroupNIds. 
        /// </summary>
        /// <remarks>
        /// <para>Set during the Filteration process by Subgroups on the DataView page only.</para>
        /// <para>Will store IndicatorNId_SubgroupNId if the property ShowSubgroupByIndicator = true</para>
        /// <para>Will store SubgroupNId if the property ShowSubgroupByIndicator = false</para>
        /// </remarks>
        public string DeletedSubgroupNIds
        {
            get { return _DeletedSubgroupNIds; }
            set { _DeletedSubgroupNIds = value; }
        }

        private string _DeletedSubgroupGIds = string.Empty;
        /// <summary>
        /// Deleted comma delimited SubgroupGIds. 
        /// </summary>
        /// <remarks>
        /// <para>To achieve interoperability of userselection among devinfo databases</para>
        /// <para>Set during the Filteration process by Subgroups on the DataView page only.</para>
        /// <para>Will store IndicatorGId_SubgroupGId if the property ShowSubgroupByIndicator = true</para>
        /// <para>Will store SubgroupGIds if the property ShowSubgroupByIndicator = false</para>
        /// </remarks>
        public string DeletedSubgroupGIds
        {
            get { return _DeletedSubgroupGIds; }
            set { _DeletedSubgroupGIds = value; }
        }

        private Boolean _ShowSubgroupByIndicator = true;
        /// <summary>
        /// Display Subgroups by Indicator or only Subgroups
        /// </summary>
        /// <remarks>
        /// <para>True if Subgroups selected on the Subgroup Filter page are by Indicator.</para>
        /// <para>True if Subgroups selected on the Subgroup Filter page are by Subgroup only.</para>
        /// <para>Generally used on the Subgroup Filter page for Data View</para>
        /// </remarks>
        public Boolean ShowSubgroupByIndicator
        {
            get { return _ShowSubgroupByIndicator; }
            set { _ShowSubgroupByIndicator = value; }
        }

        #endregion

        #region " -- DataPoint Filter --"

        private string _DeletedDataNIds = string.Empty;
        /// <summary>
        /// Deleted comma delimited Data NIDs. 
        /// </summary>
        /// <remarks>
        /// <para>Generally set during the UnCheck process on the DataView page.</para>
        /// <para>Will store the Data NIDs that have been removed/deleted from the DataView</para>
        /// </remarks>
        public string DeletedDataNIds
        {
            get { return _DeletedDataNIds; }
            set { _DeletedDataNIds = value; }
        }


        private string _DeletedDataGIds = string.Empty;
        /// <summary>
        /// Deleted comma delimited Data NIDs. 
        /// </summary>
        /// <remarks>
        /// <para>To achieve interoperability of userselection among devinfo databases</para>
        /// <para>Generally set during the UnCheck process on the DataView page.</para>
        /// <para>Will store the IndicatorGId_UnitGId_SubgroupGId_TimePeriod_AreaId_SourceName that have been removed/deleted from the DataView</para>
        /// </remarks>
        public string DeletedDataGIds
        {
            get { return _DeletedDataGIds; }
            set { _DeletedDataGIds = value; }
        }

        #endregion

        #region " -- MRD Filter --"

        private bool _MostRecentData = false;
        /// <summary>
        /// Gets or sets the condition whether Most Recent Data is to be considered
        /// </summary>
        public bool MostRecentData
        {
            get { return _MostRecentData; }
            set { _MostRecentData = value; }
        }

        #endregion

        #region " -- DataValueFilter -- "
        private DataValueFilter _DataValueFilter = new DataValueFilter();
        /// <summary>
        /// Gets DataValue Filters
        /// </summary>
        public DataValueFilter DataValueFilter
        {
            get { return _DataValueFilter; }
            set { _DataValueFilter = value; } //Only for XML Serilaization mandate
        }


        #endregion

        #region " -- IndicatorDataValueFilters -- "
        private IndicatorDataValueFilters _IndicatorDataValueFilters = new IndicatorDataValueFilters();
        /// <summary>
        /// Collection of IndicatorDataValueFilters 
        /// </summary>
        public IndicatorDataValueFilters IndicatorDataValueFilters
        {
            get { return _IndicatorDataValueFilters; }
            set { _IndicatorDataValueFilters = value; } //Only for XML Serilaization mandate
        }

        #endregion

        #region " -- UltraWinGrid Auto Filters -- "

        private UltraWinGridAutoFilters _UltraWinGridAutoFilters = new UltraWinGridAutoFilters();
        /// <summary>
        /// Collection of UltraWinGridAutoFilters 
        /// </summary>
        public UltraWinGridAutoFilters UltraWinGridAutoFilters
        {
            get { return _UltraWinGridAutoFilters; }
            set { _UltraWinGridAutoFilters = value; } //Only for XML Serilaization mandate
        }


        #endregion

        #region " -- Recommended Source Filter -- "

        private bool _ShowRecommendedSourceByRank = true;
        /// <summary>
        /// Gets or sets the ShowRecommendedSourceByRank
        /// </summary>
        public bool ShowRecommendedSourceByRank
        {
            get 
            {
                return this._ShowRecommendedSourceByRank; 
            }
            set 
            {
                this._ShowRecommendedSourceByRank = value; 
            }
        }

        private string _DeletedRanks = string.Empty;
        /// <summary>
        /// Gets or sets the deleted rank
        /// </summary>
        public string DeletedRanks
        {
            get 
            { 
                return this._DeletedRanks; 
            }
            set 
            {
                this._DeletedRanks = value; 
            }
        }
	
        #endregion

        #endregion

        #region " -- Methods -- "

        /// <summary>
        /// Clears all dataview filters
        /// </summary>
        public void Reset()
        {
            this._MostRecentData = false;
            this._DeletedDataNIds = string.Empty;
            this._DeletedSourceNIds = string.Empty;
            this._DeletedUnitNIds = string.Empty;
            this._DeletedSubgroupNIds = string.Empty;

            this._DeletedDataGIds = string.Empty;
            this._DeletedSourceNames = string.Empty;
            this._DeletedUnitGIds = string.Empty;
            this._DeletedSubgroupGIds = string.Empty;

            this._ShowSourceByIUS = true;
            this._ShowSubgroupByIndicator = true;
            this._ShowUnitByIndicator = true;

            this._ShowRecommendedSourceByRank = true;
            this._DeletedRanks = string.Empty;

            this._MostRecentData = false;

            this._DataValueFilter = new DataValueFilter();
            this._IndicatorDataValueFilters.Clear();
            this._IndicatorDataValueFilters.ShowIUS = true;
            this._IndicatorDataValueFilters.IncludeArea = false;
            this._UltraWinGridAutoFilters.Clear();
        }

        /// <summary>
        /// It returns a boolean value based on existance of any filter condition in DataViewFilters
        /// </summary>
        /// <param name="ExcludeDeletedDataPointFilter">boolean to identify whether DataPoint marked for deletion are to be considered</param>
        /// <returns></returns>
        public bool FilterExists(bool ExcludeDeletedDataPointFilter)
        {
            bool RetVal = false;

            if (this._DeletedRanks.Trim().Length > 0 || this._UltraWinGridAutoFilters.Count > 0 || this._DeletedSourceNIds.Trim().Length > 0 || this._DeletedUnitNIds.Trim().Length > 0 || this._DeletedSubgroupNIds.Trim().Length > 0 || this._IndicatorDataValueFilters.Count > 0 || this._DataValueFilter.OpertorType != DevInfo.Lib.DI_LibDAL.UserSelection.OpertorType.None || this._MostRecentData)
            {
                RetVal = true;
            }
            else
            {
                if (ExcludeDeletedDataPointFilter == true)
                {
                    RetVal = false;
                }
                else
                {
                    if (this._DeletedDataNIds.Trim().Length > 0)
                    {
                        RetVal = true;
                    }
                    else
                    {
                        RetVal = false;
                    }
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Constructs SQL statement from DataViewFilters structure
        /// </summary>
        /// <param name="DIServerType">Server type required for building database specific sql syntax</param>
        /// <param name="ExcludeDeletedDataPointFilter">boolean to identify whether DataPoint marked for deletion are to be considered</param>
        /// <returns>SQL Statement with "AND" prefixed</returns>
        internal string SQL_GetDataViewFilters(DIServerType DIServerType, bool ExcludeDeletedDataPointFilter)
        {
            string RetVal = string.Empty;


            // Deleted Sources
            if (this._DeletedSourceNIds.Trim().Length > 0)
            {
                if (this._ShowSourceByIUS == true)
                {
                    RetVal += " AND (" + DIQueries.SQL_GetConcatenatedValues("D." + Data.IUSNId + ",D." + Data.SourceNId, Delimiter.NUMERIC_DELIMITER, Delimiter.NUMERIC_SEPARATOR, DIServerType) + ") NOT IN (" + this._DeletedSourceNIds + ")";
                }
                else
                {
                    RetVal += " AND D." + Data.DataNId + " NOT IN (" + this._DeletedSourceNIds + ")";
                }
            }

            // Deleted Units
            if (this._DeletedUnitNIds.Trim().Length > 0)
            {
                if (this._ShowUnitByIndicator == true)
                {
                    RetVal += " AND (" + DIQueries.SQL_GetConcatenatedValues("IUS." + Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + Indicator_Unit_Subgroup.UnitNId, Delimiter.NUMERIC_DELIMITER, Delimiter.NUMERIC_SEPARATOR, DIServerType) + ") NOT IN (" + this._DeletedUnitNIds + ")";
                }
                else
                {
                    RetVal += " AND IUS." + Indicator_Unit_Subgroup.UnitNId + " NOT IN (" + this._DeletedUnitNIds + ")";
                }
            }

            // Deleted Subgroups
            if (this._DeletedSubgroupNIds.Trim().Length > 0)
            {
                if (this._ShowSubgroupByIndicator == true)
                {
                    RetVal += " AND (" + DIQueries.SQL_GetConcatenatedValues("IUS." + Indicator_Unit_Subgroup.IndicatorNId + ",IUS." + Indicator_Unit_Subgroup.SubgroupValNId, Delimiter.NUMERIC_DELIMITER, Delimiter.NUMERIC_SEPARATOR, DIServerType) + ") NOT IN (" + this._DeletedSubgroupNIds + ")";
                }
                else
                {
                    RetVal += " AND IUS." + Indicator_Unit_Subgroup.SubgroupValNId + " NOT IN (" + this._DeletedSubgroupNIds + ")";
                }
            }

            // DataValue Filter
            RetVal += this._DataValueFilter.SQL_GetDataValueFilter(DIServerType);

            // Indicator DataValue Filters
            RetVal += this._IndicatorDataValueFilters.SQL_GetIndicatorDataValueFilters(DIServerType);

            //TODO UltraWinGridAutoFilters

            // Deleted DataPoints 
            if (!ExcludeDeletedDataPointFilter)
            {
                if (this._DeletedDataNIds.Trim().Length > 0)
                {
                    RetVal += " AND D." + Data.DataNId + " NOT IN (" + this._DeletedDataNIds + ")";
                }
            }
            return RetVal;
        }



        #endregion

        #region " -- ICloneable Members -- "

        public object Clone()
        {
            object RetVal = null;
            try
            {
                XmlSerializer XmlSerializer = new XmlSerializer(typeof(DataViewFilters));
                MemoryStream MemoryStream = new MemoryStream();
                XmlSerializer.Serialize(MemoryStream, this);
                MemoryStream.Position = 0;
                RetVal = (DataViewFilters)XmlSerializer.Deserialize(MemoryStream);
                MemoryStream.Close();
                MemoryStream.Dispose();
            }
            catch (Exception)
            {
                RetVal = null;
            }
            return (DataViewFilters)RetVal;
        }

        #endregion


        #endregion
    }
}
