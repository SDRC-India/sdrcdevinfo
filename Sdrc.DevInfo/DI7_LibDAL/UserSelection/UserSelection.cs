using System;
using System.Data;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Text;

using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;


namespace DevInfo.Lib.DI_LibDAL.UserSelection
{


    #region " -- Delegates -- "

    /// <summary>
    /// Delegate for change in user selection.
    /// </summary>
    public delegate void UserSelectionChangeDelegate();

    #endregion


    /// <summary>
    /// Represents the user selections for generating dataview
    /// </summary>
    public class UserSelection : ICloneable
    {

        #region " -- Public --"

        #region " -- Constants --"



        #endregion

        #region " -- Properties -- "

        #region " -- Selection -- "

        #region " -- Indicator -- "

        private string _IndicatorNIds = string.Empty;
        /// <summary>
        /// Selected comma delimited Indicator/IUS NIDs. 
        /// </summary>
        /// <remarks>
        /// <para>Generally set during the selection process on the Indicator page.</para>
        /// <para>Will store IUSNIDs if the property _ShowIUS = true</para>
        /// <para>Will store IndicatorNIDs if the property ShowIUS = false</para>
        /// </remarks>
        public string IndicatorNIds
        {
            get { return _IndicatorNIds; }
            set { _IndicatorNIds = value; }
        }

        private string _IndicatorGIds = string.Empty;
        /// <summary>
        /// Selected comma delimited Indicator / I_U_S GIds. 
        /// </summary>
        /// <remarks>
        /// <para>To achieve interoperability of userselection among devinfo databases</para>
        /// <para>Generally set during the selection process on the Indicator page.</para>
        /// <para>Will store IUSGIds if the property _ShowIUS = true</para>
        /// <para>Will store IndicatorGIds if the property ShowIUS = false</para>
        /// </remarks>
        public string IndicatorGIds
        {
            get { return _IndicatorGIds; }
            set { _IndicatorGIds = value; }
        }

        private Boolean _ShowIUS = true;
        /// <summary>
        /// Display Indicator by Unit and Subgroup or only Indicator
        /// </summary>
        /// <remarks>
        /// <para>True if Indicators selected on the Indicator Selection page are by IUS.</para>
        /// <para>False if Indicators selected on the Indicator Selection page are by Indicator only.</para>
        /// </remarks>
        public Boolean ShowIUS
        {
            get
            {
                return _ShowIUS;
            }
            set
            {
                _ShowIUS = value;
            }
        }

        private string _IndicatorUnitSelectionDetails = string.Empty;
        /// <summary>
        /// Gets or sets the IndicatorUnitSelectionDetails
        /// </summary>
        /// <remarks>Indicator,Unit_WhereDataExists,Combinations_{#}_</remarks>
        public string IndicatorUnitSelectionDetails
        {
            get 
            {
                return this._IndicatorUnitSelectionDetails; 
            }
            set 
            {
                this._IndicatorUnitSelectionDetails = value; 
            }
        }
	

        #endregion

        #region " -- Unit -- "

        private string _UnitNIds = string.Empty;
        /// <summary>
        /// Selected comma delimited Unit NIDs. 
        /// </summary>
        /// <remarks>
        /// Generally set during the selection process on Tools Export. 
        /// </remarks>
        public string UnitNIds
        {
            get { return _UnitNIds; }
            set { _UnitNIds = value; }
        }

        private string _UnitGIds = string.Empty;
        /// <summary>
        /// Selected comma delimited Unit GIds. 
        /// </summary>
        /// <remarks>
        /// <para>To achieve interoperability of userselection among devinfo databases</para>
        /// <para>Generally set during the selection process  on Tools Export</para>
        /// </remarks>
        public string UnitGIds
        {
            get { return _UnitGIds; }
            set { _UnitGIds = value; }
        }

        #endregion

        #region " -- SubgroupVal -- "

        private string _SubgroupValNIds = string.Empty;
        /// <summary>
        /// Selected comma delimited SubgroupVal NIDs. 
        /// </summary>
        /// <remarks>
        /// Generally set during the selection process on Tools Export. 
        /// </remarks>
        public string SubgroupValNIds
        {
            get { return _SubgroupValNIds; }
            set { _SubgroupValNIds = value; }
        }

        private string _SubgroupValGIds = string.Empty;
        /// <summary>
        /// Selected comma delimited SubgroupVal GIds. 
        /// </summary>
        /// <remarks>
        /// <para>To achieve interoperability of userselection among devinfo databases</para>
        /// <para>Generally set during the selection process  on Tools Export</para>
        /// </remarks>
        public string SubgroupValGIds
        {
            get { return _SubgroupValGIds; }
            set { _SubgroupValGIds = value; }
        }

        #endregion

        #region " -- TimePeriod -- "

        private string _TimePeriodNIds = string.Empty;
        /// <summary>
        /// Selected comma delimited Time Period NIDs. 
        /// </summary>
        /// <remarks>
        /// Generally set during the selection process on the Time Period page
        /// </remarks>
        public string TimePeriodNIds
        {
            get { return _TimePeriodNIds; }
            set { _TimePeriodNIds = value; }
        }

        private string _TimePeriods = string.Empty;
        /// <summary>
        /// Selected comma delimited TimePeriod Values. 
        /// </summary>
        /// <remarks>
        /// <para>To achieve interoperability of userselection among devinfo databases</para>
        /// <para>Generally set during the selection process on the Time Period page</para>
        /// </remarks>
        public string TimePeriods
        {
            get { return _TimePeriods; }
            set { _TimePeriods = value; }
        }

        #endregion

        #region " -- Area -- "

        private string _AreaNIds = string.Empty;
        /// <summary>
        /// Selected comma delimited Area NIDs. 
        /// </summary>
        /// <remarks>
        /// Generally set during the selection process on the Area page
        /// </remarks>
        public string AreaNIds
        {
            get { return _AreaNIds; }
            set { _AreaNIds = value; }
        }

