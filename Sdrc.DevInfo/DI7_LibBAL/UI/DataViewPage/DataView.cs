using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Text;
using System.IO;
using System.Xml;
using System.Data;
using DevInfo.Lib.DI_LibDAL;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.UserSelection;
using DevInfo.Lib.DI_LibBAL.UI.UserPreference;
using DevInfo.Lib.DI_LibBAL.Utility;
using System.Text.RegularExpressions;
using DevInfo.Lib.DI_LibBAL.Controls.TimeperiodBAL;


//note that a DataTable object can't be linked to more than one DataSet object at a time. Take precautions while creating too many relationships
namespace DevInfo.Lib.DI_LibBAL.UI.DataViewPage
{

    #region " -- Delegates --"


    /// <summary>
    /// Declare the delegate for progree bar
    /// </summary>
    /// <param name="ParentProgress"></param>
    /// <param name="ChildProgress"></param>
    public delegate void PagingProgressDelegate(int PageExecuted, int TotalPages);

    #endregion

    /// <summary>
    /// DIDataView class is basically used to generate dataview based on user selection
    /// It utilizes threading for generation paged data and building MainDataTable
    /// </summary>
    [Serializable()]
    public class DIDataView : ILanguage
    {

        #region Private

        #region Variables


        // All DataNIds based on Indicator, TimePeriod and Area Selection
        private string msAllDataNIDs = string.Empty;

        // MRD Data NIDs
        private string msMRDDataNIDs = string.Empty;

        //IUS SourceNId combination used in GetIUSSource
        private DataTable mdtIUSSourceNIDs = GetInvariantDataTable();

        private DataTable mdtRecSourceLabel = GetInvariantDataTable();

        // Array of bool to keep track of pages for which data has been generated
        private bool[] mbPageDataStatus = new bool[1];


        // ------------ usage in ApplyFilters function
        // ApplyFilters can work on Partially retrieved DataView or the Fully retrieved DataView
        // ...Partial DataView - Filter applied as a usage of Query
        // ...Fully Generated View - Filter applied as a usage of RowFilter of DataView class
        // ------------
        // Once the Filter has been applied on the Partial DataView, the control should not go to FULLY GENERATED DATAVIEW unless the DataView is reloaded.
        // Scenario: If the filtered record count results in one page of data, then the control will go to the FULLY GENERATED DATAVIEW (IsDataViewFullyGenerated will return true)
        // In such a scenario, when the user will restore the filters, wrong dataview might be sent back to the user.
        private bool mbFilterAppliedByQuery = false;


        //Private variable to store UserPReference and connection details. Set while a call to GetDataView is made and then deferred usage in GetIUSSource
        private UserPreference.UserPreference mUserPreference;
        private DIQueries mdIQueries = null;
        private DIConnection mdIConnection = null;
        private DataView PresentationData = null;
        private DataView MRDData = null;

        // Metadata Information
        private string msMaskFolder = string.Empty;

        private string msTotalRecordsLanguageString = string.Empty;
        private string msFilterOnTranslation = string.Empty;
        private string msPAGETranslation = string.Empty;
        private string msOFTranslation = string.Empty;


        // BackgroundWorker to fetch page records when page count > 1

        //Threading
        // Worker Thread for building data page by page in background
        Thread moWorkerThread;

        // Volatile is used as hint to the compiler that this data member will be accessed by multiple threads.
        private volatile bool mbCancellationPending;



        #endregion

        #region Methods

        #region Initialization
        /// <summary>
        /// Reset all internal datatables as well as those exposed through properties when ever GetDataView function is called
        /// </summary>
        /// <remarks>Normally GetDataView fuction shall be called whenever there are changes in userselection</remarks>
        private void InitializeVariables()
        {
            // Main Data Table
            this._MainDataTable.Rows.Clear();

            // DataNIds
            this._AllDataNIDs.Rows.Clear();

            // SourceNIDs
            this.mdtIUSSourceNIDs.Rows.Clear();
            // Indicator Master
            this._IUSIndicator.Rows.Clear();

            // IUSUnit
            this._IUSUnit.Rows.Clear();
            // IndicatorUnit
            this._IndicatorUnit.Rows.Clear();
            // Unit Master
            this._Units.Rows.Clear();

            // IUSSubgroupVal
            this._IUSSubgroupVal.Rows.Clear();
            // IndicatorSubgroupVal
            this._IndicatorSubgroupVal.Rows.Clear();
            // SubgroupVals Master
            this._SubgroupVals.Rows.Clear();
            // Subgroup Info
            this._SubgroupInfo.Rows.Clear();
            this._SubgroupInfoCheckedAndNotFound = false;

            // IUS Source 
            this._IUSSource.Rows.Clear();
            // Source Master
            this._Sources.Rows.Clear();
            this.mdtRecSourceLabel.Rows.Clear();

            this._MetadataIndicator.Rows.Clear();
            this._MetadataArea.Rows.Clear();
            this._MetadataSource.Rows.Clear();
            this._IUSICInfo.Rows.Clear();
            this._IUSICNIdInfo.Rows.Clear();

            this._RecordCount = 0;
            this._PageCount = 0;

            // Setting the Table Names
            this._MainDataTable.TableName = "DataView_MainDataTable";

            this._AllDataNIDs.TableName = "DataView_UserSelection_AllDataNIDs";
            this.mdtIUSSourceNIDs.TableName = "DataView_UserSelection_IUSSourceNIDs";

            this._IUSIndicator.TableName = "DataView_UserSelection_IUSIndicator";

            this._IUSUnit.TableName = "DataView_UserSelection_IUSUnit";
            this._IndicatorUnit.TableName = "DataView_UserSelection_IndicatorUnit";
            this._Units.TableName = "DataView_UserSelection_Units";

            this._IUSSubgroupVal.TableName = "DataView_UserSelection_IUSSubgroupVal";
            this._IndicatorSubgroupVal.TableName = "DataView_UserSelection_IndicatorSubgroupVal";
            this._SubgroupVals.TableName = "DataView_UserSelection_SubgroupVals";
            this._SubgroupInfo.TableName = "DataView_UserSelection_SubgroupInfo";

            this._IUSSource.TableName = "DataView_UserSelection_IUSSource";
            this._Sources.TableName = "DataView_UserSelection_Sources";
            this.mdtRecSourceLabel.TableName = "DataView_UserSelection_RecSourceLabel";

            this._MetadataIndicator.TableName = "DataView_UserSelection_MetadataIndicator";
            this._MetadataArea.TableName = "DataView_UserSelection_MetadataArea";
            this._MetadataSource.TableName = "DataView_UserSelection_MetadataSource";
            this._IUSICInfo.TableName = "DataView_UserSelection_IUSICInfo";
            this._IUSICNIdInfo.TableName = "DataView_UserSelection_IUSICNIdInfo";

            this.msMRDDataNIDs = string.Empty;
            this.mbFilterAppliedByQuery = false;
        }

        /// <summary>
        /// Get a culture neutral data table with its locale set to invariant culture to handle cases for different regional settings
        /// </summary>
        /// <returns></returns>
        private static DataTable GetInvariantDataTable()
        {
            // 
            DataTable RetVal = new DataTable();
            RetVal.Locale = new System.Globalization.CultureInfo("", false);
            return RetVal;
        }

        /// <summary>
        /// Get a culture neutral data set with its locale set to invariant culture to handle cases for different regional settings
        /// </summary>
        /// <returns></returns>
        private static DataSet GetInvariantDataSet()
        {
            // Set locale of dataset to invariant culture to handle cases for different regional settings
            DataSet RetVal = new DataSet();
            RetVal.Locale = new System.Globalization.CultureInfo("", false);
            return RetVal;
        }

        /// <summary>
        /// Data table returned from DIConnection have locale set to current thread.
        /// This funtions modifies the datatabe locale to invariant culture to handle cases for different regional settings
        /// </summary>
        /// <param name="sqlQuery"></param>
        /// <returns></returns>
        private DataTable ExecuteInvariantDataTable(string sqlQuery)
        {
            DataTable RetVal = this.mdIConnection.ExecuteDataTable(sqlQuery);
            RetVal.Locale = new System.Globalization.CultureInfo("", false);
            return RetVal;
        }

        /// <summary>
        /// Replaces blank space, "," and "'" by "_"
        /// </summary>
        /// <param name="columnName">Original column name</param>
        /// <returns></returns>
        private string GetValidColumnName(string columnName)
        {
            string RetVal = columnName;
            RetVal = RetVal.Replace(" ", "_");
            RetVal = RetVal.Replace(",", "_");
            RetVal = RetVal.Replace("'", "_");
            //For Web--This value will be represented as column name, so to Display it's name , remove #
            RetVal = RetVal.Replace("#", "");
            return RetVal;
        }

        #endregion

        #region Build Master Tables
        /// <summary>
        /// Build data table storing IUSNId and Indicator fields
        /// </summary>
        /// <param name="DIConnection"></param>
        /// <param name="DIQueries"></param>
        /// <returns></returns>
        private bool BuildIUSIndicator(DI_LibDAL.Connection.DIConnection DIConnection, DI_LibDAL.Queries.DIQueries DIQueries)
        {
            bool RetVal = true;
            // If the Indicator Master Table has not been built then build it
            if (this._IUSIndicator.Rows.Count == 0)
            {
                string _Query = string.Empty;
                if (this._IUSNIds.Length > 0)
                {
                    _Query = DIQueries.Indicators.GetIndicators(this._IUSNIds, false);
                    this._IUSIndicator = this.ExecuteInvariantDataTable(_Query);
                    this._IUSIndicator.TableName = "DataView_UserSelection_IUSIndicator";
                }
                RetVal = true;
            }
            else
            {
                RetVal = true;
            }

            return RetVal;
        }

        /// <summary>
        /// Build data table storing IUSNId and Unit fields
        /// </summary>
        /// <param name="DIConnection"></param>
        /// <param name="DIQueries"></param>
        /// <returns></returns>
        private bool BuildIUSUnit(DI_LibDAL.Connection.DIConnection DIConnection, DI_LibDAL.Queries.DIQueries DIQueries)
        {
            bool RetVal = true;
            // If the Unit Master Table has not been built then build it
            if (this._IUSUnit.Rows.Count == 0)
            {
                string _Query = string.Empty;
                if (this._IUSNIds.Length > 0)
                {
                    _Query = DIQueries.Unit.GetUnits(this._IUSNIds);
                    this._IUSUnit = this.ExecuteInvariantDataTable(_Query);
                    // ExecuteInvariantDataTable sets table name to "". Set Table name which is used while defining relationship
                    this._IUSUnit.TableName = "DataView_UserSelection_IUSUnit";

                }
                RetVal = true;
            }
            else
            {
                RetVal = true;
            }

            return RetVal;
        }

        /// <summary>
        /// Build data table storing IUSNId and SubgroupVal fields
        /// </summary>
        /// <param name="DIConnection"></param>
        /// <param name="DIQueries"></param>
        /// <returns></returns>
        private bool BuildIUSSubgroup(DI_LibDAL.Connection.DIConnection DIConnection, DI_LibDAL.Queries.DIQueries DIQueries)
        {
            bool RetVal = true;
            // If the Unit Master Table has not been built then build it
            if (this._IUSSubgroupVal.Rows.Count == 0)
            {
                string _Query = string.Empty;
                if (this._IUSNIds.Length > 0)
                {
                    _Query = DIQueries.Subgroup.GetSubgroups(this._IUSNIds);
                    this._IUSSubgroupVal = this.ExecuteInvariantDataTable(_Query);
                    this._IUSSubgroupVal.TableName = "DataView_UserSelection_IUSSubgroupVal";
                }
                RetVal = true;
            }
            else
            {
                RetVal = true;
            }

            return RetVal;
        }

        /// <summary>
        /// Build data table storing IUSNId and Source fields
        /// </summary>
        /// <param name="DIConnection"></param>
        /// <param name="DIQueries"></param>
        /// <returns></returns>
        private bool BuildSource(DI_LibDAL.Connection.DIConnection DIConnection, DI_LibDAL.Queries.DIQueries DIQueries)
        {
            bool RetVal = true;
            // If the Source Master Table has not been built then build it
            if (this._Sources.Rows.Count == 0)
            {
                try
                {
                    string _Query = string.Empty;
                    if (this.mdtIUSSourceNIDs.Rows.Count > 0)
                    {
                        string[] SourceDistinctColumns = new string[1];
                        SourceDistinctColumns[0] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.SourceNId;
                        DataTable tmpSourceTable = this.mdtIUSSourceNIDs.DefaultView.ToTable(true, SourceDistinctColumns);
                        string sSources = string.Empty;
                        StringBuilder sbSources = new StringBuilder();
                        for (int i = 0; i < tmpSourceTable.Rows.Count; i++)
                        {
                            sbSources.Append("," + tmpSourceTable.Rows[i][Data.SourceNId]);
                        }
                        sSources = sbSources.ToString().Substring(1);

                        _Query = DIQueries.Source.GetSource(DevInfo.Lib.DI_LibDAL.Queries.FilterFieldType.NId, sSources, DevInfo.Lib.DI_LibDAL.Queries.FieldSelection.Light, false);
                        this._Sources = this.ExecuteInvariantDataTable(_Query);
                        this._Sources.TableName = "DataView_UserSelection_Sources";

                        // Add Selected column
                        this._Sources.Columns.Add(DataExpressionColumns.Selected, typeof(bool));
                        this.ClearSourceFilter(); // Set selected column value to true
                    }
                    RetVal = true;
                }
                catch (Exception)
                {
                    RetVal = false;
                    throw;
                }
            }
            else
            {
                RetVal = true;
            }

            return RetVal;
        }

        /// <summary>
        /// Get dataview  containing  all columns related to Source, IUS, Indicator, Unit, SubgroupVal and Recommended source information
        /// </summary>
        /// <param name="DIConnection"></param>
        /// <param name="DIQueries"></param>
        /// <returns></returns>
        private DataView GetIUSSource(DI_LibDAL.Connection.DIConnection DIConnection, DI_LibDAL.Queries.DIQueries DIQueries)
        {

            // If _IUSSource is precomputed, need not to recompute
            if (this._IUSSource.Rows.Count > 0)
            {

            }
            else
            {
                string IUSSourceFilterString = string.Empty;
                string _Query = string.Empty;

                // Indicator            	            Unit           Subgroup  	    Source                  RecSrc      Selected
                // ----------------------------------------------------------------------------------------------------------------------
                // Annual GDP growth rate	            Percent	        Total	        World Bank_WDI 2006     True        True
                // Average annual rate of inflation	    Percent	        Total	        World Bank_WDI 2006     False       True

                // We Have the following
                // 1. _Indicators Data Table (All fields of Indicator by IUS)
                // 2. _Units Data Table (All fields of Unit by IUS)
                // 3. _Subgroups Data Table (All fields of Subgroups by IUS)
                // 4. mdtIUSSourceNIDs Data Table (IUSNId and Source NId)


                // STEP 1: Start building dtIUSSource on the basis of the dtIUSSourceNIDs data table
                DataTable dtIUSSource = null;
                dtIUSSource = this.mdtIUSSourceNIDs.Copy();
                // Set column name to ICNId instead of SourceNId
                dtIUSSource.Columns[Data.SourceNId].ColumnName = IndicatorClassifications.ICNId;
                // Add RecommendedSource column
                dtIUSSource.Columns.Add(IndicatorClassificationsIUS.RecommendedSource, typeof(bool));
                dtIUSSource.Columns.Add(IndicatorClassificationsIUS.ICIUSOrder, typeof(int));
                dtIUSSource.Columns.Add(IndicatorClassificationsIUS.ICIUSLabel, typeof(string));

                // Add Selected column
                dtIUSSource.Columns.Add(DataExpressionColumns.Selected, typeof(bool));

                //---------------------------New Code Starts ------------------------------------------------
                // Check for existanace of recommended source 
                // Fetch all records from IndicatorClassificationsIUS table where RecommendedSource = true
                StringBuilder sbIUSSourceFilter = new StringBuilder();

                foreach (DataRow dr in dtIUSSource.Rows)
                {
                    sbIUSSourceFilter.Append(",'" + dr[Indicator_Unit_Subgroup.IUSNId] + "_" + dr[IndicatorClassifications.ICNId] + "'");
                }

                if (sbIUSSourceFilter.Length > 0)
                {

                    _Query = DIQueries.Source.GetSource_Rec(sbIUSSourceFilter.ToString().Substring(1), mdIConnection.ConnectionStringParameters.ServerType);
                    DataView dtRecSource = this.ExecuteInvariantDataTable(_Query).DefaultView;

                    //DataTable 
                    // Set recommended source value
                    if (dtRecSource != null && dtRecSource.Count > 0)
                    {

                        // If recommended source exists then set RecommendedSource value based on dtRecSource
                        for (int i = 0; i < dtIUSSource.Rows.Count; i++)
                        {
                            dtRecSource.RowFilter = IndicatorClassificationsIUS.ICNId + "=" + dtIUSSource.Rows[i][IndicatorClassificationsIUS.ICNId] + " AND " + IndicatorClassificationsIUS.IUSNId + "=" + dtIUSSource.Rows[i][IndicatorClassificationsIUS.IUSNId];
                            if (dtRecSource.Count > 0)
                            {
                                dtIUSSource.Rows[i][IndicatorClassificationsIUS.RecommendedSource] = dtRecSource[0][IndicatorClassificationsIUS.RecommendedSource];
                                dtIUSSource.Rows[i][IndicatorClassificationsIUS.ICIUSOrder] = dtRecSource[0][IndicatorClassificationsIUS.ICIUSOrder];
                                dtIUSSource.Rows[i][IndicatorClassificationsIUS.ICIUSLabel] = dtRecSource[0][IndicatorClassificationsIUS.ICIUSLabel];
                            }
                            else
                            {
                                dtIUSSource.Rows[i][IndicatorClassificationsIUS.RecommendedSource] = false;
                            }
                            dtIUSSource.Rows[i][DataExpressionColumns.Selected] = true;
                        }

                    }
                    else
                    {
                        // If no recommended source exists then set RecommendedSource value to false for all rows
                        for (int i = 0; i < dtIUSSource.Rows.Count; i++)
                        {
                            dtIUSSource.Rows[i][IndicatorClassificationsIUS.RecommendedSource] = false;
                            dtIUSSource.Rows[i][DataExpressionColumns.Selected] = true;
                        }
                    }
                }
                else
                {
                    // If no recommended source exists then set RecommendedSource value to false for all rows
                    for (int i = 0; i < dtIUSSource.Rows.Count; i++)
                    {
                        dtIUSSource.Rows[i][IndicatorClassificationsIUS.RecommendedSource] = false;
                        dtIUSSource.Rows[i][DataExpressionColumns.Selected] = true;
                    }
                }

                if (dtIUSSource.Rows.Count > 0)
                {
                    try
                    {

                        // STEP 2: We need to now build up the following IUS_Source Data Table IUS_Source with following columns
                        // ICNId (SourceNId) : IUSNId : Indicator Name : Indicator Global 
                        // Unit Name : Unit Global : Subgroup Name : Subgroup Global 
                        // Source Name : Source Global : Recommended Source
                        dtIUSSource.Columns.Add(Indicator.IndicatorName, typeof(string));
                        dtIUSSource.Columns.Add(Indicator.IndicatorGlobal, typeof(bool));
                        dtIUSSource.Columns.Add(Unit.UnitName, typeof(string));
                        dtIUSSource.Columns.Add(Unit.UnitGlobal, typeof(bool));
                        dtIUSSource.Columns.Add(DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupVal, typeof(string));
                        dtIUSSource.Columns.Add(DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGlobal, typeof(bool));
                        dtIUSSource.Columns.Add(IndicatorClassifications.ICName, typeof(string));
                        dtIUSSource.Columns.Add(IndicatorClassifications.ICGlobal, typeof(bool));

                        // STEP 3: Use ADO.NET Relations to build up the Data Table for dtIUSSource
                        DataSet _Base = GetInvariantDataSet();
                        // Add dtIUSSource Datatable into the base dataset
                        _Base.Tables.Add(dtIUSSource);

                        // STEP 3.1 - Add Indicator Name into dtIUSSource table using
                        // 1. dtIUSSource
                        // 2. _Indicators
                        // Add _Indicators Datatable into the base dataset
                        _Base.Tables.Add(this._IUSIndicator);
                        _Base.Relations.Add("RelIUS_For_Indicator", this._IUSIndicator.Columns[Indicator_Unit_Subgroup.IUSNId], dtIUSSource.Columns[Indicator_Unit_Subgroup.IUSNId], false);
                        // add expression on Indicator Column
                        dtIUSSource.Columns[Indicator.IndicatorName].Expression = "parent(RelIUS_For_Indicator)." + Indicator.IndicatorName;
                        dtIUSSource.Columns[Indicator.IndicatorGlobal].Expression = "parent(RelIUS_For_Indicator)." + Indicator.IndicatorGlobal;
                        _Base.AcceptChanges();

                        // STEP 3.2 - Add Unit Name into dtIUSSource table using
                        // 1. dtIUSSource
                        // 2. _Units
                        // Add _Units Datatable into the base dataset
                        _Base.Tables.Add(this._IUSUnit);
                        _Base.Relations.Add("RelIUS_For_Unit", this._IUSUnit.Columns[Indicator_Unit_Subgroup.IUSNId], dtIUSSource.Columns[Indicator_Unit_Subgroup.IUSNId], false);
                        // add expression on Unit Column
                        dtIUSSource.Columns[Unit.UnitName].Expression = "parent(RelIUS_For_Unit)." + Unit.UnitName;
                        dtIUSSource.Columns[Unit.UnitGlobal].Expression = "parent(RelIUS_For_Unit)." + Unit.UnitGlobal;
                        _Base.AcceptChanges();

                        // STEP 3.3 - Add Subgroup Name into dtIUSSource table using
                        // 1. dtIUSSource
                        // 2. _Subgroups
                        // Add _Subgroups Datatable into the base dataset
                        _Base.Tables.Add(this._IUSSubgroupVal);
                        _Base.Relations.Add("RelIUS_For_Subgroup", this._IUSSubgroupVal.Columns[Indicator_Unit_Subgroup.IUSNId], dtIUSSource.Columns[Indicator_Unit_Subgroup.IUSNId], false);
                        // add expression on Unit Column
                        dtIUSSource.Columns[DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupVal].Expression = "parent(RelIUS_For_Subgroup)." + DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupVal;
                        dtIUSSource.Columns[DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGlobal].Expression = "parent(RelIUS_For_Subgroup)." + DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGlobal;
                        _Base.AcceptChanges();

                        // STEP 3.4 - Add Source Name into dtIUSSource table using
                        // 1. dtIUSSource
                        // 2. _Sources
                        // Add _Sources Datatable into the base dataset
                        _Base.Tables.Add(this._Sources);
                        _Base.Relations.Add("RelIUS_For_Source", this._Sources.Columns[IndicatorClassificationsIUS.ICNId], dtIUSSource.Columns[IndicatorClassificationsIUS.ICNId], false);
                        // add expression on Unit Column
                        dtIUSSource.Columns[IndicatorClassifications.ICName].Expression = "parent(RelIUS_For_Source)." + IndicatorClassifications.ICName;
                        dtIUSSource.Columns[IndicatorClassifications.ICGlobal].Expression = "parent(RelIUS_For_Source)." + IndicatorClassifications.ICGlobal;
                        _Base.AcceptChanges();

                        // Set Return DataView
                        this._IUSSource = dtIUSSource;
                    }
                    catch (Exception)
                    {
                        this._IUSSource = dtIUSSource;
                        throw;
                    }
                }
                else
                {
                    // -- Return empty DataView
                    this._IUSSource = dtIUSSource;
                }


            }


            return this._IUSSource.DefaultView;
        }

        #endregion

        #region Add Extra Columns to main DataTable

        /// <summary>
        /// Add Extra Columns in the Main Data Table
        /// </summary>
        /// <param name="dtDataTable"></param>
        private void AddExtraColumnsToDataTable(DataTable dtPageData)
        {
            //-- Add Subgroup Columns (AGE, SEX, LOCATION, OTHERS)
            foreach (DataRow dr in this.SubgroupInfo.Rows)
            {
                dtPageData.Columns.Add(dr[SubgroupInfoColumns.NId].ToString(), typeof(int));
                dtPageData.Columns.Add(dr[SubgroupInfoColumns.Name].ToString(), typeof(string));
                dtPageData.Columns.Add(dr[SubgroupInfoColumns.GId].ToString(), typeof(string));
                dtPageData.Columns.Add(dr[SubgroupInfoColumns.Global].ToString(), typeof(bool));
                dtPageData.Columns.Add(dr[SubgroupInfoColumns.OrderColName].ToString(), typeof(int));
            }

            // -- Add Filter Columns and set values through expression (IUS_Source, I_Unit, I_SubgroupVal)
            // IUSNId_Source_NId
            dtPageData.Columns.Add(Indicator_Unit_Subgroup.IUSNId + "_" + IndicatorClassifications.ICNId, typeof(string), Indicator_Unit_Subgroup.IUSNId + " + '_' + " + IndicatorClassifications.ICNId);
            // Indicator_Unit_NId
            dtPageData.Columns.Add(Indicator.IndicatorNId + "_" + Unit.UnitNId, typeof(string), Indicator.IndicatorNId + " + '_' + " + Unit.UnitNId);
            // Indicator_Subgroup_NId
            dtPageData.Columns.Add(Indicator.IndicatorNId + "_" + DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValNId, typeof(string), Indicator.IndicatorNId + " + '_' + " + DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValNId);

            //-- Add Rec Source Label Column
            dtPageData.Columns.Add(RecommendedSources.ICIUSLabel, typeof(string));

            //-- Add Selection Column for marking deleted records 
            dtPageData.Columns.Add(DataExpressionColumns.Selected, typeof(bool));
            dtPageData.Columns[DataExpressionColumns.Selected].DefaultValue = true;

            //-- Add DataExpression Columns for numeric data handling
            dtPageData.Columns.Add(DataExpressionColumns.DataType, typeof(int));
            dtPageData.Columns.Add(DataExpressionColumns.NumericData, typeof(decimal));
            dtPageData.Columns.Add(DataExpressionColumns.TextualData, typeof(string));

            dtPageData.AcceptChanges();

            //-- Add Metadata Columns
            this.AddMetadataColumnsToDataTable(dtPageData);

            //-- Add IC Columns
            this.AddIUSICInfoColumnsToDataTable(dtPageData);
        }

