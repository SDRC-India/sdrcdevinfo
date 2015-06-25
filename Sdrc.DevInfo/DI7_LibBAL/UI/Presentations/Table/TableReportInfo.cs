using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DevInfo.Lib.DI_LibBAL.Utility;

using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.UserSelection;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Common;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Table
{
    public class TableReportInfo
    {        
        #region " -- Public -- "

        #region " -- New / Dispose -- "

        public TableReportInfo()
        {
            //-- Object of area selection
            this._AreaSelection = new AreaInfo();

            //-- Object of TP selection
            this._TimePeriodSelection = new TimePeriodInfo();

            //-- Object of source selection
            this._SourceSelection = new SourceInfo();
        }

        #endregion

        #region " -- Properties -- "

        private string _ReportFileName = string.Empty;
        /// <summary>
        /// Name of the report file with extension (.xml)
        /// </summary>
        public string ReportFileName
        {
            get { return _ReportFileName; }
            set { _ReportFileName = value; }
        }

        private string _ReportDescription = string.Empty;
        /// <summary>
        /// Metadat for report
        /// </summary>
        public string ReportDescription
        {
            get { return _ReportDescription; }
            set { _ReportDescription = value; }
        }

        private string _ReportCategory = string.Empty;
        /// <summary>
        /// Gets or sets the report category
        /// </summary>
        public string ReportCategory
        {
            get 
            {
                return this._ReportCategory; 
            }
            set 
            {
                this._ReportCategory = value; 
            }
        }
	

        private AreaLevelFormats _LevelFormat = new AreaLevelFormats();
        /// <summary>
        /// Gets or sets the area level format.
        /// </summary>
        public AreaLevelFormats LevelFormat
        {
            get 
            { 
                return this._LevelFormat; 
            }
            set 
            {
                this._LevelFormat = value; 
            }
        }

        private AreaInfo _AreaSelection = null;
        /// <summary>
        /// Gets or sets the area info
        /// </summary>
        public AreaInfo AreaSelection
        {
            get
            {
                return _AreaSelection;
            }
            set
            {
                _AreaSelection = value;
            }
        }

        private SourceInfo _SourceSelection = null;
        /// <summary>
        /// Gets or sets the source info
        /// </summary>
        public SourceInfo SourceSelection
        {
            get
            {
               
                return this._SourceSelection;
            }
            set
            {
                this._SourceSelection = value;
            }
        }

        private TimePeriodInfo _TimePeriodSelection = null;
        /// <summary>
        /// Gets or sets the time period info
        /// </summary>
        public TimePeriodInfo TimePeriodSelection
        {
            get
            {
                return this._TimePeriodSelection;
            }
            set
            {
                this._TimePeriodSelection = value;
            }
        }

        #endregion    
        
        #endregion

        #region " -- Inner class -- "

        #region " -- Area Selections -- "

        //TODO None case handling
        //TODO preserving font information in style template
        public class AreaInfo
        {
            #region " -- Public -- "

            #region " -- New / Dispose -- "

            public AreaInfo()
            {
            }

            #endregion

            #region " -- Properties -- "

            private int _PrimaryAreaLevel = -1;
            /// <summary>
            /// Gets or sets the primary arealevel
            /// </summary>
            public int PrimaryAreaLevel
            {
                get { return _PrimaryAreaLevel; }
                set { _PrimaryAreaLevel = value; }
            }

            private string _SecondryAreaLevels = string.Empty;
            /// <summary>
            /// Gets or sets the comma seprated secondary arealevel
            /// </summary>
            public string SecondryAreaLevels
            {
                get
                {
                    return this._SecondryAreaLevels;
                }
                set
                {
                    _SecondryAreaLevels = value;
                }
            }

            private string _AreaLevels = string.Empty;
            /// <summary>
            /// Get the area levels
            /// </summary>
            public string AreaLevels
            {
                get 
                {
                    return this._AreaLevels; 
                }
                set 
                {
                    this._AreaLevels = value; 
                }
            }
	

            #endregion

            #region " -- Methods -- "

            /// <summary> 
            /// Get all the AreaNIds of the selected level and update the user selection. 
            /// </summary> 
            /// <param name="selectedAreaNIds"></param> 
            /// <param name="levels"></param> 
            /// <remarks></remarks> 
            public void GetSubNationals(DIConnection dbConnection, DIQueries dbQueries, UserSelection dbUserSelection, string selectedAreaNIds, string levels)
            {
                try
                {
                    if (!string.IsNullOrEmpty(this._SecondryAreaLevels))
                    {
                        StringBuilder sbArea = new StringBuilder();
                        string[] SelectedLevel = new string[0];
                        string sLevels = string.Empty;
                        DataTable Areadt = new DataTable();
                        DataRow[] Rows = null;
                        string SelectedAreaNId = string.Empty;
                        Areas = new Dictionary<int, string>();

                        foreach (Int32 Level in GetAreaLevel(dbConnection, dbQueries))
                        {
                            IDataReader AreaReader;
                            //'-- Get Area NIDs of the levles 
                            AreaReader = dbConnection.ExecuteReader(dbQueries.Area.GetAreaNIdByAreaLevel(dbUserSelection.AreaNIds, Level));

                            while (AreaReader.Read())
                            {
                                sbArea.Append("," + AreaReader[Area.AreaNId].ToString());
                            }
                            //'-- Add the selected AreaNIDs according to their level 
                            if (sbArea.Length > 0)
                            {
                                Areas.Add(Level, sbArea.ToString().Substring(1));
                            }
                            AreaReader.Close();
                            sbArea.Length = 0;
                        }

                        //'-- Get the Levels.
                        sLevels = this.GetParentLevel(this._SecondryAreaLevels, dbConnection, dbQueries);
                        this._AreaLevels = sLevels;

                        SelectedLevel = DICommon.SplitString(sLevels, ",");
                        //'-- Insert all the required level to build the comma seprated AreaNIds of selected levels 
                        for (int Index = GetMaxKey(Areas); Index <= GetMinSelectedLevel(SelectedLevel) - 1; Index++)
                        {
                            Array.Resize(ref SelectedLevel, SelectedLevel.Length + 1);
                            SelectedLevel[SelectedLevel.Length - 1] = Index.ToString();
                        }
                        //'-- Sort on the all the required levels 
                        SelectedLevel = Sort(SelectedLevel);

                        //'-- Comma seprated required levels. 
                        sLevels = string.Empty;
                        foreach (string NewLevel in SelectedLevel)
                        {
                            sLevels += "," + NewLevel;
                        }
                        if (!string.IsNullOrEmpty(sLevels))
                        {
                            sLevels = sLevels.Substring(1);
                        }


                        if (!string.IsNullOrEmpty(this._SecondryAreaLevels))
                        {

                            //'-- Get all the areas of the required levels 

                            Areadt = dbConnection.ExecuteDataTable(dbQueries.Area.GetAreasByAreaLevels(sLevels));

                            foreach (string sLevel in SelectedLevel)
                            {
                                sbArea.Length = 0;
                                if (string.IsNullOrEmpty(GetSelectedAreaNId(Areas, Convert.ToInt32(sLevel))))
                                {
                                    //'-- Get the saved AreaNIds 
                                    if (Convert.ToInt32(sLevel) > 1)
                                    {
                                        SelectedAreaNId = GetSelectedAreaNId(Areas, Convert.ToInt32(sLevel) - 1);
                                    }

                                    //'-- Get the AreaNIds on the basis of selected areas and level 
                                    if (!string.IsNullOrEmpty(SelectedAreaNId))
                                    {
                                        Rows = Areadt.Select(Area.AreaLevel + " = " + Convert.ToInt32(sLevel) + " AND " + Area.AreaParentNId + " IN (" + SelectedAreaNId + ")");
                                    }
                                    else
                                    {
                                        Rows = Areadt.Select(Area.AreaLevel + " = " + Convert.ToInt32(sLevel));
                                    }

                                    //'-- Add the AreaNIds in the collection. 
                                    foreach (DataRow Row in Rows)
                                    {
                                        sbArea.Append("," + Row[Area.AreaNId].ToString());
                                    }
                                    if (sbArea.Length > 0)
                                    {
                                        Areas.Add(Convert.ToInt32(sLevel), sbArea.ToString().Substring(1));
                                    }
                                }
                            }

                            SelectedLevel = DICommon.SplitString(this._SecondryAreaLevels, ",");

                            //'-- Update user selection with AreaNIds 
                            dbUserSelection.AreaNIds = string.Empty;
                            dbUserSelection.AreaIds = string.Empty;
                            foreach (string selLevel in SelectedLevel)
                            {
                                SelectedAreaNId = GetSelectedAreaNId(Areas, Convert.ToInt32(selLevel));
                                if (!string.IsNullOrEmpty(SelectedAreaNId))
                                {
                                    dbUserSelection.AreaNIds += "," + SelectedAreaNId;
                                }
                            }

                            if (!string.IsNullOrEmpty(dbUserSelection.AreaNIds))
                            {
                                dbUserSelection.AreaNIds = dbUserSelection.AreaNIds.Substring(1);
                            }

                            Areadt = dbConnection.ExecuteDataTable(dbQueries.Area.GetArea(FilterFieldType.NId, dbUserSelection.AreaNIds, Lib.DI_LibDAL.Queries.Area.Select.OrderBy.AreaName));

                            foreach (DataRow Row in Areadt.Rows)
                            {
                                if (string.IsNullOrEmpty(dbUserSelection.AreaIds))
                                {
                                    dbUserSelection.AreaIds = Row[Area.AreaID].ToString();
                                }
                                else
                                {
                                    dbUserSelection.AreaIds += Delimiter.TEXT_DELIMITER + Row[Area.AreaID].ToString();
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }

            #endregion

            #endregion

            #region " -- Private -- "

            #region " -- Variable -- "

            private Dictionary<Int32, string> Areas = new Dictionary<int, string>();

            #endregion

            #region " -- Methods -- "

            /// <summary> 
            /// Get the AreaNIds of the selected level, if exists 
            /// </summary> 
            /// <param name="selectedAreas"></param> 
            /// <param name="level"></param> 
            /// <returns></returns> 
            /// <remarks></remarks> 
            private string GetSelectedAreaNId(Dictionary<Int32, string> selectedAreas, Int32 level)
            {
                string Retval = string.Empty;
                try
                {
                    foreach (KeyValuePair<Int32, string> AreaByLevel in selectedAreas)
                    {
                        if (AreaByLevel.Key == level)
                        {
                            Retval = AreaByLevel.Value;
                            break; // TODO: might not be correct. Was : Exit For 
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                return Retval;
            }

            /// <summary> 
            /// Get all the area levels 
            /// </summary> 
            /// <returns></returns> 
            /// <remarks></remarks> 
            private List<Int32> GetAreaLevel(DIConnection dbConnection, DIQueries dbQueries)
            {
                List<Int32> Retval = new List<Int32>();
                IDataReader LevelReader = null;
                try
                {

                    LevelReader = dbConnection.ExecuteReader(dbQueries.Area.GetAreaLevel(Lib.DI_LibDAL.Queries.FilterFieldType.None, ""));

                    while (LevelReader.Read())
                    {
                        Retval.Add(Convert.ToInt32(LevelReader[Area_Level.AreaLevel]));
                    }
                }

                catch (Exception ex)
                {
                }
                finally
                {
                    LevelReader.Close();
                }
                return Retval;
            }

            /// <summary> 
            /// Get the max level stoded in collection 
            /// </summary> 
            /// <param name="selectedAreas"></param> 
            /// <returns></returns> 
            /// <remarks></remarks> 
            private int GetMaxKey(Dictionary<Int32, string> selectedAreas)
            {
                int Retval = 0;
                try
                {
                    foreach (KeyValuePair<Int32, string> AreaByLevel in selectedAreas)
                    {
                        if (AreaByLevel.Key > Retval)
                        {
                            Retval = AreaByLevel.Key;
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                return Retval;
            }

            /// <summary> 
            /// Get the min level selected by the user 
            /// </summary> 
            /// <param name="selectedLevels"></param> 
            /// <returns></returns> 
            /// <remarks></remarks> 
            private int GetMinSelectedLevel(string[] selectedLevels)
            {
                int Retval = 10;
                try
                {
                    foreach (string Level in selectedLevels)
                    {
                        if (Convert.ToInt32(Level) < Retval)
                        {
                            Retval = Convert.ToInt32(Level);
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                return Retval;

            }

            /// <summary> 
            /// Sort on the selected levels 
            /// </summary> 
            /// <param name="selectedLevels"></param> 
            /// <returns></returns> 
            /// <remarks></remarks> 
            private string[] Sort(string[] selectedLevels)
            {
                try
                {
                    string Temp = string.Empty;
                    for (int i = 0; i <= selectedLevels.Length - 1; i++)
                    {
                        for (int j = i + 1; j <= selectedLevels.Length - 1; j++)
                        {
                            if (Convert.ToInt32(selectedLevels[i]) > Convert.ToInt32(selectedLevels[j]))
                            {
                                Temp = selectedLevels[i];
                                selectedLevels[i] = selectedLevels[j];
                                selectedLevels[j] = Temp;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
                return selectedLevels;
            }

            /// <summary>
            /// Get the parent levels of selected level
            /// </summary>
            /// <param name="levels"></param>
            /// <param name="dbConnection"></param>
            /// <param name="dbQueries"></param>
            /// <returns></returns>
            private string GetParentLevel(string levels,DIConnection dbConnection, DIQueries dbQueries)
            {
                string RetVal = string.Empty;
                try
                {
                    int MaxLevel = -1;
                    IDataReader LevelReader;
                    string[] Levels = new string[0];
                    Levels = DICommon.SplitString(levels, ",");

                    //-- Get the maximum level from the selected level.
                    MaxLevel = Convert.ToInt32(Levels[0]);
                    foreach (string SecondaryLevel in Levels)
                    {
                        if (Convert.ToInt32(MaxLevel) < Convert.ToInt32(SecondaryLevel))
                        {
                            MaxLevel = Convert.ToInt32(SecondaryLevel);
                        }
                    }
                    RetVal=MaxLevel.ToString();

                    //-- Get the parent levels.
                    LevelReader = dbConnection.ExecuteReader(dbQueries.Area.GetAreaLevel());
                    while (LevelReader.Read())
                    {
                        if (MaxLevel > Convert.ToInt32(LevelReader[Area_Level.AreaLevel]))
                        {                          
                            RetVal += "," + LevelReader[Area_Level.AreaLevel].ToString();
                        }
                    }
                    LevelReader.Close();
                }
                catch (Exception)
                {
                }
                return RetVal;
            }

            #endregion

            #endregion
        }

        #endregion

        #region " -- Timeperiod Selections -- "

        /// <summary>
        /// User shal opt for any one of the three mutually exclusive options
        /// 1. Use Dataview
        /// 2. MRD
        /// 3. From - To
        /// Additionally user may set timeperiods for exclusion
        /// </summary>
        public class TimePeriodInfo
        {
            #region " -- Public -- "

            #region " -- New / Dispose -- "

            public TimePeriodInfo()
            {
            }

            #endregion

            #region " -- Properties -- "
            
            private bool _MRD = false;
            /// <summary>
            /// Gets or sets the MRD Info
            /// </summary>
            public bool MRD
            {
                get
                {
                    return this._MRD;
                }
                set
                {
                    this._MRD = value;
                }
            }


            private string _FromTimePeriod = string.Empty;
            /// <summary>
            /// Gets or sets the from time period
            /// </summary>
            public string FromTimePeriod
            {
                get
                {
                    return this._FromTimePeriod;
                }
                set
                {
                    this._FromTimePeriod = value;
                }
            }

            private string _ToTimePeriod = string.Empty;
            /// <summary>
            /// Gets or sets the to time period
            /// </summary>
            public string ToTimePeriod
            {
                get
                {
                    return this._ToTimePeriod;
                }
                set
                {
                    this._ToTimePeriod = value;
                }
            }

            private bool _IsToMRD = false;
            /// <summary>
            /// Gets or sets the TOMRD
            /// </summary>
            public bool IsToMRD
            {
                get
                {
                    return this._IsToMRD;
                }
                set
                {
                    this._IsToMRD = value;
                }
            }

            private string _ExcludeTimePeriods = string.Empty;
            /// <summary>
            /// Gets or sets the comma seprated excluded time periods.
            /// </summary>
            public string ExcludeTimePeriods
            {
                get
                {
                    return this._ExcludeTimePeriods;
                }
                set
                {
                    this._ExcludeTimePeriods = value;
                }
            }

            #endregion

            #region " -- Methods -- "

            public string UpdateTimePeriods(DIConnection dbConnection, DIQueries dbQueries, UserSelection dbUserSelection)
            {
                string RetVal = string.Empty;
                try
                {
                    IDataReader TimeReader = null;

                    //'-- Set the MRD filter to true 
                    if (this._IsToMRD | this._MRD)
                    {
                        dbUserSelection.DataViewFilters.MostRecentData = true;
                    }
                    else
                    {
                        dbUserSelection.DataViewFilters.MostRecentData = false;
                    }

                    if (this._IsToMRD || !string.IsNullOrEmpty(this._ToTimePeriod) || string.IsNullOrEmpty(dbUserSelection.TimePeriodNIds.Trim()))
                    {

                        if (this._IsToMRD)
                        {
                            //'-- Get all the time period greater then TO time period 
                            TimeReader = dbConnection.ExecuteReader(dbQueries.Timeperiod.GetAutoSelectedTimePeriodsRange(this._FromTimePeriod, "", dbUserSelection));
                        }
                        else if (!string.IsNullOrEmpty(this._ToTimePeriod))
                        {
                            //'-- Get auto selected time period between from and two 
                            TimeReader = dbConnection.ExecuteReader(dbQueries.Timeperiod.GetAutoSelectedTimePeriodsRange(this._FromTimePeriod, this._ToTimePeriod, dbUserSelection));
                        }
                        else if (string.IsNullOrEmpty(dbUserSelection.TimePeriodNIds.Trim()))
                        {
                            //-- Get auto selected time periods
                            TimeReader = dbConnection.ExecuteReader(dbQueries.Timeperiod.GetAutoSelectByIndicatorAreaSource(string.Empty, dbUserSelection.AreaNIds, dbUserSelection.SourceNIds, dbUserSelection.IndicatorNIds));
                        }

                        while (TimeReader.Read())
                        {
                            RetVal += "," + TimeReader[Timeperiods.TimePeriodNId];
                        }

                        if (!string.IsNullOrEmpty(RetVal))
                        {
                            RetVal = RetVal.Substring(1);
                        }
                        else
                        {
                            RetVal = "-1";
                        }

                        TimeReader.Close();
                    }
                    //-- Exclude the TimePeriod from the selections.
                    //this.ExcludeTimePeriodsFromUserSelection(dbConnection, dbQueries, dbUserSelection);
                }
                catch (Exception)
                {
                }
                return RetVal;
            }

            /// <summary>
            /// Exclude the time periods from the selections
            /// </summary>
            /// <param name="dbConnection"></param>
            /// <param name="dbQueries"></param>
            /// <param name="dbUserSelection"></param>
            public void ExcludeTimePeriodsFromUserSelection(DIConnection dbConnection, DIQueries dbQueries, UserSelection dbUserSelection)
            {

                if (!string.IsNullOrEmpty(this._ExcludeTimePeriods))
                {

                    string[] ExcludedTimePeriods = new string[0];
                    string[] SelectedTimePeriods = new string[0];
                    string[] AutoSelectedTimePeriods = new string[0];
                    bool NoRecord = true;
                    bool RefillWithAutoSelection = false;
                    IDataReader TimeReader;

                    //'-- Create the array of excluded and selected TP 
                    ExcludedTimePeriods = DICommon.SplitString(this._ExcludeTimePeriods, ",");
                    SelectedTimePeriods = DICommon.SplitString(dbUserSelection.TimePeriods, Delimiter.TEXT_DELIMITER);
                    AutoSelectedTimePeriods = this.AutoSelectedTimePeriods(dbConnection, dbQueries, dbUserSelection);

                    for (int ExcludedIndex = 0; ExcludedIndex <= ExcludedTimePeriods.Length - 1; ExcludedIndex++)
                    {

                        for (int SelectedIndex = 0; SelectedIndex <= SelectedTimePeriods.Length - 1; SelectedIndex++)
                        {
                            if (ExcludedTimePeriods[ExcludedIndex] == SelectedTimePeriods[SelectedIndex])
                            {
                                SelectedTimePeriods[SelectedIndex] = string.Empty;
                                break;
                            }
                        }
                        //-- Remove the excluded time period from the auto selected time period array.
                        for (int AutoSelectedIndex = 0; AutoSelectedIndex <= AutoSelectedTimePeriods.Length - 1; AutoSelectedIndex++)
                        {
                            if (ExcludedTimePeriods[ExcludedIndex] == AutoSelectedTimePeriods[AutoSelectedIndex])
                            {
                                AutoSelectedTimePeriods[AutoSelectedIndex] = string.Empty;
                                break;
                            }
                        }
                    }


                    //'-- Update the table user selection 
                    dbUserSelection.TimePeriodNIds = string.Empty;
                    dbUserSelection.TimePeriods = string.Empty;

                    for (int SelectedIndex = 0; SelectedIndex <= SelectedTimePeriods.Length - 1; SelectedIndex++)
                    {
                        if (!string.IsNullOrEmpty(SelectedTimePeriods[SelectedIndex]))
                        {
                            if (string.IsNullOrEmpty(dbUserSelection.TimePeriods))
                            {
                                dbUserSelection.TimePeriods = SelectedTimePeriods[SelectedIndex];
                            }
                            else
                            {
                                dbUserSelection.TimePeriods += Delimiter.TEXT_DELIMITER + SelectedTimePeriods[SelectedIndex];
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(dbUserSelection.TimePeriodNIds.Trim()))
                    {
                        RefillWithAutoSelection = true;
                    }

                    for (int AutoSelectedIndex = 0; AutoSelectedIndex <= AutoSelectedTimePeriods.Length - 1; AutoSelectedIndex++)
                    {
                        if (!string.IsNullOrEmpty(AutoSelectedTimePeriods[AutoSelectedIndex]))
                        {
                            NoRecord = false;
                            //-- Fill time period NIds with auto selected nids
                            if (RefillWithAutoSelection)
                            {
                                if (string.IsNullOrEmpty(dbUserSelection.TimePeriods))
                                {
                                    dbUserSelection.TimePeriods = AutoSelectedTimePeriods[AutoSelectedIndex];
                                }
                                else
                                {
                                    dbUserSelection.TimePeriods += Delimiter.TEXT_DELIMITER + AutoSelectedTimePeriods[AutoSelectedIndex];
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    if (NoRecord)
                    {
                        dbUserSelection.TimePeriodNIds = "-1";
                        dbUserSelection.TimePeriods = string.Empty;
                    }
                }
            } 

            #endregion

            #endregion

            #region " -- Private -- "

            #region " -- Methods -- "

            private string[] AutoSelectedTimePeriods(DIConnection dbConnection, DIQueries dbQueries, UserSelection dbUserSelection)
            {
                string[] Retval = new string[0];
                try
                {
                    string TimePeriods = string.Empty;
                    IDataReader TimePeriodReader;
                    if (string.IsNullOrEmpty(this._FromTimePeriod))
                    {
                        TimePeriodReader = dbConnection.ExecuteReader(dbQueries.Timeperiod.GetAutoSelectTimeperiod(dbUserSelection.IndicatorNIds, dbUserSelection.ShowIUS, dbUserSelection.AreaNIds, dbUserSelection.SourceNIds));
                    }
                    else
                    {
                        if (this._IsToMRD)
                        {
                            TimePeriodReader = dbConnection.ExecuteReader(dbQueries.Timeperiod.GetAutoSelectedTimePeriodsRange(this._FromTimePeriod, string.Empty, dbUserSelection));
                        }
                        else
                        {
                            TimePeriodReader = dbConnection.ExecuteReader(dbQueries.Timeperiod.GetAutoSelectedTimePeriodsRange(this._FromTimePeriod, this._ToTimePeriod, dbUserSelection));
                        }                        
                    }

                    while (TimePeriodReader.Read())
                    {
                        TimePeriods += "," + TimePeriodReader[Timeperiods.TimePeriod].ToString();
                    }
                    TimePeriodReader.Close();

                    if (!string.IsNullOrEmpty(TimePeriods))
                    {
                        TimePeriods = TimePeriods.Substring(1);
                    }
                    Retval = DICommon.SplitString(TimePeriods, ",");                    
                }
                catch (Exception)
                {
                }
                return Retval;
            }

            #endregion

            #endregion

        }

        #endregion

        #region " -- Source Selections -- "

        public class SourceInfo
        {
            #region " -- New / Dispose -- "

            public SourceInfo()
            {
            }

            #endregion

            #region " -- Properties -- "

       
            private bool _UseRecommendedSources = false;
            /// <summary>
            /// Gets or sets the option to use recommended sources
            /// </summary>
            public bool UseRecommendedSources
            {
                get
                {
                    return this._UseRecommendedSources;
                }
                set
                {
                    this._UseRecommendedSources = value;
                }
            }

            #endregion
        }

        #endregion

        #endregion      

    }
}