        private string _AreaIds = string.Empty;
        /// <summary>
        /// Selected comma delimited AreaIDs. 
        /// </summary>
        /// <remarks>
        /// <para>To achieve interoperability of userselection among devinfo databases</para>
        /// <para>Generally set during the selection process on the Area page</para>
        /// </remarks>
        public string AreaIds
        {
            get { return _AreaIds; }
            set { _AreaIds = value; }
        }

        #endregion

        #region " -- Source -- "

        private string _SourceNIds = string.Empty;
        /// <summary>
        /// Selected comma delimited Source NIDs. 
        /// </summary>
        /// <remarks>
        /// Generally set during the selection process on Tools Export
        /// </remarks>
        public string SourceNIds
        {
            get { return _SourceNIds; }
            set { _SourceNIds = value; }
        }

        private string _SourceNames = string.Empty;
        /// <summary>
        /// Selected comma delimited SourceNames. 
        /// </summary>
        /// <remarks>
        /// <para>To achieve interoperability of userselection among devinfo databases</para>
        /// <para>Generally set during the selection process on Tools Export</para>
        /// </remarks>
        public string SourceNames
        {
            get { return _SourceNames; }
            set { _SourceNames = value; }
        }

        #endregion

        #region " -- IC -- "

        private string _ICNIds = string.Empty;
        /// <summary>
        /// Selected comma delimited ICNIds. 
        /// </summary>
        /// <remarks>
        /// Generally set during the selection process of Extract Module
        /// </remarks>
        public string ICNIds
        {
            get { return _ICNIds; }
            set { _ICNIds = value; }
        }

        private string _ICGIds = string.Empty;
        /// <summary>
        /// Selected comma delimited _ICGIds. 
        /// </summary>
        /// <remarks>
        /// <para>To achieve interoperability of userselection among devinfo databases</para>
        /// <para>Generally set during the selection process of Extract Module</para>
        /// </remarks>
        public string ICGIds
        {
            get { return _ICGIds; }
            set { _ICGIds = value; }
        }

        #endregion

        #endregion

        #region " -- Filters -- "

        private DataViewFilters _DataViewFilters = new DataViewFilters();
        /// <summary>Userselection Filters</summary>
        /// <remarks>
        /// <para>Filters Class exposed as property</para>
        /// <para>set only for xml serialization</para>
        /// </remarks>
        public DataViewFilters DataViewFilters
        {
            get { return _DataViewFilters; }
            set { _DataViewFilters = value; }

        }

        #endregion

        #region " -- Selection Changed -- "
        private bool _UserSelectionChanged = true;
        /// <summary>
        /// Gets or sets the changed user selection.
        /// </summary>
        public bool UserSelectionChanged
        {
            get
            {
                return this._UserSelectionChanged;
            }
            set
            {
                this._UserSelectionChanged = value;
                if (value)
                {
                    this.RaiseUserSelectionChangedEvent();
                }
            }
        }

        #endregion

        #endregion

        #region " -- Methods -- "

        /// <summary>
        /// Save the UserSelection in form of XML file.
        /// </summary>
        /// <param name="fileNameWPath">File path of Serialized xml</param>        
        public void Save(string fileNameWPath)
        {
            XmlSerializer UserSelectionSerialize = new XmlSerializer(typeof(UserSelection));
            StreamWriter UserSelectionWriter = new StreamWriter(fileNameWPath);
            UserSelectionSerialize.Serialize(UserSelectionWriter, this);
            UserSelectionWriter.Close();
        }

        /// <summary>
        /// Load the UserSelection in form of XML file.
        /// </summary>
        /// <param name="fileNameWPath">File path of Serialized xml</param>        
        public static UserSelection Load(string fileNameWPath)
        {
            UserSelection Retval;
            try
            {
                XmlSerializer UserSelectionSerialize = new XmlSerializer(typeof(UserSelection));
                StreamReader UserSelectionReader = new StreamReader(fileNameWPath);
                Retval = (UserSelection)UserSelectionSerialize.Deserialize(UserSelectionReader);
                UserSelectionReader.Close();
            }
            catch (Exception ex)
            {
                Retval = null;
            }
            return Retval;
        }

        /// <summary>
        /// Reset the user selection.
        /// </summary>
        public void Reset()
        {
            this._ShowIUS = true;

            this._IndicatorNIds = string.Empty;
            this._IndicatorGIds = string.Empty;
            this._IndicatorUnitSelectionDetails = string.Empty;

            this._UnitNIds = string.Empty;
            this._UnitGIds = string.Empty;

            this._SubgroupValNIds = string.Empty;
            this._SubgroupValGIds = string.Empty;

            this._TimePeriodNIds = string.Empty;
            this._TimePeriods = string.Empty;

            this._AreaNIds = string.Empty;
            this._AreaIds = string.Empty;

            this._SourceNIds = string.Empty;
            this._SourceNames = string.Empty;

            this._ICNIds = string.Empty;
            this._ICGIds = string.Empty;

            this.UserSelectionChanged = true;

            this._DataViewFilters.Reset();
        }