        /// <summary>
        /// Add Expression columns for sorting by end date
        /// </summary>
        /// <param name="dtDataTable"></param>
        private void AddStartDateEndDateColumnsToDataTable(DataTable dtDataTable)
        {
            try
            {
                //dtDataTable.Columns.Add(DataExpressionColumns.TimePeriodStartDate, typeof(string));
                dtDataTable.Columns.Add(DataExpressionColumns.TimePeriodEndDate, typeof(string));
                dtDataTable.AcceptChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Set Column Values for Extra Columns

        /// <summary>
        /// Set Subgroup values (AGE, SEX, LOCATION, OTHERS) to the main data table
        /// </summary>
        /// <param name="dtDataTable"></param>
        private void SetSubgroupValues(DataTable dtDataTable)
        {

            string sSql = string.Empty;
            string SubgroupValNIds = string.Empty;
            StringBuilder sbSubgroupValNIds = new StringBuilder();
            DataView dvSubgroupValSubgroup = null;
            foreach (DataRow dr in this.SubgroupVals.Rows)
            {
                sbSubgroupValNIds.Append("," + dr[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValNId].ToString());
            }

            if (sbSubgroupValNIds.Length > 0)
            {
                sSql = this.mdIQueries.SubgroupValSubgroup.GetSubgroupValsSubgroupWithType(sbSubgroupValNIds.ToString().Substring(1), string.Empty);
                dvSubgroupValSubgroup = this.ExecuteInvariantDataTable(sSql).DefaultView;
            }

            // -- Add Subgroup values (AGE, SEX, LOCATION, OTHERS) to the main data table
            // Create a Data Set to store dtDataTable and the Subgroup Data Table for creating the Subgroups
            DataSet dsParentChild = GetInvariantDataSet();

            // Add Main Data Table to Data Set for genereatuing the SUBGROUPS
            dsParentChild.Tables.Add(dtDataTable);
            dsParentChild.Tables[0].TableName = "Parent";

            DataColumn[] PK = new DataColumn[1];
            string ChildTableName = string.Empty;
            string RelationshipName = string.Empty;
            foreach (DataRow dr in this.SubgroupInfo.Rows)
            {
                DataTable dtSubgroup = null;
                dvSubgroupValSubgroup.RowFilter = Subgroup.SubgroupType + " = " + dr[SubgroupInfoColumns.Type];
                ChildTableName = "Child" + dr[SubgroupInfoColumns.Type].ToString();
                RelationshipName = "Rel" + dr[SubgroupInfoColumns.Type].ToString();
                dtSubgroup = dvSubgroupValSubgroup.ToTable(ChildTableName);

                // Set Primaty Key in the Subgroup Data Table
                PK[0] = dtSubgroup.Columns[SubgroupValsSubgroup.SubgroupValNId];
                dtSubgroup.PrimaryKey = PK;

                // Add Subgroup Data Table
                dsParentChild.Tables.Add(dtSubgroup); // Since would already be part if a relationship, therefore adding the COPY

                // Define Relationship
                dsParentChild.Relations.Add(RelationshipName, dsParentChild.Tables[ChildTableName].Columns[SubgroupValsSubgroup.SubgroupValNId], dsParentChild.Tables[0].Columns[DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValNId], false);
                dsParentChild.Tables[0].Columns[dr[0].ToString()].Expression = "Parent([" + RelationshipName + "])." + SubgroupValsSubgroup.SubgroupNId;
            }


            string DistinctColumnName = SubgroupValsSubgroup.SubgroupNId;

            // Get distinct subgroup nids
            dvSubgroupValSubgroup.RowFilter = "";
            DataTable DistinctSubgroupNIdTable = dvSubgroupValSubgroup.ToTable(true, DistinctColumnName);
            string[] DistinctSubgroupNIds = new string[DistinctSubgroupNIdTable.Rows.Count];
            for (int i = 0; i < DistinctSubgroupNIdTable.Rows.Count; i++)
            {
                DistinctSubgroupNIds[i] = DistinctSubgroupNIdTable.Rows[i][DistinctColumnName].ToString();
            }

            // Get Subgroup table 
            if (DistinctSubgroupNIds.Length > 0)
            {
                sSql = this.mdIQueries.Subgroup.GetSubgroup(FilterFieldType.NId, string.Join(",", DistinctSubgroupNIds));
                DataTable dtSubgroupMaster = this.ExecuteInvariantDataTable(sSql);
                dtSubgroupMaster.TableName = "SubgroupMaster";
                dsParentChild.Tables.Add(dtSubgroupMaster);
                foreach (DataRow dr in this.SubgroupInfo.Rows)
                {
                    RelationshipName = "Rel" + dr[SubgroupInfoColumns.Name].ToString();
                    // As Subgroup_Type_Name values in Subgroup_Type table may contain spaces remove spaces if any  
                    RelationshipName = RelationshipName.Replace(" ", "");
                    // Define Relationship
                    try
                    {
                        dsParentChild.Relations.Add(RelationshipName, dsParentChild.Tables[dtSubgroupMaster.TableName].Columns[Subgroup.SubgroupNId], dsParentChild.Tables[0].Columns[dr[SubgroupInfoColumns.NId].ToString()], false);
                        //-- use [] braces around realtionship name to avoid error due to apostrophe
                        dsParentChild.Tables[0].Columns[dr[SubgroupInfoColumns.Name].ToString()].Expression = "Parent([" + RelationshipName + "])." + Subgroup.SubgroupName;
                        dsParentChild.Tables[0].Columns[dr[SubgroupInfoColumns.GId].ToString()].Expression = "Parent([" + RelationshipName + "])." + Subgroup.SubgroupGId;
                        dsParentChild.Tables[0].Columns[dr[SubgroupInfoColumns.Global].ToString()].Expression = "Parent([" + RelationshipName + "])." + Subgroup.SubgroupGlobal;
                        dsParentChild.Tables[0].Columns[dr[SubgroupInfoColumns.OrderColName].ToString()].Expression = "Parent([" + RelationshipName + "])." + Subgroup.SubgroupOrder;
                    }
                    catch (Exception ex)
                    {

                        System.Diagnostics.Debug.Print(ex.Message);
                    }

                }

                dsParentChild.AcceptChanges();
            }


        }

        /// <summary>
        /// This is a variant of SetSubgroupValues which populates subgroup and other expression column avoiding dataset relationship
        /// Dataset realtionship causes issues in case of MySql
        /// </summary>
        /// <param name="dtDataTable"></param>
        /// <param name="pageDataNIds"></param>
        private void SetSubgroupValuesMySql(DataTable dtDataTable, string pageDataNIds)
        {
            string sSql = string.Empty;
            StringBuilder sbSubgroupValNIds = new StringBuilder();
            DataView dvSubgroupValSubgroup = null;
            DataTable dtSubgroupValSubgroup = null;

            foreach (DataRow dr in this.SubgroupVals.Rows)
            {
                sbSubgroupValNIds.Append("," + dr[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValNId].ToString());
            }

            if (sbSubgroupValNIds.Length > 0)
            {
                sSql = this.mdIQueries.SubgroupValSubgroup.GetSubgroupValsSubgroupWithType(sbSubgroupValNIds.ToString().Substring(1), string.Empty);
                dtSubgroupValSubgroup = this.ExecuteInvariantDataTable(sSql);
                dtSubgroupValSubgroup.Columns.Add(SubgroupInfoColumns.Name, typeof(string));
                dtSubgroupValSubgroup.Columns.Add(SubgroupInfoColumns.GId, typeof(string));
                dtSubgroupValSubgroup.Columns.Add(SubgroupInfoColumns.Global, typeof(bool));
                dtSubgroupValSubgroup.Columns.Add(SubgroupInfoColumns.Order, typeof(int));

                // Get distinct subgroup nids
                DataTable DistinctSubgroupNIdTable = dtSubgroupValSubgroup.DefaultView.ToTable(true, SubgroupValsSubgroup.SubgroupNId);
                string[] DistinctSubgroupNIds = new string[DistinctSubgroupNIdTable.Rows.Count];
                for (int i = 0; i < DistinctSubgroupNIdTable.Rows.Count; i++)
                {
                    DistinctSubgroupNIds[i] = DistinctSubgroupNIdTable.Rows[i][SubgroupValsSubgroup.SubgroupNId].ToString();
                }
                if (DistinctSubgroupNIds.Length > 0)
                {
                    sSql = this.mdIQueries.Subgroup.GetSubgroup(FilterFieldType.NId, string.Join(",", DistinctSubgroupNIds));
                    DataTable dtSubgroupMaster = this.ExecuteInvariantDataTable(sSql);
                    foreach (DataRow drSubgroupMaster in dtSubgroupMaster.Rows)
                    {
                        foreach (DataRow drSubgroupValSubgroup in dtSubgroupValSubgroup.Select(SubgroupValsSubgroup.SubgroupNId + " = " + drSubgroupMaster[Subgroup.SubgroupNId]))
                        {
                            drSubgroupValSubgroup[SubgroupInfoColumns.Name] = drSubgroupMaster[Subgroup.SubgroupName];
                            drSubgroupValSubgroup[SubgroupInfoColumns.GId] = drSubgroupMaster[Subgroup.SubgroupGId];
                            drSubgroupValSubgroup[SubgroupInfoColumns.Global] = drSubgroupMaster[Subgroup.SubgroupGlobal];
                            drSubgroupValSubgroup[SubgroupInfoColumns.Order] = drSubgroupMaster[Subgroup.SubgroupOrder];
                        }
                    }
                }
                dvSubgroupValSubgroup = dtSubgroupValSubgroup.DefaultView;
            }


            // Set locale of dataset to invariant culture to handle cases for different regional settings
            if (dtDataTable.DataSet != null)
            {
                //Changing locale of dataset automatically changes locale of all datatables it contains
                dtDataTable.DataSet.Locale = new System.Globalization.CultureInfo("", false);
            }
            else
            {
                dtDataTable.Locale = new System.Globalization.CultureInfo("", false);
            }
            System.Globalization.CultureInfo CurrentCultureInfo = System.Globalization.CultureInfo.CurrentCulture;



            // -- Add Subgroup values (AGE, SEX, LOCATION, OTHERS) to the main data table
            foreach (DataRow dr in this.SubgroupInfo.Rows)
            {
                dvSubgroupValSubgroup.RowFilter = Subgroup.SubgroupType + " = " + dr[SubgroupInfoColumns.Type];
                foreach (DataRowView drv in dvSubgroupValSubgroup)
                {
                    foreach (DataRow drDataTable in dtDataTable.Select(DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValNId + " = " + drv[SubgroupValsSubgroup.SubgroupValNId]))
                    {
                        //-- Set Subgroup Info
                        drDataTable[dr[SubgroupInfoColumns.NId].ToString()] = drv[SubgroupValsSubgroup.SubgroupNId];
                        drDataTable[dr[SubgroupInfoColumns.Name].ToString()] = drv[SubgroupInfoColumns.Name];
                        drDataTable[dr[SubgroupInfoColumns.GId].ToString()] = drv[SubgroupInfoColumns.GId];
                        drDataTable[dr[SubgroupInfoColumns.Global].ToString()] = drv[SubgroupInfoColumns.Global];
                        drDataTable[dr[SubgroupInfoColumns.OrderColName].ToString()] = drv[SubgroupInfoColumns.Order];

                        //-- Set DataType and NumericData 
                        if (Utility.DICommon.IsNumeric(drDataTable[Data.DataValue].ToString()) == true)
                        {
                            try
                            {
                                drDataTable[DataExpressionColumns.DataType] = DataType.Numeric;
                                drDataTable[DataExpressionColumns.NumericData] = drDataTable[Data.DataValue];
                                //-- Based on fact that NumberDecimalSeparator stored inside database is always "."
                                drDataTable[Data.DataValue] = drDataTable[Data.DataValue].ToString().Replace(".", CurrentCultureInfo.NumberFormat.NumberDecimalSeparator);
                            }
                            catch (Exception)
                            {
                                //http://www.blackwasp.co.uk/CSharpStringToNumeric.aspx
                                //-- TODO values like "(234)" are considered numeric but give exception "Couldn't store in NumericData Column.  Expected type is Decimal."
                                drDataTable[DataExpressionColumns.DataType] = DataType.Text;
                                drDataTable[DataExpressionColumns.TextualData] = drDataTable[Data.DataValue];
                            }
                        }
                        else
                        {
                            drDataTable[DataExpressionColumns.DataType] = DataType.Text;
                            drDataTable[DataExpressionColumns.TextualData] = drDataTable[Data.DataValue];
                        }

                        //-- Set selection value to true for all records by default
                        drDataTable[DataExpressionColumns.Selected] = true;
                    }
                }
            }


            //-- Set selection value to false for all records marked for deletion
            if (this.mUserPreference.UserSelection.DataViewFilters.DeletedDataNIds.Length > 0)
            {
                foreach (DataRow dr in dtDataTable.Select(Data.DataNId + " IN (" + this.mUserPreference.UserSelection.DataViewFilters.DeletedDataNIds + ")"))
                {
                    dr[DataExpressionColumns.Selected] = false;
                }
            }

            //-- Set Rec Source Labels
            string _Query = this.mdIQueries.RecommendedSources.GetRecommendedSources(pageDataNIds);
            DataView dvRecSourceLabel = this.ExecuteInvariantDataTable(_Query).DefaultView;
            foreach (DataRowView drv in dvRecSourceLabel)
            {
                foreach (DataRow drDataTable in dtDataTable.Select(Data.DataNId + " = " + drv[RecommendedSources.DataNId]))
                {
                    drDataTable[RecommendedSources.ICIUSLabel] = drv[RecommendedSources.ICIUSLabel];
                }
            }



        }



        /// <summary>
        /// Set startdate and enddate column values 
        /// </summary>
        /// <param name="dtDataTable"></param>
        private void SetStartDateAndEndDateColumnValues(DataTable dtDataTable)
        {
            DateTime StartDate = DateTime.MinValue;
            DateTime EndDate = DateTime.MinValue;
            for (int i = 0; i < dtDataTable.Rows.Count; i++)
            {
                TimePeriodFacade.SetStartDateEndDate(dtDataTable.Rows[i][Timeperiods.TimePeriod].ToString(), ref StartDate, ref EndDate);
                dtDataTable.Rows[i][DataExpressionColumns.TimePeriodEndDate] = EndDate.ToString("yyyy.MM.dd");
            }
        }

        /// <summary>
        /// Set data expression column values and selected column values
        /// </summary>
        /// <param name="dtDataTable"></param>
        private void SetDataExpressionAndSelectedColumnValues(DataTable dtDataTable)
        {
            // Set locale of dataset to invariant culture to handle cases for different regional settings
            if (dtDataTable.DataSet != null)
            {
                //Changing locale of dataset automatically changes locale of all datatables it contains
                dtDataTable.DataSet.Locale = new System.Globalization.CultureInfo("", false);
            }
            else
            {
                dtDataTable.Locale = new System.Globalization.CultureInfo("", false);
            }

            System.Globalization.CultureInfo CurrentCultureInfo = System.Globalization.CultureInfo.CurrentCulture;

            //CurrentCultureInfo.NumberFormat.CurrencyDecimalDigits = -1;
            // Fill the values for Expression Columns 
            //OPT If loop can be avoided and column values can be set directly using expressions
            for (int i = 0; i < dtDataTable.Rows.Count; i++)
            {
                if (Utility.DICommon.IsNumeric(dtDataTable.Rows[i][Data.DataValue].ToString()) == true)
                {
                    try
                    {
                        dtDataTable.Rows[i][DataExpressionColumns.DataType] = DataType.Numeric;
                        dtDataTable.Rows[i][DataExpressionColumns.NumericData] = dtDataTable.Rows[i][Data.DataValue];
                        //  specifier  	type  	    format  output(double 1.2345)   output(int -12345)
                        //  c 	        currency 	{0:c} 	£1.23 	                -£12,345.00
                        //  d 	        decimal     {0:d} 	System.FormatException 	-12345
                        //  e 	        exponent    {0:e} 	1.234500e+000 	        -1.234500e+004
                        //  f 	        fixed point {0:f} 	1.23 	                -12345.00
                        //  g 	        general 	{0:g} 	1.2345 	                -12345
                        //  n 	        number 	    {0:n} 	1.23 	                -12,345.00

                        //TODO Apply regional formatting to DataValues
                        //Problem with commneted code is that it shows 2 digit decimal even for non decimal values
                        //dtDataTable.Rows[i][Data.DataValue] = string.Format(CurrentCultureInfo, "{0:n}", dtDataTable.Rows[i][DataExpressionColumns.NumericData]);

                        //-- Based on fact that NumberDecimalSeparator stored inside database is always "."
                        dtDataTable.Rows[i][Data.DataValue] = dtDataTable.Rows[i][Data.DataValue].ToString().Replace(".", CurrentCultureInfo.NumberFormat.NumberDecimalSeparator);
                    }
                    catch (Exception)
                    {
                        //http://www.blackwasp.co.uk/CSharpStringToNumeric.aspx
                        //-- TODO values like "(234)" are considered numeric but give exception "Couldn't store in NumericData Column.  Expected type is Decimal."
                        dtDataTable.Rows[i][DataExpressionColumns.DataType] = DataType.Text;
                        dtDataTable.Rows[i][DataExpressionColumns.TextualData] = dtDataTable.Rows[i][Data.DataValue];
                    }

                }
                else
                {
                    dtDataTable.Rows[i][DataExpressionColumns.DataType] = DataType.Text;
                    dtDataTable.Rows[i][DataExpressionColumns.TextualData] = dtDataTable.Rows[i][Data.DataValue];
                }

                //set selection value to true for all records by default
                dtDataTable.Rows[i][DataExpressionColumns.Selected] = true;
            }

            //set selection value to false for all records marked for deletion
            if (this.mUserPreference.UserSelection.DataViewFilters.DeletedDataNIds.Length > 0)
            {
                foreach (DataRow dr in dtDataTable.Select(Data.DataNId + " IN (" + this.mUserPreference.UserSelection.DataViewFilters.DeletedDataNIds + ")"))
                {
                    dr[DataExpressionColumns.Selected] = false;
                }
            }


        }

        private void SetRecSourceLabels(DataTable dtDataTable)
        {
            try
            {
                // Create a Data Set to store dtDataTable and the Metadata Data Table
                DataSet dsParentChild;

                if (dtDataTable.DataSet == null)
                {
                    dsParentChild = GetInvariantDataSet();

                    // Add Main Data Table to Data Set for genereating the Metadata 
                    dsParentChild.Tables.Add(dtDataTable);
                }
                else
                {
                    dsParentChild = dtDataTable.DataSet;
                }

                // If RecSourceLabel table is not populated alreday then build RecSourceLabel table
                if (this.mdtRecSourceLabel.Rows.Count == 0)
                {
                    string _Query = this.mdIQueries.RecommendedSources.GetRecommendedSources(string.Empty);
                    this.mdtRecSourceLabel = this.ExecuteInvariantDataTable(_Query);
                    this.mdtRecSourceLabel.TableName = "DataView_UserSelection_RecSourceLabel";
                }

                dsParentChild.Tables.Add(this.mdtRecSourceLabel.Copy());



                // Define relationship 
                dsParentChild.Relations.Add("RelRecSourceLabel", dsParentChild.Tables[this.mdtRecSourceLabel.TableName].Columns[RecommendedSources.DataNId], dsParentChild.Tables[dtDataTable.TableName].Columns[Data.DataNId], false);

                dsParentChild.Tables[dtDataTable.TableName].Columns[RecommendedSources.ICIUSLabel].Expression = "Parent(RelRecSourceLabel)." + RecommendedSources.ICIUSLabel;
                dsParentChild.AcceptChanges();



            }
            catch (Exception)
            {

                throw;
            }

        }


        #endregion

        #region Build Metadata Master Tables, Add Metadata Columns, Set Metadata Column values

        /// <summary>
        /// Build Metadata tables for Indicator, Area and Source
        /// </summary>
        /// <param name="userPreference"></param>
        /// <param name="dIConnection"></param>
        /// <param name="dIQueries"></param>
        /// <returns></returns>
        private void BuildMetadataTables(UserPreference.UserPreference userPreference, DI_LibDAL.Connection.DIConnection dIConnection, DI_LibDAL.Queries.DIQueries dIQueries)
        {
            if (userPreference.DataView.MetadataIndicatorField.Length > 0)
            {
                SetMetedataTables(MetadataElementType.Indicator, userPreference, dIConnection, dIQueries);
            }

            if (userPreference.DataView.MetadataAreaField.Length > 0)
            {
                SetMetedataTables(MetadataElementType.Area, userPreference, dIConnection, dIQueries);
            }

            if (userPreference.DataView.MetadataSourceField.Length > 0)
            {
                SetMetedataTables(MetadataElementType.Source, userPreference, dIConnection, dIQueries);
            }

        }

        /// <summary>
        /// Set metadata master tables for Indicator / Area / Source
        /// </summary>
        /// <param name="metadataElementType">Indicator / Area / Source</param>
        /// <param name="userPreference"></param>
        /// <param name="dIConnection"></param>
        /// <param name="dIQueries"></param>
        /// <returns></returns>
        private bool SetMetedataTables(MetadataElementType metadataElementType, UserPreference.UserPreference userPreference, DI_LibDAL.Connection.DIConnection dIConnection, DI_LibDAL.Queries.DIQueries dIQueries)
        {
            bool RetVal = false;
            string MaskFilePath = string.Empty;
            XmlDocument MaskFileDocument = new XmlDocument();
            XmlNodeList MaskNodeList;

            string MaskPathText = string.Empty;
            string MaskPositionText = string.Empty;
            string[] MaskPositionArray;

            XmlDocument MetadataDocument = new XmlDocument();
            string MetadataXml = string.Empty;
            XmlNodeList MetadataFld_ValNodeList;

            string[] arrSelectedMetadataFields = null;
            string JoinColumnName = string.Empty;
            string MetadataColumnName = string.Empty;

            string _Query = string.Empty;
            DataTable DistinctElementTable;
            string[] DistinctNIds = null;

            DataView dvMetadataInfo = null;

            DataTable ElementMetadataTable = null;

            string CategoryType = string.Empty; // I / A / S
            string MetadataPrefix = string.Empty; //MD_IND_, MD_SRC_

            switch (metadataElementType)
            {
                case MetadataElementType.Indicator:
                    // Set Category Type
                    CategoryType = DIQueries.MetadataElementTypeText[MetadataElementType.Indicator];
                    MetadataPrefix = UserPreference.UserPreference.DataviewPreference.MetadataIndicator;
                    // Get Metadata fields from  userPreference
                    arrSelectedMetadataFields = userPreference.DataView.MetadataIndicatorField.Split(",".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);

                    // Set Mask file path
                    MaskFilePath = Path.Combine(this.msMaskFolder, MASK_FILE_INDICATOR);

                    //Set Join column and Metadata Column name
                    JoinColumnName = Indicator.IndicatorNId;
                    MetadataColumnName = Indicator.IndicatorInfo;

                    // Get distinct indicator nids
                    DistinctElementTable = this._IUSIndicator.DefaultView.ToTable(true, JoinColumnName);
                    DistinctNIds = new string[DistinctElementTable.Rows.Count];
                    for (int i = 0; i < DistinctElementTable.Rows.Count; i++)
                    {
                        DistinctNIds[i] = DistinctElementTable.Rows[i][JoinColumnName].ToString();
                    }

                    // Get Metadata Info for all Indicators
                    _Query = this.mdIQueries.Indicators.GetIndicator(FilterFieldType.NId, string.Join(",", DistinctNIds), FieldSelection.Heavy);
                    dvMetadataInfo = this.ExecuteInvariantDataTable(_Query).DefaultView;

                    ElementMetadataTable = this._MetadataIndicator;

                    break;
                case MetadataElementType.Area:
                    // Set Category Type
                    CategoryType = DIQueries.MetadataElementTypeText[MetadataElementType.Area];
                    MetadataPrefix = UserPreference.UserPreference.DataviewPreference.MetadataArea;
                    // Get Metadata fields from  userPreference
                    arrSelectedMetadataFields = userPreference.DataView.MetadataAreaField.Split(",".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);

                    // Set Mask file path
                    MaskFilePath = Path.Combine(this.msMaskFolder, MASK_FILE_AREA);

                    //Get LayerNids for these areas
                    _Query = this.mdIQueries.Area.GetAreaMapByAreaNIds(this._AreaNIds, false);
                    DataTable dtAreaMap = this.ExecuteInvariantDataTable(_Query);

                    //Get Distinct LayerNIds
                    string[] ColumnArray = new string[1];
                    ColumnArray[0] = Area_Map.LayerNId;
                    DistinctElementTable = dtAreaMap.DefaultView.ToTable(true, ColumnArray);
                    DistinctNIds = new string[DistinctElementTable.Rows.Count];
                    for (int i = 0; i < DistinctElementTable.Rows.Count; i++)
                    {
                        DistinctNIds[i] = DistinctElementTable.Rows[i][ColumnArray[0]].ToString();
                    }

                    //Get Metadata for LayerNIds along with associated AreaNIds
                    DataTable dtMetadata = GetInvariantDataTable();
                    if (DistinctNIds.Length > 0)
                    {
                        //Join to Associate AreaNId and LayerNId
                        DataSet dsParentChild = GetInvariantDataSet();

                        // Add AreaMap Table
                        dtMetadata = dtAreaMap.Copy();  // Since it might be part if a relationship, therefore adding the COPY
                        dtMetadata.TableName = "Parent";
                        dtMetadata.Columns.Add(Area_Map_Metadata.MetadataText, typeof(string));
                        dsParentChild.Tables.Add(dtMetadata);

                        // Add AreaMapMetadata Table
                        _Query = this.mdIQueries.Area.GetAreaMapMetadata(string.Join(",", DistinctNIds));
                        DataTable dtAreaMapMetadata = this.ExecuteInvariantDataTable(_Query);
                        dtAreaMapMetadata.TableName = "Child";

                        // Set Primaty Key in the AreaMapMetadata
                        DataColumn[] PK = new DataColumn[1];
                        PK[0] = dtAreaMapMetadata.Columns[Area_Map_Metadata.LayerNId];
                        dtAreaMapMetadata.PrimaryKey = PK;

                        dsParentChild.Tables.Add(dtAreaMapMetadata);

                        // Define relationship 
                        dsParentChild.Relations.Add("RelAreaMap", dsParentChild.Tables[1].Columns[Area_Map.LayerNId], dsParentChild.Tables[0].Columns[Area_Map_Metadata.LayerNId], false);
                        dsParentChild.Tables[0].Columns[Area_Map_Metadata.MetadataText].Expression = "Parent(RelAreaMap)." + Area_Map_Metadata.MetadataText;
                        dsParentChild.AcceptChanges();
                    }
                    dvMetadataInfo = dtMetadata.DefaultView; //(AreaNId-LayerNId-MetadataText)

                    //Set Join column and Metadata Column name
                    JoinColumnName = Area.AreaNId;
                    MetadataColumnName = Area_Map_Metadata.MetadataText;

                    // Get distinct area nids
                    DistinctNIds = this._AreaNIds.Split(",".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);

                    // Set metadata table
                    ElementMetadataTable = this._MetadataArea;

                    break;
                case MetadataElementType.Source:
                    // Set Category Type
                    CategoryType = DIQueries.MetadataElementTypeText[MetadataElementType.Source];
                    MetadataPrefix = UserPreference.UserPreference.DataviewPreference.MetadataSource;

                    // Get Metadata fields from  userPreference
                    arrSelectedMetadataFields = userPreference.DataView.MetadataSourceField.Split(",".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);

                    // Set Mask file path
                    MaskFilePath = Path.Combine(this.msMaskFolder, MASK_FILE_SOURCE);

                    //Set Join column and Metadata Column name
                    JoinColumnName = IndicatorClassifications.ICNId;
                    MetadataColumnName = IndicatorClassifications.ICInfo;

                    // Get distinct source nids
                    DistinctNIds = new string[this._Sources.Rows.Count];
                    for (int i = 0; i < this._Sources.Rows.Count; i++)
                    {
                        DistinctNIds[i] = this._Sources.Rows[i][JoinColumnName].ToString();
                    }

                    // Get Metadata Info for all sources
                    _Query = this.mdIQueries.IndicatorClassification.GetIC(FilterFieldType.NId, string.Join(",", DistinctNIds), ICType.Source, FieldSelection.Heavy);
                    dvMetadataInfo = this.ExecuteInvariantDataTable(_Query).DefaultView;

                    // Set metadata table
                    ElementMetadataTable = this._MetadataSource;

                    break;
            }


            // Check for existence of indicator mask file
            if (File.Exists(MaskFilePath))
            {
                // Try loading indicator mask file
                try
                {
                    MaskFileDocument.Load(MaskFilePath);
                }
                catch (Exception)
                {
                    //TODO do not proceed further
                }
            }

            // Add Join columns to Metadata table 
            ElementMetadataTable.Columns.Clear();
            ElementMetadataTable.Columns.Add(JoinColumnName, typeof(int));

            if (this._CensusInfoFeature)
            {
                _Query = this.mdIQueries.Metadata_Category.GetMetadataCategories(FilterFieldType.Type, CategoryType);
                DataView dvMetadataCategory = this.ExecuteInvariantDataTable(_Query).DefaultView;

                for (int j = 0; j < arrSelectedMetadataFields.Length; j++)
                {
                    // Add Metadata columns to Metadata table 

                    ElementMetadataTable.Columns.Add(arrSelectedMetadataFields[j], typeof(string));
                    ElementMetadataTable.Columns[arrSelectedMetadataFields[j]].DefaultValue = string.Empty;

                    // For each distinct Element 
                    for (int i = 0; i < DistinctNIds.Length; i++)
                    {
                        DataRow drMetadata;
                        if (j == 0)
                        {
                            // Create data row for each Element (Indicator / Area / Source) only once
                            drMetadata = ElementMetadataTable.NewRow();
                            drMetadata[JoinColumnName] = DistinctNIds[i];
                        }
                        else
                        {
                            // If data row alreday exists simply update the desired field
                            drMetadata = ElementMetadataTable.Select(JoinColumnName + " = " + DistinctNIds[i])[0];
                        }

                        if (dvMetadataInfo != null && dvMetadataInfo.Table.Columns.Contains(JoinColumnName))
                        {
                            dvMetadataInfo.RowFilter = JoinColumnName + " = " + DistinctNIds[i];
                            // Check for valid xml content
                            if (dvMetadataInfo.Count > 0)
                            {
                                MetadataXml = dvMetadataInfo[0][MetadataColumnName].ToString();
                                if (MetadataXml != null && MetadataXml.Trim().Length > 0)
                                {
                                    try   // Try loading metadata xml
                                    {
                                        //Get all FLD_VAL contents into a Nodelist
                                        XmlDocument xmlDoc = new XmlDocument();
                                        xmlDoc.LoadXml(dvMetadataInfo[0][MetadataColumnName].ToString());

                                        dvMetadataCategory.RowFilter = Metadata_Category.CategoryNId + " = " + arrSelectedMetadataFields[j].Replace(MetadataPrefix, "");
                                        if (dvMetadataCategory != null && dvMetadataCategory.Count > 0)
                                        {

                                            XmlNode CategoryXmlNode = xmlDoc.SelectSingleNode("//Category[@name='" + dvMetadataCategory[0][Metadata_Category.CategoryName].ToString() + "']");

                                            string ParaNodeText = string.Empty;
                                            if (!String.IsNullOrEmpty(CategoryXmlNode.InnerText))
                                            {
                                                // Replace Para defining delimiter by new line character
                                                ParaNodeText = CategoryXmlNode.InnerText.Replace(DevInfo.Lib.DI_LibBAL.DA.DML.MetadataManagerConstants.ReplaceableSymbol, Environment.NewLine);
                                            }

                                            // get category value from xml (from <para> tags)
                                            //foreach (XmlNode ParaNode in CategoryXmlNode.ChildNodes)
                                            //{
                                            //    // get para node text
                                            //    // add para node into concatenated category value
                                            //    if (!string.IsNullOrEmpty(ParaNodeText))
                                            //    {
                                            //        ParaNodeText += Microsoft.VisualBasic.ControlChars.NewLine ;
                                            //    }
                                            //     ParaNodeText +=  ParaNode.InnerText.Trim();

                                            //}

                                            // Based on position information (index) get Metadata content
                                            drMetadata[arrSelectedMetadataFields[j]] = ParaNodeText;
                                        }



                                    }
                                    catch (Exception)
                                    {
                                        //If unable to load metadata text then fields shall remain empty
                                    }
                                }

                            }
                            dvMetadataInfo.RowFilter = string.Empty;
                        }
                        // Add element row only while looping for first field
                        if (j == 0)
                        {
                            ElementMetadataTable.Rows.Add(drMetadata);
                        }
                    }

                }
            }
            else
            {
                // For each selected field for indicator / area (map) / source metadata
                for (int j = 0; j < arrSelectedMetadataFields.Length; j++)
                {

                    // Add Metadata columns to Metadata table 
                    ElementMetadataTable.Columns.Add(arrSelectedMetadataFields[j], typeof(string));
                    ElementMetadataTable.Columns[arrSelectedMetadataFields[j]].DefaultValue = string.Empty;

                    // Get position information from mask file
                    MaskNodeList = MaskFileDocument.SelectNodes("*/*[ID='" + arrSelectedMetadataFields[j] + "']"); //root/Input1/ID

                    if (MaskNodeList.Count > 0)
                    {
                        if (MaskNodeList[0].SelectNodes("Position")[0] != null)     //TODO Remove Hardcoding "Position"
                        {
                            MaskPositionText = MaskNodeList[0].SelectNodes("Position")[0].InnerText;    //TODO Remove Hardcoding "Position"
                            MaskPositionArray = MaskPositionText.Split("/".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
                            if (MaskPositionArray.Length == 5) // 1/1/1/n/1   1/2/1/n/1
                            {
                                // For each distinct Element 
                                for (int i = 0; i < DistinctNIds.Length; i++)
                                {
                                    DataRow drMetadata;
                                    if (j == 0)
                                    {
                                        // Create data row for each Element (Indicator / Area / Source) only once
                                        drMetadata = ElementMetadataTable.NewRow();
                                        drMetadata[JoinColumnName] = DistinctNIds[i];
                                    }
                                    else
                                    {
                                        // If data row alreday exists simply update the desired field
                                        drMetadata = ElementMetadataTable.Select(JoinColumnName + " = " + DistinctNIds[i])[0];
                                    }

                                    if (dvMetadataInfo != null && dvMetadataInfo.Table.Columns.Contains(JoinColumnName))
                                    {
                                        dvMetadataInfo.RowFilter = JoinColumnName + " = " + DistinctNIds[i];
                                        // Check for valid xml content
                                        if (dvMetadataInfo.Count > 0)
                                        {
                                            MetadataXml = dvMetadataInfo[0][MetadataColumnName].ToString();
                                            if (MetadataXml.Trim().Length > 0)
                                            {
                                                try   // Try loading metadata xml
                                                {
                                                    MetadataDocument.LoadXml(MetadataXml);

                                                    //Get all FLD_VAL contents into a Nodelist
                                                    MetadataFld_ValNodeList = MetadataDocument.SelectNodes("Indicator_Info/Row1/FLD_VAL"); //TODO Remove Hardcoding

                                                    // Based on position information (index) get Metadata content
                                                    //drMetadata[arrSelectedMetadataFields[j]] = "abc";
                                                    if (MetadataFld_ValNodeList.Count >= int.Parse(MaskPositionArray[1]))
                                                    {
                                                        drMetadata[arrSelectedMetadataFields[j]] = MetadataFld_ValNodeList[(int.Parse(MaskPositionArray[1]) - 1)].InnerText.Trim();
                                                    }
                                                }
                                                catch (Exception)
                                                {
                                                    //If unable to load metadata text then fields shall remain empty
                                                }
                                            }

                                        }
                                    }
                                    // Add element row only while looping for first field
                                    if (j == 0)
                                    {
                                        ElementMetadataTable.Rows.Add(drMetadata);
                                    }
                                }
                            }
                        }
                    }
                }
            }



            return RetVal;
        }

        /// <summary>
        /// Add Indicator, Area and Source Metadata Columns into Main Data Table, based on user preference
        /// </summary>
        /// <param name="dtDataTable"></param>
        private void AddMetadataColumnsToDataTable(DataTable dtDataTable)
        {

            string[] arrSelectedMetadataFields;
            // -- Add Indicator Metadata fields and set their language based captions
            arrSelectedMetadataFields = this.mUserPreference.DataView.MetadataIndicatorField.Split(",".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < arrSelectedMetadataFields.Length; i++)
            {
                dtDataTable.Columns.Add(arrSelectedMetadataFields[i], typeof(string));
                dtDataTable.Columns[arrSelectedMetadataFields[i]].Caption = this._MetadataIndicator.Columns[arrSelectedMetadataFields[i]].Caption;
            }

            // -- Add Map Metadata fields and set their language based captions
            arrSelectedMetadataFields = this.mUserPreference.DataView.MetadataAreaField.Split(",".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < arrSelectedMetadataFields.Length; i++)
            {
                dtDataTable.Columns.Add(arrSelectedMetadataFields[i], typeof(string));
                dtDataTable.Columns[arrSelectedMetadataFields[i]].Caption = this._MetadataArea.Columns[arrSelectedMetadataFields[i]].Caption;
            }


            // -- Add Source Metadata fields and set their language based captions
            arrSelectedMetadataFields = this.mUserPreference.DataView.MetadataSourceField.Split(",".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < arrSelectedMetadataFields.Length; i++)
            {
                dtDataTable.Columns.Add(arrSelectedMetadataFields[i], typeof(string));
                dtDataTable.Columns[arrSelectedMetadataFields[i]].Caption = this._MetadataSource.Columns[arrSelectedMetadataFields[i]].Caption;
            }


            dtDataTable.AcceptChanges();
        }

        /// <summary>
        /// Set metadata column values in main table by defining relationship with metadata tables
        /// </summary>
        /// <param name="dtDataTable"></param>
        private void SetMetadataColumnValues(DataTable dtDataTable)
        {
            // -- Add Metadata (Indicator, Area, Source) to the main data table
            // Create a Data Set to store dtDataTable and the Metadata Data Table
            DataSet dsParentChild;
            string[] arrSelectedMetadataFields;

            if (dtDataTable.DataSet == null)
            {
                dsParentChild = GetInvariantDataSet();

                // Add Main Data Table to Data Set for genereating the Metadata 
                dsParentChild.Tables.Add(dtDataTable);
            }
            else
            {
                dsParentChild = dtDataTable.DataSet;
            }

            //Indicator
            arrSelectedMetadataFields = this.mUserPreference.DataView.MetadataIndicatorField.Split(",".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
            if (arrSelectedMetadataFields.Length > 0)
            {
                // Add Indicator Metadata Data Table
                dsParentChild.Tables.Add(this._MetadataIndicator.Copy()); // Since would already be part if a relationship, therefore adding the COPY

                // Set Primaty Key in the Indicator Metadata Data Table
                DataColumn[] PK = new DataColumn[1];
                PK[0] = dsParentChild.Tables[this._MetadataIndicator.TableName].Columns[Indicator.IndicatorNId];
                dsParentChild.Tables[this._MetadataIndicator.TableName].PrimaryKey = PK;

                // Define relationship 

                dsParentChild.Relations.Add("RelIndicatorMetadata", dsParentChild.Tables[this._MetadataIndicator.TableName].Columns[Indicator.IndicatorNId], dsParentChild.Tables[dtDataTable.TableName].Columns[Indicator.IndicatorNId], false);

                for (int i = 0; i < arrSelectedMetadataFields.Length; i++)
                {
                    dsParentChild.Tables[dtDataTable.TableName].Columns[arrSelectedMetadataFields[i]].Expression = "Parent(RelIndicatorMetadata)." + arrSelectedMetadataFields[i];
                }
            }

            //Area
            arrSelectedMetadataFields = this.mUserPreference.DataView.MetadataAreaField.Split(",".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
            if (arrSelectedMetadataFields.Length > 0)
            {
                // Add Indicator Metadata Data Table
                dsParentChild.Tables.Add(this._MetadataArea.Copy()); // Since would already be part if a relationship, therefore adding the COPY

                // Set Primaty Key in the Indicator Metadata Data Table
                DataColumn[] PK = new DataColumn[1];
                PK[0] = dsParentChild.Tables[this._MetadataArea.TableName].Columns[Area.AreaNId];
                dsParentChild.Tables[this._MetadataArea.TableName].PrimaryKey = PK;

                // Define relationship 

                dsParentChild.Relations.Add("RelAreaMetadata", dsParentChild.Tables[this._MetadataArea.TableName].Columns[Area.AreaNId], dsParentChild.Tables[dtDataTable.TableName].Columns[Data.AreaNId], false);

                for (int i = 0; i < arrSelectedMetadataFields.Length; i++)
                {
                    dsParentChild.Tables[dtDataTable.TableName].Columns[arrSelectedMetadataFields[i]].Expression = "Parent(RelAreaMetadata)." + arrSelectedMetadataFields[i];
                }
            }


            //Source
            arrSelectedMetadataFields = this.mUserPreference.DataView.MetadataSourceField.Split(",".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
            if (arrSelectedMetadataFields.Length > 0)
            {
                // Add Indicator Metadata Data Table
                dsParentChild.Tables.Add(this._MetadataSource.Copy());

                // Set Primaty Key in the Indicator Metadata Data Table
                DataColumn[] PK = new DataColumn[1];
                PK[0] = dsParentChild.Tables[this._MetadataSource.TableName].Columns[IndicatorClassifications.ICNId];
                dsParentChild.Tables[this._MetadataSource.TableName].PrimaryKey = PK;

                // Define relationship 

                dsParentChild.Relations.Add("RelSourceMetadata", dsParentChild.Tables[this._MetadataSource.TableName].Columns[IndicatorClassifications.ICNId], dsParentChild.Tables[dtDataTable.TableName].Columns[IndicatorClassifications.ICNId], false);

                for (int i = 0; i < arrSelectedMetadataFields.Length; i++)
                {
                    dsParentChild.Tables[dtDataTable.TableName].Columns[arrSelectedMetadataFields[i]].Expression = "Parent(RelSourceMetadata)." + arrSelectedMetadataFields[i];
                }
            }

            dsParentChild.AcceptChanges();

        }

        /// <summary>
        /// Set language based captions for metadata fields in main data table (mdtData)
        /// </summary>
        /// <param name="MetadataFields">Comma delimited metadata fields. MD_IND_1,MD_IND_3...</param>
        /// <param name="MaskFilePath">Mask file path with extension</param>
        /// <param name="languageCode">language code against which caption shall be picked from mask file. en / fr / ru ...</param>
        private void SetMetadataCaption(MetadataElementType metadataElementType, string MetadataFields, string MaskFilePath)
        {

            string[] arrSelectedMetadataFields = null;
            DataTable MetadataTable = null;
            string CategoryType = string.Empty; // I / A / S
            string MetadataPrefix = string.Empty; //MD_IND_, MD_SRC_

            switch (metadataElementType)
            {
                case MetadataElementType.Indicator:
                    MetadataTable = this._MetadataIndicator;
                    CategoryType = DIQueries.MetadataElementTypeText[MetadataElementType.Indicator];
                    MetadataPrefix = UserPreference.UserPreference.DataviewPreference.MetadataIndicator;
                    break;
                case MetadataElementType.Area:
                    MetadataTable = this._MetadataArea;
                    CategoryType = DIQueries.MetadataElementTypeText[MetadataElementType.Area];
                    MetadataPrefix = UserPreference.UserPreference.DataviewPreference.MetadataArea;
                    break;
                case MetadataElementType.Source:
                    MetadataTable = this._MetadataSource;
                    // Set Category Type
                    CategoryType = DIQueries.MetadataElementTypeText[MetadataElementType.Source];
                    MetadataPrefix = UserPreference.UserPreference.DataviewPreference.MetadataSource;
                    break;
            }

            // Get Metadata fields from  userPreference
            arrSelectedMetadataFields = MetadataFields.Split(",".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);

            if (this._CensusInfoFeature)
            {

                try
                {
                    string _Query = this.mdIQueries.Metadata_Category.GetMetadataCategories(FilterFieldType.Type, CategoryType);
                    DataView dvMetadataCategory = this.ExecuteInvariantDataTable(_Query).DefaultView;

                    //Set Metadata captions
                    for (int i = 0; i < arrSelectedMetadataFields.Length; i++)
                    {
                        // Filter for Category
                        dvMetadataCategory.RowFilter = Metadata_Category.CategoryNId + " = " + arrSelectedMetadataFields[i].Replace(MetadataPrefix, "");

                        // Get Caption from Mask file based on current interface language
                        if (dvMetadataCategory != null && dvMetadataCategory.Count > 0)
                        {
                            // Set Column caption for both main table as well as metadata master tables
                            this._MainDataTable.Columns[arrSelectedMetadataFields[i]].Caption = dvMetadataCategory[0][Metadata_Category.CategoryName].ToString();
                            MetadataTable.Columns[arrSelectedMetadataFields[i]].Caption = dvMetadataCategory[0][Metadata_Category.CategoryName].ToString();
                        }



                    }
                }
                catch (Exception)
                {

                }


            }
            else
            {

                string languageCode = DILanguage.GetLanguageString("LANGUAGE_CODE");
                XmlDocument MaskFileDocument = new XmlDocument();
                XmlNodeList MaskNodeList;
                XmlNode MaskCaptionNode;
                // Check for existence of indicator mask file
                if (File.Exists(MaskFilePath))
                {
                    try
                    {
                        // Try loading indicator mask file
                        MaskFileDocument.Load(MaskFilePath);

                        //Set Metadata captions
                        for (int i = 0; i < arrSelectedMetadataFields.Length; i++)
                        {
                            // Get position information from mask file
                            MaskNodeList = MaskFileDocument.SelectNodes("*/*[ID='" + arrSelectedMetadataFields[i] + "']"); //root/Input1/ID

                            // Get Caption from Mask file based on current interface language
                            //TODO Remove Hardcoding "Caption"
                            if (MaskNodeList[0].SelectNodes("Caption[@lang='" + languageCode + "']") != null)
                            {
                                MaskCaptionNode = MaskNodeList[0].SelectNodes("Caption[@lang='" + languageCode + "']")[0];
                                if (MaskCaptionNode != null)
                                {
                                    // Set Column caption for both main table as well as metadata master tables
                                    this._MainDataTable.Columns[arrSelectedMetadataFields[i]].Caption = MaskCaptionNode.InnerText;
                                    MetadataTable.Columns[arrSelectedMetadataFields[i]].Caption = MaskCaptionNode.InnerText;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }


        }

        #endregion

        #region  Build IUSICInfo Master Table, Add IC Columns, Set IC Column values

        /// <summary>
        /// Set IC Field Information for columns selected by user
        /// </summary>
        /// <param name="userPreference"></param>
        /// <param name="dIConnection"></param>
        /// <param name="dIQueries"></param>
        /// <param name="dtIUSNIDs">Datatable containing distinct IUSNIds for full dataview</param>
        private void BuildIUSICInfoTable(UserPreference.UserPreference userPreference, DI_LibDAL.Connection.DIConnection dIConnection, DI_LibDAL.Queries.DIQueries dIQueries, DataTable dtIUSNIDs)
        {

            string sIUSICInfoTableName = this._IUSICInfo.TableName; //Preserve Table name
            string sIUSICNIdInfoTableName = this._IUSICNIdInfo.TableName; //Preserve Table name

            // Get Table with IUSNId column. Later add additional columns based on IC fields selected
            this._IUSICInfo = dtIUSNIDs.Copy();
            this._IUSICNIdInfo = dtIUSNIDs.Copy();

            this._IUSICInfo.TableName = sIUSICInfoTableName; //Restore Table Name
            this._IUSICNIdInfo.TableName = sIUSICNIdInfoTableName; //Restore Table Name

            string[] arrSelectedICFields = null;

            // This dictionary will store all distinct ICType(KEY) for which fields are selected and Maximum child level (Value) selected  
            System.Collections.Generic.Dictionary<ICType, int> ICMaxLevelCol = new Dictionary<ICType, int>();

            System.Collections.Generic.Dictionary<ICType, string> ICGIdsCol = new Dictionary<ICType, string>();

            string sICType = string.Empty;
            int ICIndex = -1;
            ICType ICType = ICType.Sector;
            string sLevel = string.Empty;
            string sCaption = string.Empty;


            // Add selected IC columns to _IUSICInfo table
            // Set language based caption
            // Preserve information of distinct IC selected and max child level to traverse in ICMaxLevelCol
            if (userPreference.DataView.ICFields.Length > 0)
            {
                arrSelectedICFields = userPreference.DataView.ICFields.Split(",".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < arrSelectedICFields.Length; i++)
                {
                    sICType = arrSelectedICFields[i].Substring(4, 2);
                    ICIndex = DIQueries.ICTypeText.Values.IndexOf("'" + sICType + "'");
                    if (ICIndex > -1)
                    {
                        ICType = DIQueries.ICTypeText.Keys[ICIndex];
                    }

                    sLevel = arrSelectedICFields[i].Substring(7);
                    this._IUSICInfo.Columns.Add(arrSelectedICFields[i], typeof(string));
                    this._IUSICNIdInfo.Columns.Add(arrSelectedICFields[i], typeof(int));

                    this._IUSICInfo.Columns[arrSelectedICFields[i]].DefaultValue = string.Empty;
                    this._IUSICNIdInfo.Columns[arrSelectedICFields[i]].DefaultValue = -1;

                    if (ICMaxLevelCol.ContainsKey(ICType))
                    {
                        ICMaxLevelCol[ICType] = Math.Max(ICMaxLevelCol[ICType], int.Parse(sLevel));
                    }
                    else
                    {
                        ICMaxLevelCol.Add(ICType, int.Parse(sLevel));
                    }

                    if (ICGIdsCol.ContainsKey(ICType) == false)
                    {
                        string[] sICGIds = null;
                        switch (ICType)
                        {
                            case ICType.Sector:
                                sICGIds = userPreference.DataView.ICSectorGIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                break;
                            case ICType.Goal:
                                sICGIds = userPreference.DataView.ICGoalGIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                break;
                            case ICType.CF:
                                sICGIds = "-1".Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                break;
                            case ICType.Theme:
                                sICGIds = userPreference.DataView.ICThemeGIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                break;
                            case ICType.Source:
                                sICGIds = userPreference.DataView.ICSourceGIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                break;
                            case ICType.Institution:
                                sICGIds = userPreference.DataView.ICInstitutionalGIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                break;
                            case ICType.Convention:
                                sICGIds = userPreference.DataView.ICConventionGIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                break;
                        }
                        for (int j = 0; j < sICGIds.Length; j++)
                        {
                            sICGIds[j] = "'" + sICGIds[j] + "'";
                        }
                        ICGIdsCol.Add(ICType, string.Join(",", sICGIds));

                    }
                }
            }

            string sCommaDelimitedICTypes = string.Empty;
            string _Query = string.Empty;

            // Get distinct comma delimited IC types for query
            foreach (ICType sKey in ICMaxLevelCol.Keys)
            {
                sCommaDelimitedICTypes += "," + DIQueries.ICTypeText[sKey];
            }

            // Fetch records for IC based on IUSNIds and ICTypes
            if (sCommaDelimitedICTypes.Length > 0)
            {
                sCommaDelimitedICTypes = sCommaDelimitedICTypes.Substring(1); //Remove extra ","
                _Query = dIQueries.IndicatorClassification.GetICForIUSNId(FilterFieldType.Type, sCommaDelimitedICTypes, this._IUSNIds, FieldSelection.Light);
            }
            else
            {
                _Query = dIQueries.IndicatorClassification.GetICForIUSNId(FilterFieldType.None, "", this._IUSNIds, FieldSelection.Light);
            }
            DataView dvIC = this.ExecuteInvariantDataTable(_Query).DefaultView;

            int iMaxLevel;
            string sParentNIds = string.Empty;
            string sColumnName = string.Empty;

            //for each classification type (for which fields has been selected)
            foreach (ICType sKey in ICMaxLevelCol.Keys)
            {
                ICType = sKey;
                iMaxLevel = ICMaxLevelCol[sKey];
                sParentNIds = "-1";

                // for each level of current classification type
                for (int i = 1; i <= iMaxLevel; i++)
                {
                    if (!string.IsNullOrEmpty(sParentNIds))
                    {

                        // Build the column name based on field naming convention "CLS_SC_1", "CLS_GL_3"
                        sColumnName = "CLS_" + DIQueries.ICTypeText[ICType].Replace("'", "") + "_" + i.ToString();

                        // Check whether this column is part of _IUSICInfo table
                        if (this._IUSICInfo.Columns.Contains(sColumnName))
                        {

                            // for each IUS set _ICInfo[sColumnName] if its associated to Current IC record
                            for (int k = 0; k < this._IUSICInfo.Rows.Count; k++)
                            {
                                dvIC.RowFilter = IndicatorClassificationsIUS.IUSNId + " = " + this._IUSICInfo.Rows[k][IndicatorClassificationsIUS.IUSNId].ToString() + " AND " + IndicatorClassifications.ICParent_NId + " IN (" + sParentNIds + ") AND " + IndicatorClassifications.ICGId + " IN (" + ICGIdsCol[ICType] + ") AND " + IndicatorClassifications.ICType + " = " + DIQueries.ICTypeText[ICType];

                                if (dvIC.Count > 0)
                                {
                                    this._IUSICInfo.Rows[k][sColumnName] = dvIC[0][IndicatorClassifications.ICName];
                                    this._IUSICNIdInfo.Rows[k][sColumnName] = dvIC[0][IndicatorClassifications.ICNId];
                                }
                                else
                                {
                                    this._IUSICInfo.Rows[k][sColumnName] = string.Empty;
                                }
                            }
                        }


                        // Set ParentNIds for next level
                        dvIC.RowFilter = IndicatorClassifications.ICParent_NId + " IN (" + sParentNIds + ") AND " + IndicatorClassifications.ICType + " = " + DIQueries.ICTypeText[ICType];
                        sParentNIds = string.Empty;
                        for (int j = 0; j < dvIC.Count; j++)
                        {
                            sParentNIds += "," + dvIC[j][IndicatorClassifications.ICNId];
                        }

                        // remove extra ","
                        if (sParentNIds.Length > 0)
                        {
                            sParentNIds = sParentNIds.Substring(1);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Add IC Columns into Main Data Table, based on user preference
        /// </summary>
        /// <param name="dtDataTable"></param>
        private void AddIUSICInfoColumnsToDataTable(DataTable dtDataTable)
        {
            string[] arrSelectedICFields = this.mUserPreference.DataView.ICFields.Split(",".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);

            // -- Add IUSICInfo fields and set their language based captions
            for (int i = 0; i < arrSelectedICFields.Length; i++)
            {
                dtDataTable.Columns.Add(arrSelectedICFields[i], typeof(string));
                dtDataTable.Columns[arrSelectedICFields[i]].Caption = this._IUSICInfo.Columns[arrSelectedICFields[i]].Caption;
            }
        }

        /// <summary>
        /// Set IC column values in main table by defining relationship with IUSICInfo table
        /// </summary>
        /// <param name="dtDataTable"></param>
        private void SetIUSICInfoColumnValues(DataTable dtDataTable)
        {
            // -- Add Metadata (Indicator, Area, Source) to the main data table
            // Create a Data Set to store dtDataTable and the Metadata Data Table
            DataSet dsParentChild;

            if (dtDataTable.DataSet == null)
            {
                dsParentChild = GetInvariantDataSet();

                // Add Main Data Table to Data Set for genereating the Metadata 
                dsParentChild.Tables.Add(dtDataTable);
            }
            else
            {
                dsParentChild = dtDataTable.DataSet;
            }


            //IC Fields
            string[] arrSelectedICFields = this.mUserPreference.DataView.ICFields.Split(",".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
            if (arrSelectedICFields.Length > 0)
            {
                // Add IUSICInfo Data Table
                dsParentChild.Tables.Add(this._IUSICInfo.Copy());

                // Set Primaty Key in the IUSICInfo Data Table
                DataColumn[] PK = new DataColumn[1];
                PK[0] = dsParentChild.Tables[this._IUSICInfo.TableName].Columns[IndicatorClassificationsIUS.IUSNId];
                dsParentChild.Tables[this._IUSICInfo.TableName].PrimaryKey = PK;

                // Define relationship 
                dsParentChild.Relations.Add("RelICInfo", dsParentChild.Tables[this._IUSICInfo.TableName].Columns[IndicatorClassificationsIUS.IUSNId], dsParentChild.Tables[dtDataTable.TableName].Columns[Data.IUSNId], false);

                // Set IC Column Values
                for (int i = 0; i < arrSelectedICFields.Length; i++)
                {
                    dsParentChild.Tables[dtDataTable.TableName].Columns[arrSelectedICFields[i]].Expression = "Parent(RelICInfo)." + arrSelectedICFields[i];
                }
            }


            dsParentChild.AcceptChanges();
        }

        #endregion

        #region DataPaging

        /// <summary>
        /// Reurns bool value based on condition that background worker process to fill dataview records is completed or in process
        /// </summary>
        /// <returns></returns>
        private bool IsDataViewFullyGenerated()
        {
            // How do we know DataView is fully generated
            // If total pages generated are more than 1 then DataView might not be fully generated
            // If the Bakground process for building up the DataView is finished then the DataView is fully generated

            // Fully generated cases
            // CASE 1: Only one page of Data
            // CASE 2: More than one page of data and Background porocess finished

            bool retVal = false;

            // CASE 1: Check Case 1
            if (this._PageCount == 1)
            {
                // DataView Fully Generated
                retVal = true;
            }
            else
            {
                if (moWorkerThread != null && moWorkerThread.IsAlive)
                {
                    retVal = false;
                }
                else
                {
                    retVal = true;
                }

            }
            return retVal;
        }

        /// <summary>
        /// Set Total Record Count and Page Count when ever MainDataTable is regenerated
        /// </summary>
        /// <param name="iRecordCount"></param>
        private void SetPagingInfo(int iRecordCount)
        {
            // Record Count
            this._RecordCount = iRecordCount;

            if (this._RecordCount == 0)
            {
                this._RecordCount = 0;
                this._PageCount = 0;
            }

            this._PageCount = (int)Math.Ceiling((double)((double)this._RecordCount / (double)this.mUserPreference.General.PageSize));

            // Array of bool to keep track of pages for which data has been generated
            mbPageDataStatus = new bool[this._PageCount];

        }

        /// <summary>
        /// Get datatable for a single page. Append all extra columns desired
        /// </summary>
        /// <param name="pageDataNIds">comma delimited datanids for single page</param>
        /// <returns></returns>
        private DataTable GetDataforDataNIDs(string pageDataNIds)
        {
            DataTable dtPageData = GetInvariantDataTable();
            // use default settings of invariant culture
            string _Query = string.Empty;

            if (pageDataNIds.Length > 0)
            {
                //-- Get All required Fields on the basis of the Data_NIds  and Create Main Data Table
                _Query = this.mdIQueries.Data.GetDataViewDataByDataNIDs(pageDataNIds);
                dtPageData = this.ExecuteInvariantDataTable(_Query);

                //-- Add Extra Columns in the Main Data Table
                this.AddExtraColumnsToDataTable(dtPageData);


                #region Set Values for extra Columns in the Main Data Table

                //-- Set Subgroup values (Age, Sex, Location, Others) in the main data table based on relationship
                this.SetSubgroupValues(dtPageData);


                //-- Set Recommended Source Labels
                // recommended source label should be set only at the time of first request rather than by default
                // When user pref is altered and ShowRecSource is toggled, DataContentChangedEvent is raised and new datview is created
                if (this.mUserPreference.General.ShowRecommendedSourceColor)
                {
                    this.SetRecSourceLabels(dtPageData);
                }

                //-- Set Metadata text in the main data table based on relationship
                this.SetMetadataColumnValues(dtPageData);

                //-- Set IC Column Values in the main data table based on relationship
                this.SetIUSICInfoColumnValues(dtPageData);

                //-- Set DataExpression and Selection Column values
                this.SetDataExpressionAndSelectedColumnValues(dtPageData);


                #endregion

                #region Add and Set Extra Columns for TimePeriodEndDate only if TimePeriodEndDate is part of Sort Field
                //-- Add End date expression column if filter clause includes filter for end date and main table does not contain end column
                if (this.mUserPreference.DataView.SortFields.Contains(DataExpressionColumns.TimePeriodEndDate) && !dtPageData.Columns.Contains(DataExpressionColumns.TimePeriodEndDate))
                {
                    //-- Add expression column and set values 
                    this.AddStartDateEndDateColumnsToDataTable(dtPageData);
                    this.SetStartDateAndEndDateColumnValues(dtPageData);
                }
                #endregion



            }

            return dtPageData;
        }


        /// <summary>
        /// This is a variant of GetDataforDataNIDs for MySql connection.
        /// It handles issues like dataset relationship, maxlength for memo fields (DataValue, IcName, ...) 
        /// </summary>
        /// <param name="pageDataNIds"></param>
        /// <returns></returns>
        private DataTable GetDataforDataNIDsMySql(string pageDataNIds)
        {
            DataTable dtPageData = GetInvariantDataTable();
            // use default settings of invariant culture
            string _Query = string.Empty;

            if (pageDataNIds.Length > 0)
            {
                //-- Get All required Fields on the basis of the Data_NIds  and Create Main Data Table
                _Query = this.mdIQueries.Data.GetDataViewDataByDataNIDs(pageDataNIds);
                dtPageData = this.ExecuteInvariantDataTable(_Query);


                //-- Add Extra Columns in the Main Data Table
                this.AddExtraColumnsToDataTable(dtPageData);


                #region Set Values for extra Columns in the Main Data Table


                // This is a variant of SetSubgroupValues which populates subgroup and other expression column avoiding dataset relationship
                // Dataset realtionship causes issues in case of MySql
                //-- Set Subgroup values (Age, Sex, Location, Others) in the main data table based on relationship
                //-- Set Recommended Source Labels
                //-- Set DataExpression and Selection Column values
                this.SetSubgroupValuesMySql(dtPageData, pageDataNIds);

                //-- Set End date expression column if filter clause includes filter for end date and main table does not contain end column
                if (this.mUserPreference.DataView.SortFields.Contains(DataExpressionColumns.TimePeriodEndDate) && !dtPageData.Columns.Contains(DataExpressionColumns.TimePeriodEndDate))
                {
                    //-- Add expression column and set values 
                    this.AddStartDateEndDateColumnsToDataTable(dtPageData);
                    this.SetStartDateAndEndDateColumnValues(dtPageData);
                }

                //-- Set Metadata text in the main data table based on relationship
                this.SetMetadataColumnValues(dtPageData);

                //-- Set IC Column Values in the main data table based on relationship
                this.SetIUSICInfoColumnValues(dtPageData);

                //-- Set MaxLength for memo fields. In case of mysql Maxlength for memo fields is set to 0
                //-- value 536870910 is set for memo field in case of MS Acess so same value is set here in case of MySql
                dtPageData.Columns[Data.DataValue].MaxLength = 536870910; //TODO Maxlength for Datavalue can be more 536870910
                dtPageData.Columns[IndicatorClassifications.ICName].MaxLength = 536870910;
                this._Sources.Columns[IndicatorClassifications.ICName].MaxLength = 536870910; //536870910
                //Other Memo Fields - IC_Info, Notes.Note 

                #endregion
            }

            return dtPageData;
        }


        /// <summary>
        /// Fetch data for desired page
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        private DataView GetPageData(int pageNumber, bool isBackgroundProcess)
        {

            DataTable dtPageData = null;
            DataView retVal;

            // If data for this page has been already fetched then retrieve records for desired page from main datatable
            if (mbPageDataStatus[pageNumber - 1])
            {
                DataView dvMainData = this._MainDataTable.DefaultView;

                //Get the First and last record count for the page
                int _FirstRecord = (pageNumber - 1) * this.mUserPreference.General.PageSize;
                int _LastRecord = (pageNumber * this.mUserPreference.General.PageSize) - 1;
                if (_LastRecord >= this._RecordCount)
                {
                    _LastRecord = this._RecordCount - 1;
                }

                //Set table structure by adding individual column. Clone and XmlRead / Write doesnt work due to relationship
                dtPageData = new DataTable(this._MainDataTable.TableName);
                foreach (DataColumn dc in this._MainDataTable.Columns)
                {
                    dtPageData.Columns.Add(dc.ColumnName, dc.DataType);
                    dtPageData.Columns[dc.ColumnName].Caption = dc.Caption;
                }
                //Fetch records for page
                for (int i = _FirstRecord; i <= _LastRecord; i++)
                {
                    dtPageData.ImportRow(dvMainData[i].Row);
                }
            }
            // Else get page data and merge it with the main data table
            else
            {
                // -- Get page datanids
                string PageDataNIds = this.GetPageDataNIds(pageNumber);
                if (!string.IsNullOrEmpty(PageDataNIds))
                {
                    // Get complete Data on the basis of the DataNIDs
                    if (this.mdIConnection.ConnectionStringParameters.ServerType == DIServerType.MySql)
                    {
                        dtPageData = this.GetDataforDataNIDsMySql(PageDataNIds);
                    }
                    else
                    {
                        dtPageData = this.GetDataforDataNIDs(PageDataNIds);
                    }

                    if (this._MainDataTable.Rows.Count == 0)
                    {
                        // First Time - First Page
                        this._MainDataTable = dtPageData;
                        mbPageDataStatus[pageNumber - 1] = true;
                        // Set Column Header caption once only
                        this.ApplyLanguageSettings();
                        // -- Raise this event explicitly for first page.

                        if (this.PagingProgress != null)
                        {
                            this.PagingProgress(1, this._PageCount);
                        }
                    }
                    else
                    {
                        if (isBackgroundProcess)
                        {
                            // BACKGROUND PROCESS ONLY - merging into the main data table will happen only for the background process of data retrieval
                            if (this.mbCancellationPending)
                            {
                                this.mbCancellationPending = false;
                            }
                            else
                            {
                                try
                                {
                                    //-- dtPageData should not be merge with _MainDataTable in case of filter data paging 
                                    //  as _MainDataTable is alreday build
                                    if (this._MainDataTable.Rows.Count != this._RecordCount)
                                    {
                                        this._MainDataTable.Merge(dtPageData);
                                    }

                                    //Set status for paging completion
                                    mbPageDataStatus[pageNumber - 1] = true;
                                }
                                catch (Exception ex)
                                {
                                    //TODO Identify reason for thread abort exception
                                    System.Diagnostics.Debug.Print(ex.Message);
                                    dtPageData.Rows.Clear();
                                    //throw;
                                }
                            }
                        }
                        else
                        {
                            //-- Set language captions for dtPageData
                            foreach (DataColumn dc in this._MainDataTable.Columns)
                            {
                                try
                                {
                                    dtPageData.Columns[dc.ColumnName].Caption = dc.Caption;
                                }
                                catch (Exception ex)
                                {
                                    //-- End date column might not exist in dtPageData on clear of filter
                                    Console.Write(ex.Message);
                                }
                            }
                        }
                    }

                }
            }


            if (dtPageData == null)
            {
                retVal = null;
            }
            else
            {
                //-- Set selected column status for deleted records
                if (!string.IsNullOrEmpty(this.mUserPreference.UserSelection.DataViewFilters.DeletedDataNIds))
                {
                    DataRow[] DeletedRows = dtPageData.Select(Data.DataNId + " IN (" + this.mUserPreference.UserSelection.DataViewFilters.DeletedDataNIds + ")");
                    foreach (DataRow dr in DeletedRows)
                    {
                        dr[DataExpressionColumns.Selected] = false;
                    }
                }
                retVal = dtPageData.DefaultView;

                //-- Apply Sorting for page data. Calling ApplySort just set the order of datanid in this._FilteredDataNIds
                //-- for fetching page data, datanid's for a page are retrieved from _FilteredDataNIds, but the data fetched based on page datanids is not in sorted order
                retVal.Sort = this.GetSortString();
            }

            this._PagingCompleted = this.IsPagingCompleted();


            return retVal;
        }


        private bool IsPagingCompleted()
        {
            bool RetVal = true;

            try
            {
                for (int i = 0; i < this._PageCount; i++)
                {
                    if (mbPageDataStatus[i] == false)
                    {
                        RetVal = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.Print(ex.Message);
            }

            return RetVal;
        }

        #endregion

        #region Filters

        /// <summary>
        /// Get row filter string for Source, Unit, SubgroupVal, IUS DataValue and DataValue filters based on user selection
        /// </summary>
        /// <returns></returns>
        private string GetFilterString()
        {
            string retVal = string.Empty;

            #region Source Filter

            if (mUserPreference.UserSelection.DataViewFilters.DeletedSourceNIds.Trim().Length > 0)
            {
                if (mUserPreference.UserSelection.DataViewFilters.ShowSourceByIUS)
                {
                    retVal = Indicator_Unit_Subgroup.IUSNId + "_" + IndicatorClassifications.ICNId + " NOT IN (" + mUserPreference.UserSelection.DataViewFilters.DeletedSourceNIds + ")";
                }
                else
                {
                    retVal = Data.DataNId + " NOT IN (" + mUserPreference.UserSelection.DataViewFilters.DeletedSourceNIds + ")";
                    //retVal = IndicatorClassifications.ICNId + " NOT IN (" + mUserPreference.UserSelection.DataViewFilters.DeletedSourceNIds + ")";
                }
            }
            #endregion

            #region Recommended Source Filter

            if (mUserPreference.UserSelection.DataViewFilters.ShowRecommendedSourceByRank && mUserPreference.UserSelection.DataViewFilters.DeletedRanks.Trim().Length > 0)
            {
                if (retVal.Length > 0)
                {
                    retVal = retVal + " AND ";
                }
                retVal += Data.DataNId + " IN (" + this.GetDataNIdFromDeletedRank() + ")";
            }
            else if (!mUserPreference.UserSelection.DataViewFilters.ShowRecommendedSourceByRank && mUserPreference.UserSelection.DataViewFilters.DeletedRanks.Trim().Length > 0)
            {
                if (retVal.Length > 0)
                {
                    retVal = retVal + " AND ";
                }
                retVal += Data.DataNId + " IN (" + this.GetDataNIdFromDeletedLabels() + ")";
            }

            #endregion

            #region Unit Filter
            if (mUserPreference.UserSelection.DataViewFilters.DeletedUnitNIds.Trim().Length > 0)
            {
                if (retVal.Length > 0)
                {
                    retVal = retVal + " AND ";
                }

                if (mUserPreference.UserSelection.DataViewFilters.ShowUnitByIndicator)
                {
                    retVal += Indicator.IndicatorNId + "_" + Unit.UnitNId + " NOT IN (" + mUserPreference.UserSelection.DataViewFilters.DeletedUnitNIds + ")";
                }
                else
                {
                    retVal += Unit.UnitNId + " NOT IN (" + mUserPreference.UserSelection.DataViewFilters.DeletedUnitNIds + ")";
                }
            }
            #endregion

            #region SubgroupVal Filter
            if (mUserPreference.UserSelection.DataViewFilters.DeletedSubgroupNIds.Trim().Length > 0)
            {
                if (retVal.Length > 0)
                {
                    retVal = retVal + " AND ";
                }

                if (mUserPreference.UserSelection.DataViewFilters.ShowSubgroupByIndicator)
                {
                    retVal += Indicator.IndicatorNId + "_" + DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValNId + " NOT IN (" + mUserPreference.UserSelection.DataViewFilters.DeletedSubgroupNIds + ")";
                }
                else
                {
                    retVal += DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValNId + " NOT IN (" + mUserPreference.UserSelection.DataViewFilters.DeletedSubgroupNIds + ")";
                }
            }
            #endregion

            #region IUS DataValues Filters

            string IndicatorDataValueFiltersString = GetIndicatorDataValueFiltersString(this.mUserPreference.UserSelection.DataViewFilters.IndicatorDataValueFilters);
            if (IndicatorDataValueFiltersString.Length > 0)
            {
                if (retVal.Length > 0)
                {
                    retVal = retVal + " AND ";
                }
                retVal += IndicatorDataValueFiltersString;
            }

            #endregion

            #region DataValue Filter

            //-- Set invariant culture data values in filter clause as row filter will be set on an invariant dataview
            string CurrentThreadDecimalChar = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            string InvariantCultureDecimalChar = new System.Globalization.CultureInfo("en-US").NumberFormat.NumberDecimalSeparator;
            if (mUserPreference.UserSelection.DataViewFilters.DataValueFilter.OpertorType != DevInfo.Lib.DI_LibDAL.UserSelection.OpertorType.None)
            {

                if (retVal.Length > 0)
                {
                    retVal = retVal + " AND ";
                }
                switch (mUserPreference.UserSelection.DataViewFilters.DataValueFilter.OpertorType)
                {
                    case OpertorType.EqualTo:
                        retVal += DataExpressionColumns.NumericData + " = " + mUserPreference.UserSelection.DataViewFilters.DataValueFilter.FromDataValue.ToString().Replace(CurrentThreadDecimalChar, InvariantCultureDecimalChar);
                        break;
                    case OpertorType.Between:
                        //Inclusive of min & max values as per ANSI SQL
                        retVal += "(" + DataExpressionColumns.NumericData + " >= " + mUserPreference.UserSelection.DataViewFilters.DataValueFilter.FromDataValue.ToString().Replace(CurrentThreadDecimalChar, InvariantCultureDecimalChar) + " AND " + DataExpressionColumns.NumericData + " <= " + mUserPreference.UserSelection.DataViewFilters.DataValueFilter.ToDataValue.ToString().Replace(CurrentThreadDecimalChar, InvariantCultureDecimalChar) + ")";
                        break;
                    case OpertorType.GreaterThan:
                        retVal += "(" + DataExpressionColumns.NumericData + " > " + mUserPreference.UserSelection.DataViewFilters.DataValueFilter.FromDataValue.ToString().Replace(CurrentThreadDecimalChar, InvariantCultureDecimalChar) + ")";
                        break;
                    case OpertorType.LessThan:
                        retVal += "(" + DataExpressionColumns.NumericData + " < " + mUserPreference.UserSelection.DataViewFilters.DataValueFilter.ToDataValue.ToString().Replace(CurrentThreadDecimalChar, InvariantCultureDecimalChar) + ")";
                        break;
                }
            }
            #endregion

            #region Most Recent Data Filter - discard deleted records
            // Incase MRD Filter is to be applied discard deleted records also
            if (mUserPreference.UserSelection.DataViewFilters.MostRecentData == true)
            {
                if (mUserPreference.UserSelection.DataViewFilters.DeletedDataNIds.Length > 0)
                {
                    if (retVal.Length > 0)
                    {
                        retVal += " AND ";
                    }
                    retVal += Data.DataNId + " NOT IN (" + mUserPreference.UserSelection.DataViewFilters.DeletedDataNIds + ")";
                }
            }

            #endregion

            #region UltraWinGrid Filter
            string UltraWinGridFilterString = mUserPreference.UserSelection.DataViewFilters.UltraWinGridAutoFilters.GetUltraWinGridFilterString();
            if (UltraWinGridFilterString.Length > 0)
            {
                if (retVal.Length > 0)
                {
                    retVal = retVal + " AND ";
                }
                retVal += UltraWinGridFilterString;
            }
            #endregion

            return retVal;

        }

        /// <summary>
        /// Get DataNIds on the basis of deleted labels
        /// </summary>
        /// <returns></returns>
        private string GetDataNIdFromDeletedLabels()
        {
            string RetVal = string.Empty;
            try
            {
                IDataReader LabelReader = null;
                string[] DeletedLabels = new string[0];

                DeletedLabels = DICommon.SplitString(mUserPreference.UserSelection.DataViewFilters.DeletedRanks, Delimiter.TEXT_DELIMITER);
                foreach (string DeletedLabel in DeletedLabels)
                {
                    LabelReader = this.mdIConnection.ExecuteReader(this.mdIQueries.RecommendedSources.GetDataNIDsByLabel(DICommon.RemoveQuotes(DeletedLabel)));
                    while (LabelReader.Read())
                    {
                        RetVal += "," + LabelReader[Data.DataNId].ToString();
                    }
                    LabelReader.Close();
                }
                if (!string.IsNullOrEmpty(RetVal))
                {
                    RetVal = RetVal.Substring(1);
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        /// Get DataNIds on the basis of deleted ranks
        /// </summary>
        /// <returns></returns>
        private string GetDataNIdFromDeletedRank()
        {
            string RetVal = string.Empty;
            try
            {
                IDataReader RankReader = null;

                RankReader = this.mdIConnection.ExecuteReader(this.mdIQueries.RecommendedSources.GetDataNIDsByRank(mUserPreference.UserSelection.DataViewFilters.DeletedRanks));
                while (RankReader.Read())
                {
                    RetVal += "," + RankReader[Data.DataNId].ToString();
                }
                RankReader.Close();
                if (!string.IsNullOrEmpty(RetVal))
                {
                    RetVal = RetVal.Substring(1);
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        /// Get Filter string for IUS Filter based on IndicatorDataValueFilters collection
        /// </summary>
        /// <param name="indicatorDataValueFilters"></param>
        /// <returns></returns>
        private string GetIndicatorDataValueFiltersString(IndicatorDataValueFilters indicatorDataValueFilters)
        {
            string RetVal = string.Empty;
            string IndicatorNIdsWithoutFilters = string.Empty;

            //( (IUS.Indicator_NId = 74 AND (D.Data_value BETWEEN 0 AND 100)) OR  (IUS.Indicator_NId = 95 AND (D.Data_value >= 50)) OR IUS.Indicator_NId IN (60,70,80))
            //( (IUS.IUSNId = 93 AND (D.Data_value BETWEEN 0 AND 100)) OR  (IUS.IUSNId = 48 AND (D.Data_value >= 50)) OR IUS.IUSNID IN (60,70,80))

            if (indicatorDataValueFilters.Count > 0)
            {
                foreach (IndicatorDataValueFilter IndicatorDataValueFilter in indicatorDataValueFilters)
                {
                    //-- Collect the Indicator_NId having no data values
                    if (IndicatorDataValueFilter.OpertorType == OpertorType.None)
                    {
                        if (IndicatorNIdsWithoutFilters.Length == 0)
                        {
                            IndicatorNIdsWithoutFilters = IndicatorDataValueFilter.IndicatorNId.ToString();
                        }
                        else
                        {
                            IndicatorNIdsWithoutFilters += "," + IndicatorDataValueFilter.IndicatorNId.ToString();
                        }
                    }
                    else
                    {
                        // -- Create the SQL statement having datavalue
                        if (RetVal.Length == 0)
                        {
                            RetVal = "(" + GetIndicatorDataValueFilterString(IndicatorDataValueFilter, indicatorDataValueFilters.ShowIUS);
                        }
                        else
                        {
                            RetVal += " OR " + GetIndicatorDataValueFilterString(IndicatorDataValueFilter, indicatorDataValueFilters.ShowIUS);
                        }
                    }
                }
                RetVal = " (" + RetVal + ")";
            }
            // -- Insert the Indicator_NId at the end of SQL statement. These Indicator_NId have no data value.
            if (IndicatorNIdsWithoutFilters.Length > 0)
            {
                if (RetVal.Length > 0)
                {
                    RetVal += " OR ";
                }
                if (indicatorDataValueFilters.ShowIUS)
                {
                    RetVal += Indicator_Unit_Subgroup.IUSNId + " IN (" + IndicatorNIdsWithoutFilters + ")";
                }
                else
                {
                    RetVal += Indicator_Unit_Subgroup.IndicatorNId + " IN (" + IndicatorNIdsWithoutFilters + ")";
                }
            }
            // -- close the sql statement with ")"
            if (RetVal.Length > 0)
            {
                RetVal += ")";
            }

            return RetVal;
        }

        /// <summary>
        /// Get Get Filter string for IUS Filter for individual IndicatorDataValueFilter item of IndicatorDataValueFilters collection
        /// </summary>
        /// <param name="indicatorDataValueFilter"></param>
        /// <param name="showIUS"></param>
        /// <returns></returns>
        private string GetIndicatorDataValueFilterString(IndicatorDataValueFilter indicatorDataValueFilter, bool showIUS)
        {
            string RetVal = String.Empty;
            if (showIUS == true)
            {
                // Sample (IUS.IUSNId = 93 AND (D.Data_value BETWEEN 0 AND 100))
                RetVal = "(" + Indicator_Unit_Subgroup.IUSNId;
            }
            else
            {
                // Sample (IUS.Indicator_NId = 74 AND (D.Data_value BETWEEN 0 AND 100))
                RetVal = "(" + Indicator_Unit_Subgroup.IndicatorNId;
            }

            RetVal += " = " + indicatorDataValueFilter.IndicatorNId;

            //-- Set invariant culture data values in filter clause as row filter will be set on an invariant dataview
            string CurrentThreadDecimalChar = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            string InvariantCultureDecimalChar = new System.Globalization.CultureInfo("en-US").NumberFormat.NumberDecimalSeparator;
            switch (indicatorDataValueFilter.OpertorType)
            {
                case OpertorType.EqualTo:
                    RetVal += " AND " + DataExpressionColumns.NumericData + " = " + indicatorDataValueFilter.FromDataValue.ToString().Replace(CurrentThreadDecimalChar, InvariantCultureDecimalChar);
                    break;
                case OpertorType.Between:
                    //Inclusive of min & max values as per ANSI SQL
                    RetVal += " AND (" + DataExpressionColumns.NumericData + " >= " + indicatorDataValueFilter.FromDataValue.ToString().Replace(CurrentThreadDecimalChar, InvariantCultureDecimalChar) + " AND " + DataExpressionColumns.NumericData + " <= " + indicatorDataValueFilter.ToDataValue.ToString().Replace(CurrentThreadDecimalChar, InvariantCultureDecimalChar) + ")";
                    break;
                case OpertorType.GreaterThan:
                    RetVal += " AND (" + DataExpressionColumns.NumericData.ToString().Replace(CurrentThreadDecimalChar, InvariantCultureDecimalChar) + " > " + indicatorDataValueFilter.FromDataValue.ToString().Replace(CurrentThreadDecimalChar, InvariantCultureDecimalChar) + ")";
                    break;
                case OpertorType.LessThan:
                    RetVal += " AND (" + DataExpressionColumns.NumericData.ToString().Replace(CurrentThreadDecimalChar, InvariantCultureDecimalChar) + " < " + indicatorDataValueFilter.ToDataValue.ToString().Replace(CurrentThreadDecimalChar, InvariantCultureDecimalChar) + ")";
                    break;
            }

            RetVal += ")";

            return RetVal;
        }

        /// <summary>
        /// Apply Most Recent Data Filters
        /// </summary>
        /// <param name="dvData">Row Filter and Sort if any will be lost for dataview passed as argument</param>
        /// <returns></returns>
        /// <remarks>Record of latest timeperiod for each unique combination of IUSNId and AreaNId is considered</remarks>
        private DataView ApplyMRDFilter(DataView dvData)
        {
            // The most recent data is generated based upon IUSNId, AreaNId and TimePeriod.
            // Record of latest timeperiod for each unique combination is IUSNId and AreaNId is included.

            DataView RetVal = dvData;
            string AreaNId = string.Empty;
            string IUSNId = string.Empty;
            StringBuilder sbMRDDataNIDs = new StringBuilder();

            // Most Recent Data
            //sort dataview in decending order of timeperiod so that latest record can be obtained
            RetVal.Sort = Indicator_Unit_Subgroup.IUSNId + "," + Area.AreaNId + "," + Timeperiods.TimePeriod + " Desc";
            foreach (DataRow DRowParentTable in RetVal.ToTable().Rows)
            {
                // Get the record for latest timeperiod.
                if (AreaNId != DRowParentTable[Area.AreaNId].ToString() || IUSNId != DRowParentTable[Indicator_Unit_Subgroup.IUSNId].ToString())
                {
                    AreaNId = DRowParentTable[Area.AreaNId].ToString();
                    IUSNId = DRowParentTable[Indicator_Unit_Subgroup.IUSNId].ToString();
                    sbMRDDataNIDs.Append("," + DRowParentTable[Data.DataNId].ToString());
                }
            }
            if (sbMRDDataNIDs.Length > 0)
            {
                string sMRDDataNIDs = sbMRDDataNIDs.ToString().Substring(1);
                // Store the MRD Data NIDs into a module level variable
                msMRDDataNIDs = sMRDDataNIDs;
            }

            // Apply Row Filter on DataNIDs
            RetVal.RowFilter = Data.DataNId + " IN (" + msMRDDataNIDs + ")";
            RetVal = RetVal.ToTable(dvData.Table.TableName).DefaultView;
            return RetVal;
        }

        /// <summary>
        /// Set selected column status in all filter related master tables
        /// </summary>
        private void SetFilters()
        {

            #region Set Source Filters

            // Clear Source Filters
            this.ClearSourceFilter(); // Set selected column value to true

            if (mUserPreference.UserSelection.DataViewFilters.DeletedSourceNIds.Length > 0)
            {
                // Set Source Filters
                if (mUserPreference.UserSelection.DataViewFilters.ShowSourceByIUS)
                {
                    string FilterString = Indicator_Unit_Subgroup.IUSNId + " + '_' + " + IndicatorClassifications.ICNId + " IN (" + mUserPreference.UserSelection.DataViewFilters.DeletedSourceNIds + ")";
                    foreach (DataRow dr in this.IUSSource.Select(FilterString))
                    {
                        dr[DataExpressionColumns.Selected] = false;
                    }

                    FilterString = GetTransformedDeletedSourceNIDs(mUserPreference.UserSelection, this.IUSSource, this.Sources);
                    if (!string.IsNullOrEmpty(FilterString))
                    {
                        FilterString = IndicatorClassifications.ICNId + " IN (" + FilterString + ")";
                        foreach (DataRow dr in this._Sources.Select(FilterString))
                        {
                            dr[DataExpressionColumns.Selected] = false;
                        }
                    }
                }
                else
                {
                    //string FilterString = IndicatorClassifications.ICNId + " IN (" + mUserPreference.UserSelection.DataViewFilters.DeletedSourceNIds + ")";
                    //foreach (DataRow dr in this._Sources.Select(FilterString))
                    //{
                    //    dr[DataExpressionColumns.Selected] = false;
                    //}
                    //FilterString = GetTransformedDeletedSourceNIDs(mUserPreference.UserSelection, this.IUSSource, this.Sources);
                    //if (!string.IsNullOrEmpty(FilterString))
                    //{
                    //    FilterString = Indicator_Unit_Subgroup.IUSNId + " + '_' + " + IndicatorClassifications.ICNId + " IN (" + FilterString + ")";
                    //    foreach (DataRow dr in this._IUSSource.Select(FilterString))
                    //    {
                    //        dr[DataExpressionColumns.Selected] = false;
                    //    }
                    //}
                }
            }

            #endregion

            #region Set Unit Filters

            // Clear Unit Filters
            this.ClearUnitFilter(); // Set selected column value to true

            if (mUserPreference.UserSelection.DataViewFilters.DeletedUnitNIds.Length > 0)
            {
                // Set Unit Filters
                if (mUserPreference.UserSelection.DataViewFilters.ShowUnitByIndicator)
                {
                    string FilterString = Indicator.IndicatorNId + " + '_' + " + Unit.UnitNId + " IN (" + mUserPreference.UserSelection.DataViewFilters.DeletedUnitNIds + ")";
                    foreach (DataRow dr in this.IndicatorUnit.Select(FilterString)) // Use property instead of private variable 
                    {
                        dr[DataExpressionColumns.Selected] = false;
                    }

                    FilterString = GetTransformedDeletedUnitNIDs(mUserPreference.UserSelection);
                    if (!string.IsNullOrEmpty(FilterString))
                    {
                        FilterString = Unit.UnitNId + " IN (" + FilterString + ")";
                        foreach (DataRow dr in this._Units.Select(FilterString))
                        {
                            dr[DataExpressionColumns.Selected] = false;
                        }
                    }
                }
                else
                {
                    string FilterString = Unit.UnitNId + " IN (" + mUserPreference.UserSelection.DataViewFilters.DeletedUnitNIds + ")";
                    foreach (DataRow dr in this.Units.Select(FilterString)) // Use property instead of private variable 
                    {
                        dr[DataExpressionColumns.Selected] = false;
                    }
                    FilterString = GetTransformedDeletedUnitNIDs(mUserPreference.UserSelection);
                    if (!string.IsNullOrEmpty(FilterString))
                    {
                        FilterString = Indicator.IndicatorNId + " + '_' + " + Unit.UnitNId + " IN (" + FilterString + ")";
                        foreach (DataRow dr in this._IndicatorUnit.Select(FilterString))
                        {
                            dr[DataExpressionColumns.Selected] = false;
                        }
                    }
                }
            }

            #endregion

            #region Set SubgroupVal Filters

            // Clear SubgroupVal Filters
            this.ClearSubgroupValFilter(); // Set selected column value to true

            if (mUserPreference.UserSelection.DataViewFilters.DeletedSubgroupNIds.Length > 0)
            {
                // Set SubgroupVal Filters
                if (mUserPreference.UserSelection.DataViewFilters.ShowSubgroupByIndicator)
                {
                    string FilterString = Indicator.IndicatorNId + " + '_' + " + DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValNId + " IN (" + mUserPreference.UserSelection.DataViewFilters.DeletedSubgroupNIds + ")";
                    foreach (DataRow dr in this.IndicatorSubgroupVal.Select(FilterString))// Use property instead of private variable 
                    {
                        dr[DataExpressionColumns.Selected] = false;
                    }

                    FilterString = GetTransformedDeletedSubgroupValNIDs(mUserPreference.UserSelection);
                    if (!string.IsNullOrEmpty(FilterString))
                    {
                        FilterString = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValNId + " IN (" + FilterString + ")";
                        foreach (DataRow dr in this._SubgroupVals.Select(FilterString))
                        {
                            dr[DataExpressionColumns.Selected] = false;
                        }
                    }
                }
                else
                {
                    string FilterString = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValNId + " IN (" + mUserPreference.UserSelection.DataViewFilters.DeletedSubgroupNIds + ")";
                    foreach (DataRow dr in this._SubgroupVals.Select(FilterString)) // Use property instead of private variable 
                    {
                        dr[DataExpressionColumns.Selected] = false;
                    }
                    FilterString = GetTransformedDeletedSubgroupValNIDs(mUserPreference.UserSelection);
                    if (!string.IsNullOrEmpty(FilterString))
                    {
                        FilterString = Indicator.IndicatorNId + " + '_' + " + DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValNId + " IN (" + FilterString + ")";
                        foreach (DataRow dr in this._IndicatorSubgroupVal.Select(FilterString))
                        {
                            dr[DataExpressionColumns.Selected] = false;
                        }
                    }
                }
            }



            #endregion
        }

        /// <summary>
        /// Set selected column status to true in all  filter related master tables
        /// </summary>
        private void ClearFilters()
        {
            this.ClearDeletedDataPointFilter();
            this.ClearSourceFilter();
            this.ClearUnitFilter();
            this.ClearSubgroupValFilter();
        }

        /// <summary>
        /// Clear deleted data point filter
        /// </summary>
        private void ClearDeletedDataPointFilter()
        {
            // As all deleted data nids are reset update Selected column value to true for all records
            // In no record found case Selected expression column might not exist so check for column existance
            if (this._MainDataTable.Columns.Contains(DataExpressionColumns.Selected))
            {
                //setting expression makes column readonly and its value cant be set externally
                this._MainDataTable.Columns[DataExpressionColumns.Selected].Expression = "TRUE";
            }
        }

        /// <summary>
        /// Clear Source Filters in IUSSource and Sources data table. Set the selected column value to true
        /// </summary>
        private void ClearSourceFilter()
        {
            if (this._IUSSource.Columns.Contains(DataExpressionColumns.Selected))
            {
                foreach (DataRow dr in this._IUSSource.Rows)
                {
                    dr[DataExpressionColumns.Selected] = true;
                }
            }
            if (this._Sources.Columns.Contains(DataExpressionColumns.Selected))
            {
                foreach (DataRow dr in this._Sources.Rows)
                {
                    dr[DataExpressionColumns.Selected] = true;
                }
            }
        }

        /// <summary>
        /// Clear Unit Filters in IndicatorUnit and Units data table. Set the selected column value to true
        /// </summary>
        private void ClearUnitFilter()
        {
            if (this._IndicatorUnit.Columns.Contains(DataExpressionColumns.Selected))
            {
                foreach (DataRow dr in this._IndicatorUnit.Rows)
                {
                    dr[DataExpressionColumns.Selected] = true;
                }
            }
            if (this._Units.Columns.Contains(DataExpressionColumns.Selected))
            {
                foreach (DataRow dr in this._Units.Rows)
                {
                    dr[DataExpressionColumns.Selected] = true;
                }
            }
        }

        /// <summary>
        /// Clear SubgroupVal Filters in IndicatorSubgroupVal and SubgroupVals data table. Set the selected column value to true
        /// </summary>
        private void ClearSubgroupValFilter()
        {
            if (this._IndicatorSubgroupVal.Columns.Contains(DataExpressionColumns.Selected))
            {
                foreach (DataRow dr in this._IndicatorSubgroupVal.Rows)
                {
                    dr[DataExpressionColumns.Selected] = true;
                }
            }
            if (this._SubgroupVals.Columns.Contains(DataExpressionColumns.Selected))
            {
                foreach (DataRow dr in this._SubgroupVals.Rows)
                {
                    dr[DataExpressionColumns.Selected] = true;
                }
            }
        }

        #endregion

        #region UserSelectionChanged

        private void UserSelectionChanged()
        {
            if (this.PresentationData != null)
            {
                this.PresentationData.Dispose();
            }
            this.PresentationData = null;

            if (this.MRDData != null)
            {
                this.MRDData.Dispose();
            }
            this.MRDData = null;
        }

        #endregion

        #endregion

        #endregion

        #region Public

        #region -- Constant --

        /// <summary>
        /// Constant for the name of the boolean data column, used in marking records for deletion, in dataview
        /// </summary>
        //public const string SELECTION_COLUMN_NAME = "Selected";
        public const string MASK_FILE_INDICATOR = "IndMask.xml";
        public const string MASK_FILE_AREA = "MapMask.xml";
        public const string MASK_FILE_SOURCE = "SrcMask.xml";

        #endregion

        #region -- Constructor --

        /// <summary>
        /// Constructor for DIDataView builds up the primary information for getting paged data
        /// </summary>
        /// <param name="userPreference"></param>
        /// <param name="dIConnection"></param>
        /// <param name="dIQueries"></param>
        /// <param name="maskFolder">Path of Mask Folder where mask files for Indicator, Area and Source metadata resides. Constants for mask file name are available within this class</param>
        /// <param name="languageFilePath">Path of language file, used to set language based column captions</param>
        public DIDataView(UserPreference.UserPreference userPreference, DI_LibDAL.Connection.DIConnection dIConnection, DI_LibDAL.Queries.DIQueries dIQueries, string maskFolder, string commentsDataNIds)
        {

            string _Query = string.Empty;
            string[] DistinctColumns = new string[1];

            // -- Connection details preserved for getting IUSSource (recommended source) later on demand
            this.mdIConnection = dIConnection;
            this.mdIQueries = dIQueries;
            this.mUserPreference = userPreference;
            this.mUserPreference.UserSelection.UserSelectionChangeEvent += UserSelectionChanged;

            // Mask folder for Metadata
            this.msMaskFolder = maskFolder;

            this._PagingCompleted = false;

            #region STEP 1: Get DataNIDs for the selected Indicator, Time and Area and Build the Master Tables

            // -- STEP 1: Get DataNIDs for the selected Indicator, Time and Area 
            // -- STEP 1.1 - Build Query for the Data View - This query will return only the DataNIds on the basis of the selected I, A and T 
            // Query for Data NIDs

            //We have to get data for records which are marked as deleted and still to be displayed at Data page
            if (string.IsNullOrEmpty(commentsDataNIds))
            {
                _Query = dIQueries.Data.GetDataNIDByIUSTimePeriodAreaSource(userPreference.UserSelection, dIConnection.ConnectionStringParameters.ServerType, "");
            }
            else
            {
                _Query = dIQueries.Data.GetDataNIDByIUSTimePeriodAreaSource(userPreference.UserSelection, dIConnection.ConnectionStringParameters.ServerType, commentsDataNIds);
            }

            // -- STEP 1.2: Execute the Query 
            // -- This Data Table will have all the DataNIDs and wil be used in other operations to fetch data 
            DataTable dtData = this.ExecuteInvariantDataTable(_Query);

            // -- STEP 1.3: Inititalize Data Tables
            InitializeVariables();

            // -- STEP 1.4: Creating the Data Tables for 
            //          DataNID, IUSNId and IUS_SourceNId (These Datatables will be used in the DataView Page to fill the Data, Source, Unit and Subgroup Lists)
            //          Indicator (master table based on IUSNId)
            //          Unit (master table based on IUSNId)
            //          Subgroup (master table based on IUSNId)
            //          Source (master table based on IUS_SourceNId)
            if (dtData != null && dtData.Rows.Count > 0)
            {
                // Distinct DataNId
                DistinctColumns[0] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.DataNId;
                this._AllDataNIDs = dtData.DefaultView.ToTable(false, DistinctColumns);
                this._FilteredDataNIds = this._AllDataNIDs.Copy();

                StringBuilder sbDataNIDs = new StringBuilder();
                for (int i = 0; i < this._AllDataNIDs.Rows.Count; i++)
                {
                    sbDataNIDs.Append("," + this._AllDataNIDs.Rows[i][Data.DataNId]);
                }
                if (sbDataNIDs.Length > 0)
                {
                    msAllDataNIDs = sbDataNIDs.ToString().Substring(1);
                }

                // Distinct IUSNID
                DistinctColumns[0] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.IUSNId;
                DataTable dtIUSNIDs = GetInvariantDataTable();
                dtIUSNIDs = dtData.DefaultView.ToTable(true, DistinctColumns);
                StringBuilder sbIUSNIDs = new StringBuilder();
                this._IUSNIds = string.Empty;
                for (int i = 0; i < dtIUSNIDs.Rows.Count; i++)
                {
                    sbIUSNIDs.Append("," + dtIUSNIDs.Rows[i][Data.IUSNId]);
                }
                if (sbIUSNIDs.Length > 0)
                {
                    this._IUSNIds = sbIUSNIDs.ToString().Substring(1);
                }


                // Distinct AreaNIds
                DistinctColumns[0] = Data.AreaNId;
                DataTable dtAreaNIds = GetInvariantDataTable();
                dtAreaNIds = dtData.DefaultView.ToTable(true, DistinctColumns);
                StringBuilder sbAreaNIds = new StringBuilder();
                this._AreaNIds = string.Empty;
                for (int i = 0; i < dtAreaNIds.Rows.Count; i++)
                {
                    sbAreaNIds.Append("," + dtAreaNIds.Rows[i][Data.AreaNId]);
                }
                if (sbAreaNIds.Length > 0)
                {
                    this._AreaNIds = sbAreaNIds.ToString().Substring(1);
                }

                // Distinct TimePeriodNIds
                DistinctColumns[0] = Data.TimePeriodNId;
                DataTable dtTimePeriodNIds = GetInvariantDataTable();
                dtTimePeriodNIds = dtData.DefaultView.ToTable(true, DistinctColumns);
                StringBuilder sbTimePeriodNIds = new StringBuilder();
                this._TimePeriodNIds = string.Empty;
                for (int i = 0; i < dtTimePeriodNIds.Rows.Count; i++)
                {
                    sbTimePeriodNIds.Append("," + dtTimePeriodNIds.Rows[i][Data.TimePeriodNId]);
                }
                if (sbTimePeriodNIds.Length > 0)
                {
                    this._TimePeriodNIds = sbTimePeriodNIds.ToString().Substring(1);
                }

                // Distinct SourceNIDs
                string[] SourceDistinctColumns = new string[2];
                SourceDistinctColumns[0] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.IUSNId;
                SourceDistinctColumns[1] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.SourceNId;
                this.mdtIUSSourceNIDs = dtData.DefaultView.ToTable(true, SourceDistinctColumns);

                // Build IUSIndicator table
                this.BuildIUSIndicator(dIConnection, dIQueries);
                // Build IUSUnit table
                this.BuildIUSUnit(dIConnection, dIQueries);
                // Build IUSSubgroup 
                this.BuildIUSSubgroup(dIConnection, dIQueries);
                // Build Source
                this.BuildSource(dIConnection, dIQueries);
                //Build IUSSource
                this.GetIUSSource(dIConnection, dIQueries);

                // BuildMetadataTables
                this.BuildMetadataTables(userPreference, dIConnection, dIQueries);
                // BuildIUSICInfoTables
                this.BuildIUSICInfoTable(userPreference, dIConnection, dIQueries, dtIUSNIDs);

                // Set Paging Info
                SetPagingInfo(this._AllDataNIDs.Rows.Count);

            }
            #endregion

        }

        #endregion

        #region  -- Event --


        /// <summary>
        /// Event for progress bar
        /// </summary>
        public event PagingProgressDelegate PagingProgress;

        #endregion

        #region -- Properties --

        //
        private string _IUSNIds = string.Empty;
        /// <summary>
        /// Comma delimited string of distinct IUSNIds in whole dataview
        /// </summary>
        public string IUSNIds
        {
            get
            {
                return _IUSNIds;
            }
        }

        private string _AreaNIds = string.Empty;
        /// <summary>
        /// Comma delimited string of distinct AreaNIds in whole dataview
        /// </summary>
        public string AreaNIds
        {
            get
            {
                return _AreaNIds;
            }
        }

        private string _TimePeriodNIds = string.Empty;
        /// <summary>
        /// Comma delimited string of distinct TimePeriodNIds in whole dataview
        /// </summary>
        public string TimePeriodNIds
        {
            get
            {
                return _TimePeriodNIds;
            }
        }

        private DataTable _AllDataNIDs = GetInvariantDataTable();
        /// <summary>
        /// Get a single column (Data_NId) table containing all DataNIds based on user selection and irrespective of paging
        /// </summary>
        public DataTable AllDataNIDs
        {
            get
            {
                return _AllDataNIDs;
            }
        }

        private DataTable _FilteredDataNIds = GetInvariantDataTable();
        /// <summary>
        /// All Filtered Data NIDs based on Indicator, TimePeriod and Area Selection and applying DataView filters
        /// </summary>
        public DataTable FilteredDataNIds
        {
            get
            {
                return _FilteredDataNIds;
            }
        }

        private DataTable _MainDataTable = GetInvariantDataTable();
        /// <summary>
        /// Main datatable containing all records. Pages records appended through background process
        /// </summary>
        public DataTable MainDataTable
        {
            get
            {
                return this._MainDataTable;
            }
        }

        private DataTable _IUSIndicator = GetInvariantDataTable();
        /// <summary>
        /// Get datatable for IUSNId and Indicator columns    
        /// </summary>
        public DataTable IUSIndicator
        {
            get { return _IUSIndicator; }
        }

        private DataTable _IUSUnit = GetInvariantDataTable();
        /// <summary>
        /// Get datatable for IUSNId and Unit columns        
        /// </summary>
        public DataTable IUSUnit
        {
            get { return _IUSUnit; }
        }

        private DataTable _IndicatorUnit = GetInvariantDataTable();
        /// <summary>
        /// Get datatable containing distinct Indicator Unit columns   
        /// </summary>
        public DataTable IndicatorUnit
        {
            get
            {

                if (this._IndicatorUnit.Rows.Count == 0)
                {

                    //Make a copy of _IUSUnit as it may already be added to some other dataset relationship (GetIUSSource)
                    DataTable dtIUSUnitCopy = this._IUSUnit.Copy();

                    this._IndicatorUnit = this._IUSIndicator.Copy();
                    this._IndicatorUnit.Columns.Add(Unit.UnitNId, typeof(int));
                    this._IndicatorUnit.Columns.Add(Unit.UnitName, typeof(string));
                    this._IndicatorUnit.Columns.Add(Unit.UnitGlobal, typeof(bool));

                    DataSet _Base = GetInvariantDataSet();
                    _Base.Tables.Add(this._IndicatorUnit);
                    _Base.Tables.Add(dtIUSUnitCopy);
                    _Base.Relations.Add("RelIUSUnit_IndicatorUnit", dtIUSUnitCopy.Columns[Indicator_Unit_Subgroup.IUSNId], this._IndicatorUnit.Columns[Indicator_Unit_Subgroup.IUSNId], false);
                    // add expression on Indicator Column
                    this._IndicatorUnit.Columns[Unit.UnitNId].Expression = "parent(RelIUSUnit_IndicatorUnit)." + Unit.UnitNId;
                    this._IndicatorUnit.Columns[Unit.UnitName].Expression = "parent(RelIUSUnit_IndicatorUnit)." + Unit.UnitName;
                    this._IndicatorUnit.Columns[Unit.UnitGlobal].Expression = "parent(RelIUSUnit_IndicatorUnit)." + Unit.UnitGlobal;
                    _Base.AcceptChanges();

                    string[] IndicatorUnitDistinctColumns = new string[6];
                    IndicatorUnitDistinctColumns[0] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId;
                    IndicatorUnitDistinctColumns[1] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorName;
                    IndicatorUnitDistinctColumns[2] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGlobal;
                    IndicatorUnitDistinctColumns[3] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitNId;
                    IndicatorUnitDistinctColumns[4] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitName;
                    IndicatorUnitDistinctColumns[5] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitGlobal;
                    this._IndicatorUnit = this._IndicatorUnit.DefaultView.ToTable(true, IndicatorUnitDistinctColumns);

                    // Add selected column
                    this._IndicatorUnit.Columns.Add(DataExpressionColumns.Selected, typeof(bool));
                    foreach (DataRow dr in this._IndicatorUnit.Rows)
                    {
                        dr[DataExpressionColumns.Selected] = true;
                    }

                }
                return this._IndicatorUnit;
            }
        }

        private DataTable _Units = GetInvariantDataTable();
        /// <summary>
        /// Get master datatable for units
        /// </summary>
        public DataTable Units
        {
            get
            {
                if (this._Units.Rows.Count == 0)
                {
                    string[] UnitDistinctColumns = new string[4];
                    UnitDistinctColumns[0] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitNId;
                    UnitDistinctColumns[1] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitName;
                    UnitDistinctColumns[2] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitGlobal;
                    UnitDistinctColumns[3] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Unit.UnitGId;
                    this._Units = this._IUSUnit.DefaultView.ToTable(true, UnitDistinctColumns);

                    // Add selected column
                    this._Units.Columns.Add(DataExpressionColumns.Selected, typeof(bool));
                    foreach (DataRow dr in this._Units.Rows)
                    {
                        dr[DataExpressionColumns.Selected] = true;
                    }
                }
                return this._Units;
            }
        }

        private DataTable _IUSSubgroupVal = GetInvariantDataTable();
        /// <summary>
        ///  Get datatable for IUSNId and SubgroupVal columns     
        /// </summary>
        public DataTable IUSSubgroupVal
        {
            get { return _IUSSubgroupVal; }
        }

        private DataTable _IndicatorSubgroupVal = GetInvariantDataTable();
        /// <summary>
        /// Get datatable containing distinct Indicator SubgroupVal columns    
        /// </summary>
        public DataTable IndicatorSubgroupVal
        {
            get
            {

                if (this._IndicatorSubgroupVal.Rows.Count == 0)
                {

                    //Make a copy of _IUSSubgroupVal as it may already be added to some other dataset relationship (GetIUSSource)
                    DataTable dtIUSSubgroupVal = this._IUSSubgroupVal.Copy();

                    this._IndicatorSubgroupVal = this._IUSIndicator.Copy();
                    this._IndicatorSubgroupVal.Columns.Add(DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValNId, typeof(int));
                    this._IndicatorSubgroupVal.Columns.Add(DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupVal, typeof(string));
                    this._IndicatorSubgroupVal.Columns.Add(DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGlobal, typeof(bool));

                    DataSet _Base = GetInvariantDataSet();
                    _Base.Tables.Add(this._IndicatorSubgroupVal);
                    _Base.Tables.Add(dtIUSSubgroupVal);
                    _Base.Relations.Add("RelIUSUnit_IndicatorSubgroupVal", dtIUSSubgroupVal.Columns[Indicator_Unit_Subgroup.IUSNId], this._IndicatorSubgroupVal.Columns[Indicator_Unit_Subgroup.IUSNId], false);
                    // add expression on Indicator Column
                    this._IndicatorSubgroupVal.Columns[DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValNId].Expression = "parent(RelIUSUnit_IndicatorSubgroupVal)." + DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValNId;
                    this._IndicatorSubgroupVal.Columns[DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupVal].Expression = "parent(RelIUSUnit_IndicatorSubgroupVal)." + DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupVal;
                    this._IndicatorSubgroupVal.Columns[DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGlobal].Expression = "parent(RelIUSUnit_IndicatorSubgroupVal)." + DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGlobal;
                    _Base.AcceptChanges();

                    string[] IndicatorSubgroupValDistinctColumns = new string[6];
                    IndicatorSubgroupValDistinctColumns[0] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorNId;
                    IndicatorSubgroupValDistinctColumns[1] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorName;
                    IndicatorSubgroupValDistinctColumns[2] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Indicator.IndicatorGlobal;
                    IndicatorSubgroupValDistinctColumns[3] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValNId;
                    IndicatorSubgroupValDistinctColumns[4] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupVal;
                    IndicatorSubgroupValDistinctColumns[5] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGlobal;
                    this._IndicatorSubgroupVal = this._IndicatorSubgroupVal.DefaultView.ToTable(true, IndicatorSubgroupValDistinctColumns);

                    // Add selected column
                    this._IndicatorSubgroupVal.Columns.Add(DataExpressionColumns.Selected, typeof(bool));
                    foreach (DataRow dr in this._IndicatorSubgroupVal.Rows)
                    {
                        dr[DataExpressionColumns.Selected] = true;
                    }
                }
                return this._IndicatorSubgroupVal;
            }
        }

        private DataTable _SubgroupVals = GetInvariantDataTable();
        /// <summary>
        /// Get master datatable for SubgroupVal 
        /// </summary>
        public DataTable SubgroupVals
        {
            get
            {

                if (this._SubgroupVals.Rows.Count == 0)
                {
                    string[] SubgroupValDistinctColumns = new string[5];
                    SubgroupValDistinctColumns[0] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValNId;
                    SubgroupValDistinctColumns[1] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupVal;
                    SubgroupValDistinctColumns[2] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGlobal;
                    SubgroupValDistinctColumns[3] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGId;
                    SubgroupValDistinctColumns[4] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValOrder;
                    this._SubgroupVals = this._IUSSubgroupVal.DefaultView.ToTable(true, SubgroupValDistinctColumns);

                    // Add selected column
                    this._SubgroupVals.Columns.Add(DataExpressionColumns.Selected, typeof(bool));
                    foreach (DataRow dr in this._SubgroupVals.Rows)
                    {
                        dr[DataExpressionColumns.Selected] = true;
                    }
                }
                return this._SubgroupVals;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private bool _SubgroupInfoCheckedAndNotFound = false;

        private DataTable _SubgroupInfo = GetInvariantDataTable();
        /// <summary>
        /// Get master datatable for SubgroupVal 
        /// </summary>
        public DataTable SubgroupInfo
        {
            get
            {

                if (this._SubgroupInfo.Rows.Count == 0 && this._SubgroupInfoCheckedAndNotFound == false)
                {

                    // Add Columns to SubgroupInfo Table
                    if (this._SubgroupInfo.Columns.Count == 0)
                    {
                        this._SubgroupInfo.Columns.Add(SubgroupInfoColumns.NId, typeof(string));
                        this._SubgroupInfo.Columns.Add(SubgroupInfoColumns.Name, typeof(string));
                        this._SubgroupInfo.Columns.Add(SubgroupInfoColumns.GId, typeof(string));
                        this._SubgroupInfo.Columns.Add(SubgroupInfoColumns.Global, typeof(string));
                        this._SubgroupInfo.Columns.Add(SubgroupInfoColumns.Type, typeof(int));
                        this._SubgroupInfo.Columns.Add(SubgroupInfoColumns.Order, typeof(int));
                        this._SubgroupInfo.Columns.Add(SubgroupInfoColumns.Caption, typeof(string));
                        this._SubgroupInfo.Columns.Add(SubgroupInfoColumns.GIdValue, typeof(string));
                        this._SubgroupInfo.Columns.Add(SubgroupInfoColumns.OrderColName, typeof(string));
                    }


                    // Add rows to SubgroupInfo Table 
                    string SubgroupValNIds = string.Empty;
                    StringBuilder sbSubgroupValNIds = new StringBuilder();
                    foreach (DataRow dr in this.SubgroupVals.Rows)
                    {
                        sbSubgroupValNIds.Append("," + dr[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValNId].ToString());
                    }

                    //TODO There might be cases when Subgroup_Vals_Subgroup may not contain records against given SubgroupValNId
                    // This may happen when subgroups are generated through xls import

                    if (sbSubgroupValNIds.Length > 0)
                    {
                        string sSql = this.mdIQueries.SubgroupTypes.GetSubgroupTypes(sbSubgroupValNIds.ToString().Substring(1));
                        DataTable dtSubgroupTypes = this.mdIConnection.ExecuteDataTable(sSql);
                        string ValidColumnName = string.Empty;
                        foreach (DataRow dr in dtSubgroupTypes.Rows)
                        {
                            DataRow drSubgroupInfo = this._SubgroupInfo.NewRow();
                            //-- remove any spaces apostrophe or comma in field name 
                            ValidColumnName = GetValidColumnName(GetValidColumnName(dr[SubgroupTypes.SubgroupTypeName].ToString()));
                            drSubgroupInfo[SubgroupInfoColumns.NId] = "Subgroup_" + ValidColumnName + "_NId";
                            drSubgroupInfo[SubgroupInfoColumns.Name] = "Subgroup_" + ValidColumnName + "_Name";
                            drSubgroupInfo[SubgroupInfoColumns.GId] = "Subgroup_" + ValidColumnName + "_GId";
                            drSubgroupInfo[SubgroupInfoColumns.Global] = "Subgroup_" + ValidColumnName + "_Global";
                            drSubgroupInfo[SubgroupInfoColumns.OrderColName] = "Subgroup_" + ValidColumnName + "_Order";
                            drSubgroupInfo[SubgroupInfoColumns.Type] = dr[SubgroupTypes.SubgroupTypeNId];
                            drSubgroupInfo[SubgroupInfoColumns.Order] = dr[SubgroupTypes.SubgroupTypeOrder];
                            drSubgroupInfo[SubgroupInfoColumns.Caption] = dr[SubgroupTypes.SubgroupTypeName];
                            drSubgroupInfo[SubgroupInfoColumns.GIdValue] = dr[SubgroupTypes.SubgroupTypeGID];

                            this._SubgroupInfo.Rows.Add(drSubgroupInfo);
                        }
                    }

                    if (this._SubgroupInfo.Rows.Count == 0)
                    {
                        this._SubgroupInfoCheckedAndNotFound = true;
                    }
                }
                return this._SubgroupInfo;
            }
        }

        private DataTable _IUSSource = GetInvariantDataTable();
        /// <summary>
        /// Get a datatable containing  all columns related to Source, IUS, Indicator, Unit, SubgroupVal and Recommended source information
        /// </summary>
        /// <remarks>Used for source filters and applying colors to rows with recommended sources in dataview</remarks>
        public DataTable IUSSource
        {
            get
            {
                if (_IUSSource.Rows.Count == 0)
                {
                    this.GetIUSSource(this.mdIConnection, this.mdIQueries);
                }
                return _IUSSource;
            }
        }

        private DataTable _Sources = GetInvariantDataTable();
        /// <summary>
        /// Get master datatable for Source    
        /// </summary>
        public DataTable Sources
        {
            get { return _Sources; }
        }

        private DataTable _MetadataIndicator = GetInvariantDataTable();
        /// <summary>
        /// Indicator Metadata Table. Contains information for selected metadata fields of all indicators in dataview     
        /// </summary>
        public DataTable MetadataIndicator
        {
            get { return _MetadataIndicator; }
        }

        private DataTable _MetadataArea = GetInvariantDataTable();
        /// <summary>
        /// Area Metadata Table. Contains information for selected metadata fields of all areas (Associated Map) in dataview    
        /// </summary>
        public DataTable MetadataArea
        {
            get { return _MetadataArea; }
        }

        private DataTable _MetadataSource = GetInvariantDataTable();
        /// <summary>
        /// Source Metadata Table. Contains information for selected metadata fields of all sources in dataview     
        /// </summary>
        public DataTable MetadataSource
        {
            get { return _MetadataSource; }
        }

        private DataTable _IUSICInfo = GetInvariantDataTable();
        /// <summary>
        /// IUS - ICName Table.  Contains IC Name for selected IC fields against all IUS in dataview
        /// </summary>
        public DataTable IUSICInfo
        {
            get { return _IUSICInfo; }
        }

        private DataTable _IUSICNIdInfo = GetInvariantDataTable();
        /// <summary>
        /// IUS - ICNId Table.  Contains ICNIds for selected IC fields against all IUS in dataview
        /// </summary>
        /// <remarks>Required  </remarks>
        public DataTable IUSICNIdInfo
        {
            get { return _IUSICNIdInfo; }
        }

        private int _RecordCount;
        /// <summary>
        /// Total record count after applying all filters (excluding unselected record filter)   
        /// </summary>
        public int RecordCount
        {
            get { return _RecordCount; }
        }

        private int _PresentationRecordCount;
        /// <summary>
        /// Total record count after applying all filters (including unselected record filter)
        /// </summary>
        public int PresentationRecordCount
        {
            get
            {
                //-- Set record count based on records marked for deletion
                if (string.IsNullOrEmpty(this.mUserPreference.UserSelection.DataViewFilters.DeletedDataNIds))
                {
                    _PresentationRecordCount = this._RecordCount;
                }
                else
                {
                    string[] ArrDeletedNId = this.mUserPreference.UserSelection.DataViewFilters.DeletedDataNIds.Split(",".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
                    //-- Record count to displayed = Total Record Count - Records marked for deletion
                    //-- Case when few / all records are marked for deletion and then filter is applied then PresentationRecordCount may result into negetive values
                    _PresentationRecordCount = Math.Max(0, this._RecordCount - ArrDeletedNId.Length);


                }
                return _PresentationRecordCount;
            }
        }

        private int _PageCount;
        /// <summary>
        /// Total number of pages required based on page size and total record count
        /// </summary>
        public int PageCount
        {
            get { return _PageCount; }
        }

        private bool _PagingCompleted;

        /// <summary>
        /// Get paging completion status. Used in web. May use paging event where possible
        /// </summary>
        public bool PagingCompleted
        {
            get { return _PagingCompleted; }
        }

        private bool _CensusInfoFeature = true;
        /// <summary>
        /// To show / supress the CensusInfo Features
        /// </summary>
        /// <remarks></remarks>
        public bool CensusInfoFeature
        {
            get { return _CensusInfoFeature; }
            set { _CensusInfoFeature = value; }
        }

        private DataTable _SubgroupDataInfo = GetInvariantDataTable();
        /// <summary>
        /// Gets or sets the subgroup data
        /// </summary>
        /// <remarks>Subgroup Data Table (AGE, SEX, LOCATION, OTHERS)</remarks>
        public DataTable SubGroupDataInfo
        {
            get
            {
                if (this._SubgroupDataInfo.Rows.Count == 0)
                {
                    DataRow[] Rows = new DataRow[0];

                    // -- SUBGROUPS - SEX, LOCATION, AGEGROUP, OTHERS
                    string _Query = this.mdIQueries.Subgroup.GetSubgroup(FilterFieldType.None, "");
                    this._SubgroupDataInfo = this.ExecuteInvariantDataTable(_Query);

                    this._SubgroupDataInfo.Columns.Add(SubgroupInfoColumns.Name, typeof(string));

                    foreach (DataRow Row in this._SubgroupDataInfo.Rows)
                    {
                        Rows = this.SubgroupInfo.Select(SubgroupInfoColumns.Type + " = " + Convert.ToInt32(Row[Subgroup.SubgroupType]));
                        if (Rows.Length > 0)
                        {
                            Row[SubgroupInfoColumns.Name] = Rows[0][SubgroupInfoColumns.Name].ToString();
                        }
                    }

                }
                return this._SubgroupDataInfo;
            }
        }

        #endregion

        #region -- Methods --


        #region Threading

        public void CancelBackgroundProcess()
        {
            if (moWorkerThread != null)
            {
                try
                {
                    moWorkerThread.Abort();
                    Thread.Sleep(1); // Put the main thread to sleep for 1 millisecond to allow the worker thread to abort:
                    this.mbCancellationPending = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print(ex.Message);
                }
            }

        }

        public void RunBackgroundProcess()
        {

            // Create the thread object. This does not start the thread.
            this.CancelBackgroundProcess();

            moWorkerThread = new Thread(this.DoWork); //Method call where paged data are fetched in loop
            moWorkerThread.Name = "DataViewBackgroundProcess";
            moWorkerThread.IsBackground = true;

            // Start the worker thread.
            moWorkerThread.Start();

            // Loop until worker thread activates.
            //while (!moWorkerThread.IsAlive) ;

            // Put the main thread to sleep for 1 millisecond to allow the worker thread to do some work:
            Thread.Sleep(1);

            // Request that the worker thread stop itself:
            //workerObject.RequestStop();

            // Use the Join method to block the current thread until the object's thread terminates.
            //workerThread.Join();
        }

        private void DoWork()
        {
            string PageDataNIds = string.Empty;
            try
            {


                //-- Get the remaining page records through background worker process if page count > 1
                if (this._PageCount > 1)
                {
                    for (int iPageNumber = 2; iPageNumber < this._PageCount + 1; iPageNumber++)
                    {
                        //If the operation was canceled by the user, set the DoWorkEventArgs.Cancel property to true.
                        if (this.mbCancellationPending)
                        {
                            this.mbCancellationPending = false;
                        }
                        else
                        {
                            this.GetPageData(iPageNumber, true);
                            // Raise event for the completion of a page - This event will be handled in the client application
                            if (this.PagingProgress != null)
                            {
                                this.PagingProgress(iPageNumber, this._PageCount);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }


        #endregion

        /// <summary>
        /// Get comma delimited DataNIds for records of a given page
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public string GetPageDataNIds(int pageNumber)
        {
            //TODO implement thread lock mechanism as main thread as well as background thread may be calling this function

            string RetVal = string.Empty;
            StringBuilder sbPageDataNIDs = new System.Text.StringBuilder();

            int _FirstRecord;
            int _LastRecord;


            //Get the Data NIDs for a Page (sPageDataNIDs)
            _FirstRecord = (pageNumber - 1) * this.mUserPreference.General.PageSize;
            _LastRecord = (pageNumber * this.mUserPreference.General.PageSize) - 1;
            if (_LastRecord >= this._RecordCount)
            {
                _LastRecord = this._RecordCount - 1;
            }

            for (int i = _FirstRecord; i <= _LastRecord; i++)
            {
                try
                {

                    sbPageDataNIDs.Append("," + this._FilteredDataNIds.Rows[i][0].ToString());
                }
                catch (Exception ex)
                {

                    System.Diagnostics.Debug.Print(ex.Message);
                }
            }

            if (sbPageDataNIDs.Length > 0)
            {
                RetVal = sbPageDataNIDs.ToString().Substring(1);
            }

            return RetVal;
        }

        /// <summary>
        /// Get records for desired page
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        public DataView GetPageData(int pageNumber)
        {
            DataView retVal = null;
            if (pageNumber <= this._PageCount)
            {
                retVal = GetPageData(pageNumber, false);
            }
            return retVal;
        }

        /// <summary>
        /// Returns dataview used for generating presentation wizards. 
        /// This should be called only when Main Data table is fully build for all pages.
        /// This discards any records marked for deletion.
        /// Calling routines should not alter filter condition etc. May use a copy of this instead, for furter manipulations
        /// </summary>
        /// <returns></returns>
        public DataView GetPresentationData()
        {
            //Assumption - This function is called only when _MainDataTable is fully build for all pages
            DataView RetVal = null;

            if (this.PresentationData == null)
            {


                string OriginalRowFilter = string.Empty;

                if (this._MainDataTable != null && this._MainDataTable.Rows.Count > 0)
                {
                    DataView dvMainData = this._MainDataTable.DefaultView;

                    // Get a separate instance of data table for presentation data free from any inherent sort and filter clause
                    DataTable TempDT = dvMainData.ToTable(this._MainDataTable.TableName);

                    // Apply Deleted datapoint filter if any
                    if (this.mUserPreference.UserSelection.DataViewFilters.DeletedDataNIds.Length > 0)
                    {
                        DataView dvPresentation = TempDT.DefaultView;
                        dvPresentation.RowFilter = Data.DataNId + " NOT IN (" + this.mUserPreference.UserSelection.DataViewFilters.DeletedDataNIds + ")";
                        TempDT = dvPresentation.ToTable(this._MainDataTable.TableName);
                    }

                    this.PresentationData = TempDT.DefaultView;

                    //Set Numeric Data Column for Textual Values with distinct numeric values
                    this.PresentationData.RowFilter = DataExpressionColumns.DataType + " = " + (int)DataType.Text;
                    if (this.PresentationData.Count > 0)
                    {
                        string CurrentDataValue = string.Empty;
                        int DistinctNumericValue = 0;
                        string InferredNumericValue = string.Empty;

                        this.PresentationData.Sort = Data.DataValue;
                        foreach (DataRowView drv in this.PresentationData)
                        {
                            if (string.Compare(CurrentDataValue, drv[Data.DataValue].ToString(), true) != 0)
                            {
                                InferredNumericValue = InferNumericValue(drv[Data.DataValue].ToString());
                                if (InferredNumericValue == string.Empty)
                                {
                                    DistinctNumericValue++;
                                    drv[DataExpressionColumns.NumericData] = DistinctNumericValue;
                                }
                                else
                                {
                                    drv[DataExpressionColumns.NumericData] = InferredNumericValue;
                                }
                                CurrentDataValue = drv[Data.DataValue].ToString();
                            }
                            else
                            {
                                if (InferredNumericValue == string.Empty)
                                {
                                    drv[DataExpressionColumns.NumericData] = DistinctNumericValue;
                                }
                                else
                                {
                                    drv[DataExpressionColumns.NumericData] = InferredNumericValue;
                                }
                            }
                        }
                    }
                    this.PresentationData.RowFilter = string.Empty;
                }
            }
            // confirm that 
            // else return a clone of presentation data
            RetVal = this.PresentationData;
            return RetVal;
        }

        /// <summary>
        /// Get Most Recent Data. Used for Mapping
        /// This should be called only when Main Data table is fully build for all pages.
        /// </summary>
        /// <param name="OnlyNumericRecords"></param>
        /// <returns></returns>
        public DataView GetMostRecentData(bool OnlyNumericRecords)
        {
            //Assumption - This function is called only when _MainDataTable is fully build for all pages
            DataView RetVal = null;

            if (this.MRDData == null)
            {
                DataView PresentationData = GetPresentationData().Table.Copy().DefaultView;
                if (this.mUserPreference.UserSelection.DataViewFilters.MostRecentData == true)
                {
                    this.MRDData = PresentationData;
                }
                else
                {
                    this.MRDData = ApplyMRDFilter(PresentationData);
                }

                //Apply filter for numeric records
                if (OnlyNumericRecords)
                {
                    this.MRDData.RowFilter = DataExpressionColumns.DataType + " = " + (int)DataType.Numeric;
                }

                // Get a separate instance of data table for MRD data free from any inherent sort and filter clause
                DataTable TempDT = this.MRDData.ToTable("MRDData");
                this.MRDData = TempDT.DefaultView;
            }

            RetVal = this.MRDData;
            return RetVal;
        }

        /// <summary>
        /// Get numeric value from Textual values like '<1234', '>1234', '1234-5678'
        /// </summary>
        /// <param name="textualValue"></param>
        /// <returns></returns>
        private string InferNumericValue(string nonNumericDataValue)
        {
            const string Ascii105 = "–"; //hyphen character as in 21c database)
            const string Ascii45 = "-";  // normal keyboard hyphen character

            string RetVal = string.Empty;
            if (string.IsNullOrEmpty(nonNumericDataValue) || nonNumericDataValue == Ascii105 || nonNumericDataValue == Ascii45)
            {
                RetVal = string.Empty;
            }
            else
            {
                string sTextualValue = nonNumericDataValue.Trim();
                // Case 1  - non numeric data value starts with < symbol and it has single instance and trailing text is a numeric value
                if (sTextualValue.StartsWith("<") && Regex.Matches(sTextualValue, "<").Count == 1)
                {
                    if (DICommon.IsNumeric(sTextualValue.Substring("<".Length)))
                    {
                        RetVal = sTextualValue.Substring("<".Length);
                    }
                }
                // Case 2  - non numeric data value starts with > symbol and it has single instance and trailing text is a numeric value
                else if (sTextualValue.StartsWith(">") && Regex.Matches(sTextualValue, ">").Count == 1)
                {
                    if (DICommon.IsNumeric(sTextualValue.Substring(">".Length)))
                    {
                        RetVal = sTextualValue.Substring(">".Length);
                    }
                }
                // Case 3  - non numeric data value has hyphen symbol (21c database) and it has single instance and surrounding text are numeric values
                else if (sTextualValue.Contains(Ascii105) && Regex.Matches(sTextualValue, Ascii105).Count == 1)
                {
                    string[] DataValues = sTextualValue.Split(Ascii105.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    if (DataValues.Length > 1)
                    {
                        if (DICommon.IsNumeric(DataValues[0]) && DICommon.IsNumeric(DataValues[1]))
                        {
                            decimal AverageValue = (decimal.Parse(DataValues[0].Trim()) + decimal.Parse(DataValues[1].Trim())) / 2;
                            RetVal = AverageValue.ToString();
                        }
                    }
                }
                // Case 4  - non numeric data value has hyphen symbol (normal keyboard) and it has single instance and surrounding text are numeric values
                else if (sTextualValue.Contains(Ascii45) && Regex.Matches(sTextualValue, Ascii45).Count == 1)
                {
                    string[] DataValues = sTextualValue.Split(Ascii45.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (DataValues.Length > 1)
                    {
                        if (DICommon.IsNumeric(DataValues[0]) && DICommon.IsNumeric(DataValues[1]))
                        {
                            decimal AverageValue = (decimal.Parse(DataValues[0].Trim()) + decimal.Parse(DataValues[1].Trim())) / 2;
                            RetVal = AverageValue.ToString();
                        }

                    }
                }
            }
            return RetVal;

        }

        /// <summary>
        /// Get all the data without using background process.
        /// </summary>
        /// <returns></returns>
        public DataView GetAllDataByUserSelection()
        {
            DataView RetVal = null;

            // Generate all dataview pages based on userselection
            this.GenerateAllPages();

            // Apply Filters
            this.ApplyFilters();

            RetVal = this.GetPresentationData();
            // return complete DataView
            return RetVal;
        }

        /// <summary>
        /// Use to generate all pages explicitly when background processing is not desired
        /// and control of main application need to halt before all dataview pages are populated.
        /// </summary>
        public void GenerateAllPages()
        {
            // Loop for paged data
            if (this.GetPageData(1, false) != null)
            {
                if (this._PageCount > 1)
                {
                    this.DoWork();
                }
            }

        }

        /// <summary>
        /// Get Statistics table which contains aggregate information like count, min, max, mean, stddev and variance for each disinct IUS combination in current dataview
        /// </summary>
        /// <returns></returns>
        public DataTable GetStatsTable()
        {
            DataTable RetVal = GetInvariantDataTable();
            DataTable dtStats = this.IUSIndicator.Copy();
            dtStats.TableName = "Stats";

            dtStats.Columns[Indicator.IndicatorName].Caption = DILanguage.GetLanguageString("INDICATOR");

            dtStats.Columns.Add(Unit.UnitName, typeof(string));
            dtStats.Columns[Unit.UnitName].Caption = DILanguage.GetLanguageString("UNIT");
            dtStats.Columns.Add(Unit.UnitGlobal, typeof(bool));

            dtStats.Columns.Add(DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupVal, typeof(string));
            dtStats.Columns[DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupVal].Caption = DILanguage.GetLanguageString("SUBGROUP");
            dtStats.Columns.Add(DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGlobal, typeof(bool));

            dtStats.Columns.Add(StatsExpressionColumns.Count, typeof(int));
            dtStats.Columns[StatsExpressionColumns.Count].Caption = DILanguage.GetLanguageString("COUNT");

            dtStats.Columns.Add(StatsExpressionColumns.Minimum, typeof(double));
            dtStats.Columns[StatsExpressionColumns.Minimum].Caption = DILanguage.GetLanguageString("MIN");

            dtStats.Columns.Add(StatsExpressionColumns.Maximum, typeof(double));
            dtStats.Columns[StatsExpressionColumns.Maximum].Caption = DILanguage.GetLanguageString("MAX");

            dtStats.Columns.Add(StatsExpressionColumns.Sum, typeof(double));
            dtStats.Columns[StatsExpressionColumns.Sum].Caption = DILanguage.GetLanguageString("SUM");

            if (this._CensusInfoFeature)
            {
                //-- Add the histogram and IUSNID column
                dtStats.Columns.Add(StatsExpressionColumns.Histogram, typeof(string));
                dtStats.Columns[StatsExpressionColumns.Histogram].Caption = string.Empty;

                //dtStats.Columns.Add(Indicator_Unit_Subgroup.IUSNId, typeof(int));
            }
            else
            {
                //-- TODO: Will remove Mean, SD, and varaince after implementing Census info features in the web application
                dtStats.Columns.Add(StatsExpressionColumns.Mean, typeof(double));
                dtStats.Columns[StatsExpressionColumns.Mean].Caption = DILanguage.GetLanguageString("AVG");

                dtStats.Columns.Add(StatsExpressionColumns.StandardDeviation, typeof(double));
                dtStats.Columns[StatsExpressionColumns.StandardDeviation].Caption = DILanguage.GetLanguageString("STDDEV");

                dtStats.Columns.Add(StatsExpressionColumns.Variance, typeof(double));
                dtStats.Columns[StatsExpressionColumns.Variance].Caption = DILanguage.GetLanguageString("VARIANCE");
            }

            // Ignore non numeric data records 
            DataView PresentationData = this.GetPresentationData();
            PresentationData.RowFilter = DataExpressionColumns.DataType + " = " + (int)DataType.Numeric;
            DataTable dtMain = PresentationData.ToTable();

            DataSet _Base = GetInvariantDataSet();
            _Base.Tables.Add(dtMain);
            _Base.Tables.Add(dtStats);
            _Base.Tables.Add(this.IUSUnit.Copy());
            _Base.Tables.Add(this.IUSSubgroupVal.Copy());
            _Base.Relations.Add("RelUnit", _Base.Tables[this.IUSUnit.TableName].Columns[Indicator_Unit_Subgroup.IUSNId], dtStats.Columns[Indicator_Unit_Subgroup.IUSNId], false);
            dtStats.Columns[Unit.UnitName].Expression = "Parent(RelUnit)." + Unit.UnitName;
            dtStats.Columns[Unit.UnitGlobal].Expression = "Parent(RelUnit)." + Unit.UnitGlobal;

            _Base.Relations.Add("RelSubgroupVal", _Base.Tables[this.IUSSubgroupVal.TableName].Columns[Indicator_Unit_Subgroup.IUSNId], dtStats.Columns[Indicator_Unit_Subgroup.IUSNId], false);
            dtStats.Columns[DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupVal].Expression = "Parent(RelSubgroupVal)." + DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupVal;
            dtStats.Columns[DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGlobal].Expression = "Parent(RelSubgroupVal)." + DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValGlobal;

            _Base.Relations.Add("RelStats", dtStats.Columns[Indicator_Unit_Subgroup.IUSNId], dtMain.Columns[Indicator_Unit_Subgroup.IUSNId], false);
            //dtStats.Columns[StatsExpressionColumns.Count].Expression = "Count(Child(RelStats)." + Data.DataNId + ")";
            dtStats.Columns[StatsExpressionColumns.Minimum].Expression = "Min(Child(RelStats)." + DataExpressionColumns.NumericData + ")";
            dtStats.Columns[StatsExpressionColumns.Maximum].Expression = "Max(Child(RelStats)." + DataExpressionColumns.NumericData + ")";
            dtStats.Columns[StatsExpressionColumns.Sum].Expression = "Sum(Child(RelStats)." + DataExpressionColumns.NumericData + ")";

            if (!this._CensusInfoFeature)
            {
                dtStats.Columns[StatsExpressionColumns.Mean].Expression = "Avg(Child(RelStats)." + DataExpressionColumns.NumericData + ")";
                dtStats.Columns[StatsExpressionColumns.StandardDeviation].Expression = "StDev(Child(RelStats)." + DataExpressionColumns.NumericData + ")";
                dtStats.Columns[StatsExpressionColumns.Variance].Expression = "Var(Child(RelStats)." + DataExpressionColumns.NumericData + ")";
            }

            // DI6 Median Expression http://www.statcan.ca/english/edu/power/ch11/median/median.htm

            // Set Count based on all records (not based on numeric records alone)
            // Normally all data values for an IUS will be numeric or non numeric
            // In case of non numeric datavalues count will be for available records but arithmetical values like sum mean sd will not be calculated
            // In rare cases where data values are mix of numeric and non numeric values arithmetical values will be calculated based on count of numeric records but actual count displayed will be the composite count
            PresentationData.RowFilter = "";
            for (int i = 0; i < dtStats.Rows.Count; i++)
            {
                PresentationData.RowFilter = Indicator_Unit_Subgroup.IUSNId + " = " + dtStats.Rows[i][Indicator_Unit_Subgroup.IUSNId];
                dtStats.Rows[i][StatsExpressionColumns.Count] = PresentationData.Count;

                if (this._CensusInfoFeature)
                {
                    dtStats.Rows[i][StatsExpressionColumns.Histogram] = DILanguage.GetLanguageString("HISTOGRAM");
                }
            }

            PresentationData.RowFilter = string.Empty;

            RetVal = dtStats;
            return RetVal;
        }

        /// <summary>
        /// Apply / Reset Source, Unit, Subgroup, IUSDataValue and DataValue Filters
        /// </summary>
        /// <returns></returns>
        public void ApplyFilters()
        {
            // -- Apply Filters could be called in the following conditions:
            // Condition 1: When user filters on Source, Unit, Subgroups, Datavalues and MRD
            // Condition 2: When the user clears the filters

            // If DataView is fully generated then apply fiilters on the Main Data Table
            // Else Regenerate the DataView using the query based method

            // How do we know DataView is fully generated
            // If total pages generated are more than 1 then DataView might not be fully generated
            // If total pages generated are more than 1 then check the the Bakground process for building up the DataView - if finished then the DataView is fully generated

            // Fully generated cases
            // CASE 1: Only one page of Data
            // CASE 2: More than one page of data and Background porocess finished

            // --- STEPS ---
            // STEP 1: Check if any filters available
            // STEP 2: Apply Filters OR STEP 3: Clear Filters
            // STEP 4: Set Page Info
            // STEP 5: In case for FULLY GENERATED VIEW, set Page Data Status to true for each page
            // ------------------------------------------------------------------ 

            // NOTE 1: When filter is applied, mAllDataNIDsFiltered variable stores the filtered DataNIDs
            // NOTE 2: _AllDataNIDs at any stage stores the complete set of DataNIDs based on the I, T and A selections
            // NOTE 3: Paging is always based on mAllDataNIDsFiltered (GetPageData)

            bool bDataViewFullyGenerated = false;
            string _Query = string.Empty;
            int iRecordCount = 0;
            DataView dvData = null;

            // STEP 1: Check if any filters available
            if (mUserPreference.UserSelection.DataViewFilters.FilterExists(true))
            {
                // STEP 2: Apply Filters
                // STEP 2.1: Check whether the main Data View is Fully generated or not
                bDataViewFullyGenerated = this.IsDataViewFullyGenerated() && !mbFilterAppliedByQuery;
                string sOriginalSort = this._MainDataTable.DefaultView.Sort; //Preserve Original Sort

                if (bDataViewFullyGenerated)
                {
                    // STEP 2.2: FULLY GENERATED DATAVIEW
                    dvData = this._MainDataTable.DefaultView;

                    // STEP 2.2.2: Apply / Remove Filter  for Source, Unit, Subgroup and DataValues
                    dvData.RowFilter = this.GetFilterString();

                    // STEP 2.2.3: Apply MRD Filter
                    if (mUserPreference.UserSelection.DataViewFilters.MostRecentData == true && dvData.Count > 0)
                    {
                        // Apply MRD
                        dvData = ApplyMRDFilter(dvData);
                        // Restore original sort 
                        this._MainDataTable.DefaultView.Sort = sOriginalSort;
                        // Set DeletedDataNIds to blank as records marked for deletion are discarded in MRD
                        mUserPreference.UserSelection.DataViewFilters.DeletedDataNIds = String.Empty;
                        mUserPreference.UserSelection.DataViewFilters.DeletedDataGIds = String.Empty;
                    }
                }
                else
                {

                    // STEP 2.3: PARTIALLY GENERATED DATAVIEW


                    // STEP 2.3.1: STOP current Background process for DataView generation
                    if (moWorkerThread.IsAlive)
                    {
                        this.CancelBackgroundProcess();
                    }

                    // STEP 2.3.2: Create mAllDataNIDsFiltered Data Table - Get ALL Data NIDS on the Basis of the Filters 
                    // Use msAllDataNIDs to get filtered result set

                    //TODO Case when IUSDataValue filter is for Indicator instead of IUS then IndicatorNId field is not available and query fails
                    _Query = mdIQueries.Data.GetDataNIDByIUSTimePeriodAreaSource(this.mUserPreference.UserSelection, this.mdIConnection.ConnectionStringParameters.ServerType, this.msAllDataNIDs);

                    // -- This Data Table will have all the DataNIDs and wil be used in other operations to fetch data 
                    try
                    {
                        // STEP 2.3.2.1: Initialize mdtData
                        this._MainDataTable.Rows.Clear();

                        // STEP 2.3.2.2
                        // Once the Filter has been applied on the Partial DataView, the control should not go to FULLY GENERATED DATAVIEW unless the DataView is reloaded
                        // IF the filtered record count results in one page of data, then the control will go to the FULLY GENERATED DATAVIEW (IsDataViewFullyGenerated will return true)
                        // In such a scenario, when the user will restore the filters, wrong dataview will be sent back to the user.
                        mbFilterAppliedByQuery = true;

                        // -- Execute the Query 
                        dvData = this.ExecuteInvariantDataTable(_Query).DefaultView;

                        // STEP 2.3.2.3: Check for MRD filter
                        if (mUserPreference.UserSelection.DataViewFilters.MostRecentData && dvData.Count > 0)
                        {
                            // Apply MRD
                            dvData = ApplyMRDFilter(dvData);

                            // Set DeletedDataNIds to blank as records marked for deletion are discarded in MRD
                            mUserPreference.UserSelection.DataViewFilters.DeletedDataNIds = String.Empty;
                            mUserPreference.UserSelection.DataViewFilters.DeletedDataGIds = String.Empty;

                        }
                        // Apply original sorting if any
                        if (sOriginalSort.Length > 0)
                        {
                            dvData.Sort = sOriginalSort;
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.Print(ex.Message);
                    }
                }

                // STEP 2.4: If IndicatorDataValueFilters exists and FilterByArea condition is on, modify the original row filter
                string[] DistinctColumns = new string[1];
                try
                {
                    if (mUserPreference.UserSelection.DataViewFilters.IndicatorDataValueFilters.Count > 1 && mUserPreference.UserSelection.DataViewFilters.IndicatorDataValueFilters.IncludeArea == true)
                    {
                        //-- Store Indicator_NId / IUSNId filed to be used in filter clause
                        string Indicator_IUSField = string.Empty;
                        if (mUserPreference.UserSelection.DataViewFilters.IndicatorDataValueFilters.ShowIUS)
                        {
                            Indicator_IUSField = Indicator_Unit_Subgroup.IUSNId;
                        }
                        else
                        {
                            Indicator_IUSField = Indicator.IndicatorNId;
                        }

                        //-- Get list of Indicator_NId / IUSNId considered in IndicatorDataValueFilters
                        List<string> Indicator_IUS_NIds = new List<string>();
                        foreach (IndicatorDataValueFilter IndicatorDataValueFilter in mUserPreference.UserSelection.DataViewFilters.IndicatorDataValueFilters)
                        {
                            Indicator_IUS_NIds.Add(IndicatorDataValueFilter.IndicatorNId.ToString());
                        }

                        bool AreaQualified = true; //-- Flag to identify whether an area qualifies for all IndicatorDataValueFilters
                        StringBuilder sbAreaNId = new StringBuilder();

                        //-- Get distinct AreaNIds in dataview
                        DistinctColumns[0] = Area.AreaNId;
                        DataTable dtDistinctArea = dvData.ToTable(true, DistinctColumns);

                        DataTable dtFilteredData = dvData.ToTable();

                        //-- Itereate all distinct areas in dataview to check whether it qualifies for all IndicatorDataValueFilters
                        foreach (DataRow dr in dtDistinctArea.Rows)
                        {
                            AreaQualified = true;
                            foreach (string Indicator_IUS_NId in Indicator_IUS_NIds)
                            {
                                if (dtFilteredData.Select(Area.AreaNId + " = " + dr[Area.AreaNId] + " AND " + Indicator_IUSField + " = " + Indicator_IUS_NId).Length == 0) //== mUserPreference.UserSelection.DataViewFilters.IndicatorDataValueFilters.Count
                                {
                                    AreaQualified = false;
                                    break;
                                }
                            }

                            if (AreaQualified)
                            {
                                sbAreaNId.Append("," + dr[Area.AreaNId]);
                            }
                        }
                        dtFilteredData.Dispose();

                        // -- Modify orginal rowfilter with qualifying area nids 
                        if (sbAreaNId.Length > 0)
                        {
                            string IndicatorDataValueAreaFilter = dvData.RowFilter + " AND " + Area.AreaNId + " IN (" + sbAreaNId.ToString().Substring(1) + ")";
                            dvData.RowFilter = IndicatorDataValueAreaFilter;
                        }
                    }


                }
                catch (Exception ex)
                {

                    Console.Write(ex.Message);
                }


                // STEP 2.5: Overwrite Filtered DataNIds to mAllDataNIDsFiltered
                DistinctColumns[0] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.DataNId;
                this._FilteredDataNIds = dvData.ToTable(false, DistinctColumns);

                // STEP 2.6: Set Record Count
                iRecordCount = dvData.Count;

                //-- Reset deleted data nids. Remove deleted data nids which are not part of FilteredDataNIds. This will help in showing proper record count
                if (!string.IsNullOrEmpty(mUserPreference.UserSelection.DataViewFilters.DeletedDataNIds))
                {
                    string[] DeletedDataNId = mUserPreference.UserSelection.DataViewFilters.DeletedDataNIds.Split(",".ToCharArray());
                    for (int i = 0; i < DeletedDataNId.Length; i++)
                    {
                        if (this._FilteredDataNIds.Select(DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.DataNId + " = " + DeletedDataNId[i]).Length == 0)
                        {
                            DeletedDataNId[i] = string.Empty; //Set empty string which will be eliminated using StringSplitOptions.RemoveEmptyEntries
                        }
                    }
                    string DeletedDataNIds = String.Join(",", DeletedDataNId);
                    DeletedDataNId = DeletedDataNIds.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    mUserPreference.UserSelection.DataViewFilters.DeletedDataNIds = String.Join(",", DeletedDataNId);
                }

                // Set Fillters
                this.SetFilters();

            }
            else   // Reset filters
            {
                // STEP 3: Clear Filters
                this._MainDataTable.DefaultView.RowFilter = string.Empty;
                this.ClearFilters();

                // STEP 3.1: Once the Filter has been applied on the Partial DataView, the control should not go to FULLY GENERATED DATAVIEW unless the DataView is reloaded
                mbFilterAppliedByQuery = false;
                // STEP 3.2: Restore the original AllDataNIDs and call the GetDataView function
                this._FilteredDataNIds = this._AllDataNIDs.Copy();
                // STEP 3.3: Record Count
                iRecordCount = _FilteredDataNIds.Rows.Count;

            }

            // STEP 4: Set Page Info
            SetPagingInfo(iRecordCount);

            // STEP 5: In case for FULLY GENERATED VIEW, set Page Data Status to true for each page
            if (bDataViewFullyGenerated)
            {
                // In case for FULLY GENERATED VIEW, set Page Data Status to true for each page
                for (int i = 0; i < mbPageDataStatus.Length; i++)
                {
                    mbPageDataStatus[i] = true;
                }
            }
        }

        /// <summary>
        /// Transforms deleted source nids from IUS_SourceNIDs format into SourceNIDs format and vice a versa
        /// Also updates selected field value in Source / IUSSource table
        /// After transformation, calling routine should explicitly toggle the status of ShowSourceByIUS property
        /// </summary>
        /// <param name="userSelection">UserSelection</param>
        /// <returns>transformed deleted source nids</returns>
        public static string GetTransformedDeletedSourceNIDs(UserSelection userSelection, DataTable dtIUSSource, DataTable dtSources)
        {

            // -- moDIDataView.IUSSource - Available IUS _ Source

            string sNewDelSourceNIDs = string.Empty;
            System.Text.StringBuilder sbNewDelSourceNIDs = new System.Text.StringBuilder();
            string retVal = string.Empty;
            DataView dvNewDelSourceNIDs;
            DataTable dtNewDelSourceNIDs = null;
            string[] DistinctColumns = new string[1];
            string[] IUSDistinctColumns = new string[2];

            if (string.IsNullOrEmpty(userSelection.DataViewFilters.DeletedSourceNIds))
            {

                // -- Do Nothing

                retVal = sNewDelSourceNIDs;
            }

            else
            {

                // -- Do Transformation

                dvNewDelSourceNIDs = dtIUSSource.DefaultView; //this.IUSSource.DefaultView;            // Use property instead of private variable 
                if (userSelection.DataViewFilters.ShowSourceByIUS)
                {
                    // -- Trnasform to Source
                    // -- sOldDelSourceNIDs = Comma seperated IUS_SourceNIDs - "'4_5','4_6','9_112'"
                    // -- Get the Sources from moDIDataView.IUSSource on the basis of the aboe IUS_Source NIDs

                    // -- Apply filter on moDIDataView.IUSSource by deleted IUS_SourceNIDs
                    dvNewDelSourceNIDs.RowFilter = Indicator_Unit_Subgroup.IUSNId + " + '_' + " + IndicatorClassifications.ICNId + " IN (" + userSelection.DataViewFilters.DeletedSourceNIds + ")";
                    DistinctColumns[0] = IndicatorClassifications.ICNId;
                    dtNewDelSourceNIDs = dvNewDelSourceNIDs.ToTable(true, DistinctColumns);
                }
                else
                {

                    // -- Trnasform to IUS_Source
                    // -- sOldDelSourceNIDs = Comma seperated SourceNIDs - 5,6,9
                    // -- Get the IUS_Source combinmation from moDIDataView.IUSSource on the basis of the aboe Source NIDs

                    // -- Apply filter on moDIDataView.IUSSource by deleted SourceNIDs
                    dvNewDelSourceNIDs.RowFilter = IndicatorClassifications.ICNId + " IN (" + userSelection.DataViewFilters.DeletedSourceNIds + ")";
                    IUSDistinctColumns[0] = Indicator_Unit_Subgroup.IUSNId;
                    IUSDistinctColumns[1] = IndicatorClassifications.ICNId;
                    dtNewDelSourceNIDs = dvNewDelSourceNIDs.ToTable(true, IUSDistinctColumns);
                }

                // -- Loop through to build the new Source Deleted String
                foreach (DataRow dr in dtNewDelSourceNIDs.Rows)
                {
                    if (userSelection.DataViewFilters.ShowSourceByIUS)
                    {
                        // -- Trnasform to Source
                        sbNewDelSourceNIDs.Append("," + dr[IndicatorClassifications.ICNId]);
                    }
                    else
                    {
                        // -- Trnasform to IUS_Source
                        sbNewDelSourceNIDs.Append(",'" + dr[Indicator_Unit_Subgroup.IUSNId] + "_" + dr[IndicatorClassifications.ICNId] + "'");

                    }
                }
                if (sbNewDelSourceNIDs.Length != 0)
                {
                    sNewDelSourceNIDs = sbNewDelSourceNIDs.ToString().Substring(1);
                }
                retVal = sNewDelSourceNIDs;
                dvNewDelSourceNIDs.RowFilter = "";

                // Update status of selected field in Source / IUSSource table
                if (!string.IsNullOrEmpty(retVal))
                {
                    if (userSelection.DataViewFilters.ShowSourceByIUS)
                    {
                        foreach (DataRow dr in dtSources.Rows)           // Use property instead of private variable 
                        {
                            dr[DataExpressionColumns.Selected] = true;
                        }
                        foreach (DataRow dr in dtSources.Select(IndicatorClassifications.ICNId + " IN (" + retVal + ")"))
                        {
                            dr[DataExpressionColumns.Selected] = false;
                        }
                    }
                    else
                    {
                        foreach (DataRow dr in dtIUSSource.Rows)         // Use property instead of private variable 
                        {
                            dr[DataExpressionColumns.Selected] = true;
                        }
                        foreach (DataRow dr in dtIUSSource.Select(Indicator_Unit_Subgroup.IUSNId + " + '_' + " + IndicatorClassifications.ICNId + " IN (" + retVal + ")"))
                        {
                            dr[DataExpressionColumns.Selected] = false;
                        }
                    }
                }

            }

            return retVal;
        }

        /// <summary>
        /// Transforms deleted source nids from IUS_SourceNIDs format into SourceNIDs format and vice a versa
        /// Also updates selected field value in Source / IUSSource table
        /// After transformation, calling routine should explicitly toggle the status of ShowSourceByIUS property
        /// </summary>
        /// <param name="userSelection">UserSelection</param>
        /// <returns>transformed deleted source nids</returns>
        public static string GetTransformedDeletedSourceNIDs(UserSelection userSelection, DataTable dtIUSSource, DIConnection dbConnection, DIQueries dbQueries)
        {

            // -- moDIDataView.IUSSource - Available IUS _ Source
            string sNewDelSourceNIDs = string.Empty;
            DataRow[] Rows = new DataRow[0];
            string DelSourceNIDs = string.Empty;


            System.Text.StringBuilder sbNewDelSourceNIDs = new System.Text.StringBuilder();
            string retVal = string.Empty;
            string[] DistinctColumns = new string[1];
            string[] IUSDistinctColumns = new string[2];

            if (userSelection.DataViewFilters.ShowSourceByIUS || string.IsNullOrEmpty(userSelection.DataViewFilters.DeletedSourceNIds))
            {

                // -- Do Nothing

                retVal = sNewDelSourceNIDs;
            }

            else
            {
                try
                {
                    IDataReader IUSReader;
                    IUSReader = dbConnection.ExecuteReader(dbQueries.Data.GetDataViewDataByDataNIDs(userSelection.DataViewFilters.DeletedSourceNIds));
                    while (IUSReader.Read())
                    {
                        sNewDelSourceNIDs = "'" + IUSReader[Data.IUSNId].ToString() + "_" + IUSReader[IndicatorClassifications.ICNId].ToString() + "'";
                        retVal += "," + sNewDelSourceNIDs;
                        Rows = dtIUSSource.Select(Indicator_Unit_Subgroup.IUSNId + " + '_' + " + IndicatorClassifications.ICNId + " IN (" + sNewDelSourceNIDs + ")");
                        foreach (DataRow Row in Rows)
                        {
                            Row[DataExpressionColumns.Selected] = false;
                        }
                    }
                    IUSReader.Close();

                    if (!string.IsNullOrEmpty(retVal))
                    {
                        retVal = retVal.Substring(1);
                    }

                }
                catch (Exception)
                {
                }
            }
            return retVal;
        }


        /// <summary>
        /// Transforms deleted unit nids from Indicator_UnitNIds format into UnitNIds format and vice a versa
        /// Also updates selected field value in Unit / IndicatorUnit table
        /// After transformation, calling routine should explicitly toggle the status of ShowUnitByIndicator property
        /// </summary>
        /// <param name="userSelection">UserSelection</param>
        /// <returns>transformed deleted unit nids</returns>
        public string GetTransformedDeletedUnitNIDs(UserSelection userSelection)
        {
            string RetVal = string.Empty;
            string sNewDelUnitNIds = string.Empty;
            System.Text.StringBuilder sbNewDelUnitNIds = new System.Text.StringBuilder();
            DataView dvNewDelUnitNIds;
            DataTable dtNewDelUnitNIDs = null;
            string[] DistinctUnitColumns = new string[1];
            string[] DistinctIndicatorUnitColumns = new string[2];

            if (string.IsNullOrEmpty(userSelection.DataViewFilters.DeletedUnitNIds))
            {
                // -- Do Nothing
                RetVal = sNewDelUnitNIds;
            }
            else
            {
                // -- Do Transformation
                dvNewDelUnitNIds = this.IndicatorUnit.DefaultView; // Use property instead of private variable 
                if (userSelection.DataViewFilters.ShowUnitByIndicator == true)
                {
                    // -- Trnasform to Unit
                    // -- sOldDelUnitNIds = Comma seperated Indicator_UnitNIds - 4_5, 4_6, 9_112
                    // -- Get the Units from moDIDataView.IndicatorUnit on the basis of the above Indicator_UnitNIds

                    // -- Apply filter on moDIDataView.IndicatorUnit by deleted Indicator_UnitNIds
                    dvNewDelUnitNIds.RowFilter = Indicator.IndicatorNId + " + '_' + " + Unit.UnitNId + " IN (" + userSelection.DataViewFilters.DeletedUnitNIds + ")";
                    DistinctUnitColumns[0] = Unit.UnitNId;
                    dtNewDelUnitNIDs = dvNewDelUnitNIds.ToTable(true, DistinctUnitColumns);
                }
                else
                {
                    // -- Trnasform to Indicator_Unit
                    // -- sOldDelUnitNIds = Comma seperated UnitNIds - 5,6,9
                    // -- Get the Indicator_Unit combinmation from moDIDataView.IndicatorUnit on the basis of the aboe Unit NIds 

                    // -- Apply filter on moDIDataView.IndicatorUnit by deleted UnitNIds
                    dvNewDelUnitNIds.RowFilter = Unit.UnitNId + " IN (" + userSelection.DataViewFilters.DeletedUnitNIds + ")";
                    DistinctIndicatorUnitColumns[0] = Indicator.IndicatorNId;
                    DistinctIndicatorUnitColumns[1] = Unit.UnitNId;
                    dtNewDelUnitNIDs = dvNewDelUnitNIds.ToTable(true, DistinctIndicatorUnitColumns);
                }

                // -- Loop through to build the new Unit Deleted String
                foreach (DataRow dr in dtNewDelUnitNIDs.Rows)
                {
                    if (userSelection.DataViewFilters.ShowUnitByIndicator == true)
                    {
                        // -- Trnasform to Unit
                        sbNewDelUnitNIds.Append("," + dr[Unit.UnitNId]);

                    }
                    else
                    {
                        // -- Trnasform to Indicator_Unit
                        sbNewDelUnitNIds.Append(",'" + dr[Indicator.IndicatorNId] + "_" + dr[Unit.UnitNId] + "'");
                    }
                }
                sNewDelUnitNIds = sbNewDelUnitNIds.ToString().Substring(1);
                RetVal = sNewDelUnitNIds;
                dvNewDelUnitNIds.RowFilter = "";
            }

            // Update status of selected field in Unit / IndicatorUnit table
            if (!string.IsNullOrEmpty(RetVal))
            {
                if (userSelection.DataViewFilters.ShowUnitByIndicator)
                {
                    foreach (DataRow dr in this.Units.Rows)             // Use property instead of private variable 
                    {
                        dr[DataExpressionColumns.Selected] = true;
                    }
                    foreach (DataRow dr in this.Units.Select(Unit.UnitNId + " IN (" + RetVal + ")"))
                    {
                        dr[DataExpressionColumns.Selected] = false;
                    }
                }
                else
                {
                    foreach (DataRow dr in this.IndicatorUnit.Rows)     // Use property instead of private variable 
                    {
                        dr[DataExpressionColumns.Selected] = true;
                    }
                    foreach (DataRow dr in this.IndicatorUnit.Select(Indicator.IndicatorNId + " + '_' + " + Unit.UnitNId + " IN (" + RetVal + ")"))
                    {
                        dr[DataExpressionColumns.Selected] = false;
                    }
                }
            }


            return RetVal;
        }

        /// <summary>
        /// Transforms deleted subgroupval nids from Indicator_SubgroupValNIds format into SubgroupNIds format and vice a versa
        /// Also updates selected field value in SubgroupVals / IndicatorSubgroupVal table
        /// After transformation, calling routine should explicitly toggle the status of ShowSubgroupByIndicator property
        /// </summary>
        /// <param name="userSelection">UserSelection</param>
        /// <returns>transformed deleted subgroupval nids</returns>
        public string GetTransformedDeletedSubgroupValNIDs(UserSelection userSelection)
        {
            string RetVal = string.Empty;
            string sNewDelSubgroupValNIds = string.Empty;
            System.Text.StringBuilder sbNewDelSubgroupValNIds = new System.Text.StringBuilder();
            DataView dvNewDelSubgroupValNIds;
            DataTable dtNewDelSubgroupValNIDs = null;
            string[] DistinctSubgroupValColumns = new string[1];
            string[] DistinctIndicatorSubgroupValColumns = new string[2];

            if (string.IsNullOrEmpty(userSelection.DataViewFilters.DeletedSubgroupNIds))
            {
                // -- Do Nothing
                RetVal = sNewDelSubgroupValNIds;
            }
            else
            {
                // -- Do Transformation
                dvNewDelSubgroupValNIds = this.IndicatorSubgroupVal.DefaultView; //Use property instead of private variable
                if (userSelection.DataViewFilters.ShowSubgroupByIndicator == true)
                {
                    // -- Trnasform to SubgroupVal
                    // -- sOldDelSubgroupValNIds = Comma seperated Indicator_SubgroupValNIds - 4_5, 4_6, 9_112
                    // -- Get the SubgroupVals from moDIDataView.IndicatorSubgroupVal on the basis of the above Indicator_SubgroupValNIds

                    // -- Apply filter on moDIDataView.IndicatorSubgroupVal by deleted Indicator_SubgroupValNIds
                    dvNewDelSubgroupValNIds.RowFilter = Indicator.IndicatorNId + " + '_' + " + DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValNId + " IN (" + userSelection.DataViewFilters.DeletedSubgroupNIds + ")";
                    DistinctSubgroupValColumns[0] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValNId;
                    dtNewDelSubgroupValNIDs = dvNewDelSubgroupValNIds.ToTable(true, DistinctSubgroupValColumns);
                }
                else
                {
                    // -- Trnasform to Indicator_Subgroup
                    // -- sOldDelSubgroupNIds = Comma seperated SubgroupNIds - 5,6,9
                    // -- Get the Indicator_Subgroup combinmation from moDIDataView.IndicatorSubgroupVal on the basis of the aboe Subgroup NIds 

                    // -- Apply filter on moDIDataView.IndicatorSubgroupVal by deleted SubgroupValNIds
                    dvNewDelSubgroupValNIds.RowFilter = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValNId + " IN (" + userSelection.DataViewFilters.DeletedSubgroupNIds + ")";
                    DistinctIndicatorSubgroupValColumns[0] = Indicator.IndicatorNId;
                    DistinctIndicatorSubgroupValColumns[1] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValNId;
                    dtNewDelSubgroupValNIDs = dvNewDelSubgroupValNIds.ToTable(true, DistinctIndicatorSubgroupValColumns);
                }

                // -- Loop through to build the new SubgroupVal Deleted String
                foreach (DataRow dr in dtNewDelSubgroupValNIDs.Rows)
                {
                    if (userSelection.DataViewFilters.ShowSubgroupByIndicator == true)
                    {
                        // -- Trnasform to SubgroupVal
                        sbNewDelSubgroupValNIds.Append("," + dr[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValNId]);
                    }
                    else
                    {
                        // -- Trnasform to Indicator_SubgroupVal
                        sbNewDelSubgroupValNIds.Append(",'" + dr[Indicator.IndicatorNId] + "_" + dr[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValNId] + "'");
                    }
                }
                sNewDelSubgroupValNIds = sbNewDelSubgroupValNIds.ToString().Substring(1);
                RetVal = sNewDelSubgroupValNIds;
                dvNewDelSubgroupValNIds.RowFilter = "";
            }


            // Update status of selected field in SubgroupVals / IndicatorSubgroupVal table
            if (!string.IsNullOrEmpty(RetVal))
            {
                if (userSelection.DataViewFilters.ShowSubgroupByIndicator)
                {
                    foreach (DataRow dr in this.SubgroupVals.Rows)          // Use property instead of private variable 
                    {
                        dr[DataExpressionColumns.Selected] = true;
                    }
                    foreach (DataRow dr in this.SubgroupVals.Select(DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValNId + " IN (" + RetVal + ")"))
                    {
                        dr[DataExpressionColumns.Selected] = false;
                    }
                }
                else
                {
                    foreach (DataRow dr in this.IndicatorSubgroupVal.Rows)   // Use property instead of private variable 
                    {
                        dr[DataExpressionColumns.Selected] = true;
                    }
                    foreach (DataRow dr in this.IndicatorSubgroupVal.Select(Indicator.IndicatorNId + " + '_' + " + DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupValNId + " IN (" + RetVal + ")"))
                    {
                        dr[DataExpressionColumns.Selected] = false;
                    }
                }
            }
            return RetVal;
        }

        /// <summary>
        /// Apply / Reset sorting
        /// </summary>
        public void ApplySort()
        {
            // Sorting will always be applied only after whole MainData table has been completely built
            // Single page records will be sorted through grid sorting except for data value filter

            string SortString = this.GetSortString();

            this._MainDataTable.DefaultView.Sort = SortString; // this.mUserPreference.DataView.SortFields;
            // STEP 2.4: Overwrite Filtered DataNIds to mAllDataNIDsFiltered
            string[] DistinctColumns = new string[1];
            DistinctColumns[0] = DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.DataNId;
            this._FilteredDataNIds = this._MainDataTable.DefaultView.ToTable(false, DistinctColumns);
        }

        private string GetSortString()
        {
            string RetVal = this.mUserPreference.DataView.SortFields;
            if (RetVal.Contains(DataExpressionColumns.TimePeriodStartDate))
            {
                RetVal = RetVal.Replace(DataExpressionColumns.TimePeriodStartDate, Timeperiods.TimePeriod);
            }

            //-- Add End date expression column if filter clause includes filter for end date and main table does not contain end column
            if (RetVal.Contains(DataExpressionColumns.TimePeriodEndDate) && !this._MainDataTable.Columns.Contains(DataExpressionColumns.TimePeriodEndDate))
            {
                //-- set values 
                this.AddStartDateEndDateColumnsToDataTable(this._MainDataTable);
                this.SetStartDateAndEndDateColumnValues(this._MainDataTable);
            }


            return RetVal;
        }

        /// <summary>
        /// Update the dataview sort fields. It update the "TimePeriodStartDate" with "TimePeriod".
        /// </summary>
        /// <returns></returns>
        public string UpdateSortFields()
        {
            string RetVal = string.Empty;
            try
            {
                RetVal = this.mUserPreference.DataView.SortFields;
                if (RetVal.Contains(DataExpressionColumns.TimePeriodStartDate))
                {
                    RetVal = RetVal.Replace(DataExpressionColumns.TimePeriodStartDate, Timeperiods.TimePeriod);
                }

            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        /// Set language based caption for the 
        /// </summary>
        /// <param name="languageFilePath"></param>
        public void ApplyLanguageSettings()
        {
            msTotalRecordsLanguageString = DILanguage.GetLanguageString("SELECTED");
            msFilterOnTranslation = DILanguage.GetLanguageString("FILTERON");
            msPAGETranslation = DILanguage.GetLanguageString("PAGE");
            msOFTranslation = DILanguage.GetLanguageString("OF");

            if (this._MainDataTable.Columns.Count > 0)
            {


                this._MainDataTable.Columns[Timeperiods.TimePeriod].Caption = DILanguage.GetLanguageString("TIMEPERIOD");
                this._MainDataTable.Columns[Area.AreaID].Caption = DILanguage.GetLanguageString("AREAID");
                this._MainDataTable.Columns[Area.AreaName].Caption = DILanguage.GetLanguageString("AREANAME");
                this._MainDataTable.Columns[Indicator.IndicatorName].Caption = DILanguage.GetLanguageString("INDICATOR");
                this._MainDataTable.Columns[Data.DataValue].Caption = DILanguage.GetLanguageString("DATAVALUE");
                this._MainDataTable.Columns[Unit.UnitName].Caption = DILanguage.GetLanguageString("UNIT");
                this._MainDataTable.Columns[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupVal].Caption = DILanguage.GetLanguageString("SUBGROUP");
                this._MainDataTable.Columns[Data.ICIUSOrder].Caption = DILanguage.GetLanguageString("RANK");
                if (this.mUserPreference.General.ShowRecommendedSourceColor)
                {
                    this._MainDataTable.Columns[RecommendedSources.ICIUSLabel].Caption = DILanguage.GetLanguageString("LABEL");
                }


                // Set subgroup (age sex location others) column caption
                foreach (DataRow dr in this.SubgroupInfo.Rows)
                {
                    this._MainDataTable.Columns[dr[SubgroupInfoColumns.Name].ToString()].Caption = dr[SubgroupInfoColumns.Caption].ToString();
                }

                this._MainDataTable.Columns[IndicatorClassifications.ICName].Caption = DILanguage.GetLanguageString("SOURCECOMMON");
                this._MainDataTable.Columns[Data.DataDenominator].Caption = DILanguage.GetLanguageString("DENOMINATOR");



                // Set Captions for Indicator Metadata 
                if (this.mUserPreference.DataView.MetadataIndicatorField.Length > 0)
                {
                    this.SetMetadataCaption(MetadataElementType.Indicator, this.mUserPreference.DataView.MetadataIndicatorField, Path.Combine(this.msMaskFolder, MASK_FILE_INDICATOR));
                }

                // Set Captions for Area (Map) Metadata 
                if (this.mUserPreference.DataView.MetadataAreaField.Length > 0)
                {
                    this.SetMetadataCaption(MetadataElementType.Area, this.mUserPreference.DataView.MetadataAreaField, Path.Combine(this.msMaskFolder, MASK_FILE_AREA));
                }

                // Set Captions for Source Metadata 
                if (this.mUserPreference.DataView.MetadataSourceField.Length > 0)
                {
                    this.SetMetadataCaption(MetadataElementType.Source, this.mUserPreference.DataView.MetadataSourceField, Path.Combine(this.msMaskFolder, MASK_FILE_SOURCE));
                }

                // Set Caption for IC columns
                string[] arrSelectedICFields = this.mUserPreference.DataView.ICFields.Split(",".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
                string sICType = string.Empty;
                string sLevel = string.Empty;
                for (int i = 0; i < arrSelectedICFields.Length; i++)
                {
                    sICType = arrSelectedICFields[i].Substring(4, 2); //"CLS_SC_1"
                    sLevel = arrSelectedICFields[i].Substring(7);
                    this._MainDataTable.Columns[arrSelectedICFields[i]].Caption = "";
                    switch (sICType)
                    {
                        case "SC":
                            this._MainDataTable.Columns[arrSelectedICFields[i]].Caption = DILanguage.GetLanguageString("SECTOR") + " " + DILanguage.GetLanguageString("LEVEL") + " " + sLevel;
                            this._IUSICInfo.Columns[arrSelectedICFields[i]].Caption = DILanguage.GetLanguageString("SECTOR") + " " + DILanguage.GetLanguageString("LEVEL") + " " + sLevel;
                            break;
                        case "GL":
                            this._MainDataTable.Columns[arrSelectedICFields[i]].Caption = DILanguage.GetLanguageString("GOAL") + " " + DILanguage.GetLanguageString("LEVEL") + " " + sLevel;
                            this._IUSICInfo.Columns[arrSelectedICFields[i]].Caption = DILanguage.GetLanguageString("GOAL") + " " + DILanguage.GetLanguageString("LEVEL") + " " + sLevel;
                            break;
                        case "CF":
                            this._MainDataTable.Columns[arrSelectedICFields[i]].Caption = DILanguage.GetLanguageString("CF") + " " + DILanguage.GetLanguageString("LEVEL") + " " + sLevel;
                            this._IUSICInfo.Columns[arrSelectedICFields[i]].Caption = DILanguage.GetLanguageString("CF") + " " + DILanguage.GetLanguageString("LEVEL") + " " + sLevel;
                            break;
                        case "TH":
                            this._MainDataTable.Columns[arrSelectedICFields[i]].Caption = DILanguage.GetLanguageString("THEME") + " " + DILanguage.GetLanguageString("LEVEL") + " " + sLevel;
                            this._IUSICInfo.Columns[arrSelectedICFields[i]].Caption = DILanguage.GetLanguageString("THEME") + " " + DILanguage.GetLanguageString("LEVEL") + " " + sLevel;
                            break;
                        case "SR":
                            this._MainDataTable.Columns[arrSelectedICFields[i]].Caption = DILanguage.GetLanguageString("SOURCE") + " " + DILanguage.GetLanguageString("LEVEL") + " " + sLevel;
                            this._IUSICInfo.Columns[arrSelectedICFields[i]].Caption = DILanguage.GetLanguageString("SOURCE") + " " + DILanguage.GetLanguageString("LEVEL") + " " + sLevel;
                            break;
                        case "IT":
                            this._MainDataTable.Columns[arrSelectedICFields[i]].Caption = DILanguage.GetLanguageString("INSTITUTION") + " " + DILanguage.GetLanguageString("LEVEL") + " " + sLevel;
                            this._IUSICInfo.Columns[arrSelectedICFields[i]].Caption = DILanguage.GetLanguageString("INSTITUTION") + " " + DILanguage.GetLanguageString("LEVEL") + " " + sLevel;
                            break;
                        case "CN":
                            this._MainDataTable.Columns[arrSelectedICFields[i]].Caption = DILanguage.GetLanguageString("CONVENTION") + " " + DILanguage.GetLanguageString("LEVEL") + " " + sLevel;
                            this._IUSICInfo.Columns[arrSelectedICFields[i]].Caption = DILanguage.GetLanguageString("CONVENTION") + " " + DILanguage.GetLanguageString("LEVEL") + " " + sLevel;
                            break;
                    }
                }
            }


        }

        /// <summary>
        /// Get captions to be displayed in statusbar of dataview page
        /// </summary>
        /// <returns></returns>
        public string GetRecordCountCaption()
        {
            string RetVal = string.Empty;
            //-- Check for filter on condition
            //(this.mUserPreference.UserSelection.DataViewFilters.MostRecentData || !string.IsNullOrEmpty(this.mUserPreference.UserSelection.DataViewFilters.DeletedDataNIds) || (this.mUserPreference.UserSelection.DataViewFilters.IndicatorDataValueFilters.Count > 0 | this.mUserPreference.UserSelection.DataViewFilters.DataValueFilter.OpertorType != OpertorType.None))
            if (mUserPreference.UserSelection.DataViewFilters.FilterExists(false))
            {
                RetVal = msFilterOnTranslation + " - ";
            }
            else
            {
                RetVal = "";
            }

            if ((this._MainDataTable != null))
            {
                RetVal += msTotalRecordsLanguageString + " (" + this.PresentationRecordCount.ToString() + ")";
            }
            else
            {
                RetVal += msTotalRecordsLanguageString + " (0)";
            }

            return RetVal;
        }
        #endregion

        #endregion


    }
}