        /// <summary>
        /// Resets the current selection and copies the new selection.
        /// </summary>
        /// <param name="userSelection"></param>
        public void CopyUserSelection(UserSelection userSelection)
        {
            // -- Reset the current Selections.
            this.Reset();

            // -- Indicators
            this._IndicatorNIds = userSelection.IndicatorNIds;
            this._IndicatorGIds = userSelection.IndicatorGIds;

            this._ShowIUS = userSelection.ShowIUS;

            // -- Area
            this._AreaIds = userSelection.AreaIds;
            this._AreaNIds = userSelection._AreaNIds;

            // -- Time periods
            this.TimePeriodNIds = userSelection.TimePeriodNIds;
            this.TimePeriods = userSelection.TimePeriods;

            // -- source
            this.SourceNames = userSelection.SourceNames;
            this.SourceNIds = userSelection.SourceNIds;

            // -- Subgroup
            this.SubgroupValNIds = userSelection.SubgroupValNIds;
            this.SubgroupValGIds = userSelection.SubgroupValGIds;

            // -- Unit
            this.UnitNIds = userSelection.UnitNIds;
            this.UnitGIds = userSelection.UnitGIds;

            // -- ICs
            this.ICNIds = userSelection.ICNIds;
            this.ICGIds = userSelection.ICGIds;

            // -- Datavalue filters
            this.DataViewFilters.DataValueFilter.FromDataValue = userSelection.DataViewFilters.DataValueFilter.FromDataValue;
            this.DataViewFilters.DataValueFilter.ToDataValue = userSelection.DataViewFilters.DataValueFilter.ToDataValue;
            this.DataViewFilters.DataValueFilter.OpertorType = userSelection.DataViewFilters.DataValueFilter.OpertorType;

            // -- Deleted Data NIds
            this.DataViewFilters.DeletedDataNIds = userSelection.DataViewFilters.DeletedDataNIds;
            this.DataViewFilters.DeletedDataGIds = userSelection.DataViewFilters.DeletedDataGIds;

            // -- Deleted Source Nids
            this.DataViewFilters.ShowSourceByIUS = userSelection.DataViewFilters.ShowSourceByIUS;
            this.DataViewFilters.DeletedSourceNames = userSelection.DataViewFilters.DeletedSourceNames;
            this.DataViewFilters.DeletedSourceNIds = userSelection.DataViewFilters.DeletedSourceNIds;

            // -- Deleted Recommended Source 
            this.DataViewFilters.ShowRecommendedSourceByRank = userSelection.DataViewFilters.ShowRecommendedSourceByRank;
            this.DataViewFilters.DeletedRanks = userSelection.DataViewFilters.DeletedRanks;

            // -- Deleted Subgroup Nids
            this.DataViewFilters.ShowSubgroupByIndicator = userSelection.DataViewFilters.ShowSubgroupByIndicator;
            this.DataViewFilters.DeletedSubgroupNIds = userSelection.DataViewFilters.DeletedSubgroupNIds;
            this.DataViewFilters.DeletedSubgroupGIds = userSelection.DataViewFilters.DeletedSubgroupGIds;

            // -- Deleted Indicators Nids
            this.DataViewFilters.ShowUnitByIndicator = userSelection.DataViewFilters.ShowUnitByIndicator;
            this.DataViewFilters.DeletedUnitNIds = userSelection.DataViewFilters.DeletedUnitNIds;
            this.DataViewFilters.DeletedUnitGIds = userSelection.DataViewFilters.DeletedUnitGIds;

            // -- MRD
            this.DataViewFilters.MostRecentData = userSelection.DataViewFilters.MostRecentData;

            // -- Indicator Data Value filters
            foreach (IndicatorDataValueFilter Filter in userSelection.DataViewFilters.IndicatorDataValueFilters)
            {
                this.DataViewFilters.IndicatorDataValueFilters.Add(Filter);
            }
            this.DataViewFilters.IndicatorDataValueFilters.IncludeArea = userSelection.DataViewFilters.IndicatorDataValueFilters.IncludeArea;
        }

        #endregion

        #region " -- NId / GId Conversion -- "
        /// <summary>
        /// Update / Save GIds on the basis of NIds
        /// </summary>
        /// <param name="DIConnection"></param>
        /// <param name="DIQueries"></param>
        public void UpdateGIdsFromNIds(DIConnection DIConnection, DIQueries DIQueries)
        {
            IDataReader IDataReader;
            string IdValue = string.Empty;
            string sSql = string.Empty;

            #region Indicator GId

            if (this._IndicatorNIds.Length > 0)
            {
                if (this._ShowIUS == true)
                {
                    sSql = DIQueries.IUS.GetIUSGIds(this._IndicatorNIds, DIConnection.ConnectionStringParameters.ServerType);
                }
                else
                {
                    sSql = DIQueries.Indicators.GetIndicator(FilterFieldType.NId, this._IndicatorNIds, FieldSelection.Light);
                }
                IDataReader = DIConnection.ExecuteReader(sSql);
                while (IDataReader.Read())
                {
                    if (IdValue.Length > 0)
                    {
                        IdValue += Delimiter.TEXT_DELIMITER;
                    }

                    if (this._ShowIUS == true)
                    {
                        IdValue += IDataReader[0].ToString(); // Expression column with concatinated I_U_S Gids
                    }
                    else
                    {
                        IdValue += IDataReader[Indicator.IndicatorGId].ToString();
                    }
                }
                IDataReader.Close();
                this._IndicatorGIds = IdValue;
            }
            else
            { 
                //-- Clear the GID, if NID is blank
                this._IndicatorGIds = string.Empty;
            }

            #endregion

            #region Unit GId

            if (this._UnitNIds.Length > 0)
            {
                sSql = DIQueries.Unit.GetUnit(FilterFieldType.NId, this._UnitNIds);
                IDataReader = DIConnection.ExecuteReader(sSql);
                IdValue = string.Empty;

                while (IDataReader.Read())
                {
                    if (IdValue.Length > 0)
                    {
                        IdValue += Delimiter.NUMERIC_DELIMITER;
                    }
                    IdValue += IDataReader[Unit.UnitGId].ToString();
                }
                IDataReader.Close();
                this._UnitGIds = IdValue;
            }
            else
            {
                //-- Clear the GID, if NID is blank
                this._UnitGIds = string.Empty;
            }

            #endregion

            #region SubgroupVal GId

            if (this._SubgroupValNIds.Length > 0)
            {
                sSql = DIQueries.Subgroup.GetSubgroupVals(FilterFieldType.NId, this._SubgroupValNIds);
                IDataReader = DIConnection.ExecuteReader(sSql);
                IdValue = string.Empty;

                while (IDataReader.Read())
                {
                    if (IdValue.Length > 0)
                    {
                        IdValue += Delimiter.NUMERIC_DELIMITER;
                    }
                    IdValue += IDataReader[SubgroupVals.SubgroupValGId].ToString();

                }
                IDataReader.Close();
                this._SubgroupValGIds = IdValue;
            }
            else
            {
                //-- Clear the GID, if NID is blank
                this._SubgroupValGIds = string.Empty;
            }

            #endregion

            #region Source Name

            if (this._SourceNIds.Length > 0)
            {
                sSql = DIQueries.Source.GetSource(FilterFieldType.NId, this._SourceNIds, FieldSelection.Light, false);
                IDataReader = DIConnection.ExecuteReader(sSql);
                IdValue = string.Empty;

                while (IDataReader.Read())
                {
                    if (IdValue.Length > 0)
                    {
                        IdValue += Delimiter.TEXT_DELIMITER;
                    }
                    IdValue += IDataReader[IndicatorClassifications.ICName].ToString();
                }
                IDataReader.Close();
                this._SourceNames = IdValue;
            }
            else
            {
                //-- Clear the GID, if NID is blank
                this._SourceNames = string.Empty;
            }

            #endregion

            #region TimePeriod

            if (this._TimePeriodNIds.Length > 0)
            {
                sSql = DIQueries.Timeperiod.GetTimePeriod(FilterFieldType.NId, this.TimePeriodNIds);
                IDataReader = DIConnection.ExecuteReader(sSql);
                IdValue = string.Empty;
                while (IDataReader.Read())
                {
                    if (IdValue.Length > 0)
                    {
                        IdValue += Delimiter.TEXT_DELIMITER;
                    }
                    IdValue += IDataReader[Timeperiods.TimePeriod].ToString();
                }
                IDataReader.Close();
                this._TimePeriods = IdValue;
            }
            else
            {
                //-- Clear the GID, if NID is blank
                this._TimePeriods = string.Empty;
            }

            #endregion

            #region AreaId

            if (this._AreaNIds.Length > 0)
            {
                sSql = DIQueries.Area.GetArea(FilterFieldType.NId, this._AreaNIds);
                IDataReader = DIConnection.ExecuteReader(sSql);
                IdValue = string.Empty;
                while (IDataReader.Read())
                {
                    if (IdValue.Length > 0)
                    {
                        IdValue += Delimiter.TEXT_DELIMITER;
                    }
                    IdValue += IDataReader[Area.AreaID].ToString();
                }
                this._AreaIds = IdValue;

                IDataReader.Close();
            }
            else
            {
                //-- Clear the GID, if NID is blank
                this._AreaIds = string.Empty;
            }

            #endregion

            #region Source Filter

            if (this._DataViewFilters.DeletedSourceNIds.Length > 0 && this.DataViewFilters.ShowSourceByIUS)
            {
                //if (this.DataViewFilters.ShowSourceByIUS)
                //{
                    sSql = DIQueries.Source.GetIndGId_UnitGId_SubgroupGID_SoureNames(this.DataViewFilters.DeletedSourceNIds, DIConnection.ConnectionStringParameters.ServerType);
                //}
                //else
                //{
                //    sSql = DIQueries.IndicatorClassification.GetIC(FilterFieldType.NId, this.DataViewFilters.DeletedSourceNIds, ICType.Source, FieldSelection.Name);
                //}
                IDataReader = DIConnection.ExecuteReader(sSql);

                IdValue = string.Empty;
                while (IDataReader.Read())
                {
                    if (IdValue.Length > 0)
                    {
                        IdValue += Delimiter.TEXT_DELIMITER;
                    }

                    if (this.DataViewFilters.ShowSourceByIUS)
                    {
                        IdValue += IDataReader[0].ToString();   // Expression column with concatinated IndGId_UnitGId_SubgroupGId_SourceName
                    }
                    else
                    {
                        IdValue += IDataReader[IndicatorClassifications.ICName].ToString();
                    }

                }
                this.DataViewFilters.DeletedSourceNames = IdValue;
                IDataReader.Close();
            }
            else if (this._DataViewFilters.DeletedSourceNames.Length == 0)
            {
                //-- Clear the GID, if NID is blank
                this._DataViewFilters.DeletedSourceNames = string.Empty;
            }

            #endregion

            #region Unit Filter

            if (this._DataViewFilters.DeletedUnitNIds.Length > 0)
            {
                if (this.DataViewFilters.ShowUnitByIndicator)
                {
                    sSql = DIQueries.Unit.GetIndGId_UnitGIds(this.DataViewFilters.DeletedUnitNIds, DIConnection.ConnectionStringParameters.ServerType);
                }
                else
                {
                    sSql = DIQueries.Unit.GetUnit(FilterFieldType.NId, this.DataViewFilters.DeletedUnitNIds);
                }
                IdValue = string.Empty;
                IDataReader = DIConnection.ExecuteReader(sSql);
                while (IDataReader.Read())
                {
                    if (IdValue.Length > 0)
                    {
                        IdValue += Delimiter.TEXT_DELIMITER;
                    }

                    if (this.DataViewFilters.ShowUnitByIndicator)
                    {
                        IdValue += IDataReader[0].ToString();
                    }
                    else
                    {
                        IdValue += IDataReader[Unit.UnitGId].ToString();
                    }
                }
                IDataReader.Close();
                this.DataViewFilters.DeletedUnitGIds = IdValue;
            }
            else
            {
                //-- Clear the GID, if NID is blank
                this._DataViewFilters.DeletedUnitGIds = string.Empty;
            }

            #endregion

            #region Subgroup Filter

            if (this.DataViewFilters.DeletedSubgroupNIds.Length > 0)
            {
                if (this.DataViewFilters.ShowSubgroupByIndicator)
                {
                    sSql = DIQueries.Subgroup.GetIndGId_SubgroupGIds(this.DataViewFilters.DeletedSubgroupNIds, DIConnection.ConnectionStringParameters.ServerType);
                }
                else
                {
                    sSql = DIQueries.Subgroup.GetSubgroupVals(FilterFieldType.NId, this.DataViewFilters.DeletedSubgroupNIds);
                }
                IdValue = string.Empty;
                IDataReader = DIConnection.ExecuteReader(sSql);
                while (IDataReader.Read())
                {
                    if (IdValue.Length > 0)
                    {
                        IdValue += Delimiter.TEXT_DELIMITER;
                    }

                    if (this.DataViewFilters.ShowSubgroupByIndicator)
                    {
                        IdValue += IDataReader[0].ToString();
                    }
                    else
                    {
                        IdValue += IDataReader[SubgroupVals.SubgroupValGId].ToString();
                    }
                }
                IDataReader.Close();
                this.DataViewFilters.DeletedSubgroupGIds = IdValue;
            }
            else
            {
                //-- Clear the GID, if NID is blank
                this._DataViewFilters.DeletedSubgroupGIds = string.Empty;
            }

            #endregion

            #region DataPoint Filter

            if (this.DataViewFilters.DeletedDataNIds.Trim().Length > 0)
            {
                StringBuilder DataPointGid = new StringBuilder();
                sSql = DIQueries.Data.GetDataViewDataByDataNIDs(this.DataViewFilters.DeletedDataNIds);
                IDataReader = DIConnection.ExecuteReader(sSql);
                while (IDataReader.Read())
                {
                    if (DataPointGid.Length > 0)
                    {
                        DataPointGid.Append(Delimiter.TEXT_DELIMITER);
                    }
                    DataPointGid.Append(IDataReader[Indicator.IndicatorGId] + Delimiter.TEXT_SEPARATOR + IDataReader[Unit.UnitGId] + Delimiter.TEXT_SEPARATOR);
                    DataPointGid.Append(IDataReader[SubgroupVals.SubgroupValGId] + Delimiter.TEXT_SEPARATOR + IDataReader[Timeperiods.TimePeriod] + Delimiter.TEXT_SEPARATOR);
                    DataPointGid.Append(IDataReader[Area.AreaID] + Delimiter.TEXT_SEPARATOR + IDataReader[IndicatorClassifications.ICName]);
                }
                this.DataViewFilters.DeletedDataGIds = DataPointGid.ToString();
                IDataReader.Close();
            }
            else
            {
                //-- Clear the GID, if NID is blank
                this._DataViewFilters.DeletedDataGIds = string.Empty;
            }

            #endregion

            #region IUS Filter

            if (this.DataViewFilters.IndicatorDataValueFilters.Count > 0)
            {
                for (int i = 0; i < this.DataViewFilters.IndicatorDataValueFilters.Count; i++)
                {
                    if (this.DataViewFilters.IndicatorDataValueFilters.ShowIUS)
                    {
                        sSql = DIQueries.IUS.GetIUSGIds(this.DataViewFilters.IndicatorDataValueFilters[i].IndicatorNId.ToString(), DIConnection.ConnectionStringParameters.ServerType);
                    }
                    else
                    {
                        sSql = DIQueries.Indicators.GetIndicator(FilterFieldType.NId, this.DataViewFilters.IndicatorDataValueFilters[i].IndicatorNId.ToString(), FieldSelection.Light);
                    }
                    IDataReader = DIConnection.ExecuteReader(sSql.ToString());
                    if (IDataReader.Read())
                    {
                        if (this.DataViewFilters.IndicatorDataValueFilters.ShowIUS)
                        {
                            this.DataViewFilters.IndicatorDataValueFilters[i].IndicatorGId = IDataReader[0].ToString();
                        }
                        else
                        {
                            this.DataViewFilters.IndicatorDataValueFilters[i].IndicatorGId = IDataReader[Indicator.IndicatorGId].ToString();
                        }
                    }
                    IDataReader.Close();
                }
            }

            #endregion

        }

        /// <summary>
        /// Update / Save NIds on the basis of GIds
        /// </summary>
        /// <param name="DIConnection"></param>
        /// <param name="DIQueries"></param>
        public void UpdateNIdsFromGIds(DIConnection DIConnection, DIQueries DIQueries)
        {
            IDataReader IDataReader;

            StringBuilder IDValues = new StringBuilder();
            string sSql = string.Empty;
            string[] ColumnField = new string[0];
            StringBuilder Values = new StringBuilder();

            #region Indicator

            if (this._IndicatorGIds.Length > 0)
            {
                string[] IndicatorValues = DIQueries.GetSplittedValues(this._IndicatorGIds, Delimiter.TEXT_DELIMITER);
                StringBuilder IndicatorText = new StringBuilder();
                for (int i = 0; i < IndicatorValues.Length; i++)
                {
                    if (i > 0)
                    {
                        IndicatorText.Append(Delimiter.NUMERIC_DELIMITER);
                    }
                    IndicatorText.Append("'" + IndicatorValues[i] + "'");
                }

                if (this._ShowIUS)
                {
                    sSql = DIQueries.IUS.GetIUSNIds(IndicatorText.ToString(), DIConnection.ConnectionStringParameters.ServerType);
                }
                else
                {
                    sSql = DIQueries.Indicators.GetIndicator(FilterFieldType.GId, IndicatorText.ToString(), FieldSelection.NId);
                }

                IDataReader = DIConnection.ExecuteReader(sSql);
                while (IDataReader.Read())
                {
                    if (IDValues.Length > 0)
                    {
                        IDValues.Append(Delimiter.NUMERIC_DELIMITER);
                    }

                    if (this._ShowIUS == true)
                    {
                        IDValues.Append(IDataReader[Indicator_Unit_Subgroup.IUSNId]);
                    }
                    else
                    {
                        IDValues.Append(IDataReader[Indicator.IndicatorNId]);
                    }
                }
                IDataReader.Close();
                this._IndicatorNIds = IDValues.ToString();
            }
            else
            {
                //-- Clear the NId, if GId is blank
                this._IndicatorNIds = string.Empty;
            }

            #endregion

            #region Unit

            if (this._UnitGIds.Length > 0)
            {
                sSql = DIQueries.Unit.GetUnit(FilterFieldType.GId, this._UnitGIds);
                IDataReader = DIConnection.ExecuteReader(sSql);
                IDValues.Length = 0;

                while (IDataReader.Read())
                {
                    if (IDValues.Length > 0)
                    {
                        IDValues.Append(Delimiter.NUMERIC_DELIMITER);
                    }
                    IDValues.Append(IDataReader[Unit.UnitNId]);
                }
                IDataReader.Close();
                this._UnitNIds = IDValues.ToString();
            }
            else
            {
                //-- Clear the NId, if GId is blank
                this._UnitNIds = string.Empty;
            }

            #endregion

            #region SubgroupVal

            if (this._SubgroupValGIds.Length > 0)
            {
                sSql = DIQueries.Subgroup.GetSubgroupVals(FilterFieldType.GId, this._SubgroupValGIds);
                IDataReader = DIConnection.ExecuteReader(sSql);
                IDValues.Length = 0;

                while (IDataReader.Read())
                {
                    if (IDValues.Length > 0)
                    {
                        IDValues.Append(Delimiter.NUMERIC_DELIMITER);
                    }
                    IDValues.Append(IDataReader[SubgroupVals.SubgroupValNId]);

                }
                IDataReader.Close();
                this._SubgroupValNIds = IDValues.ToString();
            }
            else
            {
                //-- Clear the NId, if GId is blank
                this._SubgroupValNIds = string.Empty;
            }

            #endregion

            #region Source

            if (this._SourceNames.Length > 0)
            {
                Values.Length = 0;  //Clear Stringbuilder
                ColumnField = DIQueries.GetSplittedValues(this._SourceNames, Delimiter.TEXT_DELIMITER);
                for (int i = 0; i < ColumnField.Length; i++)
                {
                    if (i > 0)
                    {
                        Values.Append(Delimiter.NUMERIC_DELIMITER);
                    }
                    Values.Append("'" + ColumnField[i].Replace("'","''") + "'");
                }
                sSql = DIQueries.Source.GetSource(FilterFieldType.Name, Values.ToString(), FieldSelection.Light, false);
                IDataReader = DIConnection.ExecuteReader(sSql);
                IDValues.Length = 0;

                while (IDataReader.Read())
                {
                    if (IDValues.Length > 0)
                    {
                        IDValues.Append(Delimiter.NUMERIC_DELIMITER);
                    }
                    IDValues.Append(IDataReader[IndicatorClassifications.ICNId]);
                }
                IDataReader.Close();
                this._SourceNIds = IDValues.ToString();
            }
            else
            {
                //-- Clear the NId, if GId is blank
                this._SourceNIds = string.Empty;
            }

            #endregion

            #region TimePeriod

            if (this._TimePeriods.Length > 0)
            {
                Values.Length = 0; //Clear Stringbuilder
                ColumnField = DIQueries.GetSplittedValues(this._TimePeriods, Delimiter.TEXT_DELIMITER);
                for (int i = 0; i < ColumnField.Length; i++)
                {
                    if (i > 0)
                    {
                        Values.Append(Delimiter.NUMERIC_DELIMITER);
                    }
                    Values.Append("'" + ColumnField[i].Replace("'", "''") + "'");
                }
                sSql = DIQueries.Timeperiod.GetTimePeriod(FilterFieldType.Name, Values.ToString());
                IDataReader = DIConnection.ExecuteReader(sSql);
                IDValues.Length = 0;
                while (IDataReader.Read())
                {
                    if (IDValues.Length > 0)
                    {
                        IDValues.Append(Delimiter.NUMERIC_DELIMITER);
                    }
                    IDValues.Append(IDataReader[Timeperiods.TimePeriodNId]);
                }
                IDataReader.Close();
                this._TimePeriodNIds = IDValues.ToString();
            }
            else
            {
                //-- Clear the NId, if GId is blank
                this._TimePeriodNIds = string.Empty;
            }

            #endregion

            #region Area NId

            if (this._AreaIds.Length > 0)
            {
                Values.Length = 0;
                ColumnField = DIQueries.GetSplittedValues(this._AreaIds, Delimiter.TEXT_DELIMITER);
                for (int i = 0; i < ColumnField.Length; i++)
                {
                    if (i > 0)
                    {
                        Values.Append(Delimiter.NUMERIC_DELIMITER);
                    }
                    Values.Append("'" + ColumnField[i].Replace("'", "''") + "'");
                }
                sSql = DIQueries.Area.GetArea(FilterFieldType.ID, Values.ToString());
                IDataReader = DIConnection.ExecuteReader(sSql);
                IDValues.Length = 0;
                while (IDataReader.Read())
                {
                    if (IDValues.Length > 0)
                    {
                        IDValues.Append(Delimiter.NUMERIC_DELIMITER);
                    }
                    IDValues.Append(IDataReader[Area.AreaNId]);
                }
                this._AreaNIds = IDValues.ToString();
                IDataReader.Close();
            }
            else
            {
                //-- Clear the NId, if GId is blank
                this._AreaNIds = string.Empty;
            }

            #endregion

            #region Source Filter

            if (this._DataViewFilters.DeletedSourceNames.Length > 0 && this.DataViewFilters.ShowSourceByIUS)
            {

                string[] SourceGIds = DIQueries.GetSplittedValues(this.DataViewFilters.DeletedSourceNames, Delimiter.TEXT_DELIMITER);
                StringBuilder SourceText = new StringBuilder();
                for (int i = 0; i < SourceGIds.Length; i++)
                {
                    if (i > 0)
                    {
                        SourceText.Append(Delimiter.NUMERIC_DELIMITER);
                    }
                    SourceText.Append("'" + SourceGIds[i].Replace("'", "''") + "'");
                }

                //if (this.DataViewFilters.ShowSourceByIUS)
                //{
                    sSql = DIQueries.Source.GetIUSNId_SourceNIds(SourceText.ToString(), DIConnection.ConnectionStringParameters.ServerType);
                //}
                //else
                //{
                //    sSql = DIQueries.IndicatorClassification.GetIC(FilterFieldType.Name, SourceText.ToString(), ICType.Source, FieldSelection.NId);
                //}
                IDataReader = DIConnection.ExecuteReader(sSql);

                IDValues.Length = 0;
                while (IDataReader.Read())
                {
                    if (IDValues.Length > 0)
                    {
                        IDValues.Append(Delimiter.NUMERIC_DELIMITER);
                    }

                    if (this.DataViewFilters.ShowSourceByIUS)
                    {
                        IDValues.Append("'" + IDataReader[0] + "'");   // Expression column with concatinated IUSNId_ICNId
                    }
                    else
                    {
                        IDValues.Append(IDataReader[IndicatorClassifications.ICNId]);
                    }

                }
                this.DataViewFilters.DeletedSourceNIds = IDValues.ToString();
                IDataReader.Close();
            }
            else if (this._DataViewFilters.DeletedSourceNIds.Length == 0)
            {
                //-- Clear the NId, if GId is blank
                this._DataViewFilters.DeletedSourceNIds = string.Empty;
            }

            #endregion

            #region Unit Filter

            if (this._DataViewFilters.DeletedUnitGIds.Length > 0)
            {

                string[] UnitGIds = DIQueries.GetSplittedValues(this.DataViewFilters.DeletedUnitGIds, Delimiter.TEXT_DELIMITER);
                StringBuilder UnitText = new StringBuilder();
                for (int i = 0; i < UnitGIds.Length; i++)
                {
                    if (i > 0)
                    {
                        UnitText.Append(Delimiter.NUMERIC_DELIMITER);
                    }
                    UnitText.Append("'" + UnitGIds[i] + "'");
                }

                if (this.DataViewFilters.ShowUnitByIndicator)
                {
                    sSql = DIQueries.Unit.GetIndicatorNId_UnitNIds(UnitText.ToString(), DIConnection.ConnectionStringParameters.ServerType);
                }
                else
                {
                    sSql = DIQueries.Unit.GetUnit(FilterFieldType.GId, UnitText.ToString());
                }

                IDValues.Length = 0;
                IDataReader = DIConnection.ExecuteReader(sSql);
                while (IDataReader.Read())
                {
                    if (IDValues.Length > 0)
                    {
                        IDValues.Append(Delimiter.NUMERIC_DELIMITER);
                    }

                    if (this.DataViewFilters.ShowUnitByIndicator)
                    {
                        IDValues.Append("'" + IDataReader[0] + "'");    // Expression column with concatinated IndicatorNId_UnitNId
                    }
                    else
                    {
                        IDValues.Append(IDataReader[Unit.UnitNId]);
                    }
                }
                IDataReader.Close();
                this.DataViewFilters.DeletedUnitNIds = IDValues.ToString();
            }
            else
            {
                //-- Clear the NId, if GId is blank
                this._DataViewFilters.DeletedUnitNIds = string.Empty;
            }

            #endregion

            #region Subgroup Filter

            if (this.DataViewFilters.DeletedSubgroupGIds.Length > 0)
            {
                string[] SubgroupGIds = DIQueries.GetSplittedValues(this.DataViewFilters.DeletedSubgroupGIds, Delimiter.TEXT_DELIMITER);
                StringBuilder SubgroupText = new StringBuilder();
                for (int i = 0; i < SubgroupGIds.Length; i++)
                {
                    if (i > 0)
                    {
                        SubgroupText.Append(Delimiter.NUMERIC_DELIMITER);
                    }
                    SubgroupText.Append("'" + SubgroupGIds[i] + "'");
                }

                if (this.DataViewFilters.ShowSubgroupByIndicator)
                {

                    sSql = DIQueries.Subgroup.GetIndicatorNId_SubgroupNIds(SubgroupText.ToString(), DIConnection.ConnectionStringParameters.ServerType);
                }
                else
                {
                    sSql = DIQueries.Subgroup.GetSubgroupVals(FilterFieldType.GId, SubgroupText.ToString());
                }
                IDValues.Length = 0;
                IDataReader = DIConnection.ExecuteReader(sSql);
                while (IDataReader.Read())
                {
                    if (IDValues.Length > 0)
                    {
                        IDValues.Append(Delimiter.NUMERIC_DELIMITER);
                    }

                    if (this.DataViewFilters.ShowSubgroupByIndicator)
                    {
                        IDValues.Append("'" + IDataReader[0] + "'");    // Expression column with concatinated IndicatorNId_SubgroupvalNId
                    }
                    else
                    {
                        IDValues.Append(IDataReader[SubgroupVals.SubgroupValNId]);
                    }
                }
                IDataReader.Close();
                this.DataViewFilters.DeletedSubgroupNIds = IDValues.ToString();
            }
            else
            {
                //-- Clear the NId, if GId is blank
                this._DataViewFilters.DeletedSubgroupNIds = string.Empty;
            }

            #endregion

            #region DataPoint Filter

            if (this.DataViewFilters.DeletedDataGIds.Length > 0)
            {
                string[] DeletedDataGIds = DIQueries.GetSplittedValues(this.DataViewFilters.DeletedDataGIds, Delimiter.TEXT_DELIMITER);
                StringBuilder DataGIds = new StringBuilder();
                for (int i = 0; i < DeletedDataGIds.Length; i++)
                {
                    if (i > 0)
                    {
                        DataGIds.Append(Delimiter.NUMERIC_DELIMITER);
                    }
                    DataGIds.Append("'" + DeletedDataGIds[i] + "'");
                }


                sSql = DIQueries.Data.GetDataPointNIds(DataGIds.ToString(), DIConnection.ConnectionStringParameters.ServerType);

                IDValues.Length = 0;
                IDataReader = DIConnection.ExecuteReader(sSql);
                while (IDataReader.Read())
                {
                    if (IDValues.Length > 0)
                    {
                        IDValues.Append(Delimiter.NUMERIC_DELIMITER);
                    }

                    IDValues.Append("" + IDataReader[Data.DataNId] + "");
                }
                IDataReader.Close();
                this.DataViewFilters.DeletedDataNIds = IDValues.ToString();
            }
            else
            {
                //-- Clear the NId, if GId is blank
                this._DataViewFilters.DeletedDataNIds = string.Empty;
            }

            #endregion

            #region IUS Filter

            if (this.DataViewFilters.IndicatorDataValueFilters.Count > 0)
            {
                string InvalidRow = string.Empty;
                int InvalidRowIndex = 0;
                for (int i = 0; i < this.DataViewFilters.IndicatorDataValueFilters.Count; i++)
                {
                    if (this.DataViewFilters.IndicatorDataValueFilters.ShowIUS)
                    {
                        sSql = DIQueries.IUS.GetIUSNIds("'" + this.DataViewFilters.IndicatorDataValueFilters[i].IndicatorGId + "'", DIConnection.ConnectionStringParameters.ServerType);
                    }
                    else
                    {
                        sSql = DIQueries.Indicators.GetIndicator(FilterFieldType.GId, "'" + this.DataViewFilters.IndicatorDataValueFilters[i].IndicatorGId + "'", FieldSelection.Light);
                    }
                    IDataReader = DIConnection.ExecuteReader(sSql.ToString());
                    if (IDataReader.Read())
                    {
                        if (this.DataViewFilters.IndicatorDataValueFilters.ShowIUS)
                        {
                            this.DataViewFilters.IndicatorDataValueFilters[i].IndicatorNId = (int)IDataReader[Indicator_Unit_Subgroup.IUSNId];
                        }
                        else
                        {
                            this.DataViewFilters.IndicatorDataValueFilters[i].IndicatorNId = (int)IDataReader[Indicator.IndicatorNId];
                        }
                    }
                    else
                    {
                        if (InvalidRow.Length > 0)
                        {
                            InvalidRow += ",";
                        }
                        InvalidRowIndex += 1;
                        //-- Insert the index of invalid rows.
                        InvalidRow += i.ToString();
                    }
                    IDataReader.Close();
                }
                //-- delete the Invalid rows from the collection.
                string[] Row = new string[0];
                Row = DIQueries.GetSplittedValues(InvalidRow, ",");
                for (int i = 0; i < InvalidRowIndex; i++)
                {
                    Row[i] = (Convert.ToInt32(Row[i]) - i).ToString();
                    this.DataViewFilters.IndicatorDataValueFilters.RemoveAt(Convert.ToInt32(Row[i]));
                }
            }

            #endregion

        }

        #endregion

        #region " -- Comparison -- "

        /// <summary>
        /// Check for existence of nids in the in the updated user selection. If I,A,T is missing in the new updated user selection, then "no record found" msg will appears.
        /// </summary>
        /// <param name="oldUserSelection"></param>
        /// <param name="updatedUserSelection"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool CompareIndicatorTimePeriodArea(UserSelection oldUserSelection, UserSelection updatedUserSelection)
        {
            bool Retval = false;
            try
            {
                //-- Indicator check
                if (!string.IsNullOrEmpty(oldUserSelection.IndicatorNIds) && string.IsNullOrEmpty(updatedUserSelection.IndicatorNIds))
                {
                    Retval = true;
                }

                //-- Area check
                if (!Retval && (!string.IsNullOrEmpty(oldUserSelection.AreaNIds) && string.IsNullOrEmpty(updatedUserSelection.AreaNIds)))
                {
                    Retval = true;
                }

                //-- Time period. check
                if (!Retval && (!string.IsNullOrEmpty(oldUserSelection.TimePeriodNIds) && string.IsNullOrEmpty(updatedUserSelection.TimePeriodNIds)))
                {
                    Retval = true;
                }
            }
            catch (Exception ex)
            {
            }
            return Retval;
        }


        #endregion

        #region " -- Events -- "

        /// <summary>
        /// Event for change in user selection.
        /// </summary>
        public event UserSelectionChangeDelegate UserSelectionChangeEvent;

        #endregion

        #region " -- Raise Event -- "

        /// <summary>
        /// Raise the event for change in user selection.
        /// </summary>
        public void RaiseUserSelectionChangedEvent()
        {
            if (UserSelectionChangeEvent != null)
            {
                this.UserSelectionChangeEvent();
            }
        }


        #endregion

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            object RetVal = null;
            try
            {
                XmlSerializer XmlSerializer = new XmlSerializer(typeof(UserSelection));
                MemoryStream MemoryStream = new MemoryStream();
                XmlSerializer.Serialize(MemoryStream, this);
                MemoryStream.Position = 0;
                RetVal = (UserSelection)XmlSerializer.Deserialize(MemoryStream);
                MemoryStream.Close();
                MemoryStream.Dispose();
            }
            catch (Exception)
            {
                RetVal = null;
            }
            return (UserSelection)RetVal;
        }

        #endregion

    }
}
