using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Data.SqlClient;
using System.ComponentModel;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.UserSelection;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.UI.Presentations;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Common;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Common.TableGraph;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Common.TableMap;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibBAL.UI.DataViewPage;
using DevInfo.Lib.DI_LibBAL.Controls.TreeSelectionBAL;


using Microsoft.VisualBasic;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Table
{

    #region " -- Structure -- "

    /// <summary>
    /// Constants for Column of legends data table used in step 2
    /// </summary>
    /// <remarks>These constanats are ment for internal application logic, no langugae handling desired</remarks>
    public struct ColorLegendColumns
    {
        public const string CAPTION = "Caption";
        public const string LEGEND = "Legend";
        public const string FROM = "From";
        public const string TO = "To";
        public const string COUNT = "Count";
        public const string COLOR = "Color";
    }

    #endregion

    #region " -- Delegates --"

    /// <summary>
    /// Declare the delegate to update the UI on change of column arrangement
    /// </summary>
    public delegate void ChangeColumnArrangementDelegate();

    /// <summary>
    /// Declare the delegate for progree bar
    /// </summary>
    /// <param name="ParentProgress"></param>
    /// <param name="ChildProgress"></param>
    public delegate void DelegateProgressBar(int ParentProgress, int ChildProgress);

    /// <summary>
    /// Delegate for UpdateSortEvent
    /// </summary>
    public delegate void DelegateUpdateSort();

    #endregion

    /// <summary>
    /// Represents the main class for Table Wizard.
    /// <para>Represents the functionality of table wizard from step 1 to step 7</para>
    /// <para>In step 1 initialize the field collections. Field collection will contain Available, All, Rows and Column collection</para>
    /// <para>In step 2 Set the Title, Sub-title, Themes and Legend</para>
    /// <para>In step 3....</para>
    /// <para>In step 4....</para>
    /// <para>In step 5....</para>
    /// <para>In step 6 the output combining all the steps will be displayed.</para>
    /// </summary>
    /// <remarks>
    /// For desktop an instance of this class shall be held in a variable throughout the lifecycle of table wizard 
    /// For Web application instance of this class shall be held in session variable or Serialized object (XML Serialization only - To consider Medium Trust Issue)
    /// </remarks>	
    public partial class TablePresentation : ICloneable, IDisposable, ILanguage
    {

        # region " -- Private -- "

        #region " -- Variables -- "

        /// <summary>
        /// List will contain the ICNIds which are unchecked on step3
        /// </summary>
        private ArrayList DeletedICNIds = new ArrayList();

        private string RootParentChildID = string.Empty;
        private int RootParentChildIDCount = 0;

        public int ExtraColumnCount = 4;

        /// <summary>
        /// Sets the RowIndex for TableXLS
        /// </summary>
        private int NewRowIndex = 1;

        /// <summary>
        /// Contains maximum area level information in case of subnational report
        /// </summary>
        private int MaxAreaLevel = -1;
        /// <summary>
        /// Contains minimum area level information in case of subnational report
        /// </summary>
        private int MinAreaLevel = -1;

        /// <summary>
        /// The difference between maximum and minimum area level. Used when report is of subnational type.
        /// </summary>
        private int MaxMinAreaLevelDiff = 0;

        private int SuppressColumnCount = 0;

        /// <summary>
        /// User to remove single Quote problem. If This variable is true then Suppress the columns for Title/subtitle. 
        /// </summary>
        //private bool MultiTimePeriod = false;

        /// <summary>
        /// Contains the count of total Rows in XLS file
        /// </summary>
        private int TotalXLSRows = 0;

        /// <summary>
        /// Used in SuppressDuiplicate function. It contains DataValues of first column.
        /// </summary>
        /// <remarks>
        /// Suppress Duplicate will be the last function( called at last after Exceel sheet gets generatede). at that time 
        /// We suppress rows and as there may be Aggregate and IC string present , we don't want to suppress them.
        /// So this Arraylist maintains the DataValues.
        /// </remarks>
        private ArrayList ALSuppressColValues = new ArrayList();

        /// <summary>
        /// Describes whether atleast one footnote is associated with data values or not.
        /// </summary>		
        /// <remarks>
        /// None means neither Footnote nor Comment is associated with datavalues.
        /// </remarks>
        private FootnoteCommentType FootnoteCommentType = FootnoteCommentType.None;

        /// <summary>
        /// Used to set the total column required for Notes and footnotes in step 6.
        /// </summary>
        private int FootnoteCommentColumnCount = 0;

        //OPT  : Use array instead of ArrayList
        /// <summary>
        /// 
        /// </summary>
        private ArrayList ALColumnType = null;

        /// <summary>
        /// DataTable containg FootNote Information.
        /// </summary>
        private DataTable DTFootNote;

        /// <summary>
        /// Datatable Containing Comments (Notes) information.
        /// </summary>
        private DataTable DTComments;

        /// <summary>
        /// Datatable Containing sources
        /// </summary>
        private DataTable DTSources;

        //private StreamWriter SW;
        //private string LogFilePath = "C:\\-- Others --\\BALTablePresentationLogs";

        /// <summary>
        /// DataTable for Data WorkSheet.
        /// </summary>
        private DataTable DTDataSheet;

        /// <summary>
        /// Holds the records containing Area and AreaParent_NIDs.
        /// </summary>
        /// <remarks>
        /// structure
        /// -----------------------------
        /// |Area_Parent_NId | Area_Name |
        /// -----------------------------
        ///  
        /// Table will be used when aggregate function at step1 is selected as areaname, then grouping will be dne on the basis of Parent area.
        /// </remarks>
        private DataTable DTAreaParentNId;

        /// <summary>
        /// Datatable Containing relation of DataNId and NotesNId.
        /// </summary>
        /// <remarks>
        /// structure
        /// --------------------
        /// |Data_NId|Notes_NId	|
        /// --------------------
        /// </remarks>
        private DataTable DTMasterComments;

        private DataTable TempTableXLS;

        /// <summary>
        /// Holds the values of presentation data with their NIds.
        /// </summary>
        private DataTable DTImage;

        /// <summary>
        /// Holds the RowsCollection values and will be used to set images against values.
        /// </summary>
        private DataTable DTRowsForIcons = null;

        /// <summary>
        /// 
        /// </summary>
        //private DataTable _TableXLS;




        /// <summary>
        /// Contains unique Area_Parent_NIds.
        /// </summary>
        private ArrayList ALAreaParentNIds;

        /// <summary>
        /// contains unique IndicatorNIs.
        /// </summary>
        private string IndicatorNIds = string.Empty;

        /// <summary>
        /// contains unique UnitNIds.
        /// </summary>
        private string UnitNIds = string.Empty;


        /// <summary>
        /// contains unique comma delimited ICNIds (SourceNIds)
        /// </summary>
        private string ICNIds = string.Empty;

        /// <summary>
        /// contains unique AreaNIds.
        /// </summary>
        private string AreaNIds = string.Empty;

        /// <summary>
        /// contains unique SubgroupNIds.
        /// </summary>
        private string SubgroupNIds = string.Empty;

        /// <summary>
        /// String contains the fields required for sorting
        /// </summary>
        private string SortColumnArrangement = string.Empty;

        /// <summary>
        /// Footnotes display style
        /// </summary>
        private FootNoteDisplayStyle FootnoteInLine = FootNoteDisplayStyle.Inline;

        /// <summary>
        ///  Contains list of IC nodes.
        /// </summary>
        private List<SelectedNodeInfo> TempSelectedNodes = new List<SelectedNodeInfo>();

        /// <summary>
        /// Load the Connection object in the constructor
        /// </summary>
        private DIConnection DIConnection = null;

        /// <summary>
        /// Load the DIQueries object in the constructor
        /// </summary>
        private DIQueries DIQueries = null;

        /// <summary>
        /// Set true, if one want to preserve the theme.
        /// </summary>
        private bool PreserveTheme = false;

        /// <summary>
        /// 
        /// </summary>
        private bool IsPyramidChart = false;

        private DataTable PyramidSubgroupDimensions = new DataTable();

        /// <summary>
        /// Describes the Language keys and their default values.
        /// </summary>
        private class LanguageString
        {
            public string Sum = "#Sum";
            public string GrandSum = "#Grand Sum";
            public string Count = "#Count";
            public string GrandCount = "#Grand Count";
            public string Average = "#Average";
            public string GrandAverage = "#Grand Average";
            public string DataValue = "#Data Value";
            public string Table = "#TABLE";
            public string FrequencyTable = "#FrequencyTable";
            public string Data = "#Data";
            public string Source = "#Source";
            public string TimePeriod = "#TimePeriod";
            public string AreaID = "#AreaID";
            public string AreaName = "#AreaName";
            public string Indicator = "#Indicator";
            public string Unit = "#Unit";
            public string Subgroup = "#Subgroup";
            public string FootNotes = "#FootNotes";
            public string Comments = "#Comments";
            public string Maximum = "#Maximum";
            public string Minimum = "#Minimum";
            public string Chart = "Graph";
        }

        private LanguageString LanguageStrings = new LanguageString();


        /// <summary>
        /// Variables to store the information about Metadata
        /// </summary>
        private string MDIndicatorFields = string.Empty; //Comma delimited fields for indicator metadata "MD_IND_1, MD_IND_3.."
        private string MDAreaFields = string.Empty;
        private string MDSourceFields = string.Empty;
        private int SelectedICLevel = 1;

        ///// <summary>
        ///// Variables to store the information about Indicator Classification
        ///// </summary>
        private string ICInfoFields = string.Empty;

        //TODO preserve Metadata fields and IC Fileds while serialization

        #endregion

        #region "-- Constant --"

        private const string DistinctThemeIUS = "DistinctIUS";
        private const string IUSName = "IUSName";
        private const string Seprator = " - ";

        #endregion

        #region " -- Methods -- "

        #region " ---  STEP 1 ---"

        /// <summary>
        /// Fill the Fields collection named ALL.
        /// <para>Will be used for filling up the Rows,Columns and available collection</para>
        /// </summary>
        private void FillAllFields()
        {
            if (this.IsPyramidChart == false)
            {
                this._Fields.All.Add(new Field(Area.AreaID, DILanguage.GetLanguageString(AREAID)));
                this._Fields.All.Add(new Field(IndicatorClassifications.ICName, DILanguage.GetLanguageString(SOURCE)));
                this._Fields.All.Add(new Field(SubgroupVals.SubgroupVal, DILanguage.GetLanguageString(SUBGROUP)));
                this._Fields.All.Add(new Field(Data.DataDenominator, DILanguage.GetLanguageString(DENOMINATOR)));
                this._Fields.All.Add(new Field(Unit.UnitName, DILanguage.GetLanguageString(UNIT)));
                this._Fields.All.Add(new Field(Indicator.IndicatorName, DILanguage.GetLanguageString(INDICATOR)));
                this._Fields.All.Add(new Field(TablePresentation.NONE, DILanguage.GetLanguageString(NONE)));
                this._Fields.All.Add(new Field(Data.DataValue, DILanguage.GetLanguageString(DATAVALUE)));

                foreach (DataRow DR in this.DIDataView.SubgroupInfo.Rows)
                {
                    this._Fields.All.Add(new Field(DR[SubgroupInfoColumns.Name].ToString(), DR[SubgroupInfoColumns.Caption].ToString()));
                }
            }
            else
            {
                this.PyramidSubgroupDimensions = this.UpdateSubgroupDimensions();
                foreach (DataRow DR in this.PyramidSubgroupDimensions.Rows)
                {
                    this._Fields.All.Add(new Field(DR[SubgroupInfoColumns.Name].ToString(), DR[SubgroupInfoColumns.Caption].ToString()));
                    this._Fields.Available.Add(new Field(DR[SubgroupInfoColumns.Name].ToString(), DR[SubgroupInfoColumns.Caption].ToString()));
                }
            }

            this._Fields.All.Add(new Field(Area.AreaName, DILanguage.GetLanguageString(AREANAME)));
            this._Fields.All.Add(new Field(Timeperiods.TimePeriod, DILanguage.GetLanguageString(TIMEPERIOD)));
        }

        /// <summary>
        ///  Create the table for aggregate fields and define its column.
        /// </summary>
        private void SetAggregateTable()
        {
            this._AggregatesFields = new DataTable("AggregatesFields");
            this._AggregatesFields.Columns.Add(FieldIds);
            this._AggregatesFields.Columns.Add(FieldCaption);
            DataRow Row;
            Row = this._AggregatesFields.NewRow();
            Row[0] = TablePresentation.NONE;
            Row[1] = this._Fields.All[TablePresentation.NONE].Caption;
            this._AggregatesFields.Rows.Add(Row);
            this._AggregatesFields.AcceptChanges();
        }

        /// <summary>
        /// Arrange fields in rows and columns for pyramid 
        /// </summary>
        private void ArrangeCollectionFieldsForPyramid(DataView dVPresentationData)
        {
            string SexFieldGId = string.Empty;
            string AgeFieldGId = string.Empty;
            string SearchKey = string.Empty;
            string[] Param = new string[1];
            bool IsSexExists = false;
            bool IsAgeGroupExists = false;
            DataTable dtValue = new DataTable();


            //-- Add the field in the available collection
            this._Fields.Available.Add(new Field(Area.AreaName, DILanguage.GetLanguageString(AREANAME)));
            this._Fields.Available.Add(new Field(Timeperiods.TimePeriod, DILanguage.GetLanguageString(TIMEPERIOD)));

            // -- BUILD ROW COLLECTION -- 
            // -- Check Subgroup GID for Age, If found add in the Rows            
            foreach (DataRow Row in this.PyramidSubgroupDimensions.Rows)
            {
                if (Row[SubgroupInfoColumns.GIdValue].ToString() == UserPreference.General.AgeGroupGId)
                {
                    IsAgeGroupExists = true;
                    _Fields.Rows.Add(_Fields.All[Row[SubgroupInfoColumns.Name].ToString()]);
                    _Fields.Sort.Add(_Fields.All[Row[SubgroupInfoColumns.Name].ToString()]);
                    AgeFieldGId = Row[SubgroupInfoColumns.GIdValue].ToString();
                    break;
                }
            }

            // -- BUILD Column COLLECTION --  
            // -- Check Subgroup GID for SEX, If found add in the columns and available
            foreach (DataRow Row in this.PyramidSubgroupDimensions.Rows)
            {
                if (Row[SubgroupInfoColumns.GIdValue].ToString() == UserPreference.General.SexGroupGId)
                {
                    IsSexExists = true;
                    _Fields.Columns.Add(_Fields.All[Row[SubgroupInfoColumns.Name].ToString()]);
                    SexFieldGId = Row[SubgroupInfoColumns.GIdValue].ToString();
                    break;
                }
            }

            // -- If AGE is not in the SubgroupInfo then add any other SubgroupInfo value
            if (!IsAgeGroupExists)
            {
                foreach (DataRow Row in this.PyramidSubgroupDimensions.Rows)
                {
                    if (AgeFieldGId != Row[SubgroupInfoColumns.GIdValue].ToString() && SexFieldGId != Row[SubgroupInfoColumns.GIdValue].ToString())
                    {
                        _Fields.Rows.Add(_Fields.All[Row[SubgroupInfoColumns.Name].ToString()]);
                        _Fields.Sort.Add(_Fields.All[Row[SubgroupInfoColumns.Name].ToString()]);
                        AgeFieldGId = Row[SubgroupInfoColumns.GIdValue].ToString();
                        break;
                    }
                }
            }

            // -- If GID does not matches the check Subgroup Name and if found then add in columns
            if (!IsSexExists)
            {
                foreach (DataRow dr in this.PyramidSubgroupDimensions.Rows)
                {
                    if (AgeFieldGId != dr[SubgroupInfoColumns.GIdValue].ToString() && SexFieldGId != dr[SubgroupInfoColumns.GIdValue].ToString())
                    {
                        _Fields.Columns.Add(_Fields.All[dr[SubgroupInfoColumns.Name].ToString()]);
                        break;
                    }
                }
            }

            if (_Fields.Rows.Count == 0)
            {
                _Fields.Rows.Add(_Fields.All[Timeperiods.TimePeriod]);
            }

            //-- Add the area in the column collection, if no field was present or if multiple area exists
            Param[0] = Area.AreaName;
            dtValue = this.PresentationData.ToTable(true, Param);
            if (_Fields.Columns.Count == 0 || dtValue.Rows.Count > 1)
            {
                _Fields.Columns.Add(_Fields.All[Area.AreaName]);
            }

            //-- Sort the column arrangement
            this.SortColumnArrangementTable();
        }

        /// <summary>
        /// Arrange the default collection field into Rows, Columns and Available.
        /// </summary>
        /// <remarks>
        /// </remarks>
        private void ArrangeCollectionFields(DataView dVPresentationData)
        {

            /// Step 1: Get the default data view derived from User Selection</para>
            /// Step 2: Find the total number of rows in the dataview.</para>
            /// Step 3: Auto Arrange the Fields. Identify the Fields to be mioved into Rows or Columns. 
            /// Fields with unique value are the candidate for auto-arrange.
            /// Prepare a filter string for the field that will filter the view on the bases of field NID</para>
            /// Step 4: Apply filter and go to step 3.</para>
            /// Step 5: after applying filters check whether total rows of the view before applying filter are equal to total rows after applying filter.</para>
            /// Step 6: If yes then move the field according in rows or columns.</para>
            /// Step 7: If not then that means the field is available in all rows , so move that into available.</para>
            ///	Step 8: Reset the dataview</para>


            // By default Row Collection can contain AreaName, Source and TimePeriod if their multiple values exists in presentation data
            // By default Column Collection can contain Indiactor, Unit, SubgroupVal and Subgroups(Sex, Age Location, Others) 

            int TotalRows;
            bool FieldFound;
            string SearchKey = string.Empty;

            //Used to apply filter on the dataview
            string RowFilter = string.Empty;

            //Get the total number of rows from the dataview
            TotalRows = dVPresentationData.Count;

            // -- BUILD ROW COLLECTION --

            //AREAS
            RowFilter = Area.AreaID + "='" + DICommon.RemoveQuotes(dVPresentationData[0][Area.AreaID].ToString()) + "'";
            dVPresentationData.RowFilter = RowFilter;
            //if total rows before appying filter is less the total rows after applying filter this means multiple area exists in dataview
            //if multiple areas exists then move the field in the rows collection else move it into available collection.
            // By default, AreaID will always remain in the Available list
            _Fields.Available.Add(_Fields.All[Area.AreaID]);
            if (TotalRows > dVPresentationData.Count)
            {
                _Fields.Rows.Add(_Fields.All[Area.AreaName]);

                // -- update the sorting order.

                // -- Add the filed in Sort Collection
                _Fields.Sort.Add(_Fields.All[Area.AreaName]);
            }
            else
            {
                _Fields.Available.Add(_Fields.All[Area.AreaName]);
            }
            dVPresentationData.RowFilter = string.Empty;
            RowFilter = string.Empty;

            ////TIMEPERIOD
            RowFilter = Timeperiods.TimePeriodNId + "=" + dVPresentationData[0][Timeperiods.TimePeriodNId].ToString();
            dVPresentationData.RowFilter = RowFilter;
            //if there is no selection of rows or if total rows before appying filter is less the total rows after applying filter
            //then move the field in the available collectio else move it into rows collection.
            if (TotalRows > dVPresentationData.Count)
            {
                _Fields.Rows.Add(_Fields.All[Timeperiods.TimePeriod]);

                // -- Add the filed in Sort Collection
                _Fields.Sort.Add(_Fields.All[Timeperiods.TimePeriod]);
            }
            else
            {
                _Fields.Available.Add(_Fields.All[Timeperiods.TimePeriod]);
            }
            dVPresentationData.RowFilter = string.Empty;
            RowFilter = string.Empty;

            ////SOURCE
            RowFilter = IndicatorClassifications.ICNId + "=" + dVPresentationData[0][IndicatorClassifications.ICNId].ToString();
            dVPresentationData.RowFilter = RowFilter;
            //if there is no selection of rows or if total rows before appying filter is less the total rows after applying filter
            //then move the field in the available collectio else move it into rows collection.
            if (TotalRows > dVPresentationData.Count)
            {
                _Fields.Rows.Add(_Fields.All[IndicatorClassifications.ICName]);

                // -- Add the filed in Sort Collection
                _Fields.Sort.Add(_Fields.All[IndicatorClassifications.ICName]);
            }
            else
            {
                _Fields.Available.Add(_Fields.All[IndicatorClassifications.ICName]);
            }
            dVPresentationData.RowFilter = string.Empty;
            RowFilter = string.Empty;


            // -- BUILD COLUMN COLLECTION --
            //INDICATOR
            RowFilter = Indicator.IndicatorNId + "=" + dVPresentationData[0][Indicator.IndicatorNId].ToString();
            dVPresentationData.RowFilter = RowFilter;
            //if there is no selection of rows or if total rows before appying filter is less the total rows after applying filter
            //then move the field in the available collection else move it into rows collection.
            if (TotalRows > dVPresentationData.Count)
            {
                _Fields.Columns.Add(_Fields.All[Indicator.IndicatorName]);
            }
            else
            {
                _Fields.Available.Add(_Fields.All[Indicator.IndicatorName]);
            }
            dVPresentationData.RowFilter = string.Empty;
            RowFilter = string.Empty;

            ////UNIT
            RowFilter = Unit.UnitNId + "=" + dVPresentationData[0][Unit.UnitNId].ToString();
            dVPresentationData.RowFilter = RowFilter;
            //if there is no selection of rows or if total rows before appying filter is less the total rows after applying filter
            //then move the field in the available collectio else move it into rows collection.
            if (TotalRows > dVPresentationData.Count)
            {
                _Fields.Columns.Add(_Fields.All[Unit.UnitName]);
            }
            else
            {
                _Fields.Available.Add(_Fields.All[Unit.UnitName]);
            }
            dVPresentationData.RowFilter = string.Empty;
            RowFilter = string.Empty;

            ///SUBGROUP
            RowFilter = SubgroupVals.SubgroupValNId + "=" + dVPresentationData[0][SubgroupVals.SubgroupValNId].ToString();
            dVPresentationData.RowFilter = RowFilter;
            //if there is no selection of rows or if total rows before appying filter is less the total rows after applying filter
            //then move the field in the available collectio else move it into rows collection.
            if (TotalRows > dVPresentationData.Count)
            {
                _Fields.Columns.Add(_Fields.All[SubgroupVals.SubgroupVal]);
            }
            else
            {
                _Fields.Available.Add(_Fields.All[SubgroupVals.SubgroupVal]);
            }
            dVPresentationData.RowFilter = string.Empty;
            RowFilter = string.Empty;


            foreach (DataRow dr in this.DIDataView.SubgroupInfo.Rows)
            {
                _Fields.Available.Add(_Fields.All[dr[SubgroupInfoColumns.Name].ToString()]);
            }

            if (_Fields.Rows.Count == 0)
            {
                _Fields.Rows.Add(_Fields.All[Area.AreaName]);
                Field Field;
                Field = this._Fields.All[Area.AreaName];
                _Fields.Available.Remove(Field);
            }

            // -- Data value is mandatory field for Step 4
            _Fields.Sort.Add(_Fields.All[Data.DataValue]);
        }

        #endregion

        #region " ---  STEP 2 ---"

        /// <summary>
        /// Set the title and subtitle of table according to precedence.
        /// <para>Step 1:Check for every field according to precedence.</para>
        /// <para>If title is not set then set the title from the field object's caption</para>
        /// <papa>If title is set then set the subtitle.</papa>
        /// </summary>		
        public void SetTitleAndSubtitle()
        {
            bool IndAdded = false;
            bool SubgrpAdded = false;
            bool UntAdded = false;
            int SubTitleCount = 0;
            this._Title = string.Empty;
            this._Subtitle = string.Empty;
            if (this.PresentationData != null)
            {

                //Set Title -- Use Indicator Name and AreaName to set title. If both are absent then use the word "TITLE"
                if (this._Fields.Available[Indicator.IndicatorName] != null)
                {
                    if (this.Title == string.Empty)
                    {
                        this.Title = this.PresentationData[0][Indicator.IndicatorName].ToString();
                    }
                }
                if (this._Fields.Available[Area.AreaName] != null)
                {
                    if (this.Title == string.Empty)
                    {
                        this.Title = this.PresentationData[0][Area.AreaName].ToString();
                    }
                    else
                    {
                        this.Title = this.Title + " - " + this.PresentationData[0][Area.AreaName].ToString();
                    }
                }
                if (this.Title == string.Empty)
                {
                    this.Title = DILanguage.GetLanguageString("TITLE").ToString();
                }

                //Set the subtitle. Use Unit,Subgroup and TimePeriod to set subtitle. if all are absent then leave it blank
                if (this._Fields.Available[Unit.UnitName] != null)
                {
                    if (this.Subtitle == string.Empty)
                    {
                        this.Subtitle = this.PresentationData[0][Unit.UnitName].ToString();
                    }
                }
                if (this._Fields.Available[SubgroupVals.SubgroupVal] != null)
                {
                    if (this.Subtitle == string.Empty)
                    {
                        this.Subtitle = this.PresentationData[0][SubgroupVals.SubgroupVal].ToString();
                    }
                    else
                    {
                        this.Subtitle = this.Subtitle + " - " + this.PresentationData[0][SubgroupVals.SubgroupVal].ToString();
                    }
                }
                if (this._Fields.Available[Timeperiods.TimePeriod] != null)
                {
                    if (this.Subtitle == string.Empty)
                    {
                        this.Subtitle = this.PresentationData[0][Timeperiods.TimePeriod].ToString();
                    }
                    else
                    {
                        this.Subtitle = this.Subtitle + " - " + this.PresentationData[0][Timeperiods.TimePeriod].ToString();
                    }
                }
            }
        }

        #endregion

        #region "-- STEP 3 --"

        /// <summary>
        /// Create the table structure of column headers
        /// </summary>
        private DataTable PrepareColumnHeaderStructure()
        {
            //Will contain the structure of Column Headers.
            DataTable RetVal = new DataTable("Header Table");

            SortColumnArrangement = string.Empty;
            string ColumnGID = string.Empty;
            int GIdColumnIndex = 1;

            if (_Fields.Columns.Count > 0)
            {
                //Iterate through each field in Column collection and add the columns in the table RetVal
                foreach (Field Field in _Fields.Columns)
                {
                    if (SortColumnArrangement.Length > 0)
                    {
                        SortColumnArrangement += ",";
                    }
                    RetVal.Columns.Add(Field.FieldID);
                    SortColumnArrangement += Field.FieldID;
                    RetVal.Columns.Add("Column_" + GIdColumnIndex.ToString(), typeof(string));
                    GIdColumnIndex += 1;
                }
            }
            // -- To save the sorting order of column arrangement
            RetVal.Columns.Add(RecordId, typeof(int));
            RetVal.Columns.Add(Order, typeof(int));
            //if (_Fields.Columns.Count > 0)
            //{
            //    //Iterate through each field in Column collection and add the columns in the table RetVal
            //    foreach (Field Field in _Fields.Columns)
            //    {						
            //        RetVal.Columns.Add(DisplayText + Field.FieldID);										
            //    }
            //}			
            return RetVal;
        }

        /// <summary>
        /// Sets the Property ColumnArrangementTable with all Column Values. 
        /// </summary>
        /// <param name="arrangementType">True, for new table wizard and False, for Loading the report.</param>
        private DataTable SetColumnArrangement(bool arrangementType)
        {
            //Prepare the Column structure.
            DataTable RetVal = PrepareColumnHeaderStructure();
            DTImage = PrepareColumnHeaderStructure();

            try
            {
                //to prevent the duplicate entry.
                string TableFilter = string.Empty;
                int Counter;

                //Will contain the record for NewRowdtColumnHeaders 
                DataRow NewRowDTColumnHeaders;

                DataRow RowDTFieldNIds;

                //Get the default dataview from which the data has to be transferred into table TableColumnHeaders
                //DataView DVFields = this._PresentationData;

                int RecordId = 0;

                //Iterate through the dataview and transfer the unique records into TableColumnHeaders
                string PreviousSort = this.PresentationData.Sort;
                this.PresentationData.Sort = Indicator.IndicatorName;

                foreach (DataRowView RowViewDVFields in this.PresentationData)
                {
                    TableFilter = string.Empty;

                    //Itereate though each column in TableColumnHeaders for RowViewDVFields and
                    //create the filer to be applied when inserting records into TableColumnHeaders.
                    //"-3" bec TableColumnHeaders contains RecordNo and order number field also.
                    for (Counter = 0; Counter <= RetVal.Columns.Count - 3; Counter++)
                    {
                        //Leave the metadata columns                       
                        if (!RetVal.Columns[Counter].ColumnName.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.Metadata) && !RetVal.Columns[Counter].ColumnName.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.IC))
                        {
                            if (TableFilter != string.Empty)
                            {
                                TableFilter += " AND ";
                            }
                            if (Counter % 2 != 0)
                            {
                                //Add columnNid Name next to column name based on column name added
                                if (RetVal.Columns[Counter - 1].ColumnName == Area.AreaID)
                                {
                                    TableFilter += this.AddSlash(RetVal.Columns[Counter].ColumnName) + "='" + Utility.DICommon.RemoveQuotes(RowViewDVFields[Area.AreaID].ToString()) + "'";
                                }
                                else if (RetVal.Columns[Counter - 1].ColumnName == Area.AreaName)
                                {
                                    TableFilter += this.AddSlash(RetVal.Columns[Counter].ColumnName) + "='" + Utility.DICommon.RemoveQuotes(RowViewDVFields[Area.AreaID].ToString()) + "'";
                                }
                                else if (RetVal.Columns[Counter - 1].ColumnName == IndicatorClassifications.ICName)
                                {
                                    TableFilter += this.AddSlash(RetVal.Columns[Counter].ColumnName) + "='" + Utility.DICommon.RemoveQuotes(RowViewDVFields[IndicatorClassifications.ICName].ToString()) + "'";
                                }
                                else if (RetVal.Columns[Counter - 1].ColumnName == Timeperiods.TimePeriod)
                                {
                                    TableFilter += this.AddSlash(RetVal.Columns[Counter].ColumnName) + "='" + Utility.DICommon.RemoveQuotes(RowViewDVFields[Timeperiods.TimePeriod].ToString()) + "'";
                                }
                                else if (RetVal.Columns[Counter - 1].ColumnName == Indicator.IndicatorName)
                                {
                                    TableFilter += this.AddSlash(RetVal.Columns[Counter].ColumnName) + "='" + Utility.DICommon.RemoveQuotes(RowViewDVFields[Indicator.IndicatorGId].ToString()) + "'";
                                }
                                else if (RetVal.Columns[Counter - 1].ColumnName == Unit.UnitName)
                                {
                                    TableFilter += this.AddSlash(RetVal.Columns[Counter].ColumnName) + "='" + Utility.DICommon.RemoveQuotes(RowViewDVFields[Unit.UnitGId].ToString()) + "'";
                                }
                                else if (RetVal.Columns[Counter - 1].ColumnName == SubgroupVals.SubgroupVal)
                                {
                                    TableFilter += this.AddSlash(RetVal.Columns[Counter].ColumnName) + "='" + Utility.DICommon.RemoveQuotes(RowViewDVFields[SubgroupVals.SubgroupValGId].ToString()) + "'";
                                }
                                else
                                {
                                    bool subgroupfound = false;
                                    foreach (DataRow dr in this.DIDataView.SubgroupInfo.Rows)
                                    {
                                        if (RetVal.Columns[Counter - 1].ColumnName == dr[SubgroupInfoColumns.Name].ToString())
                                        {
                                            TableFilter += this.AddSlash(RetVal.Columns[Counter].ColumnName) + "='" + Utility.DICommon.RemoveQuotes(RowViewDVFields[dr[SubgroupInfoColumns.GId].ToString()].ToString()) + "'";
                                            subgroupfound = true;
                                            break;
                                        }
                                    }
                                    if (!subgroupfound)
                                    {
                                        TableFilter += this.AddSlash(RetVal.Columns[Counter].ColumnName) + "=' '";
                                    }
                                }
                            }
                            else
                            {
                                TableFilter += this.AddSlash(RetVal.Columns[Counter].ColumnName) + "='" + Utility.DICommon.RemoveQuotes(RowViewDVFields[RetVal.Columns[Counter].ColumnName].ToString()) + "'";
                            }
                        }
                    }
                    //Check if the record already exists or not  in TableColumnHeaders.
                    if (RetVal.Select(TableFilter).Length == 0)
                    {
                        NewRowDTColumnHeaders = RetVal.NewRow();
                        RowDTFieldNIds = DTImage.NewRow();
                        //"-2" bec TableColumnHeaders contains RecordNo and order number field also.
                        for (Counter = 0; Counter < RetVal.Columns.Count - 2; Counter++)
                        {
                            if (!RetVal.Columns[Counter].ColumnName.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.Metadata) && !RetVal.Columns[Counter].ColumnName.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.IC))
                            {
                                if (Counter % 2 == 0)
                                {
                                    NewRowDTColumnHeaders[RetVal.Columns[Counter].ColumnName] = RowViewDVFields[RetVal.Columns[Counter].ColumnName].ToString();

                                    if (RetVal.Columns[Counter].ColumnName == Area.AreaName)
                                    {
                                        RowDTFieldNIds[RetVal.Columns[Counter].ColumnName] = IconElementType.Area;
                                    }
                                    else if (RetVal.Columns[Counter].ColumnName == IndicatorClassifications.ICName)
                                    {
                                        RowDTFieldNIds[RetVal.Columns[Counter].ColumnName] = IconElementType.IndicatorClassification;
                                    }
                                    else if (RetVal.Columns[Counter].ColumnName == Indicator.IndicatorName)
                                    {
                                        RowDTFieldNIds[RetVal.Columns[Counter].ColumnName] = IconElementType.Indicator;
                                    }
                                    else if (RetVal.Columns[Counter].ColumnName == Unit.UnitName)
                                    {
                                        RowDTFieldNIds[RetVal.Columns[Counter].ColumnName] = IconElementType.Unit;
                                    }
                                    else if (RetVal.Columns[Counter].ColumnName == SubgroupVals.SubgroupVal)
                                    {
                                        RowDTFieldNIds[RetVal.Columns[Counter].ColumnName] = IconElementType.SubgroupVals;
                                    }
                                    else
                                    {
                                        RowDTFieldNIds[RetVal.Columns[Counter].ColumnName] = string.Empty;
                                    }
                                }
                                else	 //Insert GIDs in case of ColumnArrangement and NIds in case of FieldNIds.
                                {
                                    if (RetVal.Columns[Counter - 1].ColumnName == Area.AreaID)
                                    {
                                        NewRowDTColumnHeaders[RetVal.Columns[Counter].ColumnName] = RowViewDVFields[Area.AreaID].ToString();
                                        RowDTFieldNIds[RetVal.Columns[Counter].ColumnName] = string.Empty;
                                    }
                                    else if (RetVal.Columns[Counter - 1].ColumnName == Area.AreaName)
                                    {
                                        NewRowDTColumnHeaders[RetVal.Columns[Counter].ColumnName] = RowViewDVFields[Area.AreaID].ToString();
                                        RowDTFieldNIds[RetVal.Columns[Counter].ColumnName] = RowViewDVFields[Area.AreaNId].ToString();
                                    }
                                    else if (RetVal.Columns[Counter - 1].ColumnName == IndicatorClassifications.ICName)
                                    {
                                        NewRowDTColumnHeaders[RetVal.Columns[Counter].ColumnName] = RowViewDVFields[IndicatorClassifications.ICName].ToString();
                                        RowDTFieldNIds[RetVal.Columns[Counter].ColumnName] = RowViewDVFields[IndicatorClassifications.ICNId].ToString();
                                    }
                                    else if (RetVal.Columns[Counter - 1].ColumnName == Timeperiods.TimePeriod)
                                    {
                                        NewRowDTColumnHeaders[RetVal.Columns[Counter].ColumnName] = RowViewDVFields[Timeperiods.TimePeriod].ToString();
                                        RowDTFieldNIds[RetVal.Columns[Counter].ColumnName] = string.Empty;
                                    }
                                    else if (RetVal.Columns[Counter - 1].ColumnName == Indicator.IndicatorName)
                                    {
                                        NewRowDTColumnHeaders[RetVal.Columns[Counter].ColumnName] = RowViewDVFields[Indicator.IndicatorGId].ToString();
                                        RowDTFieldNIds[RetVal.Columns[Counter].ColumnName] = RowViewDVFields[Indicator.IndicatorNId].ToString();
                                    }
                                    else if (RetVal.Columns[Counter - 1].ColumnName == Unit.UnitName)
                                    {
                                        NewRowDTColumnHeaders[RetVal.Columns[Counter].ColumnName] = RowViewDVFields[Unit.UnitGId].ToString();
                                        RowDTFieldNIds[RetVal.Columns[Counter].ColumnName] = RowViewDVFields[Unit.UnitNId].ToString();
                                    }
                                    else if (RetVal.Columns[Counter - 1].ColumnName == SubgroupVals.SubgroupVal)
                                    {
                                        NewRowDTColumnHeaders[RetVal.Columns[Counter].ColumnName] = RowViewDVFields[SubgroupVals.SubgroupValGId].ToString();
                                        RowDTFieldNIds[RetVal.Columns[Counter].ColumnName] = RowViewDVFields[SubgroupVals.SubgroupValNId].ToString();
                                    }
                                    else
                                    {
                                        bool subgroupfound = false;
                                        foreach (DataRow dr in this.DIDataView.SubgroupInfo.Rows)
                                        {
                                            if (RetVal.Columns[Counter - 1].ColumnName == dr[SubgroupInfoColumns.Name].ToString())
                                            {
                                                NewRowDTColumnHeaders[RetVal.Columns[Counter].ColumnName] = RowViewDVFields[dr[SubgroupInfoColumns.GId].ToString()].ToString();
                                                RowDTFieldNIds[RetVal.Columns[Counter].ColumnName] = string.Empty;
                                                subgroupfound = true;
                                                break;
                                            }
                                        }
                                        if (!subgroupfound)
                                        {
                                            NewRowDTColumnHeaders[RetVal.Columns[Counter].ColumnName] = string.Empty;
                                            RowDTFieldNIds[RetVal.Columns[Counter].ColumnName] = string.Empty;
                                        }
                                    }
                                }
                            }
                            // Leave the metadata GID column
                            else if (!RetVal.Columns[Counter].ColumnName.EndsWith("_GID"))
                            {
                                Hashtable FieldContent = new Hashtable();
                                Hashtable CLS_ICContent = new Hashtable();
                                // Indicator Metadata
                                if (RetVal.Columns[Counter].ColumnName.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.MetadataIndicator))
                                {
                                    foreach (DataRow Drow in this.DIDataView.MetadataIndicator.Select(Indicator.IndicatorNId + " = " + RowViewDVFields[Indicator.IndicatorNId].ToString()))
                                    {
                                        NewRowDTColumnHeaders[RetVal.Columns[Counter].ColumnName] = Drow[RetVal.Columns[Counter].ColumnName].ToString();
                                        break;
                                    }
                                }
                                // Area Metadata
                                else if (RetVal.Columns[Counter].ColumnName.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.MetadataArea))
                                {
                                    foreach (DataRow Drow in this.DIDataView.MetadataArea.Select(Area.AreaNId + " = " + RowViewDVFields[Area.AreaNId].ToString()))
                                    {
                                        NewRowDTColumnHeaders[RetVal.Columns[Counter].ColumnName] = Drow[RetVal.Columns[Counter].ColumnName].ToString();
                                        break;
                                    }
                                }
                                // Source Metadata
                                else if (RetVal.Columns[Counter].ColumnName.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.MetadataSource))
                                {
                                    foreach (DataRow Drow in this.DIDataView.MetadataSource.Select(IndicatorClassifications.ICNId + " = " + RowViewDVFields[IndicatorClassifications.ICNId].ToString()))
                                    {
                                        NewRowDTColumnHeaders[RetVal.Columns[Counter].ColumnName] = Drow[RetVal.Columns[Counter].ColumnName].ToString();
                                        break;
                                    }
                                }
                                //IC Info
                                else if (RetVal.Columns[Counter].ColumnName.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.IC))
                                {
                                    foreach (DataRow Drow in this.DIDataView.IUSICInfo.Select(Indicator_Unit_Subgroup.IUSNId + " = " + RowViewDVFields[Indicator_Unit_Subgroup.IUSNId].ToString()))
                                    {
                                        NewRowDTColumnHeaders[RetVal.Columns[Counter].ColumnName] = Drow[RetVal.Columns[Counter].ColumnName].ToString();
                                        break;
                                    }
                                }
                            }
                        }

                        bool EmptyColumn = false;
                        //-- If all the value in the array are blank or null, then we will not add into the column arrangement table
                        for (int i = 0; i < NewRowDTColumnHeaders.ItemArray.Length - 2; i++)
                        {
                            if (string.IsNullOrEmpty(NewRowDTColumnHeaders[i].ToString()))
                            {
                                EmptyColumn = true;
                            }
                            else
                            {
                                EmptyColumn = false;
                                break;
                            }
                        }

                        if (!EmptyColumn)
                        {
                            // -- Set the Record Id and order no of column arrangement.
                            NewRowDTColumnHeaders[RetVal.Columns[Counter].ColumnName] = RecordId;
                            RowDTFieldNIds[DTImage.Columns[Counter].ColumnName] = RecordId;
                            NewRowDTColumnHeaders[RetVal.Columns[Counter + 1].ColumnName] = RecordId;
                            RowDTFieldNIds[DTImage.Columns[Counter + 1].ColumnName] = RecordId;

                            RetVal.Rows.Add(NewRowDTColumnHeaders);
                            DTImage.Rows.Add(RowDTFieldNIds);
                            NewRowDTColumnHeaders = null;
                            RecordId += 1;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(RowViewDVFields[Indicator.IndicatorNId].ToString()))
                            {
                                NewRowDTColumnHeaders[RetVal.Columns[Counter].ColumnName] = RecordId;
                                RowDTFieldNIds[DTImage.Columns[Counter].ColumnName] = RecordId;
                                NewRowDTColumnHeaders[RetVal.Columns[Counter + 1].ColumnName] = RecordId;
                                RowDTFieldNIds[DTImage.Columns[Counter + 1].ColumnName] = RecordId;
                                RetVal.Rows.Add(NewRowDTColumnHeaders);
                                DTImage.Rows.Add(RowDTFieldNIds);
                                NewRowDTColumnHeaders = null;
                                RecordId += 1;
                            }
                        }
                    }
                }
                this.PresentationData.Sort = PreviousSort;
                RetVal.AcceptChanges();

                //-- Sort the column arrangement.
                DataView DvSort = new DataView();
                DvSort = RetVal.DefaultView;
                DvSort.Sort = SortColumnArrangement;
                RetVal = UpdateSortOrder(DvSort.ToTable());
            }
            catch (Exception ex)
            {
                RetVal = null;
            }

            //Set the order of Subgroup dimention and Subgroup Vals
            if (RetVal != null)
            {
                try
                {
                    DataTable DTRetVal = RetVal.Clone();
                    DataTable DTIntermediate = RetVal.Clone();
                    string LastItem = "CurrentItem";
                    string CurrentItem = "CurrentItem";
                    ArrayList ALColumnValues = new ArrayList();
                    int RowCounter = 0;
                    int TotalRetValRowsCount = RetVal.Rows.Count;
                    //Loop through each item in the Column arrangement table so that each item can be sorted
                    for (int Counter = 0; Counter <= RetVal.Columns.Count - 3; Counter = Counter + 2)
                    {
                        LastItem = "CurrentItem";
                        CurrentItem = "CurrentItem";
                        DTRetVal.Clear();
                        DTIntermediate.Clear();
                        //If the current column is of Subgroup vals and not for sungroup dimensions
                        if (RetVal.Columns[Counter].ColumnName == SubgroupVals.SubgroupVal)
                        {
                            //Sort the SubgroupVals table on Sort Order this can be referred for sorting in columnArrangement table
                            RowCounter = 0;
                            this.DIDataView.SubgroupVals.DefaultView.Sort = SubgroupVals.SubgroupValOrder;
                            DataTable DTSubgroupVals = this.DIDataView.SubgroupVals.DefaultView.ToTable();
                            foreach (DataRow dr in RetVal.Rows)
                            {
                                RowCounter++;
                                if (Counter == 0)
                                {
                                    CurrentItem = "CurrentItem";
                                }
                                else
                                {
                                    CurrentItem = dr[Counter - 2].ToString();
                                }
                                if (CurrentItem == LastItem && RowCounter != TotalRetValRowsCount)
                                {
                                    DTIntermediate.ImportRow(dr);
                                }
                                else if (CurrentItem != LastItem || RowCounter == TotalRetValRowsCount)
                                {
                                    //If Row is last and current Item is same as before for previous column 
                                    //then Insert import last row also before applying order                                    
                                    if (RowCounter == TotalRetValRowsCount && CurrentItem == LastItem)
                                    {
                                        DTIntermediate.ImportRow(dr);
                                    }
                                    //For the current group of subgroupVal values. Club all unique subgroupVal values in chunks into DTRetVal 
                                    //which will further used to merge into RetVal
                                    foreach (DataRow drSub in DTSubgroupVals.Rows)
                                    {
                                        foreach (DataRow drIntermediate in DTIntermediate.Select(SubgroupVals.SubgroupVal + "='" + DICommon.RemoveQuotes(Convert.ToString(drSub[SubgroupVals.SubgroupVal])) + "'"))
                                        {
                                            DTRetVal.ImportRow(drIntermediate);
                                        }
                                    }
                                    if (RowCounter == TotalRetValRowsCount && CurrentItem != LastItem)
                                    {
                                        DTRetVal.ImportRow(dr);
                                    }
                                    DTIntermediate.Clear();
                                    DTIntermediate.ImportRow(dr);
                                    LastItem = CurrentItem;
                                }
                            }
                        }
                        else
                        {
                            foreach (DataRow DrSI in this.DIDataView.SubgroupInfo.Rows)
                            {
                                if (RetVal.Columns[Counter].ColumnName == DrSI[SubgroupInfoColumns.Name].ToString())
                                {
                                    //Filter the datatable based on Subgroup Dimention Column Name
                                    DataTable DTSubDimension = this.DIDataView.SubGroupDataInfo.Clone();
                                    this.DIDataView.SubGroupDataInfo.DefaultView.RowFilter = SubgroupInfoColumns.Name + "='" + DrSI[SubgroupInfoColumns.Name].ToString() + "'";
                                    //Sort the SubgroupDataInfo table on Sort Order
                                    this.DIDataView.SubGroupDataInfo.DefaultView.Sort = Subgroup.SubgroupOrder;
                                    DTSubDimension = this.DIDataView.SubGroupDataInfo.DefaultView.ToTable();
                                    this.DIDataView.SubGroupDataInfo.DefaultView.RowFilter = string.Empty;
                                    RowCounter = 0;
                                    TotalRetValRowsCount = RetVal.Rows.Count;
                                    foreach (DataRow dr in RetVal.Rows)
                                    {
                                        RowCounter++;
                                        if (Counter == 0)
                                        {
                                            CurrentItem = "CurrentItem";
                                        }
                                        else
                                        {
                                            CurrentItem = dr[Counter - 2].ToString();
                                        }
                                        if (CurrentItem == LastItem && RowCounter != TotalRetValRowsCount)
                                        {
                                            DTIntermediate.ImportRow(dr);
                                        }
                                        else if (CurrentItem != LastItem || RowCounter == TotalRetValRowsCount)
                                        {
                                            //If Row is last and current Item is same as before for previous column 
                                            //then Insert import last row also before applying order
                                            if (RowCounter == TotalRetValRowsCount && CurrentItem == LastItem)
                                            {
                                                DTIntermediate.ImportRow(dr);
                                            }
                                            //For the current group of subgroup Dimentions values. Club all unique subgroup Dimentions values in chunks into DTRetVal 
                                            //which will further used to merge into RetVal
                                            foreach (DataRow drSub in DTSubDimension.Rows)
                                            {
                                                foreach (DataRow drIntermediate in DTIntermediate.Select("[" +DrSI[SubgroupInfoColumns.Name].ToString() + "]='" + DICommon.RemoveQuotes(Convert.ToString(drSub[Subgroup.SubgroupName])) + "'"))
                                                {
                                                    DTRetVal.ImportRow(drIntermediate);
                                                }
                                            }
                                            if (RowCounter == TotalRetValRowsCount && CurrentItem != LastItem)
                                            {
                                                DTRetVal.ImportRow(dr);
                                            }
                                            DTIntermediate.Clear();
                                            DTIntermediate.ImportRow(dr);
                                            LastItem = CurrentItem;
                                        }
                                    }
                                }
                            }
                        }
                        //Assign the Ordered 
                        if (DTRetVal.Rows.Count > 0)
                        {
                            RetVal = DTRetVal.Copy();
                        }
                    }
                }
                catch (Exception)
                {
                    RetVal = null;
                }
            }
            //Set the Order now
            if (RetVal != null)
            {
                int OrderNo = 0;
                foreach (DataRow DrRetVal in RetVal.Rows)
                {
                    DrRetVal[Order] = OrderNo;
                    OrderNo++;
                }
            }
            return RetVal;
        }

        /// <summary>
        /// Add square braces in string.
        /// </summary>
        /// <param name="filtertext"></param>
        /// <returns></returns>
        private string AddSlash(string filtertext)
        {
            string Retval = string.Empty;
            try
            {
                Retval = "[" + filtertext + "]";
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        /// <summary>
        /// Update the sort order of column arrangement.
        /// </summary>
        /// <param name="columnArrangementTable">columnArrangementTable</param>
        /// <returns>DataTable</returns>
        private DataTable UpdateSortOrder(DataTable columnArrangementTable)
        {
            DataTable RetVal;
            try
            {
                int OrderNo = 0;
                DataRow[] Rows = new DataRow[0];

                // -- column set to be true, after getting their record number.
                DTImage.Columns.Add("Sorted", typeof(Boolean));
                foreach (DataRow Row in columnArrangementTable.Rows)
                {
                    // -- Synch the DTImage records with columnarrangement table.
                    Rows = DTImage.Select(RecordId + " = " + Row[columnArrangementTable.Columns.Count - 2] + " AND Sorted is null");

                    if (Rows.Length > 0)
                    {
                        Rows[0][DTImage.Columns.Count - 3] = OrderNo;
                        Rows[0][DTImage.Columns.Count - 2] = OrderNo;
                        Rows[0][DTImage.Columns.Count - 1] = true;
                    }
                    // -- arrange the recordId, order no of column arrangement table.
                    Row[columnArrangementTable.Columns.Count - 2] = OrderNo;
                    Row[columnArrangementTable.Columns.Count - 1] = OrderNo;
                    OrderNo += 1;
                }
                //-- Remove the last column
                DTImage.Columns.RemoveAt(DTImage.Columns.Count - 1);

                DataView PasteImageView = DTImage.DefaultView;
                PasteImageView.Sort = DTImage.Columns[DTImage.Columns.Count - 1].ColumnName;
                DTImage = PasteImageView.ToTable();

                RetVal = columnArrangementTable;
            }
            catch (Exception)
            {
                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        /// Get the IC parent Node
        /// </summary>
        private void GetParentNode()
        {
            try
            {

                DataTable DTICs = this.DIConnection.ExecuteDataTable(this.DIQueries.IndicatorClassification.GetIC(FilterFieldType.None, "", this.IndicatorClassification, FieldSelection.Light));
                foreach (SelectedNodeInfo Node in this.TempSelectedNodes)
                {

                    DataRow[] Row = new DataRow[0];
                    Row = DTICs.Select(IndicatorClassifications.ICNId + " = " + Node.NId);
                    if (Row.Length > 0)
                    {
                        //-- Fill the IC mparent Node in their respective field
                        Node.ParentNid = Convert.ToInt32(Row[0][IndicatorClassifications.ICParent_NId]);
                    }
                }

                int ParentRecordIndex = 0;
                int SelectNodeCount = this.TempSelectedNodes.Count;
                for (int Index = 0; Index < SelectNodeCount; Index++)
                {
                    if (this.TempSelectedNodes[Index].ParentNid != -1 && this.TempSelectedNodes[Index].ParentNid != 0)
                    {
                        //-- Search for tyhe parent in the collection.
                        ParentRecordIndex = this.SearchParentRecord(Index);
                    }
                    if (this.TempSelectedNodes[ParentRecordIndex].ParentNid != -1 && this.TempSelectedNodes[Index].ParentNid != 0)
                    {
                        //-- Add the parent record in the collection.
                        this.GetICParent(DTICs, ParentRecordIndex, this.TempSelectedNodes[ParentRecordIndex].Level);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Searches for the parent Record from the collection.
        /// </summary>
        /// <param name="currentRecord"></param>
        /// <returns></returns>
        private int SearchParentRecord(int currentRecord)
        {
            int Retval = currentRecord;
            try
            {
                int RowIndex = 0;
                foreach (SelectedNodeInfo Nodes in this.TempSelectedNodes)
                {
                    if (this.TempSelectedNodes[currentRecord].ParentNid == Nodes.NId)
                    {
                        Retval = RowIndex;
                        break;
                    }
                    RowIndex += 1;
                }
                if (this.TempSelectedNodes[Retval].ParentNid != -1 && RowIndex < this.TempSelectedNodes.Count)
                {
                    //-- Recursive call, to search for the parent
                    this.SearchParentRecord(Retval);
                }
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        /// <summary>
        /// Add the parent node in the collection.
        /// </summary>
        /// <param name="icTable"></param>
        /// <param name="rowIndex"></param>
        /// <param name="selectedLevel"></param>
        private void GetICParent(DataTable icTable, int rowIndex, int selectedLevel)
        {
            DataRow[] Rows = new DataRow[0];
            bool AddNodeinCollection = true;

            Rows = icTable.Select(IndicatorClassifications.ICNId + " = " + this.TempSelectedNodes[rowIndex].ParentNid);
            if (Rows.Length > 0)
            {
                selectedLevel -= 1;
                SelectedNodeInfo Node = new SelectedNodeInfo(Convert.ToInt32(Rows[0][IndicatorClassifications.ICNId]), Convert.ToString(Rows[0][IndicatorClassifications.ICName]), Convert.ToString(Rows[0][IndicatorClassifications.ICGId]), selectedLevel, Convert.ToInt32(Rows[0][IndicatorClassifications.ICParent_NId]), false);
                foreach (SelectedNodeInfo NodeInfo in this.TempSelectedNodes)
                {
                    if (NodeInfo.GId.ToLower() == Node.GId.ToLower())
                    {
                        AddNodeinCollection = false;
                        break;
                    }
                }
                if (AddNodeinCollection)
                {
                    //-- add the parent record in the collection.
                    this.TempSelectedNodes.Add(Node);
                }
            }
            if (AddNodeinCollection && this.TempSelectedNodes[this.TempSelectedNodes.Count - 1].ParentNid != -1)
            {
                //-- Recursive call to add the parent node.
                this.GetICParent(icTable, this.TempSelectedNodes.Count - 1, selectedLevel);
            }
        }

        /// <summary>
        /// Copy the IC nodes
        /// </summary>
        private void CopyICSelectedNodes()
        {
            this.TempSelectedNodes.Clear();
            foreach (SelectedNodeInfo ICNodes in this._SelectedNodes)
            {
                this.TempSelectedNodes.Add(new SelectedNodeInfo(ICNodes.NId, ICNodes.Text, ICNodes.GId, ICNodes.Level, ICNodes.ParentNid));
            }
        }

        #endregion

        #region "-- STEP 4 --"

        /// <summary>
        /// Generates a Sort order string in Step 4. (i.e  area asc,timeperiod desc)
        /// </summary>
        /// <returns>sort string</returns>
        private string GetSortExpression()
        {
            bool ISSubgroupDimension = false;
            string RetVal = string.Empty;
            foreach (Field field in this._Fields.Sort)
            {
                ISSubgroupDimension = false;
                if (field.FieldID == Data.DataValue)
                {
                    //For Numeric sorting of textual DataValues, use expression columns 
                    RetVal += "," + DataExpressionColumns.DataType + " " + OrderType.Asc.ToString() + "," + DataExpressionColumns.NumericData + " " + field.SortType.ToString() + "," + DataExpressionColumns.TextualData + " " + field.SortType.ToString();
                }
                else if (field.FieldID == Area.AreaName)
                {
                    if (!string.IsNullOrEmpty(this._TableReport.AreaSelection.AreaLevels))
                    {
                        for (int i = this.MinAreaLevel; i <= this.MaxAreaLevel; i++)
                        {
                            RetVal += "," + "AL" + i.ToString() + " " + field.SortType.ToString();
                        }
                    }
                    else
                    {
                        RetVal += "," + field.FieldID + " " + field.SortType.ToString();
                    }
                }
                else
                {
                    if (field.FieldID == SubgroupVals.SubgroupVal)
                    {
                        RetVal += "," + SubgroupVals.SubgroupValOrder + " " + field.SortType.ToString();
                        ISSubgroupDimension = true;
                    }
                    else
                    {
                        foreach (DataRow DrSI in this.DIDataView.SubgroupInfo.Rows)
                        {
                            if (field.FieldID == DrSI[SubgroupInfoColumns.Name].ToString())
                            {
                                RetVal += "," + DrSI[SubgroupInfoColumns.OrderColName].ToString() + " " + field.SortType.ToString();
                                ISSubgroupDimension = true;
                            }
                        }
                    }
                    if (ISSubgroupDimension == false)
                    {
                        RetVal += "," + field.FieldID + " " + field.SortType.ToString();
                    }
                }
            }


            // Remove extra comma at the start of SortExpression
            if (RetVal.Length > 0)
            {
                RetVal = RetVal.Substring(1);
            }
            return RetVal;
        }

        #endregion

        #region "-- STEP 6 --"

        /// <summary>
        /// Set the Table Structure for FootNotes table.
        /// </summary>
        private void SetFootNoteTableStructure()
        {
            this.DTFootNote = new DataTable();
            this.DTFootNote.Columns.Add("FootNoteIndex");
            this.DTFootNote.Columns.Add(FootNotes.FootNoteNId, typeof(int));
            this.DTFootNote.Columns.Add(FootNotes.FootNote);
        }

        /// <summary>
        /// Set the table structure of Master table of comments which is containing the relationship between DataNId and NoteNId.
        /// </summary>
        private void SetCommentsMasterTableStructure()
        {
            this.DTMasterComments = new DataTable();
            this.DTMasterComments.Columns.Add(Notes_Data.DataNId);
            this.DTMasterComments.Columns.Add(Notes.NotesNId);
        }

        /// <summary>
        /// Set the table structure for comments table.
        /// </summary>
        private void SetCommentsTableStructure()
        {
            this.DTComments = new DataTable();
            this.DTComments.Columns.Add("NoteIndex");
            //this.DTComments.Columns.Add(Notes.DataNId);
            this.DTComments.Columns.Add(Notes.NotesNId);
            this.DTComments.Columns.Add("NoteSymbol");
            this.DTComments.Columns.Add(Notes.Note);

        }

        /// <summary>
        /// Sets the table structure for DataTable DTAreaParentNId
        /// </summary>
        private void SetDTAreaParentNIdTableStructure()
        {
            this.DTAreaParentNId = new DataTable();
            this.DTAreaParentNId.Columns.Add(Area.AreaParentNId);
            this.DTAreaParentNId.Columns.Add(Area.AreaName);
        }

        /// <summary>
        /// Generate the Header structure for TableXLS
        /// </summary>
        /// <param name="TableXLS">Empty TableXLS</param>
        /// <param name="TotalColFieldVals">//Total CFVs (Column Field Vaues)</param>
        /// <param name="TotalColumnFields">//Total CFs (Column Fields) including NId fields</param>
        /// <param name="TotalRowFields">//Total RFs (Row Fields)</param>
        /// <returns></returns>
        private DataTable GenerateTableXLSHeader(DataTable TableXLS, int TotalColFieldVals, int TotalColumnFields, int TotalRowFields)
        {
            int Columns;
            int Rows;
            int Index;
            bool ColumnTypeArrayList = true;				//Set to true one the arraylist ALColumnType is made once and this boolean value ensures that it doesn't continue to make for next rows of tableXLS.
            bool AddNewRow = false;
            ArrayList AL = new ArrayList();
            DataRow TableXLSRow;								// Each Row in TableXLS.
            this.ALColumnType = new ArrayList();
            //Total columns = Total CFV + Total RF.
            int TotalColumns = 0;
            if (TotalColFieldVals == 0)
            {
                //-- To add the column, only If Column arrangement table is empty
                TotalColumns = TotalRowFields + 1;
            }
            else
            {
                TotalColumns = TotalColFieldVals + TotalRowFields;
            }

            //Add the rows collection fields into an array list that will be used to generate the Table Header(Row containing column names) of TableXLS.
            if (this._Fields.Rows.Count > 0)
            {
                foreach (Field Field in this._Fields.Rows)
                {
                    AL.Add(Field.FieldID);
                }
            }

            //If ColumnHeaders table is not null then add columns to table TableXLS 
            //otherwise add a default column named "Data Value"
            //Rows of _ColumnArrangementTable will be used to make column of TableXLS 			
            if (TotalColFieldVals >= 0)
            {
                //---------------------------------------
                //Add columns to TableXLS. Add Two column extra.One to hold rowtype and second will hold the row index				
                for (int i = 0; i < TotalColumns + (TotalColFieldVals * FootnoteCommentColumnCount) + 4 + this.MaxMinAreaLevelDiff; i++)
                {
                    TableXLS.Columns.Add("");
                }

                //Provide ColumnName to the LastColunm ( Column used to set colors )
                TableXLS.Columns[TotalColumns + (TotalColFieldVals * FootnoteCommentColumnCount)].ColumnName = ROWTYPE;
                TableXLS.Columns[TotalColumns + (TotalColFieldVals * FootnoteCommentColumnCount) + 1].ColumnName = ROWINDEX;
                TableXLS.Columns[TotalColumns + (TotalColFieldVals * FootnoteCommentColumnCount) + 2].ColumnName = AREALEVEL;
                TableXLS.Columns[TotalColumns + (TotalColFieldVals * FootnoteCommentColumnCount) + 3].ColumnName = AREAPARENTCHILDRELATION;
                //Insert Columns for Order
                this.TotalOrderColCount = 0;
                foreach (Field field in this.Fields.Rows)
                {
                    if (field.FieldID == SubgroupVals.SubgroupVal)
                    {
                        TableXLS.Columns.Add(SubgroupVals.SubgroupValOrder, typeof(int));
                        TableXLS.Columns[SubgroupVals.SubgroupValOrder].ColumnName = SubgroupVals.SubgroupValOrder;
                        this._TotalOrderColCount++;
                    }
                    else
                    {
                        foreach (DataRow DrSI in this.DIDataView.SubgroupInfo.Rows)
                        {
                            if (field.FieldID == DrSI[SubgroupInfoColumns.Name].ToString())
                            {
                                TableXLS.Columns.Add(DrSI[SubgroupInfoColumns.OrderColName].ToString(), typeof(int));
                                TableXLS.Columns[DrSI[SubgroupInfoColumns.OrderColName].ToString()].ColumnName = DrSI[SubgroupInfoColumns.OrderColName].ToString();
                                this._TotalOrderColCount++;
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(this._TableReport.AreaSelection.AreaLevels))
                {
                    int ColCount = 1;
                    for (int i = this.MinAreaLevel; i <= this.MaxAreaLevel; i++)
                    {
                        TableXLS.Columns[TotalColumns + (TotalColFieldVals * FootnoteCommentColumnCount) + 3 + ColCount].ColumnName = "AL" + i.ToString();
                        ColCount++;
                    }
                }


                //------------------------------------------------------------------------------

                //Make the Header of TableXLS.
                //If TotalColumnFields = 0 then Add a default column called "Data_Value" in the header.
                if (TotalColumnFields == 0)
                {
                    Columns = 0;
                    TableXLSRow = TableXLS.NewRow();
                    foreach (Field Field in this._Fields.Rows)
                    {
                        TableXLSRow[Columns] = this._Fields.Rows.GetCaption(Field.FieldID);
                        TableXLS.Columns[Columns].ColumnName = Field.FieldID;
                        Columns++;
                    }
                    TableXLSRow[Columns] = LanguageStrings.DataValue;
                    TableXLSRow[TablePresentation.ROWTYPE] = RowType.TableHeaderRow.ToString();
                    TableXLS.Rows.Add(TableXLSRow);
                    GenerateColumnTypeArray(TotalRowFields);
                }
                else
                {
                    //Use to leave the blank column for footnote in the TableXLS after each column of datavalue.
                    int Footnotes = 0;
                    //Leave the alternate Row as alternate row containes the GID values which are not required at this stage.

                    TableXLSRow = TableXLS.NewRow();

                    for (Rows = 0; Rows < TotalColumnFields; Rows = Rows + 2)
                    {
                        //Add the new row only if the presentation type is Table and after inserting the first row in the table.
                        if ((this._PresentationType == Presentation.PresentationType.Table || this._PresentationType == Presentation.PresentationType.FrequencyTable) && AddNewRow)
                        {
                            TableXLSRow = TableXLS.NewRow();
                        }
                        for (Columns = 0; Columns < Math.Max(TotalColFieldVals, TotalRowFields); Columns++)
                        {
                            //Make the Columns in the ROWS section of TableXLS
                            //This Row will be the last Row in the Table Header. 
                            if (Rows == TotalColumnFields - 2 && Columns < TotalRowFields)
                            {
                                //OPT :Remove AL if possible.
                                TableXLSRow[Columns] = SetColumnData(TableXLSRow[Columns].ToString()) + this._Fields.Rows.GetCaption(AL[Columns].ToString());
                                TableXLS.Columns[Columns].ColumnName = AL[Columns].ToString();
                                if (Columns < TotalColFieldVals)
                                {
                                    if (this._ColumnArrangementTable.Columns[Rows].ColumnName == Timeperiods.TimePeriod)
                                    {
                                        TableXLSRow[Columns + TotalRowFields + Footnotes] = "'" + SetColumnData(TableXLSRow[Columns + TotalRowFields + Footnotes].ToString()) + this._ColumnArrangementTable.Rows[Columns][Rows].ToString();
                                    }
                                    else
                                    {
                                        if (this._ColumnArrangementTable.Rows[Columns][Rows].ToString() == string.Empty)
                                        {
                                            TableXLSRow[Columns + TotalRowFields + Footnotes] = SetColumnData(TableXLSRow[Columns + TotalRowFields + Footnotes].ToString()) + DILanguage.GetLanguageString("TOTAL");
                                        }
                                        else
                                        {
                                            TableXLSRow[Columns + TotalRowFields + Footnotes] = SetColumnData(TableXLSRow[Columns + TotalRowFields + Footnotes].ToString()) + this._ColumnArrangementTable.Rows[Columns][Rows].ToString();
                                        }

                                    }
                                    Index = 1;
                                    while (Index <= FootnoteCommentColumnCount)
                                    {
                                        Footnotes += 1;
                                        TableXLSRow[Columns + TotalRowFields + Footnotes] = SetColumnData(TableXLSRow[Columns + TotalRowFields + Footnotes].ToString()) + "";
                                        Index += 1;
                                    }
                                    //Make an arrayList holding the column type information.
                                    if (ColumnTypeArrayList)
                                    {
                                        GenerateColumnTypeArray(TotalRowFields);
                                    }
                                }
                            }
                            else if (Columns < TotalColFieldVals)
                            {
                                if (this._ColumnArrangementTable.Columns[Rows].ColumnName == Timeperiods.TimePeriod)
                                {
                                    TableXLSRow[Columns + TotalRowFields + Footnotes] = "'" + SetColumnData(TableXLSRow[Columns + TotalRowFields + Footnotes].ToString()) + this._ColumnArrangementTable.Rows[Columns][Rows].ToString();
                                }
                                else
                                {
                                    if (this._ColumnArrangementTable.Rows[Columns][Rows].ToString() == string.Empty)
                                    {
                                        TableXLSRow[Columns + TotalRowFields + Footnotes] = SetColumnData(TableXLSRow[Columns + TotalRowFields + Footnotes].ToString()) + DILanguage.GetLanguageString("TOTAL");
                                    }
                                    else
                                    {
                                        TableXLSRow[Columns + TotalRowFields + Footnotes] = SetColumnData(TableXLSRow[Columns + TotalRowFields + Footnotes].ToString()) + this._ColumnArrangementTable.Rows[Columns][Rows].ToString();
                                    }

                                }
                                Index = 1;
                                while (Index <= FootnoteCommentColumnCount)
                                {
                                    Footnotes += 1;
                                    TableXLSRow[Columns + TotalRowFields + Footnotes] = SetColumnData(TableXLSRow[Columns + TotalRowFields + Footnotes].ToString()) + "";
                                    Index += 1;
                                }

                                //Make an arrayList holding the column type information.
                                if (ColumnTypeArrayList)
                                {
                                    GenerateColumnTypeArray(TotalRowFields);
                                }
                            }
                        }
                        ColumnTypeArrayList = false;
                        Footnotes = 0;
                        // add the row in the table if the PresentationType is table
                        if (this._PresentationType == Presentation.PresentationType.Table || this._PresentationType == Presentation.PresentationType.FrequencyTable)
                        {
                            TableXLSRow[TablePresentation.ROWTYPE] = RowType.TableHeaderRow.ToString();
                            TableXLS.Rows.Add(TableXLSRow);
                            AddNewRow = true;
                        }
                    }
                    // add the row in the table if the PresentationType is graph
                    if (this._PresentationType == Presentation.PresentationType.Graph)
                    {
                        TableXLS.Rows.Add(TableXLSRow);
                    }
                }
            }

            this.ALColumnType.Add(TableColumnType.Others);
            this.ALColumnType.Add(TableColumnType.Others);
            this.ALColumnType.Add(TableColumnType.Others);
            this.ALColumnType.Add(TableColumnType.Others);


            if (!string.IsNullOrEmpty(this._TableReport.AreaSelection.AreaLevels))
            {
                for (int i = this.MinAreaLevel; i <= this.MaxAreaLevel; i++)
                {
                    this.ALColumnType.Add(TableColumnType.Others);
                }
            }

            //-- if the column data is empty, add the "'" to make the column data string
            if (this._PresentationType == Presentation.PresentationType.Graph)
            {
                for (int i = TotalRowFields; i < TableXLS.Columns.Count - 2; i++)
                {
                    if (string.IsNullOrEmpty(TableXLS.Rows[0][i].ToString().Trim()))
                    {
                        TableXLS.Rows[0][i] = "'";
                    }
                }
            }
            return TableXLS;
        }

        /// <summary>
        /// set the column string with new line character in case of graph.
        /// </summary>
        /// <param name="data">Column Data</param>
        /// <returns>string</returns>
        private string SetColumnData(string data)
        {
            string RetVal = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(data))
                {
                    RetVal = data + " " + "\n" + " ";
                }
                else
                {
                    RetVal = string.Empty;
                }
            }
            catch (Exception ex)
            {
                RetVal = string.Empty;
            }
            return RetVal;
        }

        /// <summary>
        /// Prepares the arraylist containing information of columntype of TableXLS.
        /// </summary>
        /// <param name="TotalRowFields">Total rowfields count</param>
        private void GenerateColumnTypeArray(int TotalRowFields)
        {
            if (ALColumnType.Count == 0)
            {
                for (int ColumnType = 0; ColumnType < TotalRowFields; ColumnType++)
                {
                    this.ALColumnType.Add(TableColumnType.RowHeader);
                }
            }
            this.ALColumnType.Add(TableColumnType.DataValue);
            switch (this.FootnoteCommentType)
            {
                case FootnoteCommentType.None:
                    if (this._TemplateStyle.Denominator.Show)
                    {
                        this.ALColumnType.Add(TableColumnType.Denominator);
                    }
                    break;
                case FootnoteCommentType.Comment:
                    this.ALColumnType.Add(TableColumnType.Comment);
                    if (this._TemplateStyle.Denominator.Show)
                    {
                        this.ALColumnType.Add(TableColumnType.Denominator);
                    }
                    break;
                case FootnoteCommentType.Footnote:
                    this.ALColumnType.Add(TableColumnType.FootNote);
                    if (this._TemplateStyle.Denominator.Show)
                    {
                        this.ALColumnType.Add(TableColumnType.Denominator);
                    }
                    break;
                case FootnoteCommentType.Both:
                    this.ALColumnType.Add(TableColumnType.FootNote);
                    this.ALColumnType.Add(TableColumnType.Comment);
                    if (this._TemplateStyle.Denominator.Show)
                    {
                        this.ALColumnType.Add(TableColumnType.Denominator);
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private DataTable FillClassificationTable(DataTable DTFilterICNames)
        {
            DataTable RetVal = new DataTable();
            DataRow[] dRow = null;
            //DataRow[] ClassificationRows;
            this.DeletedICNIds.Clear();
            DataTable DTClassifications = new DataTable();
            DTClassifications.Columns.Add(IndicatorClassifications.ICNId);
            DTClassifications.Columns.Add(IndicatorClassifications.ICGId);
            DTClassifications.Columns.Add(IndicatorClassifications.ICName);
            DTClassifications.Columns.Add(IndicatorClassifications.ICOrder, typeof(int));
            DTClassifications.Columns.Add("Selected", typeof(bool));
            DataRow row;

            foreach (SelectedNodeInfo snode in this.TempSelectedNodes)
            {
                if (!snode.Show)
                {
                    this.DeletedICNIds.Add(snode.NId);
                }

                if (snode.Level == 1)
                {
                    if (DTFilterICNames.Select(IndicatorClassifications.ICNId + " in(" + snode.NId + ")").Length > 0)
                    {
                        row = DTClassifications.NewRow();
                        row[IndicatorClassifications.ICNId] = snode.NId;
                        row[IndicatorClassifications.ICGId] = string.Empty;
                        row[IndicatorClassifications.ICName] = snode.Text;
                        try
                        {
                            dRow = DTFilterICNames.Select(IndicatorClassifications.ICNId + " in(" + snode.NId + ")");
                            row[IndicatorClassifications.ICOrder] = Convert.ToInt32(dRow[0][IndicatorClassifications.ICOrder].ToString());
                        }
                        catch (Exception)
                        {
                            row[IndicatorClassifications.ICOrder] = 0;
                        }

                        row["Selected"] = snode.Show;
                        DTClassifications.Rows.Add(row);
                    }
                }
            }

            RetVal = DTClassifications;

            return RetVal;
        }

        /// <summary>
        /// Filter down the Indicator classification to selected level.
        /// </summary>
        /// <param name="DTICNames">Contains Indicator classification up to all levels</param>
        /// <param name="Level">Level to filter DTICNames</param>
        /// <returns>Filtered DataTable.</returns>
        private DataTable FilterICNames(DataTable DTICNames, int Level)
        {
            string ParentNid = "-1";
            int Counter;
            string ICNIds = string.Empty;
            ArrayList AL = new ArrayList();
            int StartIndex = 0;
            int ALCount = 0;
            DataTable RetVal = new DataTable();
            ArrayList ALFilteredIC = new ArrayList();
            DataRow[] DrowCount = new DataRow[0];
            foreach (SelectedNodeInfo snode in this.TempSelectedNodes)
            {
                ALFilteredIC.Add(snode.NId);
            }


            //Make the structure of RetVal same as DTICNames.
            RetVal = DTICNames.Clone();

            //Make a loop upto leves selected
            for (int Levels = 1; Levels <= Level; Levels++)
            {
                //If Level is NOT first
                if (AL.Count > 0)
                {
                    ALCount = AL.Count;
                    for (Counter = StartIndex; Counter < ALCount; Counter++)
                    {
                        foreach (DataRow DRowDTICNames in DTICNames.Select(IndicatorClassifications.ICParent_NId + "='" + AL[Counter] + "'"))
                        {
                            if (DRowDTICNames[Indicator.IndicatorNId].ToString() != string.Empty)
                            {
                                if (ALFilteredIC.Contains(int.Parse(DRowDTICNames[IndicatorClassifications.ICNId].ToString())))
                                {
                                    if (!AL.Contains(DRowDTICNames[IndicatorClassifications.ICNId].ToString()))
                                    {
                                        AL.Add(DRowDTICNames[IndicatorClassifications.ICNId].ToString());
                                    }
                                    RetVal.ImportRow(DRowDTICNames);
                                }
                            }
                        }

                    }
                    StartIndex = ALCount;   //Initialize the counter so that for each level loop starts  with its relative count.
                }
                //Executes only for first level
                else
                {
                    //Filter down DTICNames on Parent Nid where Parent Nid=-1   . Means it is first level.
                    foreach (DataRow DRowDTICNames in DTICNames.Select(IndicatorClassifications.ICParent_NId + " IN (" + ParentNid + ")"))
                    {
                        if (DRowDTICNames[Indicator.IndicatorNId].ToString() != string.Empty)
                        {
                            if (ALFilteredIC.Contains(int.Parse(DRowDTICNames[IndicatorClassifications.ICNId].ToString())))
                            {
                                DrowCount = DTICNames.Select(IndicatorClassifications.ICParent_NId + "='" + DRowDTICNames[IndicatorClassifications.ICNId] + "'");
                                if (DrowCount.Length > 0)
                                {
                                    if (Level > 1)
                                    {
                                        DRowDTICNames[Indicator.IndicatorNId] = string.Empty;
                                    }
                                }
                                if (!AL.Contains(DRowDTICNames[IndicatorClassifications.ICNId].ToString()))
                                {
                                    AL.Add(DRowDTICNames[IndicatorClassifications.ICNId].ToString());
                                }
                                RetVal.ImportRow(DRowDTICNames);
                            }
                        }
                    }
                }
            }
            return RetVal;
        }

        /// <summary>
        /// Gets IC_ParentNId, IC_NId,IC_Name,IndicatorNid of all levels when IC Arrangement is on in STEP 3
        /// </summary>
        /// <returns>DataTable that holds the IC_ParentNId,IC_NId,IC_Name,IndicatorNId</returns>
        /// <remarks>
        /// Structure of DataTable is
        /// 
        /// ---------------------------------------------------
        /// | IC_ParentNId | IC_NId  | IC_Name | IndicatorNId  |
        /// ------------------------------------------------ --
        /// </remarks>
        private DataTable GetICNames()
        {
            DataTable DTICNames;
            DataTable Retval = new DataTable();
            Retval.Columns.Add(IndicatorClassifications.ICParent_NId);
            Retval.Columns.Add(IndicatorClassifications.ICNId);
            Retval.Columns.Add(IndicatorClassifications.ICName);
            Retval.Columns.Add(Indicator_Unit_Subgroup.IndicatorNId);
            Retval.Columns.Add(IndicatorClassifications.ICOrder);
            string ICNIds = string.Empty;

            string QueryGetICNames = this.DIQueries.IndicatorClassification.GetICForIndicators(this._IndicatorClassification, this.IndicatorNIds, FieldSelection.Light);
            DTICNames = this.DIConnection.ExecuteDataTable(QueryGetICNames);
            foreach (DataRow DRowDTICNames in DTICNames.Rows)
            {
                Retval.ImportRow(DRowDTICNames);
                if (ICNIds != string.Empty)
                {
                    ICNIds += ",";
                }
                ICNIds += DRowDTICNames[IndicatorClassifications.ICNId].ToString();
            }
            DTICNames = null;

            QueryGetICNames = this.DIQueries.IndicatorClassification.GetIC(FilterFieldType.NIdNotIn, ICNIds, this._IndicatorClassification, FieldSelection.Light);
            DTICNames = this.DIConnection.ExecuteDataTable(QueryGetICNames);
            foreach (DataRow DRowDTICNames in DTICNames.Rows)
            {
                Retval.ImportRow(DRowDTICNames);
            }
            return Retval;
        }

        /// <summary>
        /// Iterate each level of IC recursively.
        /// </summary>
        /// <param name="DTFilterICNames">Contains IC_Parent_Nid,IC_Nid,IC_Name,Indicator_Nid (from  IUS) based on levels selected.</param>
        /// <param name="TableXLS">XLS Table</param>
        /// <param name="DRowDTClassifications">DataRow of DataTable DTClassification</param>
        /// <param name="UniqueRows">Contains unique indicators.Used when inserting records in TableXLS.</param>
        /// <param name="TotalRowFields">Total RFs (Row Fields)</param>
        /// <param name="TotalColFieldVals">Total CFVs (Column Field Vaues)</param>
        /// <param name="TotalColumnFields">Total columns = Total CFV + Total RF.</param>
        /// <param name="ParentICNId">Empty string variable</param>
        /// <param name="DesiredLevel">Level up to which classification is to be arranged</param>
        /// <param name="CurrentLevel">Current Level</param>
        /// <param name="DuplicateItem">Empty string variable</param>
        /// <param name="ALClassification">Null arrayList</param>
        private void MakeRecursiveClassificationCalls(DataTable DTFilterICNames, DataTable TableXLS, DataRow DRowDTClassifications, int TotalRowFields, int TotalColFieldVals, int TotalColumnFields, string ParentICNId, int DesiredLevel, int CurrentLevel, string DuplicateItem, ArrayList ALClassification, int TableXLSColumns)
        {
            //It will group the records based on Indicator classification.

            string FilterString;
            string ICNID;
            DataRow[] RowsDTFilterICNames;
            string IC_Parent_Nid = string.Empty;
            DataRow TableXLSRow;//DataRow for DataTable TableXLS.
            DataRow RowDTRowsForIcons;

            //If for the selected ICNId there is Child ICNId then move to next level of recursion, else add records to TableXLS.			
            if (DTFilterICNames.Select(IndicatorClassifications.ICParent_NId + "='" + DRowDTClassifications[IndicatorClassifications.ICNId] + "'").Length != 0 && (CurrentLevel < DesiredLevel))
            {
                CurrentLevel++;
                //Iterate in each child level in recursive mode.
                foreach (DataRow DRowDTFilterICNames in DTFilterICNames.Select(IndicatorClassifications.ICParent_NId + "='" + DRowDTClassifications[IndicatorClassifications.ICNId] + "'"))
                {
                    //Consider ICNid of a level only one time if there exits multiple entries of parent ICNID.
                    if (ParentICNId != DRowDTFilterICNames[IndicatorClassifications.ICNId].ToString())
                    {
                        //RowDTRowsForIcons = this.DTRowsForIcons.NewRow();
                        //this.DTRowsForIcons.Rows.Add(RowDTRowsForIcons);




                        //Suppress dulicate entries of IC_Name in TableXLS.
                        if (DuplicateItem != DRowDTClassifications[IndicatorClassifications.ICName].ToString())
                        {
                            TableXLSRow = TableXLS.NewRow();
                            TableXLSRow[ROWTYPE] = RowType.EmptyRow;
                            TableXLS.Rows.Add(TableXLSRow);
                            RowDTRowsForIcons = this.DTRowsForIcons.NewRow();
                            this.DTRowsForIcons.Rows.Add(RowDTRowsForIcons);

                            if (CurrentLevel > 2)
                            {
                                //Assign ICName is reverse order level in the array list.
                                IC_Parent_Nid = DRowDTFilterICNames[IndicatorClassifications.ICParent_NId].ToString();
                                for (int i = CurrentLevel; i > 1; i--)
                                {
                                    if (ParentICNId != IC_Parent_Nid)
                                    {
                                        RowsDTFilterICNames = DTFilterICNames.Select(IndicatorClassifications.ICNId + "='" + IC_Parent_Nid + "'");
                                        ICNID = RowsDTFilterICNames[0][IndicatorClassifications.ICNId].ToString();
                                        if (this.DeletedICNIds.BinarySearch(Convert.ToInt32(ICNID)) < 0)
                                        {
                                            ALClassification.Add(RowsDTFilterICNames[0][IndicatorClassifications.ICName].ToString());
                                        }
                                        IC_Parent_Nid = RowsDTFilterICNames[0][IndicatorClassifications.ICParent_NId].ToString();
                                    }
                                }
                            }
                            //ALClassification is set in above block of code.
                            if (ALClassification.Count > 0)
                            {
                                //Traverse the array list in reverse order so that IC Names can be inserted in correct order
                                for (int j = ALClassification.Count - 1; j > -1; j--)
                                {
                                    TableXLSRow = TableXLS.NewRow();
                                    RowDTRowsForIcons = this.DTRowsForIcons.NewRow();
                                    TableXLSRow[0] = ALClassification[j].ToString();
                                    TableXLSRow[TablePresentation.ROWTYPE] = RowType.ICAggregate;
                                    TableXLSRow[TablePresentation.AREALEVEL] = "-1";
                                    TableXLSRow[TablePresentation.ROWINDEX] = TableXLS.Rows.Count + 1;
                                    this.DTRowsForIcons.Rows.Add(RowDTRowsForIcons);
                                    TableXLS.Rows.Add(TableXLSRow);
                                }
                            }
                            else
                            {
                                ICNID = DRowDTClassifications[IndicatorClassifications.ICNId].ToString();
                                if (this.DeletedICNIds.BinarySearch(Convert.ToInt32(ICNID)) < 0)
                                {
                                    TableXLSRow = TableXLS.NewRow();
                                    TableXLSRow[0] = DRowDTClassifications[IndicatorClassifications.ICName].ToString();
                                    TableXLSRow[TablePresentation.ROWTYPE] = RowType.ICAggregate;
                                    TableXLSRow[TablePresentation.AREALEVEL] = "-1";
                                    TableXLSRow[TablePresentation.ROWINDEX] = TableXLS.Rows.Count + 1;
                                    TableXLS.Rows.Add(TableXLSRow);
                                }
                                RowDTRowsForIcons = this.DTRowsForIcons.NewRow();
                                this.DTRowsForIcons.Rows.Add(RowDTRowsForIcons);
                            }
                        }
                        ALClassification.Clear();

                        ICNID = DRowDTFilterICNames[IndicatorClassifications.ICNId].ToString();
                        if (this.DeletedICNIds.BinarySearch(Convert.ToInt32(ICNID)) < 0)
                        {
                            TableXLSRow = TableXLS.NewRow();
                            TableXLSRow[0] = DRowDTFilterICNames[IndicatorClassifications.ICName].ToString();
                            TableXLSRow[TablePresentation.ROWTYPE] = RowType.ICAggregate;
                            TableXLSRow[TablePresentation.AREALEVEL] = "-1";
                            TableXLSRow[TablePresentation.ROWINDEX] = TableXLS.Rows.Count + 1;
                            TableXLS.Rows.Add(TableXLSRow);
                        }

                        RowDTRowsForIcons = this.DTRowsForIcons.NewRow();
                        this.DTRowsForIcons.Rows.Add(RowDTRowsForIcons);
                        DuplicateItem = DRowDTFilterICNames[IndicatorClassifications.ICName].ToString();
                        ParentICNId = DRowDTFilterICNames[IndicatorClassifications.ICNId].ToString();
                        MakeRecursiveClassificationCalls(DTFilterICNames, TableXLS, DRowDTFilterICNames, TotalRowFields, TotalColFieldVals, TotalColumnFields, ParentICNId, DesiredLevel, CurrentLevel, DuplicateItem, ALClassification, TableXLSColumns);
                    }
                    //if indicator were added then exit for
                }
            }
            else    //Add records under last level node.
            {
                foreach (DataRow DRowDTFilterICNames in DTFilterICNames.Select(IndicatorClassifications.ICNId + "='" + DRowDTClassifications[IndicatorClassifications.ICNId] + "'"))
                {
                    if (DRowDTFilterICNames[Indicator.IndicatorNId].ToString().Length > 0)
                    {
                        FilterString = Indicator.IndicatorNId + " = " + DRowDTFilterICNames[Indicator.IndicatorNId];
                        this.FillTableXLS(FilterString, TableXLS, TotalRowFields, TotalColFieldVals, TotalColumnFields);
                    }
                }
            }
        }

        /// <summary>
        /// Populate blank TableXLS.
        /// </summary>
        /// <param name="FilterString">FilterString for TableXLS which filter it on the bases of IC arrangement</param>
        /// <param name="TableXLS">Table XLS</param>
        /// <param name="UniqueRows">DataTable that holds the Unique Rows</param>
        /// <param name="TotalRowFields">Total RFs (Row Fields)</param>
        /// <param name="TotalColFieldVals">Total CFVs (Column Field Vaues)</param>
        /// <param name="TotalColumnFields">Total CFs (Column Fields) including NId fields</param>
        private void FillTableXLS(string FilterString, DataTable TableXLS, int TotalRowFields, int TotalColFieldVals, int TotalColumnFields)
        {
            DataTable TableXLSUniqueRows = new DataTable();		// DataTable that holds the Unique Rows in presntation data based on Row Selections on step 1 
            DataRow RowTableXLSUniqueRows;

            DataTable DTFiltered = new DataTable();				// DataTable that holds the filtered records from presentatioData based on each row of TableXLSUniqueRows.
            DataRow TableXLSRow;
            DataRow RowDTDataSheet;
            DataRow RowDTRowsForIcons;
            string FootNoteIndex = string.Empty;
            int FieldIndex = 0;
            decimal dataValue = 0;

            System.Globalization.CultureInfo InvariantCulture = new System.Globalization.CultureInfo("en-US", false);
            //Make the header of DataTable UniqueRows.			
            foreach (Field field in this._Fields.Rows)
            {
                if (field.FieldID == Area.AreaName)
                {
                    if (!TableXLSUniqueRows.Columns.Contains(Area.AreaID))
                    {
                        TableXLSUniqueRows.Columns.Add(Area.AreaID);
                    }
                }
                else
                {
                    if (!TableXLSUniqueRows.Columns.Contains(field.FieldID))
                    {
                        TableXLSUniqueRows.Columns.Add(field.FieldID);
                    }
                }
            }

            int TotalTableXLSColumns = TableXLS.Columns.Count - (this.ExtraColumnCount + this.MaxMinAreaLevelDiff);

            //Make a loop for each Record in the PresentationData

            string[] ArrIndicatorFields = this.MDIndicatorFields.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string[] ArrAreaFields = this.MDAreaFields.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string[] ArrSourceFields = this.MDSourceFields.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string[] ArrICFields = this.ICInfoFields.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (DataRow DRVPresentationData in this.PresentationData.ToTable().Select(FilterString))
            {
                //-----------------Fill DataTable for DataSheet---------------
                RowDTDataSheet = this.DTDataSheet.NewRow();
                RowDTDataSheet[Timeperiods.TimePeriod] = DRVPresentationData[Timeperiods.TimePeriod];
                RowDTDataSheet[Area.AreaID] = DRVPresentationData[Area.AreaID];
                RowDTDataSheet[Area.AreaName] = DRVPresentationData[Area.AreaName];
                RowDTDataSheet[Indicator.IndicatorName] = DRVPresentationData[Indicator.IndicatorName];
                RowDTDataSheet[Data.DataValue] = DRVPresentationData[Data.DataValue];
                RowDTDataSheet[Unit.UnitName] = DRVPresentationData[Unit.UnitName];
                RowDTDataSheet[SubgroupVals.SubgroupVal] = DRVPresentationData[SubgroupVals.SubgroupVal];
                RowDTDataSheet[IndicatorClassifications.ICName] = DRVPresentationData[IndicatorClassifications.ICName];
                if (DRVPresentationData[FootNotes.FootNoteNId].ToString() != "-1")
                {
                    RowDTDataSheet[FootNotes.FootNote] = DRVPresentationData[FootNotes.FootNoteNId];
                }
                else
                {
                    RowDTDataSheet[FootNotes.FootNote] = string.Empty;
                }

                //For each indicator NId
                for (FieldIndex = 0; FieldIndex < ArrIndicatorFields.Length; FieldIndex++)
                {
                    foreach (DataRow Drow in this.DIDataView.MetadataIndicator.Select(Indicator.IndicatorNId + " = " + DRVPresentationData[Indicator.IndicatorNId].ToString()))
                    {
                        RowDTDataSheet[ArrIndicatorFields[FieldIndex]] = Drow[ArrIndicatorFields[FieldIndex]].ToString();
                        break;
                    }
                }
                for (FieldIndex = 0; FieldIndex < ArrAreaFields.Length; FieldIndex++)
                {
                    foreach (DataRow Drow in this.DIDataView.MetadataArea.Select(Area.AreaNId + " = " + DRVPresentationData[Area.AreaNId].ToString()))
                    {
                        RowDTDataSheet[ArrAreaFields[FieldIndex]] = Drow[ArrAreaFields[FieldIndex]].ToString();
                        break;
                    }
                }
                for (FieldIndex = 0; FieldIndex < ArrSourceFields.Length; FieldIndex++)
                {
                    foreach (DataRow Drow in this.DIDataView.MetadataSource.Select(IndicatorClassifications.ICNId + " = " + DRVPresentationData[IndicatorClassifications.ICNId].ToString()))
                    {
                        RowDTDataSheet[ArrSourceFields[FieldIndex]] = Drow[ArrSourceFields[FieldIndex]].ToString();
                        break;
                    }
                }

                for (FieldIndex = 0; FieldIndex < ArrICFields.Length; FieldIndex++)
                {
                    foreach (DataRow Drow in this.DIDataView.IUSICInfo.Select(Indicator_Unit_Subgroup.IUSNId + " = " + DRVPresentationData[Indicator_Unit_Subgroup.IUSNId].ToString()))
                    {
                        RowDTDataSheet[ArrICFields[FieldIndex]] = Drow[ArrICFields[FieldIndex]].ToString();
                        break;
                    }
                }



                this.DTDataSheet.Rows.Add(RowDTDataSheet);

                //------------------------------------------------------------


                FilterString = string.Empty;

                //Make a filter string for extracting unique rows in presntation data based on Row Selection on Step 1
                foreach (Field Field in this._Fields.Rows)
                {
                    if (!Field.FieldID.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.Metadata) && !Field.FieldID.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.IC))
                    {
                        if (FilterString != string.Empty)
                        {
                            FilterString = FilterString + " AND ";
                        }
                        if (DRVPresentationData[Field.FieldID] == System.DBNull.Value)
                        {
                            FilterString = FilterString + "IsNull(" + this.AddSlash(Field.FieldID) + ",'0')='0'";
                        }
                        else
                        {
                            if (Field.FieldID == Area.AreaName)
                            {
                                FilterString = FilterString + this.AddSlash(Area.AreaID) + "='" + DICommon.RemoveQuotes(DRVPresentationData[Area.AreaID].ToString()) + "'";
                            }
                            else
                            {
                                FilterString = FilterString + this.AddSlash(Field.FieldID) + "='" + DICommon.RemoveQuotes(DRVPresentationData[Field.FieldID].ToString()) + "'";
                            }

                        }
                    }
                }

                //For Unique record insert the record into tables UniqueRows and TableXLS.
                if (TableXLSUniqueRows.Select(FilterString).Length == 0)
                {
                    RowTableXLSUniqueRows = TableXLSUniqueRows.NewRow();
                    TableXLSRow = TableXLS.NewRow();
                    RowDTRowsForIcons = DTRowsForIcons.NewRow();


                    //Insert the Values for Rows in	RowUniqueRows and TableXLS.
                    foreach (Field Field in this._Fields.Rows)
                    {
                        if (Field.FieldID.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.MetadataIndicator))
                        {
                            if (this.MDIndicatorFields.Length > 0)
                            {
                                foreach (DataRow Drow in this.DIDataView.MetadataIndicator.Select(Indicator.IndicatorNId + " = " + DRVPresentationData[Indicator.IndicatorNId].ToString()))
                                {
                                    RowTableXLSUniqueRows[Field.FieldID] = Drow[Field.FieldID];
                                    TableXLSRow[Field.FieldID] = Drow[Field.FieldID];
                                    break;
                                }
                            }
                        }
                        else if (Field.FieldID.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.MetadataArea))
                        {
                            foreach (DataRow Drow in this.DIDataView.MetadataArea.Select(Area.AreaNId + " = " + DRVPresentationData[Area.AreaNId].ToString()))
                            {
                                RowTableXLSUniqueRows[Field.FieldID] = Drow[Field.FieldID];
                                TableXLSRow[Field.FieldID] = Drow[Field.FieldID];
                                break;
                            }
                        }
                        else if (Field.FieldID.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.MetadataSource))
                        {
                            foreach (DataRow Drow in this.DIDataView.MetadataSource.Select(IndicatorClassifications.ICNId + " = " + DRVPresentationData[IndicatorClassifications.ICNId].ToString()))
                            {
                                RowTableXLSUniqueRows[Field.FieldID] = Drow[Field.FieldID];
                                TableXLSRow[Field.FieldID] = Drow[Field.FieldID];
                                break;
                            }
                        }
                        else if (Field.FieldID.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.IC))
                        {

                            foreach (DataRow Drow in this.DIDataView.IUSICInfo.Select(Indicator_Unit_Subgroup.IUSNId + " = " + DRVPresentationData[Indicator_Unit_Subgroup.IUSNId].ToString()))
                            {
                                RowTableXLSUniqueRows[Field.FieldID] = Drow[Field.FieldID];
                                TableXLSRow[Field.FieldID] = Drow[Field.FieldID];
                                break;
                            }

                            foreach (DataRow Drow in this.DIDataView.IUSICNIdInfo.Select(Indicator_Unit_Subgroup.IUSNId + " = " + DRVPresentationData[Indicator_Unit_Subgroup.IUSNId].ToString()))
                            {
                                if (TableXLSRow[Field.FieldID].ToString() != string.Empty)
                                {
                                    RowDTRowsForIcons[Field.FieldID] = IconElementType.IndicatorClassification;
                                    if (Drow[Field.FieldID] != System.DBNull.Value)
                                    {
                                        RowDTRowsForIcons[Field.FieldID + "_NID"] = Drow[Field.FieldID];
                                    }
                                    else
                                    {
                                        System.Diagnostics.Debug.Print("System.DBNull");
                                    }
                                }
                                break;
                            }
                        }
                        else
                        {
                            if (Field.FieldID == Area.AreaName)
                            {
                                RowTableXLSUniqueRows[Area.AreaID] = DRVPresentationData[Area.AreaID];
                            }
                            else
                            {
                                RowTableXLSUniqueRows[Field.FieldID] = DRVPresentationData[Field.FieldID];
                            }
                            if (string.IsNullOrEmpty(DRVPresentationData[Field.FieldID].ToString()))
                            {
                                if (!string.IsNullOrEmpty(DRVPresentationData[Indicator.IndicatorNId].ToString()))
                                {
                                    TableXLSRow[Field.FieldID] = DILanguage.GetLanguageString("TOTAL");
                                }
                            }
                            else
                            {
                                TableXLSRow[Field.FieldID] = DRVPresentationData[Field.FieldID].ToString();
                            }

                            if (Field.FieldID == Indicator.IndicatorName)
                            {
                                RowDTRowsForIcons[Field.FieldID] = IconElementType.Indicator;
                                RowDTRowsForIcons[Indicator.IndicatorNId] = DRVPresentationData[Indicator.IndicatorNId].ToString();
                            }
                            if (Field.FieldID == Area.AreaName)
                            {
                                RowDTRowsForIcons[Field.FieldID] = IconElementType.Area;
                                RowDTRowsForIcons[Area.AreaNId] = DRVPresentationData[Area.AreaNId].ToString();
                            }
                            if (Field.FieldID == Unit.UnitName)
                            {
                                RowDTRowsForIcons[Field.FieldID] = IconElementType.Unit;
                                RowDTRowsForIcons[Unit.UnitNId] = DRVPresentationData[Unit.UnitNId].ToString();
                            }
                            if (Field.FieldID == IndicatorClassifications.ICName)
                            {
                                RowDTRowsForIcons[Field.FieldID] = IconElementType.IndicatorClassification;
                                RowDTRowsForIcons[IndicatorClassifications.ICNId] = DRVPresentationData[IndicatorClassifications.ICNId].ToString();
                            }
                            if (Field.FieldID == SubgroupVals.SubgroupVal)
                            {
                                RowDTRowsForIcons[Field.FieldID] = IconElementType.SubgroupVals;
                                RowDTRowsForIcons[SubgroupVals.SubgroupValNId] = DRVPresentationData[SubgroupVals.SubgroupValNId].ToString();
                            }
                        }
                    }

                    DTRowsForIcons.Rows.Add(RowDTRowsForIcons);
                    TableXLSUniqueRows.Rows.Add(RowTableXLSUniqueRows);

                    //Get Filtered Rows		
                    string PyramidFilterString = string.Empty;
                    if (this._PresentationType == Presentation.PresentationType.Graph && this._ChartType == 58)
                    {
                        PyramidFilterString = this.PresentationData.RowFilter;
                        FilterString = PyramidFilterString + " AND " + FilterString;
                    }
                    this.PresentationData.RowFilter = FilterString;	 //Filter the records in the PresentationData based on unique row.					
                    DTFiltered = this.PresentationData.ToTable();
                    this.PresentationData.RowFilter = string.Empty;	 //Reset the PresentationData.
                    if (this._PresentationType == Presentation.PresentationType.Graph && this._ChartType == 58)
                    {
                        this.PresentationData.RowFilter = PyramidFilterString;
                    }

                    int x = 0 - this.FootnoteCommentColumnCount;
                    string FilterExpr = string.Empty;

                    for (int Counter = 0; Counter < TotalColFieldVals; Counter++)
                    {
                        FilterExpr = string.Empty;
                        x = x + this.FootnoteCommentColumnCount;

                        //Loop for each column header row value and make a filter string expression.
                        //Filter expression will be used to filter rows in DRFilterd 
                        for (int Columns = 0; Columns < TotalColumnFields; Columns = Columns + 2)
                        {
                            //if (!string.IsNullOrEmpty(this._ColumnArrangementTable.Rows[Counter][Columns].ToString()) && (!this._ColumnArrangementTable.Columns[Columns].ColumnName.StartsWith(TablePresentation.MetadataPrefix) && !this._ColumnArrangementTable.Columns[Columns].ColumnName.StartsWith(TablePresentation.ICPrefix)))
                            if ((!this._ColumnArrangementTable.Columns[Columns].ColumnName.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.Metadata) && !this._ColumnArrangementTable.Columns[Columns].ColumnName.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.IC)))
                            {
                                if (string.IsNullOrEmpty(this._ColumnArrangementTable.Rows[Counter][Columns].ToString()))
                                {
                                    FilterExpr += " AND " + "IsNull(" + this.AddSlash(this._ColumnArrangementTable.Columns[Columns].ColumnName.ToString()) + ",'0')='0'";
                                }
                                else
                                {
                                    FilterExpr += " AND " + this.AddSlash(this._ColumnArrangementTable.Columns[Columns].ColumnName.ToString()) + "='" + DICommon.RemoveQuotes(this._ColumnArrangementTable.Rows[Counter][Columns].ToString()) + "'";
                                }

                            }
                        }
                        if (FilterExpr.Length > 0)
                        {
                            FilterExpr = FilterExpr.Substring(5);
                        }

                        try
                        {
                            foreach (DataRow DRowDTFiltered in DTFiltered.Select(FilterExpr))
                            {

                                // -- Add only numeric data for graph wizard.
                                if (this._PresentationType == Presentation.PresentationType.Graph)
                                {
                                    if ((DataType)DRowDTFiltered[DataExpressionColumns.DataType] == DataType.Numeric)
                                    {
                                        //Regional setting handling for graphwizard
                                        //dataValue = (decimal)DRowDTFiltered[DataExpressionColumns.NumericData];
                                        //TableXLSRow[TotalRowFields + Counter + x] = dataValue.ToString(InvariantCulture); //DRowDTFiltered[Data.DataValue];
                                        TableXLSRow[TotalRowFields + Counter + x] = DRowDTFiltered[Data.DataValue];

                                    }
                                }
                                else
                                {
                                    if (TableXLSRow[TotalRowFields + Counter + x].ToString() == string.Empty)
                                    {
                                        TableXLSRow[TotalRowFields + Counter + x] = DRowDTFiltered[Data.DataValue];
                                    }
                                }

                                switch (this.FootnoteCommentType)
                                {
                                    case FootnoteCommentType.None:
                                        //DO Nothing
                                        break;
                                    case FootnoteCommentType.Comment:
                                        TableXLSRow[TotalRowFields + Counter + x + 1] = GetCommentIndex(DRowDTFiltered[Notes_Data.DataNId].ToString());
                                        break;
                                    case FootnoteCommentType.Footnote:
                                        if (DRowDTFiltered[FootNotes.FootNoteNId].ToString() != "-1" && DRowDTFiltered[FootNotes.FootNoteNId].ToString() != string.Empty)
                                        {
                                            TableXLSRow[TotalRowFields + Counter + x + 1] = GetFootNoteIndex(DRowDTFiltered[FootNotes.FootNoteNId].ToString());
                                        }
                                        break;
                                    case FootnoteCommentType.Both:
                                        if (DRowDTFiltered[FootNotes.FootNoteNId].ToString() != "-1" && DRowDTFiltered[FootNotes.FootNoteNId].ToString() != string.Empty)
                                        {
                                            TableXLSRow[TotalRowFields + Counter + x + 1] = GetFootNoteIndex(DRowDTFiltered[FootNotes.FootNoteNId].ToString());
                                        }
                                        TableXLSRow[TotalRowFields + Counter + x + 2] = GetCommentIndex(DRowDTFiltered[Notes_Data.DataNId].ToString());
                                        break;
                                    default:
                                        break;
                                }
                                //TODO : Code set for Denominator
                                if (this._TemplateStyle.Denominator.Show)
                                {
                                    if (this.FootnoteCommentType == FootnoteCommentType.None)
                                    {
                                        TableXLSRow[TotalRowFields + Counter + x + 1] = DRowDTFiltered[Data.DataDenominator];
                                    }
                                    else if (this.FootnoteCommentType == FootnoteCommentType.Footnote || this.FootnoteCommentType == FootnoteCommentType.Comment)
                                    {
                                        TableXLSRow[TotalRowFields + Counter + x + 2] = DRowDTFiltered[Data.DataDenominator];
                                    }
                                    else if (this.FootnoteCommentType == FootnoteCommentType.Both)
                                    {
                                        TableXLSRow[TotalRowFields + Counter + x + 3] = DRowDTFiltered[Data.DataDenominator];
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            string Message = ex.Message;
                            throw;
                        }
                    }
                    TableXLSRow[TablePresentation.ROWINDEX] = TableXLS.Rows.Count + 1;
                    TableXLSRow[TablePresentation.ROWTYPE] = RowType.DataRow;
                    TableXLSRow[TablePresentation.AREALEVEL] = DRVPresentationData[Area.AreaLevel].ToString();
                    //Fill the order column values(Subgroup,subgroupdimention)
                    foreach (Field field in this.Fields.Rows)
                    {
                        if (field.FieldID == SubgroupVals.SubgroupVal)
                        {
                            try
                            {
                                TableXLSRow[SubgroupVals.SubgroupValOrder] = Convert.ToInt32(DRVPresentationData[SubgroupVals.SubgroupValOrder].ToString());
                            }
                            catch (Exception)
                            {
                                TableXLSRow[SubgroupVals.SubgroupValOrder] = 0;
                            }

                        }
                        else
                        {
                            foreach (DataRow DrSI in this.DIDataView.SubgroupInfo.Rows)
                            {
                                if (field.FieldID == DrSI[SubgroupInfoColumns.Name].ToString())
                                {
                                    try
                                    {
                                        TableXLSRow[DrSI[SubgroupInfoColumns.OrderColName].ToString()] = Convert.ToInt32(DRVPresentationData[DrSI[SubgroupInfoColumns.OrderColName].ToString()].ToString());
                                    }
                                    catch (Exception)
                                    {
                                        TableXLSRow[DrSI[SubgroupInfoColumns.OrderColName].ToString()] = 0;
                                    }
                                }
                            }
                        }
                    }



                    if (!string.IsNullOrEmpty(this._TableReport.AreaSelection.AreaLevels))
                    {
                        TableXLSRow[TablePresentation.AREAPARENTCHILDRELATION] = DRVPresentationData[TablePresentation.AREAPARENTCHILDRELATION].ToString();
                        for (int i = this.MinAreaLevel; i <= this.MaxAreaLevel; i++)
                        {
                            TableXLSRow["AL" + i.ToString()] = DRVPresentationData["AL" + i.ToString()].ToString();
                        }
                        for (int i = this._Fields.Rows.Count; i < TotalTableXLSColumns; i++)
                        {
                            if (!this._TemplateStyle.LevelFormat[DRVPresentationData[Area.AreaLevel].ToString()].ShowDataValues)
                            {
                                TableXLSRow[i] = string.Empty;
                            }
                        }
                    }
                    TableXLS.Rows.Add(TableXLSRow);
                    //Will be used in suppressduplicate function only.Arraylist will contains the unique values of first column.
                    if (!this.ALSuppressColValues.Contains(TableXLSRow[0].ToString()))
                    {
                        this.ALSuppressColValues.Add(TableXLSRow[0].ToString().Trim());
                    }
                }
            }

            this.MoveRowsInSingleColumn(TableXLS);
        }

        /// <summary>
        /// Move Rows filed into the single column. This code execute for graph non-excel mode.
        /// </summary>
        /// <param name="TableXLS"></param>
        private void MoveRowsInSingleColumn(DataTable TableXLS)
        {
            if ((TableXLS.Rows.Count > 0) && (this._PresentationType == Presentation.PresentationType.Graph & !this._ShowExcel))
            {
                int Index = 0;
                foreach (Field RowField in this._Fields.Rows)
                {
                    Index += 1;
                    if (Index > 1)
                    {
                        foreach (DataRow Row in TableXLS.Rows)
                        {
                            Row[0] = Row[0].ToString() + " - " + Row[RowField.FieldID].ToString();
                        }
                    }
                }
                Index = 0;
                foreach (Field RowField in this._Fields.Rows)
                {
                    Index += 1;
                    if (Index > 1)
                    {
                        TableXLS.Columns.Remove(RowField.FieldID);
                    }
                }
            }
        }

        /// <summary>
        /// Populate blank TableXLS.
        /// </summary>
        /// <param name="FilterString">FilterString for TableXLS which filter it on the bases of IC arrangement</param>
        /// <param name="TableXLS">Table XLS</param>
        /// <param name="UniqueRows">DataTable that holds the Unique Rows</param>
        /// <param name="TotalRowFields">Total RFs (Row Fields)</param>
        /// <param name="TotalColFieldVals">Total CFVs (Column Field Vaues)</param>
        /// <param name="TotalColumnFields">Total CFs (Column Fields) including NId fields</param>
        //private void FillTableXLS(string FilterString, DataTable TableXLS, int TotalRowFields, int TotalColFieldVals, int TotalColumnFields)
        //{
        //    //Hashtable HTIndicator;
        //    //Hashtable HTIC;
        //    bool ColumnHeaderNotFound; //

        //    DataTable TableXLSUniqueRows = new DataTable();		// DataTable that holds the Unique Rows in presntation data based on Row Selections on step 1 
        //    DataRow RowTableXLSUniqueRows;

        //    DataTable DTFiltered = new DataTable();				// DataTable that holds the filtered records from presentatioData based on each row of TableXLSUniqueRows.
        //    DataRow TableXLSRow;
        //    DataRow RowDTDataSheet;
        //    DataRow RowDTRowsForIcons;
        //    string FootNoteIndex = string.Empty;
        //    int FieldIndex = 0;
        //    int Count = 0;

        //    //Make the header of DataTable UniqueRows.			
        //    foreach (Field field in this._Fields.Rows)
        //    {
        //        TableXLSUniqueRows.Columns.Add(field.FieldID);
        //    }
        //    //Make a loop for each Record in the PresentationData

        //    string[] ArrIndicatorFields = this.MDIndicatorFields.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        //    string[] ArrAreaFields = this.MDAreaFields.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        //    string[] ArrSourceFields = this.MDSourceFields.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        //    string[] ArrICFields = this.ICInfoFields.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        //    foreach (DataRow DRVPresentationData in this.PresentationData.ToTable().Select(FilterString))
        //    {
        //        //-----------------Fill DataTable for DataSheet---------------
        //        RowDTDataSheet = this.DTDataSheet.NewRow();
        //        RowDTDataSheet[Timeperiods.TimePeriod] = DRVPresentationData[Timeperiods.TimePeriod];
        //        RowDTDataSheet[Area.AreaID] = DRVPresentationData[Area.AreaID];
        //        RowDTDataSheet[Area.AreaName] = DRVPresentationData[Area.AreaName];
        //        RowDTDataSheet[Indicator.IndicatorName] = DRVPresentationData[Indicator.IndicatorName];
        //        RowDTDataSheet[Data.DataValue] = DRVPresentationData[Data.DataValue];
        //        RowDTDataSheet[Unit.UnitName] = DRVPresentationData[Unit.UnitName];
        //        RowDTDataSheet[SubgroupVals.SubgroupVal] = DRVPresentationData[SubgroupVals.SubgroupVal];
        //        RowDTDataSheet[IndicatorClassifications.ICName] = DRVPresentationData[IndicatorClassifications.ICName];
        //        if (DRVPresentationData[FootNotes.FootNoteNId].ToString() != "-1")
        //        {
        //            RowDTDataSheet[FootNotes.FootNote] = DRVPresentationData[FootNotes.FootNoteNId];
        //        }
        //        else
        //        {
        //            RowDTDataSheet[FootNotes.FootNote] = string.Empty;
        //        }

        //        //For each indicator NId

        //        for (FieldIndex = 0; FieldIndex < ArrIndicatorFields.Length; FieldIndex++)
        //        {
        //            foreach (DataRow Drow in this.DIDataView.MetadataIndicator.Select(Indicator.IndicatorNId + " = " + DRVPresentationData[Indicator.IndicatorNId].ToString()))
        //            {
        //                RowDTDataSheet[ArrIndicatorFields[FieldIndex]] = Drow[ArrIndicatorFields[FieldIndex]].ToString();
        //                break;
        //            }
        //        }
        //        for (FieldIndex = 0; FieldIndex < ArrAreaFields.Length; FieldIndex++)
        //        {
        //            foreach (DataRow Drow in this.DIDataView.MetadataArea.Select(Area.AreaNId + " = " + DRVPresentationData[Area.AreaNId].ToString()))
        //            {
        //                RowDTDataSheet[ArrAreaFields[FieldIndex]] = Drow[ArrAreaFields[FieldIndex]].ToString();
        //                break;
        //            }
        //        }
        //        for (FieldIndex = 0; FieldIndex < ArrSourceFields.Length; FieldIndex++)
        //        {
        //            foreach (DataRow Drow in this.DIDataView.MetadataSource.Select(IndicatorClassifications.ICNId + " = " + DRVPresentationData[IndicatorClassifications.ICNId].ToString()))
        //            {
        //                RowDTDataSheet[ArrSourceFields[FieldIndex]] = Drow[ArrSourceFields[FieldIndex]].ToString();
        //                break;
        //            }
        //        }

        //        for (FieldIndex = 0; FieldIndex < ArrICFields.Length; FieldIndex++)
        //        {
        //            foreach (DataRow Drow in this.DIDataView.IUSICInfo.Select(Indicator_Unit_Subgroup.IUSNId + " = " + DRVPresentationData[Indicator_Unit_Subgroup.IUSNId].ToString()))
        //            {
        //                RowDTDataSheet[ArrICFields[FieldIndex]] = Drow[ArrICFields[FieldIndex]].ToString();
        //                break;
        //            }
        //        }



        //        this.DTDataSheet.Rows.Add(RowDTDataSheet);

        //        //------------------------------------------------------------


        //        FilterString = string.Empty;

        //        //Make a filter string for extracting unique rows in presntation data based on Row Selection on Step 1
        //        foreach (Field Field in this._Fields.Rows)
        //        {
        //            if (!Field.FieldID.StartsWith(TablePresentation.MetadataPrefix) && !Field.FieldID.StartsWith(TablePresentation.ICPrefix))
        //            {
        //                if (!string.IsNullOrEmpty(DRVPresentationData[Field.FieldID].ToString()))
        //                {
        //                    if (FilterString != string.Empty)
        //                    {
        //                        FilterString = FilterString + " AND ";
        //                    }
        //                    FilterString = FilterString + Field.FieldID + "='" + DICommon.RemoveQuotes(DRVPresentationData[Field.FieldID].ToString()) + "'";
        //                }
        //            }
        //        }



        //        //For Unique record insert the record into tables UniqueRows and TableXLS.
        //        if (TableXLSUniqueRows.Select(FilterString).Length == 0)
        //        {
        //            if (Count % 100 == 0)
        //            {
        //                SW.WriteLine(DateTime.Now.ToString() + " Loop Starts : Loop No -- " + Count);

        //            }
        //            Count++;

        //            RowTableXLSUniqueRows = TableXLSUniqueRows.NewRow();
        //            TableXLSRow = TableXLS.NewRow();
        //            RowDTRowsForIcons = DTRowsForIcons.NewRow();

        //            //Insert the Values for Rows in	RowUniqueRows and TableXLS.
        //            foreach (Field Field in this._Fields.Rows)
        //            {
        //                if (Field.FieldID.StartsWith(MetadataIndicator))
        //                {
        //                    if (this.MDIndicatorFields.Length > 0)
        //                    {
        //                        foreach (DataRow Drow in this.DIDataView.MetadataIndicator.Select(Indicator.IndicatorNId + " = " + DRVPresentationData[Indicator.IndicatorNId].ToString()))
        //                        {
        //                            RowTableXLSUniqueRows[Field.FieldID] = Drow[Field.FieldID];
        //                            TableXLSRow[Field.FieldID] = Drow[Field.FieldID];
        //                            break;
        //                        }
        //                    }
        //                }
        //                else if (Field.FieldID.StartsWith(MetadataArea))
        //                {
        //                    foreach (DataRow Drow in this.DIDataView.MetadataArea.Select(Area.AreaNId + " = " + DRVPresentationData[Area.AreaNId].ToString()))
        //                    {
        //                        RowTableXLSUniqueRows[Field.FieldID] = Drow[Field.FieldID];
        //                        TableXLSRow[Field.FieldID] = Drow[Field.FieldID];
        //                        break;
        //                    }
        //                }
        //                else if (Field.FieldID.StartsWith(MetadataSource))
        //                {
        //                    foreach (DataRow Drow in this.DIDataView.MetadataSource.Select(IndicatorClassifications.ICNId + " = " + DRVPresentationData[IndicatorClassifications.ICNId].ToString()))
        //                    {
        //                        RowTableXLSUniqueRows[Field.FieldID] = Drow[Field.FieldID];
        //                        TableXLSRow[Field.FieldID] = Drow[Field.FieldID];
        //                        break;
        //                    }
        //                }
        //                else if (Field.FieldID.StartsWith(TablePresentation.ICPrefix))
        //                {

        //                    foreach (DataRow Drow in this.DIDataView.IUSICInfo.Select(Indicator_Unit_Subgroup.IUSNId + " = " + DRVPresentationData[Indicator_Unit_Subgroup.IUSNId].ToString()))
        //                    {
        //                        RowTableXLSUniqueRows[Field.FieldID] = Drow[Field.FieldID];
        //                        TableXLSRow[Field.FieldID] = Drow[Field.FieldID];
        //                        break;
        //                    }

        //                    foreach (DataRow Drow in this.DIDataView.IUSICNIdInfo.Select(Indicator_Unit_Subgroup.IUSNId + " = " + DRVPresentationData[Indicator_Unit_Subgroup.IUSNId].ToString()))
        //                    {
        //                        if (TableXLSRow[Field.FieldID].ToString() != string.Empty)
        //                        {
        //                            RowDTRowsForIcons[Field.FieldID] = IconElementType.IndicatorClassification;
        //                            if (Drow[Field.FieldID] != System.DBNull.Value)
        //                            {
        //                                RowDTRowsForIcons[Field.FieldID + "_NID"] = Drow[Field.FieldID];
        //                            }
        //                            else
        //                            {
        //                                System.Diagnostics.Debug.Print("System.DBNull");
        //                            }
        //                        }
        //                        break;
        //                    }
        //                }
        //                else
        //                {
        //                    RowTableXLSUniqueRows[Field.FieldID] = DRVPresentationData[Field.FieldID].ToString();
        //                    TableXLSRow[Field.FieldID] = DRVPresentationData[Field.FieldID].ToString();
        //                    if (Field.FieldID == Indicator.IndicatorName)
        //                    {
        //                        RowDTRowsForIcons[Field.FieldID] = IconElementType.Indicator;
        //                        RowDTRowsForIcons[Indicator.IndicatorNId] = DRVPresentationData[Indicator.IndicatorNId].ToString();
        //                    }
        //                    if (Field.FieldID == Area.AreaName)
        //                    {
        //                        RowDTRowsForIcons[Field.FieldID] = IconElementType.Area;
        //                        RowDTRowsForIcons[Area.AreaNId] = DRVPresentationData[Area.AreaNId].ToString();
        //                    }
        //                    if (Field.FieldID == Unit.UnitName)
        //                    {
        //                        RowDTRowsForIcons[Field.FieldID] = IconElementType.Unit;
        //                        RowDTRowsForIcons[Unit.UnitNId] = DRVPresentationData[Unit.UnitNId].ToString();
        //                    }
        //                    if (Field.FieldID == IndicatorClassifications.ICName)
        //                    {
        //                        RowDTRowsForIcons[Field.FieldID] = IconElementType.IndicatorClassification;
        //                        RowDTRowsForIcons[IndicatorClassifications.ICNId] = DRVPresentationData[IndicatorClassifications.ICNId].ToString();
        //                    }
        //                    if (Field.FieldID == SubgroupVals.SubgroupVal)
        //                    {
        //                        RowDTRowsForIcons[Field.FieldID] = IconElementType.SubgroupVals;
        //                        RowDTRowsForIcons[SubgroupVals.SubgroupValNId] = DRVPresentationData[SubgroupVals.SubgroupValNId].ToString();
        //                    }
        //                }
        //            }

        //            DTRowsForIcons.Rows.Add(RowDTRowsForIcons);
        //            TableXLSUniqueRows.Rows.Add(RowTableXLSUniqueRows);

        //            //Get Filtered Rows
        //            this.PresentationData.RowFilter = FilterString;	 //Filter the records in the PresentationData based on unique row.
        //            DTFiltered = this.PresentationData.ToTable();
        //            this.PresentationData.RowFilter = string.Empty;	 //Reset the PresentationData.


        //            int x = 0 - this.FootnoteCommentColumnCount;
        //            string FilterExpr = string.Empty;

        //            for (int Counter = 0; Counter < TotalColFieldVals; Counter++)
        //            {
        //                FilterExpr = string.Empty;
        //                x = x + this.FootnoteCommentColumnCount;

        //                //Loop for each column header row value and make a filter string expression.
        //                //Filter expression will be used to filter rows in DRFilterd 
        //                for (int Columns = 0; Columns < TotalColumnFields; Columns = Columns + 2)
        //                {
        //                    if (!this._ColumnArrangementTable.Columns[Columns].ColumnName.StartsWith(TablePresentation.MetadataPrefix) && !this._ColumnArrangementTable.Columns[Columns].ColumnName.StartsWith(TablePresentation.ICPrefix))
        //                    {
        //                        FilterExpr += " AND " + this._ColumnArrangementTable.Columns[Columns].ColumnName.ToString() + "='" + this._ColumnArrangementTable.Rows[Counter][Columns].ToString() + "'";
        //                    }
        //                }
        //                if (FilterExpr.Length > 0)
        //                {
        //                    FilterExpr = FilterExpr.Substring(5);
        //                }

        //                //////foreach (DataRow DRowDTFiltered in DTFiltered.Rows)
        //                //foreach (DataRow DRowDTFiltered in DTFiltered.Select(ColumnName + "='" + this._ColumnArrangementTable.Rows[Counter][0].ToString() + "'"))
        //                foreach (DataRow DRowDTFiltered in DTFiltered.Select(FilterExpr))
        //                {
        //                    //////ColumnHeaderNotFound = false;




        //                    ////////If the column values of DRowDTFiltered are equals to column values of _ColumnArrangementTable
        //                    ////////then Insert data values into TableXLS

        //                    ////////Loop for each column header row value
        //                    //////for (int Columns = 0; Columns < TotalColumnFields; Columns = Columns + 2)
        //                    //////{
        //                    //////    if (!this._ColumnArrangementTable.Columns[Columns].ColumnName.StartsWith(TablePresentation.MetadataPrefix) && !this._ColumnArrangementTable.Columns[Columns].ColumnName.StartsWith(TablePresentation.ICPrefix))
        //                    //////    {
        //                    //////        if (DRowDTFiltered[this._ColumnArrangementTable.Columns[Columns].ColumnName].ToString() != this._ColumnArrangementTable.Rows[Counter][Columns].ToString())
        //                    //////        {
        //                    //////            ColumnHeaderNotFound = true;
        //                    //////            break;
        //                    //////        }
        //                    //////    }
        //                    //////}

        //                    //////if (ColumnHeaderNotFound == false)
        //                    //////{
        //                    // -- Add only numeric data for graph wizard.
        //                    if (this._PresentationType == Presentation.PresentationType.Graph)
        //                    {
        //                        if ((DataType)DRowDTFiltered[DataExpressionColumns.DataType] == DataType.Numeric)
        //                        {
        //                            TableXLSRow[TotalRowFields + Counter + x] = DRowDTFiltered[Data.DataValue];
        //                        }
        //                    }
        //                    else
        //                    {
        //                        TableXLSRow[TotalRowFields + Counter + x] = DRowDTFiltered[Data.DataValue];
        //                    }

        //                    switch (this.FootnoteCommentType)
        //                    {
        //                        case FootnoteCommentType.None:
        //                        //DO Nothing
        //                        break;
        //                        case FootnoteCommentType.Comment:
        //                        TableXLSRow[TotalRowFields + Counter + x + 1] = GetCommentIndex(DRowDTFiltered[Notes_Data.DataNId].ToString());
        //                        break;
        //                        case FootnoteCommentType.Footnote:
        //                        if (DRowDTFiltered[FootNotes.FootNoteNId].ToString() != "-1")
        //                        {
        //                            TableXLSRow[TotalRowFields + Counter + x + 1] = GetFootNoteIndex(DRowDTFiltered[FootNotes.FootNoteNId].ToString());
        //                        }
        //                        break;
        //                        case FootnoteCommentType.Both:
        //                        if (DRowDTFiltered[FootNotes.FootNoteNId].ToString() != "-1")
        //                        {
        //                            TableXLSRow[TotalRowFields + Counter + x + 1] = GetFootNoteIndex(DRowDTFiltered[FootNotes.FootNoteNId].ToString());
        //                        }
        //                        TableXLSRow[TotalRowFields + Counter + x + 2] = GetCommentIndex(DRowDTFiltered[Notes_Data.DataNId].ToString());
        //                        break;
        //                        default:
        //                        break;
        //                    }
        //                    //TODO : Code set for Denominator
        //                    if (this._ShowDenominator)
        //                    {
        //                        if (this.FootnoteCommentType == FootnoteCommentType.None)
        //                        {
        //                            TableXLSRow[TotalRowFields + Counter + x + 1] = DRowDTFiltered[Data.DataDenominator];
        //                        }
        //                        else if (this.FootnoteCommentType == FootnoteCommentType.Footnote || this.FootnoteCommentType == FootnoteCommentType.Comment)
        //                        {
        //                            TableXLSRow[TotalRowFields + Counter + x + 2] = DRowDTFiltered[Data.DataDenominator];
        //                        }
        //                        else if (this.FootnoteCommentType == FootnoteCommentType.Both)
        //                        {
        //                            TableXLSRow[TotalRowFields + Counter + x + 3] = DRowDTFiltered[Data.DataDenominator];
        //                        }
        //                    }
        //                    // OLD : Nothing.
        //                    //////}
        //                }
        //            }
        //            TableXLSRow[TablePresentation.ROWINDEX] = TableXLS.Rows.Count + 1;
        //            TableXLSRow[TablePresentation.ROWTYPE] = RowType.DataRow;
        //            TableXLS.Rows.Add(TableXLSRow);
        //        }
        //    }
        //}


        /// <summary>
        /// Sets the Columns for Notes and Footnotes.
        /// </summary>
        /// <param name="DataNIds">Comma delimited string containing DataNIds of PresentationData</param>
        /// <returns>Column Count for notes and footnote</returns>
        private int FootnoteCommentColumnsCount(string DataNIds)
        {
            int RetVal = 0;
            string NotesNIds = string.Empty;		//comma delimited string of NotesNIds.		
            string TemRowFilter = this.PresentationData.RowFilter;
            DataRow RowMasterComments;				//DataRow containing record of DataTable DTMasterComments

            //Select the NotesNIds from Notes_Data
            string QueryGetNotesNIds = this.DIQueries.Notes.GetNotes_Data("", DataNIds, CheckedStatus.True);

            try
            {

                System.Data.IDataReader DrNotesNIds = DIConnection.ExecuteReader(QueryGetNotesNIds);
                while (DrNotesNIds.Read())
                {
                    if (NotesNIds != string.Empty)
                    {
                        NotesNIds += ",";
                    }
                    NotesNIds += DrNotesNIds[Notes_Data.NotesNId];
                    RowMasterComments = this.DTMasterComments.NewRow();
                    RowMasterComments[Notes_Data.DataNId] = DrNotesNIds[Notes_Data.DataNId].ToString();
                    RowMasterComments[Notes_Data.NotesNId] = DrNotesNIds[Notes_Data.NotesNId].ToString();
                    this.DTMasterComments.Rows.Add(RowMasterComments);
                }
                DrNotesNIds.Close();
                DrNotesNIds.Dispose();

            }
            catch (Exception)
            {
                // Case where note table does not exists or notes_approved field is not available
            }

            // Get Footnote column count 

            if (this._TemplateStyle.Footnotes.Show)
            {
                this.PresentationData.RowFilter = FootNotes.FootNoteNId + " <> -1";
                if (this.PresentationData.Count > 0)
                {
                    if (NotesNIds == string.Empty || this._TemplateStyle.Comments.Show == false)   //No Comments are Present
                    {
                        this.FootnoteCommentType = FootnoteCommentType.Footnote;
                        RetVal = 1;
                    }
                    else
                    {
                        this.FootnoteCommentType = FootnoteCommentType.Both;
                        RetVal = 2;
                    }
                }
                else
                {
                    this.FootnoteCommentType = FootnoteCommentType.None;
                    RetVal = 0;
                }
                this.PresentationData.RowFilter = string.Empty;
            }
            else if (this._TemplateStyle.Comments.Show)
            {
                this.PresentationData.RowFilter = FootNotes.FootNoteNId + " <> -1";
                if (this.PresentationData.Count > 0)
                {
                    if (NotesNIds == string.Empty)   //No Comments are Present
                    {
                        this.FootnoteCommentType = FootnoteCommentType.None;
                        RetVal = 0;
                    }
                    else
                    {
                        this.FootnoteCommentType = FootnoteCommentType.Comment;
                        RetVal = 1;
                    }
                }
                else
                {
                    if (this._TemplateStyle.Comments.Show)
                    {
                        if (NotesNIds == string.Empty)	//No Comments are present
                        {
                            this.FootnoteCommentType = FootnoteCommentType.None;
                            RetVal = 0;
                        }
                        else
                        {
                            this.FootnoteCommentType = FootnoteCommentType.Comment;
                            RetVal = 1;
                        }
                    }
                }
                this.PresentationData.RowFilter = string.Empty;
            }
            this.PresentationData.RowFilter = TemRowFilter;
            return RetVal;
        }

        /// <summary>
        /// Generates or Retrieves the FootNoteIndex.
        /// </summary>
        /// <param name="FootNoteNid">FootNote Nid</param>
        /// <returns>FootNote Index</returns>
        private string GetFootNoteIndex(string FootNoteNid)
        {
            string Retval = string.Empty;
            DataRow RowDTFootNote;
            DataRow[] RowsDTFootNote;
            RowsDTFootNote = this.DTFootNote.Select(FootNotes.FootNoteNId + "=" + FootNoteNid);
            if (RowsDTFootNote.Length > 0)
            {
                Retval = RowsDTFootNote[0][0].ToString();
            }
            else
            {
                RowDTFootNote = this.DTFootNote.NewRow();
                RowDTFootNote[0] = this.DTFootNote.Rows.Count + 1;
                Retval = (this.DTFootNote.Rows.Count + 1).ToString();
                RowDTFootNote[FootNotes.FootNoteNId] = FootNoteNid;
                this.DTFootNote.Rows.Add(RowDTFootNote);
            }
            return Retval;
        }

        /// <summary>
        /// Generates or Retrieves the Comment Index.
        /// </summary>
        /// <param name="DataNId">DataNId</param>
        /// <returns>Comment Index.</returns>
        private string GetCommentIndex(string DataNId)
        {
            string RetVal = string.Empty;
            char[] Characters;
            DataRow[] RowsDTMasterComments;
            DataRow RowDTComments;
            DataRow[] RowsDTComments;
            RowsDTMasterComments = this.DTMasterComments.Select(Notes_Data.DataNId + "='" + DataNId + "'");
            foreach (DataRow DRowDTMasterComments in RowsDTMasterComments)
            {
                RowsDTComments = this.DTComments.Select(Notes_Data.NotesNId + "='" + DRowDTMasterComments[Notes_Data.NotesNId] + "'");
                if (RowsDTComments.Length > 0)
                {
                    if (RetVal != string.Empty)
                    {
                        RetVal += ",";
                    }
                    RetVal += RowsDTComments[0][2].ToString();
                }
                else
                {
                    RowDTComments = DTComments.NewRow();
                    RowDTComments[0] = this.DTComments.Rows.Count + 1;
                    RowDTComments[Notes_Data.NotesNId] = DRowDTMasterComments[Notes_Data.NotesNId].ToString();
                    Characters = System.Text.Encoding.ASCII.GetChars(new byte[] { Convert.ToByte(this.DTComments.Rows.Count + 65) });
                    RowDTComments["NoteSymbol"] = Characters[0].ToString();
                    if (RetVal != string.Empty)
                    {
                        RetVal += ",";
                    }
                    RetVal += Characters[0].ToString();
                    this.DTComments.Rows.Add(RowDTComments);
                }
            }
            return RetVal;
        }

        /// <summary>
        /// Receives row DataTable and inserts rows for aggregate values and grand aggregate values.
        /// </summary>
        /// <param name="DTTableXLS">DataTable</param>
        /// <param name="TotalColumns">TotalColumn of DataTable</param>
        /// <param name="TotalRowFields">Total Rows of DataTable.</param>
        /// <returns>DataTable after implementation of aggregate.</returns>
        private DataTable ApplyAggregate(DataTable TableXLS, int TotalColFieldVals, int TotalRowFields)
        {
            int TotalColumns = TotalRowFields + (TotalColFieldVals * (this.FootnoteCommentColumnCount + 1));
            DataTable ICDataTable = new DataTable();						// DataTable consisting a group of records with associated IC.
            ICDataTable = TableXLS.Clone();						// Copy the schema of TableXLS into ICDataTable.
            DataTable RetVal = null;
            DataTable DTDistinctFields;				// DataTable containing distinct record based on Aggregate function.
            const int MeanDecPlaces = 2;

            DataTable DTAggregateTable = new DataTable();		// DataTable containing TableXLS with Aggregates applied 
            DTAggregateTable = TableXLS.Clone();				// Get the Structure of TableXLS.
            DataRow DTAggregateTableRow;						// DataRow Containing record of DTAggregateTable

            string FilterString = string.Empty;
            string AreaParentNId = string.Empty;
            int TotalColumnFields = (this._ColumnArrangementTable.Columns.Count - 2) / 2;
            bool RecordsInICDataTable = false;									// Is set to true when there is data and set to false when RowType is 			
            bool DistinctFieldsFound = true;
            bool IsEmptyRow = false;
            int Rows;
            int Columns;

            //Arrays that will hold the values of each column in the DataTable DTTableXLS.
            //Values will depend upon the value of property AggregateFunction 
            Double[] GrandSumDataValue = new double[TotalColumns];
            Double[] TotalNumericValues = new double[TotalColumns];
            Double[] TotalValuesCount = new double[TotalColumns];
            Nullable<Double>[] GrandMinimumDataValue = new Nullable<double>[TotalColumns];
            Nullable<Double>[] GrandMaximumDataValue = new Nullable<double>[TotalColumns];

            //Arrays that will hold the values of each column in the DataTable DTTableXLS for each group(Grouping in AggregateField) .
            Double[] GroupSumDataValue;
            Double[] GroupTotalNumericValues;
            Double[] GroupTotalValueCount;
            Nullable<Double>[] GroupMinimumDataValue;
            Nullable<Double>[] GroupMaximumDataValue;

            //If no Aggregate Field is selected.
            //Don't do aggregate for FootNotes and Notes Columns.
            if (this._AggregateFieldID == TablePresentation.NONE)
            {

                //Add a Header in DTAggregateTable.
                Rows = 0;
                //If no Columns are present then insert the first row containing header.
                if (TotalColumnFields == 0)
                {
                    DTAggregateTable.ImportRow(TableXLS.Rows[0]);
                    Rows++;
                }
                foreach (DataRow DRowTableXLS in TableXLS.Rows)
                {
                    if (Rows >= TotalColumnFields)
                    {
                        break;
                    }
                    else
                    {
                        DTAggregateTable.ImportRow(DRowTableXLS);
                    }
                    Rows++;
                }

                //Itetrate through TableXLS from starting row carrying data to last row.
                for (int i = Rows; i < TableXLS.Rows.Count; i++)
                {
                    //If Row's ROWTYPE is of ICAggregate or Row is the last row then Add records into DTAggregateTable
                    if ((TableXLS.Rows[i][TablePresentation.ROWTYPE].ToString() == RowType.ICAggregate.ToString()) || (i == TableXLS.Rows.Count - 1))
                    {
                        //If there is any record in ICDataTable
                        if (RecordsInICDataTable == true || (TableXLS.Rows.Count - TotalColumnFields) == 1)
                        {
                            //Insert the last record of TableXLS  when row has datavalue.
                            if ((i == TableXLS.Rows.Count - 1) && (TableXLS.Rows[i][TablePresentation.ROWTYPE].ToString() == RowType.DataRow.ToString()))
                            {
                                TableXLS.Rows[i][TablePresentation.ROWINDEX] = DTAggregateTable.Rows.Count + 1;
                                ICDataTable.ImportRow(TableXLS.Rows[i]);
                            }

                            foreach (DataRow DRowICDataTable in ICDataTable.Rows)
                            {
                                if (DRowICDataTable[TablePresentation.ROWTYPE].ToString() == RowType.EmptyRow.ToString())
                                {
                                    IsEmptyRow = true;
                                }
                                else
                                {
                                    IsEmptyRow = false;
                                    DRowICDataTable[TablePresentation.ROWINDEX] = DTAggregateTable.Rows.Count + 1;
                                    DTAggregateTable.ImportRow(DRowICDataTable);
                                    for (Columns = 0; Columns < TotalColumns; Columns++)
                                    {
                                        //Leave the Rows Section in DataTable DTTableXLS.
                                        if (Columns >= TotalRowFields)
                                        {
                                            if (DICommon.IsNumeric(DRowICDataTable[Columns].ToString()))
                                            {
                                                GrandSumDataValue[Columns] = GrandSumDataValue[Columns] + DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                                                TotalNumericValues[Columns] = TotalNumericValues[Columns] + 1;
                                                TotalValuesCount[Columns] = TotalValuesCount[Columns] + 1;
                                                if (GrandMinimumDataValue[Columns] == null)
                                                {
                                                    if (DRowICDataTable[Columns].ToString() != string.Empty)
                                                    {
                                                        GrandMinimumDataValue[Columns] = DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                                                    }
                                                }
                                                else
                                                {
                                                    if (DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture) < GrandMinimumDataValue[Columns])
                                                    {
                                                        GrandMinimumDataValue[Columns] = DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                                                    }
                                                }
                                                //For Maximum
                                                if (GrandMaximumDataValue[Columns] == null)
                                                {
                                                    if (DRowICDataTable[Columns].ToString() != string.Empty)
                                                    {
                                                        GrandMaximumDataValue[Columns] = DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                                                    }
                                                }
                                                else
                                                {
                                                    if (DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture) > GrandMaximumDataValue[Columns])
                                                    {
                                                        GrandMaximumDataValue[Columns] = DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                                                    }
                                                }
                                            }
                                            else if (DRowICDataTable[Columns].ToString().Trim() != "")
                                            {
                                                TotalValuesCount[Columns] = TotalValuesCount[Columns] + 1;
                                            }
                                            Columns = Columns + this.FootnoteCommentColumnCount;
                                        }
                                    }
                                }
                            }

                            //Insert a Empty row.
                            if (IsEmptyRow)
                            {
                                DTAggregateTableRow = DTAggregateTable.NewRow();
                                DTAggregateTableRow[TablePresentation.ROWTYPE] = RowType.EmptyRow;
                                DTAggregateTableRow[TablePresentation.ROWINDEX] = DTAggregateTable.Rows.Count + 1;
                                DTAggregateTable.Rows.Add(DTAggregateTableRow);
                                IsEmptyRow = false;
                            }
                            else
                            {
                                foreach (AggregateType AggType in this._LstAggregateFunction)
                                {
                                    DTAggregateTableRow = DTAggregateTable.NewRow();
                                    if (AggType == AggregateType.Sum)
                                    {
                                        DTAggregateTableRow[0] = LanguageStrings.GrandSum;
                                    }
                                    else if (AggType == AggregateType.Mean)
                                    {
                                        DTAggregateTableRow[0] = LanguageStrings.GrandAverage;
                                    }
                                    else if (AggType == AggregateType.Count)
                                    {
                                        DTAggregateTableRow[0] = LanguageStrings.Count;
                                    }
                                    else if (AggType == AggregateType.Minimum)
                                    {
                                        DTAggregateTableRow[0] = LanguageStrings.Minimum;
                                    }
                                    else if (AggType == AggregateType.Maximum)
                                    {
                                        DTAggregateTableRow[0] = LanguageStrings.Maximum;
                                    }

                                    //Add the Values for respective Aggregate Function for each column in the last Row of DTTableXLS
                                    for (Columns = 0; Columns < TotalColumns; Columns++)
                                    {
                                        if (Columns >= TotalRowFields)
                                        {
                                            if (AggType == AggregateType.Sum)
                                            {
                                                DTAggregateTableRow[Columns] = GrandSumDataValue[Columns].ToString();
                                            }
                                            else if (AggType == AggregateType.Mean)
                                            {
                                                if (!string.IsNullOrEmpty(TotalNumericValues[Columns].ToString().Trim()) && Convert.ToInt32(TotalNumericValues[Columns]) > 0)
                                                {
                                                    DTAggregateTableRow[Columns] = Strings.FormatNumber(GrandSumDataValue[Columns] / TotalNumericValues[Columns], MeanDecPlaces, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault);
                                                }
                                                else
                                                {
                                                    DTAggregateTableRow[Columns] = string.Empty;
                                                }
                                            }
                                            else if (AggType == AggregateType.Count)
                                            {
                                                DTAggregateTableRow[Columns] = TotalValuesCount[Columns].ToString();
                                            }
                                            else if (AggType == AggregateType.Minimum)
                                            {
                                                DTAggregateTableRow[Columns] = GrandMinimumDataValue[Columns].ToString();
                                            }
                                            else if (AggType == AggregateType.Maximum)
                                            {
                                                DTAggregateTableRow[Columns] = GrandMaximumDataValue[Columns].ToString();
                                            }
                                            Columns = Columns + this.FootnoteCommentColumnCount;
                                        }
                                    }
                                    DTAggregateTableRow[TablePresentation.ROWTYPE] = RowType.GroupAggregate;
                                    DTAggregateTableRow[TablePresentation.ROWINDEX] = DTAggregateTable.Rows.Count + 1;
                                    DTAggregateTable.Rows.Add(DTAggregateTableRow);
                                }
                            }
                        }
                        if (this._SelectedIndicatorClassification)
                        {
                            if (TableXLS.Rows[i][TablePresentation.ROWTYPE].ToString() == RowType.ICAggregate.ToString())
                            {
                                //Insert a row of type ICAggregate.
                                TableXLS.Rows[i][TablePresentation.ROWINDEX] = DTAggregateTable.Rows.Count + 1;
                                DTAggregateTable.ImportRow(TableXLS.Rows[i]);
                            }
                        }
                        ICDataTable.Clear();
                        RecordsInICDataTable = false;
                    }
                    else
                    {
                        //Insert a DataRow if the Row is not a blank row.
                        ICDataTable.ImportRow(TableXLS.Rows[i]);
                        RecordsInICDataTable = true;
                    }
                }
                RetVal = DTAggregateTable;
            }

            //If aggregate field is selected then
            else
            {
                //AddHeader
                Rows = 0;
                //If no Columns are present then insert the first row containing header.
                if (TotalColumnFields == 0)
                {
                    //DTAggregateTable.ImportRow(TableXLS.Rows[0]);
                }
                foreach (DataRow DRowTableXLS in TableXLS.Rows)
                {
                    if (Rows >= TotalColumnFields)
                    {
                        if (TotalColumnFields == 0 && Rows == 0)
                        {
                            DTAggregateTable.ImportRow(DRowTableXLS);
                        }
                        break;
                    }
                    else
                    {
                        DTAggregateTable.ImportRow(DRowTableXLS);
                    }
                    Rows++;
                }

                //Itetrate through TableXLS from starting row carrying data to last row.
                for (int i = Rows; i < TableXLS.Rows.Count; i++)
                {
                    //If Row's ROWTYPE is of ICAggregate or Row is the last row then Add records into DTAggregateTable
                    if ((TableXLS.Rows[i][TablePresentation.ROWTYPE].ToString() == RowType.ICAggregate.ToString()) || (i == TableXLS.Rows.Count - 1))
                    {
                        //If there is any record in ICDataTable
                        if (RecordsInICDataTable == true || (TableXLS.Rows.Count - TotalColumnFields) == 1)
                        {
                            if (i == TableXLS.Rows.Count - 1 && RecordsInICDataTable == true)
                            {
                                ICDataTable.ImportRow(TableXLS.Rows[i]);
                            }

                            //check if there is only one record in the TableXLS.
                            if (i == TableXLS.Rows.Count - 1 && RecordsInICDataTable == false && ICDataTable.Rows.Count == 0)
                            {
                                ICDataTable.ImportRow(TableXLS.Rows[i]);
                            }

                            //If Indicator Classification is on.
                            if (this._SelectedIndicatorClassification)
                            {
                                //Get Distinct records for from TableXLS based on _AggregateFieldID column.
                                DTDistinctFields = GetDistinctAggregateFieldValues(ICDataTable, this._AggregateFieldID);

                                GrandSumDataValue = new double[TotalColumns];
                                TotalNumericValues = new double[TotalColumns];
                                TotalValuesCount = new double[TotalColumns];
                                GrandMinimumDataValue = new Nullable<double>[TotalColumns];
                                GrandMaximumDataValue = new Nullable<double>[TotalColumns];
                            }
                            //If Indicator Classification is off.
                            else
                            {
                                //Get Distinct records for from TableXLS based on _AggregateFieldID column.
                                //ICDataTable.ImportRow(TableXLS.Rows[i]);
                                DTDistinctFields = GetDistinctAggregateFieldValues(ICDataTable, this._AggregateFieldID);
                            }

                            //Insert a blank line after a group of IcAggregate.
                            if (DTDistinctFields.Rows.Count == 0)
                            {
                                DistinctFieldsFound = false;
                                DTAggregateTableRow = DTAggregateTable.NewRow();
                                DTAggregateTableRow[TablePresentation.ROWTYPE] = RowType.EmptyRow;
                                DTAggregateTableRow[TablePresentation.ROWINDEX] = DTAggregateTable.Rows.Count + 1;
                                DTAggregateTable.Rows.Add(DTAggregateTableRow);
                            }
                            else		//Insert the records into DTAggregateTable
                            {
                                if (((this._AggregateFieldID == Area.AreaName || this._AggregateFieldID == Area.AreaID) && string.IsNullOrEmpty(this._TableReport.AreaSelection.AreaLevels) == false) || ((this._AggregateFieldID == Area.AreaName || this._AggregateFieldID == Area.AreaID) && this._TemplateStyle.SubAggregateSetting.AggregateAreaByParent))
                                {
                                    //If report is subnational type
                                    if (!string.IsNullOrEmpty(this._TableReport.AreaSelection.AreaLevels))
                                    {
                                        //Initialize the Arrays.										
                                        GroupSumDataValue = new double[TotalColumns];
                                        GroupTotalNumericValues = new double[TotalColumns];
                                        GroupTotalValueCount = new double[TotalColumns];
                                        GroupMinimumDataValue = new Nullable<double>[TotalColumns];
                                        GroupMaximumDataValue = new Nullable<double>[TotalColumns];
                                        bool AggregateExecuted = false;
                                        foreach (DataRow DRowICDataTable in ICDataTable.Rows)
                                        {
                                            if (DRowICDataTable[TablePresentation.ROWTYPE].ToString() != RowType.EmptyRow.ToString())
                                            {
                                                DRowICDataTable[TablePresentation.ROWINDEX] = DTAggregateTable.Rows.Count + 1;
                                                if (DRowICDataTable[TablePresentation.ROWTYPE].ToString() != RowType.TableHeaderRow.ToString() && Convert.ToInt32(DRowICDataTable[TablePresentation.AREALEVEL]) < this.MaxAreaLevel)
                                                {
                                                    if (AggregateExecuted)
                                                    {
                                                        //Add the Values for respective Aggregate Function for each column in the last of each GroupFunction.
                                                        foreach (AggregateType AggType in this._LstAggregateFunction)
                                                        {
                                                            DTAggregateTableRow = DTAggregateTable.NewRow();
                                                            if (AggType == AggregateType.Sum)
                                                            {
                                                                DTAggregateTableRow[0] = LanguageStrings.Sum;
                                                            }
                                                            else if (AggType == AggregateType.Mean)
                                                            {
                                                                DTAggregateTableRow[0] = LanguageStrings.Average;
                                                            }
                                                            else if (AggType == AggregateType.Count)
                                                            {
                                                                DTAggregateTableRow[0] = LanguageStrings.Count;
                                                            }
                                                            else if (AggType == AggregateType.Minimum)
                                                            {
                                                                DTAggregateTableRow[0] = LanguageStrings.Minimum;
                                                            }
                                                            else if (AggType == AggregateType.Maximum)
                                                            {
                                                                DTAggregateTableRow[0] = LanguageStrings.Maximum;
                                                            }

                                                            for (Columns = 0; Columns < TotalColumns; Columns++)
                                                            {
                                                                if (Columns >= TotalRowFields)
                                                                {
                                                                    if (AggType == AggregateType.Sum)
                                                                    {
                                                                        DTAggregateTableRow[Columns] = GroupSumDataValue[Columns].ToString();
                                                                    }
                                                                    else if (AggType == AggregateType.Mean)
                                                                    {
                                                                        if (!string.IsNullOrEmpty(GroupTotalNumericValues[Columns].ToString().Trim()) && Convert.ToInt32(GroupTotalNumericValues[Columns]) > 0)
                                                                        {
                                                                            DTAggregateTableRow[Columns] = Strings.FormatNumber(GroupSumDataValue[Columns] / GroupTotalNumericValues[Columns], MeanDecPlaces, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault);
                                                                        }
                                                                        else
                                                                        {
                                                                            DTAggregateTableRow[Columns] = string.Empty;
                                                                        }
                                                                    }
                                                                    else if (AggType == AggregateType.Count)
                                                                    {
                                                                        DTAggregateTableRow[Columns] = GroupTotalValueCount[Columns].ToString();
                                                                    }
                                                                    else if (AggType == AggregateType.Minimum)
                                                                    {
                                                                        DTAggregateTableRow[Columns] = GroupMinimumDataValue[Columns].ToString();
                                                                    }
                                                                    else if (AggType == AggregateType.Maximum)
                                                                    {
                                                                        DTAggregateTableRow[Columns] = GroupMaximumDataValue[Columns].ToString();
                                                                    }
                                                                    Columns = Columns + this.FootnoteCommentColumnCount;
                                                                }
                                                            }
                                                            DTAggregateTableRow[TablePresentation.ROWTYPE] = RowType.SubAggregate;
                                                            DTAggregateTableRow[TablePresentation.ROWINDEX] = DTAggregateTable.Rows.Count + 1;
                                                            DTAggregateTable.Rows.Add(DTAggregateTableRow);
                                                        }
                                                        AggregateExecuted = false;
                                                        GroupSumDataValue = new double[TotalColumns];
                                                        GroupTotalNumericValues = new double[TotalColumns];
                                                        GroupTotalValueCount = new double[TotalColumns];
                                                        GroupMinimumDataValue = new Nullable<double>[TotalColumns];
                                                        GroupMaximumDataValue = new Nullable<double>[TotalColumns];
                                                    }
                                                    DTAggregateTable.ImportRow(DRowICDataTable);
                                                    continue;
                                                }
                                                else
                                                {
                                                    DTAggregateTable.ImportRow(DRowICDataTable);
                                                    if (DRowICDataTable[TablePresentation.ROWTYPE].ToString() != RowType.TableHeaderRow.ToString())
                                                    {
                                                        AggregateExecuted = true;
                                                        //Move through each column in the selected DataRow and perform aggregation operation.
                                                        for (Columns = 0; Columns < TotalColumns; Columns++)
                                                        {
                                                            //Leave the Rows Section in DataTable DTTableXLS.
                                                            if (Columns >= TotalRowFields)
                                                            {
                                                                //Take the DataValue if that is Numeric only.
                                                                if (DICommon.IsNumeric(DRowICDataTable[Columns].ToString()))
                                                                {
                                                                    GroupSumDataValue[Columns] = GroupSumDataValue[Columns] + Convert.ToDouble(DRowICDataTable[Columns].ToString());
                                                                    GroupTotalNumericValues[Columns] = GroupTotalNumericValues[Columns] + 1;
                                                                    GroupTotalValueCount[Columns] = GroupTotalValueCount[Columns] + 1;

                                                                    //Group Minimum
                                                                    if (GroupMinimumDataValue[Columns] == null)
                                                                    {
                                                                        if (DRowICDataTable[Columns].ToString() != string.Empty)
                                                                        {
                                                                            GroupMinimumDataValue[Columns] = DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture) < GroupMinimumDataValue[Columns])
                                                                        {
                                                                            GroupMinimumDataValue[Columns] = DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                                                                        }
                                                                    }

                                                                    //Group Maximum
                                                                    if (GroupMaximumDataValue[Columns] == null)
                                                                    {
                                                                        if (DRowICDataTable[Columns].ToString() != string.Empty)
                                                                        {
                                                                            GroupMaximumDataValue[Columns] = DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        if (DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture) > GroupMaximumDataValue[Columns])
                                                                        {
                                                                            GroupMaximumDataValue[Columns] = DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                                                                        }
                                                                    }
                                                                }
                                                                else if (DRowICDataTable[Columns].ToString().Trim() != "")
                                                                {
                                                                    GroupTotalValueCount[Columns] = GroupTotalValueCount[Columns] + 1;
                                                                }
                                                                Columns = Columns + this.FootnoteCommentColumnCount;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        if (AggregateExecuted)
                                        {
                                            //Add the Values for respective Aggregate Function for each column in the last of each GroupFunction.
                                            foreach (AggregateType AggType in this._LstAggregateFunction)
                                            {
                                                DTAggregateTableRow = DTAggregateTable.NewRow();
                                                if (AggType == AggregateType.Sum)
                                                {
                                                    DTAggregateTableRow[0] = LanguageStrings.Sum;
                                                }
                                                else if (AggType == AggregateType.Mean)
                                                {
                                                    DTAggregateTableRow[0] = LanguageStrings.Average;
                                                }
                                                else if (AggType == AggregateType.Count)
                                                {
                                                    DTAggregateTableRow[0] = LanguageStrings.Count;
                                                }
                                                else if (AggType == AggregateType.Minimum)
                                                {
                                                    DTAggregateTableRow[0] = LanguageStrings.Minimum;
                                                }
                                                else if (AggType == AggregateType.Maximum)
                                                {
                                                    DTAggregateTableRow[0] = LanguageStrings.Maximum;
                                                }

                                                for (Columns = 0; Columns < TotalColumns; Columns++)
                                                {
                                                    if (Columns >= TotalRowFields)
                                                    {
                                                        if (AggType == AggregateType.Sum)
                                                        {
                                                            DTAggregateTableRow[Columns] = GroupSumDataValue[Columns].ToString();
                                                        }
                                                        else if (AggType == AggregateType.Mean)
                                                        {
                                                            if (!string.IsNullOrEmpty(GroupTotalNumericValues[Columns].ToString().Trim()) && Convert.ToInt32(GroupTotalNumericValues[Columns]) > 0)
                                                            {
                                                                DTAggregateTableRow[Columns] = Strings.FormatNumber(GroupSumDataValue[Columns] / GroupTotalNumericValues[Columns], MeanDecPlaces, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault);
                                                            }
                                                            else
                                                            {
                                                                DTAggregateTableRow[Columns] = string.Empty;
                                                            }
                                                        }
                                                        else if (AggType == AggregateType.Count)
                                                        {
                                                            DTAggregateTableRow[Columns] = GroupTotalValueCount[Columns].ToString();
                                                        }
                                                        else if (AggType == AggregateType.Minimum)
                                                        {
                                                            DTAggregateTableRow[Columns] = GroupMinimumDataValue[Columns].ToString();
                                                        }
                                                        else if (AggType == AggregateType.Maximum)
                                                        {
                                                            DTAggregateTableRow[Columns] = GroupMaximumDataValue[Columns].ToString();
                                                        }
                                                        Columns = Columns + this.FootnoteCommentColumnCount;
                                                    }
                                                }
                                                DTAggregateTableRow[TablePresentation.ROWTYPE] = RowType.SubAggregate;
                                                DTAggregateTableRow[TablePresentation.ROWINDEX] = DTAggregateTable.Rows.Count + 1;
                                                DTAggregateTable.Rows.Add(DTAggregateTableRow);
                                            }
                                            AggregateExecuted = false;
                                        }
                                    }
                                    else	 //Report is not subnational
                                    {
                                        for (int Counter = 0; Counter < ALAreaParentNIds.Count; Counter++)
                                        {
                                            //Initialize the Arrays.
                                            GroupSumDataValue = new double[TotalColumns];
                                            GroupTotalNumericValues = new double[TotalColumns];
                                            GroupTotalValueCount = new double[TotalColumns];
                                            GroupMinimumDataValue = new Nullable<double>[TotalColumns];
                                            GroupMaximumDataValue = new Nullable<double>[TotalColumns];

                                            foreach (DataRow DRowDTDistinctFields in DTDistinctFields.Select(Area.AreaParentNId + "='" + ALAreaParentNIds[Counter].ToString() + "'"))
                                            {
                                                FilterString = this._AggregateFieldID.ToString() + " = '" + DICommon.RemoveQuotes(DRowDTDistinctFields[this._AggregateFieldID].ToString()) + "'";
                                                foreach (DataRow DRowICDataTable in ICDataTable.Select(FilterString))
                                                {
                                                    DRowICDataTable[TablePresentation.ROWINDEX] = DTAggregateTable.Rows.Count + 1;
                                                    DTAggregateTable.ImportRow(DRowICDataTable);

                                                    //Move through each column in the selected DataRow and perform aggregation operation.
                                                    for (Columns = 0; Columns < TotalColumns; Columns++)
                                                    {
                                                        //Leave the Rows Section in DataTable DTTableXLS.
                                                        if (Columns >= TotalRowFields)
                                                        {
                                                            //Take the DataValue if that is Numeric only.
                                                            if (DICommon.IsNumeric(DRowICDataTable[Columns].ToString()))
                                                            {
                                                                GroupSumDataValue[Columns] = GroupSumDataValue[Columns] + Convert.ToDouble(DRowICDataTable[Columns].ToString());
                                                                GrandSumDataValue[Columns] = GrandSumDataValue[Columns] + Convert.ToDouble(DRowICDataTable[Columns].ToString());
                                                                GroupTotalNumericValues[Columns] = GroupTotalNumericValues[Columns] + 1;
                                                                TotalNumericValues[Columns] = TotalNumericValues[Columns] + 1;
                                                                GroupTotalValueCount[Columns] = GroupTotalValueCount[Columns] + 1;
                                                                TotalValuesCount[Columns] = TotalValuesCount[Columns] + 1;

                                                                //Group Minimum
                                                                if (GroupMinimumDataValue[Columns] == null)
                                                                {
                                                                    if (DRowICDataTable[Columns].ToString() != string.Empty)
                                                                    {
                                                                        GroupMinimumDataValue[Columns] = DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture) < GroupMinimumDataValue[Columns])
                                                                    {
                                                                        GroupMinimumDataValue[Columns] = DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                                                                    }
                                                                }
                                                                //Grand Minimum
                                                                if (GrandMinimumDataValue[Columns] == null)
                                                                {
                                                                    if (DRowICDataTable[Columns].ToString() != string.Empty)
                                                                    {
                                                                        GrandMinimumDataValue[Columns] = DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture) < GrandMinimumDataValue[Columns])
                                                                    {
                                                                        GrandMinimumDataValue[Columns] = DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                                                                    }
                                                                }

                                                                //Group Maximum
                                                                if (GroupMaximumDataValue[Columns] == null)
                                                                {
                                                                    if (DRowICDataTable[Columns].ToString() != string.Empty)
                                                                    {
                                                                        GroupMaximumDataValue[Columns] = DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture) > GroupMaximumDataValue[Columns])
                                                                    {
                                                                        GroupMaximumDataValue[Columns] = DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                                                                    }
                                                                }

                                                                //Grand Maximum
                                                                if (GrandMaximumDataValue[Columns] == null)
                                                                {
                                                                    if (DRowICDataTable[Columns].ToString() != string.Empty)
                                                                    {
                                                                        GrandMaximumDataValue[Columns] = DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture) > GrandMaximumDataValue[Columns])
                                                                    {
                                                                        GrandMaximumDataValue[Columns] = DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                                                                    }
                                                                }
                                                            }
                                                            else if (DRowICDataTable[Columns].ToString().Trim() != "")
                                                            {
                                                                GroupTotalValueCount[Columns] = GroupTotalValueCount[Columns] + 1;
                                                                TotalValuesCount[Columns] = TotalValuesCount[Columns] + 1;
                                                            }
                                                            Columns = Columns + this.FootnoteCommentColumnCount;
                                                        }
                                                    }
                                                }
                                            }
                                            //Add the Values for respective Aggregate Function for each column in the last of each GroupFunction.
                                            foreach (AggregateType AggType in this._LstAggregateFunction)
                                            {
                                                DTAggregateTableRow = DTAggregateTable.NewRow();
                                                if (AggType == AggregateType.Sum)
                                                {
                                                    DTAggregateTableRow[0] = LanguageStrings.Sum;
                                                }
                                                else if (AggType == AggregateType.Mean)
                                                {
                                                    DTAggregateTableRow[0] = LanguageStrings.Average;
                                                }
                                                else if (AggType == AggregateType.Count)
                                                {
                                                    DTAggregateTableRow[0] = LanguageStrings.Count;
                                                }
                                                else if (AggType == AggregateType.Minimum)
                                                {
                                                    DTAggregateTableRow[0] = LanguageStrings.Minimum;
                                                }
                                                else if (AggType == AggregateType.Maximum)
                                                {
                                                    DTAggregateTableRow[0] = LanguageStrings.Maximum;
                                                }


                                                for (Columns = 0; Columns < TotalColumns; Columns++)
                                                {
                                                    if (Columns >= TotalRowFields)
                                                    {
                                                        if (AggType == AggregateType.Sum)
                                                        {
                                                            DTAggregateTableRow[Columns] = GroupSumDataValue[Columns].ToString();
                                                        }
                                                        else if (AggType == AggregateType.Mean)
                                                        {
                                                            if (!string.IsNullOrEmpty(GroupTotalNumericValues[Columns].ToString().Trim()) && Convert.ToInt32(GroupTotalNumericValues[Columns]) > 0)
                                                            {
                                                                DTAggregateTableRow[Columns] = Strings.FormatNumber(GroupSumDataValue[Columns] / GroupTotalNumericValues[Columns], MeanDecPlaces, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault);
                                                            }
                                                            else
                                                            {
                                                                DTAggregateTableRow[Columns] = string.Empty;
                                                            }
                                                        }
                                                        else if (AggType == AggregateType.Count)
                                                        {
                                                            DTAggregateTableRow[Columns] = GroupTotalValueCount[Columns].ToString();
                                                        }
                                                        else if (AggType == AggregateType.Minimum)
                                                        {
                                                            DTAggregateTableRow[Columns] = GroupMinimumDataValue[Columns].ToString();
                                                        }
                                                        else if (AggType == AggregateType.Maximum)
                                                        {
                                                            DTAggregateTableRow[Columns] = GroupMaximumDataValue[Columns].ToString();
                                                        }
                                                        Columns = Columns + this.FootnoteCommentColumnCount;
                                                    }
                                                }
                                                DTAggregateTableRow[TablePresentation.ROWTYPE] = RowType.SubAggregate;
                                                DTAggregateTableRow[TablePresentation.ROWINDEX] = DTAggregateTable.Rows.Count + 1;
                                                DTAggregateTable.Rows.Add(DTAggregateTableRow);
                                            }

                                        }
                                    }

                                }
                                else
                                {
                                    foreach (DataRow DRowDTDistinctFields in DTDistinctFields.Rows)
                                    {

                                        //Initialize the Arrays.
                                        bool RecordExists = false;
                                        GroupSumDataValue = new double[TotalColumns];
                                        GroupTotalNumericValues = new double[TotalColumns];
                                        GroupTotalValueCount = new double[TotalColumns];
                                        GroupMinimumDataValue = new Nullable<double>[TotalColumns];
                                        GroupMaximumDataValue = new Nullable<double>[TotalColumns];

                                        FilterString = this.AddSlash(this._AggregateFieldID.ToString()) + " = '" + DICommon.RemoveQuotes(DRowDTDistinctFields[this._AggregateFieldID].ToString()) + "'";

                                        foreach (DataRow DRowICDataTable in ICDataTable.Select(FilterString))
                                        {
                                            RecordExists = true;
                                            DRowICDataTable[TablePresentation.ROWINDEX] = DTAggregateTable.Rows.Count + 1;
                                            DTAggregateTable.ImportRow(DRowICDataTable);

                                            //Move through each column in the selected DataRow and perform aggregation operation.
                                            for (Columns = 0; Columns < TotalColumns; Columns++)
                                            {
                                                //Leave the Rows Section in DataTable DTTableXLS.
                                                if (Columns >= TotalRowFields)
                                                {
                                                    //Take the DataValue if that is Numeric only.
                                                    if (DICommon.IsNumeric(DRowICDataTable[Columns].ToString()))
                                                    {
                                                        GroupSumDataValue[Columns] = GroupSumDataValue[Columns] + Convert.ToDouble(DRowICDataTable[Columns].ToString());
                                                        GrandSumDataValue[Columns] = GrandSumDataValue[Columns] + Convert.ToDouble(DRowICDataTable[Columns].ToString());
                                                        GroupTotalNumericValues[Columns] = GroupTotalNumericValues[Columns] + 1;
                                                        TotalNumericValues[Columns] = TotalNumericValues[Columns] + 1;
                                                        GroupTotalValueCount[Columns] = GroupTotalValueCount[Columns] + 1;
                                                        TotalValuesCount[Columns] = TotalValuesCount[Columns] + 1;
                                                        //Group Minimum
                                                        if (GroupMinimumDataValue[Columns] == null)
                                                        {
                                                            if (DRowICDataTable[Columns].ToString() != string.Empty)
                                                            {
                                                                GroupMinimumDataValue[Columns] = DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture) < GroupMinimumDataValue[Columns])
                                                            {
                                                                GroupMinimumDataValue[Columns] = DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                                                            }
                                                        }
                                                        //Grand Minimum
                                                        if (GrandMinimumDataValue[Columns] == null)
                                                        {
                                                            if (DRowICDataTable[Columns].ToString() != string.Empty)
                                                            {
                                                                GrandMinimumDataValue[Columns] = DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture) < GrandMinimumDataValue[Columns])
                                                            {
                                                                GrandMinimumDataValue[Columns] = DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                                                            }
                                                        }

                                                        //Group Maximum
                                                        if (GroupMaximumDataValue[Columns] == null)
                                                        {
                                                            if (DRowICDataTable[Columns].ToString() != string.Empty)
                                                            {
                                                                GroupMaximumDataValue[Columns] = DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture) > GroupMaximumDataValue[Columns])
                                                            {
                                                                GroupMaximumDataValue[Columns] = DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                                                            }
                                                        }

                                                        //Grand Maximum
                                                        if (GrandMaximumDataValue[Columns] == null)
                                                        {
                                                            if (DRowICDataTable[Columns].ToString() != string.Empty)
                                                            {
                                                                GrandMaximumDataValue[Columns] = DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture) > GrandMaximumDataValue[Columns])
                                                            {
                                                                GrandMaximumDataValue[Columns] = DICommon.ParseStringToDouble(DRowICDataTable[Columns].ToString(), System.Globalization.CultureInfo.CurrentCulture);
                                                            }
                                                        }
                                                    }
                                                    else if (DRowICDataTable[Columns].ToString().Trim() != "")
                                                    {
                                                        GroupTotalValueCount[Columns] = GroupTotalValueCount[Columns] + 1;
                                                        TotalValuesCount[Columns] = TotalValuesCount[Columns] + 1;
                                                    }
                                                    Columns = Columns + this.FootnoteCommentColumnCount;
                                                }
                                            }
                                        }
                                        //Add the Values for respective Aggregate Function for each column in the last of each GroupFunction.

                                        if (RecordExists)
                                        {
                                            foreach (AggregateType AggType in this._LstAggregateFunction)
                                            {
                                                DTAggregateTableRow = DTAggregateTable.NewRow();
                                                if (AggType == AggregateType.Sum)
                                                {
                                                    DTAggregateTableRow[0] = LanguageStrings.Sum;
                                                }
                                                else if (AggType == AggregateType.Mean)
                                                {
                                                    DTAggregateTableRow[0] = LanguageStrings.Average;
                                                }
                                                else if (AggType == AggregateType.Count)
                                                {
                                                    DTAggregateTableRow[0] = LanguageStrings.Count;
                                                }
                                                else if (AggType == AggregateType.Minimum)
                                                {
                                                    DTAggregateTableRow[0] = LanguageStrings.Minimum;
                                                }
                                                else if (AggType == AggregateType.Maximum)
                                                {
                                                    DTAggregateTableRow[0] = LanguageStrings.Maximum;
                                                }

                                                for (Columns = 0; Columns < TotalColumns; Columns++)
                                                {
                                                    if (Columns >= TotalRowFields)
                                                    {
                                                        if (AggType == AggregateType.Sum)
                                                        {
                                                            DTAggregateTableRow[Columns] = GroupSumDataValue[Columns].ToString();
                                                        }
                                                        else if (AggType == AggregateType.Mean)
                                                        {
                                                            if (!string.IsNullOrEmpty(GroupTotalNumericValues[Columns].ToString().Trim()) && Convert.ToInt32(GroupTotalNumericValues[Columns]) > 0)
                                                            {
                                                                DTAggregateTableRow[Columns] = Strings.FormatNumber(GroupSumDataValue[Columns] / GroupTotalNumericValues[Columns], MeanDecPlaces, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault);
                                                            }
                                                            else
                                                            {
                                                                DTAggregateTableRow[Columns] = string.Empty;
                                                            }
                                                        }
                                                        else if (AggType == AggregateType.Count)
                                                        {
                                                            DTAggregateTableRow[Columns] = GroupTotalValueCount[Columns].ToString();
                                                        }
                                                        else if (AggType == AggregateType.Minimum)
                                                        {
                                                            DTAggregateTableRow[Columns] = GroupMinimumDataValue[Columns].ToString();
                                                        }
                                                        else if (AggType == AggregateType.Maximum)
                                                        {
                                                            DTAggregateTableRow[Columns] = GroupMaximumDataValue[Columns].ToString();
                                                        }
                                                        Columns = Columns + this.FootnoteCommentColumnCount;
                                                    }
                                                }
                                                DTAggregateTableRow[TablePresentation.ROWTYPE] = RowType.SubAggregate;
                                                DTAggregateTableRow[TablePresentation.ROWINDEX] = DTAggregateTable.Rows.Count + 1;
                                                DTAggregateTable.Rows.Add(DTAggregateTableRow);
                                            }
                                        }


                                    }
                                }

                                if (string.IsNullOrEmpty(this._TableReport.AreaSelection.AreaLevels))
                                {
                                    //Add a Row containg GrandValues to the last of DTAggregateTable
                                    foreach (AggregateType AggType in this._LstAggregateFunction)
                                    {
                                        DTAggregateTableRow = DTAggregateTable.NewRow();
                                        if (AggType == AggregateType.Sum)
                                        {
                                            DTAggregateTableRow[0] = LanguageStrings.GrandSum;
                                        }
                                        else if (AggType == AggregateType.Mean)
                                        {
                                            DTAggregateTableRow[0] = LanguageStrings.GrandAverage;
                                        }
                                        else if (AggType == AggregateType.Count)
                                        {
                                            DTAggregateTableRow[0] = LanguageStrings.Count;
                                        }
                                        else if (AggType == AggregateType.Minimum)
                                        {
                                            DTAggregateTableRow[0] = LanguageStrings.Minimum;
                                        }
                                        else if (AggType == AggregateType.Maximum)
                                        {
                                            DTAggregateTableRow[0] = LanguageStrings.Maximum;
                                        }

                                        for (Columns = 0; Columns < TotalColumns; Columns++)
                                        {
                                            if (Columns >= TotalRowFields)
                                            {
                                                if (AggType == AggregateType.Sum)
                                                {
                                                    DTAggregateTableRow[Columns] = GrandSumDataValue[Columns].ToString();
                                                }
                                                else if (AggType == AggregateType.Mean)
                                                {
                                                    if (!string.IsNullOrEmpty(TotalNumericValues[Columns].ToString().Trim()) && Convert.ToInt32(TotalNumericValues[Columns]) > 0)
                                                    {
                                                        DTAggregateTableRow[Columns] = Strings.FormatNumber(GrandSumDataValue[Columns] / TotalNumericValues[Columns], MeanDecPlaces, TriState.UseDefault, TriState.UseDefault, TriState.UseDefault);
                                                    }
                                                    else
                                                    {
                                                        DTAggregateTableRow[Columns] = string.Empty;
                                                    }
                                                }
                                                else if (AggType == AggregateType.Count)
                                                {
                                                    DTAggregateTableRow[Columns] = TotalValuesCount[Columns].ToString();
                                                }
                                                else if (AggType == AggregateType.Minimum)
                                                {
                                                    DTAggregateTableRow[Columns] = GrandMinimumDataValue[Columns].ToString();
                                                }
                                                else if (AggType == AggregateType.Maximum)
                                                {
                                                    DTAggregateTableRow[Columns] = GrandMaximumDataValue[Columns].ToString();
                                                }
                                                Columns = Columns + this.FootnoteCommentColumnCount;
                                            }
                                        }

                                        DTAggregateTableRow[TablePresentation.ROWTYPE] = RowType.GroupAggregate;
                                        DTAggregateTableRow[TablePresentation.ROWINDEX] = DTAggregateTable.Rows.Count + 1;
                                        DTAggregateTable.Rows.Add(DTAggregateTableRow);
                                    }
                                }
                            }
                        }
                        if (this._SelectedIndicatorClassification)
                        {
                            //Insert a row of type ICAggregate.
                            if (RecordsInICDataTable == true && DistinctFieldsFound == true)
                            {
                                //DTAggregateTableRow = DTAggregateTable.NewRow();
                                //DTAggregateTableRow[TablePresentation.ROWTYPE] = RowType.EmptyRow;
                                //DTAggregateTableRow[TablePresentation.ROWINDEX] = DTAggregateTable.Rows.Count + 1;
                                //DTAggregateTable.Rows.Add(DTAggregateTableRow);
                            }
                            if (i != TableXLS.Rows.Count - 1)
                            {
                                TableXLS.Rows[i][TablePresentation.ROWINDEX] = DTAggregateTable.Rows.Count + 1;
                                DTAggregateTable.ImportRow(TableXLS.Rows[i]);
                                DistinctFieldsFound = true;
                            }
                            //Import the row if table has only one row and that is of type ICAgregate
                            if ((i == TableXLS.Rows.Count - 1) && (DTAggregateTable.Rows.Count == this._Fields.Columns.Count + 2))     //1 row for blank row and second for 
                            {
                                TableXLS.Rows[i][TablePresentation.ROWINDEX] = DTAggregateTable.Rows.Count + 1;
                                DTAggregateTable.ImportRow(TableXLS.Rows[i]);
                            }
                        }
                        ICDataTable.Clear();
                        RecordsInICDataTable = false;
                    }
                    else
                    {
                        //Insert a DataRow.
                        ICDataTable.ImportRow(TableXLS.Rows[i]);
                        RecordsInICDataTable = true;
                    }
                }
                RetVal = DTAggregateTable;
            }
            return RetVal;
        }

        /// <summary>
        /// Generates distinct records for Aggregate Field.
        /// </summary>
        /// <param name="DTTableXLS">DataTable</param>
        /// <param name="AggregateFieldID">String Aggregate Field ID.</param>
        /// <returns>DataTable</returns>
        private DataTable GetDistinctAggregateFieldValues(DataTable DTTableXLS, string AggregateFieldID)
        {
            if (ALAreaParentNIds != null)
            {
                ALAreaParentNIds.Clear();
            }
            else
            {
                ALAreaParentNIds = new ArrayList();
            }

            DataTable RetVal = new DataTable();
            DataRow RetValRow;							// DataRow containing record for RetVal.			
            string ColumnNameCaption = string.Empty;
            DataTable DTPresentationData = this.PresentationData.ToTable();
            DataRow[] AreaParentNid;

            string FilterExpression = string.Empty;

            RetVal.Columns.Add(AggregateFieldID);		//Add Column to RetVal.
            if (AggregateFieldID == Area.AreaName || AggregateFieldID == Area.AreaID)
            {
                RetVal.Columns.Add(Area.AreaParentNId);
            }
            ColumnNameCaption = this._Fields.Rows.GetCaption(AggregateFieldID);  //Do Language handeling for AggregateFieldID.

            //Generate Unique record.
            foreach (DataRow DRowDTTableXLS in DTTableXLS.Rows)
            {
                //Do not take Blank Values				
                if ((DRowDTTableXLS[AggregateFieldID].ToString().Trim() != string.Empty) || ((DRowDTTableXLS[AggregateFieldID].ToString().Trim() == string.Empty) && DRowDTTableXLS[ROWTYPE].ToString() == RowType.DataRow.ToString()))
                {
                    FilterExpression = this.AddSlash(AggregateFieldID) + "='" + DICommon.RemoveQuotes(DRowDTTableXLS[AggregateFieldID].ToString()) + "'";
                    if (RetVal.Select(FilterExpression).Length == 0)
                    {
                        //Do not take the value if that value is Column Name
                        if (ColumnNameCaption != DRowDTTableXLS[AggregateFieldID].ToString().Trim())
                        {
                            RetValRow = RetVal.NewRow();
                            RetValRow[AggregateFieldID] = DRowDTTableXLS[AggregateFieldID].ToString();
                            if (AggregateFieldID == Area.AreaName)
                            {
                                AreaParentNid = DTPresentationData.Select(Area.AreaName + "='" + DICommon.RemoveQuotes(DRowDTTableXLS[AggregateFieldID].ToString()) + "'");
                                if (AreaParentNid.Length > 0)
                                {
                                    RetValRow[Area.AreaParentNId] = AreaParentNid[0][Area.AreaParentNId];
                                    if (!ALAreaParentNIds.Contains(AreaParentNid[0][Area.AreaParentNId].ToString()))
                                    {
                                        ALAreaParentNIds.Add(AreaParentNid[0][Area.AreaParentNId].ToString());
                                    }
                                }
                            }
                            if (AggregateFieldID == Area.AreaID)
                            {
                                AreaParentNid = DTPresentationData.Select(Area.AreaID + "='" + DRowDTTableXLS[AggregateFieldID].ToString() + "'");
                                if (AreaParentNid.Length > 0)
                                {
                                    RetValRow[Area.AreaParentNId] = AreaParentNid[0][Area.AreaParentNId];
                                    if (!ALAreaParentNIds.Contains(AreaParentNid[0][Area.AreaParentNId].ToString()))
                                    {
                                        ALAreaParentNIds.Add(AreaParentNid[0][Area.AreaParentNId].ToString());
                                    }
                                }
                            }
                            RetVal.Rows.Add(RetValRow);
                        }
                    }
                }
            }
            DTPresentationData.Dispose();
            //Sort the datatable based on AreaName.
            if (AggregateFieldID == Area.AreaName || AggregateFieldID == Area.AreaID)
            {
                DataView DvRetVal = RetVal.DefaultView;
                DvRetVal.Sort = Area.AreaParentNId;
                RetVal = DvRetVal.ToTable();
            }
            return RetVal;
        }

        /// <summary>
        /// Configure the excel worksheet.
        /// </summary>
        /// <param name="presentationXLSFilePath"></param>
        /// <returns></returns>
        private DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper.DIExcel ConfigureExcelSettings(string presentationXLSFilePath, string TableSheetName)
        {
            DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper.DIExcel Retval = null;
            try
            {
                int sheetIndex = 0;
                if (File.Exists(presentationXLSFilePath) && PresentationType == Presentation.PresentationType.Table)
                {
                    File.Delete(presentationXLSFilePath);
                }

                //Create XLS WorkSheets
                if (PresentationType == Presentation.PresentationType.Table || PresentationType == Presentation.PresentationType.FrequencyTable)
                {
                    Retval = new DIExcel();
                    Retval.SaveAs(presentationXLSFilePath);
                    Retval.RenameWorkSheet("Sheet1", TableSheetName);	//Rename it as one worksheet is already available in thw workbook.		
                    Retval.CreateWorksheet(LanguageStrings.Data);
                    Retval.CreateWorksheet(LanguageStrings.Source);
                    Retval.CreateWorksheet(Presentation.KEYWORD_WORKSHEET_NAME);
                }
                else
                {
                    Retval = new DIExcel(presentationXLSFilePath, System.Globalization.CultureInfo.CurrentCulture);
                    Retval.RenameWorkSheet("Table", LanguageStrings.Table);	//Rename it as one worksheet is already available in thw workbook.		
                    Retval.RenameWorkSheet("Data", LanguageStrings.Data);
                    Retval.RenameWorkSheet("Source", LanguageStrings.Source);
                    Retval.RenameWorkSheet("Keywords", Presentation.KEYWORD_WORKSHEET_NAME);
                }




                // save excel file
                Retval.Save();

                //Get the integer sheetindex of worksheet 
                sheetIndex = 0;

                //Activate the desired Worksheet.
                //ExcelFile.ActivateSheet = sheetIndex;
                Retval.ActivateSheet(sheetIndex);

                //hide grid lines
                Retval.ShowWorkSheetGridLine(sheetIndex, false);
                Retval.ShowWorkSheetGridLine(Retval.GetSheetIndex(LanguageStrings.Data), false);
                Retval.ShowWorkSheetGridLine(Retval.GetSheetIndex(LanguageStrings.Source), false);
                Retval.ShowWorkSheetGridLine(Retval.GetSheetIndex(Presentation.KEYWORD_WORKSHEET_NAME), false);
            }
            catch (Exception)
            {

                throw;
            }
            return Retval;
        }

        /// <summary>
        /// Generates the .XLS File and Saves in the Provided Location.
        /// </summary>
        /// <param name="TableXLS">DataTable that holds the actual XLS output</param>
        /// <param name="TableFootNote">DataTable Containing FootNotes</param>
        /// <param name="DTComments">DataTable Containing Commets</param>
        /// <returns>XLS File Name</returns>
        private string GeneratesXLSFile(DataTable TableXLS, DataTable TableFootNote, DataTable DTComments, string PresentationOutputFolder, string presentationFileName)
        {
            //Remove Footnote Nid from DTFootNote as it is not to be shown in worksheet
            if (this.DTFootNote != null)
            {
                this.DTFootNote.Columns.Remove(FootNotes.FootNoteNId);
            }
            //Remove NoteIndex and NotesNid from DTcomments as they are not to be shown in worksheet.
            if (this.DTComments != null)
            {
                this.DTComments.Columns.Remove("NoteIndex");
                this.DTComments.Columns.Remove(Notes.NotesNId);
            }

            // Remove the footnotes column, if the FottnoteShow is false.
            if (!this._TemplateStyle.Footnotes.Show)
            {
                this.DTDataSheet.Columns.Remove(FootNotes.FootNote);
            }


            if (!string.IsNullOrEmpty(this._TableReport.AreaSelection.AreaLevels))
            {
                DataTable DTTableXLSTemp = TableXLS.Clone();
                ArrayList ALAreaLevel = new ArrayList();
                int RowIndex = 1;
                DataRow Dr;
                string LastAreaLevel = string.Empty;
                //this._TableReport.LevelFormat[0].
                string[] ALevel = DICommon.SplitString(this._TableReport.AreaSelection.SecondryAreaLevels, ",");
                for (int i = 0; i < ALevel.Length; i++)
                {
                    ALAreaLevel.Add(ALevel[i].ToString());
                }
                for (int RowCount = 0; RowCount < TableXLS.Rows.Count; RowCount++)
                {
                    if (RowCount == TableXLS.Rows.Count - 1)
                    {
                        if (TableXLS.Rows[RowCount][ROWTYPE].ToString() != RowType.DataRow.ToString())
                        {
                            TableXLS.Rows[RowCount][ROWINDEX] = RowIndex.ToString();
                            DTTableXLSTemp.ImportRow(TableXLS.Rows[RowCount]);
                            RowIndex++;
                        }
                        else
                        {
                            if (this._TableReport.LevelFormat[TableXLS.Rows[RowCount]["AreaLevel"].ToString()].ShowDataValues)
                            {
                                TableXLS.Rows[RowCount][ROWINDEX] = RowIndex.ToString();
                                DTTableXLSTemp.ImportRow(TableXLS.Rows[RowCount]);
                                RowIndex++;
                            }
                            else
                            {
                                if (Convert.ToInt32(TableXLS.Rows[RowCount]["AreaLevel"].ToString()) >= Convert.ToInt32(LastAreaLevel))
                                {
                                    TableXLS.Rows[RowCount][ROWINDEX] = RowIndex.ToString();
                                    DTTableXLSTemp.ImportRow(TableXLS.Rows[RowCount]);
                                    RowIndex++;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (TableXLS.Rows[RowCount][ROWTYPE].ToString() != RowType.DataRow.ToString())
                        {
                            TableXLS.Rows[RowCount][ROWINDEX] = RowIndex.ToString();
                            DTTableXLSTemp.ImportRow(TableXLS.Rows[RowCount]);
                            RowIndex++;
                        }
                        else if (ALAreaLevel.Contains(TableXLS.Rows[RowCount]["AreaLevel"].ToString()))
                        {
                            if (this._TableReport.LevelFormat[TableXLS.Rows[RowCount]["AreaLevel"].ToString()].ShowDataValues)
                            {
                                TableXLS.Rows[RowCount][ROWINDEX] = RowIndex.ToString();
                                DTTableXLSTemp.ImportRow(TableXLS.Rows[RowCount]);
                                RowIndex++;
                                LastAreaLevel = TableXLS.Rows[RowCount]["AreaLevel"].ToString();
                            }
                            else
                            {
                                if (TableXLS.Rows[RowCount + 1][ROWTYPE].ToString() == RowType.DataRow.ToString())
                                {
                                    if (Convert.ToInt32(TableXLS.Rows[RowCount]["AreaLevel"].ToString()) < Convert.ToInt32(TableXLS.Rows[RowCount + 1]["AreaLevel"].ToString()) || TableXLS.Rows[RowCount]["AreaLevel"].ToString() == ALAreaLevel[ALAreaLevel.Count - 1].ToString())
                                    {
                                        TableXLS.Rows[RowCount][ROWINDEX] = RowIndex.ToString();
                                        DTTableXLSTemp.ImportRow(TableXLS.Rows[RowCount]);
                                        RowIndex++;
                                        LastAreaLevel = TableXLS.Rows[RowCount]["AreaLevel"].ToString();
                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32(TableXLS.Rows[RowCount]["AreaLevel"].ToString()) >= Convert.ToInt32(LastAreaLevel))
                                    {
                                        TableXLS.Rows[RowCount][ROWINDEX] = RowIndex.ToString();
                                        DTTableXLSTemp.ImportRow(TableXLS.Rows[RowCount]);
                                        RowIndex++;
                                    }
                                }
                            }
                        }
                    }
                }


                //foreach (DataRow DrowTableXLS in TableXLS.Rows)
                //{
                //    if (ALAreaLevel.Contains(DrowTableXLS["AreaLevel"].ToString()) || DrowTableXLS[ROWTYPE].ToString()==RowType.TableHeaderRow.ToString())
                //    {
                //        DrowTableXLS[ROWINDEX] = RowIndex.ToString();
                //        DTTableXLSTemp.ImportRow(DrowTableXLS);
                //        RowIndex++;
                //    }
                //}
                TableXLS = DTTableXLSTemp;
            }

            //Logic to Change the TableXLS by introducing rows for SuppressDuplicate in it . Don't do it for Graph Wizard.
            #region "-- Suppress Duplicate logic --"


            //This will be suppress duplicate 
            if (this._IsSuppressDuplicateRows && this._PresentationType != Presentation.PresentationType.Graph)
            {
                DataTable TableXLSClone = TableXLS.Clone();
                this.NewRowIndex = 1;
                DataRow TableXLSCloneRow;
                string BaseHeading = string.Empty;
                string Heading = string.Empty;
                string SecondLevelHeading = string.Empty;
                string SecondLevelBaseHeading = string.Empty;
                this.SuppressColumnCount = 0;
                ArrayList ALHeading = new ArrayList();
                ArrayList ALBaseHeading = new ArrayList();
                int SubtractFromRows = 1;
                if (TableXLS.Columns.Contains(IndicatorClassifications.ICName) && this._Fields.Rows.Count > 1 && TableXLS.Columns[0].ColumnName != IndicatorClassifications.ICName && this._MoveSourceToLast == true && this.TableXLS.Columns[this._Fields.Rows.Count - 1].ColumnName == IndicatorClassifications.ICName)
                {
                    SubtractFromRows = 2;
                }

                foreach (DataRow DrowTableXLS in TableXLS.Rows)
                {
                    if (DrowTableXLS[TablePresentation.ROWTYPE].ToString() != RowType.TableHeaderRow.ToString())
                    {
                        //Get the cell value of first column
                        Heading = DrowTableXLS[0].ToString();
                        //Compare it with BaseHeadin( contains cell value of first column for previous row when header get found)
                        if (Heading != BaseHeading)
                        {
                            //Execute when value is DataValue only( not Aggregate or IC value)
                            if (ALSuppressColValues.Contains(Heading.Trim()))
                            {
                                if (DrowTableXLS[1].ToString() == string.Empty && this._Fields.Rows.Count > 1)
                                {
                                    DrowTableXLS[TablePresentation.ROWTYPE] = RowType.SupressRow.ToString();
                                }
                                else
                                {
                                    TableXLSCloneRow = TableXLSClone.NewRow();
                                    this.SuppressColumnCount++;
                                    TableXLSCloneRow[0] = DrowTableXLS[0];
                                    if (this._Fields.Rows[Timeperiods.TimePeriod] != null)
                                    {
                                        if (this._Fields.Rows[Timeperiods.TimePeriod].FieldIndex == 0)
                                        {
                                            if (this._PresentationOutputType == PresentationOutputType.MHT)
                                            {
                                                TableXLSCloneRow[0] = DrowTableXLS[0];
                                            }
                                            else
                                            {
                                                TableXLSCloneRow[0] = "'" + DrowTableXLS[0];
                                            }
                                        }
                                    }
                                    TableXLSCloneRow[TablePresentation.AREALEVEL] = "-1";
                                    if (this._Fields.Rows[Area.AreaName] != null)
                                    {
                                        if (this._Fields.Rows[Area.AreaName].FieldIndex == 0)
                                        {
                                            TableXLSCloneRow[TablePresentation.AREALEVEL] = DrowTableXLS[TablePresentation.AREALEVEL];
                                        }
                                    }

                                    TableXLSCloneRow[TablePresentation.ROWTYPE] = RowType.SupressRow.ToString();
                                    TableXLSCloneRow[TablePresentation.ROWINDEX] = this.NewRowIndex.ToString();
                                    this.NewRowIndex++;
                                    TableXLSClone.Rows.Add(TableXLSCloneRow);
                                    if (this._Fields.Rows.Count > 2)
                                    {
                                        if (DrowTableXLS[1].ToString() != string.Empty)
                                        {
                                            ALHeading.Clear();
                                            ALBaseHeading.Clear();
                                            for (int ColCount = 1; ColCount < this._Fields.Rows.Count - SubtractFromRows; ColCount++)
                                            {
                                                ALHeading.Insert(ColCount - 1, DrowTableXLS[ColCount].ToString());
                                                ALBaseHeading.Insert(ColCount - 1, string.Empty);
                                                TableXLSCloneRow = TableXLSClone.NewRow();
                                                TableXLSCloneRow[ColCount] = DrowTableXLS[ColCount];
                                                if (this._Fields.Rows[Timeperiods.TimePeriod] != null)
                                                {
                                                    if (this._Fields.Rows[Timeperiods.TimePeriod].FieldIndex == ColCount)
                                                    {
                                                        if (this._PresentationOutputType == PresentationOutputType.MHT)
                                                        {
                                                            TableXLSCloneRow[ColCount] = DrowTableXLS[ColCount];
                                                        }
                                                        else
                                                        {
                                                            TableXLSCloneRow[ColCount] = "'" + DrowTableXLS[ColCount];
                                                        }
                                                    }
                                                }

                                                TableXLSCloneRow[TablePresentation.AREALEVEL] = "-1";
                                                TableXLSCloneRow[TablePresentation.ROWTYPE] = RowType.SupressRow.ToString();
                                                TableXLSCloneRow[TablePresentation.ROWINDEX] = this.NewRowIndex.ToString();
                                                this.NewRowIndex++;
                                                TableXLSClone.Rows.Add(TableXLSCloneRow);
                                                ALBaseHeading[ColCount - 1] = ALHeading[ColCount - 1];
                                            }
                                        }
                                    }
                                }
                            }
                            BaseHeading = Heading;
                        }
                        if (this._Fields.Rows.Count > 2 && ALHeading.Count > 0)
                        {
                            this.SetSuppressForInnerLevels(1, ALHeading, ALBaseHeading, DrowTableXLS, TableXLSClone, SubtractFromRows);
                        }

                        DrowTableXLS[TablePresentation.ROWINDEX] = this.NewRowIndex.ToString();
                        TableXLSClone.ImportRow(DrowTableXLS);

                    }
                    else
                    {
                        DrowTableXLS[TablePresentation.ROWINDEX] = this.NewRowIndex.ToString();
                        TableXLSClone.ImportRow(DrowTableXLS);
                    }

                    this.NewRowIndex++;

                }
                TableXLS = TableXLSClone;
            }

            #endregion

            string RetVal = string.Empty;
            int Columns = 0;
            int Margin = 0;
            int TotalTableXLSROWS = TableXLS.Rows.Count;
            int TotalTableXLSColumns = TableXLS.Columns.Count - (this.ExtraColumnCount + this.MaxMinAreaLevelDiff);	//Remove 2 columns as they need not to be shown to the user (i.e. Rowtype and Rownumber).
            //int TotalColumnFields = (this._ColumnArrangementTable.Columns.Count - 2) / 2;
            int TotalColumnFields = this._Fields.Columns.Count;
            int TotalRowFields = this._Fields.Rows.Count;
            string[,] TableFootNoteArray = new string[0, 0];
            string[,] TableCommentsArray = new string[0, 0];

            //Generate name of the Excel File
            if (presentationFileName.Length > 0)
            {
                RetVal = presentationFileName;
            }
            else
            {
                if (this.PresentationOutputType == PresentationOutputType.ExcellSheet)
                {
                    RetVal = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day + DateTime.Now.TimeOfDay.Hours.ToString() + DateTime.Now.TimeOfDay.Minutes.ToString() + DateTime.Now.TimeOfDay.Seconds.ToString() + ".xls";
                }
                else
                {
                    RetVal = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day + DateTime.Now.TimeOfDay.Hours.ToString() + DateTime.Now.TimeOfDay.Minutes.ToString() + DateTime.Now.TimeOfDay.Seconds.ToString() + ".html";
                }

            }

            //Set relative path to absolute path when PresentationOutput type is MHT.
            string PresentationXLSFilePath = Path.Combine(PresentationOutputFolder, RetVal);



            if (!Directory.Exists(PresentationOutputFolder))
            {
                Directory.CreateDirectory(PresentationOutputFolder);
            }
            if (File.Exists(PresentationXLSFilePath) && PresentationType == Presentation.PresentationType.Table)
            {
                File.Delete(PresentationXLSFilePath);
            }


            #region "-- Make data arrays which will be used to be get dumped on Worksheets --"

            //Data Arays which wil be used to be get dumped on Worksheets
            //Data Array containing TableXLS Data. 
            Margin = TitleSubTitleMargin;
            string[,] TableXLSArray = new string[TotalTableXLSROWS + Margin, TotalTableXLSColumns];

            //DataArray containing FootNotes.
            if (this._TemplateStyle.Footnotes.Show)
            {
                TableFootNoteArray = new string[TableFootNote.Rows.Count, 1];
            }

            //DataArray Containing Comments.
            if (this._TemplateStyle.Comments.Show)
            {
                TableCommentsArray = new string[DTComments.Rows.Count, 1];
            }

            //DataArray Containing sources.			
            string[,] TableSourcesArray = new string[this.DTSources.Rows.Count, this.DTSources.Columns.Count];

            string[,] TableDataSheetArray = new string[this.DTDataSheet.Rows.Count, this.DTDataSheet.Columns.Count];

            #endregion

            this.RaiseEventProgressBar(80, 100);

            #region "-- Fill Data Arrays --"

            //Fill Data Arrays-----------------

            //Insert TableXLS cells into the Array one by one.

            foreach (DataRow DRowTableXLS in TableXLS.Rows)
            {
                for (Columns = 0; Columns < TotalTableXLSColumns; Columns++)
                {
                    //- Prefix "'" before time periods so that they are considered as textual values inside excel
                    if (TableXLS.Columns[Columns].ColumnName == Timeperiods.TimePeriod && DRowTableXLS[ROWTYPE].ToString() == RowType.DataRow.ToString())
                    {
                        if (this._PresentationOutputType == PresentationOutputType.MHT)
                        {
                            TableXLSArray[Margin, Columns] = DRowTableXLS[Columns].ToString();
                        }
                        else
                        {
                            TableXLSArray[Margin, Columns] = "'" + DRowTableXLS[Columns].ToString();
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(this._TableReport.AreaSelection.AreaLevels))
                        {
                            if (TableXLS.Columns[Columns].ColumnName == Area.AreaName)
                            {
                                if (DRowTableXLS[TablePresentation.AREALEVEL].ToString() == "1")
                                {
                                    TableXLSArray[Margin, Columns] = DRowTableXLS[Columns].ToString();
                                }
                                else if (DRowTableXLS[TablePresentation.AREALEVEL].ToString() == "2")
                                {
                                    TableXLSArray[Margin, Columns] = "   " + DRowTableXLS[Columns].ToString();
                                }
                                else if (DRowTableXLS[TablePresentation.AREALEVEL].ToString() == "3")
                                {
                                    TableXLSArray[Margin, Columns] = "      " + DRowTableXLS[Columns].ToString();
                                }
                                else if (DRowTableXLS[TablePresentation.AREALEVEL].ToString() == "4")
                                {
                                    TableXLSArray[Margin, Columns] = "         " + DRowTableXLS[Columns].ToString();
                                }
                                else if (DRowTableXLS[TablePresentation.AREALEVEL].ToString() == "5")
                                {
                                    TableXLSArray[Margin, Columns] = "            " + DRowTableXLS[Columns].ToString();
                                }
                                else
                                {
                                    TableXLSArray[Margin, Columns] = DRowTableXLS[Columns].ToString();
                                }
                            }
                            else
                            {
                                TableXLSArray[Margin, Columns] = DRowTableXLS[Columns].ToString();
                            }
                        }
                        else
                        {
                            TableXLSArray[Margin, Columns] = DRowTableXLS[Columns].ToString();
                        }

                    }

                }
                Margin++;
            }
            Margin = TitleSubTitleMargin;


            //If FooNote are not set to inline then show them at the bottom.
            if (this._TemplateStyle.Footnotes.Show == true && this.FootnoteInLine == FootNoteDisplayStyle.Separate && (this.FootnoteCommentType == FootnoteCommentType.Footnote || this.FootnoteCommentType == FootnoteCommentType.Both))
            {
                //Insert TableFootNote cells into the Array one by one.
                Margin = 0;
                foreach (DataRow DRowTableFootNote in TableFootNote.Rows)
                {
                    for (Columns = 0; Columns < TableFootNote.Columns.Count; Columns++)
                    {
                        TableFootNoteArray[Margin, 0] += " " + DRowTableFootNote[Columns].ToString();
                    }
                    Margin++;
                }

                Margin = TitleSubTitleMargin;
            }

            //If Comments are not set to inline then show them at the bottom.
            if (this._TemplateStyle.Comments.Show == true && this._TemplateStyle.Comments.FontTemplate.Inline == false && (this.FootnoteCommentType == FootnoteCommentType.Comment || this.FootnoteCommentType == FootnoteCommentType.Both))
            {
                //Insert DTComments cells into the Array one by one.
                Margin = 0;
                foreach (DataRow DRowDTComments in DTComments.Rows)
                {
                    for (Columns = 0; Columns < DTComments.Columns.Count; Columns++)
                    {
                        TableCommentsArray[Margin, 0] += " " + DRowDTComments[Columns].ToString();
                    }
                    Margin++;
                }
                Margin = TitleSubTitleMargin;
            }

            //Insert Sources into data array. Sources will be shown on sources WorkSheet. 
            Margin = 0;
            foreach (DataRow DRowDTSources in this.DTSources.Rows)
            {
                for (Columns = 0; Columns < this.DTSources.Columns.Count; Columns++)
                {
                    TableSourcesArray[Margin, Columns] = "   " + DRowDTSources[Columns].ToString();
                }
                Margin++;
            }
            Margin = TitleSubTitleMargin;


            //Insert DTDataSheet into array which are shown on Data worksheet.
            Margin = 0;
            foreach (DataRow DRowDTDataSheet in this.DTDataSheet.Rows)
            {
                for (Columns = 0; Columns < this.DTDataSheet.Columns.Count; Columns++)
                {
                    //- Prefix "'" before time periods so that they are considered as textual values inside excel
                    if (DTDataSheet.Columns[Columns].ColumnName == Timeperiods.TimePeriod)
                    {
                        TableDataSheetArray[Margin, Columns] = "'" + DRowDTDataSheet[Columns].ToString();
                    }
                    else
                    {
                        TableDataSheetArray[Margin, Columns] = DRowDTDataSheet[Columns].ToString();
                    }
                }
                Margin++;
            }
            Margin = TitleSubTitleMargin;

            #endregion


            //Fill Excel Worksheets.
            if (this.PresentationOutputType == PresentationOutputType.ExcellSheet)
            {
                this.ProcessExcelFile(TableXLS, TableFootNote, DTComments, PresentationXLSFilePath, TableXLSArray, TableFootNoteArray, TableCommentsArray, TableSourcesArray, TableDataSheetArray, TotalTableXLSROWS + TitleSubTitleMargin, TotalTableXLSColumns);
            }
            else if (this._PresentationOutputType == PresentationOutputType.MHT)
            {
                this.ProcessExcelFileMHT(TableXLS, TableFootNote, DTComments, PresentationXLSFilePath, TableXLSArray, TableFootNoteArray, TableCommentsArray, TableSourcesArray, TableDataSheetArray, TotalTableXLSROWS + TitleSubTitleMargin, TotalTableXLSColumns);
            }
            return RetVal;
        }


        private void SetSuppressForInnerLevels(int ColCount, ArrayList ALHeading, ArrayList ALBaseHeading, DataRow DrowTableXLS, DataTable TableXLSClone, int SubtractFromRows)
        {
            if (ColCount < this._Fields.Rows.Count - SubtractFromRows)
            {
                ALHeading[ColCount - 1] = DrowTableXLS[ColCount].ToString();
                //SecondLevelHeading = DrowTableXLS[1].ToString();
                if (ALHeading[ColCount - 1].ToString() != ALBaseHeading[ColCount - 1].ToString())
                {
                    for (int i = ColCount + 1; i < this._Fields.Rows.Count - SubtractFromRows; i++)
                    {
                        ALHeading[i - 1] = string.Empty;
                        ALBaseHeading[i - 1] = string.Empty;
                    }
                    DataRow TableXLSCloneRow;
                    TableXLSCloneRow = TableXLSClone.NewRow();
                    TableXLSCloneRow[ColCount] = DrowTableXLS[ColCount];

                    if (this._Fields.Rows[Timeperiods.TimePeriod] != null)
                    {
                        if (this._Fields.Rows[Timeperiods.TimePeriod].FieldIndex == ColCount)
                        {
                            if (this._PresentationOutputType == PresentationOutputType.MHT)
                            {
                                TableXLSCloneRow[ColCount] = DrowTableXLS[ColCount];
                            }
                            else
                            {
                                TableXLSCloneRow[ColCount] = "'" + DrowTableXLS[ColCount];
                            }
                        }
                    }
                    TableXLSCloneRow[TablePresentation.AREALEVEL] = "-1";
                    TableXLSCloneRow[TablePresentation.ROWTYPE] = RowType.SupressRow.ToString();
                    TableXLSCloneRow[TablePresentation.ROWINDEX] = this.NewRowIndex.ToString();
                    this.NewRowIndex++;
                    TableXLSClone.Rows.Add(TableXLSCloneRow);
                    ALBaseHeading[ColCount - 1] = ALHeading[ColCount - 1];
                    ColCount++;
                    SetSuppressForInnerLevels(ColCount, ALHeading, ALBaseHeading, DrowTableXLS, TableXLSClone, SubtractFromRows);
                    //SecondLevelBaseHeading = SecondLevelHeading;
                }
                else
                {
                    ColCount++;
                    SetSuppressForInnerLevels(ColCount, ALHeading, ALBaseHeading, DrowTableXLS, TableXLSClone, SubtractFromRows);
                }
            }
        }


        /// <summary>
        /// This procedure returns a text alignment type
        /// </summary>
        /// <param name="SAlignment">Input alignment type in System.Drawing.StringAlignment format</param>
        /// <returns>Return alignment type in string format</returns>
        private string GetTextAlignment(StringAlignment StrAlignment)
        {
            string TextAlign = string.Empty;

            if (StrAlignment == StringAlignment.Near)
                TextAlign = "left";
            else if (StrAlignment == StringAlignment.Far)
                TextAlign = "Right";
            else
                TextAlign = "Center";

            return TextAlign;
        }


        /// <summary>
        /// Prepares XLS file with TableXLS content,Footnotes, Notes and with other required Values.
        /// </summary>
        /// <param name="TableXLS">DataTable that holds the actual XLS output</param>
        /// <param name="TableFootNote">DataTable containing FootNote valus</param>
        /// <param name="TableComments">DataTable containing comments values</param>
        /// <param name="XlsFilePath">FilePath with File name where XLS file will be saved</param>
        /// <param name="TableXLSArray">Two dimentional array containing TableXLS Data.</param>
        /// <param name="TableFootNoteArray">Two dimentional array containing Footnote Data.</param>
        /// <param name="TableCommentsArray">Two dimentional array containing comments Data.</param>
        /// <param name="TableSourcesArray">Two dimentional array containing sources Data.</param>
        /// <param name="TableDataSheetArray">Two dimentional array containing Data to be displayes on Data Worksheet.</param>
        /// <param name="totalRows">TotalRows required in TableXLS with the margin required at top</param>
        /// <param name="totalColumns">Total DataColumns of TableXLS</param>		
        private void ProcessExcelFileMHT(DataTable TableXLS, DataTable TableFootNote, DataTable TableComments, string PresentationXLSFilePath, string[,] TableXLSArray, string[,] TableFootNoteArray, string[,] TableCommentsArray, string[,] TableSourcesArray, string[,] TableDataSheetArray, int totalRows, int totalColumns)
        {
            #region "-- Variable declaration --"

            int ColumnArrangementColumns;
            StringBuilder MHTFile = new StringBuilder();

            //If DataValue column is present 
            if (this._ColumnArrangementTable.Columns.Count > 2)
            {
                ColumnArrangementColumns = (this._ColumnArrangementTable.Columns.Count - 2) / 2;
            }
            else  //If DataValue column is present then assign 0
            {
                ColumnArrangementColumns = 1;
            }
            int XLSRows = TableXLS.Rows.Count;
            int TotalXLSRows = TableXLSArray.GetLength(0);
            int TotalXLSColumns = TableXLS.Columns.Count - (4 + this.MaxMinAreaLevelDiff);	 //2 for rowtype and row count columns. Leave One as index starts from 0 in spreadsheet

            string GroupHeader = string.Empty;
            string BaseGroupHeader = string.Empty;
            string AggregateString = "#AggregateString";
            int AggregateCounter = 0;
            int ColumnCounter = 0;
            string IndicatorLocation = string.Empty;
            string IndicatorName = string.Empty;
            int LegendIndex = 0;
            Double DataValue;
            bool PresentInRange = false;
            int TotalFormatAreaLevel = 0;


            if (!string.IsNullOrEmpty(this._TableReport.AreaSelection.AreaLevels))
            {
                TotalFormatAreaLevel = this._TableReport.AreaSelection.PrimaryAreaLevel - 1;
            }

            #endregion

            #region "-- Table WorkSheet -- "

            #region "-- Variable Declaration --"

            TableColumnType ColumnType;
            int ColumnIndex;
            bool FootnotesExists = false;
            bool CommentExists = false;
            string Header = string.Empty;
            int ColSpanIndex = 1;
            ArrayList ColumnsRemovedIndex = new ArrayList();
            int RowIndex;
            ArrayList ALBaseIndexes = new ArrayList();
            int BaseIndex = 0;
            int IndexALBaseIndexes = 0;
            Dictionary<int, int> Suppress = new Dictionary<int, int>();
            Dictionary<int, int> BaseSuppress = new Dictionary<int, int>();

            #endregion

            #region "-- Make the Head and Script Section --

            if (this._IsWizardMode)
            {
                MHTFile.Append("<HTML><HEAD><meta charset='UTF-8'>");
                MHTFile.Append("<METAHTTP-EQUIV='PRAGMA' CONTENT='NO-CACHE'>");
                MHTFile.Append("</HEAD><BODY onresize='SetDivTblHeight();'>");

                if (this._IsLaunchedFromWizard == false)
                {
                    MHTFile.Append("<script>function showView(val){try {if(val=='DivTable'){document.getElementById(val).style.display='block';document.getElementById('DivData').style.display='none';");
                    MHTFile.Append("document.getElementById('DivSource').style.display='none';document.getElementById('DivGraph').style.display='none';}");

                    MHTFile.Append("if(val=='DivData'){document.getElementById(val).style.display='block';document.getElementById('DivTable').style.display='none';document.getElementById('DivSource').style.display='none';document.getElementById('DivGraph').style.display='none';}");

                    if (this._PresentationType == Presentation.PresentationType.Graph)
                    {
                        MHTFile.Append("if(val=='DivGraph'){document.getElementById(val).style.display='block';document.getElementById('DivTable').style.display='none';document.getElementById('DivSource').style.display='none';document.getElementById('DivData').style.display='none';}");
                    }
                    MHTFile.Append("if(val=='DivSource'){document.getElementById(val).style.display='block';document.getElementById('DivData').style.display='none';");
                    MHTFile.Append("document.getElementById('DivTable').style.display='none';document.getElementById('DivGraph').style.display='none';}}catch (ex){}}</script>");
                }
            }

            #endregion

            //Start of outer table
            MHTFile.Append("<table width='100%' cellSpacing='0px' cellPadding='0px'><tr height='100%'><td>");
            if (this._IsWizardMode)
            {
                MHTFile.Append("<div id='DivTable' style='overflow:auto;width:100%;top:0px;left:0px'>");
            }
            MHTFile.Append("<Div id='d0'>");

            #region "-- Title and Subtitle Settings --

            int tableWidth = 0;
            int dataRowsCount = 0;

            //Hide Title SubTitle if not Required--If both Title and SubTitle are empty and HideTitleSubtitle is set to false
            if (!(this._Title == string.Empty && this._Subtitle == string.Empty && this._HideTitleSubtitleRows == true))
            {
                //Second Inner Table - table for Title and SubTitle                
                //MHTFile.Append("<Table id='Title-SubTitle' width='100%'>");
                MHTFile.Append("<Table id='Title-SubTitle'>");

                //Set style and Caption for Title 
                if (this._TemplateStyle.TitleSetting.FontTemplate.FontStyle == FontStyle.Bold)
                    MHTFile.Append("<tr><td id='tdTitle' align='" + GetTextAlignment(this._TemplateStyle.TitleSetting.FontTemplate.TextAlignment) + "'  style='color:" + ColorTranslator.ToHtml(this._TemplateStyle.TitleSetting.FontTemplate.ForeColor) + ";font-size:" + this._TemplateStyle.TitleSetting.FontTemplate.FontSize + "px;background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.TitleSetting.FontTemplate.BackColor) + ";font-weight:bold;font-family: " + this._TemplateStyle.TitleSetting.FontTemplate.FontName + "' > " + this._Title + " </td></tr>");
                else if (this._TemplateStyle.TitleSetting.FontTemplate.FontStyle == FontStyle.Underline)
                    MHTFile.Append("<tr><td id='tdTitle' align='" + GetTextAlignment(this._TemplateStyle.TitleSetting.FontTemplate.TextAlignment) + "'  style='color:" + ColorTranslator.ToHtml(this._TemplateStyle.TitleSetting.FontTemplate.ForeColor) + ";font-size:" + this._TemplateStyle.TitleSetting.FontTemplate.FontSize + "px;background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.TitleSetting.FontTemplate.BackColor) + ";text-decoration:underline;font-family: " + this._TemplateStyle.TitleSetting.FontTemplate.FontName + "' > " + this._Title + " </td></tr>");
                else
                    MHTFile.Append("<tr><td id='tdTitle' align='" + GetTextAlignment(this._TemplateStyle.TitleSetting.FontTemplate.TextAlignment) + "'  style='color:" + ColorTranslator.ToHtml(this._TemplateStyle.TitleSetting.FontTemplate.ForeColor) + ";font-size:" + this._TemplateStyle.TitleSetting.FontTemplate.FontSize + "px;background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.TitleSetting.FontTemplate.BackColor) + ";font-style:" + this._TemplateStyle.TitleSetting.FontTemplate.FontStyle + ";font-family: " + this._TemplateStyle.TitleSetting.FontTemplate.FontName + "' > " + this._Title + " </td></tr>");


                //Set style and caption for SubTitle
                if (this._TemplateStyle.SubTitleSetting.FontTemplate.FontStyle == FontStyle.Bold)
                    MHTFile.Append("<tr><td id='tdSubtitle' align='" + GetTextAlignment(this._TemplateStyle.SubTitleSetting.FontTemplate.TextAlignment) + "' style='color:" + ColorTranslator.ToHtml(this._TemplateStyle.SubTitleSetting.FontTemplate.ForeColor) + ";font-size:" + this._TemplateStyle.SubTitleSetting.FontTemplate.FontSize + "px;background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.SubTitleSetting.FontTemplate.BackColor) + ";font-weight:bold;font-family: " + this._TemplateStyle.SubTitleSetting.FontTemplate.FontName + " '> " + this._Subtitle + " </td></tr>");
                else if (this._TemplateStyle.SubTitleSetting.FontTemplate.FontStyle == FontStyle.Underline)
                    MHTFile.Append("<tr><td id='tdSubtitle' align='" + GetTextAlignment(this._TemplateStyle.SubTitleSetting.FontTemplate.TextAlignment) + "' style='color:" + ColorTranslator.ToHtml(this._TemplateStyle.SubTitleSetting.FontTemplate.ForeColor) + ";font-size:" + this._TemplateStyle.SubTitleSetting.FontTemplate.FontSize + "px;background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.SubTitleSetting.FontTemplate.BackColor) + ";text-decoration:underline;font-family: " + this._TemplateStyle.SubTitleSetting.FontTemplate.FontName + " '> " + this._Subtitle + " </td></tr>");
                else
                    MHTFile.Append("<tr><td id='tdSubtitle' align='" + GetTextAlignment(this._TemplateStyle.SubTitleSetting.FontTemplate.TextAlignment) + "' style='color:" + ColorTranslator.ToHtml(this._TemplateStyle.SubTitleSetting.FontTemplate.ForeColor) + ";font-size:" + this._TemplateStyle.SubTitleSetting.FontTemplate.FontSize + "px;background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.SubTitleSetting.FontTemplate.BackColor) + ";font-style: " + this._TemplateStyle.SubTitleSetting.FontTemplate.FontStyle + ";font-family: " + this._TemplateStyle.SubTitleSetting.FontTemplate.FontName + " '> " + this._Subtitle + " </td></tr>");


                //Second Table closed -- Title and subtitle
                MHTFile.Append("</Table>");
            }

            #endregion

            #region "-- Set border for TableXLS --

            //Third Inner Table - -Table for TableXLS
            MHTFile.Append("<Table id='TableXLS' class='bodytext' cellspacing='0' cellpadding='3'");
            if (this._TemplateStyle.ShowBorderLines)
            {
                MHTFile.Append("border='1px' style='border-style:solid;border-collapse:collapse'");
            }
            else
            {
                MHTFile.Append("border='0px'");
            }
            MHTFile.Append(">");

            #endregion

            #region "-- Maintains a list of column index in which there is no value exists ( List of empty columns) --"

            if (ALColumnType.Count > 0)
            {
                for (ColumnIndex = 0; ColumnIndex < TotalXLSColumns; ColumnIndex++)
                {
                    if ((TableColumnType)ALColumnType[ColumnIndex] == TableColumnType.FootNote)
                    {
                        for (int rows = 0; rows < TableXLS.Rows.Count; rows++)
                        {
                            if (TableXLS.Rows[rows][ColumnIndex].ToString() != string.Empty)
                            {
                                FootnotesExists = true;
                            }
                        }
                    }
                    if ((TableColumnType)ALColumnType[ColumnIndex] == TableColumnType.Comment)
                    {
                        for (int rows = 0; rows < TableXLS.Rows.Count; rows++)
                        {
                            if (TableXLS.Rows[rows][ColumnIndex].ToString() != string.Empty)
                            {
                                CommentExists = true;
                            }
                        }
                    }
                    if (FootnotesExists == false && (TableColumnType)ALColumnType[ColumnIndex] == TableColumnType.FootNote)
                    {
                        ColumnsRemovedIndex.Add(ColumnIndex);
                    }
                    if (CommentExists == false && (TableColumnType)ALColumnType[ColumnIndex] == TableColumnType.Comment)
                    {
                        ColumnsRemovedIndex.Add(ColumnIndex);
                    }
                    FootnotesExists = false;
                    CommentExists = false;
                }
            }

            #endregion

            //Initializes the arraylist BaseSuppress.
            for (int Count = 0; Count < TotalXLSColumns; Count++)
            {
                BaseSuppress.Add(Count, 0);
            }

            #region "-- Render TableXLS and apply style


            System.Globalization.CultureInfo OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture;

            if (this._Fields.Rows[Indicator.IndicatorName] != null)
            {
                IndicatorLocation = "Rows";
            }
            else if (this._Fields.Columns[Indicator.IndicatorName] != null)
            {
                IndicatorLocation = "Columns";
            }

            DataTable dt = this.ColumnArrangementTable;
            bool AlternateRow = false;
            int totalHeaderRows = TableXLS.Select("RowType='TableHeaderRow'").Length;
            //Render TableXLS Content and apply style			
            for (int rows = 0; rows < TableXLS.Rows.Count; rows++)
            {
                //Initialize column counter. Used in Setting Applycolor. 
                //Used to get IndicatorName from ColumnArrangement table when indicator are in columns.
                ColumnCounter = 0;

                IndexALBaseIndexes = 0;
                if (rows > 0)
                {
                    BaseSuppress.Clear();
                    foreach (int key in Suppress.Keys)
                    {
                        BaseSuppress[key] = Suppress[key];
                    }
                    Suppress.Clear();
                }
                BaseIndex = 0;
                //Pick Indicator Name to get the color of column in case apply color is on
                if (IndicatorLocation == "Rows" && this._AddColor)
                {
                    IndicatorName = TableXLS.Rows[rows][Indicator.IndicatorName].ToString();
                }

                MHTFile.Append("<tr");
                if (this._TemplateStyle.RowSetting.FontTemplate.ShowAlternateColor && !this._AddColor && TableXLS.Rows[rows][ROWTYPE].ToString() == RowType.DataRow.ToString())
                {
                    if (AlternateRow == false)
                    {
                        MHTFile.Append(" style='background-color:" + this._TemplateStyle.RowSetting.FontTemplate.AlternateBackColor1 + "'");
                        AlternateRow = true;
                    }
                    else
                    {
                        MHTFile.Append(" style='background-color:" + this._TemplateStyle.RowSetting.FontTemplate.AlternateBackColor2 + "'");
                        AlternateRow = false;
                    }
                }
                else
                {
                    MHTFile.Append(" style='background-color:" + this._TemplateStyle.RowSetting.FontTemplate.BackColor + "'");
                }
                MHTFile.Append(" >");
                for (int Columns = 0; Columns < TotalXLSColumns; Columns++)
                {

                    //if ((TableColumnType)ALColumnType[Columns] == TableColumnType.DataValue)
                    //{          
                    //    Header = TableXLS.Rows[rows][Columns].ToString();
                    //    if (Header != string.Empty && Header.Substring(0,1) == "'")
                    //    {
                    //        Header = Header.Substring(1);
                    //    }
                    //}

                    //-----------------------
                    //Table XLS Header
                    //-----------------------

                    dataRowsCount++;
                    if (rows < ColumnArrangementColumns)	//Apply style for Table XLS header.
                    {
                        if (!ColumnsRemovedIndex.Contains(Columns) || Columns == TotalXLSColumns)	//If column is not blank or column index is the last index.
                        {
                            if (this._IsSuppressDuplicateColumns && Columns >= this._Fields.Rows.Count)		   //If Suppress duplicate is checked
                            {
                                if ((TableColumnType)ALColumnType[Columns] == TableColumnType.DataValue)
                                {
                                    Header = TableXLS.Rows[rows][Columns].ToString();
                                    if (Header != string.Empty && Header.Substring(0, 1) == "'")
                                    {
                                        Header = Header.Substring(1);
                                    }
                                }
                                if (Columns != TotalXLSColumns)		//Do not execute when column is the last column
                                {
                                    //Executes the if condition when next data value is same or next column is footnote/comment and lies in the same parent column.
                                    if ((Header == TableXLS.Rows[rows][Columns + 1].ToString() || (TableColumnType)ALColumnType[Columns + 1] == TableColumnType.FootNote || (TableColumnType)ALColumnType[Columns + 1] == TableColumnType.Comment) && IndexALBaseIndexes == BaseSuppress[Columns])
                                    {
                                        //Executes when next column is empty and second next columns datavalue is not same as of current
                                        if ((ColumnsRemovedIndex.Contains(Columns + 1) && (TableColumnType)ALColumnType[Columns + 2] == TableColumnType.DataValue && Header != TableXLS.Rows[rows][Columns + 2].ToString()) || (ColumnsRemovedIndex.Contains(Columns + 1) && ColumnsRemovedIndex.Contains(Columns + 2) && (TableColumnType)ALColumnType[Columns + 3] == TableColumnType.DataValue && Header != TableXLS.Rows[rows][Columns + 3].ToString()))
                                        {
                                            IndexALBaseIndexes = BaseSuppress[Columns];
                                            if (ColSpanIndex > 1)
                                            {
                                                //MHTFile.Append("<td align=center colspan=" + ColSpanIndex + " style='padding:4px;color: " + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.BackColor) + ";font-size:" + this._TemplateStyle.ColumnSetting.FontSize + ";font-family:" + this._TemplateStyle.ColumnSetting.FontName + ";font-style:" + this._TemplateStyle.ColumnSetting.FontStyle + "' id=" + rows + "_" + Columns + ">" + Header + "</td>");												
                                                if (this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle == FontStyle.Bold)
                                                    MHTFile.Append("<td align='center' colspan=" + ColSpanIndex + " style='padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color: " + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";font-weight:bold;' id=" + rows + "_" + Columns + ">" + Header + "</td>");
                                                else if (this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle == FontStyle.Underline)
                                                    MHTFile.Append("<td align='center' colspan=" + ColSpanIndex + " style='padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color: " + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";text-decoration:underline;' id=" + rows + "_" + Columns + ">" + Header + "</td>");
                                                else
                                                    MHTFile.Append("<td align='center' colspan=" + ColSpanIndex + " style='padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color: " + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle + "' id=" + rows + "_" + Columns + ">" + Header + "</td>");

                                                ColSpanIndex = 1;
                                                BaseIndex++;
                                                Suppress[Columns] = BaseIndex;
                                            }
                                            else
                                            {
                                                //MHTFile.Append("<td align=center style='padding:4px;color: " + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.BackColor) + ";font-size:" + this._TemplateStyle.ColumnSetting.FontSize + ";font-family:" + this._TemplateStyle.ColumnSetting.FontName + ";font-style:" + this._TemplateStyle.ColumnSetting.FontStyle + "' id=" + rows + "_" + Columns + ">" + Header + "</td>");
                                                MHTFile.Append("<td  align='center' style='padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color: " + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + "' id=" + rows + "_" + Columns + ">" + Header + "</td>");
                                                BaseIndex++;
                                                Suppress[Columns] = BaseIndex;
                                            }
                                        }
                                        else
                                        {
                                            ColSpanIndex++;
                                            Suppress[Columns] = BaseIndex;
                                        }
                                    }
                                    else
                                    {
                                        IndexALBaseIndexes = BaseSuppress[Columns];

                                        if (ColSpanIndex > 1)
                                        {

                                            if (this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle == FontStyle.Bold)
                                                MHTFile.Append("<td align='center' colspan=" + ColSpanIndex + " style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color: " + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";font-weight:bold;' id=" + rows + "_" + Columns + ">" + Header + "</td>");
                                            else if (this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle == FontStyle.Underline)
                                                MHTFile.Append("<td align='center' colspan=" + ColSpanIndex + " style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color: " + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";text-decoration:underline;' id=" + rows + "_" + Columns + ">" + Header + "</td>");
                                            else
                                                MHTFile.Append("<td align='center' colspan=" + ColSpanIndex + " style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color: " + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.Comments.FontTemplate.FontStyle + ";' id=" + rows + "_" + Columns + ">" + Header + "</td>");

                                            tableWidth += Convert.ToInt32(this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth);

                                            ColSpanIndex = 1;
                                            BaseIndex++;
                                            Suppress[Columns] = BaseIndex;
                                        }
                                        else
                                        {

                                            if (this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle == FontStyle.Bold)
                                                MHTFile.Append("<td align='center' style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color: " + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";font-weight:bold;' id=" + rows + "_" + Columns + ">" + Header + "</td>");
                                            else if (this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle == FontStyle.Underline)
                                                MHTFile.Append("<td align='center' style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color: " + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";text-decoration:underline;' id=" + rows + "_" + Columns + ">" + Header + "</td>");
                                            else
                                                MHTFile.Append("<td align='center' style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color: " + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.Comments.FontTemplate.FontStyle + "' id=" + rows + "_" + Columns + ">" + Header + "</td>");

                                            tableWidth += Convert.ToInt32(this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth);

                                            BaseIndex++;
                                            Suppress[Columns] = BaseIndex;
                                        }
                                    }
                                }
                                else
                                {
                                    if (ColSpanIndex > 1)
                                    {
                                        MHTFile.Append("<td align='center' colspan=" + ColSpanIndex + " style='padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color: " + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + "' id=" + rows + "_" + Columns + ">" + Header + "</td>");
                                        ColSpanIndex = 1;
                                    }
                                    else
                                    {
                                        MHTFile.Append("<td align='center' style='padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color: " + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + "' id=" + rows + "_" + Columns + ">" + Header + "</td>");
                                        BaseIndex++;
                                        Suppress[Columns] = BaseIndex;
                                    }
                                }
                            }
                            else	   //If Suppress duplicate is unchecked
                            {
                                ColSpanIndex = 1;
                                if (!ColumnsRemovedIndex.Contains(Columns))
                                {
                                    //TODO : Set borders of cell.
                                    ColumnType = (TableColumnType)ALColumnType[Columns];
                                    switch (ColumnType)
                                    {
                                        case TableColumnType.DataValue:
                                            Header = TableXLS.Rows[rows][Columns].ToString().TrimStart(("'").ToCharArray());

                                            if ((TableColumnType)ALColumnType[Columns] == TableColumnType.FootNote)
                                            {
                                                if (!ColumnsRemovedIndex.Contains(Columns + 1))
                                                {
                                                    if ((TableColumnType)ALColumnType[Columns] == TableColumnType.Comment)
                                                    {
                                                        if (!ColumnsRemovedIndex.Contains(Columns + 2))
                                                        {
                                                            ColSpanIndex = 3;
                                                        }
                                                        else
                                                        {
                                                            ColSpanIndex = 2;
                                                        }
                                                        Columns += 2;
                                                    }
                                                    else
                                                    {
                                                        ColSpanIndex = 2;
                                                        Columns++;
                                                    }
                                                }
                                                else
                                                {
                                                    if ((TableColumnType)ALColumnType[Columns] == TableColumnType.Comment)
                                                    {
                                                        if (!ColumnsRemovedIndex.Contains(Columns + 2))
                                                        {
                                                            ColSpanIndex = 2;
                                                        }
                                                        else
                                                        {
                                                            ColSpanIndex = 1;
                                                        }
                                                        Columns += 2;
                                                    }
                                                    else
                                                    {
                                                        ColSpanIndex = 1;
                                                        Columns++;
                                                    }
                                                }
                                            }
                                            else if ((TableColumnType)ALColumnType[Columns] == TableColumnType.Comment)
                                            {
                                                if (!ColumnsRemovedIndex.Contains(Columns + 1))
                                                {
                                                    ColSpanIndex = 2;
                                                }
                                                else
                                                {
                                                    ColSpanIndex = 1;
                                                }
                                                Columns++;
                                            }
                                            else
                                            {
                                                ColSpanIndex = 1;
                                            }

                                            if (this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle == FontStyle.Bold)
                                                MHTFile.Append("<td colspan=" + ColSpanIndex + " align='center' style='border-right-width:0px;width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";font-weight:bold;' id=" + rows + "_" + Columns + ">" + Header + "</td>");
                                            else if (this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle == FontStyle.Underline)
                                                MHTFile.Append("<td colspan=" + ColSpanIndex + " align='center' style='border-right-width:0px;width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";text-decoration:underline;' id=" + rows + "_" + Columns + ">" + Header + "</td>");
                                            else
                                                MHTFile.Append("<td colspan=" + ColSpanIndex + " align='center' style='border-right-width:0px;width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle + "' id=" + rows + "_" + Columns + ">" + Header + "</td>");
                                            tableWidth += Convert.ToInt32(this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth);
                                            break;
                                        case TableColumnType.FootNote:
                                            MHTFile.Append("<td style='padding:4px;border-left-width:0px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + "' id=" + rows + "_" + Columns + "></td>");
                                            break;
                                        case TableColumnType.Comment:
                                            MHTFile.Append("<td style='padding:4px;border-left-width:0px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + "' id=" + rows + "_" + Columns + "></td>");
                                            break;
                                        case TableColumnType.Denominator:
                                            MHTFile.Append("<td style='padding:4px;border-left-width:0px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + "' id=" + rows + "_" + Columns + "></td>");
                                            break;
                                        case TableColumnType.RowHeader:
                                            if (rows == this._Fields.Columns.Count - 1)
                                            {
                                                if (Columns == 0 && this._IsSuppressDuplicateRows)
                                                {
                                                    if (this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle == FontStyle.Bold)
                                                        MHTFile.Append("<td style='padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";font-weight:bold;' id=" + rows + "_" + Columns + "></td>");
                                                    else if (this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle == FontStyle.Underline)
                                                        MHTFile.Append("<td style='padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";text-decoration:underline;' id=" + rows + "_" + Columns + "></td>");
                                                    else
                                                        MHTFile.Append("<td style='padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle + "' id=" + rows + "_" + Columns + "></td>");

                                                }
                                                else
                                                {

                                                    if (totalHeaderRows - 1 == rows)
                                                    {
                                                        if (this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle == FontStyle.Bold)
                                                            MHTFile.Append("<td style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";font-weight:bold;' id=" + rows + "_" + Columns + ">" + this._Fields.Rows[Columns].Caption + "</td>");
                                                        else if (this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle == FontStyle.Underline)
                                                            MHTFile.Append("<td style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";text-decoration:underline;' id=" + rows + "_" + Columns + ">" + this._Fields.Rows[Columns].Caption + "</td>");
                                                        else
                                                            MHTFile.Append("<td style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle + "' id=" + rows + "_" + Columns + ">" + this._Fields.Rows[Columns].Caption + "</td>");
                                                    }
                                                    else
                                                    {
                                                        MHTFile.Append("<td style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle + "' id=" + rows + "_" + Columns + "></td>");
                                                    }
                                                    tableWidth += Convert.ToInt32(this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth);
                                                }
                                            }
                                            else
                                            {

                                                if (Columns == 0 && this._IsSuppressDuplicateRows)
                                                {
                                                    //MHTFile.Append("<td style='padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle + "' id=" + rows + "_" + Columns + "></td>");

                                                    if (this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle == FontStyle.Bold)
                                                        MHTFile.Append("<td style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";font-weight:bold' id=" + rows + "_" + Columns + "></td>");
                                                    else if (this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle == FontStyle.Underline)
                                                        MHTFile.Append("<td style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";text-decoration:underline' id=" + rows + "_" + Columns + "></td>");
                                                    else
                                                        MHTFile.Append("<td style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle + "' id=" + rows + "_" + Columns + "></td>");

                                                }
                                                else
                                                {
                                                    if (totalHeaderRows - 1 == rows)
                                                    {
                                                        if (this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle == FontStyle.Bold)
                                                            MHTFile.Append("<td style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";font-weight:bold;' id=" + rows + "_" + Columns + ">" + this._Fields.Rows[Columns].Caption + "</td>");
                                                        else if (this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle == FontStyle.Underline)
                                                            MHTFile.Append("<td style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";text-decoration:underline;' id=" + rows + "_" + Columns + ">" + this._Fields.Rows[Columns].Caption + "</td>");
                                                        else
                                                            MHTFile.Append("<td style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle + "' id=" + rows + "_" + Columns + ">" + this._Fields.Rows[Columns].Caption + "</td>");
                                                    }
                                                    else
                                                    {
                                                        MHTFile.Append("<td style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;font-size:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontSize + "px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ColumnSetting.FontTemplate.BackColor) + ";font-family:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle + "' id=" + rows + "_" + Columns + "></td>");
                                                    }
                                                    tableWidth += Convert.ToInt32(this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth);
                                                }
                                            }
                                            break;
                                        case TableColumnType.Others:

                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    else        //Make Rows after header rows
                    {
                        //dataRowsCount++;
                        if (TableXLS.Rows[rows][TotalXLSColumns].ToString() == RowType.ICAggregate.ToString())
                        {
                            MHTFile.Append("<td style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.GroupHeaderSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.GroupHeaderSetting.FontTemplate.BackColor) + ";font-size:" + this._TemplateStyle.GroupHeaderSetting.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.GroupHeaderSetting.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.GroupHeaderSetting.FontTemplate.FontStyle + "' id=" + rows + "_" + Columns + ">" + TableXLS.Rows[rows][Columns].ToString() + "</td>");
                            tableWidth += Convert.ToInt32(this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth);
                        }
                        else if (TableXLS.Rows[rows][TotalXLSColumns].ToString() == RowType.SubAggregate.ToString())
                        {
                            MHTFile.Append("<td style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.SubAggregateSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.SubAggregateSetting.FontTemplate.BackColor) + ";font-size:" + this._TemplateStyle.SubAggregateSetting.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.SubAggregateSetting.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.SubAggregateSetting.FontTemplate.FontStyle + "' id=" + rows + "_" + Columns + ">" + TableXLS.Rows[rows][Columns].ToString() + "</td>");
                            tableWidth += Convert.ToInt32(this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth);
                        }
                        else if (TableXLS.Rows[rows][TotalXLSColumns].ToString() == RowType.GroupAggregate.ToString())
                        {
                            //Formate data value for Group Aggregation.
                            if (TableXLS.Rows[rows][Columns].ToString() != "")
                            {
                                string tempValAgr = TableXLS.Rows[rows][Columns].ToString();
                                string NumberDecimalSeparatorAgr = string.Empty;
                                string NumberGroupSeparatorAgr = string.Empty;

                                if (DICommon.IsNumeric(TableXLS.Rows[rows][Columns].ToString()))
                                {
                                    //Check the current language & Set the NumberDecimalSeparatorAgr and NumberGroupSeparatorAgr value                                                                      
                                    if (_UserPreference.Language.InterfaceLanguage == "DI_French [fr].xml")
                                    {
                                        NumberDecimalSeparatorAgr = ",";
                                        NumberGroupSeparatorAgr = " ";
                                    }
                                    else
                                    {
                                        NumberDecimalSeparatorAgr = ".";
                                        NumberGroupSeparatorAgr = ",";
                                    }

                                    if (this._TemplateStyle.ContentSetting.FontTemplate.Show == true)
                                    {
                                        tempValAgr = Convert.ToString(Math.Round(Convert.ToDouble(tempValAgr), this._TemplateStyle.ContentSetting.FontTemplate.DecimalPlace));
                                        tempValAgr = string.Format(this.NumberFormat(this._TemplateStyle.ContentSetting.FontTemplate.DecimalPlace, tempValAgr), Convert.ToDouble(tempValAgr));

                                        if (tempValAgr.IndexOf('.') > -1)
                                        {
                                            int tempValLen = tempValAgr.Substring(tempValAgr.IndexOf('.') + 1).Length;

                                            if (this._TemplateStyle.ContentSetting.FontTemplate.DecimalPlace > tempValLen)
                                            {
                                                for (int i = 0; i < this._TemplateStyle.ContentSetting.FontTemplate.DecimalPlace - tempValLen; i++)
                                                    tempValAgr += "0";
                                            }
                                            else
                                                tempValAgr = tempValAgr.Substring(0, tempValAgr.Length - (tempValLen - this._TemplateStyle.ContentSetting.FontTemplate.DecimalPlace));
                                        }
                                        else if (tempValAgr.IndexOf('.') == -1 && this._TemplateStyle.ContentSetting.FontTemplate.DecimalPlace > 0)
                                        {
                                            tempValAgr += ".";
                                            for (int i = 0; i < this._TemplateStyle.ContentSetting.FontTemplate.DecimalPlace; i++)
                                                tempValAgr += "0";
                                        }

                                        tempValAgr = tempValAgr.Replace(",", NumberGroupSeparatorAgr);
                                        tempValAgr = tempValAgr.Replace(".", NumberDecimalSeparatorAgr);
                                    }
                                    else
                                    {
                                        tempValAgr = string.Format(this.NumberFormat(this._TemplateStyle.ContentSetting.FontTemplate.DecimalPlace, tempValAgr), Convert.ToDouble(tempValAgr));
                                        tempValAgr = tempValAgr.Replace(",", NumberGroupSeparatorAgr);
                                        tempValAgr = tempValAgr.Replace(".", NumberDecimalSeparatorAgr);
                                    }
                                }

                                if (this._TemplateStyle.GroupAggregateSetting.FontTemplate.FontStyle == FontStyle.Bold)
                                    MHTFile.Append("<td align='" + GetTextAlignment(this._TemplateStyle.ContentSetting.FontTemplate.TextAlignment) + "' style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.GroupAggregateSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.GroupAggregateSetting.FontTemplate.BackColor) + ";font-size:" + this._TemplateStyle.GroupAggregateSetting.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.GroupAggregateSetting.FontTemplate.FontName + ";font-weight:bold;' id=" + rows + "_" + Columns + ">" + tempValAgr + "</td>");
                                else if (this._TemplateStyle.GroupAggregateSetting.FontTemplate.FontStyle == FontStyle.Underline)
                                    MHTFile.Append("<td align='" + GetTextAlignment(this._TemplateStyle.ContentSetting.FontTemplate.TextAlignment) + "' style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.GroupAggregateSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.GroupAggregateSetting.FontTemplate.BackColor) + ";font-size:" + this._TemplateStyle.GroupAggregateSetting.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.GroupAggregateSetting.FontTemplate.FontName + ";text-decoration:underline;' id=" + rows + "_" + Columns + ">" + tempValAgr + "</td>");
                                else
                                    MHTFile.Append("<td align='" + GetTextAlignment(this._TemplateStyle.ContentSetting.FontTemplate.TextAlignment) + "' style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.GroupAggregateSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.GroupAggregateSetting.FontTemplate.BackColor) + ";font-size:" + this._TemplateStyle.GroupAggregateSetting.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.GroupAggregateSetting.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.GroupAggregateSetting.FontTemplate.FontStyle + "' id=" + rows + "_" + Columns + ">" + tempValAgr + "</td>");

                                tableWidth += Convert.ToInt32(this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth);

                            }
                            else
                            {
                                //MHTFile.Append("<td style='width:8px;background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.GroupAggregateSetting.FontTemplate.BackColor) + "' id=" + rows + "_" + Columns + ">" + "</td>");

                                //Check Footnotes element in a column.
                                int FootnotesRow = 1;
                                int FootnotesMark = 0;
                                for (; FootnotesRow < TableXLS.Rows.Count; FootnotesRow++)
                                {
                                    if (TableXLS.Rows[FootnotesRow][Columns].ToString() != "")
                                    {
                                        FootnotesMark = 1;
                                        break;
                                    }
                                }

                                //Add column if footnotes element exist in a column.
                                if (FootnotesMark == 1)
                                    MHTFile.Append("<td style='width:8px;background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.GroupAggregateSetting.FontTemplate.BackColor) + "' id=" + rows + "_" + Columns + ">" + "</td>");
                            }
                        }
                        else
                        {
                            ColumnType = (TableColumnType)ALColumnType[Columns];
                            switch (ColumnType)
                            {
                                case TableColumnType.DataValue:
                                    string formatIndex = string.Empty;
                                    if (!string.IsNullOrEmpty(this._TableReport.AreaSelection.AreaLevels))
                                    {
                                        formatIndex = TableXLS.Rows[rows][TablePresentation.AREALEVEL].ToString();
                                    }
                                    if (formatIndex.Length > 0)
                                    {
                                        // for Report presentation 
                                        MHTFile.Append("<td align='right' style='width:300px;padding:4px;color:" + ColorTranslator.ToHtml(this._TableReport.LevelFormat[formatIndex].FontSetting.ForeColor) + ";font-family:" + this._TableReport.LevelFormat[formatIndex].FontSetting.FontName + ";font-size:" + this._TableReport.LevelFormat[formatIndex].FontSetting.FontSize + "px;height:" + this._TableReport.LevelFormat[formatIndex].FontSetting.RowHeight + "px; ");
                                    }
                                    else
                                    {
                                        // for table presentation
                                        if (this._TemplateStyle.ContentSetting.FontTemplate.FontStyle == FontStyle.Bold)
                                        {
                                            MHTFile.Append("<td align='" + GetTextAlignment(this._TemplateStyle.ContentSetting.FontTemplate.TextAlignment) + "' style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + ";padding:4px;font-size:" + this._TemplateStyle.ContentSetting.FontTemplate.FontSize + "px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ContentSetting.FontTemplate.ForeColor) + ";font-family:" + this._TemplateStyle.ContentSetting.FontTemplate.FontName + ";font-weight:bold;");
                                            tableWidth += Convert.ToInt32(this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth);
                                        }
                                        else if (this._TemplateStyle.ContentSetting.FontTemplate.FontStyle == FontStyle.Underline)
                                        {
                                            MHTFile.Append("<td align='" + GetTextAlignment(this._TemplateStyle.ContentSetting.FontTemplate.TextAlignment) + "' style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + ";padding:4px;font-size:" + this._TemplateStyle.ContentSetting.FontTemplate.FontSize + "px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ContentSetting.FontTemplate.ForeColor) + ";font-family:" + this._TemplateStyle.ContentSetting.FontTemplate.FontName + ";text-decoration:underline;");
                                            tableWidth += Convert.ToInt32(this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth);
                                        }
                                        else
                                        {
                                            MHTFile.Append("<td align='" + GetTextAlignment(this._TemplateStyle.ContentSetting.FontTemplate.TextAlignment) + "' style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + ";padding:4px;font-size:" + this._TemplateStyle.ContentSetting.FontTemplate.FontSize + "px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ContentSetting.FontTemplate.ForeColor) + ";font-family:" + this._TemplateStyle.ContentSetting.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.ContentSetting.FontTemplate.FontStyle + ";");
                                            tableWidth += Convert.ToInt32(this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth);
                                        }
                                    }
                                    //if (((this._AddColor == true && IndicatorLocation == "Rows") || (this._AddColor == true && IndicatorLocation == "Columns") || (this._AddColor == true && this._DistinctIndicator.Rows.Count == 1)) && this._IsWizardMode)
                                    if (this._AddColor) //For apply theme color on table
                                    {
                                        if (IndicatorLocation == "Columns")
                                        {
                                            IndicatorName = this._ColumnArrangementTable.Rows[ColumnCounter][Indicator.IndicatorName].ToString();
                                            ColumnCounter++;
                                        }
                                        if (DICommon.IsNumeric(TableXLS.Rows[rows][Columns].ToString(), OldCulture))		//If DataValue exists for the cell.
                                        {
                                            if (this._DistinctIndicator.Rows.Count == 1)
                                            {
                                                IndicatorName = this._DistinctIndicator.Rows[0][Indicator.IndicatorName].ToString();
                                            }
                                            DataValue = this.GetRoundedDataValue(TableXLS.Rows[rows][Columns].ToString(), OldCulture, this.Themes[IndicatorName].Decimal);
                                            LegendIndex = -1;
                                            PresentInRange = false;
                                            foreach (Legend Legend in this.Themes[DICommon.RemoveQuotes(IndicatorName)].Legends)
                                            {
                                                if (DataValue >= Convert.ToDouble(Legend.RangeFrom) && DataValue <= Convert.ToDouble(Legend.RangeTo))
                                                {
                                                    LegendIndex++;
                                                    PresentInRange = true;
                                                    break;
                                                }
                                                LegendIndex++;
                                            }
                                            if (PresentInRange)
                                            {
                                                MHTFile.Append("background-color:" + ColorTranslator.ToHtml(this.Themes[DICommon.RemoveQuotes(IndicatorName)].Legends[LegendIndex].Color) + "");
                                            }
                                            else
                                            {
                                                //For Report presentation 
                                                if (formatIndex.Length > 0)
                                                {
                                                    MHTFile.Append("background-color:" + ColorTranslator.ToHtml(this._TableReport.LevelFormat[formatIndex].FontSetting.BackColor) + "");
                                                }
                                                //For Table Presentation 
                                                else
                                                {
                                                    MHTFile.Append("background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ContentSetting.FontTemplate.BackColor) + "");
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //For Report presentation
                                        if (formatIndex.Length > 0)
                                        {
                                            MHTFile.Append("background-color:" + ColorTranslator.ToHtml(this._TableReport.LevelFormat[formatIndex].FontSetting.BackColor) + "");
                                        }
                                        //For Table Presentation 
                                        else
                                        {
                                            if (this._TemplateStyle.ContentSetting.FontTemplate.ShowBackColor)
                                            {
                                                MHTFile.Append("background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ContentSetting.FontTemplate.BackColor) + "");
                                            }
                                        }
                                    }
                                    if ((TableColumnType)ALColumnType[Columns] == TableColumnType.FootNote || (TableColumnType)ALColumnType[Columns] == TableColumnType.Comment)
                                    {
                                        //MHTFile.Append("<td align=right style='border-right-width:0;width:300px;padding:4px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ContentSetting.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ContentSetting.BackColor) + ";font-size:" + this._TemplateStyle.ContentSetting.FontSize + ";font-family:" + this._TemplateStyle.ContentSetting.FontName + ";font-style:" + this._TemplateStyle.ContentSetting.FontStyle + "' id=" + rows + "_" + Columns + ">" + TableXLS.Rows[rows][Columns].ToString() + "</td>");
                                        MHTFile.Append(";border-right-width:0px' id=" + rows + "_" + Columns + ">" + TableXLS.Rows[rows][Columns].ToString() + "</td>");
                                    }
                                    else
                                    {
                                        //MHTFile.Append("<td align=right style='width:300px;padding:4px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ContentSetting.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ContentSetting.BackColor) + ";font-size:" + this._TemplateStyle.ContentSetting.FontSize + ";font-family:" + this._TemplateStyle.ContentSetting.FontName + ";font-style:" + this._TemplateStyle.ContentSetting.FontStyle + "' id=" + rows + "_" + Columns + ">" + TableXLS.Rows[rows][Columns].ToString() + "</td>");
                                        //MHTFile.Append("' id=" + rows + "_" + Columns + ">" + TableXLS.Rows[rows][Columns].ToString() + "</td>");

                                        if (DICommon.IsNumeric(TableXLS.Rows[rows][Columns].ToString()))
                                        {
                                            string tempVal = TableXLS.Rows[rows][Columns].ToString();
                                            string NumberDecimalSeparator = string.Empty;
                                            string NumberGroupSeparator = string.Empty;

                                            //Check the current language & Set the NumberDecimalSeparator and NumberGroupSeparator value                                                                      
                                            if (_UserPreference.Language.InterfaceLanguage == "DI_French [fr].xml")
                                            {
                                                NumberDecimalSeparator = ",";
                                                NumberGroupSeparator = " ";
                                            }
                                            else
                                            {
                                                NumberDecimalSeparator = ".";
                                                NumberGroupSeparator = ",";
                                            }

                                            if (this._TemplateStyle.ContentSetting.FontTemplate.Show == true)
                                            {
                                                tempVal = Convert.ToString(Math.Round(Convert.ToDouble(tempVal), this._TemplateStyle.ContentSetting.FontTemplate.DecimalPlace));
                                                tempVal = string.Format(this.NumberFormat(this._TemplateStyle.ContentSetting.FontTemplate.DecimalPlace, tempVal), Convert.ToDouble(tempVal));

                                                if (tempVal.IndexOf('.') > -1)
                                                {
                                                    int tempValLen = tempVal.Substring(tempVal.IndexOf('.') + 1).Length;

                                                    if (this._TemplateStyle.ContentSetting.FontTemplate.DecimalPlace > tempValLen)
                                                    {
                                                        for (int i = 0; i < this._TemplateStyle.ContentSetting.FontTemplate.DecimalPlace - tempValLen; i++)
                                                            tempVal += "0";
                                                    }
                                                    else
                                                        tempVal = tempVal.Substring(0, tempVal.Length - (tempValLen - this._TemplateStyle.ContentSetting.FontTemplate.DecimalPlace));
                                                }
                                                else if (tempVal.IndexOf('.') == -1 && this._TemplateStyle.ContentSetting.FontTemplate.DecimalPlace > 0)
                                                {
                                                    tempVal += ".";
                                                    for (int i = 0; i < this._TemplateStyle.ContentSetting.FontTemplate.DecimalPlace; i++)
                                                        tempVal += "0";
                                                }

                                                tempVal = tempVal.Replace(",", NumberGroupSeparator);
                                                tempVal = tempVal.Replace(".", NumberDecimalSeparator);
                                                MHTFile.Append("' id=" + rows + "_" + Columns + ">" + tempVal + "</td>");
                                            }
                                            else
                                            {
                                                //tempVal = string.Format(this.NumberFormat(this._TemplateStyle.ContentSetting.FontTemplate.DecimalPlace, tempVal), Convert.ToDouble(tempVal));
                                                tempVal = tempVal.Replace(",", NumberGroupSeparator);
                                                tempVal = tempVal.Replace(".", NumberDecimalSeparator);
                                                MHTFile.Append("' id=" + rows + "_" + Columns + ">" + tempVal + "</td>");
                                            }
                                        }
                                        else
                                            MHTFile.Append("' id=" + rows + "_" + Columns + ">" + TableXLS.Rows[rows][Columns].ToString() + "</td>");
                                    }

                                    break;
                                case TableColumnType.FootNote:
                                    if (!ColumnsRemovedIndex.Contains(Columns))
                                    {
                                        if (ALColumnType.Count > Columns)
                                        {
                                            if ((TableColumnType)ALColumnType[Columns] == TableColumnType.Comment)
                                            {
                                                MHTFile.Append("<td align='left' style='width:8px;border-left-width:0px;border-right-width:0px;padding:4px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.Footnotes.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.Footnotes.FontTemplate.BackColor) + ";font-size:" + this._TemplateStyle.Footnotes.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Footnotes.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.Footnotes.FontTemplate.FontStyle + "' id=" + rows + "_" + Columns + "><sup>" + TableXLS.Rows[rows][Columns].ToString() + "</sup></td>");
                                            }
                                            else
                                            {
                                                //MHTFile.Append("<td align='left' style='width:8px;border-left-width:0px;padding:4px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.Footnotes.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.Footnotes.FontTemplate.BackColor) + ";font-size:" + this._TemplateStyle.Footnotes.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Footnotes.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.Footnotes.FontTemplate.FontStyle + "' id=" + rows + "_" + Columns + "><sup>" + TableXLS.Rows[rows][Columns].ToString() + "</sup></td>");
                                                if (this._TemplateStyle.Footnotes.FontTemplate.FontStyle == FontStyle.Bold)
                                                    MHTFile.Append("<td align='left' style='width:8px;border-left-width:0px;padding:4px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.Footnotes.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.Footnotes.FontTemplate.BackColor) + ";font-size:" + this._TemplateStyle.Footnotes.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Footnotes.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.Footnotes.FontTemplate.FontStyle + ";font-weight:bold;' id=" + rows + "_" + Columns + "><sup>" + TableXLS.Rows[rows][Columns].ToString() + "</sup></td>");
                                                else if (this._TemplateStyle.Footnotes.FontTemplate.FontStyle == FontStyle.Underline)
                                                    MHTFile.Append("<td align='left' style='width:8px;border-left-width:0px;padding:4px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.Footnotes.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.Footnotes.FontTemplate.BackColor) + ";font-size:" + this._TemplateStyle.Footnotes.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Footnotes.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.Footnotes.FontTemplate.FontStyle + ";text-decoration:underline;' id=" + rows + "_" + Columns + "><sup>" + TableXLS.Rows[rows][Columns].ToString() + "</sup></td>");
                                                else
                                                    MHTFile.Append("<td align='left' style='width:8px;border-left-width:0px;padding:4px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.Footnotes.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.Footnotes.FontTemplate.BackColor) + ";font-size:" + this._TemplateStyle.Footnotes.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Footnotes.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.Footnotes.FontTemplate.FontStyle + ";font-style:" + this._TemplateStyle.Footnotes.FontTemplate.FontStyle + ";' id=" + rows + "_" + Columns + "><sup>" + TableXLS.Rows[rows][Columns].ToString() + "</sup></td>");
                                            }
                                        }
                                        else
                                        {
                                            MHTFile.Append("<td align='left' style='width:8px;border-left-width:0px;padding:4px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.Footnotes.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.Footnotes.FontTemplate.BackColor) + ";font-size:" + this._TemplateStyle.Footnotes.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Footnotes.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.Footnotes.FontTemplate.FontStyle + "' id=" + rows + "_" + Columns + "><sup>" + TableXLS.Rows[rows][Columns].ToString() + "</sup></td>");
                                        }
                                    }
                                    break;
                                case TableColumnType.Comment:
                                    if (!ColumnsRemovedIndex.Contains(Columns))
                                    {
                                        //MHTFile.Append("<td align='left' style='border-left-width:0px;padding:4px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.Comments.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.Comments.FontTemplate.BackColor) + ";font-size:" + this._TemplateStyle.Comments.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Comments.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.Comments.FontTemplate.FontStyle + "' id=" + rows + "_" + Columns + "><sup>" + TableXLS.Rows[rows][Columns].ToString() + "</sup></td>");
                                        if (this._TemplateStyle.Comments.FontTemplate.FontStyle == FontStyle.Bold)
                                            MHTFile.Append("<td align='left' style='border-left-width:0px;padding:4px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.Comments.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.Comments.FontTemplate.BackColor) + ";font-size:" + this._TemplateStyle.Comments.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Comments.FontTemplate.FontName + ";font-weight:bold;' id=" + rows + "_" + Columns + "><sup>" + TableXLS.Rows[rows][Columns].ToString() + "</sup></td>");
                                        else if (this._TemplateStyle.Comments.FontTemplate.FontStyle == FontStyle.Underline)
                                            MHTFile.Append("<td align='left' style='border-left-width:0px;padding:4px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.Comments.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.Comments.FontTemplate.BackColor) + ";font-size:" + this._TemplateStyle.Comments.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Comments.FontTemplate.FontName + ";text-decoration:underline;' id=" + rows + "_" + Columns + "><sup>" + TableXLS.Rows[rows][Columns].ToString() + "</sup></td>");
                                        else
                                            MHTFile.Append("<td align='left' style='border-left-width:0px;padding:4px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.Comments.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.Comments.FontTemplate.BackColor) + ";font-size:" + this._TemplateStyle.Comments.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Comments.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.Comments.FontTemplate.FontStyle + "' id=" + rows + "_" + Columns + "><sup>" + TableXLS.Rows[rows][Columns].ToString() + "</sup></td>");
                                    }
                                    break;
                                case TableColumnType.Denominator:
                                    //MHTFile.Append("<td align='right' style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ContentSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ContentSetting.FontTemplate.BackColor) + ";font-size:" + this._TemplateStyle.ContentSetting.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.ContentSetting.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.ContentSetting.FontTemplate.FontStyle + "' id=" + rows + "_" + Columns + ">" + TableXLS.Rows[rows][Columns].ToString() + "</td>");
                                    if (this._TemplateStyle.Denominator.FontTemplate.FontStyle == FontStyle.Bold)
                                        MHTFile.Append("<td align='right' style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.Denominator.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.Denominator.FontTemplate.BackColor) + ";font-size:" + this._TemplateStyle.Denominator.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Denominator.FontTemplate.FontName + ";font-weight:bold;' id=" + rows + "_" + Columns + ">" + TableXLS.Rows[rows][Columns].ToString() + "</td>");
                                    else if (this._TemplateStyle.Denominator.FontTemplate.FontStyle == FontStyle.Underline)
                                        MHTFile.Append("<td align='right' style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.Denominator.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.Denominator.FontTemplate.BackColor) + ";font-size:" + this._TemplateStyle.Denominator.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Denominator.FontTemplate.FontName + ";text-decoration:underline;' id=" + rows + "_" + Columns + ">" + TableXLS.Rows[rows][Columns].ToString() + "</td>");
                                    else
                                        MHTFile.Append("<td align='right' style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.Denominator.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.Denominator.FontTemplate.BackColor) + ";font-size:" + this._TemplateStyle.Denominator.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Denominator.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.Denominator.FontTemplate.FontStyle + "' id=" + rows + "_" + Columns + ">" + TableXLS.Rows[rows][Columns].ToString() + "</td>");

                                    tableWidth += Convert.ToInt32(this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth);
                                    break;
                                case TableColumnType.RowHeader:
                                    if (Columns == 0 && this._IsSuppressDuplicateRows && TableXLS.Rows[rows][TotalXLSColumns].ToString() == RowType.DataRow.ToString())
                                    {
                                        MHTFile.Append("<td style='width:10px;border-right-width:0px;padding:4px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.RowSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.RowSetting.FontTemplate.BackColor) + ";font-size:" + this._TemplateStyle.RowSetting.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.RowSetting.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.RowSetting.FontTemplate.FontStyle + "' id=" + rows + "_" + Columns + "></td>");
                                    }
                                    else
                                    {
                                        MHTFile.Append("<td style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px");
                                        tableWidth += Convert.ToInt32(this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth);
                                        if (Columns == 1 && this._IsSuppressDuplicateRows)
                                        {
                                            MHTFile.Append(";border-left-width:0px");
                                        }
                                        // for handling of table report format
                                        if (!string.IsNullOrEmpty(this._TableReport.AreaSelection.AreaLevels))
                                        {
                                            string paddingLeft = string.Empty;
                                            string AreaLevelIndex = string.Empty;
                                            AreaLevelIndex = TableXLS.Rows[rows][TablePresentation.AREALEVEL].ToString();
                                            //For Report presentation
                                            if (TableXLS.Columns[Columns].ColumnName == Area.AreaName)
                                            {
                                                //apply the area level styles on the basic of AreaLevelFormatIndex
                                                if (Convert.ToInt32(TableXLS.Rows[rows][TablePresentation.AREALEVEL]) < TotalFormatAreaLevel)
                                                {
                                                    MHTFile.Append(";padding:4px;color:" + ColorTranslator.ToHtml(this._TableReport.LevelFormat[AreaLevelIndex].FontSetting.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TableReport.LevelFormat[AreaLevelIndex].FontSetting.BackColor) + ";font-family:" + this._TableReport.LevelFormat[AreaLevelIndex].FontSetting.FontName + ";font-size:" + this._TableReport.LevelFormat[AreaLevelIndex].FontSetting.FontSize + "px;height:" + this._TableReport.LevelFormat[AreaLevelIndex].FontSetting.RowHeight + "px;' id=" + rows + "_" + Columns + ">" + TableXLS.Rows[rows][Columns].ToString() + "</td>");
                                                }
                                                else
                                                {
                                                    MHTFile.Append(";padding:4px;padding-left:" + AreaLevelIndex + "0px;color:" + ColorTranslator.ToHtml(this._TableReport.LevelFormat[AreaLevelIndex].FontSetting.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TableReport.LevelFormat[AreaLevelIndex].FontSetting.BackColor) + ";font-family:" + this._TableReport.LevelFormat[AreaLevelIndex].FontSetting.FontName + ";font-size:" + this._TableReport.LevelFormat[AreaLevelIndex].FontSetting.FontSize + "px;height:" + this._TableReport.LevelFormat[AreaLevelIndex].FontSetting.RowHeight + "px;' id=" + rows + "_" + Columns + ">" + TableXLS.Rows[rows][Columns].ToString() + "</td>");
                                                }
                                            }
                                            //other than area name
                                            else
                                            {
                                                MHTFile.Append(";padding:4px;color:" + ColorTranslator.ToHtml(this._TableReport.LevelFormat[AreaLevelIndex].FontSetting.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TableReport.LevelFormat[AreaLevelIndex].FontSetting.BackColor) + ";font-family:" + this._TableReport.LevelFormat[AreaLevelIndex].FontSetting.FontName + ";font-size:" + this._TableReport.LevelFormat[AreaLevelIndex].FontSetting.FontSize + "px;height:" + this._TableReport.LevelFormat[AreaLevelIndex].FontSetting.RowHeight + "px;' id=" + rows + "_" + Columns + ">" + TableXLS.Rows[rows][Columns].ToString() + "</td>");
                                            }

                                        }
                                        else
                                        {
                                            MHTFile.Append(";padding:4px;font-size:" + this._TemplateStyle.RowSetting.FontTemplate.FontSize + "px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.RowSetting.FontTemplate.ForeColor));

                                            if (this._TemplateStyle.RowSetting.FontTemplate.FontStyle == FontStyle.Bold)
                                                MHTFile.Append(";font-weight:bold");
                                            else if (this._TemplateStyle.RowSetting.FontTemplate.FontStyle == FontStyle.Underline)
                                                MHTFile.Append(";text-decoration:underline");
                                            else
                                                MHTFile.Append(";font-style:" + this._TemplateStyle.RowSetting.FontTemplate.FontStyle);

                                            if (!(this._TemplateStyle.RowSetting.FontTemplate.ShowAlternateColor && !this._AddColor && TableXLS.Rows[rows][ROWTYPE].ToString() == RowType.DataRow.ToString()))
                                            {
                                                MHTFile.Append(";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.RowSetting.FontTemplate.BackColor));
                                            }
                                            if ((TableXLS.Rows[rows][TablePresentation.ROWTYPE].ToString() == RowType.DataRow.ToString()) && this.IsSuppressDuplicateRows)
                                            {
                                                string innerTdText = string.Empty;
                                                if (Columns < this.Fields.Rows.Count - 1)
                                                {
                                                    innerTdText = "&nbsp;";
                                                }
                                                else
                                                {
                                                    innerTdText = TableXLS.Rows[rows][Columns].ToString();
                                                }
                                                MHTFile.Append(";font-family:" + this._TemplateStyle.RowSetting.FontTemplate.FontName + "' id=" + rows + "_" + Columns + ">" + innerTdText + "</td>");
                                            }
                                            else
                                            {
                                                MHTFile.Append(";font-family:" + this._TemplateStyle.RowSetting.FontTemplate.FontName + "' id=" + rows + "_" + Columns + ">" + TableXLS.Rows[rows][Columns].ToString() + "</td>");
                                            }


                                            //MHTFile.Append(";font-family:" + this._TemplateStyle.RowSetting.FontTemplate.FontName + "' id=" + rows + "_" + Columns + ">" + TableXLS.Rows[rows][Columns].ToString() + "</td>");

                                        }
                                    }
                                    break;
                                case TableColumnType.Others:
                                    //MHTFile.Append("<td align='right' style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ContentSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ContentSetting.FontTemplate.BackColor) + ";font-size:" + this._TemplateStyle.ContentSetting.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.ContentSetting.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.ContentSetting.FontTemplate.FontStyle + "' id=" + rows + "_" + Columns + ">" + TableXLS.Rows[rows][Columns].ToString() + "</td>");

                                    if (this._TemplateStyle.ContentSetting.FontTemplate.FontStyle == FontStyle.Bold)
                                    {
                                        MHTFile.Append("<td align='right' style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ContentSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ContentSetting.FontTemplate.BackColor) + ";font-size:" + this._TemplateStyle.ContentSetting.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.ContentSetting.FontTemplate.FontName + ";font-weight:bold; id=" + rows + "_" + Columns + ">" + TableXLS.Rows[rows][Columns].ToString() + "</td>");
                                        tableWidth += Convert.ToInt32(this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth);
                                    }
                                    else if (this._TemplateStyle.ContentSetting.FontTemplate.FontStyle == FontStyle.Underline)
                                    {
                                        MHTFile.Append("<td align='right' style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ContentSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ContentSetting.FontTemplate.BackColor) + ";font-size:" + this._TemplateStyle.ContentSetting.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.ContentSetting.FontTemplate.FontName + ";text-decoration:underline; id=" + rows + "_" + Columns + ">" + TableXLS.Rows[rows][Columns].ToString() + "</td>");
                                        tableWidth += Convert.ToInt32(this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth);
                                    }
                                    else
                                    {
                                        MHTFile.Append("<td align='right' style='width:" + this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth + "px;padding:4px;color:" + ColorTranslator.ToHtml(this._TemplateStyle.ContentSetting.FontTemplate.ForeColor) + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.ContentSetting.FontTemplate.BackColor) + ";font-size:" + this._TemplateStyle.ContentSetting.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.ContentSetting.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.ContentSetting.FontTemplate.FontStyle + "' id=" + rows + "_" + Columns + ">" + TableXLS.Rows[rows][Columns].ToString() + "</td>");
                                        tableWidth += Convert.ToInt32(this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                MHTFile.Append("</tr>");
            }

            #endregion
            //Third Inner Table Closed -- Table for TableXLS
            MHTFile.Append("</Table>");


            #region "-- Render content of Footnotes, comments and sources and apply styles.


            //Set Footnotes and it's style
            if (this._TemplateStyle.Footnotes.Show && TableFootNote.Rows.Count > 0)
            {
                MHTFile.Append("<br />");
                MHTFile.Append("<br />");
                MHTFile.Append("<Table id='Footnotes' width='100%'>");
                MHTFile.Append("<tr><td>" + LanguageStrings.FootNotes + "</td></tr>");
                for (int rows = 0; rows < TableFootNote.Rows.Count; rows++)
                {
                    if (this._TemplateStyle.Footnotes.FontTemplate.FontStyle == FontStyle.Bold)
                        MHTFile.Append("<tr><td style='color:" + ColorTranslator.ToHtml(this._TemplateStyle.Footnotes.FontTemplate.ForeColor) + ";font-size:" + this._TemplateStyle.Footnotes.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Footnotes.FontTemplate.FontName + ";font-weight:bold;'>" + TableFootNote.Rows[rows][0].ToString().Replace("<", "&lt;").Replace(">", "&gt;") + "    " + TableFootNote.Rows[rows][1].ToString().Replace("<", "&lt;").Replace(">", "&gt;") + "</td></tr>");
                    else if (this._TemplateStyle.Footnotes.FontTemplate.FontStyle == FontStyle.Underline)
                        MHTFile.Append("<tr><td style='color:" + ColorTranslator.ToHtml(this._TemplateStyle.Footnotes.FontTemplate.ForeColor) + ";font-size:" + this._TemplateStyle.Footnotes.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Footnotes.FontTemplate.FontName + ";text-decoration:underline;'>" + TableFootNote.Rows[rows][0].ToString().Replace("<", "&lt;").Replace(">", "&gt;") + "    " + TableFootNote.Rows[rows][1].ToString().Replace("<", "&lt;").Replace(">", "&gt;") + "</td></tr>");
                    else
                        MHTFile.Append("<tr><td style='color:" + ColorTranslator.ToHtml(this._TemplateStyle.Footnotes.FontTemplate.ForeColor) + ";font-size:" + this._TemplateStyle.Footnotes.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Footnotes.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.Footnotes.FontTemplate.FontStyle + "'>" + TableFootNote.Rows[rows][0].ToString().Replace("<", "&lt;").Replace(">", "&gt;") + "    " + TableFootNote.Rows[rows][1].ToString().Replace("<", "&lt;").Replace(">", "&gt;") + "</td></tr>");
                }
                MHTFile.Append("</Table>");
            }

            //Set Comments and it's style
            if (this._TemplateStyle.Comments.Show && TableComments.Rows.Count > 0)
            {
                MHTFile.Append("<br />");
                MHTFile.Append("<br />");
                MHTFile.Append("<Table id='Comments'>");
                MHTFile.Append("<tr><td>" + LanguageStrings.Comments + "</td></tr>");
                for (int rows = 0; rows < TableComments.Rows.Count; rows++)
                {
                    //MHTFile.Append("<tr><td style='color:" + ColorTranslator.ToHtml(this._TemplateStyle.Comments.FontTemplate.ForeColor) + ";font-size:" + this._TemplateStyle.Comments.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Comments.FontTemplate.FontName + "'>" + TableComments.Rows[rows][0].ToString() + "    " + TableComments.Rows[rows][1].ToString() + "</td></tr>");
                    if (this._TemplateStyle.Comments.FontTemplate.FontStyle == FontStyle.Bold)
                        MHTFile.Append("<tr><td style='color:" + ColorTranslator.ToHtml(this._TemplateStyle.Comments.FontTemplate.ForeColor) + ";font-size:" + this._TemplateStyle.Comments.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Comments.FontTemplate.FontName + ";font-weight:bold;'>" + TableComments.Rows[rows][0].ToString() + "    " + TableComments.Rows[rows][1].ToString() + "</td></tr>");
                    else if (this._TemplateStyle.Comments.FontTemplate.FontStyle == FontStyle.Underline)
                        MHTFile.Append("<tr><td style='color:" + ColorTranslator.ToHtml(this._TemplateStyle.Comments.FontTemplate.ForeColor) + ";font-size:" + this._TemplateStyle.Comments.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Comments.FontTemplate.FontName + ";text-decoration:underline;'>" + TableComments.Rows[rows][0].ToString() + "    " + TableComments.Rows[rows][1].ToString() + "</td></tr>");
                    else
                        MHTFile.Append("<tr><td style='color:" + ColorTranslator.ToHtml(this._TemplateStyle.Comments.FontTemplate.ForeColor) + ";font-size:" + this._TemplateStyle.Comments.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Comments.FontTemplate.FontName + ";font-style:" + this._TemplateStyle.Comments.FontTemplate.FontStyle + ";'>" + TableComments.Rows[rows][0].ToString() + "    " + TableComments.Rows[rows][1].ToString() + "</td></tr>");

                }
                MHTFile.Append("</Table>");
            }

            //Set the Legends
            if (this._AddColor)
            {

                MHTFile.Append("<br />");
                MHTFile.Append("<br />");
                MHTFile.Append("<Table id='Legends'>");

                //Set style for Legends displayed at th bottom of TableXLS Sheet.
                for (int Count = 0; Count < this.Themes[0].Legends.Count; Count++)
                {
                    //if (this.TemplateStyle.Legends.ShowCaption==true && this.TemplateStyle.Legends.ShowRange==true)
                    //    MHTFile.Append("<tr><td width='30px' bgcolor='" + ColorTranslator.ToHtml(this.Themes[0].Legends[Count].Color) + "'></td><td>" + this.Themes[0].Legends[Count].Caption + "</td> <td>" + " " + this.Themes[0].Legends[Count].Range + "</td> </tr>");
                    //else if(this.TemplateStyle.Legends.ShowCaption==true)
                    //    MHTFile.Append("<tr><td width='30px' bgcolor='" + ColorTranslator.ToHtml(this.Themes[0].Legends[Count].Color) + "'></td><td>" + this.Themes[0].Legends[Count].Caption + "</td></tr>");
                    //else if(this.TemplateStyle.Legends.ShowRange==true)
                    //    MHTFile.Append("<tr><td width='30px' bgcolor='" + ColorTranslator.ToHtml(this.Themes[0].Legends[Count].Color) + "'></td><td>" + this.Themes[0].Legends[Count].Range + "</td></tr>");

                    if (this.TemplateStyle.Legends.ShowCaption == true && this.TemplateStyle.Legends.ShowRange == true)
                    {
                        if (this._TemplateStyle.Legends.FontTemplate.FontStyle == FontStyle.Bold)
                            MHTFile.Append("<tr><td width='30px' bgcolor='" + ColorTranslator.ToHtml(this.Themes[0].Legends[Count].Color) + "'></td><td style='color:" + ColorTranslator.ToHtml(this._TemplateStyle.Legends.FontTemplate.ForeColor) + ";font-size:" + this._TemplateStyle.Legends.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Legends.FontTemplate.FontName + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.Legends.FontTemplate.BackColor) + ";font-weight:bold;'>" + this.Themes[0].Legends[Count].Caption + "</td> <td style='color:" + ColorTranslator.ToHtml(this._TemplateStyle.Legends.FontTemplate.ForeColor) + ";font-size:" + this._TemplateStyle.Legends.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Legends.FontTemplate.FontName + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.Legends.FontTemplate.BackColor) + ";font-weight:bold;'>" + " " + this.Themes[0].Legends[Count].Range + "</td> </tr>");
                        else if (this._TemplateStyle.Legends.FontTemplate.FontStyle == FontStyle.Underline)
                            MHTFile.Append("<tr><td width='30px' bgcolor='" + ColorTranslator.ToHtml(this.Themes[0].Legends[Count].Color) + "'></td><td style='color:" + ColorTranslator.ToHtml(this._TemplateStyle.Legends.FontTemplate.ForeColor) + ";font-size:" + this._TemplateStyle.Legends.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Legends.FontTemplate.FontName + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.Legends.FontTemplate.BackColor) + ";text-decoration:underline;'>" + this.Themes[0].Legends[Count].Caption + "</td> <td style='color:" + ColorTranslator.ToHtml(this._TemplateStyle.Legends.FontTemplate.ForeColor) + ";font-size:" + this._TemplateStyle.Legends.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Legends.FontTemplate.FontName + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.Legends.FontTemplate.BackColor) + ";text-decoration:underline;'>" + " " + this.Themes[0].Legends[Count].Range + "</td> </tr>");
                        else
                            MHTFile.Append("<tr><td width='30px' bgcolor='" + ColorTranslator.ToHtml(this.Themes[0].Legends[Count].Color) + "'></td><td style='color:" + ColorTranslator.ToHtml(this._TemplateStyle.Legends.FontTemplate.ForeColor) + ";font-size:" + this._TemplateStyle.Legends.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Legends.FontTemplate.FontName + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.Legends.FontTemplate.BackColor) + ";font-style:" + this._TemplateStyle.Legends.FontTemplate.FontStyle + ";'>" + this.Themes[0].Legends[Count].Caption + "</td> <td style='color:" + ColorTranslator.ToHtml(this._TemplateStyle.Legends.FontTemplate.ForeColor) + ";font-size:" + this._TemplateStyle.Legends.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Legends.FontTemplate.FontName + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.Legends.FontTemplate.BackColor) + ";font-style:" + this._TemplateStyle.Legends.FontTemplate.FontStyle + ";'>" + " " + this.Themes[0].Legends[Count].Range + "</td> </tr>");
                    }
                    else if (this.TemplateStyle.Legends.ShowCaption == true)
                    {
                        if (this._TemplateStyle.Legends.FontTemplate.FontStyle == FontStyle.Bold)
                            MHTFile.Append("<tr><td width='30px' bgcolor='" + ColorTranslator.ToHtml(this.Themes[0].Legends[Count].Color) + "'></td><td style='color:" + ColorTranslator.ToHtml(this._TemplateStyle.Legends.FontTemplate.ForeColor) + ";font-size:" + this._TemplateStyle.Legends.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Legends.FontTemplate.FontName + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.Legends.FontTemplate.BackColor) + ";font-weight:bold;'>" + this.Themes[0].Legends[Count].Caption + "</td></tr>");
                        else if (this._TemplateStyle.Legends.FontTemplate.FontStyle == FontStyle.Bold)
                            MHTFile.Append("<tr><td width='30px' bgcolor='" + ColorTranslator.ToHtml(this.Themes[0].Legends[Count].Color) + "'></td><td style='color:" + ColorTranslator.ToHtml(this._TemplateStyle.Legends.FontTemplate.ForeColor) + ";font-size:" + this._TemplateStyle.Legends.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Legends.FontTemplate.FontName + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.Legends.FontTemplate.BackColor) + ";text-decoration:underline;'>" + this.Themes[0].Legends[Count].Caption + "</td></tr>");
                        else
                            MHTFile.Append("<tr><td width='30px' bgcolor='" + ColorTranslator.ToHtml(this.Themes[0].Legends[Count].Color) + "'></td><td style='color:" + ColorTranslator.ToHtml(this._TemplateStyle.Legends.FontTemplate.ForeColor) + ";font-size:" + this._TemplateStyle.Legends.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Legends.FontTemplate.FontName + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.Legends.FontTemplate.BackColor) + ";font-style:" + this._TemplateStyle.Legends.FontTemplate.FontStyle + ";'>" + this.Themes[0].Legends[Count].Caption + "</td></tr>");
                    }
                    else if (this.TemplateStyle.Legends.ShowRange == true)
                    {
                        if (this._TemplateStyle.Legends.FontTemplate.FontStyle == FontStyle.Bold)
                            MHTFile.Append("<tr><td width='30px' bgcolor='" + ColorTranslator.ToHtml(this.Themes[0].Legends[Count].Color) + "'></td><td style='color:" + ColorTranslator.ToHtml(this._TemplateStyle.Legends.FontTemplate.ForeColor) + ";font-size:" + this._TemplateStyle.Legends.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Legends.FontTemplate.FontName + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.Legends.FontTemplate.BackColor) + ";font-weight:bold;'>" + this.Themes[0].Legends[Count].Range + "</td></tr>");
                        else if (this._TemplateStyle.Legends.FontTemplate.FontStyle == FontStyle.Bold)
                            MHTFile.Append("<tr><td width='30px' bgcolor='" + ColorTranslator.ToHtml(this.Themes[0].Legends[Count].Color) + "'></td><td style='color:" + ColorTranslator.ToHtml(this._TemplateStyle.Legends.FontTemplate.ForeColor) + ";font-size:" + this._TemplateStyle.Legends.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Legends.FontTemplate.FontName + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.Legends.FontTemplate.BackColor) + ";text-decoration:underline;'>" + this.Themes[0].Legends[Count].Range + "</td></tr>");
                        else
                            MHTFile.Append("<tr><td width='30px' bgcolor='" + ColorTranslator.ToHtml(this.Themes[0].Legends[Count].Color) + "'></td><td style='color:" + ColorTranslator.ToHtml(this._TemplateStyle.Legends.FontTemplate.ForeColor) + ";font-size:" + this._TemplateStyle.Legends.FontTemplate.FontSize + "px;font-family:" + this._TemplateStyle.Legends.FontTemplate.FontName + ";background-color:" + ColorTranslator.ToHtml(this._TemplateStyle.Legends.FontTemplate.BackColor) + ";font-style:" + this._TemplateStyle.Legends.FontTemplate.FontStyle + ";'>" + this.Themes[0].Legends[Count].Range + "</td></tr>");
                    }

                }
                MHTFile.Append("</Table>");
            }

            if (this._ShowSource)
            {
                //Set sources and it's style
                if (this.DTSources.Rows.Count > 0)
                {
                    MHTFile.Append("<br />");
                    MHTFile.Append("<br />");
                    MHTFile.Append("<Table id='Sources' width='100%' style='font-size:8pt;font-family:Arial, Verdana, Helvetica, sans-serif'>");
                    MHTFile.Append("<tr><td>" + LanguageStrings.Source + "</td></tr>");
                    for (int rows = 0; rows < this.DTSources.Rows.Count; rows++)
                    {
                        MHTFile.Append("<tr><td>" + this.DTSources.Rows[rows][0].ToString() + "</td></tr>");
                    }
                    MHTFile.Append("</Table>");
                }
            }


            #endregion

            //First Inner Table Closed
            MHTFile.Append("</Table>");
            MHTFile.Append("</Div>");
            if (this._IsWizardMode)
            {
                MHTFile.Append("</div>");
            }
            if (this._IsWizardMode)
            {

                #region	"--	DataSheet --"

                if (this._IsLaunchedFromWizard == false)
                {
                    //DataSheet
                    //MHTFile.Append("<div id='DivData'	style='overflow:auto;width:690;height:100%;position:absolute;top:0px;left:1px'>");
                    MHTFile.Append("<div id='DivData' style='overflow:auto;width:100%;height:98%;top:0px;left:1px'>");
                    MHTFile.Append("<Table id=d1 width='100%' style='font-size:8pt;font-family:Arial, Verdana, Helvetica, sans-serif' cellSpacing=1 cellPadding=1 border=0>");

                    for (int rows = 0; rows < this.DTDataSheet.Rows.Count; rows++)
                    {
                        MHTFile.Append("<tr>");
                        for (int Columns = 0; Columns < this.DTDataSheet.Columns.Count; Columns++)
                        {
                            if (rows == 0)
                            {
                                MHTFile.Append("<td	noWrap style='color:Black;padding-left:1px;border-bottom:#bdbece 2px solid;height:22px;background-color:#f9fafd'>" + this.DTDataSheet.Rows[rows][Columns] + "</td>");
                            }
                            else
                            {
                                MHTFile.Append("<td>" + this.DTDataSheet.Rows[rows][Columns] + "</td>");
                            }
                        }
                        MHTFile.Append("</tr>");
                    }
                    MHTFile.Append("</Table>");
                    MHTFile.Append("</div>");

                }

                #endregion

                #region "-- Start for the Graph image --"
                if (this._PresentationType == Presentation.PresentationType.Graph)
                {
                    MHTFile.Append("<div id='DivGraph' style='overflow:auto;width:100%;height:98%;top:0px;left:1px;border:1px #DDDDDD Solid;'>");
                    MHTFile.Append("<Table id=G1 style='width:100%;height:100%;font-size:8pt;font-family:Arial, Verdana, Helvetica, sans-serif;bgcolor:red' cellSpacing=1 cellPadding=1 border=0>");
                    MHTFile.Append("<tr>");
                    //MHTFile.Append("<td><img id='graphimage' src='" + this._GraphImagePath +"'/> </td>");
                    MHTFile.Append("<td style='padding-left:08px;' valign='top'>" + this._GraphImagePath + "</td>");
                    MHTFile.Append("</tr>");
                    MHTFile.Append("</Table>");
                    MHTFile.Append("</div>");
                }

                #endregion


                #region "-- Source Sheet --"
                if (this._IsLaunchedFromWizard == false)
                {
                    MHTFile.Append("<div id='DivSource' style='overflow:auto;width:100%;height:98%;top:0px;left:0px;'>");
                    MHTFile.Append("<Table width='100%' style='font-size:8pt;font-family:Arial, Verdana, Helvetica, sans-serif'><tr><td>" + this.LanguageStrings.Source + " : " + DIConnection.ConnectionStringParameters.DbName + "</td></tr></Table>");
                    MHTFile.Append("<br />");
                    MHTFile.Append("<Table style='font-size:8pt;font-family:Arial, Verdana, Helvetica, sans-serif' id=d2 cellSpacing=1 cellPadding=0 border=0>");
                    for (int rows = 0; rows < this.DTSources.Rows.Count; rows++)
                    {
                        MHTFile.Append("<tr>");
                        for (int Columns = 0; Columns < this.DTSources.Columns.Count; Columns++)
                        {
                            MHTFile.Append("<td>" + this.DTSources.Rows[rows][Columns] + "</td>");
                        }
                        MHTFile.Append("</tr>");
                    }
                    MHTFile.Append("</Table>");
                    MHTFile.Append("</div>");
                }

                #endregion

                MHTFile.Append("</td></tr>");
                //        FONT-SIZE: 8pt;
                //FONT-FAMILY: Arial, Verdana, Helvetica, sans-serif
                #region "-- Make the Nagigation bar --"

                if (this._IsLaunchedFromWizard == false)
                {
                    MHTFile.Append("<tr><td><TABLE width='100%' style='font-size:8pt;font-family:Arial, Verdana, Helvetica, sans-serif' cellSpacing=0 cellPadding=0 width=100% border=0>");
                    MHTFile.Append("<TR>");

                    // Graph image Tab
                    if (this._PresentationType == Presentation.PresentationType.Graph)
                    {
                        MHTFile.Append("<TD id=V00 style='BORDER-RIGHT:#dddddd 1px solid;BORDER-TOP:#dddddd 1px solid;BORDER-LEFT:#dddddd 1px solid;BORDER-BOTTOM:#dddddd 1px solid;PADDING-RIGHT: 8px; PADDING-LEFT: 8px; CURSOR: hand' onclick=showView('DivGraph') noWrap>" + LanguageStrings.Chart + "</TD>");
                    }
                    MHTFile.Append("<TD id=V0 style='BORDER-RIGHT:#dddddd 1px solid;BORDER-TOP:#dddddd 1px solid;BORDER-LEFT:#dddddd 1px solid;BORDER-BOTTOM:#dddddd 1px solid;PADDING-RIGHT: 8px; PADDING-LEFT: 8px; CURSOR: hand' onclick=showView('DivTable') noWrap>" + LanguageStrings.Table + "</TD>");
                    MHTFile.Append("<TD id=V1 style='BORDER-RIGHT:#dddddd 1px solid;BORDER-TOP:#dddddd 1px solid;BORDER-LEFT:#dddddd 1px solid;BORDER-BOTTOM:#dddddd 1px solid;PADDING-RIGHT: 8px; PADDING-LEFT: 8px; CURSOR: hand' onclick=showView('DivData') noWrap>" + LanguageStrings.Data + "</TD>");
                    MHTFile.Append("<TD id=V2 style='BORDER-RIGHT:#dddddd 1px solid;BORDER-TOP:#dddddd 1px solid;BORDER-LEFT:#dddddd 1px solid;BORDER-BOTTOM:#dddddd 1px solid;PADDING-RIGHT: 8px; PADDING-LEFT: 8px; CURSOR: hand' onclick=showView('DivSource') noWrap>" + LanguageStrings.Source + "</TD>");
                    MHTFile.Append("<TD width=100%></TD></TR></TABLE></td></tr>");
                }

                #endregion

            }

            //Outer table closed
            MHTFile.Append("</table>");

            #region "-- Set default page --"

            if (this._IsWizardMode && this._IsLaunchedFromWizard == false)
            {
                // Graph image Tab
                if (this._PresentationType == Presentation.PresentationType.Graph)
                {
                    MHTFile.Append("<script>showView('DivGraph')</script>");
                }
                else
                {
                    MHTFile.Append("<script>showView('DivTable');</script>");
                }
            }

            #endregion


            MHTFile.Append("<a href='#' title='" + tableWidth / (dataRowsCount / TotalXLSColumns) + "' id='tableWidth'></a>");

            MHTFile.Append("</BODY>");

            MHTFile.Append("<script>");
            MHTFile.Append("function pageHeight(){return window.innerHeight != null? window.innerHeight: document.documentElement && document.documentElement.clientHeight ? document.documentElement.clientHeight:document.body != null? document.body.clientHeight:null;}");
            MHTFile.Append("function SetDivTblHeight(){if(document.getElementById('DivTable')){document.getElementById('DivTable').style.height =  pageHeight() - 40 + 'px';}}");
            MHTFile.Append("SetDivTblHeight();");
            MHTFile.Append("</script>");

            MHTFile.Append("</HTML>");

            #region	"--	Save MHT --"
            if (this._IsTPWizardMode == true)
            {
                this._TableHTM = MHTFile.ToString();
            }
            else
            {
                StreamWriter SWriter = null;
                try
                {
                    SWriter = new StreamWriter(PresentationXLSFilePath);
                    SWriter.Write(MHTFile.ToString());
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    SWriter.Close();
                    SWriter.Dispose();
                }
            }



            #endregion

            #endregion

        }
        private SpreadsheetGear.HAlign MapHAlignWithSpreadsheetGear(StringAlignment alignment)
        {
            SpreadsheetGear.HAlign Retval = SpreadsheetGear.HAlign.Center;
            try
            {
                switch (alignment)
                {
                    case StringAlignment.Center:
                        Retval = SpreadsheetGear.HAlign.Center;
                        break;
                    case StringAlignment.Far:
                        Retval = SpreadsheetGear.HAlign.Right;
                        break;
                    case StringAlignment.Near:
                        Retval = SpreadsheetGear.HAlign.Left;
                        break;
                    default:
                        break;
                }

            }
            catch (Exception)
            {
            }
            return Retval;
        }

        /// <summary>
        /// Generates the datatable for ThemeStates
        /// </summary>
        /// <param name="ThmStats">Theme State</param>
        /// <param name="dr">data row</param>
        /// <param name="DTFrequency"> Frequency data table</param>
        /// <returns>datatable</returns>
        private DataTable GenerateThemeStateTable(Theme ThmStats, DataTable DTFrequency)
        {
            DataRow dr;
            dr = DTFrequency.NewRow();
            dr[0] = ThmStats.Indicator;
            dr[ROWTYPE] = RowType.IUSRow;
            DTFrequency.Rows.Add(dr);

            dr = DTFrequency.NewRow();
            dr[0] = MINIMUM;
            dr[1] = ThmStats.Minimum.ToString();
            dr[ROWTYPE] = RowType.DataRow;
            DTFrequency.Rows.Add(dr);

            dr = DTFrequency.NewRow();
            dr[0] = MAXIMUM;
            dr[1] = ThmStats.Maximum.ToString();
            dr[ROWTYPE] = RowType.DataRow;
            DTFrequency.Rows.Add(dr);

            dr = DTFrequency.NewRow();
            dr[0] = CAPTION;
            dr[1] = FROM;
            dr[2] = TO;
            dr[3] = RANGE;
            dr[4] = COUNT;
            dr[ROWTYPE] = RowType.TableHeaderRow;
            DTFrequency.Rows.Add(dr);

            //Make legends information available in the datatable
            foreach (Legend l in ThmStats.Legends)
            {
                dr = DTFrequency.NewRow();
                dr[0] = l.Caption;
                dr[1] = l.RangeFrom.ToString();
                dr[2] = l.RangeTo.ToString();
                dr[3] = l.Range;
                dr[4] = l.Count.ToString();
                dr[ROWTYPE] = RowType.DataRow;
                DTFrequency.Rows.Add(dr);
            }
            dr = DTFrequency.NewRow();
            dr[ROWTYPE] = RowType.EmptyRow;
            DTFrequency.Rows.Add(dr);
            dr = DTFrequency.NewRow();
            dr[ROWTYPE] = RowType.EmptyRow;
            DTFrequency.Rows.Add(dr);
            dr = DTFrequency.NewRow();
            dr[ROWTYPE] = RowType.EmptyRow;
            DTFrequency.Rows.Add(dr);
            return DTFrequency;
        }

        /// <summary>
        /// Prepares add saves the Frequency excell sheet 
        /// </summary>
        /// <param name="FrequencyOutputFolder">Folder path to save the excell sheet</param>
        /// <param name="FrequencyFileName">File name fir excell sheet</param>
        /// <param name="IUSNId">IUSNId</param>
        public void ProcessExcellFileFrequency(string FrequencyOutputFolder, string FrequencyFileName, string IUSNId)
        {
            DataTable DTFrequency = new DataTable();
            string FileName = string.Empty;
            string FilePath = string.Empty;
            int sheetIndex = 0;
            double left = 325.0;
            double top = 64.0;
            double NewTop = 0;
            int ThemeIndex = 0;
            int MainThemeIndex = 0;
            int Columns = 0;
            int UserRowIndex = 0;
            int Margin = 3;
            DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper.DIExcel ExcelFile = null;
            string[,] TableXLSArray = null;
            string[,] TableSourcesArray = null;
            string[,] TableDataSheetArray = null;
            DataTable DTSource = null;

            for (int ColCount = 0; ColCount <= 5; ColCount++)
            {
                DTFrequency.Columns.Add("");
            }

            //Assign the column names to data table DTFrequency
            DTFrequency.Columns[0].ColumnName = CAPTION;
            DTFrequency.Columns[1].ColumnName = FROM;
            DTFrequency.Columns[2].ColumnName = TO;
            DTFrequency.Columns[3].ColumnName = RANGE;
            DTFrequency.Columns[4].ColumnName = COUNT;
            DTFrequency.Columns[5].ColumnName = ROWTYPE;


            //Loop through each frequecncy and prepare XLS sheet            
            foreach (Theme ThmStats in this.FrequencyThemes)
            {
                if (IUSNId.Trim() != string.Empty)
                {
                    if (ThmStats.IndicatorNId == IUSNId)
                    {
                        DTFrequency = this.GenerateThemeStateTable(ThmStats, DTFrequency);
                        break;
                    }
                }
                else
                {
                    DTFrequency = this.GenerateThemeStateTable(ThmStats, DTFrequency);
                }
                ThemeIndex += 1;
            }
            //Generate name of the Excel File
            if (FrequencyFileName.Length > 0)
            {
                FileName = FrequencyFileName;
            }
            else
            {
                if (this.PresentationOutputType == PresentationOutputType.ExcellSheet)
                {
                    FileName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day + DateTime.Now.TimeOfDay.Hours.ToString() + DateTime.Now.TimeOfDay.Minutes.ToString() + DateTime.Now.TimeOfDay.Seconds.ToString() + DICommon.FileExtension.Excel;
                }
                else
                {
                    FileName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day + DateTime.Now.TimeOfDay.Hours.ToString() + DateTime.Now.TimeOfDay.Minutes.ToString() + DateTime.Now.TimeOfDay.Seconds.ToString() + DICommon.FileExtension.HTML;
                }
            }
            FilePath = Path.Combine(FrequencyOutputFolder, FileName);

            if (!Directory.Exists(FrequencyOutputFolder))
            {
                Directory.CreateDirectory(FrequencyOutputFolder);
            }
            if (File.Exists(FilePath) && PresentationType == Presentation.PresentationType.Table)
            {
                File.Delete(FilePath);
            }

            //Array initialization for DTFrequency table
            Margin = 3;
            TableXLSArray = new string[DTFrequency.Rows.Count + Margin, DTFrequency.Columns.Count - 1];

            //Insert DTFrequency cells into the Array one by one.            
            foreach (DataRow DRowDTFrequency in DTFrequency.Rows)
            {
                for (Columns = 0; Columns < DTFrequency.Columns.Count - 1; Columns++)
                {
                    if (DTFrequency.Columns[Columns].ColumnName == RANGE)
                    {
                        //Set "'" in front of range values so that it doesn't get converted into Date format
                        TableXLSArray[Margin, Columns] = "'" + DRowDTFrequency[Columns].ToString();
                    }
                    else
                    {
                        TableXLSArray[Margin, Columns] = DRowDTFrequency[Columns].ToString();
                    }
                }
                Margin++;
            }
            ExcelFile = this.ConfigureExcelSettings(FilePath, LanguageStrings.FrequencyTable);

            if (this._PresentationType != Presentation.PresentationType.Graph)
            {
                Color[] ColorPelatte = GetColorPelette();
                ExcelFile.SetWorkbookColorPalette(ColorPelatte);
            }
            ExcelFile.SetRangeFormatType(sheetIndex, 0, 0, DTFrequency.Rows.Count, DTFrequency.Columns.Count - 1, SpreadsheetGear.NumberFormatType.General);

            try
            {
                //-- Clean worksheets.
                for (int i = 0; i < ExcelFile.ActivateWorkbook().Worksheets.Count; i++)
                {
                    ExcelFile.GetUsedRange(i).Clear();
                }
            }
            catch (Exception)
            {
            }
            Margin = 3;
            ExcelFile.SetArrayValuesIntoSheet(0, 0, 0, DTFrequency.Rows.Count, DTFrequency.Columns.Count - 1, TableXLSArray);
            ExcelFile.SetColumnWidth(sheetIndex, 17.0, 0, 0, DTFrequency.Rows.Count, 0);
            ExcelFile.SetColumnWidth(sheetIndex, 8.0, 0, 1, DTFrequency.Rows.Count, 1);
            ExcelFile.SetColumnWidth(sheetIndex, 8.0, 0, 2, DTFrequency.Rows.Count, 2);
            ExcelFile.SetColumnWidth(sheetIndex, 12.0, 0, 3, DTFrequency.Rows.Count, 3);
            ExcelFile.SetColumnWidth(sheetIndex, 6.0, 0, 4, DTFrequency.Rows.Count, 4);

            ExcelFile.SetHorizontalAlignment(sheetIndex, 0, 1, DTFrequency.Rows.Count, DTFrequency.Columns.Count - 1, SpreadsheetGear.HAlign.Right);
            ExcelFile.GetRangeFont(sheetIndex, 0, 0, DTFrequency.Rows.Count, DTFrequency.Columns.Count - 1).Name = this._UserPreference.Language.FontName;
            ExcelFile.GetRangeFont(sheetIndex, 0, 0, DTFrequency.Rows.Count, DTFrequency.Columns.Count - 1).Size = this._UserPreference.Language.FontSize;
            ExcelFile.Save();
            if (IUSNId.Trim() != string.Empty)
            {
                MainThemeIndex = ThemeIndex;
            }

            for (int RowIndex = 0; RowIndex < DTFrequency.Rows.Count; RowIndex++)
            {
                if (DTFrequency.Rows[RowIndex][TablePresentation.ROWTYPE].ToString() == RowType.IUSRow.ToString())
                {
                    top += NewTop;
                    NewTop = 0;
                    ExcelFile.MergeCells(sheetIndex, RowIndex + Margin, 0, RowIndex, 4);
                    this.SetExcelRangeFont(ExcelFile.GetCellFont(sheetIndex, RowIndex + Margin, 0), FontStyle.Bold);
                    if (string.IsNullOrEmpty(IUSNId))
                    {
                        ExcelFile.InsertChart(sheetIndex, RowIndex + Margin + 3, 3, RowIndex + Margin + 3 + this.FrequencyThemes[MainThemeIndex].Legends.Count, 4, this.FrequencyThemes[MainThemeIndex].Indicator, left, top, 340.0, 120.0, MainThemeIndex, false);
                    }
                    else
                    {
                        ExcelFile.InsertChart(sheetIndex, RowIndex + Margin + 3, 3, RowIndex + Margin + 3 + this.FrequencyThemes[MainThemeIndex].Legends.Count, 4, this.FrequencyThemes[MainThemeIndex].Indicator, left, top, 340.0, 120.0, 0, false);
                    }
                    MainThemeIndex++;
                }
                if (DTFrequency.Rows[RowIndex][TablePresentation.ROWTYPE].ToString() == RowType.TableHeaderRow.ToString())
                {
                    this.SetExcelRangeFont(ExcelFile.GetRangeFont(sheetIndex, Margin + RowIndex, 0, Margin + RowIndex, DTFrequency.Columns.Count - 2), FontStyle.Bold);

                    ExcelFile.SetHorizontalAlignment(sheetIndex, Margin + RowIndex, 0, Margin + RowIndex, 0, SpreadsheetGear.HAlign.Left);
                    ExcelFile.SetHorizontalAlignment(sheetIndex, Margin + RowIndex, 1, Margin + RowIndex, 2, SpreadsheetGear.HAlign.Right);
                    ExcelFile.SetHorizontalAlignment(sheetIndex, Margin + RowIndex, 3, Margin + RowIndex, 3, SpreadsheetGear.HAlign.Center);
                    ExcelFile.SetHorizontalAlignment(sheetIndex, Margin + RowIndex, 4, Margin + RowIndex, 4, SpreadsheetGear.HAlign.Right);

                    ExcelFile.SetRangeColor(sheetIndex, Margin + RowIndex, 0, Margin + RowIndex, DTFrequency.Columns.Count - 2, Color.Black, Color.LightGray);
                }
                NewTop += 15;
                UserRowIndex = RowIndex;
            }

            //-- Set the blank cell value to get the valid used space
            ExcelFile.SetCellValue(sheetIndex, UserRowIndex + Margin, 4 + 8, ".");


            DTSource = this.DIDataView.Sources;
            //Source Array Containing Data.			
            TableSourcesArray = new string[DTSource.Rows.Count, 1];
            //Insert Sources into data array. Sources will be shown on sources WorkSheet. 
            Margin = 0;
            foreach (DataRow DRowDTSources in DTSource.Rows)
            {
                TableSourcesArray[Margin, 0] = "   " + DRowDTSources[IndicatorClassifications.ICName].ToString();
                Margin++;
            }

            #region "-- Sources WorkSheet --"

            sheetIndex = 2;
            ExcelFile.SetCellValue(sheetIndex, 0, 0, this.LanguageStrings.Source);
            ExcelFile.SetCellValue(sheetIndex, 0, 2, DIConnection.ConnectionStringParameters.DbName);

            if (TableSourcesArray.GetLength(0) == 1)
            {
                ExcelFile.SetArrayValuesIntoSheet(sheetIndex, 0, 0, DTSource.Rows.Count, 2, TableSourcesArray);
            }
            else
            {
                ExcelFile.SetArrayValuesIntoSheet(sheetIndex, 0, 0, DTSource.Rows.Count, 1, TableSourcesArray);
            }


            #endregion

            #region	"--	Data WorkSheet --"

            //DataArray Containing Data.
            if (this.DTDataSheet != null)
            {
                TableDataSheetArray = new string[this.DTDataSheet.Rows.Count, this.DTDataSheet.Columns.Count];
                //Insert DTDataSheet into array which are shown on Data worksheet.
                Margin = 0;
                foreach (DataRow DRowDTDataSheet in this.DTDataSheet.Rows)
                {
                    for (Columns = 0; Columns < this.DTDataSheet.Columns.Count; Columns++)
                    {
                        //- Prefix "'" before time periods so that they are considered as textual values inside excel
                        if (DTDataSheet.Columns[Columns].ColumnName == Timeperiods.TimePeriod)
                        {
                            TableDataSheetArray[Margin, Columns] = "'" + DRowDTDataSheet[Columns].ToString();
                        }
                        else
                        {
                            TableDataSheetArray[Margin, Columns] = DRowDTDataSheet[Columns].ToString();
                        }
                    }
                    Margin++;
                }

                sheetIndex = 1;
                //Pass Array containing TableXLS values to prepare XLS Worksheet.
                ExcelFile.SetRangeFormatType(sheetIndex, 1, 0, this.DTDataSheet.Rows.Count + 1, this.DTDataSheet.Columns.Count, SpreadsheetGear.NumberFormatType.Text);
                ExcelFile.SetArrayValuesIntoSheet(sheetIndex, 1, 0, this.DTDataSheet.Rows.Count + 1, this.DTDataSheet.Columns.Count, TableDataSheetArray);

            }

            #endregion

            ExcelFile.Save();
            ExcelFile.Close();
        }

        /// <summary>
        /// Prepares Frequency MHT file
        /// </summary>
        /// <param name="FrequencyOutputFolder">Folder path to save the excell sheet</param>
        /// <param name="FrequencyFileName">File name fir excell sheet</param>
        /// <param name="IUSNId">IUSNId</param>
        public void ProcessExcellFileFrequencyMHT(string xlsFileNameWithPath, string FrequencyOutputFolder, string FrequencyFileName, string IUSNId)
        {
            try
            {
                const string ICIUSNAME = "IUSNAME";
                string ImageFileName = string.Empty;
                StringBuilder MHTFile = new StringBuilder();
                DataTable IUSDataTable = this.DistinctIUS;
                String CurrentTimeStamp = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.TimeOfDay.Hours.ToString() + DateTime.Now.TimeOfDay.Minutes.ToString() + DateTime.Now.TimeOfDay.Seconds.ToString();
                int IUSNid;
                int Index = 0;
                MHTFile.Append("<table width='100%'>");

                foreach (DataRow Row in IUSDataTable.Rows)
                {
                    // First 50% TD containing the legends table
                    MHTFile.Append("<tr><td style='width:50%'>");
                    // Legends Table starting
                    MHTFile.Append("<table width='100%' cellspacing='0' cellpadding='0' border='0' style='border-width:1px;border-color:#000000;'>");
                    IUSNid = Convert.ToInt32(Row[IndicatorClassificationsIUS.IUSNId]);
                    MHTFile.Append("<tr><td colspan='2' style='padding-left:2px;'><font style='font-family:Verdana;font-size:12px;font-weight:bold;'>" + Row[ICIUSNAME].ToString() + "</font></td></tr>");
                    MHTFile.Append("<tr><td style='padding-left:2px;'><font style='font-family:Verdana;font-size:12px;'>" + MINIMUM.ToString() + "</font></td><td style='padding-left:2px;'><font style='font-family:Verdana;font-size:12px;'>" + this.FrequencyThemes[Index].Minimum.ToString() + "</font></td></tr>");
                    MHTFile.Append("<tr><td style='padding-left:2px;'><font style='font-family:Verdana;font-size:12px;'>" + MAXIMUM.ToString() + "</font></td><td style='padding-left:2px;'><font style='font-family:Verdana;font-size:12px;'>" + this.FrequencyThemes[Index].Maximum.ToString() + "</font></td></tr>");

                    MHTFile.Append("<tr><td colspan='2'>");
                    MHTFile.Append("<table width='100%' cellpadding='0' cellspacing='0'>");
                    //Build the Header row for the Legends table
                    MHTFile.Append("<tr style='height:20px;background-color:#0C6EB1;color:#ffffff;'><td style='padding-left:4px;'><font style='font-family:Verdana;font-size:12px;font-weight:bold;'>" + DILanguage.GetLanguageString("CAPTION") + "</font></td><td align='right' style='padding-left:4px;'><font style='font-family:Verdana;font-size:12px;font-weight:bold;'>" + DILanguage.GetLanguageString("RANGE") + "</font></td><td align='right' style='padding-right:4px;'><font style='font-family:Verdana;font-size:12px;font-weight:bold;'>" + DILanguage.GetLanguageString("FROM") + "</font></td><td align='right' style='padding-right:4px;'><font style='font-family:Verdana;font-size:12px;font-weight:bold;'>" + DILanguage.GetLanguageString("TO") + "</font></td><td align='right' style='padding-right:4px;'><font style='font-family:Verdana;font-size:12px;font-weight:bold;'>" + DILanguage.GetLanguageString("COUNT") + "</font></td></tr>");

                    //Build the legends table
                    foreach (DevInfo.Lib.DI_LibBAL.UI.Presentations.Common.TableMap.Legend FrequencyLegend in this.FrequencyThemes[Index].Legends)
                    {
                        //Build the legends row
                        MHTFile.Append("<tr><td><font style='font-family:Verdana;font-size:12px;'>" + FrequencyLegend.Caption + "</font></td><td align='right'><font style='font-family:Verdana;font-size:12px;'>" + FrequencyLegend.Range + "</font></td><td align='right'><font style='font-family:Verdana;font-size:12px;'>" + FrequencyLegend.RangeFrom + "</font></td><td align='right'><font style='font-family:Verdana;font-size:12px;'>" + FrequencyLegend.RangeTo + "</font></td><td align='right'><font style='font-family:Verdana;font-size:12px;'>" + FrequencyLegend.Count + "</font></td></tr>");
                    }

                    MHTFile.Append("</table>");
                    MHTFile.Append("</td></tr>");
                    MHTFile.Append("</table>");
                    // End of first 50% TD
                    MHTFile.Append("</td>");

                    // Second 50% TD containing the Graph image
                    MHTFile.Append("<td style='width:50%'>");

                    ImageFileName = CurrentTimeStamp + Index + ".png";
                    GenerateHistogramImage(xlsFileNameWithPath, 160, 350, Index).Save(FrequencyOutputFolder + "\\" + ImageFileName);
                    MHTFile.Append("<img src='" + ImageFileName + "' />");
                    // End of second 50% TD
                    MHTFile.Append("</td></tr>");
                    MHTFile.Append("<tr style='height:8px;'><td>&nbsp;</td></tr>");
                    Index += 1;
                }
                MHTFile.Append("</table>");
                this._TableFrequencyHTM = MHTFile.ToString();

                StreamWriter SWriter = null;
                try
                {
                    SWriter = new StreamWriter(FrequencyOutputFolder + "\\" + FrequencyFileName);
                    SWriter.Write(MHTFile.ToString());
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    SWriter.Close();
                    SWriter.Dispose();
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Prepares XLS file with TableXLS content,Footnotes, Notes and with other required Values.
        /// </summary>
        /// <param name="TableXLS">DataTable that holds the actual XLS output</param>
        /// <param name="TableFootNote">DataTable containing FootNote valus</param>
        /// <param name="TableComments">DataTable containing comments values</param>
        /// <param name="XlsFilePath">FilePath with File name where XLS file will be saved</param>
        /// <param name="TableXLSArray">Two dimentional array containing TableXLS Data.</param>
        /// <param name="TableFootNoteArray">Two dimentional array containing Footnote Data.</param>
        /// <param name="TableCommentsArray">Two dimentional array containing comments Data.</param>
        /// <param name="TableSourcesArray">Two dimentional array containing sources Data.</param>
        /// <param name="TableDataSheetArray">Two dimentional array containing Data to be displayes on Data Worksheet.</param>
        /// <param name="totalRows">TotalRows required in TableXLS with the margin required at top</param>
        /// <param name="totalColumns">Total DataColumns of TableXLS</param>
        private void ProcessExcelFile(DataTable TableXLS, DataTable TableFootNote, DataTable TableComments, string PresentationXLSFilePath, string[,] TableXLSArray, string[,] TableFootNoteArray, string[,] TableCommentsArray, string[,] TableSourcesArray, string[,] TableDataSheetArray, int totalRows, int totalColumns)
        {
            #region "-- Variable declaration and ExcelSheet iniialization
            int ColumnArrangementColumns;
            DataTable DTIndicator = null;
            DataTable DTIndicatorClassification = null;
            DataTable DTArea = null;
            DataTable DTUnit = null;
            DataTable DTSubgroup = null;
            //If DataValue column is present

            // set the ColumnArrangementColumns =1, in case of graph presentation tpye
            if (this._PresentationType == Presentation.PresentationType.Graph)
            {
                ColumnArrangementColumns = 1;
            }
            else
            {
                // set the ColumnArrangementColumns 
                if (this._ColumnArrangementTable.Columns.Count > 2)
                {
                    ColumnArrangementColumns = (this._ColumnArrangementTable.Columns.Count - 2) / 2;
                }
                else  //If DataValue column is present then assign 0
                {
                    ColumnArrangementColumns = 1;
                }
            }

            int XLSStartRow = TitleSubTitleMargin;
            int XLSRows = TableXLS.Rows.Count;
            this.TotalXLSRows = TableXLSArray.GetLength(0);
            int TotalXLSColumns = TableXLS.Columns.Count - (this.ExtraColumnCount + 1 + this.MaxMinAreaLevelDiff);	 //2 for rowtype and row count columns. Leave One as index starts from 0 in spreadsheet
            int StartRow = 0;
            int One = 1;
            int sheetIndex = this._XLSSheetIndex;
            int TotalTableFootNoteRows = 0;
            int TotalTableFootNoteColumns = 0;
            int TotalTableCommentsRows = 0;
            int TotalTableCommentsColumns = 0;
            int BlankRows = TitleSubTitleMargin;
            int Counter = 0;
            int Rows = 0;
            Color borderColor = Color.Gray;
            string SourceColumnHeader = string.Empty;

            if (this._TemplateStyle.Footnotes.Show)
            {
                if (this.FootnoteInLine != FootNoteDisplayStyle.InlineWithData)
                {
                    TotalTableFootNoteRows = TableFootNote.Rows.Count;
                    TotalTableFootNoteColumns = TableFootNote.Columns.Count;
                }
            }
            if (this._TemplateStyle.Comments.Show)
            {
                TotalTableCommentsRows = TableComments.Rows.Count;
                TotalTableCommentsColumns = TableComments.Columns.Count;
            }


            DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper.DIExcel ExcelFile = this.ConfigureExcelSettings(PresentationXLSFilePath, LanguageStrings.Table);

            if (this._PresentationType != Presentation.PresentationType.Graph)
            {
                Color[] ColorPelatte = GetColorPelette();
                ExcelFile.SetWorkbookColorPalette(ColorPelatte);
            }

            try
            {
                //-- Clean worksheets.
                for (int i = 0; i < ExcelFile.ActivateWorkbook().Worksheets.Count; i++)
                {
                    ExcelFile.GetUsedRange(i).Clear();
                }
            }
            catch (Exception)
            {
            }


            #endregion

            #region "-- Table WorkSheet -- "

            #region " Set arrayLists of TableXLS,Footnotes,Comments and Sources into Excell sheet --"

            ExcelFile.Save();
            //Set the DataTableRowCount;
            this._DataTableRowCount = TableXLS.Rows.Count;
            this._DataTableColumnCount = totalColumns;

            // Apply Text Format on all cells in all cases in case of Table. 
            // If Formatting or Decimal Values are on, then will be handled in the code below
            if (this._PresentationType == Presentation.PresentationType.Table || this._PresentationType == Presentation.PresentationType.FrequencyTable)
            {
                ExcelFile.SetRangeFormatType(sheetIndex, TitleSubTitleMargin, this._Fields.Rows.Count, TableXLS.Rows.Count + TitleSubTitleMargin, TotalXLSColumns, SpreadsheetGear.NumberFormatType.Text);
            }

            // Format Excell sheet to text
            if (this._PresentationType == Presentation.PresentationType.Graph && !this.UserPreference.General.ShowExcel)
            {
                ExcelFile.SetRangeFormatType(sheetIndex, 0, 0, TableXLS.Rows.Count + TitleSubTitleMargin - 1, TableXLS.Columns.Count - 5, SpreadsheetGear.NumberFormatType.Text);
            }
            else
            {
                ExcelFile.SetRangeFormatType(sheetIndex, TitleSubTitleMargin, 0, TableXLS.Rows.Count + TitleSubTitleMargin - 1, this._Fields.Rows.Count - 1, SpreadsheetGear.NumberFormatType.Text);
            }




            // Dump Array into Spreadsheet
            ExcelFile.SetArrayValuesIntoSheet(sheetIndex, 0, 0, totalRows, totalColumns, TableXLSArray);

            //Pass Array containing Foototes values to show Footnotes in XLS Worksheet.            
            if (TotalTableFootNoteRows > 0)		//Set the header for Footnote.
            {
                if (this.FootnoteInLine == FootNoteDisplayStyle.Separate)
                {
                    ExcelFile.SetCellValue(sheetIndex, totalRows + BlankRows - One, 0, LanguageStrings.FootNotes);
                    if (TableFootNoteArray.GetLength(0) == One)
                    {
                        //If only one Footnote is selected then increase the columns to One as spreadsheet is not producing the range for a single row.
                        ExcelFile.SetArrayValuesIntoSheet(sheetIndex, totalRows + BlankRows, 0, totalRows + TotalTableFootNoteRows + BlankRows + One, 2, TableFootNoteArray);
                    }
                    else
                    {
                        ExcelFile.SetArrayValuesIntoSheet(sheetIndex, totalRows + BlankRows, 0, totalRows + TotalTableFootNoteRows + BlankRows + One, One, TableFootNoteArray);
                    }
                    BlankRows += TitleSubTitleMargin + TotalTableFootNoteRows;
                }
            }


            //Pass Array containing Comments Values to show comments in XLS Worksheet.
            if (TotalTableCommentsRows > 0)	//Set the Header for Comments.
            {
                if (!this._TemplateStyle.Comments.FontTemplate.Inline)
                {
                    ExcelFile.SetCellValue(sheetIndex, totalRows + BlankRows - One, 0, LanguageStrings.Comments);
                    if (TableCommentsArray.GetLength(0) == One)
                    {
                        //If only one comment is selected then increase the columns to One as spreadsheet is not producing the range for a single row.
                        ExcelFile.SetArrayValuesIntoSheet(sheetIndex, totalRows + BlankRows, 0, totalRows + TotalTableCommentsRows + BlankRows, 2, TableCommentsArray);
                    }
                    else
                    {
                        ExcelFile.SetArrayValuesIntoSheet(sheetIndex, totalRows + BlankRows, 0, totalRows + TotalTableCommentsRows + BlankRows + One, One, TableCommentsArray);
                    }
                    BlankRows += TotalTableCommentsRows + TitleSubTitleMargin;
                }
            }

            //Set the Sources.
            if (this.AddColor)
            {

                BlankRows += this.Themes[0].BreakCount;
            }

            if (this._ShowSource)
            {
                ExcelFile.SetCellValue(sheetIndex, totalRows + BlankRows - One, 0, LanguageStrings.Source);
                if (TableSourcesArray.GetLength(0) == One)
                {
                    //colunm length is increased to one as in spreadsheet doen't provide range to null when rows and column are set to One.
                    ExcelFile.SetArrayValuesIntoSheetWithoutAutoFit(sheetIndex, totalRows + BlankRows, 0, totalRows + this.DTSources.Rows.Count + BlankRows, 2, TableSourcesArray);
                }
                else
                {
                    ExcelFile.SetArrayValuesIntoSheetWithoutAutoFit(sheetIndex, totalRows + BlankRows, 0, totalRows + this.DTSources.Rows.Count + BlankRows, One, TableSourcesArray);
                }
            }


            #endregion

            #region "-- Set the width of Rows --"
            if (!this._IsAutoFit)
            {
                ExcelFile.SetColumnWidth(sheetIndex, Convert.ToDouble(this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth), TitleSubTitleMargin, this._Fields.Rows.Count, TitleSubTitleMargin + ColumnArrangementColumns - 1, totalColumns - One);
            }
            if (this._SelectedIndicatorClassification == false)
            {
                for (int i = 0; i < this._Fields.Rows.Count; i++)
                {
                    if (TableXLS.Columns[i].ColumnName == Area.AreaName)
                    {
                        ExcelFile.SetColumnWidth(sheetIndex, 25, TitleSubTitleMargin, i, TitleSubTitleMargin + ColumnArrangementColumns - 1, i);
                    }
                    else if (TableXLS.Columns[i].ColumnName == Timeperiods.TimePeriod)
                    {
                        ExcelFile.SetColumnWidth(sheetIndex, 10, TitleSubTitleMargin, i, TitleSubTitleMargin + ColumnArrangementColumns - 1, i);
                    }
                    else if (TableXLS.Columns[i].ColumnName == IndicatorClassifications.ICName)
                    {
                        ExcelFile.SetColumnWidth(sheetIndex, 40, TitleSubTitleMargin, i, TitleSubTitleMargin + ColumnArrangementColumns - 1, i);
                    }
                    else if (TableXLS.Columns[i].ColumnName == Indicator.IndicatorName)
                    {
                        ExcelFile.SetColumnWidth(sheetIndex, 30, TitleSubTitleMargin, i, TitleSubTitleMargin + ColumnArrangementColumns - 1, i);
                    }
                    else if (TableXLS.Columns[i].ColumnName == Unit.UnitName || TableXLS.Columns[i].ColumnName == SubgroupVals.SubgroupVal)
                    {
                        ExcelFile.SetColumnWidth(sheetIndex, 15, TitleSubTitleMargin, i, TitleSubTitleMargin + ColumnArrangementColumns - 1, i);
                    }
                    else
                    {
                        foreach (DataRow DR in this.DIDataView.SubgroupInfo.Rows)
                        {
                            if (TableXLS.Columns[i].ColumnName == DR[SubgroupInfoColumns.Name].ToString())
                            {
                                ExcelFile.SetColumnWidth(sheetIndex, 8, TitleSubTitleMargin, i, TitleSubTitleMargin + ColumnArrangementColumns - 1, i);
                            }
                        }
                    }
                }
                //ExcelFile.AutoFitColumns(sheetIndex, TitleSubTitleMargin, 0, totalRows - 1, this._Fields.Rows.Count - One);
            }

            #endregion

            #region "-- Set Title Style --"

            //Set the Title		
            ExcelFile.SetCellValue(sheetIndex, 0, 0, this._Title);
            ExcelFile.SetHorizontalAlignment(sheetIndex, 0, 0, this.MapHAlignWithSpreadsheetGear(this._TemplateStyle.TitleSetting.FontTemplate.TextAlignment));
            ExcelFile.SetCellColor(sheetIndex, 0, 0, this._TemplateStyle.TitleSetting.FontTemplate.ForeColor, this._TemplateStyle.TitleSetting.FontTemplate.BackColor);
            ExcelFile.GetCellFont(sheetIndex, 0, 0).Name = this._TemplateStyle.TitleSetting.FontTemplate.FontName;
            ExcelFile.GetCellFont(sheetIndex, 0, 0).Size = this._TemplateStyle.TitleSetting.FontTemplate.FontSize;

            this.SetExcelRangeFont(ExcelFile.GetCellFont(sheetIndex, 0, 0), this._TemplateStyle.TitleSetting.FontTemplate.FontStyle);
            // Borders
            ExcelFile.SetCellBorder(sheetIndex, 0, 0, SpreadsheetGear.LineStyle.Continuous, SpreadsheetGear.BorderWeight.Thin, borderColor, SpreadsheetGear.BordersIndex.EdgeLeft);
            ExcelFile.SetCellBorder(sheetIndex, 0, 0, SpreadsheetGear.LineStyle.Continuous, SpreadsheetGear.BorderWeight.Thin, borderColor, SpreadsheetGear.BordersIndex.EdgeTop);

            #endregion

            #region "-- Set subtitle style --"

            //Set the Subtitle
            ExcelFile.SetCellValue(sheetIndex, One, 0, this._Subtitle);
            ExcelFile.SetHorizontalAlignment(sheetIndex, One, 0, this.MapHAlignWithSpreadsheetGear(this._TemplateStyle.SubTitleSetting.FontTemplate.TextAlignment));
            ExcelFile.SetCellColor(sheetIndex, One, 0, this._TemplateStyle.SubTitleSetting.FontTemplate.ForeColor, this._TemplateStyle.SubTitleSetting.FontTemplate.BackColor);
            ExcelFile.GetCellFont(sheetIndex, One, 0).Name = this._TemplateStyle.SubTitleSetting.FontTemplate.FontName;
            ExcelFile.GetCellFont(sheetIndex, One, 0).Size = this._TemplateStyle.SubTitleSetting.FontTemplate.FontSize;
            this.SetExcelRangeFont(ExcelFile.GetCellFont(sheetIndex, One, 0), this._TemplateStyle.SubTitleSetting.FontTemplate.FontStyle);
            // Borders
            ExcelFile.SetCellBorder(sheetIndex, 1, 0, SpreadsheetGear.LineStyle.Continuous, SpreadsheetGear.BorderWeight.Thin, borderColor, SpreadsheetGear.BordersIndex.EdgeLeft);

            #endregion

            #region "-- Apply Alternate row color -- "

            //-- Apply the alternate row color.
            if (this.PresentationType != Presentation.PresentationType.Graph)
            {
                if (this._TemplateStyle.RowSetting.FontTemplate.ShowAlternateColor && !this._AddColor)
                {
                    int ExcelRowIndex = TitleSubTitleMargin;
                    bool IsAlternateColor = false;
                    for (int RowIndex = 0; RowIndex < TableXLS.Rows.Count; RowIndex++)
                    {
                        if (TableXLS.Rows[RowIndex][TablePresentation.ROWTYPE].ToString() == RowType.DataRow.ToString())
                        {
                            if (!IsAlternateColor)
                            {
                                ExcelFile.SetRangeColor(ExcelRowIndex, 0, ExcelRowIndex, TotalXLSColumns, Color.Black, ColorTranslator.FromHtml(this._TemplateStyle.RowSetting.FontTemplate.AlternateBackColor1));
                                IsAlternateColor = true;
                            }
                            else
                            {
                                ExcelFile.SetRangeColor(ExcelRowIndex, 0, ExcelRowIndex, TotalXLSColumns, Color.Black, ColorTranslator.FromHtml(this._TemplateStyle.RowSetting.FontTemplate.AlternateBackColor2));
                                IsAlternateColor = false;
                            }
                        }
                        else
                        {
                            IsAlternateColor = false;
                        }
                        ExcelRowIndex += 1;
                    }
                }
            }

            #endregion

            #region "-- Set style for Column and row header --"

            //Set style for ColumnHeader
            if (this._PresentationType != Presentation.PresentationType.Graph)
            {
                ExcelFile.SetRangeColor(sheetIndex, TitleSubTitleMargin, 0, TitleSubTitleMargin + ColumnArrangementColumns - One, TotalXLSColumns, this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor, this._TemplateStyle.ColumnSetting.FontTemplate.BackColor);
                for (Counter = 0; Counter < ColumnArrangementColumns; Counter++)
                {
                    //Horizontal Alignment for Row Fields shall be default left alignment and that for Column Field shall be set to Center
                    ExcelFile.SetHorizontalAlignment(sheetIndex, TitleSubTitleMargin + Counter, this._Fields.Rows.Count, XLSStartRow + Counter, TotalXLSColumns, SpreadsheetGear.HAlign.Center);
                    ExcelFile.GetRangeFont(sheetIndex, TitleSubTitleMargin + Counter, 0, XLSStartRow + Counter, TotalXLSColumns).Name = this._TemplateStyle.ColumnSetting.FontTemplate.FontName;
                    ExcelFile.GetRangeFont(sheetIndex, TitleSubTitleMargin + Counter, 0, XLSStartRow + Counter, TotalXLSColumns).Size = this._TemplateStyle.ColumnSetting.FontTemplate.FontSize;
                    this.SetExcelRangeFont(ExcelFile.GetRangeFont(sheetIndex, TitleSubTitleMargin + Counter, 0, XLSStartRow + Counter, TotalXLSColumns), this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle);
                }
                //-- Hide the row header
                if (!this._TemplateStyle.RowSetting.FontTemplate.Show)
                {
                    string CellAddress = string.Empty;
                    if (this._Fields.Columns.Count == 0)
                    {
                        CellAddress = ExcelFile.GetRange(sheetIndex, TitleSubTitleMargin, 0, TitleSubTitleMargin, this._Fields.Rows.Count - 1);
                    }
                    else
                    {
                        CellAddress = ExcelFile.GetRange(sheetIndex, TitleSubTitleMargin + this._Fields.Columns.Count - 1, 0, TitleSubTitleMargin + this._Fields.Columns.Count - 1, this._Fields.Rows.Count - 1);
                    }
                    ExcelFile.SetCellValue(sheetIndex, CellAddress, string.Empty);
                }
                try
                {
                    this.SetExcelRangeFont(ExcelFile.GetRangeFont(sheetIndex, TitleSubTitleMargin, 0, TitleSubTitleMargin + ColumnArrangementColumns - One, totalColumns), this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle);
                }
                catch (Exception)
                { }
                if (this._TemplateStyle.RowSetting.FontTemplate.ShowBackColor)
                {
                    ExcelFile.SetRangeColor(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns, 0, totalRows - One, this._Fields.Rows.Count - One, this._TemplateStyle.RowSetting.FontTemplate.ForeColor, this._TemplateStyle.RowSetting.FontTemplate.BackColor);
                }
                ExcelFile.GetRangeFont(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns, 0, totalRows - One, this._Fields.Rows.Count - One).Name = this._TemplateStyle.RowSetting.FontTemplate.FontName;
                ExcelFile.GetRangeFont(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns, 0, totalRows - One, this._Fields.Rows.Count - One).Size = this._TemplateStyle.RowSetting.FontTemplate.FontSize;
                if (ExcelFile.GetRangeFont(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns, 0, totalRows - One, this._Fields.Rows.Count - One) != null)
                {
                    this.SetExcelRangeFont(ExcelFile.GetRangeFont(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns, 0, totalRows - One, this._Fields.Rows.Count - One), this._TemplateStyle.RowSetting.FontTemplate.FontStyle);
                }
            }

            this.RaiseEventProgressBar(90, 100);

            #endregion

            #region "-- Set Footnote/Comments Border lines --"

            if (this._TemplateStyle.ShowBorderLines && !this.IsSuppressDuplicateRows)
            {
                //Leave a single cell if either footnote or comment is available
                if ((this.FootnoteCommentType == FootnoteCommentType.Footnote && this._TemplateStyle.Footnotes.Show == true) || (this.FootnoteCommentType == FootnoteCommentType.Comment && this._TemplateStyle.Comments.Show == true))
                {
                    if (this._TemplateStyle.Denominator.Show)
                    {
                        for (Counter = this._Fields.Rows.Count; Counter <= TotalXLSColumns; Counter = Counter + 3)
                        {
                            ExcelFile.SetRangeBorder(sheetIndex, XLSStartRow + ColumnArrangementColumns, Counter + 2, this.TotalXLSRows - One, Counter + 2, SpreadsheetGear.LineStyle.None, SpreadsheetGear.BorderWeight.Thin, borderColor, SpreadsheetGear.BordersIndex.EdgeLeft);
                            ExcelFile.SetRangeBorder(sheetIndex, XLSStartRow, Counter, this.TotalXLSRows - One, Counter, SpreadsheetGear.LineStyle.None, SpreadsheetGear.BorderWeight.Thin, borderColor, SpreadsheetGear.BordersIndex.EdgeLeft);
                        }
                    }
                    else
                    {
                        //since graph wizard doesn't contains comments, so don't leave any column when drawing column borders.
                        //The border should come at each column.					
                        if (this._PresentationType == Presentation.PresentationType.Graph)
                        {
                            for (Counter = this._Fields.Rows.Count; Counter <= TotalXLSColumns; Counter++)
                            {
                                ExcelFile.SetRangeBorder(sheetIndex, XLSStartRow, Counter, this.TotalXLSRows - One, Counter, SpreadsheetGear.LineStyle.None, SpreadsheetGear.BorderWeight.Thin, borderColor, SpreadsheetGear.BordersIndex.EdgeLeft);
                            }
                        }
                        else //In case of table wizard , leave one column for data and one for the comments.
                        {
                            for (Counter = this._Fields.Rows.Count; Counter <= TotalXLSColumns; Counter = Counter + 2)
                            {
                                ExcelFile.SetRangeBorder(sheetIndex, XLSStartRow, Counter, this.TotalXLSRows - One, Counter, SpreadsheetGear.LineStyle.None, SpreadsheetGear.BorderWeight.Thin, borderColor, SpreadsheetGear.BordersIndex.EdgeLeft);
                            }
                        }
                    }
                }
                //Leave two cells if both footnote and comments are available
                if (this.FootnoteCommentType == FootnoteCommentType.Both)
                {
                    if (this._TemplateStyle.Denominator.Show)
                    {
                        for (Counter = this._Fields.Rows.Count; Counter <= TotalXLSColumns; Counter = Counter + 4)
                        {
                            ExcelFile.SetRangeBorder(sheetIndex, XLSStartRow + ColumnArrangementColumns, Counter + 2, this.TotalXLSRows - One, Counter + 2, SpreadsheetGear.LineStyle.None, SpreadsheetGear.BorderWeight.Thin, borderColor, SpreadsheetGear.BordersIndex.EdgeRight);
                            ExcelFile.SetRangeBorder(sheetIndex, XLSStartRow, Counter, this.TotalXLSRows - One, Counter, SpreadsheetGear.LineStyle.None, SpreadsheetGear.BorderWeight.Thin, borderColor, SpreadsheetGear.BordersIndex.EdgeLeft);
                        }
                    }
                    else
                    {
                        for (Counter = this._Fields.Rows.Count; Counter <= TotalXLSColumns; Counter = Counter + 3)
                        {
                            ExcelFile.SetRangeBorder(sheetIndex, XLSStartRow, Counter, this.TotalXLSRows - One, Counter, SpreadsheetGear.LineStyle.None, SpreadsheetGear.BorderWeight.Thin, borderColor, SpreadsheetGear.BordersIndex.EdgeLeft);
                        }
                    }
                }
                //Leave no cell comment type is none
                if (this.FootnoteCommentType == FootnoteCommentType.None)
                {
                    if (this._TemplateStyle.Denominator.Show)
                    {
                        for (Counter = this._Fields.Rows.Count; Counter <= TotalXLSColumns; Counter = Counter + 2)
                        {
                            ExcelFile.SetRangeBorder(sheetIndex, XLSStartRow + ColumnArrangementColumns, Counter + One, this.TotalXLSRows - One, Counter + One, SpreadsheetGear.LineStyle.None, SpreadsheetGear.BorderWeight.Thin, borderColor, SpreadsheetGear.BordersIndex.EdgeLeft);
                            ExcelFile.SetRangeBorder(sheetIndex, XLSStartRow, Counter, this.TotalXLSRows - One, Counter, SpreadsheetGear.LineStyle.None, SpreadsheetGear.BorderWeight.Thin, borderColor, SpreadsheetGear.BordersIndex.EdgeLeft);
                        }
                    }
                    else
                    {
                        for (Counter = this._Fields.Rows.Count; Counter <= TotalXLSColumns; Counter++)
                        {
                            ExcelFile.SetRangeBorder(sheetIndex, XLSStartRow, Counter, this.TotalXLSRows - One, Counter, SpreadsheetGear.LineStyle.None, SpreadsheetGear.BorderWeight.Thin, borderColor, SpreadsheetGear.BordersIndex.EdgeLeft);
                        }
                    }
                }

            }

            #endregion

            ExcelFile.WrapText(sheetIndex, TitleSubTitleMargin, this._Fields.Rows.Count, TitleSubTitleMargin + ColumnArrangementColumns - 1, totalColumns - One, true);

            #region " -- Rows Word Wrap / Alignment -- "

            //-- Apply Rows word wrap
            for (int RowIndex = 0; RowIndex < TableXLS.Rows.Count; RowIndex++)
            {
                ExcelFile.SetVerticalAlignment(sheetIndex, TitleSubTitleMargin + RowIndex, 0, TitleSubTitleMargin + RowIndex, this._Fields.Rows.Count - 1, SpreadsheetGear.VAlign.Top);
                if (TableXLS.Rows[RowIndex][TablePresentation.ROWTYPE].ToString() == RowType.DataRow.ToString())
                {
                    ExcelFile.WrapText(sheetIndex, TitleSubTitleMargin + RowIndex, 0, TitleSubTitleMargin + RowIndex, this._Fields.Rows.Count - 1, this.TemplateStyle.RowSetting.FontTemplate.WordWrap);
                }
            }

            if (string.IsNullOrEmpty(this._TableReport.AreaSelection.AreaLevels) && this._PresentationType != Presentation.PresentationType.Graph)
            {
                //-- Apply rows alignment settings
                for (int RowIndex = 0; RowIndex < TableXLS.Rows.Count; RowIndex++)
                {
                    if (TableXLS.Rows[RowIndex][TablePresentation.ROWTYPE].ToString() == RowType.DataRow.ToString())
                    {
                        ExcelFile.SetHorizontalAlignment(sheetIndex, TitleSubTitleMargin + RowIndex, 0, TitleSubTitleMargin + RowIndex, this._Fields.Rows.Count - 1, this.MapHAlignWithSpreadsheetGear(this._TemplateStyle.RowSetting.FontTemplate.TextAlignment));
                    }
                }
            }

            #endregion

            #region	"--	Set Excell sheet Borders --"
            if (this._TemplateStyle.ShowBorderLines)
            {
                //Set table	range boundry
                ExcelFile.SetRangeBorders(sheetIndex, XLSStartRow, 0, this.TotalXLSRows - One, TotalXLSColumns, SpreadsheetGear.LineStyle.Continuous, SpreadsheetGear.BorderWeight.Thin, borderColor);

                //Row headers boundry		
                ExcelFile.SetRangeBorders(sheetIndex, TitleSubTitleMargin, 0, TitleSubTitleMargin + ColumnArrangementColumns - One, this._Fields.Rows.Count - One, SpreadsheetGear.LineStyle.Continuous, SpreadsheetGear.BorderWeight.Thin, borderColor);

                //Column headers boundry			
                for (Counter = 0; Counter < ColumnArrangementColumns; Counter++)
                {
                    for (int ColCounter = this._Fields.Rows.Count; ColCounter <= TotalXLSColumns; ColCounter++)
                    {
                        ExcelFile.SetRangeBorders(sheetIndex, TitleSubTitleMargin + Counter, ColCounter, XLSStartRow + Counter, ColCounter, SpreadsheetGear.LineStyle.Continuous, SpreadsheetGear.BorderWeight.Thin, borderColor);
                    }

                }

            }

            #endregion

            #region "-- Set style for Content and ICHeaders --"

            //Set style	for	Content
            if (this._TemplateStyle.ContentSetting.FontTemplate.ShowBackColor)
            {
                ExcelFile.SetRangeColor(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns, this._Fields.Rows.Count, totalRows - One, totalColumns - 1, this._TemplateStyle.ContentSetting.FontTemplate.ForeColor, this._TemplateStyle.ContentSetting.FontTemplate.BackColor);
            }
            int RowFieldCount = this._Fields.Rows.Count;
            if (this._PresentationType == Presentation.PresentationType.Graph && !this._ShowExcel)
            {
                RowFieldCount = 1;
            }
            ExcelFile.GetRangeFont(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns, RowFieldCount, totalRows - One, totalColumns - One).Name = this._TemplateStyle.ContentSetting.FontTemplate.FontName;
            ExcelFile.GetRangeFont(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns, RowFieldCount, totalRows - One, totalColumns - One).Size = this._TemplateStyle.ContentSetting.FontTemplate.FontSize;
            ExcelFile.SetHorizontalAlignment(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns, RowFieldCount, totalRows - One, totalColumns - One, this.MapHAlignWithSpreadsheetGear(this._TemplateStyle.ContentSetting.FontTemplate.TextAlignment));
            ExcelFile.SetVerticalAlignment(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns, RowFieldCount, totalRows - One, totalColumns - One, SpreadsheetGear.VAlign.Top);
            if (ExcelFile.GetRangeFont(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns, RowFieldCount, totalRows - One, totalColumns - One) != null)
            {
                this.SetExcelRangeFont(ExcelFile.GetRangeFont(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns, RowFieldCount, totalRows - One, totalColumns - One), this._TemplateStyle.ContentSetting.FontTemplate.FontStyle);
            }


            #region	"--	Set	style for content if Report	is of Subnational Type --"

            //Set style for Excell sheet if report is of type subnational
            if (!string.IsNullOrEmpty(this._TableReport.AreaSelection.AreaLevels) && this._PresentationType != Presentation.PresentationType.Graph)
            {
                if (this._Fields.Rows[Area.AreaName] != null)
                {
                    int StartRowCount = 0;
                    int FinalRowCount = -1;
                    int ICRows = 0;
                    string AreaLevel = string.Empty;
                    string NewAreaLevel = string.Empty;
                    bool LevelExists = false;
                    foreach (DataRow TPRow in TableXLS.Rows)
                    {
                        if (TPRow[TablePresentation.AREALEVEL].ToString() != string.Empty)
                        {
                            if (ICRows > 0)
                            {
                                StartRowCount = StartRowCount + ICRows;
                                ICRows = 0;
                            }

                            NewAreaLevel = TPRow[TablePresentation.AREALEVEL].ToString();
                            LevelExists = true;
                            if (AreaLevel == string.Empty)
                            {
                                AreaLevel = NewAreaLevel;
                            }
                            if (AreaLevel != NewAreaLevel)
                            {
                                //Apply style for all arealevels except the last one
                                this.SetStyleForSubnationalReport(sheetIndex, ColumnArrangementColumns, StartRowCount, FinalRowCount, totalColumns, AreaLevel, ExcelFile, this.MaxAreaLevel);
                                StartRowCount = FinalRowCount + 1 - ColumnArrangementColumns;
                                AreaLevel = TPRow[TablePresentation.AREALEVEL].ToString();
                            }
                        }
                        else if (TPRow[TablePresentation.ROWTYPE].ToString() == RowType.ICAggregate.ToString() || TPRow[TablePresentation.ROWTYPE].ToString() == RowType.EmptyRow.ToString())
                        {
                            ICRows++;
                        }
                        FinalRowCount++;
                    }
                    if (LevelExists)
                    {
                        //Apply style for Last AreaLevel
                        this.SetStyleForSubnationalReport(sheetIndex, ColumnArrangementColumns, StartRowCount, FinalRowCount, totalColumns, NewAreaLevel, ExcelFile, this.MaxAreaLevel);
                    }
                }
            }

            #endregion

            //Set style for ICHeaders
            foreach (DataRow DRowTableXls in TableXLS.Select(TablePresentation.ROWTYPE + "='" + RowType.ICAggregate.ToString() + "'"))
            {
                ExcelFile.SetRangeColor(sheetIndex, TitleSubTitleMargin + Convert.ToInt32(DRowTableXls[TablePresentation.ROWINDEX]) - One, 0, TitleSubTitleMargin + Convert.ToInt32(DRowTableXls[TablePresentation.ROWINDEX]) - One, totalColumns - One, this._TemplateStyle.GroupHeaderSetting.FontTemplate.ForeColor, this._TemplateStyle.GroupHeaderSetting.FontTemplate.BackColor);
                ExcelFile.GetRangeFont(sheetIndex, TitleSubTitleMargin + Convert.ToInt32(DRowTableXls[TablePresentation.ROWINDEX]) - One, 0, TitleSubTitleMargin + Convert.ToInt32(DRowTableXls[TablePresentation.ROWINDEX]) - One, totalColumns - One).Name = this._TemplateStyle.GroupHeaderSetting.FontTemplate.FontName;
                ExcelFile.GetRangeFont(sheetIndex, TitleSubTitleMargin + Convert.ToInt32(DRowTableXls[TablePresentation.ROWINDEX]) - One, 0, TitleSubTitleMargin + Convert.ToInt32(DRowTableXls[TablePresentation.ROWINDEX]) - One, totalColumns - One).Size = this._TemplateStyle.GroupHeaderSetting.FontTemplate.FontSize;
                this.SetExcelRangeFont(ExcelFile.GetRangeFont(sheetIndex, TitleSubTitleMargin + Convert.ToInt32(DRowTableXls[TablePresentation.ROWINDEX]) - One, 0, TitleSubTitleMargin + Convert.ToInt32(DRowTableXls[TablePresentation.ROWINDEX]) - One, totalColumns - One), this._TemplateStyle.GroupHeaderSetting.FontTemplate.FontStyle);
            }

            #endregion

            #region "-- Set Legend colors if AddColor is checked on STEP 2. --"
            //Set Legend colors if AddColor is checked on STEP 2.
            if (this._AddColor)
            {
                SetLegendStyle(ExcelFile, TableXLS, sheetIndex);
            }
            #endregion

            #region	"--	Set	style	for	footnote and comments --"

            //Set style	for	footnote and comments.
            SetFootNoteCommentsStyle(TableXLS, TableFootNoteArray, TableCommentsArray, ExcelFile, TableFootNote, sheetIndex);

            if (this._TemplateStyle.Denominator.Show)
            {
                //Leave	a single cell if either	footnote or	comment	is available
                if ((this.FootnoteCommentType == FootnoteCommentType.Footnote && this._TemplateStyle.Footnotes.Show == true) || (this.FootnoteCommentType == FootnoteCommentType.Comment && this._TemplateStyle.Comments.Show == true))
                {
                    if (this._TemplateStyle.Denominator.Show)
                    {
                        for (Counter = this._Fields.Rows.Count; Counter <= TotalXLSColumns; Counter = Counter + 3)
                        {
                            ExcelFile.SetRangeBorder(sheetIndex, XLSStartRow + ColumnArrangementColumns, Counter + 2, this.TotalXLSRows - One, Counter + 2, SpreadsheetGear.LineStyle.None, SpreadsheetGear.BorderWeight.Thin, borderColor, SpreadsheetGear.BordersIndex.EdgeLeft);
                            ExcelFile.SetRangeBorder(sheetIndex, XLSStartRow, Counter, this.TotalXLSRows - One, Counter, SpreadsheetGear.LineStyle.None, SpreadsheetGear.BorderWeight.Thin, borderColor, SpreadsheetGear.BordersIndex.EdgeLeft);
                            ExcelFile.SetRangeColor(sheetIndex, XLSStartRow + ColumnArrangementColumns, Counter + 2, this.TotalXLSRows - One, Counter + 2, this._TemplateStyle.Denominator.FontTemplate.ForeColor, this._TemplateStyle.Denominator.FontTemplate.BackColor);
                        }

                    }
                }
                //Leave	two	cells if both footnote and comments	are	available
                if (this.FootnoteCommentType == FootnoteCommentType.Both)
                {
                    if (this._TemplateStyle.Denominator.Show)
                    {
                        for (Counter = this._Fields.Rows.Count; Counter <= TotalXLSColumns; Counter = Counter + 4)
                        {
                            ExcelFile.SetRangeBorder(sheetIndex, XLSStartRow + ColumnArrangementColumns, Counter + 2, this.TotalXLSRows - One, Counter + 2, SpreadsheetGear.LineStyle.None, SpreadsheetGear.BorderWeight.Thin, borderColor, SpreadsheetGear.BordersIndex.EdgeRight);
                            ExcelFile.SetRangeBorder(sheetIndex, XLSStartRow, Counter, this.TotalXLSRows - One, Counter, SpreadsheetGear.LineStyle.None, SpreadsheetGear.BorderWeight.Thin, borderColor, SpreadsheetGear.BordersIndex.EdgeLeft);
                            ExcelFile.SetRangeColor(sheetIndex, XLSStartRow + ColumnArrangementColumns, Counter + 2, this.TotalXLSRows - One, Counter + 2, this._TemplateStyle.Denominator.FontTemplate.ForeColor, this._TemplateStyle.Denominator.FontTemplate.BackColor);
                        }
                    }
                }
                //Leave	no cell	comment	type is	none
                if (this.FootnoteCommentType == FootnoteCommentType.None)
                {
                    if (this._TemplateStyle.Denominator.Show)
                    {
                        for (Counter = this._Fields.Rows.Count; Counter <= TotalXLSColumns; Counter = Counter + 2)
                        {
                            ExcelFile.SetRangeBorder(sheetIndex, XLSStartRow + ColumnArrangementColumns, Counter + One, this.TotalXLSRows - One, Counter + One, SpreadsheetGear.LineStyle.None, SpreadsheetGear.BorderWeight.Thin, borderColor, SpreadsheetGear.BordersIndex.EdgeLeft);
                            ExcelFile.SetRangeBorder(sheetIndex, XLSStartRow, Counter, this.TotalXLSRows - One, Counter, SpreadsheetGear.LineStyle.None, SpreadsheetGear.BorderWeight.Thin, borderColor, SpreadsheetGear.BordersIndex.EdgeLeft);
                            ExcelFile.SetRangeColor(sheetIndex, XLSStartRow + ColumnArrangementColumns, Counter + 1, this.TotalXLSRows - One, Counter + 1, this._TemplateStyle.Denominator.FontTemplate.ForeColor, this._TemplateStyle.Denominator.FontTemplate.BackColor);
                        }
                    }
                }

            }

            #endregion

            #region "-- Set style for aggregate

            //Set style for aggregate
            foreach (DataRow DRowTableXls in TableXLS.Select(TablePresentation.ROWTYPE + "='" + RowType.GroupAggregate.ToString() + "'"))
            {
                ExcelFile.SetRangeColor(sheetIndex, TitleSubTitleMargin + Convert.ToInt32(DRowTableXls[TablePresentation.ROWINDEX]) - One, 0, TitleSubTitleMargin + Convert.ToInt32(DRowTableXls[TablePresentation.ROWINDEX]) - One, TotalXLSColumns, this._TemplateStyle.GroupAggregateSetting.FontTemplate.ForeColor, this._TemplateStyle.GroupAggregateSetting.FontTemplate.BackColor);
                ExcelFile.GetRangeFont(sheetIndex, TitleSubTitleMargin + Convert.ToInt32(DRowTableXls[TablePresentation.ROWINDEX]) - One, 0, TitleSubTitleMargin + Convert.ToInt32(DRowTableXls[TablePresentation.ROWINDEX]) - One, TotalXLSColumns).Name = this._TemplateStyle.GroupAggregateSetting.FontTemplate.FontName;
                ExcelFile.GetRangeFont(sheetIndex, TitleSubTitleMargin + Convert.ToInt32(DRowTableXls[TablePresentation.ROWINDEX]) - One, 0, TitleSubTitleMargin + Convert.ToInt32(DRowTableXls[TablePresentation.ROWINDEX]) - One, TotalXLSColumns).Size = this._TemplateStyle.GroupAggregateSetting.FontTemplate.FontSize;
                this.SetExcelRangeFont(ExcelFile.GetRangeFont(sheetIndex, TitleSubTitleMargin + Convert.ToInt32(DRowTableXls[TablePresentation.ROWINDEX]) - One, 0, TitleSubTitleMargin + Convert.ToInt32(DRowTableXls[TablePresentation.ROWINDEX]) - One, totalColumns), this._TemplateStyle.GroupAggregateSetting.FontTemplate.FontStyle);
            }
            foreach (DataRow DRowTableXls in TableXLS.Select(TablePresentation.ROWTYPE + "='" + RowType.SubAggregate.ToString() + "'"))
            {
                ExcelFile.SetRangeColor(sheetIndex, TitleSubTitleMargin + Convert.ToInt32(DRowTableXls[TablePresentation.ROWINDEX]) - One, 0, TitleSubTitleMargin + Convert.ToInt32(DRowTableXls[TablePresentation.ROWINDEX]) - One, TotalXLSColumns, this._TemplateStyle.SubAggregateSetting.FontTemplate.ForeColor, this._TemplateStyle.SubAggregateSetting.FontTemplate.BackColor);
                ExcelFile.GetRangeFont(sheetIndex, TitleSubTitleMargin + Convert.ToInt32(DRowTableXls[TablePresentation.ROWINDEX]) - One, 0, TitleSubTitleMargin + Convert.ToInt32(DRowTableXls[TablePresentation.ROWINDEX]) - One, TotalXLSColumns).Name = this._TemplateStyle.SubAggregateSetting.FontTemplate.FontName;
                ExcelFile.GetRangeFont(sheetIndex, TitleSubTitleMargin + Convert.ToInt32(DRowTableXls[TablePresentation.ROWINDEX]) - One, 0, TitleSubTitleMargin + Convert.ToInt32(DRowTableXls[TablePresentation.ROWINDEX]) - One, TotalXLSColumns).Size = this._TemplateStyle.SubAggregateSetting.FontTemplate.FontSize;
                this.SetExcelRangeFont(ExcelFile.GetRangeFont(sheetIndex, TitleSubTitleMargin + Convert.ToInt32(DRowTableXls[TablePresentation.ROWINDEX]) - One, 0, TitleSubTitleMargin + Convert.ToInt32(DRowTableXls[TablePresentation.ROWINDEX]) - One, TotalXLSColumns), this._TemplateStyle.SubAggregateSetting.FontTemplate.FontStyle);
            }

            #endregion

            #region " -- Format Data value -- "

            if (this.PresentationType != Presentation.PresentationType.Graph)
            {
                if (this._TemplateStyle.ContentSetting.FontTemplate.FormatDataValue || this._TemplateStyle.ContentSetting.FontTemplate.RoundDataValues)
                {
                    string CellAddress = string.Empty;
                    int ExcelRowIndex = TitleSubTitleMargin;
                    int ExcelColIndex = this._Fields.Rows.Count;

                    for (int RowIndex = 0; RowIndex < TableXLS.Rows.Count; RowIndex++)
                    {
                        for (int ColumnIndex = this._Fields.Rows.Count; ColumnIndex < TableXLS.Columns.Count; ColumnIndex++)
                        {
                            if (TableXLS.Rows[RowIndex][TablePresentation.ROWTYPE].ToString() == RowType.DataRow.ToString() || TableXLS.Rows[RowIndex][TablePresentation.ROWTYPE].ToString() == RowType.GroupAggregate.ToString() || TableXLS.Rows[RowIndex][TablePresentation.ROWTYPE].ToString() == RowType.SubAggregate.ToString())
                            {
                                if (DICommon.IsNumeric(TableXLS.Rows[RowIndex][ColumnIndex].ToString()) && this.ALColumnType[ColumnIndex].ToString() == (TableColumnType.DataValue.ToString()))
                                {
                                    CellAddress = ExcelFile.GetRange(sheetIndex, ExcelRowIndex, ExcelColIndex, ExcelRowIndex, ExcelColIndex);
                                    if (this._TemplateStyle.ContentSetting.FontTemplate.RoundDataValues || this._TemplateStyle.ContentSetting.FontTemplate.FormatDataValue)
                                    {
                                        ExcelFile.SetFormatNumbericValue(sheetIndex, CellAddress, string.Format(this.NumberFormat(this._TemplateStyle.ContentSetting.FontTemplate.DecimalPlace, TableXLS.Rows[RowIndex][ColumnIndex].ToString()), Convert.ToDouble(TableXLS.Rows[RowIndex][ColumnIndex])));
                                        //ExcelFile.SetFormatNumbericValue(sheetIndex, CellAddress, this._TemplateStyle.ContentSetting.FontTemplate.RoundDataValues, this._TemplateStyle.ContentSetting.FontTemplate.DecimalPlace, this._TemplateStyle.ContentSetting.FontTemplate.FormatDataValue);
                                    }
                                    else
                                    {
                                        if (TableXLS.Rows[RowIndex][ColumnIndex].ToString().Contains(DICommon.NumberDecimalSeparator))
                                        {
                                            ExcelFile.SetFormatNumbericValue(sheetIndex, CellAddress, string.Format(this.NumberFormat(this._TemplateStyle.ContentSetting.FontTemplate.DecimalPlace, TableXLS.Rows[RowIndex][ColumnIndex].ToString()), Convert.ToDouble(TableXLS.Rows[RowIndex][ColumnIndex])));
                                            //ExcelFile.SetFormatNumbericValue(sheetIndex, CellAddress, true, 2, this._TemplateStyle.ContentSetting.FontTemplate.FormatDataValue);
                                        }
                                        else
                                        {
                                            ExcelFile.SetFormatNumbericValue(sheetIndex, CellAddress, string.Format(this.NumberFormat(this._TemplateStyle.ContentSetting.FontTemplate.DecimalPlace, TableXLS.Rows[RowIndex][ColumnIndex].ToString()), Convert.ToDouble(TableXLS.Rows[RowIndex][ColumnIndex])));
                                            //ExcelFile.SetFormatNumbericValue(sheetIndex, CellAddress, false, 0, this._TemplateStyle.ContentSetting.FontTemplate.FormatDataValue);
                                        }
                                    }
                                }
                                else
                                {
                                    CellAddress = ExcelFile.GetRange(sheetIndex, ExcelRowIndex, ExcelColIndex, ExcelRowIndex, ExcelColIndex);
                                    ExcelFile.SetColumnFormatType(CellAddress, sheetIndex, SpreadsheetGear.NumberFormatType.Text);
                                }
                            }
                            else
                            {
                                break;
                            }
                            ExcelColIndex += 1;
                        }
                        ExcelRowIndex += 1;
                        ExcelColIndex = this._Fields.Rows.Count;
                    }
                }
            }

            #endregion

            #region	"--	Set	Images --"

            //-- Images are only for table wizard.
            if (this._PresentationType != Presentations.Presentation.PresentationType.Graph)
            {
                //Set column Images..
                if (this.IndicatorNIds != string.Empty)	//Prepare a	datatable containing Indicator images.
                {
                    string strIndicatorNIds = DIQueries.Icon.GetIcon(this.IndicatorNIds, IconElementType.Indicator);
                    DTIndicator = DIConnection.ExecuteDataTable(strIndicatorNIds);
                }
                if (this.ICNIds != string.Empty)
                {
                    //Get all unique ICNIds referred through inclusion of IC Fields
                    ArrayList UniqueCLS_IC = new ArrayList();
                    string[] ArrICFields = this.ICInfoFields.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    for (int FieldIndex = 0; FieldIndex < ArrICFields.Length; FieldIndex++)
                    {
                        foreach (DataRow Drow in this.DIDataView.IUSICNIdInfo.Rows)
                        {
                            if (Drow[ArrICFields[FieldIndex]] != System.DBNull.Value & UniqueCLS_IC.Contains(Drow[ArrICFields[FieldIndex]].ToString()) == false)
                            {
                                UniqueCLS_IC.Add(Drow[ArrICFields[FieldIndex]].ToString());
                            }
                        }
                    }

                    //Add ICNIds to SourceNIds
                    if (UniqueCLS_IC.Count > 0)
                    {
                        string[] UniqueCLS_ICArray = new string[UniqueCLS_IC.Count];
                        UniqueCLS_IC.CopyTo(UniqueCLS_ICArray);
                        string CLS_ICNIds = string.Join(",", UniqueCLS_ICArray);
                        this.ICNIds += "," + CLS_ICNIds;
                    }
                    //Get icons for all ICNIds (Source + ICFields)
                    string strICNIds = DIQueries.Icon.GetIcon(this.ICNIds, IconElementType.IndicatorClassification);
                    DTIndicatorClassification = DIConnection.ExecuteDataTable(strICNIds);
                }
                if (this.UnitNIds != string.Empty)
                {
                    string strUnitNIds = DIQueries.Icon.GetIcon(this.UnitNIds, IconElementType.Unit);
                    DTUnit = DIConnection.ExecuteDataTable(strUnitNIds);
                }
                if (this.SubgroupNIds != string.Empty)
                {
                    string strSubgroupNIds = DIQueries.Icon.GetIcon(this.SubgroupNIds, IconElementType.SubgroupVals);
                    DTSubgroup = DIConnection.ExecuteDataTable(strSubgroupNIds);
                }
                if (this.AreaNIds != string.Empty)
                {
                    string strAreaNIds = DIQueries.Icon.GetIcon(this.AreaNIds, IconElementType.Area);
                    DTArea = DIConnection.ExecuteDataTable(strAreaNIds);
                }



                DataRow[] RowsIcons;
                byte[] Icon;
                string CellValue;

                int rowcount = 0;

                if (this._ColumnArrangementTable != null && this._ColumnArrangementTable.Rows.Count > 0)
                {
                    //-- sort the DtImages on the basis of order column.
                    DataView DvImages = DTImage.DefaultView;
                    DvImages.Sort = Order;
                    DTImage = DvImages.ToTable();
                }

                foreach (DataRow DRow in this.DTImage.Rows)
                {
                    for (int Columns = 0; Columns < DTImage.Columns.Count - 2; Columns += 2)
                    {
                        if (DRow[Columns].ToString() == IconElementType.Indicator.ToString())
                        {
                            if (DTIndicator != null)
                            {
                                RowsIcons = DTIndicator.Select(Icons.ElementNId + "='" + DRow[Columns + One].ToString() + "'");
                                if (RowsIcons.Length > 0)
                                {
                                    Icon = (byte[])RowsIcons[0][Icons.ElementIcon];
                                    CellValue = ExcelFile.GetCellValue(sheetIndex, TitleSubTitleMargin + this.Fields.Columns[Indicator.IndicatorName].FieldIndex, this._Fields.Rows.Count + rowcount, TitleSubTitleMargin + this.Fields.Columns[Indicator.IndicatorName].FieldIndex, this._Fields.Rows.Count + rowcount);
                                    ExcelFile.PasteImage(sheetIndex, Icon, this._Fields.Rows.Count + rowcount, TitleSubTitleMargin + this.Fields.Columns[Indicator.IndicatorName].FieldIndex);
                                    ExcelFile.SetCellValue(sheetIndex, TitleSubTitleMargin + this.Fields.Columns[Indicator.IndicatorName].FieldIndex, this._Fields.Rows.Count + rowcount, CellValue);

                                    //Icon = (byte[])RowsIcons[0][Icons.ElementIcon];
                                    //CellValue = ExcelFile.GetCellValue(sheetIndex, this._Fields.Rows.Count + Columns / 2, TitleSubTitleMargin + rowcount, this._Fields.Rows.Count + Columns / 2, TitleSubTitleMargin + rowcount);
                                    //ExcelFile.PasteImage(sheetIndex, Icon, TitleSubTitleMargin + rowcount, this._Fields.Rows.Count + Columns / 2);
                                    //ExcelFile.SetCellValue(sheetIndex, this._Fields.Rows.Count + Columns / 2, TitleSubTitleMargin + rowcount, "	 " + CellValue);
                                }
                            }
                        }
                        if (DRow[Columns].ToString() == IconElementType.IndicatorClassification.ToString())
                        {
                            if (DTIndicatorClassification != null)
                            {
                                RowsIcons = DTIndicatorClassification.Select(Icons.ElementNId + "='" + DRow[Columns + One].ToString() + "'");
                                if (RowsIcons.Length > 0)
                                {
                                    Icon = (byte[])RowsIcons[0][Icons.ElementIcon];
                                    CellValue = ExcelFile.GetCellValue(sheetIndex, TitleSubTitleMargin + this.Fields.Columns[IndicatorClassifications.ICName].FieldIndex, this._Fields.Rows.Count + rowcount, TitleSubTitleMargin + this.Fields.Columns[IndicatorClassifications.ICName].FieldIndex, this._Fields.Rows.Count + rowcount);
                                    ExcelFile.PasteImage(sheetIndex, Icon, this._Fields.Rows.Count + rowcount, TitleSubTitleMargin + this.Fields.Columns[IndicatorClassifications.ICName].FieldIndex);
                                    ExcelFile.SetCellValue(sheetIndex, TitleSubTitleMargin + this.Fields.Columns[IndicatorClassifications.ICName].FieldIndex, this._Fields.Rows.Count + rowcount, CellValue);


                                    //Icon = (byte[])RowsIcons[0][Icons.ElementIcon];
                                    //CellValue = ExcelFile.GetCellValue(sheetIndex, this._Fields.Rows.Count + Columns / 2, TitleSubTitleMargin + rowcount, this._Fields.Rows.Count + Columns / 2, TitleSubTitleMargin + rowcount);
                                    //ExcelFile.PasteImage(sheetIndex, Icon, TitleSubTitleMargin + rowcount, this._Fields.Rows.Count + Columns / 2);
                                    //ExcelFile.SetCellValue(sheetIndex, this._Fields.Rows.Count + Columns / 2, TitleSubTitleMargin + rowcount, "	 " + CellValue);
                                }
                            }
                        }
                        if (DRow[Columns].ToString() == IconElementType.Area.ToString())
                        {
                            if (DTArea != null)
                            {
                                RowsIcons = DTArea.Select(Icons.ElementNId + "='" + DRow[Columns + One].ToString() + "'");
                                if (RowsIcons.Length > 0)
                                {
                                    Icon = (byte[])RowsIcons[0][Icons.ElementIcon];
                                    CellValue = ExcelFile.GetCellValue(sheetIndex, TitleSubTitleMargin + this.Fields.Columns[Area.AreaName].FieldIndex, this._Fields.Rows.Count + rowcount, TitleSubTitleMargin + this.Fields.Columns[Area.AreaName].FieldIndex, this._Fields.Rows.Count + rowcount);
                                    ExcelFile.PasteImage(sheetIndex, Icon, this._Fields.Rows.Count + rowcount, TitleSubTitleMargin + this.Fields.Columns[Area.AreaName].FieldIndex);
                                    ExcelFile.SetCellValue(sheetIndex, TitleSubTitleMargin + this.Fields.Columns[Area.AreaName].FieldIndex, this._Fields.Rows.Count + rowcount, CellValue);
                                }
                            }
                        }
                        if (DRow[Columns].ToString() == IconElementType.Unit.ToString())
                        {
                            if (DTUnit != null)
                            {
                                RowsIcons = DTUnit.Select(Icons.ElementNId + "='" + DRow[Columns + One].ToString() + "'");
                                if (RowsIcons.Length > 0)
                                {
                                    Icon = (byte[])RowsIcons[0][Icons.ElementIcon];
                                    CellValue = ExcelFile.GetCellValue(sheetIndex, TitleSubTitleMargin + this.Fields.Columns[Unit.UnitName].FieldIndex, this._Fields.Rows.Count + rowcount, TitleSubTitleMargin + this.Fields.Columns[Unit.UnitName].FieldIndex, this._Fields.Rows.Count + rowcount);
                                    ExcelFile.PasteImage(sheetIndex, Icon, this._Fields.Rows.Count + rowcount, TitleSubTitleMargin + this.Fields.Columns[Unit.UnitName].FieldIndex);
                                    ExcelFile.SetCellValue(sheetIndex, TitleSubTitleMargin + this.Fields.Columns[Unit.UnitName].FieldIndex, this._Fields.Rows.Count + rowcount, CellValue);

                                    //Icon = (byte[])RowsIcons[0][Icons.ElementIcon];
                                    //CellValue = ExcelFile.GetCellValue(sheetIndex, this._Fields.Rows.Count + Columns / 2, TitleSubTitleMargin + rowcount, this._Fields.Rows.Count + Columns / 2, TitleSubTitleMargin + rowcount);
                                    //ExcelFile.PasteImage(sheetIndex, Icon, TitleSubTitleMargin + rowcount, this._Fields.Rows.Count + Columns / 2);
                                    //ExcelFile.SetCellValue(sheetIndex, this._Fields.Rows.Count + Columns / 2, TitleSubTitleMargin + rowcount, "	 " + CellValue);
                                }
                            }
                        }
                        if (DRow[Columns].ToString() == IconElementType.SubgroupVals.ToString())
                        {
                            if (DTSubgroup != null)
                            {
                                RowsIcons = DTSubgroup.Select(Icons.ElementNId + "='" + DRow[Columns + One].ToString() + "'");
                                if (RowsIcons.Length > 0)
                                {
                                    Icon = (byte[])RowsIcons[0][Icons.ElementIcon];
                                    CellValue = ExcelFile.GetCellValue(sheetIndex, TitleSubTitleMargin + this.Fields.Columns[SubgroupVals.SubgroupVal].FieldIndex, this._Fields.Rows.Count + rowcount, TitleSubTitleMargin + this.Fields.Columns[SubgroupVals.SubgroupVal].FieldIndex, this._Fields.Rows.Count + rowcount);
                                    ExcelFile.PasteImage(sheetIndex, Icon, this._Fields.Rows.Count + rowcount, TitleSubTitleMargin + this.Fields.Columns[SubgroupVals.SubgroupVal].FieldIndex);
                                    ExcelFile.SetCellValue(sheetIndex, TitleSubTitleMargin + this.Fields.Columns[SubgroupVals.SubgroupVal].FieldIndex, this._Fields.Rows.Count + rowcount, CellValue);


                                    //Icon = (byte[])RowsIcons[0][Icons.ElementIcon];
                                    //CellValue = ExcelFile.GetCellValue(sheetIndex, this._Fields.Rows.Count + Columns / 2, TitleSubTitleMargin + rowcount, this._Fields.Rows.Count + Columns / 2, TitleSubTitleMargin + rowcount);
                                    //ExcelFile.PasteImage(sheetIndex, Icon, TitleSubTitleMargin + rowcount, this._Fields.Rows.Count + Columns / 2);
                                    //ExcelFile.SetCellValue(sheetIndex, this._Fields.Rows.Count + Columns / 2, TitleSubTitleMargin + rowcount, "	 " + CellValue);
                                }
                            }
                        }

                    }
                    if (((this._TemplateStyle.Comments.Show && (this.DTComments != null & this.DTComments.Rows.Count > 0)) && (this._TemplateStyle.Footnotes.Show && (this.DTFootNote != null & this.DTFootNote.Rows.Count > 0))) && this._TemplateStyle.Denominator.Show)
                    //if (this._CommentShow && (this.DTComments != null & this.DTComments.Rows.Count > 0))
                    {
                        rowcount = rowcount + 4;
                    }
                    else if (((this._TemplateStyle.Comments.Show && (this.DTComments != null & DTComments.Rows.Count > 0)) || (this._TemplateStyle.Footnotes.Show && (DTFootNote != null & DTFootNote.Rows.Count > 0))) && this._TemplateStyle.Denominator.Show)
                    {
                        rowcount = rowcount + 3;
                    }
                    else if ((this._TemplateStyle.Footnotes.Show && (DTFootNote != null & DTFootNote.Rows.Count > 0)) && (this._TemplateStyle.Comments.Show && (this.DTComments != null & DTComments.Rows.Count > 0)))
                    {
                        rowcount = rowcount + 3;
                    }
                    else if ((this._TemplateStyle.Footnotes.Show && (DTFootNote != null & DTFootNote.Rows.Count > 0)) || (this._TemplateStyle.Comments.Show && (this.DTComments != null & DTComments.Rows.Count > 0)))
                    {
                        rowcount = rowcount + 2;
                    }
                    else
                    {
                        rowcount++;
                    }
                }


                DataRow drIcons;
                DataView DVXLS = TableXLS.DefaultView;
                DVXLS.RowFilter = "RowType<>'DataRow'";

                foreach (DataRow dr in DVXLS.ToTable().Rows)
                {
                    if (dr["RowIndex"].ToString() != string.Empty)
                    {
                        if (dr["RowType"].ToString() != "ICAggregate")
                        {
                            if (Convert.ToInt32(dr["RowIndex"].ToString()) - 1 - this._Fields.Columns.Count - 1 >= 0)
                            {
                                drIcons = DTRowsForIcons.NewRow();
                                DTRowsForIcons.Rows.InsertAt(drIcons, Convert.ToInt32(dr["RowIndex"].ToString()) - ColumnArrangementColumns - 1);

                            }
                        }
                    }
                }

                //Set images for Rows collection.
                for (Rows = 0; Rows < this.DTRowsForIcons.Rows.Count; Rows++)
                {
                    for (int Columns = 0; Columns < this.DTRowsForIcons.Columns.Count; Columns = Columns + 2)
                    {
                        if (this.DTRowsForIcons.Rows[Rows][Columns].ToString() == IconElementType.Indicator.ToString())
                        {

                            if (DTIndicator != null)
                            {
                                RowsIcons = DTIndicator.Select(Icons.ElementNId + "='" + this.DTRowsForIcons.Rows[Rows][Columns + One].ToString() + "'");
                                if (RowsIcons.Length > 0)
                                {
                                    Icon = (byte[])RowsIcons[0][Icons.ElementIcon];
                                    CellValue = ExcelFile.GetCellValue(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns + Rows, Columns / 2, TitleSubTitleMargin + ColumnArrangementColumns + Rows, Columns / 2);
                                    ExcelFile.PasteImage(sheetIndex, Icon, Columns / 2, TitleSubTitleMargin + ColumnArrangementColumns + Rows);
                                    ExcelFile.SetCellValue(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns + Rows, Columns / 2, "	    " + CellValue);
                                }
                            }
                        }
                        if (this.DTRowsForIcons.Rows[Rows][Columns].ToString() == IconElementType.Area.ToString())
                        {
                            if (DTArea != null)
                            {
                                RowsIcons = DTArea.Select(Icons.ElementNId + "='" + this.DTRowsForIcons.Rows[Rows][Columns + One].ToString() + "'");
                                if (RowsIcons.Length > 0)
                                {
                                    Icon = (byte[])RowsIcons[0][Icons.ElementIcon];
                                    CellValue = ExcelFile.GetCellValue(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns + Rows, Columns / 2, TitleSubTitleMargin + ColumnArrangementColumns + Rows, Columns / 2);
                                    ExcelFile.PasteImage(sheetIndex, Icon, Columns / 2, TitleSubTitleMargin + ColumnArrangementColumns + Rows);
                                    ExcelFile.SetCellValue(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns + Rows, Columns / 2, "	    " + CellValue);
                                }
                            }
                        }
                        if (this.DTRowsForIcons.Rows[Rows][Columns].ToString() == IconElementType.IndicatorClassification.ToString())
                        {
                            if (DTIndicatorClassification != null)
                            {
                                RowsIcons = DTIndicatorClassification.Select(Icons.ElementNId + "='" + this.DTRowsForIcons.Rows[Rows][Columns + One].ToString() + "'");
                                if (RowsIcons.Length > 0)
                                {
                                    Icon = (byte[])RowsIcons[0][Icons.ElementIcon];
                                    CellValue = ExcelFile.GetCellValue(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns + Rows, Columns / 2, TitleSubTitleMargin + ColumnArrangementColumns + Rows, Columns / 2);
                                    ExcelFile.PasteImage(sheetIndex, Icon, Columns / 2, TitleSubTitleMargin + ColumnArrangementColumns + Rows);
                                    ExcelFile.SetCellValue(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns + Rows, Columns / 2, "	    " + CellValue);
                                }
                            }
                        }
                        if (this.DTRowsForIcons.Rows[Rows][Columns].ToString() == IconElementType.SubgroupVals.ToString())
                        {
                            if (DTSubgroup != null)
                            {
                                RowsIcons = DTSubgroup.Select(Icons.ElementNId + "='" + this.DTRowsForIcons.Rows[Rows][Columns + One].ToString() + "'");
                                if (RowsIcons.Length > 0)
                                {
                                    Icon = (byte[])RowsIcons[0][Icons.ElementIcon];
                                    CellValue = ExcelFile.GetCellValue(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns + Rows, Columns / 2, TitleSubTitleMargin + ColumnArrangementColumns + Rows, Columns / 2);
                                    ExcelFile.PasteImage(sheetIndex, Icon, Columns / 2, TitleSubTitleMargin + ColumnArrangementColumns + Rows);
                                    ExcelFile.SetCellValue(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns + Rows, Columns / 2, "	    " + CellValue);
                                }
                            }
                        }
                        if (this.DTRowsForIcons.Rows[Rows][Columns].ToString() == IconElementType.Unit.ToString())
                        {
                            if (DTUnit != null)
                            {
                                RowsIcons = DTUnit.Select(Icons.ElementNId + "='" + this.DTRowsForIcons.Rows[Rows][Columns + One].ToString() + "'");
                                if (RowsIcons.Length > 0)
                                {
                                    Icon = (byte[])RowsIcons[0][Icons.ElementIcon];
                                    CellValue = ExcelFile.GetCellValue(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns + Rows, Columns / 2, TitleSubTitleMargin + ColumnArrangementColumns + Rows, Columns / 2);
                                    ExcelFile.PasteImage(sheetIndex, Icon, Columns / 2, TitleSubTitleMargin + ColumnArrangementColumns + Rows);
                                    ExcelFile.SetCellValue(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns + Rows, Columns / 2, "	    " + CellValue);
                                }
                            }
                        }
                    }
                }
            }

            #endregion

            #endregion

            #region "-- Sources WorkSheet --"

            sheetIndex = this._XLSSheetIndex + 2;
            ExcelFile.SetCellValue(sheetIndex, 0, 0, this.LanguageStrings.Source);
            ExcelFile.SetCellValue(sheetIndex, 0, 2, DIConnection.ConnectionStringParameters.DbName);

            if (TableSourcesArray.GetLength(0) == One)
            {
                //colunm length is increased to one as in spreadsheet doen't provide range to null when rows and column are set to One.
                ExcelFile.SetArrayValuesIntoSheet(sheetIndex, TitleSubTitleMargin, 0, this.DTSources.Rows.Count + TitleSubTitleMargin, 2, TableSourcesArray);
            }
            else
            {
                ExcelFile.SetArrayValuesIntoSheet(sheetIndex, TitleSubTitleMargin, 0, this.DTSources.Rows.Count + TitleSubTitleMargin, One, TableSourcesArray);
            }


            #endregion

            #region	"--	Data WorkSheet --"

            sheetIndex = this._XLSSheetIndex + 1;
            //Pass Array containing TableXLS values to prepare XLS Worksheet.
            ExcelFile.SetRangeFormatType(sheetIndex, TitleSubTitleMargin + One, 0, this.DTDataSheet.Rows.Count + TitleSubTitleMargin + 1, this.DTDataSheet.Columns.Count, SpreadsheetGear.NumberFormatType.Text);
            ExcelFile.SetArrayValuesIntoSheet(sheetIndex, TitleSubTitleMargin + One, 0, this.DTDataSheet.Rows.Count + TitleSubTitleMargin + 1, this.DTDataSheet.Columns.Count, TableDataSheetArray);

            #endregion

            #region "-- Call Suppress duplicate methos --"

            sheetIndex = this._XLSSheetIndex;
            //Suppress duplicate Headings.
            this.SuppressColumnCount = 0;

            if (this.IsSuppressDuplicateColumns)
            {
                this.SuppressDuplicateColumn(ExcelFile, sheetIndex, TableXLS, borderColor);
            }
            if (this.IsSuppressDuplicateRows)
            {
                int SubtractFromRows = 1;
                if (this.TableXLS.Columns.Contains(IndicatorClassifications.ICName) && this._Fields.Rows.Count > 1 && this.TableXLS.Columns[0].ColumnName != IndicatorClassifications.ICName && this._MoveSourceToLast == true && this.TableXLS.Columns[this._Fields.Rows.Count - 1].ColumnName == IndicatorClassifications.ICName)
                {
                    SubtractFromRows = 2;
                }
                this.SuppressDuplicateRows(ExcelFile, sheetIndex, TableXLS, borderColor, SubtractFromRows);
            }


            #endregion

            #region "-- Move Source Column to end - For Table only --"
            //Move Source to Last Position 
            //It will not move if
            //It is first column or second column
            if (this._PresentationType != Presentation.PresentationType.Graph && this.TableXLS.Columns[this._Fields.Rows.Count - 1].ColumnName == IndicatorClassifications.ICName)
            {
                int ICPosition = -1;
                if (this.TableXLS.Columns.Contains(IndicatorClassifications.ICName) && this._Fields.Rows.Count > 1 && this.TableXLS.Columns[0].ColumnName != IndicatorClassifications.ICName && this._MoveSourceToLast == true)
                {
                    ICPosition = this.TableXLS.Columns[IndicatorClassifications.ICName].Ordinal;
                }

                if (ICPosition != -1)
                {
                    SourceColumnHeader = ExcelFile.GetColumnHeader(sheetIndex, 1, ICPosition);
                    string DestinationColumnHeader = ExcelFile.GetColumnHeader(sheetIndex, 1, this.TableXLS.Columns.Count - (4 + this.MaxMinAreaLevelDiff));
                    ExcelFile.MoveColumnTo(sheetIndex, SourceColumnHeader, DestinationColumnHeader);
                    ExcelFile.DeleteColumnAt(sheetIndex, SourceColumnHeader);
                    ExcelFile.SetRangeBorders(sheetIndex, TitleSubTitleMargin, TotalXLSColumns, TableXLS.Rows.Count + TitleSubTitleMargin + this.SuppressColumnCount - 1, TotalXLSColumns, SpreadsheetGear.LineStyle.Continuous, SpreadsheetGear.BorderWeight.Thin, borderColor);
                }
            }

            if (this._PresentationType != Presentation.PresentationType.Graph)
            {
                if (this._AddColor)
                {
                    if (this.Fields.Rows[DICommon.RemoveQuotes(Indicator.IndicatorName)] != null || this.Fields.Columns[DICommon.RemoveQuotes(Indicator.IndicatorName)] != null || this._DistinctIndicator.Rows.Count == 1)
                    {
                        StartRow = TableXLS.Rows.Count + TitleSubTitleMargin + TotalTableCommentsRows;
                        if (this.FootnoteInLine == FootNoteDisplayStyle.Separate)
                        {
                            StartRow += TotalTableFootNoteRows;
                        }
                        if (TotalTableFootNoteRows > 0 && this.FootnoteInLine == FootNoteDisplayStyle.Separate)
                        {
                            StartRow += TitleSubTitleMargin;
                        }
                        if (TotalTableCommentsRows > 0)
                        {
                            StartRow += TitleSubTitleMargin;
                        }
                        //Set style for Legends displayed at th bottom of TableXLS Sheet.
                        for (int Count = 0; Count < this.Themes[0].Legends.Count; Count++)
                        {
                            ExcelFile.SetCellColor(sheetIndex, StartRow + Count + One + this.SuppressColumnCount, 0, Color.Empty, this.Themes[0].Legends[Count].Color);
                            if (!string.IsNullOrEmpty(this.Themes[0].Legends[Count].Caption))
                            {
                                //-- Fet the formated range
                                string RangeValue = this.Themes[0].Legends[Count].Range;
                                if (this._TemplateStyle.ContentSetting.FontTemplate.RoundDataValues || this._TemplateStyle.ContentSetting.FontTemplate.FormatDataValue)
                                {
                                    if (this._TemplateStyle.ContentSetting.FontTemplate.RoundDataValues)
                                    {
                                        RangeValue = String.Format(this.NumberFormat(this._TemplateStyle.ContentSetting.FontTemplate.DecimalPlace, this.Themes[0].Legends[Count].RangeFrom.ToString()), this.Themes[0].Legends[Count].RangeFrom) + " - " + string.Format(this.NumberFormat(this._TemplateStyle.ContentSetting.FontTemplate.DecimalPlace, this.Themes[0].Legends[Count].RangeTo.ToString()), this.Themes[0].Legends[Count].RangeTo);
                                    }
                                    else
                                    {
                                        RangeValue = String.Format(this.NumberFormat(this.Themes[0].Decimal, this.Themes[0].Legends[Count].RangeFrom.ToString()), this.Themes[0].Legends[Count].RangeFrom) + " - " + string.Format(this.NumberFormat(this.Themes[0].Decimal, this.Themes[0].Legends[Count].RangeTo.ToString()), this.Themes[0].Legends[Count].RangeTo);
                                    }
                                }

                                if (this._TemplateStyle.Legends.ShowCaption && this._TemplateStyle.Legends.ShowRange)
                                {
                                    //-- Display both caption and range.
                                    ExcelFile.SetCellValue(sheetIndex, StartRow + Count + One + this.SuppressColumnCount, One, this.Themes[0].Legends[Count].Caption + "  " + RangeValue);
                                }
                                else if (this._TemplateStyle.Legends.ShowCaption)
                                {
                                    //-- Display only caption.
                                    ExcelFile.SetCellValue(sheetIndex, StartRow + Count + One + this.SuppressColumnCount, One, this.Themes[0].Legends[Count].Caption);
                                }
                                else if (this._TemplateStyle.Legends.ShowRange)
                                {
                                    //-- Display only range.
                                    ExcelFile.SetCellFormatType(sheetIndex, StartRow + Count + One + this.SuppressColumnCount, One, SpreadsheetGear.NumberFormatType.Text);
                                    ExcelFile.SetCellValue(sheetIndex, StartRow + Count + One + this.SuppressColumnCount, One, RangeValue);
                                }
                                ExcelFile.SetCellColor(sheetIndex, StartRow + Count + One + this.SuppressColumnCount, One, this._TemplateStyle.Legends.FontTemplate.ForeColor, this.TemplateStyle.Legends.FontTemplate.BackColor);
                                ExcelFile.GetRangeFont(sheetIndex, StartRow + Count + One + this.SuppressColumnCount, One, StartRow + Count + One + this.SuppressColumnCount, One).Name = this._TemplateStyle.Legends.FontTemplate.FontName;
                                ExcelFile.GetRangeFont(sheetIndex, StartRow + Count + One + this.SuppressColumnCount, One, StartRow + Count + One + this.SuppressColumnCount, One).Size = this._TemplateStyle.Legends.FontTemplate.FontSize;
                                this.SetExcelRangeFont(ExcelFile.GetRangeFont(sheetIndex, StartRow + Count + One + this.SuppressColumnCount, One, StartRow + Count + One + this.SuppressColumnCount, One), this._TemplateStyle.Legends.FontTemplate.FontStyle);
                            }
                            else
                            {
                                if (this._TemplateStyle.Legends.ShowCaption)
                                {
                                    //-- If show caption is on.
                                    ExcelFile.SetCellFormatType(sheetIndex, StartRow + Count + One + this.SuppressColumnCount, One, SpreadsheetGear.NumberFormatType.Text);
                                    ExcelFile.SetCellValue(sheetIndex, StartRow + Count + One + this.SuppressColumnCount, One, this.Themes[0].Legends[Count].Range);
                                    ExcelFile.SetCellColor(sheetIndex, StartRow + Count + One + this.SuppressColumnCount, One, this._TemplateStyle.Legends.FontTemplate.ForeColor, this.TemplateStyle.Legends.FontTemplate.BackColor);
                                    ExcelFile.GetCellFont(sheetIndex, StartRow + Count + One + this.SuppressColumnCount, One).Name = this._TemplateStyle.Legends.FontTemplate.FontName;
                                    ExcelFile.GetCellFont(sheetIndex, StartRow + Count + One + this.SuppressColumnCount, One).Size = this._TemplateStyle.Legends.FontTemplate.FontSize;
                                    this.SetExcelRangeFont(ExcelFile.GetRangeFont(sheetIndex, StartRow + Count + One + this.SuppressColumnCount, One, StartRow + Count + One + this.SuppressColumnCount, One), this._TemplateStyle.Legends.FontTemplate.FontStyle);
                                }
                            }
                        }
                    }
                }

            }

            #endregion

            #region "-- Merge cells for Title and Subtitle --"
            ExcelFile.MergeCells(sheetIndex, 0, 0, 0, TotalXLSColumns);
            ExcelFile.WrapText(sheetIndex, 0, 0, 0, TotalXLSColumns, true);
            ExcelFile.MergeCells(sheetIndex, One, 0, One, TotalXLSColumns);
            ExcelFile.WrapText(sheetIndex, One, 0, One, TotalXLSColumns, true);

            #endregion

            BlankRows = TitleSubTitleMargin;

            #region "-- Merge and Wrap Foornote Rows --
            if (TableFootNote != null && (this._PresentationType == Presentation.PresentationType.Table || this._PresentationType == Presentation.PresentationType.FrequencyTable))
            {
                for (int FootnoteRowCount = 0; FootnoteRowCount < TableFootNote.Rows.Count; FootnoteRowCount++)
                {
                    ExcelFile.MergeCells(sheetIndex, totalRows + BlankRows + FootnoteRowCount, 0, totalRows + BlankRows + FootnoteRowCount, TotalXLSColumns);
                    ExcelFile.WrapText(sheetIndex, totalRows + BlankRows + FootnoteRowCount, 0, totalRows + BlankRows + FootnoteRowCount, TotalXLSColumns, true);
                }

                if (TableFootNote.Rows.Count > 0)
                {
                    BlankRows += TitleSubTitleMargin + TableFootNote.Rows.Count;
                }
            }

            #endregion

            //Set the Sources.
            if (this.AddColor)
            {

                BlankRows += this.Themes[0].BreakCount;
            }

            #region "-- Merge and Wrap text for Source and also apply the style--"

            if (this.DTSources != null && (this._PresentationType == Presentation.PresentationType.Table || this._PresentationType == Presentation.PresentationType.FrequencyTable))
            {
                for (int SourceRowCount = 0; SourceRowCount < this.DTSources.Rows.Count; SourceRowCount++)
                {
                    ExcelFile.MergeCells(sheetIndex, totalRows + BlankRows + SourceRowCount, 0, totalRows + BlankRows + SourceRowCount, TotalXLSColumns);
                    ExcelFile.WrapText(sheetIndex, totalRows + BlankRows + SourceRowCount, 0, totalRows + BlankRows + SourceRowCount, TotalXLSColumns, true);


                    ExcelFile.GetCellFont(sheetIndex, totalRows + BlankRows + SourceRowCount, 0).Name = "Arial";
                    ExcelFile.GetCellFont(sheetIndex, totalRows + BlankRows + SourceRowCount, 0).Size = 7;
                }
            }

            #endregion

            #region "-- Suppress Duplicat - Merge cells for Group header -- "
            if (this.IsSuppressDuplicateRows)
            {
                int TotalColumnFields = (this._ColumnArrangementTable.Columns.Count - 2) / 2;
                int SubtractFromRows = 1;
                int SuppressRowCounter = 0;
                string TableRowType = string.Empty;
                if (TotalColumnFields == 0)
                {
                    if (this._Fields.Rows.Count == 1)
                    {
                        ExcelFile.SetCellValue(sheetIndex, TitleSubTitleMargin + TotalColumnFields, 0, string.Empty);
                    }
                    else
                    {
                        ExcelFile.MergeCells(sheetIndex, TitleSubTitleMargin + TotalColumnFields, 0, TitleSubTitleMargin + TotalColumnFields, this._Fields.Rows.Count - 1);
                        ExcelFile.WrapText(sheetIndex, TitleSubTitleMargin + TotalColumnFields, 0, TitleSubTitleMargin + TotalColumnFields, this._Fields.Rows.Count - 1, true);
                    }
                }
                else
                {
                    if (this._Fields.Rows.Count == 1)
                    {
                        ExcelFile.SetCellValue(sheetIndex, TitleSubTitleMargin + TotalColumnFields - 1, 0, string.Empty);
                    }
                    else
                    {
                        ExcelFile.MergeCells(sheetIndex, TitleSubTitleMargin + TotalColumnFields - 1, 0, TitleSubTitleMargin + TotalColumnFields - 1, this._Fields.Rows.Count - 1);
                        ExcelFile.WrapText(sheetIndex, TitleSubTitleMargin + TotalColumnFields - 1, 0, TitleSubTitleMargin + TotalColumnFields - 1, this._Fields.Rows.Count - 1, true);
                    }
                }
                if (this.TableXLS.Columns.Contains(IndicatorClassifications.ICName) && this._Fields.Rows.Count > 1 && this.TableXLS.Columns[0].ColumnName != IndicatorClassifications.ICName && this._MoveSourceToLast == true && this.TableXLS.Columns[this._Fields.Rows.Count - 1].ColumnName == IndicatorClassifications.ICName)
                {
                    SubtractFromRows = 2;
                }
                foreach (DataRow Drow in TableXLS.Select(TablePresentation.ROWTYPE + "='" + RowType.SupressRow.ToString() + "'"))
                {
                    //ExcelFile.SetColumnWidth(sheetIndex, 2, Convert.ToInt32(Drow[TablePresentation.ROWINDEX].ToString()) + TitleSubTitleMargin - 1, 0, Convert.ToInt32(Drow[TablePresentation.ROWINDEX].ToString()) + TitleSubTitleMargin, 0);
                    ////if (TableRowType == RowType.DataRow.ToString())
                    ////{
                    ////    SuppressRowCounter = 0;
                    ////}
                    for (int ColCount = 0; ColCount < this._Fields.Rows.Count - SubtractFromRows; ColCount++)
                    {
                        if (Drow[ColCount].ToString() == string.Empty)
                        {
                            SuppressRowCounter++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    ExcelFile.MergeCells(sheetIndex, Convert.ToInt32(Drow[TablePresentation.ROWINDEX].ToString()) + TitleSubTitleMargin - 1, SuppressRowCounter, Convert.ToInt32(Drow[TablePresentation.ROWINDEX].ToString()) + TitleSubTitleMargin - 1, TableXLS.Columns.Count - 5);
                    ExcelFile.WrapText(sheetIndex, Convert.ToInt32(Drow[TablePresentation.ROWINDEX].ToString()) + TitleSubTitleMargin - 1, SuppressRowCounter, Convert.ToInt32(Drow[TablePresentation.ROWINDEX].ToString()) + TitleSubTitleMargin - 1, TableXLS.Columns.Count - 5, true);
                    SuppressRowCounter = 0;
                    ////int RowNumber = Convert.ToInt16(Drow[ROWINDEX].ToString());
                    ////TableRowType = TableXLS.Rows[RowNumber][ROWTYPE].ToString();
                    ////SuppressRowCounter++;                    
                    //ExcelFile.SetHorizontalAlignment(sheetIndex, Convert.ToInt32(Drow[TablePresentation.ROWINDEX].ToString()) + TitleSubTitleMargin - 1, 0, SpreadsheetGear.HAlign.Left);
                    //this.SetExcelRangeFont(ExcelFile.GetCellFont(sheetIndex, Convert.ToInt32(Drow[TablePresentation.ROWINDEX].ToString()) + TitleSubTitleMargin - 1, 0), FontStyle.Bold);

                    //if (this._Fields.Rows.Count > 2)
                    //{
                    //    for (int ColCount = 1; ColCount < this._Fields.Rows.Count - SubtractFromRows; ColCount++)
                    //    {
                    //        //this.SetExcelRangeFont(ExcelFile.GetCellFont(sheetIndex, Convert.ToInt32(Drow[TablePresentation.ROWINDEX].ToString()) + TitleSubTitleMargin - 1, ColCount), FontStyle.Bold);
                    //        //ExcelFile.SetColumnWidth(sheetIndex, 2, Convert.ToInt32(Drow[TablePresentation.ROWINDEX].ToString()) + TitleSubTitleMargin - 1, ColCount, Convert.ToInt32(Drow[TablePresentation.ROWINDEX].ToString()) + TitleSubTitleMargin, ColCount);
                    //        ExcelFile.MergeCells(sheetIndex, Convert.ToInt32(Drow[TablePresentation.ROWINDEX].ToString()) + TitleSubTitleMargin - 1, ColCount, Convert.ToInt32(Drow[TablePresentation.ROWINDEX].ToString()) + TitleSubTitleMargin - 1, TableXLS.Columns.Count);
                    //    }
                    //}
                }
            }
            #endregion

            #region "-- Delete Rows for Title and SubTitle if they are empty --"

            //Delete Rows for Title and SubTitle if they are empty
            if (this._PresentationType == Presentation.PresentationType.Table || this._PresentationType == Presentation.PresentationType.FrequencyTable)
            {
                if (this._Title == string.Empty && this._Subtitle == string.Empty && this._HideTitleSubtitleRows == true)
                {
                    for (int i = 0; i < TitleSubTitleMargin; i++)
                    {
                        ExcelFile.DeleteRowAt(sheetIndex, 1);
                    }
                }
            }
            else if (this._PresentationType == Presentation.PresentationType.Graph)
            {
                if (this._HideTitleSubtitleRows == true)
                {
                    for (int i = 0; i < TitleSubTitleMargin; i++)
                    {
                        ExcelFile.DeleteRowAt(sheetIndex, 1);
                    }
                }
            }

            #endregion

            #region "-- Save Excell

            //Save and close the ExcelFile.
            ExcelFile.SelectCell(sheetIndex, 0, TableXLS.Rows.Count - 1);

            ExcelFile.ActivateCell(sheetIndex, 0, TableXLS.Rows.Count - 1);
            this._TableXLS = TableXLS;
            ExcelFile.Save();
            ExcelFile.Close();

            #endregion
        }

        /// <summary>
        /// Set style for Subnational report
        /// </summary>
        /// <param name="sheetIndex">sheet index of excell file</param>
        /// <param name="ColumnArrangementColumns">Number of columns for table prasentation</param>
        /// <param name="StartRowCount">starting index from where style gets applied</param>
        /// <param name="FinalRowCount">Final index for style</param>
        /// <param name="totalColumns">Total column in TableXLS application for style</param>
        /// <param name="AreaLevel">Area Level</param>
        /// <param name="ExcelFile">object of Excell/SpreadsheetGear file</param>
        private void SetStyleForSubnationalReport(int sheetIndex, int ColumnArrangementColumns, int StartRowCount, int FinalRowCount, int totalColumns, string AreaLevel, DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper.DIExcel ExcelFile, int LastLevel)
        {
            if (Convert.ToInt32(AreaLevel) < LastLevel && AreaLevel != "-1")
            {
                if (this._TemplateStyle.LevelFormat[AreaLevel].ShowDataValues)
                {
                    ExcelFile.SetRangeColor(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns + StartRowCount, 0, TitleSubTitleMargin + FinalRowCount, totalColumns - 1, this.TemplateStyle.LevelFormat[AreaLevel].FontSetting.ForeColor, this.TemplateStyle.LevelFormat[AreaLevel].FontSetting.BackColor);
                }
                else
                {
                    ExcelFile.SetRangeColor(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns + StartRowCount, 0, TitleSubTitleMargin + FinalRowCount, this._Fields.Rows.Count - 1, this.TemplateStyle.LevelFormat[AreaLevel].FontSetting.ForeColor, this.TemplateStyle.LevelFormat[AreaLevel].FontSetting.BackColor);
                    ExcelFile.SetRangeColor(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns + StartRowCount, this._Fields.Rows.Count, TitleSubTitleMargin + FinalRowCount, totalColumns - 1, this.TemplateStyle.LevelFormat[AreaLevel].FontSetting.BackColor, this.TemplateStyle.LevelFormat[AreaLevel].FontSetting.BackColor);
                }
                ExcelFile.GetRangeFont(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns + StartRowCount, 0, TitleSubTitleMargin + FinalRowCount, totalColumns - 1).Name = this._TemplateStyle.LevelFormat[AreaLevel].FontSetting.FontName;
                ExcelFile.GetRangeFont(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns + StartRowCount, 0, TitleSubTitleMargin + FinalRowCount, totalColumns - 1).Size = this._TemplateStyle.LevelFormat[AreaLevel].FontSetting.FontSize;
                this.SetExcelRangeFont(ExcelFile.GetRangeFont(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns + StartRowCount, 0, TitleSubTitleMargin + FinalRowCount, totalColumns - 1), this._TemplateStyle.LevelFormat[AreaLevel].FontSetting.FontStyle);

                //Get the Border height and set the SpreadsheetGear property for height as Spreadsheet contains only properties for width
                SpreadsheetGear.BorderWeight SpreadsheetGearBW;
                if (this._TemplateStyle.LevelFormat[AreaLevel].FontSetting.BorderSize == 1)
                {
                    SpreadsheetGearBW = SpreadsheetGear.BorderWeight.Hairline;
                }
                else if (this._TemplateStyle.LevelFormat[AreaLevel].FontSetting.BorderSize == 2)
                {
                    SpreadsheetGearBW = SpreadsheetGear.BorderWeight.Thin;
                }
                else if (this._TemplateStyle.LevelFormat[AreaLevel].FontSetting.BorderSize == 3)
                {
                    SpreadsheetGearBW = SpreadsheetGear.BorderWeight.Medium;
                }
                else if (this._TemplateStyle.LevelFormat[AreaLevel].FontSetting.BorderSize == 4)
                {
                    SpreadsheetGearBW = SpreadsheetGear.BorderWeight.Thick;
                }
                else
                {
                    SpreadsheetGearBW = SpreadsheetGear.BorderWeight.Thin;
                }

                //Set Boder settings
                switch (this._TemplateStyle.LevelFormat[AreaLevel].FontSetting.BorderStyle)
                {
                    case FontSetting.CellBorderStyle.None:
                        break;
                    case FontSetting.CellBorderStyle.Bottom:
                        ExcelFile.SetRangeBorder(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns + StartRowCount, 0, TitleSubTitleMargin + FinalRowCount, totalColumns - 1, SpreadsheetGear.LineStyle.Continous, SpreadsheetGearBW, ColorTranslator.FromHtml(this._TemplateStyle.LevelFormat[AreaLevel].FontSetting.BorderColor), SpreadsheetGear.BordersIndex.EdgeBottom);
                        break;
                    case FontSetting.CellBorderStyle.Top:
                        ExcelFile.SetRangeBorder(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns + StartRowCount, 0, TitleSubTitleMargin + FinalRowCount, totalColumns - 1, SpreadsheetGear.LineStyle.Continous, SpreadsheetGearBW, ColorTranslator.FromHtml(this._TemplateStyle.LevelFormat[AreaLevel].FontSetting.BorderColor), SpreadsheetGear.BordersIndex.EdgeTop);
                        break;
                    case FontSetting.CellBorderStyle.Fill:
                        for (int Column = 0; Column < totalColumns; Column++)
                        {
                            ExcelFile.SetRangeBorder(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns + StartRowCount, Column, TitleSubTitleMargin + FinalRowCount, Column, SpreadsheetGear.LineStyle.None, SpreadsheetGear.BorderWeight.Hairline, this.TemplateStyle.LevelFormat[AreaLevel].FontSetting.BackColor, SpreadsheetGear.BordersIndex.EdgeRight);
                            ExcelFile.SetRangeBorder(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns + StartRowCount, Column, TitleSubTitleMargin + FinalRowCount, Column, SpreadsheetGear.LineStyle.None, SpreadsheetGear.BorderWeight.Hairline, this.TemplateStyle.LevelFormat[AreaLevel].FontSetting.BackColor, SpreadsheetGear.BordersIndex.EdgeLeft);
                        }
                        ExcelFile.SetRangeBorder(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns + StartRowCount, 0, TitleSubTitleMargin + FinalRowCount, totalColumns - 1, SpreadsheetGear.LineStyle.Continous, SpreadsheetGearBW, ColorTranslator.FromHtml(this._TemplateStyle.LevelFormat[AreaLevel].FontSetting.BorderColor), SpreadsheetGear.BordersIndex.EdgeBottom);
                        ExcelFile.SetRangeBorder(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns + StartRowCount, 0, TitleSubTitleMargin + FinalRowCount, totalColumns - 1, SpreadsheetGear.LineStyle.Continous, SpreadsheetGearBW, ColorTranslator.FromHtml(this._TemplateStyle.LevelFormat[AreaLevel].FontSetting.BorderColor), SpreadsheetGear.BordersIndex.EdgeTop);
                        ExcelFile.SetRangeBorder(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns + StartRowCount, 0, TitleSubTitleMargin + FinalRowCount, totalColumns - 1, SpreadsheetGear.LineStyle.Continous, SpreadsheetGearBW, ColorTranslator.FromHtml(this._TemplateStyle.LevelFormat[AreaLevel].FontSetting.BorderColor), SpreadsheetGear.BordersIndex.EdgeLeft);
                        ExcelFile.SetRangeBorder(sheetIndex, TitleSubTitleMargin + ColumnArrangementColumns + StartRowCount, 0, TitleSubTitleMargin + FinalRowCount, totalColumns - 1, SpreadsheetGear.LineStyle.Continous, SpreadsheetGearBW, ColorTranslator.FromHtml(this._TemplateStyle.LevelFormat[AreaLevel].FontSetting.BorderColor), SpreadsheetGear.BordersIndex.EdgeRight);
                        break;
                    default:
                        break;
                }
                ExcelFile.SetRowHeight(sheetIndex, Convert.ToDouble(this._TemplateStyle.LevelFormat[AreaLevel].FontSetting.RowHeight), TitleSubTitleMargin + ColumnArrangementColumns + StartRowCount, 0, TitleSubTitleMargin + FinalRowCount, totalColumns - 1);
            }
        }

        /// <summary>
        /// Make the color pallate for spreadsheet
        /// </summary>
        /// <returns></returns>
        private Color[] GetColorPelette()
        {
            int TotalColors = 25;
            if (this._AddColor)
            {
                TotalColors += this._Themes[0].BreakCount;
            }
            Color[] ColorPelatte = new Color[TotalColors];
            ColorPelatte[0] = this._TemplateStyle.TitleSetting.FontTemplate.ForeColor;
            ColorPelatte[1] = this._TemplateStyle.TitleSetting.FontTemplate.BackColor;
            ColorPelatte[2] = this._TemplateStyle.SubTitleSetting.FontTemplate.ForeColor;
            ColorPelatte[3] = this._TemplateStyle.SubTitleSetting.FontTemplate.BackColor;
            ColorPelatte[4] = this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor;
            ColorPelatte[5] = this._TemplateStyle.ColumnSetting.FontTemplate.BackColor;
            ColorPelatte[6] = this._TemplateStyle.Comments.FontTemplate.ForeColor;
            ColorPelatte[7] = this._TemplateStyle.Comments.FontTemplate.BackColor;
            ColorPelatte[8] = this._TemplateStyle.ContentSetting.FontTemplate.ForeColor;
            ColorPelatte[9] = this._TemplateStyle.ContentSetting.FontTemplate.BackColor;
            ColorPelatte[10] = this._TemplateStyle.Footnotes.FontTemplate.ForeColor;
            ColorPelatte[11] = this._TemplateStyle.Footnotes.FontTemplate.BackColor;
            ColorPelatte[12] = this._TemplateStyle.GroupAggregateSetting.FontTemplate.ForeColor;
            ColorPelatte[13] = this._TemplateStyle.GroupAggregateSetting.FontTemplate.BackColor;
            ColorPelatte[14] = this._TemplateStyle.GroupHeaderSetting.FontTemplate.ForeColor;
            ColorPelatte[15] = this._TemplateStyle.GroupHeaderSetting.FontTemplate.BackColor;
            ColorPelatte[16] = this._TemplateStyle.RowSetting.FontTemplate.ForeColor;
            ColorPelatte[17] = this._TemplateStyle.RowSetting.FontTemplate.BackColor;
            ColorPelatte[18] = this._TemplateStyle.SubAggregateSetting.FontTemplate.ForeColor;
            ColorPelatte[19] = this._TemplateStyle.SubAggregateSetting.FontTemplate.BackColor;
            ColorPelatte[20] = this._TemplateStyle.Legends.FontTemplate.ForeColor;
            ColorPelatte[21] = this._TemplateStyle.Legends.FontTemplate.BackColor;
            ColorPelatte[21] = this._TemplateStyle.Denominator.FontTemplate.ForeColor;
            ColorPelatte[22] = this._TemplateStyle.Denominator.FontTemplate.BackColor;
            ColorPelatte[23] = ColorTranslator.FromHtml(this._TemplateStyle.RowSetting.FontTemplate.AlternateBackColor1);
            ColorPelatte[24] = ColorTranslator.FromHtml(this._TemplateStyle.RowSetting.FontTemplate.AlternateBackColor2);

            if (this._AddColor)
            {
                for (int Count = 0; Count < this._Themes[0].BreakCount; Count++)
                {
                    ColorPelatte[25 + Count] = this._Themes[0].Legends[Count].Color;
                }

            }
            return ColorPelatte;
        }

        /// <summary>
        /// Set style for footnote and comments.
        /// </summary>
        /// <param name="TableXLS">DataTable that holds the actual XLS output</param>
        /// <param name="TableFootNoteArray">Two dimentional array containing Footnote Data.</param>
        /// <param name="TableCommentArray">Two dimentional array containing comments Data.</param>
        /// <param name="ExcelFile">DIExcel object</param>
        /// <param name="TableFootNote">DataTable containing FootNote valus</param>
        /// <param name="sheetIndex">WorkSheet index of the selected worksheet</param>		
        private void SetFootNoteCommentsStyle(DataTable TableXLS, string[,] TableFootNoteArray, string[,] TableCommentArray, DIExcel ExcelFile, DataTable TableFootNote, int sheetIndex)
        {
            string NoteSymbol = string.Empty;		//Used when comment is true and is Inline
            int Rows;
            int Columns;
            int ColumnPosition;		//DataColumn Position in TableXLS. Set when itrerating in foreach loop for each column in TableXLS.Columns.
            //And column position (i.e. x) is set by DataColumn.Ordinal property.
            int FootNoteCounter = 1;
            int SuperScriptSize = 7;
            int BlankRows = TitleSubTitleMargin;
            DataRow[] FootnoteInlineText;
            DataRow[] CommentInlineText;
            int XLSStartRow = TitleSubTitleMargin;
            int HeaderRows = (this._ColumnArrangementTable.Columns.Count - 2) / 2;	//Header of TableXLS
            if (HeaderRows == 0)
            {
                HeaderRows = 1;
            }
            int TotalTableXLSRows = TableXLS.Rows.Count;
            int FootNoteStartRow = TableXLS.Rows.Count;
            int FootNoteEndRow = TableXLS.Rows.Count + TableFootNoteArray.GetLength(0);
            if (this.FootnoteInLine == FootNoteDisplayStyle.Separate && this._TemplateStyle.Footnotes.Show)
            {
                //Skip if no footnote is available
                //Used to set the style ( coloring ) for comments .
                if (TableFootNoteArray.GetLength(0) > 0)
                {
                    BlankRows += TitleSubTitleMargin;
                }
                FootNoteStartRow += BlankRows;
                FootNoteEndRow += BlankRows;
                BlankRows += TableFootNoteArray.GetLength(0) + TitleSubTitleMargin;
            }
            int CommentStartRow = 0;
            int CommentEndRow = 0;
            if (this.FootnoteInLine == FootNoteDisplayStyle.Inline && TableFootNoteArray.GetLength(0) > 0)
            {
                CommentStartRow = TableXLS.Rows.Count + TitleSubTitleMargin;
                CommentEndRow = TableXLS.Rows.Count + TableCommentArray.GetLength(0) + TitleSubTitleMargin;
            }
            else
            {
                CommentStartRow = TableXLS.Rows.Count;
                CommentEndRow = TableXLS.Rows.Count + TableCommentArray.GetLength(0);
            }

            if (!this._TemplateStyle.Comments.FontTemplate.Inline && this._TemplateStyle.Comments.Show)
            {

                CommentStartRow += BlankRows;
                CommentEndRow += BlankRows;
            }
            int ContentStartRow = TitleSubTitleMargin + HeaderRows;

            if (this.FootnoteCommentType == FootnoteCommentType.Footnote && this._TemplateStyle.Footnotes.Show == true)
            {
                if (this.FootnoteInLine == FootNoteDisplayStyle.Inline || this.FootnoteInLine == FootNoteDisplayStyle.InlineWithData)   //Show Footnotes as XLS comments.
                {
                    if (this._TemplateStyle.Denominator.Show)   //If denominator check is on
                    {
                        //Set the column width to zero
                        this.SetColumnWidth(ExcelFile, sheetIndex, -1, TableXLS, HeaderRows, 3);

                        Rows = TitleSubTitleMargin;		//Leave the Top rows containing Title and Subtitle of XLS file.	

                        //Fill footnotes.
                        if (this.FootnoteInLine == FootNoteDisplayStyle.InlineWithData)
                        {
                            //Handle the case when denominator is to be handeled in Graph Wizard.
                        }
                        else
                        {
                            foreach (DataRow DRowTableXLS in TableXLS.Rows)
                            {
                                Rows++;
                                FootNoteCounter = -1;
                                if (Rows > HeaderRows + TitleSubTitleMargin)	  //Leave TableXLS Header
                                {
                                    //start "columns = this._Fields.Rows.Count + 1" so that only content portion have style 
                                    for (Columns = this._Fields.Rows.Count; Columns < TableXLS.Columns.Count - (4 + this.MaxMinAreaLevelDiff); Columns++)
                                    {
                                        //Apply inline footnotes for Columns containing footnotes.
                                        if (FootNoteCounter % 3 == 0)    //Leave one column for DataValues
                                        {
                                            FootnoteInlineText = TableFootNote.Select("FootNoteIndex ='" + DRowTableXLS[Columns] + "'");
                                            if (FootnoteInlineText.Length > 0)
                                            {
                                                ExcelFile.AddComment(sheetIndex, Rows - 1, Columns, FootnoteInlineText[0][FootNotes.FootNote].ToString(), true);
                                                ExcelFile.AutoFitColumns(sheetIndex, Rows - 1, Columns, Rows - 1, Columns);
                                                ExcelFile.SetCellValue(sheetIndex, Rows - 1, Columns, "");
                                            }
                                        }
                                        FootNoteCounter++;
                                    }
                                }
                            }
                        }
                    }
                    else	//If denominator check is off
                    {
                        Rows = TitleSubTitleMargin;		//Leave the Top rows containing Title and Subtitle of XLS file.	

                        //Set the column width of footnote to 0	
                        FootNoteCounter = 1;

                        //Fill footnotes.
                        if (this.FootnoteInLine == FootNoteDisplayStyle.InlineWithData)
                        {
                            int TempColumnIndex;
                            int RowIndex = 0;
                            foreach (DataRow DRowTableXLS in TableXLS.Rows)
                            {
                                if (RowIndex >= HeaderRows)	  //Leave TableXLS Header
                                {
                                    TempColumnIndex = this._Fields.Rows.Count;
                                    //start "columns = this._Fields.Rows.Count + 1" so that only content portion have style 
                                    for (Columns = this._Fields.Rows.Count; Columns < TableXLS.Columns.Count - (4 + this.MaxMinAreaLevelDiff); Columns++)
                                    {
                                        //Apply inline footnotes for Columns containing footnotes.
                                        FootnoteInlineText = TableFootNote.Select("FootNoteIndex ='" + this.TempTableXLS.Rows[RowIndex][TempColumnIndex + 1].ToString() + "'");
                                        if (FootnoteInlineText.Length > 0)
                                        {
                                            ExcelFile.AddComment(sheetIndex, RowIndex + TitleSubTitleMargin, Columns, FootnoteInlineText[0][FootNotes.FootNote].ToString(), true);
                                        }
                                        TempColumnIndex += 2;
                                    }
                                }
                                RowIndex++;
                            }
                        }
                        else
                        {
                            //Set Column width zero
                            this.SetColumnWidth(ExcelFile, sheetIndex, 1, TableXLS, HeaderRows, 2);

                            foreach (DataRow DRowTableXLS in TableXLS.Rows)
                            {
                                Rows++;
                                FootNoteCounter = 1;
                                if (Rows > HeaderRows + TitleSubTitleMargin)	  //Leave TableXLS Header
                                {
                                    //start "columns = this._Fields.Rows.Count + 1" so that only content portion have style 
                                    for (Columns = this._Fields.Rows.Count; Columns < TableXLS.Columns.Count - (4 + this.MaxMinAreaLevelDiff); Columns++)
                                    {
                                        //Apply inline footnotes for Columns containing footnotes.
                                        if (FootNoteCounter % 2 == 0)    //Leave one column for DataValues
                                        {
                                            FootnoteInlineText = TableFootNote.Select("FootNoteIndex ='" + DRowTableXLS[Columns] + "'");
                                            if (FootnoteInlineText.Length > 0)
                                            {
                                                ExcelFile.AddComment(sheetIndex, Rows - 1, Columns, FootnoteInlineText[0][FootNotes.FootNote].ToString(), true);
                                                ExcelFile.AutoFitColumns(sheetIndex, Rows - 1, Columns, Rows - 1, Columns);
                                                ExcelFile.SetCellValue(sheetIndex, Rows - 1, Columns, "");
                                            }
                                        }
                                        FootNoteCounter++;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    //Apply template settings for footnotes under the table.
                    ExcelFile.GetRangeFont(sheetIndex, FootNoteStartRow, 0, FootNoteEndRow, TableFootNoteArray.GetLength(1) - 1).Name = this._TemplateStyle.Footnotes.FontTemplate.FontName;
                    ExcelFile.GetRangeFont(sheetIndex, FootNoteStartRow, 0, FootNoteEndRow, TableFootNoteArray.GetLength(1) - 1).Color = this._TemplateStyle.Footnotes.FontTemplate.ForeColor;
                    ExcelFile.GetRangeFont(sheetIndex, FootNoteStartRow, 0, FootNoteEndRow, TableFootNoteArray.GetLength(1) - 1).Size = this._TemplateStyle.Footnotes.FontTemplate.FontSize;
                    ExcelFile.SetRangeColor(sheetIndex, FootNoteStartRow, 0, FootNoteEndRow, TableFootNoteArray.GetLength(1) - 1, this._TemplateStyle.Footnotes.FontTemplate.ForeColor, Color.Transparent);

                    if (this._TemplateStyle.Denominator.Show)	//If showDenominator check is on
                    {
                        this.SetColumnWidth(ExcelFile, sheetIndex, -1, TableXLS, HeaderRows, 3);
                        FootNoteCounter = -1;
                        foreach (DataColumn DColTableXls in TableXLS.Columns)
                        {
                            ColumnPosition = DColTableXls.Ordinal;
                            if (ColumnPosition >= this._Fields.Rows.Count && ColumnPosition <= TableXLS.Columns.Count - (5 + this.MaxMinAreaLevelDiff))	 //skip the row columns (Area , Source)
                            {
                                if (FootNoteCounter % 3 == 0)		//Apply settings for Columns having Footnotes.
                                {
                                    ExcelFile.AutoFitColumns(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin - 1, ColumnPosition);
                                    ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin - 1, ColumnPosition).Superscript = true;
                                    this.SetExcelRangeFont(ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin, ColumnPosition - 1), this._TemplateStyle.Footnotes.FontTemplate.FontStyle);
                                    ExcelFile.SetHorizontalAlignment(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin - 1, ColumnPosition, SpreadsheetGear.HAlign.Left);
                                    if (this._UserPreference.General.ShowExcel == false)
                                    {
                                        ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin - 1, ColumnPosition).Size = SuperScriptSize;
                                        ExcelFile.SetVerticalAlignment(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin - 1, ColumnPosition, SpreadsheetGear.VAlign.Top);
                                    }
                                    else
                                    {
                                        ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin - 1, ColumnPosition).Size = SuperScriptSize + 2;
                                    }
                                    for (Rows = ContentStartRow; Rows <= TotalTableXLSRows + TitleSubTitleMargin; Rows++)
                                    {
                                        if (ExcelFile.GetCellValue(sheetIndex, Rows, ColumnPosition, Rows, ColumnPosition) != string.Empty)
                                        {
                                            ExcelFile.SetRangeColor(sheetIndex, Rows, ColumnPosition, Rows, ColumnPosition, this._TemplateStyle.Footnotes.FontTemplate.ForeColor, this._TemplateStyle.Footnotes.FontTemplate.BackColor);
                                        }
                                    }
                                }
                                FootNoteCounter++;
                            }
                        }
                    }
                    else	  //If showDenominator check is off
                    {
                        //Set Column width zero
                        this.SetColumnWidth(ExcelFile, sheetIndex, 1, TableXLS, HeaderRows, 2);

                        foreach (DataColumn DColTableXls in TableXLS.Columns)
                        {
                            ColumnPosition = DColTableXls.Ordinal;
                            if (ColumnPosition >= this._Fields.Rows.Count && ColumnPosition <= TableXLS.Columns.Count - (4 + this.MaxMinAreaLevelDiff))	 //skip the row columns (Area , Source)
                            {
                                if (FootNoteCounter % 2 == 0)		//Apply settings for Columns having Footnotes.
                                {
                                    ExcelFile.AutoFitColumns(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin - 1, ColumnPosition);
                                    ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin - 1, ColumnPosition).Superscript = true;

                                    if (ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin, ColumnPosition - 1) != null)
                                    {
                                        ExcelFile.SetHorizontalAlignment(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin - 1, ColumnPosition, SpreadsheetGear.HAlign.Left);
                                    }
                                    if (this._UserPreference.General.ShowExcel == false)
                                    {
                                        ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin - 1, ColumnPosition).Size = SuperScriptSize;
                                        ExcelFile.SetVerticalAlignment(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin - 1, ColumnPosition, SpreadsheetGear.VAlign.Top);
                                    }
                                    else
                                    {
                                        ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin - 1, ColumnPosition).Size = SuperScriptSize + 2;
                                    }
                                    for (Rows = ContentStartRow; Rows <= TotalTableXLSRows + TitleSubTitleMargin; Rows++)
                                    {
                                        if (ExcelFile.GetCellValue(sheetIndex, Rows, ColumnPosition, Rows, ColumnPosition) != string.Empty)
                                        {
                                            ExcelFile.SetRangeColor(sheetIndex, Rows, ColumnPosition, Rows, ColumnPosition, this._TemplateStyle.Footnotes.FontTemplate.ForeColor, this._TemplateStyle.Footnotes.FontTemplate.BackColor);
                                        }
                                    }
                                }
                                FootNoteCounter++;
                            }
                        }
                    }
                }
            }
            if (this.FootnoteCommentType == FootnoteCommentType.Both)
            {
                if (this._TemplateStyle.Footnotes.Show)
                {
                    FootNoteCounter = -2;
                    if (this.FootnoteInLine == FootNoteDisplayStyle.Inline)
                    {
                        Rows = TitleSubTitleMargin;	//Leave the Top rows containing Title and Subtitle of XLS file.	
                        if (this._TemplateStyle.Denominator.Show)
                        {
                            //Set Column width zero
                            this.SetColumnWidth(ExcelFile, sheetIndex, -1, TableXLS, HeaderRows, 4);

                            foreach (DataRow DRowTableXLS in TableXLS.Rows)
                            {
                                Rows++;
                                FootNoteCounter = -1;
                                if (Rows > HeaderRows + TitleSubTitleMargin)  //Leave TableXLS Header
                                {
                                    for (Columns = this._Fields.Rows.Count; Columns < TableXLS.Columns.Count - (4 + this.MaxMinAreaLevelDiff); Columns++)
                                    {

                                        //Apply inline footnotes for Columns containing footnotes.
                                        if (FootNoteCounter % 4 == 0)
                                        {
                                            FootnoteInlineText = TableFootNote.Select("FootNoteIndex ='" + DRowTableXLS[Columns] + "'");
                                            if (FootnoteInlineText.Length > 0)
                                            {
                                                ExcelFile.AddComment(sheetIndex, Rows - 1, Columns, FootnoteInlineText[0][FootNotes.FootNote].ToString(), true);
                                                ExcelFile.AutoFitColumns(sheetIndex, Rows - 1, Columns, Rows - 1, Columns);
                                                ExcelFile.SetCellValue(sheetIndex, Rows - 1, Columns, "");
                                            }
                                        }
                                        FootNoteCounter++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Set Column width zero
                            this.SetColumnWidth(ExcelFile, sheetIndex, -1, TableXLS, HeaderRows, 3);

                            foreach (DataRow DRowTableXLS in TableXLS.Rows)
                            {
                                Rows++;
                                FootNoteCounter = -1;
                                if (Rows > HeaderRows + TitleSubTitleMargin)  //Leave TableXLS Header
                                {
                                    for (Columns = this._Fields.Rows.Count; Columns < TableXLS.Columns.Count - (4 + this.MaxMinAreaLevelDiff); Columns++)
                                    {
                                        //Apply inline footnotes for Columns containing footnotes.
                                        if (FootNoteCounter % 3 == 0)
                                        {
                                            FootnoteInlineText = TableFootNote.Select("FootNoteIndex ='" + DRowTableXLS[Columns] + "'");
                                            if (FootnoteInlineText.Length > 0)
                                            {
                                                ExcelFile.AddComment(sheetIndex, Rows - 1, Columns, FootnoteInlineText[0][FootNotes.FootNote].ToString(), true);
                                                ExcelFile.AutoFitColumns(sheetIndex, Rows - 1, Columns, Rows - 1, Columns);
                                                ExcelFile.SetCellValue(sheetIndex, Rows - 1, Columns, "");
                                            }
                                        }
                                        FootNoteCounter++;
                                    }
                                }
                            }
                        }

                    }
                    if (this.FootnoteInLine == FootNoteDisplayStyle.Separate)
                    {
                        //Apply template settings for footnotes under the table.
                        ExcelFile.GetRangeFont(sheetIndex, FootNoteStartRow, 0, FootNoteEndRow, TableFootNoteArray.GetLength(1) - 1).Name = this._TemplateStyle.Footnotes.FontTemplate.FontName;
                        ExcelFile.GetRangeFont(sheetIndex, FootNoteStartRow, 0, FootNoteEndRow, TableFootNoteArray.GetLength(1) - 1).Color = this._TemplateStyle.Footnotes.FontTemplate.ForeColor;
                        ExcelFile.GetRangeFont(sheetIndex, FootNoteStartRow, 0, FootNoteEndRow, TableFootNoteArray.GetLength(1) - 1).Size = this._TemplateStyle.Footnotes.FontTemplate.FontSize;
                        ExcelFile.SetRangeColor(sheetIndex, FootNoteStartRow, 0, FootNoteEndRow, TableFootNoteArray.GetLength(1) - 1, this._TemplateStyle.Footnotes.FontTemplate.ForeColor, Color.Transparent);

                        if (this._TemplateStyle.Denominator.Show)
                        {
                            //Set column width to Zero.
                            this.SetColumnWidth(ExcelFile, sheetIndex, -1, TableXLS, HeaderRows, 4);

                            FootNoteCounter = -1;
                            foreach (DataColumn DColTableXls in TableXLS.Columns)
                            {
                                ColumnPosition = DColTableXls.Ordinal;
                                if (ColumnPosition >= this._Fields.Rows.Count && ColumnPosition <= TableXLS.Columns.Count - (4 + this.MaxMinAreaLevelDiff))	 //skip the row columns (Area , Source)
                                {
                                    if (FootNoteCounter % 4 == 0)
                                    {
                                        //Style for Footnote
                                        ExcelFile.AutoFitColumns(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin, ColumnPosition);
                                        ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin, ColumnPosition).Superscript = true;
                                        this.SetExcelRangeFont(ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin, ColumnPosition), this._TemplateStyle.Footnotes.FontTemplate.FontStyle);
                                        ExcelFile.SetHorizontalAlignment(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin, ColumnPosition, SpreadsheetGear.HAlign.Left);

                                        if (this._UserPreference.General.ShowExcel == false)
                                        {
                                            ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin, ColumnPosition).Size = SuperScriptSize;
                                            ExcelFile.SetVerticalAlignment(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin, ColumnPosition, SpreadsheetGear.VAlign.Top);
                                        }
                                        else
                                        {
                                            ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin, ColumnPosition).Size = SuperScriptSize + 2;
                                        }

                                        for (Rows = ContentStartRow; Rows <= TotalTableXLSRows + TitleSubTitleMargin; Rows++)
                                        {
                                            if (ExcelFile.GetCellValue(sheetIndex, Rows, ColumnPosition, Rows, ColumnPosition) != string.Empty)
                                            {
                                                ExcelFile.SetRangeColor(sheetIndex, Rows, ColumnPosition, Rows, ColumnPosition, this._TemplateStyle.Footnotes.FontTemplate.ForeColor, this._TemplateStyle.Footnotes.FontTemplate.BackColor);
                                            }
                                        }
                                    }
                                    FootNoteCounter++;
                                }
                            }
                        }
                        else
                        {
                            //Set Column width zero
                            this.SetColumnWidth(ExcelFile, sheetIndex, -1, TableXLS, HeaderRows, 3);

                            FootNoteCounter = -1;
                            foreach (DataColumn DColTableXls in TableXLS.Columns)
                            {
                                ColumnPosition = DColTableXls.Ordinal;
                                if (ColumnPosition >= this._Fields.Rows.Count && ColumnPosition <= TableXLS.Columns.Count - (4 + this.MaxMinAreaLevelDiff))	 //skip the row columns (Area , Source)
                                {
                                    if (FootNoteCounter % 3 == 0)
                                    {
                                        //Style for Footnote  .
                                        ExcelFile.AutoFitColumns(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin, ColumnPosition);
                                        ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin, ColumnPosition).Superscript = true;
                                        if (this._UserPreference.General.ShowExcel == false)
                                        {
                                            ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin, ColumnPosition).Size = SuperScriptSize;
                                            ExcelFile.SetVerticalAlignment(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin, ColumnPosition, SpreadsheetGear.VAlign.Top);
                                        }
                                        else
                                        {
                                            ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin, ColumnPosition).Size = SuperScriptSize + 2;
                                        }

                                        for (Rows = ContentStartRow; Rows <= TotalTableXLSRows + TitleSubTitleMargin; Rows++)
                                        {
                                            if (ExcelFile.GetCellValue(sheetIndex, Rows, ColumnPosition, Rows, ColumnPosition) != string.Empty)
                                            {
                                                ExcelFile.SetRangeColor(sheetIndex, Rows, ColumnPosition, Rows, ColumnPosition, this._TemplateStyle.Footnotes.FontTemplate.ForeColor, this._TemplateStyle.Footnotes.FontTemplate.BackColor);
                                                this.SetExcelRangeFont(ExcelFile.GetRangeFont(sheetIndex, Rows, ColumnPosition, Rows, ColumnPosition), this._TemplateStyle.Footnotes.FontTemplate.FontStyle);
                                                ExcelFile.SetHorizontalAlignment(sheetIndex, Rows, ColumnPosition, Rows, ColumnPosition, SpreadsheetGear.HAlign.Left);
                                            }
                                        }
                                    }
                                    FootNoteCounter++;
                                }
                            }
                        }
                    }
                }
                if (this._TemplateStyle.Comments.Show)
                {
                    FootNoteCounter = -2;
                    if (this._TemplateStyle.Comments.FontTemplate.Inline == true)
                    {
                        Rows = TitleSubTitleMargin;	//Leave the Top rows containing Title and Subtitle of XLS file.
                        if (this._TemplateStyle.Denominator.Show)
                        {
                            //Set Column width zero
                            this.SetColumnWidth(ExcelFile, sheetIndex, -2, TableXLS, HeaderRows, 4);

                            foreach (DataRow DRowTableXLS in TableXLS.Rows)
                            {
                                Rows++;
                                FootNoteCounter = -2;
                                if (Rows > HeaderRows + TitleSubTitleMargin)  //Leave TableXLS Header
                                {
                                    for (Columns = this._Fields.Rows.Count; Columns < TableXLS.Columns.Count; Columns++)
                                    {
                                        //Apply inline footnotes for Columns containing footnotes.
                                        if (FootNoteCounter % 4 == 0)
                                        {
                                            CommentInlineText = this.DTComments.Select("NoteSymbol = '" + DRowTableXLS[Columns] + "'");
                                            if (CommentInlineText.Length > 0)
                                            {
                                                ExcelFile.AutoFitColumns(sheetIndex, Rows - 1, Columns, Rows - 1, Columns);
                                                ExcelFile.SetCellValue(sheetIndex, Rows - 1, Columns, "");
                                                ExcelFile.SetCellValue(sheetIndex, Rows - 1, Columns, CommentInlineText[0][Notes.Note].ToString());
                                                ExcelFile.SetCellColor(sheetIndex, Rows - 1, Columns, this._TemplateStyle.Comments.FontTemplate.ForeColor, this._TemplateStyle.Comments.FontTemplate.BackColor);
                                            }
                                        }
                                        FootNoteCounter++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Set Column width zero
                            this.SetColumnWidth(ExcelFile, sheetIndex, -2, TableXLS, HeaderRows, 3);
                            string CommentString = string.Empty;
                            foreach (DataRow DRowTableXLS in TableXLS.Rows)
                            {
                                Rows++;
                                FootNoteCounter = -1;
                                if (Rows > HeaderRows + TitleSubTitleMargin)  //Leave TableXLS Header
                                {
                                    for (Columns = this._Fields.Rows.Count + 1; Columns < TableXLS.Columns.Count - (4 + this.MaxMinAreaLevelDiff); Columns++)
                                    {
                                        NoteSymbol = string.Empty;

                                        //Apply inline footnotes for Columns containing footnotes.
                                        if (FootNoteCounter % 3 == 0)
                                        {
                                            NoteSymbol = DRowTableXLS[Columns].ToString();
                                            if (NoteSymbol != string.Empty)
                                            {
                                                CommentString = string.Empty;
                                                string[] ArrayNoteSymbol = DICommon.SplitString(NoteSymbol, ",");
                                                for (int i = 0; i < ArrayNoteSymbol.Length; i++)
                                                {
                                                    CommentInlineText = this.DTComments.Select("NoteSymbol = '" + ArrayNoteSymbol[i].ToString() + "'");
                                                    if (CommentInlineText.Length > 0)
                                                    {
                                                        CommentString += "," + CommentInlineText[0][Notes.Note].ToString();
                                                    }
                                                }
                                                //CommentInlineText = this.DTComments.Select("NoteSymbol = '" + DRowTableXLS[Columns] + "'");
                                                if (CommentString != string.Empty)
                                                {
                                                    CommentString = CommentString.Substring(1);
                                                    ExcelFile.AutoFitColumns(sheetIndex, Rows - 1, Columns, Rows - 1, Columns);
                                                    ExcelFile.SetCellValue(sheetIndex, Rows - 1, Columns, "");
                                                    ExcelFile.SetCellValue(sheetIndex, Rows - 1, Columns, CommentString);
                                                    ExcelFile.SetCellColor(sheetIndex, Rows - 1, Columns, this._TemplateStyle.Comments.FontTemplate.ForeColor, this._TemplateStyle.Comments.FontTemplate.BackColor);
                                                }
                                            }
                                        }
                                        FootNoteCounter++;
                                    }
                                }
                            }
                        }
                    }

                    if (this._TemplateStyle.Comments.FontTemplate.Inline == false)
                    {
                        //Apply template settings for notes under the table.
                        ExcelFile.GetRangeFont(sheetIndex, CommentStartRow, 0, CommentEndRow, TableCommentArray.GetLength(1) - 1).Name = this._TemplateStyle.Comments.FontTemplate.FontName;
                        ExcelFile.GetRangeFont(sheetIndex, CommentStartRow, 0, CommentEndRow, TableCommentArray.GetLength(1) - 1).Color = this._TemplateStyle.Comments.FontTemplate.ForeColor;
                        ExcelFile.GetRangeFont(sheetIndex, CommentStartRow, 0, CommentEndRow, TableCommentArray.GetLength(1) - 1).Size = this._TemplateStyle.Comments.FontTemplate.FontSize;
                        SetExcelRangeFont(ExcelFile.GetRangeFont(sheetIndex, CommentStartRow, 0, CommentEndRow, TableCommentArray.GetLength(1) - 1), this._TemplateStyle.Comments.FontTemplate.FontStyle);
                        ExcelFile.SetRangeColor(sheetIndex, CommentStartRow, 0, CommentEndRow, TableCommentArray.GetLength(1) - 1, this._TemplateStyle.Comments.FontTemplate.ForeColor, Color.Transparent);
                        if (this._TemplateStyle.Denominator.Show)
                        {
                            this.SetColumnWidth(ExcelFile, sheetIndex, -2, TableXLS, HeaderRows, 4);
                            FootNoteCounter = -2;
                            foreach (DataColumn DColTableXls in TableXLS.Columns)
                            {
                                ColumnPosition = DColTableXls.Ordinal;
                                if (ColumnPosition >= this._Fields.Rows.Count && ColumnPosition <= TableXLS.Columns.Count - (4 + this.MaxMinAreaLevelDiff))	 //skip the row columns (Area , Source)
                                {
                                    if (FootNoteCounter % 4 == 0)
                                    {
                                        //Style For Comments
                                        ExcelFile.AutoFitColumns(sheetIndex, HeaderRows + TitleSubTitleMargin, ColumnPosition, TableXLS.Rows.Count, ColumnPosition);
                                        ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin, ColumnPosition).Superscript = true;
                                        this.SetExcelRangeFont(ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin, ColumnPosition), this._TemplateStyle.Comments.FontTemplate.FontStyle);
                                        ExcelFile.SetHorizontalAlignment(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin, ColumnPosition, SpreadsheetGear.HAlign.Left);

                                        if (this._UserPreference.General.ShowExcel == false)
                                        {
                                            ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin, ColumnPosition).Size = SuperScriptSize;
                                            ExcelFile.SetVerticalAlignment(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin, ColumnPosition, SpreadsheetGear.VAlign.Top);
                                        }
                                        else
                                        {
                                            ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin, ColumnPosition).Size = SuperScriptSize + 2;
                                        }

                                        for (Rows = ContentStartRow; Rows <= TotalTableXLSRows + TitleSubTitleMargin; Rows++)
                                        {
                                            if (ExcelFile.GetCellValue(sheetIndex, Rows, ColumnPosition, Rows, ColumnPosition) != string.Empty)
                                            {
                                                ExcelFile.SetRangeColor(sheetIndex, Rows, ColumnPosition, Rows, ColumnPosition, this._TemplateStyle.Comments.FontTemplate.ForeColor, this._TemplateStyle.Comments.FontTemplate.BackColor);
                                            }
                                        }

                                    }
                                    FootNoteCounter++;
                                }
                            }
                        }
                        else
                        {
                            //Set Column width zero
                            this.SetColumnWidth(ExcelFile, sheetIndex, -2, TableXLS, HeaderRows, 3);

                            FootNoteCounter = -2;
                            foreach (DataColumn DColTableXls in TableXLS.Columns)
                            {
                                ColumnPosition = DColTableXls.Ordinal;
                                if (ColumnPosition >= this._Fields.Rows.Count && ColumnPosition <= TableXLS.Columns.Count - (4 + this.MaxMinAreaLevelDiff))	 //skip the row columns (Area , Source)
                                {
                                    if (FootNoteCounter % 3 == 0)
                                    {
                                        //Style For Comments
                                        //TODO :
                                        ExcelFile.AutoFitColumns(sheetIndex, HeaderRows + TitleSubTitleMargin, ColumnPosition, TableXLS.Rows.Count, ColumnPosition);
                                        ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin, ColumnPosition).Superscript = true;
                                        this.SetExcelRangeFont(ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin, ColumnPosition), this._TemplateStyle.Comments.FontTemplate.FontStyle);
                                        ExcelFile.SetHorizontalAlignment(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin, ColumnPosition, SpreadsheetGear.HAlign.Left);

                                        if (this._UserPreference.General.ShowExcel == false)
                                        {
                                            ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin, ColumnPosition).Size = SuperScriptSize;
                                            ExcelFile.SetVerticalAlignment(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin, ColumnPosition, SpreadsheetGear.VAlign.Top);
                                        }
                                        else
                                        {
                                            ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin, ColumnPosition).Size = SuperScriptSize + 2;
                                        }
                                        for (Rows = ContentStartRow; Rows <= TotalTableXLSRows + TitleSubTitleMargin; Rows++)
                                        {
                                            if (ExcelFile.GetCellValue(sheetIndex, Rows, ColumnPosition, Rows, ColumnPosition) != string.Empty)
                                            {
                                                ExcelFile.SetRangeColor(sheetIndex, Rows, ColumnPosition, Rows, ColumnPosition, this._TemplateStyle.Comments.FontTemplate.ForeColor, this._TemplateStyle.Comments.FontTemplate.BackColor);
                                            }
                                        }
                                    }
                                    FootNoteCounter++;
                                }
                            }
                        }
                    }
                }
            }
            if (this.FootnoteCommentType == FootnoteCommentType.Comment && this._TemplateStyle.Comments.Show == true)
            {
                if (this._TemplateStyle.Comments.FontTemplate.Inline == true)
                {
                    Rows = TitleSubTitleMargin;		//Leave the Top rows containing Title and Subtitle of XLS file.		
                    if (this._TemplateStyle.Denominator.Show)
                    {
                        //Set Column width zero
                        this.SetColumnWidth(ExcelFile, sheetIndex, -1, TableXLS, HeaderRows, 3);

                        foreach (DataRow DRowTableXLS in TableXLS.Rows)
                        {
                            Rows++;
                            FootNoteCounter = -1;
                            if (Rows > HeaderRows + TitleSubTitleMargin)	  //Leave TableXLS Header
                            {
                                //start "columns = this._Fields.Rows.Count + 1" so that only content portion have style 
                                for (Columns = this._Fields.Rows.Count; Columns < TableXLS.Columns.Count; Columns++)
                                {
                                    //Apply inline footnotes for Columns containing footnotes.
                                    if (FootNoteCounter % 3 == 0)    //Leave one column for DataValues
                                    {
                                        CommentInlineText = this.DTComments.Select("NoteSymbol = '" + DRowTableXLS[Columns] + "'");
                                        if (CommentInlineText.Length > 0)
                                        {
                                            ExcelFile.AutoFitColumns(sheetIndex, Rows - 1, Columns, Rows - 1, Columns);
                                            ExcelFile.SetCellValue(sheetIndex, Rows - 1, Columns, "");
                                            ExcelFile.SetCellValue(sheetIndex, Rows - 1, Columns, CommentInlineText[0][Notes.Note].ToString());
                                            ExcelFile.SetCellColor(sheetIndex, Rows - 1, Columns, this._TemplateStyle.Comments.FontTemplate.ForeColor, this._TemplateStyle.Comments.FontTemplate.BackColor);
                                        }
                                    }
                                    FootNoteCounter++;
                                }
                            }
                        }
                    }
                    else
                    {
                        //Set Column width zero
                        this.SetColumnWidth(ExcelFile, sheetIndex, 1, TableXLS, HeaderRows, 2);

                        foreach (DataRow DRowTableXLS in TableXLS.Rows)
                        {
                            Rows++;
                            FootNoteCounter = 1;
                            if (Rows > HeaderRows + TitleSubTitleMargin)	  //Leave TableXLS Header
                            {
                                //start "columns = this._Fields.Rows.Count + 1" so that only content portion have style 
                                for (Columns = this._Fields.Rows.Count; Columns < TableXLS.Columns.Count; Columns++)
                                {
                                    //Apply inline footnotes for Columns containing footnotes.
                                    if (FootNoteCounter % 2 == 0)    //Leave one column for DataValues
                                    {
                                        CommentInlineText = this.DTComments.Select("NoteSymbol = '" + DRowTableXLS[Columns] + "'");
                                        if (CommentInlineText.Length > 0)
                                        {
                                            ExcelFile.AutoFitColumns(sheetIndex, Rows - 1, Columns, Rows - 1, Columns);
                                            ExcelFile.SetCellValue(sheetIndex, Rows - 1, Columns, "");
                                            ExcelFile.SetCellValue(sheetIndex, Rows - 1, Columns, CommentInlineText[0][Notes.Note].ToString());
                                            ExcelFile.SetCellColor(sheetIndex, Rows - 1, Columns, this._TemplateStyle.Comments.FontTemplate.ForeColor, this._TemplateStyle.Comments.FontTemplate.BackColor);
                                        }
                                    }
                                    FootNoteCounter++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    //Apply template settings for notes under the table.
                    ExcelFile.GetRangeFont(sheetIndex, CommentStartRow, 0, CommentEndRow, TableCommentArray.GetLength(1) - 1).Name = this._TemplateStyle.Comments.FontTemplate.FontName;
                    ExcelFile.GetRangeFont(sheetIndex, CommentStartRow, 0, CommentEndRow, TableCommentArray.GetLength(1) - 1).Color = this._TemplateStyle.Comments.FontTemplate.ForeColor;
                    ExcelFile.GetRangeFont(sheetIndex, CommentStartRow, 0, CommentEndRow, TableCommentArray.GetLength(1) - 1).Size = this._TemplateStyle.Comments.FontTemplate.FontSize;
                    SetExcelRangeFont(ExcelFile.GetRangeFont(sheetIndex, CommentStartRow, 0, CommentEndRow, TableCommentArray.GetLength(1) - 1), this._TemplateStyle.Comments.FontTemplate.FontStyle);
                    ExcelFile.SetRangeColor(sheetIndex, CommentStartRow, 0, CommentEndRow, TableCommentArray.GetLength(1) - 1, this._TemplateStyle.Comments.FontTemplate.ForeColor, Color.Transparent);

                    if (this._TemplateStyle.Denominator.Show)
                    {
                        //Set Column width zero
                        this.SetColumnWidth(ExcelFile, sheetIndex, -1, TableXLS, HeaderRows, 3);

                        FootNoteCounter = -1;
                        foreach (DataColumn DColTableXls in TableXLS.Columns)
                        {
                            ColumnPosition = DColTableXls.Ordinal;			//Get the column position.
                            if (ColumnPosition >= this._Fields.Rows.Count && ColumnPosition <= TableXLS.Columns.Count - (4 + this.MaxMinAreaLevelDiff))	 //skip the row columns (Area , Source)
                            {
                                if (FootNoteCounter % 3 == 0)		//Apply settings for Columns having Comments.
                                {
                                    ExcelFile.AutoFitColumns(sheetIndex, TitleSubTitleMargin + HeaderRows - 1, ColumnPosition, TableXLS.Rows.Count, ColumnPosition);
                                    ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin - 1, ColumnPosition).Superscript = true;
                                    this.SetExcelRangeFont(ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin - 1, ColumnPosition), this._TemplateStyle.Comments.FontTemplate.FontStyle);
                                    ExcelFile.SetHorizontalAlignment(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin - 1, ColumnPosition, SpreadsheetGear.HAlign.Left);
                                    if (this._UserPreference.General.ShowExcel == false)
                                    {
                                        ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin - 1, ColumnPosition).Size = SuperScriptSize;
                                        ExcelFile.SetVerticalAlignment(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin - 1, ColumnPosition, SpreadsheetGear.VAlign.Top);
                                    }
                                    else
                                    {
                                        ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin - 1, ColumnPosition).Size = SuperScriptSize + 2;
                                    }

                                    for (Rows = ContentStartRow; Rows <= TotalTableXLSRows + TitleSubTitleMargin; Rows++)
                                    {
                                        if (ExcelFile.GetCellValue(sheetIndex, Rows, ColumnPosition, Rows, ColumnPosition) != string.Empty)
                                        {
                                            ExcelFile.SetRangeColor(sheetIndex, Rows, ColumnPosition, Rows, ColumnPosition, this._TemplateStyle.Comments.FontTemplate.ForeColor, this._TemplateStyle.Comments.FontTemplate.BackColor);
                                        }
                                    }
                                }
                                FootNoteCounter++;
                            }
                        }
                    }
                    else
                    {
                        //Set Column width zero
                        this.SetColumnWidth(ExcelFile, sheetIndex, 1, TableXLS, HeaderRows, 2);

                        FootNoteCounter = 1;
                        foreach (DataColumn DColTableXls in TableXLS.Columns)
                        {
                            ColumnPosition = DColTableXls.Ordinal;			//Get the column position.
                            if (ColumnPosition >= this._Fields.Rows.Count && ColumnPosition <= TableXLS.Columns.Count - (4 + this.MaxMinAreaLevelDiff))	 //skip the row columns (Area , Source)
                            {
                                if (FootNoteCounter % 2 == 0)		//Apply settings for Columns having Comments.
                                {
                                    ExcelFile.AutoFitColumns(sheetIndex, TitleSubTitleMargin + HeaderRows, ColumnPosition, TableXLS.Rows.Count, ColumnPosition);
                                    ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin - 1, ColumnPosition).Superscript = true;
                                    this.SetExcelRangeFont(ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin - 1, ColumnPosition), this._TemplateStyle.Comments.FontTemplate.FontStyle);
                                    ExcelFile.SetHorizontalAlignment(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin - 1, ColumnPosition, SpreadsheetGear.HAlign.Left);
                                    if (this._UserPreference.General.ShowExcel == false)
                                    {
                                        ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin - 1, ColumnPosition).Size = SuperScriptSize;
                                        ExcelFile.SetVerticalAlignment(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin - 1, ColumnPosition, SpreadsheetGear.VAlign.Top);
                                    }
                                    else
                                    {
                                        ExcelFile.GetRangeFont(sheetIndex, XLSStartRow + HeaderRows, ColumnPosition, TotalTableXLSRows + TitleSubTitleMargin - 1, ColumnPosition).Size = SuperScriptSize + 2;
                                    }
                                    for (Rows = ContentStartRow; Rows <= TotalTableXLSRows + TitleSubTitleMargin; Rows++)
                                    {
                                        if (ExcelFile.GetCellValue(sheetIndex, Rows, ColumnPosition, Rows, ColumnPosition) != string.Empty)
                                        {
                                            ExcelFile.SetRangeColor(sheetIndex, Rows, ColumnPosition, Rows, ColumnPosition, this._TemplateStyle.Comments.FontTemplate.ForeColor, this._TemplateStyle.Comments.FontTemplate.BackColor);
                                        }
                                    }
                                }
                                FootNoteCounter++;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets the columnwidth
        /// </summary>
        /// <param name="ExcelFile">ExcelFile</param>
        /// <param name="sheetIndex">Index for active sheet of ExcelFile</param>
        /// <param name="FootNoteCounter">Index of footnote/comment column</param>
        /// <param name="TableXLS"></param>
        /// <param name="HeaderRows">RowCount for TableXLS</param>
        private void SetColumnWidth(DIExcel ExcelFile, int sheetIndex, int FootNoteCounter, DataTable TableXLS, int HeaderRows, int divisor)
        {
            int ColumnPosition;
            //Set the column width of comments column to Zero.
            foreach (DataColumn DColTableXls in TableXLS.Columns)
            {
                ColumnPosition = DColTableXls.Ordinal;
                if (ColumnPosition >= this._Fields.Rows.Count)	 //skip the row columns (Area , Source)
                {
                    if (FootNoteCounter % divisor == 0)		//Apply settings for Columns having Footnotes.
                    {
                        if (this._TemplateStyle.Footnotes.FontTemplate.TableFootnoteInLine == FootNoteDisplayStyle.Inline)
                        {
                            ExcelFile.SetColumnWidth(sheetIndex, 0.4, HeaderRows + TitleSubTitleMargin - 1, ColumnPosition, TableXLS.Rows.Count, ColumnPosition);
                        }
                        else
                        {
                            ExcelFile.SetColumnWidth(sheetIndex, 1.7, HeaderRows + TitleSubTitleMargin - 1, ColumnPosition, TableXLS.Rows.Count, ColumnPosition);
                        }

                    }
                    FootNoteCounter++;
                }
            }
        }

        /// <summary>
        /// Sets the fontstyle of IFont
        /// </summary>
        /// <param name="IFont">IFont</param>
        /// <param name="FontStyle">Font Style</param>
        private void SetExcelRangeFont(SpreadsheetGear.IFont IFont, FontStyle FontStyle)
        {
            switch (FontStyle)
            {
                case FontStyle.Bold:
                    IFont.Bold = true;
                    break;
                case FontStyle.Italic:
                    IFont.Italic = true;
                    break;
                case FontStyle.Regular:

                    break;
                case FontStyle.Strikeout:
                    IFont.Strikethrough = true;
                    break;
                case FontStyle.Underline:
                    IFont.Underline = SpreadsheetGear.UnderlineStyle.Single;
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// Set template styl for legends.
        /// </summary>
        /// <param name="ExcelFile">object of DIExcel</param>
        /// <param name="TableXLS"></param>
        /// <param name="sheetIndex"></param> 
        private void SetLegendStyle(DIExcel ExcelFile, DataTable TableXLS, int sheetIndex)
        {
            //-- Get the current culture.
            System.Globalization.CultureInfo OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture;

            try
            {
                int Rows;				//Row position in TableXLS.
                int Columns;			//Column Position
                int FootNoteCounter = 1;
                bool PresentInRange = false;


                //if Add color check box is checked on step 2.
                if (this.AddColor)
                {
                    //-- Get the current culture.
                    //System.Globalization.CultureInfo OldCulture = System.Threading.Thread.CurrentThread.CurrentCulture;

                    //-- Reset the culture to english - US
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

                    Rows = TitleSubTitleMargin;	//Leave the Top rows of XLS file occupied by Title and subtitle.	
                    int LegendIndex = 0;
                    Double DataValue;
                    string IndicatorName = string.Empty;
                    int IndicatorPosition = 0;		//when indicators are present in columns collection only.

                    if (this._Fields.Rows[Indicator.IndicatorName] != null)   //Check if indicators are present in rows.
                    {
                        foreach (DataRow DRowTableXLS in TableXLS.Rows)
                        {
                            Rows++;
                            FootNoteCounter = 0;
                            if (Rows > (this._ColumnArrangementTable.Columns.Count - 2) / 2 + TitleSubTitleMargin)
                            {
                                //Check whether the DRowTableXLS is containing any DataValues.
                                if (DRowTableXLS[TablePresentation.ROWTYPE].ToString() == RowType.DataRow.ToString())
                                {
                                    IndicatorName = DRowTableXLS[Indicator.IndicatorName].ToString();
                                }
                                else   //Move to next DRowTableXLS in TableXLS.
                                {
                                    continue;
                                }

                                for (Columns = this._Fields.Rows.Count; Columns < TableXLS.Columns.Count - (4 + this.MaxMinAreaLevelDiff); Columns++)
                                {
                                    //If there are Footnotes or comments available then leave the appropriate columns.
                                    //Leave single column.

                                    //Leave four columns.
                                    if (this.FootnoteCommentType == FootnoteCommentType.Both && this._TemplateStyle.Denominator.Show)
                                    {
                                        //Leave the FootnoteComment column.
                                        if (FootNoteCounter % 4 == 0)
                                        {
                                            if (DICommon.IsNumeric(DRowTableXLS[Columns].ToString(), OldCulture))		//If DataValue exists for the cell.
                                            {
                                                DataValue = this.GetRoundedDataValue(DRowTableXLS[Columns].ToString(), OldCulture, this.Themes[DICommon.RemoveQuotes(IndicatorName)].Decimal);
                                                LegendIndex = -1;
                                                foreach (Legend Legend in this.Themes[DICommon.RemoveQuotes(IndicatorName)].Legends)
                                                {
                                                    if (DataValue >= Convert.ToDouble(Legend.RangeFrom) && DataValue <= Convert.ToDouble(Legend.RangeTo))
                                                    {
                                                        LegendIndex++;
                                                        PresentInRange = true;
                                                        break;
                                                    }
                                                    LegendIndex++;
                                                }
                                                if (PresentInRange)
                                                {
                                                    ExcelFile.SetRangeColor(sheetIndex, Rows - 1, Columns, Rows - 1, Columns, this._TemplateStyle.ContentSetting.FontTemplate.ForeColor, this.Themes[DICommon.RemoveQuotes(IndicatorName)].Legends[LegendIndex].Color);
                                                    PresentInRange = false;
                                                }

                                            }
                                            DataValue = 0.00;
                                        }
                                        FootNoteCounter++;
                                    }
                                    else if ((this.FootnoteCommentType == FootnoteCommentType.Both) || ((this.FootnoteCommentType == FootnoteCommentType.Footnote || this.FootnoteCommentType == FootnoteCommentType.Comment) && this._TemplateStyle.Denominator.Show))
                                    {
                                        //Leave the FootnoteComment column.
                                        if (FootNoteCounter % 3 == 0)
                                        {
                                            if (DICommon.IsNumeric(DRowTableXLS[Columns].ToString(), OldCulture))		//If DataValue exists for the cell.
                                            {
                                                DataValue = this.GetRoundedDataValue(DRowTableXLS[Columns].ToString(), OldCulture, this.Themes[DICommon.RemoveQuotes(IndicatorName)].Decimal);
                                                LegendIndex = -1;
                                                foreach (Legend Legend in this.Themes[DICommon.RemoveQuotes(IndicatorName)].Legends)
                                                {
                                                    if (DataValue >= Convert.ToDouble(Legend.RangeFrom) && DataValue <= Convert.ToDouble(Legend.RangeTo))
                                                    {
                                                        LegendIndex++;
                                                        PresentInRange = true;
                                                        break;
                                                    }
                                                    LegendIndex++;
                                                }
                                                if (PresentInRange)
                                                {
                                                    ExcelFile.SetRangeColor(sheetIndex, Rows - 1, Columns, Rows - 1, Columns, this._TemplateStyle.ContentSetting.FontTemplate.ForeColor, this.Themes[DICommon.RemoveQuotes(IndicatorName)].Legends[LegendIndex].Color);
                                                    PresentInRange = false;
                                                }

                                            }
                                            DataValue = 0.00;
                                        }
                                        FootNoteCounter++;
                                    }
                                    else if (this.FootnoteCommentType == FootnoteCommentType.Footnote || this.FootnoteCommentType == FootnoteCommentType.Comment || this._TemplateStyle.Denominator.Show)
                                    {
                                        //Leave the FootnoteComment column.
                                        if (FootNoteCounter % 2 == 0)
                                        {
                                            if (DICommon.IsNumeric(DRowTableXLS[Columns].ToString(), OldCulture))		//If DataValue exists for the cell.
                                            {
                                                DataValue = this.GetRoundedDataValue(DRowTableXLS[Columns].ToString(), OldCulture, this.Themes[DICommon.RemoveQuotes(IndicatorName)].Decimal);
                                                LegendIndex = -1;
                                                foreach (Legend Legend in this.Themes[DICommon.RemoveQuotes(IndicatorName)].Legends)
                                                {
                                                    if (DataValue >= Convert.ToDouble(Legend.RangeFrom) && DataValue <= Convert.ToDouble(Legend.RangeTo))
                                                    {
                                                        LegendIndex++;
                                                        PresentInRange = true;
                                                        break;
                                                    }
                                                    LegendIndex++;
                                                }
                                                if (PresentInRange)
                                                {
                                                    ExcelFile.SetRangeColor(sheetIndex, Rows - 1, Columns, Rows - 1, Columns, this._TemplateStyle.ContentSetting.FontTemplate.ForeColor, this.Themes[DICommon.RemoveQuotes(IndicatorName)].Legends[LegendIndex].Color);
                                                    PresentInRange = false;
                                                }

                                            }
                                            DataValue = 0.00;
                                        }
                                        FootNoteCounter++;
                                    }
                                    //DO not leave any column.
                                    else if (this.FootnoteCommentType == FootnoteCommentType.None)
                                    {
                                        if (DICommon.IsNumeric(DRowTableXLS[Columns].ToString(), OldCulture))		//If DataValue exists for the cell.
                                        {
                                            DataValue = this.GetRoundedDataValue(DRowTableXLS[Columns].ToString(), OldCulture, this.Themes[DICommon.RemoveQuotes(IndicatorName)].Decimal);
                                            LegendIndex = -1;
                                            foreach (Legend Legend in this.Themes[DICommon.RemoveQuotes(IndicatorName)].Legends)
                                            {
                                                if (DataValue >= Convert.ToDouble(Legend.RangeFrom) && DataValue <= Convert.ToDouble(Legend.RangeTo))
                                                {
                                                    LegendIndex++;
                                                    PresentInRange = true;
                                                    break;
                                                }
                                                LegendIndex++;
                                            }
                                            if (PresentInRange)
                                            {
                                                ExcelFile.SetRangeColor(sheetIndex, Rows - 1, Columns, Rows - 1, Columns, this._TemplateStyle.ContentSetting.FontTemplate.ForeColor, this.Themes[DICommon.RemoveQuotes(IndicatorName)].Legends[LegendIndex].Color);
                                                PresentInRange = false;
                                            }
                                        }
                                        DataValue = 0.00;
                                    }
                                }
                                IndicatorName = string.Empty;
                            }
                        }
                    }
                    else if (this._Fields.Columns[Indicator.IndicatorName] != null)    //Check if indicators are present in Columns..
                    {
                        //Find the indicator position in columns collection.
                        foreach (Field Field in this._Fields.Columns)
                        {
                            if (Field.FieldID != Indicator.IndicatorName)
                            {
                                IndicatorPosition += 2;
                            }
                            else
                            {
                                break;
                            }
                        }
                        foreach (DataRow DrowColumnArangement in this._ColumnArrangementTable.Rows)
                        {
                            Rows = TitleSubTitleMargin;	   //Leave the top rows of title and subtitle of XLS file.
                            IndicatorName = DrowColumnArangement[IndicatorPosition].ToString();	 //Get the indicator name value. 
                            foreach (DataRow DRowTableXLS in TableXLS.Rows)
                            {
                                Rows++;
                                FootNoteCounter = 0;
                                if (Rows > (this._ColumnArrangementTable.Columns.Count - 2) / 2 + TitleSubTitleMargin)
                                {
                                    for (Columns = this._Fields.Rows.Count; Columns < TableXLS.Columns.Count - (2 + this.MaxMinAreaLevelDiff); Columns++)
                                    {
                                        //Locate the column in TableXLS containing containing required column name.
                                        if (TableXLS.Rows[IndicatorPosition / 2][Columns].ToString() == IndicatorName)
                                        {
                                            if (DICommon.IsNumeric(DRowTableXLS[Columns].ToString(), OldCulture))		//If DataValue exists for the cell.
                                            {
                                                DataValue = this.GetRoundedDataValue(DRowTableXLS[Columns].ToString(), OldCulture, this.Themes[DICommon.RemoveQuotes(IndicatorName)].Decimal);
                                                LegendIndex = -1;
                                                PresentInRange = false;
                                                foreach (Legend Legend in this.Themes[DICommon.RemoveQuotes(IndicatorName)].Legends)
                                                {
                                                    if (DataValue >= Convert.ToDouble(Legend.RangeFrom) && DataValue <= Convert.ToDouble(Legend.RangeTo))
                                                    {
                                                        LegendIndex++;
                                                        PresentInRange = true;
                                                        break;
                                                    }
                                                    LegendIndex++;
                                                }
                                                if (PresentInRange)
                                                {
                                                    ExcelFile.SetRangeColor(sheetIndex, Rows - 1, Columns, Rows - 1, Columns, this._TemplateStyle.ContentSetting.FontTemplate.ForeColor, this.Themes[DICommon.RemoveQuotes(IndicatorName)].Legends[LegendIndex].Color);
                                                    PresentInRange = false;
                                                }
                                            }
                                            DataValue = 0.00;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (this._DistinctIndicator.Rows.Count == 1)
                    {
                        int ExtraColumn = 1;
                        LegendIndex = -1;
                        IndicatorName = this._DistinctIndicator.Rows[0][Indicator.IndicatorName].ToString();

                        if (this.FootnoteCommentType == FootnoteCommentType.Both & this._TemplateStyle.Denominator.Show)
                        {
                            ExtraColumn = 4;
                        }
                        else if ((this.FootnoteCommentType == FootnoteCommentType.Both) || ((this.FootnoteCommentType == FootnoteCommentType.Footnote || this.FootnoteCommentType == FootnoteCommentType.Comment) && this._TemplateStyle.Denominator.Show))
                        {
                            ExtraColumn = 3;
                        }
                        else if (this.FootnoteCommentType == FootnoteCommentType.Comment || this.FootnoteCommentType == FootnoteCommentType.Footnote || this._TemplateStyle.Denominator.Show)
                        {
                            ExtraColumn = 2;
                        }

                        for (int RowIndex = 0; RowIndex < TableXLS.Rows.Count; RowIndex++)
                        {
                            if (TableXLS.Rows[RowIndex][TablePresentation.ROWTYPE].ToString() == RowType.DataRow.ToString())
                            {

                                for (int Column = this._Fields.Rows.Count; Column < TableXLS.Columns.Count - (4 + this.MaxMinAreaLevelDiff); Column += ExtraColumn)
                                {
                                    if (DICommon.IsNumeric(TableXLS.Rows[RowIndex][Column].ToString(), OldCulture))
                                    {
                                        DataValue = this.GetRoundedDataValue(TableXLS.Rows[RowIndex][Column].ToString(), OldCulture, this.Themes[DICommon.RemoveQuotes(IndicatorName)].Decimal);
                                        foreach (Legend Legend in this.Themes[DICommon.RemoveQuotes(IndicatorName)].Legends)
                                        {

                                            if (DataValue >= Convert.ToDouble(Legend.RangeFrom) && DataValue <= Convert.ToDouble(Legend.RangeTo))
                                            {
                                                LegendIndex++;
                                                PresentInRange = true;
                                                break;
                                            }
                                            LegendIndex++;
                                        }
                                        if (PresentInRange)
                                        {
                                            ExcelFile.SetRangeColor(sheetIndex, RowIndex + TitleSubTitleMargin, Column, RowIndex + TitleSubTitleMargin, Column, this._TemplateStyle.ContentSetting.FontTemplate.ForeColor, this.Themes[DICommon.RemoveQuotes(IndicatorName)].Legends[LegendIndex].Color);
                                            PresentInRange = false;
                                        }
                                        LegendIndex = -1;
                                        //break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //DO not Apply color if we have multiple indicators and the indicator field is not present either in rows or columns collection.
                    }
                    //-- Restore the culture.
                    System.Threading.Thread.CurrentThread.CurrentCulture = OldCulture;
                }
            }
            catch (Exception ex)
            {
                //-- Restore the culture.
                System.Threading.Thread.CurrentThread.CurrentCulture = OldCulture;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="culture"></param>
        /// <param name="decimalPrecisions"></param>
        /// <returns></returns>
        private double GetRoundedDataValue(string value, System.Globalization.CultureInfo culture, int decimalPrecisions)
        {
            double Retval = 0;
            try
            {
                Retval = Math.Round(Double.Parse(value, System.Globalization.NumberStyles.Any, culture), decimalPrecisions);
            }
            catch (Exception)
            {
                Retval = 0;
            }
            return Retval;
        }

        /// <summary>
        /// Suppress duplicate Headings.
        /// </summary>
        /// <param name="WorkSheet1">.XLS Active Worksheet</param>
        /// <param name="TotalColumnFields">Total Column Field count</param>
        /// <param name="TotalRowFields">Total Row field count</param>
        private void SuppressDuplicateColumn(DIExcel ExcelFile, int sheetIndex, DataTable TableXLS, Color borderColor)
        {
            int TotalColumnFields = (this._ColumnArrangementTable.Columns.Count - 2) / 2;
            int TotalRowFields = this._Fields.Rows.Count;
            string Heading = string.Empty;
            int Rows = 0;
            int Columns = 0;
            int CompareCounter = 0;
            int RowStartCounter = 0;
            int RowEndCounter = 0;
            int ColumnStartCounter = 0;
            int ColumnEndCounter = 0;

            DataTable DTStorage = new DataTable();		//DataTable storing the row and column information for duplicate headers.
            DataRow RowDTStorage;						//Row containg record of DTStorage.			

            DTStorage.Columns.Add("Start1");			//duplicate header start from this row number
            DTStorage.Columns.Add("Start2");			//duplicate column start from this column number
            DTStorage.Columns.Add("End1");				//duplicate header End to this row number
            DTStorage.Columns.Add("End2");				//duplicate column end to this row number

            for (Rows = TitleSubTitleMargin; Rows < TitleSubTitleMargin + TotalColumnFields; Rows++)
            {
                for (Columns = TotalRowFields; Columns < TableXLS.Columns.Count - (2 + this.MaxMinAreaLevelDiff); )
                {
                    if (Heading == string.Empty)
                    {
                        if ((Columns - CompareCounter) > 0)
                        {
                            Columns = Columns - CompareCounter;
                        }
                        CompareCounter = 0;
                        Heading = ExcelFile.GetCellValue(sheetIndex, Rows, Columns, Rows, Columns);
                        RowStartCounter = Rows;
                        ColumnStartCounter = Columns;
                    }
                    else
                    {
                        if (Heading != ExcelFile.GetCellValue(sheetIndex, Rows, Columns, Rows, Columns))
                        {
                            if (RowStartCounter > 0 && ColumnStartCounter > 0 && RowEndCounter > 0 && ColumnEndCounter > 0)
                            {
                                if ((RowStartCounter == TitleSubTitleMargin) || DTStorage.Select("Start1 = " + (RowStartCounter - 1)).Length > 0)
                                {
                                    if (RowStartCounter != TitleSubTitleMargin)
                                    {
                                        foreach (DataRow DRowDTStorage in DTStorage.Select("Start1 = " + (RowStartCounter - 1)))
                                        {
                                            if (Convert.ToInt32(DRowDTStorage["Start2"]) <= ColumnStartCounter && Convert.ToInt32(DRowDTStorage["End2"]) >= ColumnEndCounter)
                                            {
                                                //r = WorkSheet1.get_Range(WorkSheet1.Cells[RowStartCounter, ColumnStartCounter], WorkSheet1.Cells[RowEndCounter, ColumnEndCounter]);
                                                ExcelFile.SetHorizontalAlignment(sheetIndex, RowStartCounter, ColumnStartCounter, RowEndCounter, ColumnEndCounter, SpreadsheetGear.HAlign.Center);
                                                ExcelFile.SetVerticalAlignment(sheetIndex, RowStartCounter, ColumnStartCounter, RowEndCounter, ColumnEndCounter, SpreadsheetGear.VAlign.Center);
                                                ExcelFile.MergeCells(sheetIndex, RowStartCounter, ColumnStartCounter, RowEndCounter, ColumnEndCounter);
                                                RowDTStorage = DTStorage.NewRow();
                                                RowDTStorage[0] = RowStartCounter.ToString();
                                                RowDTStorage[1] = ColumnStartCounter.ToString();
                                                RowDTStorage[2] = RowEndCounter.ToString();
                                                RowDTStorage[3] = ColumnEndCounter.ToString();
                                                DTStorage.Rows.Add(RowDTStorage);
                                            }
                                            else if (Convert.ToInt32(DRowDTStorage["Start2"]) >= ColumnStartCounter && Convert.ToInt32(DRowDTStorage["End2"]) <= ColumnEndCounter)
                                            {
                                                //r = WorkSheet1.get_Range(WorkSheet1.Cells[RowStartCounter, Convert.ToInt32(DRowDTStorage["Start2"])], WorkSheet1.Cells[RowEndCounter, Convert.ToInt32(DRowDTStorage["End2"])]);
                                                ExcelFile.SetHorizontalAlignment(sheetIndex, RowStartCounter, Convert.ToInt32(DRowDTStorage["Start2"]), RowEndCounter, Convert.ToInt32(DRowDTStorage["End2"]), SpreadsheetGear.HAlign.Center);
                                                ExcelFile.SetVerticalAlignment(sheetIndex, RowStartCounter, Convert.ToInt32(DRowDTStorage["Start2"]), RowEndCounter, Convert.ToInt32(DRowDTStorage["End2"]), SpreadsheetGear.VAlign.Center);
                                                ExcelFile.MergeCells(sheetIndex, RowStartCounter, Convert.ToInt32(DRowDTStorage["Start2"]), RowEndCounter, Convert.ToInt32(DRowDTStorage["End2"]));
                                                RowDTStorage = DTStorage.NewRow();
                                                RowDTStorage[0] = RowStartCounter.ToString();
                                                RowDTStorage[1] = DRowDTStorage["Start2"].ToString();
                                                RowDTStorage[2] = RowEndCounter.ToString();
                                                RowDTStorage[3] = DRowDTStorage["End2"].ToString();
                                                DTStorage.Rows.Add(RowDTStorage);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //r = WorkSheet1.get_Range(WorkSheet1.Cells[RowStartCounter, ColumnStartCounter], WorkSheet1.Cells[RowEndCounter, ColumnEndCounter]);
                                        ExcelFile.SetHorizontalAlignment(sheetIndex, RowStartCounter, ColumnStartCounter, RowEndCounter, ColumnEndCounter, SpreadsheetGear.HAlign.Center);
                                        ExcelFile.SetVerticalAlignment(sheetIndex, RowStartCounter, ColumnStartCounter, RowEndCounter, ColumnEndCounter, SpreadsheetGear.VAlign.Center);
                                        ExcelFile.MergeCells(sheetIndex, RowStartCounter, ColumnStartCounter, RowEndCounter, ColumnEndCounter);
                                        RowDTStorage = DTStorage.NewRow();
                                        RowDTStorage[0] = RowStartCounter.ToString();
                                        RowDTStorage[1] = ColumnStartCounter.ToString();
                                        RowDTStorage[2] = RowEndCounter.ToString();
                                        RowDTStorage[3] = ColumnEndCounter.ToString();
                                        DTStorage.Rows.Add(RowDTStorage);
                                    }
                                }
                            }
                            Heading = string.Empty;
                            RowStartCounter = -1;
                            RowEndCounter = -1;
                            ColumnStartCounter = -1;
                            ColumnEndCounter = -1;
                        }
                        else
                        {
                            RowEndCounter = Rows;
                            ColumnEndCounter = Columns;
                        }
                        if (this.FootnoteCommentColumnCount == 0)
                        {
                            CompareCounter++;
                        }
                        else
                        {
                            CompareCounter = CompareCounter + (this.FootnoteCommentColumnCount + 1);
                        }
                        //CompareCounter++;
                    }
                    if (this.FootnoteCommentColumnCount == 0)
                    {
                        Columns++;
                    }
                    else
                    {
                        Columns = Columns + (this.FootnoteCommentColumnCount + 1);
                    }
                }
                CompareCounter = 0;
            }
        }

        /// <summary>
        /// Suppress Rows
        /// </summary>
        /// <param name="ExcelFile">Excell file</param>
        /// <param name="sheetIndex">Sheet index</param>
        /// <param name="TableXLS">TableXLS object</param>
        /// <param name="borderColor">Border color</param>
        private void SuppressDuplicateRows(DIExcel ExcelFile, int sheetIndex, DataTable TableXLS, Color borderColor, int SubtractFromRows)
        {
            int TotalColumnFields = (this._ColumnArrangementTable.Columns.Count - 2) / 2;
            string RowHeaderCaption = string.Empty;
            if (TotalColumnFields == 0)
            {
                foreach (Field Fld in this._Fields.Rows)
                {
                    RowHeaderCaption += "/" + Fld.Caption;
                }
                if (RowHeaderCaption != string.Empty)
                {
                    RowHeaderCaption = RowHeaderCaption.Substring(1);
                }
                ExcelFile.SetCellValue(sheetIndex, TitleSubTitleMargin + TotalColumnFields, 0, RowHeaderCaption);
            }
            else
            {
                foreach (Field Fld in this._Fields.Rows)
                {
                    RowHeaderCaption += "/" + Fld.Caption;
                }
                if (RowHeaderCaption != string.Empty)
                {
                    RowHeaderCaption = RowHeaderCaption.Substring(1);
                }
                ExcelFile.SetCellValue(sheetIndex, TitleSubTitleMargin + TotalColumnFields - 1, 0, RowHeaderCaption);
            }
            foreach (DataRow Drow in TableXLS.Select(TablePresentation.ROWTYPE + "='" + RowType.SupressRow.ToString() + "'"))
            {
                ExcelFile.SetColumnWidth(sheetIndex, 2, Convert.ToInt32(Drow[TablePresentation.ROWINDEX].ToString()) + TitleSubTitleMargin - 1, 0, Convert.ToInt32(Drow[TablePresentation.ROWINDEX].ToString()) + TitleSubTitleMargin, 0);
                //ExcelFile.MergeCells(sheetIndex, Convert.ToInt32(Drow[TablePresentation.ROWINDEX].ToString()) + TitleSubTitleMargin - 1, 0, Convert.ToInt32(Drow[TablePresentation.ROWINDEX].ToString()) + TitleSubTitleMargin - 1, 6);
                ExcelFile.SetHorizontalAlignment(sheetIndex, Convert.ToInt32(Drow[TablePresentation.ROWINDEX].ToString()) + TitleSubTitleMargin - 1, 0, SpreadsheetGear.HAlign.Left);
                this.SetExcelRangeFont(ExcelFile.GetCellFont(sheetIndex, Convert.ToInt32(Drow[TablePresentation.ROWINDEX].ToString()) + TitleSubTitleMargin - 1, 0), FontStyle.Bold);

                if (this._Fields.Rows.Count > 2)
                {
                    for (int ColCount = 1; ColCount < this._Fields.Rows.Count - SubtractFromRows; ColCount++)
                    {
                        this.SetExcelRangeFont(ExcelFile.GetCellFont(sheetIndex, Convert.ToInt32(Drow[TablePresentation.ROWINDEX].ToString()) + TitleSubTitleMargin - 1, ColCount), FontStyle.Bold);
                        ExcelFile.SetColumnWidth(sheetIndex, 2, Convert.ToInt32(Drow[TablePresentation.ROWINDEX].ToString()) + TitleSubTitleMargin - 1, ColCount, Convert.ToInt32(Drow[TablePresentation.ROWINDEX].ToString()) + TitleSubTitleMargin, ColCount);
                        //ExcelFile.MergeCells(sheetIndex, Convert.ToInt32(Drow[TablePresentation.ROWINDEX].ToString()) + TitleSubTitleMargin - 1, 0, Convert.ToInt32(Drow[TablePresentation.ROWINDEX].ToString()) + TitleSubTitleMargin - 1, 6);
                    }
                }
            }
            foreach (DataRow Drow in TableXLS.Select(TablePresentation.ROWTYPE + "='" + RowType.DataRow.ToString() + "'"))
            {
                ExcelFile.SetCellValue(sheetIndex, Convert.ToInt32(Drow[TablePresentation.ROWINDEX].ToString()) + TitleSubTitleMargin - 1, 0, string.Empty);
                if (this._Fields.Rows.Count > 2)
                {
                    for (int ColCount = 1; ColCount < this._Fields.Rows.Count - SubtractFromRows; ColCount++)
                    {
                        ExcelFile.SetCellValue(sheetIndex, Convert.ToInt32(Drow[TablePresentation.ROWINDEX].ToString()) + TitleSubTitleMargin - 1, ColCount, string.Empty);
                    }

                }
            }
        }

        private void SetSuppressRowStyle(DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper.DIExcel ExcelFile, string AreaLevel, int sheetIndex, int StartRowCount, int EndRowIndex, int startColIndex, int endColIndex)
        {

            ExcelFile.SetRangeColor(sheetIndex, StartRowCount, startColIndex, EndRowIndex, endColIndex, this.TemplateStyle.LevelFormat[AreaLevel].FontSetting.ForeColor, this.TemplateStyle.LevelFormat[AreaLevel].FontSetting.BackColor);
            ExcelFile.GetRangeFont(sheetIndex, StartRowCount, startColIndex, EndRowIndex, endColIndex).Name = this._TemplateStyle.LevelFormat[AreaLevel].FontSetting.FontName;
            ExcelFile.GetRangeFont(sheetIndex, StartRowCount, startColIndex, EndRowIndex, endColIndex).Size = this._TemplateStyle.LevelFormat[AreaLevel].FontSetting.FontSize;
            this.SetExcelRangeFont(ExcelFile.GetRangeFont(sheetIndex, StartRowCount, startColIndex, EndRowIndex, endColIndex), this._TemplateStyle.LevelFormat[AreaLevel].FontSetting.FontStyle);
        }

        /// <summary>
        /// Get the formatted string to format the number
        /// </summary>
        /// <param name="decimalPlaces"></param>
        /// <returns></returns>
        private string NumberFormat(int decimalPlaces, string value)
        {
            string Retval = string.Empty;
            try
            {
                if (this._TemplateStyle.ContentSetting.FontTemplate.FormatDataValue)
                {
                    if (value.IndexOf(DICommon.NumberDecimalSeparator) != -1)
                    {
                        if (value.Substring(0, value.IndexOf(DICommon.NumberDecimalSeparator)).Length > 3)
                        {
                            Retval = "{0:0,0";
                        }
                        else
                        {
                            Retval = "{0:0";
                        }
                    }
                    else
                    {
                        if (value.Length > 3)
                        {
                            Retval = "{0:0,0";
                        }
                        else
                        {
                            Retval = "{0:0";
                        }
                    }
                    //Retval = "{0:0" + DICommon.NumberGroupSeparator + "0";
                }
                else
                {
                    Retval = "{0:0";
                }

                if (this._TemplateStyle.ContentSetting.FontTemplate.RoundDataValues)
                {
                    if (decimalPlaces > 0)
                    {
                        Retval += ".";
                        //Retval += DICommon.NumberDecimalSeparator;
                    }

                    for (int i = 0; i < decimalPlaces; i++)
                    {
                        Retval += "0";
                    }
                }
                else
                {
                    if (value.Contains(Utility.DICommon.NumberDecimalSeparator))
                    {
                        int DecimalPlaces = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits;
                        Retval += ".";
                        //Retval += Utility.DICommon.NumberDecimalSeparator;
                        for (int i = 0; i < DecimalPlaces; i++)
                        {
                            Retval += "0";
                        }
                    }
                }
                Retval += "}";
            }
            catch (Exception)
            {

            }
            return Retval;
        }

        /// <summary>
        /// Apply Most Recent Data Filters
        /// </summary>
        /// <param name="dvData">Row Filter and Sort if any will be lost for dataview passed as argument</param>
        /// <returns></returns>
        /// <remarks>Record of latest timeperiod for each unique combination of IUSNId and AreaNId is considered</remarks>
        private void ApplyMRDFilter()
        {
            // The most recent data is generated based upon IUSNId, AreaNId and TimePeriod.
            // Record of latest timeperiod for each unique combination is IUSNId and AreaNId is included.

            string AreaNId = string.Empty;
            string IUSNId = string.Empty;
            string msMRDDataNIDs = string.Empty;
            StringBuilder sbMRDDataNIDs = new StringBuilder();

            // Most Recent Data
            //sort dataview in decending order of timeperiod so that latest record can be obtained
            this.PresentationData.Sort = Indicator_Unit_Subgroup.IUSNId + "," + Area.AreaNId + "," + Timeperiods.TimePeriod + " Desc";
            foreach (DataRow DRowParentTable in this.PresentationData.ToTable().Rows)
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
            this.PresentationData.RowFilter = Data.DataNId + " IN (" + msMRDDataNIDs + ")";
        }

        #endregion

        #region " -- Save / Load -- "

        /// <summary>
        /// We would be using UTF-8 encoding for the creating the XML stream for the custom object as it supports a wide range of Unicode character values and surrogates. 
        /// For this purpose, we will make use of the UTF8Encoding class provided by the .Net framework
        /// </summary>
        /// <param name="characters"></param>
        /// <returns>String</returns>
        private string UTF8ByteArrayToString(byte[] characters)
        {
            string RetVal = string.Empty;
            try
            {
                UTF8Encoding Encoding = new UTF8Encoding();
                RetVal = Encoding.GetString(characters).Trim();
            }
            catch (Exception ex)
            {
                throw;
            }
            return RetVal;
        }

        /// <summary>
        /// Use UTF8 encoding to deserailize the text
        /// </summary>
        /// <param name="SerializeText"></param>
        /// <returns></returns>
        private static byte[] StringToUTF8ByteArray(string SerializeText)
        {
            byte[] RetVal;
            try
            {
                UTF8Encoding encoding = new UTF8Encoding();
                RetVal = encoding.GetBytes(SerializeText);
            }
            catch (Exception)
            {

                throw;
            }
            return RetVal;
        }


        #endregion

        #endregion

        #region " -- Deserialization Process Load Xml and updating Color, IC and Column Arrangement -- "

        /// <summary>
        /// Update the Indicator list based on the GIds.
        /// </summary>
        private void UpdateApplyColorIndicator()
        {
            Themes TempTheme = null;

            if (this._AddColor && this.PreserveTheme)
            {
                TempTheme = (Themes)this._Themes.Clone();
            }

            if (this._DistinctIndicator != null)
            {
                string DeletedRows = string.Empty;
                int DeletedRowIndex = 0;
                string[] RowIndex = new string[0];

                foreach (DataRow Row in this._DistinctIndicator.Rows)
                {
                    DataRow[] Rows;
                    // -- Check for the indicator on the basis of GId.
                    Rows = this.PresentationData.Table.Select(Indicator.IndicatorGId + " = '" + Row[Indicator.IndicatorGId].ToString() + "'");
                    if (Rows.Length <= 0)
                    {
                        // -- If the indicator is not in the new generated dataview.
                        DeletedRows += DeletedRowIndex.ToString() + ",";
                    }
                    DeletedRowIndex += 1;
                }

                RowIndex = DeletedRows.Split(",".ToCharArray());
                DeletedRowIndex = 0;
                for (int i = 0; i < RowIndex.Length - 1; i++)
                {
                    // -- Deleted the rows from the datatable.
                    this._DistinctIndicator.Rows.RemoveAt(Convert.ToInt32(RowIndex[i]) - DeletedRowIndex);
                    // -- Delete the theme
                    this.Themes.Remove(Convert.ToInt32(RowIndex[i]) - DeletedRowIndex);
                    DeletedRowIndex += 1;
                }
            }

            try
            {
                if (this._AddColor && this.PreserveTheme)
                {
                    this._Themes.Clear();
                    //-- Create the default theme
                    Color[] LegendColor = new Color[4];
                    LegendColor[0] = ColorTranslator.FromHtml(this._UserPreference.Mapping.DefaultLegendColors[0]);
                    LegendColor[1] = ColorTranslator.FromHtml(this._UserPreference.Mapping.DefaultLegendColors[1]);
                    LegendColor[2] = ColorTranslator.FromHtml(this._UserPreference.Mapping.DefaultLegendColors[2]);
                    LegendColor[3] = ColorTranslator.FromHtml(this._UserPreference.Mapping.DefaultLegendColors[3]);
                    this.SetColorTheme(LegendColor);

                    //-- Set the previous break count and break type
                    this._Themes[0].BreakCount = TempTheme[0].BreakCount;
                    this._Themes[0].BreakType = TempTheme[0].BreakType;

                    //-- Set the previous color and caption
                    int Index = 0;
                    foreach (Legend IndLegend in TempTheme[0].Legends)
                    {
                        this._Themes[0].Legends[Index].Caption = IndLegend.Caption;
                        this._Themes[0].Legends[Index].Color = IndLegend.Color;
                        Index += 1;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Update the Indicator list based on the GIds.
        /// </summary>
        private void UpdateApplyColorForIUS()
        {
            Themes TempTheme = null;

            if (this._FrequencyThemes != null)
            {
                TempTheme = (Themes)this._FrequencyThemes.Clone();


                if (this._DistinctIUS != null)
                {
                    string DeletedRows = string.Empty;
                    int DeletedRowIndex = 0;
                    string[] RowIndex = new string[0];

                    foreach (DataRow Row in this._DistinctIUS.Rows)
                    {
                        DataRow[] Rows;
                        // -- Check for the IUS on the basis of GId.
                        Rows = this.PresentationData.Table.Select(Indicator_Unit_Subgroup.IUSNId + " = '" + Row[Indicator_Unit_Subgroup.IUSNId].ToString() + "'");
                        if (Rows.Length <= 0)
                        {
                            // -- If the indicator is not in the new generated dataview.
                            DeletedRows += "," + DeletedRowIndex.ToString();
                        }
                        DeletedRowIndex += 1;
                    }

                    if (!string.IsNullOrEmpty(DeletedRows))
                    {
                        RowIndex = DICommon.SplitString(DeletedRows.Substring(1), ",");
                        DeletedRowIndex = 0;
                        for (int i = 0; i < RowIndex.Length - 1; i++)
                        {
                            // -- Deleted the rows from the datatable.
                            this._DistinctIUS.Rows.RemoveAt(Convert.ToInt32(RowIndex[i]) - DeletedRowIndex);
                            // -- Delete the theme
                            this.FrequencyThemes.Remove(Convert.ToInt32(RowIndex[i]) - DeletedRowIndex);
                            DeletedRowIndex += 1;
                        }
                    }
                }

                try
                {
                    this._FrequencyThemes.Clear();
                    //-- Create the default theme
                    Color[] LegendColor = new Color[4];
                    LegendColor[0] = ColorTranslator.FromHtml(this._UserPreference.Mapping.DefaultLegendColors[0]);
                    LegendColor[1] = ColorTranslator.FromHtml(this._UserPreference.Mapping.DefaultLegendColors[1]);
                    LegendColor[2] = ColorTranslator.FromHtml(this._UserPreference.Mapping.DefaultLegendColors[2]);
                    LegendColor[3] = ColorTranslator.FromHtml(this._UserPreference.Mapping.DefaultLegendColors[3]);
                    this.SetColorThemeForStats(LegendColor);


                    //-- Set the previous caption
                    if (TempTheme.Count == 1 && this._FrequencyThemes.Count == 1)
                    {
                        //-- Set the previous break count and break type
                        this._FrequencyThemes[0].BreakCount = TempTheme[0].BreakCount;
                        this._FrequencyThemes[0].BreakType = TempTheme[0].BreakType;
                    }
                    else
                    {
                        foreach (Theme NewTheme in this.FrequencyThemes)
                        {
                            foreach (Theme theme in TempTheme)
                            {
                                if (theme.IndicatorNId == NewTheme.IndicatorNId)
                                {
                                    //-- Set the previous break count and break type
                                    NewTheme.BreakCount = theme.BreakCount;
                                    NewTheme.BreakType = theme.BreakType;
                                    NewTheme.Decimal = theme.Decimal;
                                    int Index = 0;

                                    foreach (Legend IndLegend in theme.Legends)
                                    {
                                        NewTheme.Legends[Index].Caption = IndLegend.Caption;
                                        Index += 1;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        /// <summary>
        /// Update the indicator classification values.
        /// </summary>
        /// <param name="dIConnection">DIConnection</param>
        /// <param name="dIQueries">DIQueries</param>
        private void UpdateIndicatorClassification(DIConnection dIConnection, DIQueries dIQueries)
        {
            if (this.SelectedIndicatorClassification)
            {
                string DeletedRows = string.Empty;
                int DeletedRowIndex = 0;
                string[] RowIndex = new string[0];

                DataTable ICTable = new DataTable();
                string SqlQuery = dIQueries.IndicatorClassification.GetICTopParents(FilterFieldType.None, "", this._IndicatorClassification);
                ICTable = dIConnection.ExecuteDataTable(SqlQuery);

                int OldICValCount = this._ClassificationValuesTable.Rows.Count;
                int NewICValCount = ICTable.Rows.Count;

                for (int i = 0; i < Math.Max(OldICValCount, NewICValCount); i++)
                {
                    // -- Remove the value which does not exist in new dataset
                    if (i <= this._ClassificationValuesTable.Rows.Count)
                    {
                        DataRow Row = this._ClassificationValuesTable.Rows[i];
                        DataRow[] Rows;
                        Rows = ICTable.Select(IndicatorClassifications.ICGId + "='" + Row[IndicatorClassifications.ICGId] + "'");
                        if (Rows.Length <= 0)
                        {
                            // -- If the Indicator Classification  value is not in the new generated dataview.
                            DeletedRows += DeletedRowIndex.ToString() + ",";
                        }
                        DeletedRowIndex += 1;
                    }

                    // -- Insert the value which  are new in new generated dataset
                    if (i < ICTable.Rows.Count)
                    {
                        DataRow Row = ICTable.Rows[i];
                        DataRow[] Rows;
                        Rows = this.ClassificationValuesTable.Select(IndicatorClassifications.ICGId + "='" + Row[IndicatorClassifications.ICGId] + "'");

                        if (Rows.Length <= 0)
                        {
                            // -- Add new IC value in the Data value.
                            DataRow ICNewRow;
                            ICNewRow = this._ClassificationValuesTable.NewRow();
                            ICNewRow[IndicatorClassifications.ICNId] = Row[IndicatorClassifications.ICNId];
                            ICNewRow[IndicatorClassifications.ICGId] = Row[IndicatorClassifications.ICGId];
                            ICNewRow[IndicatorClassifications.ICName] = Row[IndicatorClassifications.ICName];
                            ICNewRow["Selected"] = false;
                            ICNewRow["Order"] = this._ClassificationValuesTable.Rows.Count;
                            this._ClassificationValuesTable.Rows.Add(ICNewRow);
                        }
                    }
                }

                RowIndex = DeletedRows.Split(",".ToCharArray());
                DeletedRowIndex = 0;
                for (int i = 0; i < RowIndex.Length - 1; i++)
                {
                    // -- deleted the rows from the datatable.
                    this._ClassificationValuesTable.Rows.RemoveAt(Convert.ToInt32(RowIndex[i]) - DeletedRowIndex);
                    DeletedRowIndex += 1;
                }
            }
        }

        private void UpdateColumnArrangement()
        {
            string DeletedRows = string.Empty;
            int DeletedRowIndex = 0;
            string[] RowIndex = new string[0];

            if (!string.IsNullOrEmpty(this._TableReport.AreaSelection.AreaLevels))
            {
                this._ColumnArrangementTable = this.SetColumnArrangement(true);
            }
            else
            {
                DataTable ColArrangement = new DataTable();
                ColArrangement = this.SetColumnArrangement(false);

                int ColumnCount = this._ColumnArrangementTable.Columns.Count;
                StringBuilder RowFilter = new StringBuilder();

                for (int i = 0; i < this._ColumnArrangementTable.Rows.Count; i++)
                {
                    for (int j = 1; j < ColumnCount - 2; j++)
                    {
                        if (!this._ColumnArrangementTable.Columns[j - 1].ColumnName.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.Metadata) && !this._ColumnArrangementTable.Columns[j - 1].ColumnName.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.IC))
                        {
                            if (j % 2 != 0)
                            {
                                // -- Build the row filter on the basis of their respective columns.
                                if (RowFilter.Length > 0)
                                {
                                    // -- add the AND word after any condition.
                                    RowFilter.Append(" And ");
                                }
                                if (this._ColumnArrangementTable.Rows.Count - 1 >= i)
                                {
                                    RowFilter.Append(this.AddSlash(this._ColumnArrangementTable.Columns[j].ColumnName) + "= '" + DICommon.RemoveQuotes(this._ColumnArrangementTable.Rows[i][j].ToString()) + "'");
                                }
                                else
                                {
                                    RowFilter.Append(this.AddSlash(this._ColumnArrangementTable.Columns[j].ColumnName) + "= ''");
                                }
                            }
                        }
                    }
                    DataRow[] Row;
                    Row = ColArrangement.Select(RowFilter.ToString());
                    if (Row.Length <= 0)
                    {
                        DeletedRows += i.ToString() + ",";
                    }
                    RowFilter.Length = 0;
                }

                try
                {
                    RowIndex = DeletedRows.Split(",".ToCharArray());
                    DeletedRowIndex = 0;
                    for (int i = 0; i < RowIndex.Length - 1; i++)
                    {
                        // -- deleted the rows from the datatable.

                        this._ColumnArrangementTable.Rows.RemoveAt(Convert.ToInt32(RowIndex[i]) - DeletedRowIndex);
                        DeletedRowIndex += 1;
                    }

                }
                catch (Exception)
                {
                }

                try
                {
                    //-- Add the new row in the column arrangement
                    ColumnCount = ColArrangement.Columns.Count;
                    RowFilter.Length = 0;
                    string NewRows = string.Empty;

                    for (int i = 0; i < ColArrangement.Rows.Count; i++)
                    {
                        for (int j = 1; j < ColumnCount - 2; j++)
                        {
                            if (!ColArrangement.Columns[j - 1].ColumnName.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.Metadata) && !ColArrangement.Columns[j - 1].ColumnName.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.IC))
                            {
                                if (j % 2 != 0)
                                {
                                    // -- Build the row filter on the basis of their respective columns.
                                    if (RowFilter.Length > 0)
                                    {
                                        // -- add the AND word after any condition.
                                        RowFilter.Append(" And ");
                                    }
                                    if (ColArrangement.Rows.Count - 1 >= i)
                                    {
                                        RowFilter.Append(this.AddSlash(ColArrangement.Columns[j].ColumnName) + "= '" + DICommon.RemoveQuotes(ColArrangement.Rows[i][j].ToString()) + "'");
                                    }
                                    else
                                    {
                                        RowFilter.Append(this.AddSlash(ColArrangement.Columns[j].ColumnName) + "= ''");
                                    }
                                }
                            }
                        }
                        DataRow[] Row;
                        Row = this._ColumnArrangementTable.Select(RowFilter.ToString());
                        if (Row.Length <= 0)
                        {
                            NewRows += i.ToString() + ",";
                        }
                        RowFilter.Length = 0;
                    }

                    try
                    {
                        RowIndex = NewRows.Split(",".ToCharArray());
                        for (int i = 0; i < RowIndex.Length - 1; i++)
                        {
                            // -- Append the new rows from the new column arrangement datatable.
                            this._ColumnArrangementTable.ImportRow(ColArrangement.Rows[Convert.ToInt32(RowIndex[i])]);
                        }
                    }
                    catch (Exception)
                    {

                        //throw;
                    }
                }
                catch (Exception)
                {
                }
            }
        }


        #endregion

        #region " -- Raise Event --"

        /// <summary>
        /// Raise the event to update UI on change of column arrangement
        /// </summary>
        public void RaiseChangeColumnArrangementEvent()
        {
            if (this.ChangeColumnArrangementEvent != null)
            {
                this.ChangeColumnArrangementEvent();
            }
        }


        /// <summary>
        /// Raise the event for the progress bar
        /// </summary>
        /// <param name="legendIndex"></param>        
        private void RaiseEventProgressBar(int ParentProgress, int ChildProgress)
        {
            if (this.ProgressBarEvent != null)
            {
                this.ProgressBarEvent(ParentProgress, ChildProgress);
            }
        }

        /// <summary>
        /// Raise the event for the update sort order
        /// </summary>
        private void RaiseUpdateSortEvent()
        {
            if (this.UpdateSortEvent != null)
            {
                this.UpdateSortEvent();
            }
        }

        #endregion

        #endregion

        #region "-- Constants --"

        /// <summary>
        /// Constant to be prefixed with Column name in columnArrangement table to handle the issue of "Total"
        /// </summary>
        public const string DisplayText = "Disp";

        /// <summary>
        /// Constant for Language string of indicator used to set caption of field object.
        /// </summary>
        public const string NONE = "NONE";
        /// <summary>
        /// Constant for Language string of indicator used to set caption of field object.
        /// </summary>
        private const string INDICATOR = "INDICATOR";
        /// <summary>
        /// Constant for Language string of areaid used to set caption of field object.
        /// </summary>
        private const string AREAID = "AREAID";
        /// <summary>
        /// Constant for Language string of areaname used to set caption of field object.
        /// </summary>
        private const string AREANAME = "AREANAME";
        /// <summary>
        /// Constant for Language string of timeperiod used to set caption of field object.
        /// </summary>
        private const string TIMEPERIOD = "TIMEPERIOD";
        /// <summary>
        /// Constant for Language string of unit used to set caption of field object.
        /// </summary>
        private const string UNIT = "UNIT";
        /// <summary>
        /// Constant for Language string of source used to set caption of field object.
        /// </summary>
        private const string SOURCE = "SOURCE";
        /// <summary>
        /// Constant for Language string of subgroup used to set caption of field object.
        /// </summary>
        private const string SUBGROUP = "SUBGROUP";
        /// <summary>
        /// Constant for Language string of age used to set caption of field object.
        /// </summary>
        private const string AGE = "AGE";
        /// <summary>
        /// Constant for Language string of sex used to set caption of field object.
        /// </summary>
        private const string SEX = "SEX";
        /// <summary>
        /// Constant for Language string of location used to set caption of field object.
        /// </summary>
        private const string LOCATION = "LOCATION";
        /// <summary>
        /// Constant for Language string of others used to set caption of field object.
        /// </summary>
        private const string OTHERS = "OTHERS";

        /// <summary>
        /// Constant for Language string of denominator used to set caption of field object.
        /// </summary>
        private const string DENOMINATOR = "DENOMINATOR";

        /// <summary>
        /// Constant for Language string of datavalue used to set caption of field object.
        /// </summary>
        private const string DATAVALUE = "DATAVALUE";

        /// <summary>
        /// Constant for DummyColumn.
        /// </summary>	
        private const string DUMMYCOLUMN = "DummyColumn";

        /// <summary>
        /// Extra Column in TableXLS to identify row type for applying color
        /// </summary>
        private const string ROWTYPE = "RowType";

        /// <summary>
        /// Constant for Area Level
        /// </summary>
        public const string AREALEVEL = "AreaLevel";

        /// <summary>
        /// Constant for Parent Child Relation
        /// </summary>
        public const string AREAPARENTCHILDRELATION = "ParentChildID";

        /// <summary>
        /// Extra Column in TableXLS to hold the Row index.
        /// </summary>
        private const string ROWINDEX = "RowIndex";

        /// <summary>
        /// Contains the number of rows left on top of XLS file for Title and SubTitle.
        /// </summary>
        public const int TitleSubTitleMargin = 3;

        /// <summary>
        /// Constant for Metadata Field Prefix
        /// </summary>
        //private const string MetadataPrefix = DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.Metadata; //"MD";

        /// <summary>
        /// Constant for Indicator Metadata  FieldId. FieldId is generated on the fly like MD_IND_1 etc
        /// </summary>
        //internal const string MetadataIndicator = TablePresentation.MetadataPrefix + "_IND_";

        /// <summary>
        /// Constant for Area Metadata FieldId. FieldId is generated on the fly like MD_AREA_1 etc
        /// </summary>
        //internal const string MetadataArea = TablePresentation.MetadataPrefix + "_AREA_";

        /// <summary>
        /// Constant for Source Metadata FieldId. FieldId is generated on the fly like MD_SRC_1 etc
        /// </summary>
        //internal const string MetadataSource = TablePresentation.MetadataPrefix + "_SRC_";

        /// <summary>
        ///  Constant for Indicator Classification Field Prefix. "CLS"
        /// </summary>
        /// <value></value>
        //private const string ICPrefix = "CLS";

        /// <summary>
        /// Constant for Sector FieldId. FieldId is generated on fly file like CLS_SC_1 etc
        /// </summary>
        //internal const string ICSector = TablePresentation.ICPrefix + "_SC_";

        /// <summary>
        /// Constant for Goal FieldId. FieldId is generated on the fly like CLS_SC_1 etc
        /// </summary>
        //internal const string ICGoal = TablePresentation.ICPrefix + "_GL_";

        /// <summary>
        /// Constant for Theme FieldId. FieldId is generated on the fly like CLS_TH_1 etc
        /// </summary>
        //internal const string ICTheme = TablePresentation.ICPrefix + "_TH_";

        /// <summary>
        /// Constant for Source FieldId. FieldId is generated on the fly like CLS_SR_1 etc
        /// </summary>
        //internal const string ICSource = TablePresentation.ICPrefix + "_SR_";

        /// <summary>
        /// Constant for Institution FieldId. FieldId is generated on the fly like CLS_IT_1 etc
        /// </summary>
        //internal const string ICInstitution = TablePresentation.ICPrefix + "_IT_";

        /// <summary>
        /// Constant for Convention FieldId. FieldId is generated on the fly like CLS_CN_1 etc
        /// </summary>
        //internal const string ICConvention = TablePresentation.ICPrefix + "_CN_";

        /// <summary>
        /// Column name for column arrangement table.
        /// </summary>
        internal const string RecordId = "RecordId";

        /// <summary>
        /// Column name for column arrangement table.
        /// </summary>
        internal const string Order = "Order";


        /// <summary>
        /// Constant to be used as datacolumn name in Aggreagate fields data table
        /// </summary>
        public const string FieldIds = "FIELDID";

        /// <summary>
        /// Constant to be used as datacolumn name in Aggreagate fields data table
        /// </summary>
        public const string FieldCaption = "FIELDCAPTION";

        /// <summary>
        /// devinfo
        /// </summary>
        public const string SHEETPASSWORD = "devinfo";

        //=======Constants for Frequency===         
        /// <summary>
        /// devinfo
        /// </summary>
        public const string MINIMUM = "Minimum";

        /// <summary>
        /// devinfo
        /// </summary>
        public const string MAXIMUM = "Maximum";

        /// <summary>
        /// devinfo
        /// </summary>
        public const string CAPTION = "Caption";

        /// <summary>
        /// devinfo
        /// </summary>
        public const string FROM = "From";

        /// <summary>
        /// devinfo
        /// </summary>
        public const string TO = "To";

        /// <summary>
        /// devinfo
        /// </summary>
        public const string RANGE = "Range";

        /// <summary>
        /// devinfo
        /// </summary>
        public const string COUNT = "Count";
        //=================================



        #endregion

        #region "-- Events / Event Handeler --"

        /// <summary>
        /// Update the column arrangement, if there is any change in Column collection
        /// </summary>
        private void _Fields_UpdateColumnAggangementEvent()
        {
            this.ColumnArrangementTable = this.SetColumnArrangement(true);
            this.RaiseChangeColumnArrangementEvent();
        }

        /// <summary>
        /// Executes when an item gets removed or added from rows collection.
        /// </summary>
        /// <param name="FieldID">Field object ID</param>
        /// <param name="OpType">true, when field is added. False, when field is removed.</param>
        /// <remarks>
        /// Aggregate Functionality on Step 1 of Table Wizard
        /// </remarks>
        private void _Fields_ChangeAggregateFieldEvent(string FieldID, bool OpType)
        {
            DataRow[] Rows;
            Rows = this._AggregatesFields.Select(FieldIds + " = " + " '" + DICommon.RemoveQuotes(FieldID) + "'");
            if (OpType)
            {
                // check Row field is in the aggregate table.
                if (Rows.Length == 0 && !FieldID.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.Metadata) && !FieldID.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.IC))
                {
                    DataRow Row;
                    Row = this._AggregatesFields.NewRow();
                    Row[0] = FieldID;
                    Row[1] = this._Fields.All[FieldID].Caption;
                    this._AggregatesFields.Rows.Add(Row);
                    this._AggregatesFields.AcceptChanges();
                }
            }
            else
            {
                // Remove the Row from table
                if (Rows.Length > 0)
                {
                    this._AggregatesFields.Rows.Remove(Rows[0]);
                }
                //If the Aggregate field is the same as the one removed from the Rows collection then reset the Aggregate field to NONE field

                if (this.AggregateFieldID == FieldID)
                {
                    this._AggregateFieldID = TablePresentation.NONE;
                    this.AggregateFunction = AggregateType.Sum;
                }
            }
        }


        #endregion

        #region " -- Public / Friend -- "

        #region "-- Constructor --"

        /// <summary>
        /// Initializes the Field object and attaches an event of type ChangeAggregateFieldDelegate
        /// </summary>
        /// <param name="presentationData">DataView</param>
        /// <param name="DIConnection">DIConnection used in IC Classification of step 3 & its implementation in step 6 and also for footnotes text and comments text in step 6</param>
        /// <param name="DIQueries">DIQueries</param>
        /// <param name="UserSelection">UserSelection2 (New UserSelection)</param>
        /// <param name="LanguageFilePath">Obsolete not in use. May set string.empty for time being </param>
        /// <remarks>
        /// Structure of PresentationData
        /// Data_NId | IUSNID | FootnoteNId | Data_Value | Data_Denominator | Indicator_NId | Indicator_GID | Indicator_Name | Unit_NId | Unit_GID | Unit_Name | Subgroup_Val_NId | Subgroup_Val_GID | Subgroup_Val | Subgroup_Val_Age | Subgroup_Val_Sex | Subgroup_Val_Location | Subgroup_Val_Others | TimePeriod_NId | TimePeriod | Area_Nid | Area_Parent_Nid | Area_ID | Area_Name | IC_NId | IC_Name | Subgroup_Name_Age | Subgroup_GID_Age | Subgroup_Name_Sex | Subgroup_GID_sex | Subgroup_Name_Location | Subgroup_GID_Location | Subgroup_Name_Others | Subgroup_GID_Others | DataType | NumericData | TextualData
        /// </remarks>
        public TablePresentation(DIDataView DIDataView, DIConnection DIConnection, DIQueries DIQueries, UserPreference.UserPreference UserPreference, string LanguageFilePath)
        {
            //TODO remove language parameter

            //Set private variable for DIDataview
            this.DIDataView = DIDataView;

            // Set the property which will hold the dataview. 
            //this.PresentationData = DIDataView.GetPresentationData();
            DataTable dtPres = DIDataView.GetPresentationData().ToTable();

            this.PresentationData = dtPres.DefaultView;

            // Set the rows count of presentation data.
            this._PresentationDataRowCount = this.PresentationData.Count;

            //Sets the private variable to connection  object.
            this.DIConnection = DIConnection;

            //Sets the private variable to DiQueries object.
            this.DIQueries = DIQueries;

            //Set the user selection.
            this._UserPreference = UserPreference;

            // Wizard STEP 5: Styles / Template.
            this._TemplateStyle = new StyleTemplate();

            // Create the object of report layout
            this._TableReport = new TableReportInfo();

            // Wizard STEP 1: initialize the Fields class to attach an event for the Aggregate Field.
            this._Fields = new Fields();
            _Fields.ChangeAggregateFieldEvent += new ChangeAggregateFieldDelegate(_Fields_ChangeAggregateFieldEvent);

            // Wizard STEP 1: Fill "ALL" fields collection with all fields and use language file for setting langugae caption.
            this.FillAllFields();

            //wizard step 1: Create the table for aggregate fields and define its column.
            this.SetAggregateTable();

            // Wizard STEP 1: Set Rows, Columns and Available collections.
            if (this.IsPyramidChart)
            {
                ArrangeCollectionFieldsForPyramid(this.PresentationData);
            }
            else
            {
                this.ArrangeCollectionFields(this.PresentationData);
            }


            // Wizard STEP 2: Set the Title and Subtitle of table.
            this.SetTitleAndSubtitle();

            // Wizard STEP 3: Update the column arrangement, if there is any change in Column collection
            this._Fields.UpdateColumnAggangementEvent += new UpdateColumnAggangementDelegate(_Fields_UpdateColumnAggangementEvent);

            // Wizard STEP 3: Column Arrangement.
            this._ColumnArrangementTable = this.SetColumnArrangement(true);

            //Set the Language key values based on Language file selected.
            this.ApplyLanguageSettings();
        }


        /// <summary>
        /// Blank constructor used for Serialization.
        /// </summary>
        public TablePresentation()
        {

        }

        #endregion

        #region " -- Event -- "

        /// <summary>
        /// Event to raise on change of column arrangement
        /// </summary>
        public event ChangeColumnArrangementDelegate ChangeColumnArrangementEvent;

        /// <summary>
        /// Event for progress bar
        /// </summary>
        public event DelegateProgressBar ProgressBarEvent;

        /// <summary>
        /// Event to update the sort order.
        /// </summary>
        public event DelegateUpdateSort UpdateSortEvent;


        #endregion

        #region " -- Properties -- "

        #region " -- Common Properties --"

        private PresentationOutputType _PresentationOutputType = PresentationOutputType.ExcellSheet;
        /// <summary>																				
        /// Gets or Sets the Presentation output type.
        /// </summary>
        public PresentationOutputType PresentationOutputType
        {
            get
            {
                return _PresentationOutputType;
            }
            set
            {
                _PresentationOutputType = value;
            }
        }

        private UserPreference.UserPreference _UserPreference;
        /// <summary>
        /// Gets or sets the user preference.
        /// </summary>
        public UserPreference.UserPreference UserPreference
        {
            get
            {
                return _UserPreference;
            }
            set
            {
                _UserPreference = value;
            }
        }

        private int _PresentationDataRowCount;
        /// <summary>
        /// Set the presentation row count. 
        /// </summary>
        /// <remarks>Validate for number of rows in dataview for the hosting application</remarks>
        [XmlIgnore()]
        public int PresentationDataRowCount
        {
            get
            {
                return this._PresentationDataRowCount;
            }
        }

        private Presentation.PresentationType _PresentationType = Presentation.PresentationType.Table;
        /// <summary>
        /// Gets or sets the access of tablepresentaion.
        /// </summary>
        public Presentation.PresentationType PresentationType
        {
            get
            {
                return this._PresentationType;
            }
            set
            {
                this._PresentationType = value;
            }
        }

        private bool _ShowRowColumnHeader = false;
        /// <summary>
        /// Gets or sets the show row and column header of spreadsheet gear
        /// </summary>
        public bool ShowRowColumnHeader
        {
            get
            {
                return this._ShowRowColumnHeader;
            }
            set
            {
                this._ShowRowColumnHeader = value;
            }
        }

        private DIDataView _DIDataView;
        /// <summary>
        /// Will contain DIDataView object in the counstructor
        /// </summary>
        [XmlIgnore()]
        public DIDataView DIDataView
        {
            get
            {
                return this._DIDataView;
            }
            set
            {
                this._DIDataView = value;
            }
        }

        private string _Title = string.Empty;
        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public string Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                this._Title = value;
            }
        }

        private string _Subtitle = string.Empty;
        /// <summary>
        /// Gets or sets the sub title
        /// </summary>
        public string Subtitle
        {
            get
            {
                return this._Subtitle;
            }
            set
            {
                this._Subtitle = value;
            }
        }

        //Holds the PresentationData, passed from the Hosting application.
        [XmlIgnore()]
        public DataView PresentationData;

        private TablePresentationMessages _TPMessage = TablePresentationMessages.None;
        /// <summary>
        /// Gets or sets the TablePresentationMessages
        /// </summary>
        public TablePresentationMessages TPMessage
        {
            get { return _TPMessage; }
            set { _TPMessage = value; }
        }

        private TableReportInfo _TableReport = new TableReportInfo();
        /// <summary>
        /// Gets or sets the report layout
        /// </summary>
        public TableReportInfo TableReport
        {
            get
            {
                return this._TableReport;
            }
            set
            {
                this._TableReport = value;
            }
        }

        /// <summary>
        /// Gets the Boolean of whether AutoFit is applicable or not
        /// </summary>										
        public Boolean IsAllowAutoFit
        {
            get
            {
                if (this._IsSuppressDuplicateColumns || this._IsSuppressDuplicateRows || this._AggregateOn || this._SelectedIndicatorClassification || !string.IsNullOrEmpty(this._TableReport.AreaSelection.AreaLevels))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private int _TotalOrderColCount = 0;
        /// <summary>
        /// Gets or sets the column count for oders columns like subgroup and subgroupvals
        /// </summary>
        public int TotalOrderColCount
        {
            get { return _TotalOrderColCount; }
            set { _TotalOrderColCount = value; }
        }

        #endregion

        #region " -- STEP 1 -- "

        private Fields _Fields;

        public Fields Fields
        {
            get
            {
                return this._Fields;
            }
            set
            {
                // added for serialization
                this._Fields = value;
            }
        }

        private bool _SuppressDuplicate = false;
        /// <summary>
        /// Get or Set the SuppressDuplicate
        /// </summary>
        public bool SuppressDuplicate
        {
            get
            {
                return this._SuppressDuplicate;
            }
            set
            {
                this._SuppressDuplicate = value;
            }
        }

        private bool _IsSuppressDuplicateColumns = false;
        /// <summary>
        /// 
        /// </summary>
        public bool IsSuppressDuplicateColumns
        {
            get { return _IsSuppressDuplicateColumns; }
            set { _IsSuppressDuplicateColumns = value; }
        }

        private bool _IsSuppressDuplicateRows = false;
        /// <summary>
        /// 
        /// </summary>
        public bool IsSuppressDuplicateRows
        {
            get { return _IsSuppressDuplicateRows; }
            set { _IsSuppressDuplicateRows = value; }
        }

        private bool _AggregateOn = false;
        /// <summary>
        /// Get or Set the AggregateOn
        /// </summary>
        public bool AggregateOn
        {
            get
            {
                return this._AggregateOn;
            }
            set
            {
                this._AggregateOn = value;
            }
        }

        private AggregateType _AggregateFunction = AggregateType.Sum;
        /// <summary>
        /// Get or Set the AggregateFunction
        /// </summary>
        public AggregateType AggregateFunction
        {
            get
            {
                return this._AggregateFunction;
            }
            set
            {
                this._AggregateFunction = value;
            }
        }

        private List<AggregateType> _LstAggregateFunction = new List<AggregateType>();
        /// <summary>
        /// Gets or sets the list of aggregatefunction
        /// </summary>
        public List<AggregateType> LstAggregateFunction
        {
            get { return _LstAggregateFunction; }
            set { _LstAggregateFunction = value; }
        }


        private string _AggregateFieldID = TablePresentation.NONE;
        /// <summary>
        /// Get or Set the AggregateField
        /// </summary>
        public string AggregateFieldID
        {
            get
            {
                return this._AggregateFieldID;
            }
            set
            {
                this._AggregateFieldID = value;
            }
        }

        #endregion

        #region " -- STEP 2 -- "

        //private string _Title = string.Empty;
        ///// <summary>
        ///// Get or Set the title
        ///// </summary>
        //public string Title
        //{
        //    get
        //    {
        //        return this._Title;
        //    }
        //    set
        //    {
        //        this._Title = value;
        //    }
        //}


        //private String _SubTitle = string.Empty;
        ///// <summary>
        ///// Get or set the sub title
        ///// </summary>
        //public String SubTitle
        //{
        //    get
        //    {
        //        return this._SubTitle;
        //    }
        //    set
        //    {
        //        this._SubTitle = value;
        //    }
        //}

        //private bool _FootnoteShow = true;
        ///// <summary>
        ///// Get or set the FootnoteShow (Footnotes visibility)
        ///// </summary>
        //public bool FootnoteShow
        //{
        //    get
        //    {
        //        return this._FootnoteShow;
        //    }
        //    set
        //    {
        //        this._FootnoteShow = value;
        //    }
        //}

        //private FootNoteDisplayStyle _FootnoteInLine = FootNoteDisplayStyle.Seprate;
        ///// <summary>
        ///// Get or set the FootnoteInLine
        ///// </summary>
        //public FootNoteDisplayStyle FootnoteInLine
        //{
        //    get
        //    {
        //        return this._FootnoteInLine;
        //    }
        //    set
        //    {
        //        this._FootnoteInLine = value;
        //    }
        //}

        //private bool _CommentShow = true;
        ///// <summary>
        ///// Get or sets the Commentshow( Comment visibility)
        ///// </summary>
        //public bool CommentShow
        //{
        //    get
        //    {
        //        return this._CommentShow;
        //    }
        //    set
        //    {
        //        this._CommentShow = value;
        //    }
        //}

        //private bool _CommentInLine = false;
        ///// <summary>
        ///// Get  or sets the CommentInLine
        ///// </summary>
        //public bool CommentInLine
        //{
        //    get
        //    {
        //        return this._CommentInLine;
        //    }
        //    set
        //    {
        //        this._CommentInLine = value;
        //    }
        //}

        //private bool _ShowDenominator = false;
        ///// <summary>
        ///// Gets or Sets the denominator
        ///// </summary>
        //public bool ShowDenominaor
        //{
        //    get
        //    {
        //        return _ShowDenominator;
        //    }
        //    set
        //    {
        //        _ShowDenominator = value;
        //    }
        //}


        private bool _AddColor = false;
        /// <summary>
        /// Get or sets the add color
        /// </summary>
        public bool AddColor
        {
            get
            {
                return this._AddColor;
            }
            set
            {
                this._AddColor = value;
            }
        }

        private Themes _Themes;
        /// <summary>
        /// Get or sets the indicator theme
        /// </summary>
        public Themes Themes
        {
            get
            {
                if (this._Themes == null)
                {
                    this._Themes = new Themes();
                }
                return this._Themes;
            }
            set
            {
                this._Themes = value;
            }
        }

        private Themes _FrequencyThemes;
        /// <summary>
        /// Get or sets the frequency indicator theme
        /// </summary>
        public Themes FrequencyThemes
        {
            get
            {
                if (this._FrequencyThemes == null)
                {
                    this._FrequencyThemes = new Themes();
                }
                return this._FrequencyThemes;
            }
            set
            {
                this._FrequencyThemes = value;
            }
        }

        private DataTable _DistinctIndicator;
        /// <summary>
        /// Get the Distinct indicator list
        /// <para> This should be used to fill the combo with unique indicator in case of apply color </para>
        /// </summary>        
        public DataTable DistinctIndicator
        {
            get
            {
                return this._DistinctIndicator;
            }
            set
            {
                this._DistinctIndicator = value;
            }
        }

        private DataTable _DistinctIUS;
        /// <summary>
        /// Get the Distinct IUS list
        /// <para> This should be used to fill the combo with unique IUS in case of Showing statistics</para>
        /// </summary>        
        public DataTable DistinctIUS
        {
            get
            {
                return this._DistinctIUS;
            }
            set
            {
                this._DistinctIUS = value;
            }
        }

        private DataTable _AggregatesFields;
        /// <summary>
        /// Gets or sets the Aggregate fields.
        /// </summary>
        public DataTable AggregatesFields
        {
            get
            {
                return this._AggregatesFields;
            }
            set
            {
                this._AggregatesFields = value;
            }
        }

        #endregion	.

        #region " -- STEP 3 -- "

        private int _IndicatorClassificationLevel = 1;
        /// <summary>
        /// Gets or sets the Classification level
        /// </summary>
        public int IndicatorClassificationLevel
        {
            get
            {
                return this._IndicatorClassificationLevel;
            }
            set
            {
                this._IndicatorClassificationLevel = value;
                if (value == -1)
                {
                    this.SelectedICLevel = 10;
                }
                else
                {
                    this.SelectedICLevel = value;
                }
            }
        }

        private bool _SelectedIndicatorClassification = false;
        /// <summary>
        /// Gets or sets the selected indicator classification(Step 3).
        /// </summary>
        public bool SelectedIndicatorClassification
        {
            get
            {
                return this._SelectedIndicatorClassification;
            }
            set
            {
                this._SelectedIndicatorClassification = value;
            }
        }

        private ICType _IndicatorClassification;
        /// <summary>
        /// Gets or sets the Indicator classification.
        /// </summary>
        public ICType IndicatorClassification
        {
            get
            {
                return this._IndicatorClassification;
            }
            set
            {
                this._IndicatorClassification = value;
            }
        }

        private DataTable _ClassificationValuesTable;
        /// <summary>
        /// Gets or sets the Classification values.		
        /// </summary>
        /// <remarks> DataTable Structure
        /// IC_NId | IC_GId | IC_Name | Selected | Order
        /// </remarks>
        public DataTable ClassificationValuesTable
        {
            get
            {
                return this._ClassificationValuesTable;
            }
            set
            {
                this._ClassificationValuesTable = value;
            }
        }

        private bool _SelectedColumnArrangement = true;
        /// <summary>
        /// Gets or sets the column arrangement.
        /// </summary>
        public bool SelectedColumnArrangement
        {
            get
            {
                return this._SelectedColumnArrangement;
            }
            set
            {
                this._SelectedColumnArrangement = value;
            }
        }

        private DataTable _ColumnArrangementTable;
        /// <summary>
        /// Get or Set column arrangement.
        /// </summary>
        ///<remarks>
        /// Contains the columnarrangent table created in step 3 for column arrangement
        ///</remarks>        
        public DataTable ColumnArrangementTable
        {
            get
            {
                return this._ColumnArrangementTable;
            }
            set
            {
                this._ColumnArrangementTable = value;
            }
        }

        private List<SelectedNodeInfo> _SelectedNodes = new List<SelectedNodeInfo>();
        /// <summary>
        /// Gets or sets the selected node info.
        /// </summary>
        public List<SelectedNodeInfo> SelectedNodes
        {
            get
            {
                return this._SelectedNodes;
            }
            set
            {
                this._SelectedNodes = value;
                if (this._SelectedNodes.Count > 0)
                {
                    this.CopyICSelectedNodes();
                }
            }
        }


        #endregion

        #region " -- Step 5 -- "

        private StyleTemplate _TemplateStyle;
        /// <summary>
        /// Get or sets the Style template
        /// </summary>
        public StyleTemplate TemplateStyle
        {
            get
            {
                return this._TemplateStyle;
            }
            set
            {
                this._TemplateStyle = value;
            }
        }

        private string _TemplateFileName = "None";
        /// <summary>
        /// Gets or sets the template file name.
        /// </summary>
        public string TemplateFileName
        {
            get
            {
                return this._TemplateFileName;
            }
            set
            {
                this._TemplateFileName = value;
            }
        }

        #endregion

        #region " -- STEP 6 -- "

        private bool _ShowSource = true;
        /// <summary>
        /// Gets or Sets the Source. False will not show the source
        /// </summary>				
        public bool ShowSource
        {
            get { return _ShowSource; }
            set { _ShowSource = value; }
        }

        private bool _MoveSourceToLast = true;
        /// <summary>
        /// Gets or sets the move Source. True will move source to last
        /// </summary>
        public bool MoveSourceToLast
        {
            get { return _MoveSourceToLast; }
            set { _MoveSourceToLast = value; }
        }

        private bool _HideTitleSubtitleRows = false;
        /// <summary>
        /// Get/Set the HideTitleSubtitleRows
        /// </summary>
        public bool HideTitleSubtitleRows
        {
            get { return _HideTitleSubtitleRows; }
            set { _HideTitleSubtitleRows = value; }
        }

        private bool _IsWizardMode = true;
        /// <summary>
        /// Gets or sets the IsNonWizardMode
        /// </summary>		
        public bool IsWizardMode
        {
            get { return _IsWizardMode; }
            set { _IsWizardMode = value; }
        }

        private bool _IsAutoFit = false;
        /// <summary>					
        /// Gets or sets the IsAuto fit
        /// </summary>				   						
        public bool IsAutoFit
        {
            get { return _IsAutoFit; }
            set { _IsAutoFit = value; }
        }

        private int _XLSSheetIndex = 0;
        /// <summary>
        /// Used to set the XLS sheet index . ByDefault it is set to Zero.
        /// </summary>
        public int XLSSheetIndex
        {
            get { return _XLSSheetIndex; }
            set { _XLSSheetIndex = value; }
        }

        private bool _ShowExcel = true;
        /// <summary>
        /// Gets or sets the show excel
        /// </summary>
        public bool ShowExcel
        {
            get
            {
                return this._ShowExcel;
            }
            set
            {
                this._ShowExcel = value;
            }
        }




        #endregion

        #region " -- Graph --"

        private int _ChartType = 51; //TODO set default value to Column Chart
        /// <summary>
        /// Chart Type
        /// </summary>
        /// <remarks>Valid only for Graph wizard</remarks>
        public int ChartType
        {
            get
            {
                return this._ChartType;
            }
            set
            {
                this._ChartType = value;
            }
        }

        private int _DataTableRowCount;
        /// <summary>
        /// Total no. of Table rows for data.
        /// </summary>
        /// <remarks>
        ///  Used by InsertChart to set the data source range
        /// </remarks>
        public int DataTableRowCount
        {
            get
            {
                return _DataTableRowCount;
            }
            set
            {
                _DataTableRowCount = value;
            }
        }

        private int _DataTableColumnCount = 0;
        /// <summary>
        /// Gets or sets the data table column count
        /// </summary>
        public int DataTableColumnCount
        {
            get
            {
                return this._DataTableColumnCount;
            }
            set
            {
                this._DataTableColumnCount = value;
            }
        }

        private DataTable _TableXLS = new DataTable();
        /// <summary>
        /// DataTable that holds the actual XLS output
        /// </summary>
        public DataTable TableXLS
        {
            get
            {
                return this._TableXLS;
            }
        }

        private string _GraphImagePath;
        /// <summary>
        /// Property used to store the graph image name with path to show in the MHT only in case for graph
        /// </summary>
        public string GraphImagePath
        {
            get { return _GraphImagePath; }
            set { _GraphImagePath = value; }
        }

        private List<string> _XAxisFilter = new List<string>();
        /// <summary>
        /// Gets or sets the XAxis filter
        /// </summary>
        /// <remarks>NId column must be stored in the Oth Index and the remaining 2 index will store the checked NIds</remarks>
        public List<string> XAxisFilter
        {
            get
            {
                return this._XAxisFilter;
            }
            set
            {
                this._XAxisFilter = value;
            }
        }

        private string _SelectedIUNId = string.Empty;
        /// <summary>
        /// Gets or sets the SelectedIUNId
        /// </summary>
        public string SelectedIUNId
        {
            get
            {
                return this._SelectedIUNId;
            }
            set
            {
                this._SelectedIUNId = value;
            }
        }


        #endregion

        #region " -- Print -- "

        private string _PageSize = "Letter";
        /// <summary>
        /// Gets or sets the selected page size.
        /// </summary>
        public string PageSize
        {
            get
            {
                return this._PageSize;
            }
            set
            {
                this._PageSize = value;
            }
        }

        private Orientation _Orientation = Orientation.Horizontal;
        /// <summary>
        /// Gets or sets the orientation of document.
        /// </summary>
        public Orientation Orientation
        {
            get
            {
                return this._Orientation;
            }
            set
            {
                this._Orientation = value;
            }
        }

        private bool _FitToPage = false;
        /// <summary>
        /// Gets or sets the fit to page.
        /// </summary>
        public bool FitToPage
        {
            get
            {
                return this._FitToPage;
            }
            set
            {
                this._FitToPage = value;
            }
        }

        private int _CustomWidth;
        /// <summary>
        /// Gets or sets the custom width.
        /// </summary>
        public int CustomWidth
        {
            get
            {
                return this._CustomWidth;
            }
            set
            {
                this._CustomWidth = value;
            }
        }

        private int _CustomHeight;
        /// <summary>
        /// Gets or sets the custom Height.
        /// </summary>
        public int CustomHeight
        {
            get
            {
                return this._CustomHeight;
            }
            set
            {
                this._CustomHeight = value;
            }
        }



        #endregion

        #region "TablePresentation For Web Application"

        private bool _IsTPWizardMode = false;
        /// <summary>
        /// Gets or sets the IsWizardMode.Set True is called for Wizard
        /// </summary>
        public bool IsTPWizardMode
        {
            get { return _IsTPWizardMode; }
            set { _IsTPWizardMode = value; }
        }

        private string _TableHTM = string.Empty;
        /// <summary>
        /// Gets or sets the TableHTM which will contain the html of table presentation
        /// </summary>									
        public string TableHTM
        {
            get { return _TableHTM; }
            set { _TableHTM = value; }
        }

        private string _TableFrequencyHTM = string.Empty;
        /// <summary>
        /// Gets or sets the Frequency TableHTM which will contain the html of frequency table presentation
        /// </summary>									
        public string TableFrequencyHTM
        {
            get { return _TableFrequencyHTM; }
            set { _TableFrequencyHTM = value; }
        }


        private bool _IsLaunchedFromWizard = false;
        /// <summary>
        /// Get or Sets the IsLaunchedFromWizard
        /// </summary>				
        public bool IsLaunchedFromWizard
        {
            get { return _IsLaunchedFromWizard; }
            set { _IsLaunchedFromWizard = value; }
        }


        #endregion

        #endregion

        #region " -- Methods -- "

        #region " -- Common -- "

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MD_IND_Fields">Comma delimited indicator metadata fields. MD_IND_1,MD_IND_2,MD_IND_5 (userPreference.DataView.MetadataIndicatorField)</param>
        /// <param name="MD_AREA_Fields"></param>
        /// <param name="MD_SRC_Fields"></param>
        public void SetMetadataInfo(string MD_IND_Fields, string MD_AREA_Fields, string MD_SRC_Fields)
        {
            this.MDIndicatorFields = MD_IND_Fields;
            this.MDAreaFields = MD_AREA_Fields;
            this.MDSourceFields = MD_SRC_Fields;
            int MDFieldCount = 0;
            if (this.MDIndicatorFields.Length > 0)
            {
                string[] ArrIndicatorFileds = this.MDIndicatorFields.Split(',');
                for (MDFieldCount = 0; MDFieldCount < ArrIndicatorFileds.Length; MDFieldCount++)
                {
                    if (this._Fields.All[ArrIndicatorFileds[MDFieldCount].ToString()] == null)
                    {
                        this.Fields.All.Add(new Field(ArrIndicatorFileds[MDFieldCount].ToString(), this.DIDataView.MetadataIndicator.Columns[ArrIndicatorFileds[MDFieldCount]].Caption));//, FieldType.MetadataIndicator));
                    }

                    if (this._Fields.Available[ArrIndicatorFileds[MDFieldCount].ToString()] == null && this._Fields.Rows[ArrIndicatorFileds[MDFieldCount].ToString()] == null && this._Fields.Columns[ArrIndicatorFileds[MDFieldCount].ToString()] == null)
                    {
                        this.Fields.Available.Add(this.Fields.All[ArrIndicatorFileds[MDFieldCount].ToString()]);
                    }
                }
            }
            if (this.MDAreaFields.Length > 0)
            {
                string[] ArrAreaFileds = this.MDAreaFields.Split(',');
                for (MDFieldCount = 0; MDFieldCount < ArrAreaFileds.Length; MDFieldCount++)
                {
                    if (this._Fields.All[ArrAreaFileds[MDFieldCount].ToString()] == null)
                    {
                        this.Fields.All.Add(new Field(ArrAreaFileds[MDFieldCount].ToString(), this.DIDataView.MetadataArea.Columns[ArrAreaFileds[MDFieldCount]].Caption));//, FieldType.MetadataArea));
                    }

                    if (this._Fields.Available[ArrAreaFileds[MDFieldCount].ToString()] == null && this._Fields.Rows[ArrAreaFileds[MDFieldCount].ToString()] == null && this._Fields.Columns[ArrAreaFileds[MDFieldCount].ToString()] == null)
                    {
                        this.Fields.Available.Add(this.Fields.All[ArrAreaFileds[MDFieldCount].ToString()]);
                    }
                }
            }
            if (this.MDSourceFields.Length > 0)
            {
                string[] ArrSourceFileds = this.MDSourceFields.Split(',');
                for (MDFieldCount = 0; MDFieldCount < ArrSourceFileds.Length; MDFieldCount++)
                {
                    if (this._Fields.All[ArrSourceFileds[MDFieldCount].ToString()] == null)
                    {
                        this.Fields.All.Add(new Field(ArrSourceFileds[MDFieldCount].ToString(), this.DIDataView.MetadataSource.Columns[ArrSourceFileds[MDFieldCount]].Caption));//, FieldType.MetadataSource));
                    }

                    if (this._Fields.Available[ArrSourceFileds[MDFieldCount].ToString()] == null && this._Fields.Rows[ArrSourceFileds[MDFieldCount].ToString()] == null && this._Fields.Columns[ArrSourceFileds[MDFieldCount].ToString()] == null)
                    {
                        this.Fields.Available.Add(this.Fields.All[ArrSourceFileds[MDFieldCount].ToString()]);
                    }
                }
            }
        }

        /// <summary>
        /// Comma delimited ICInfo fields. CLS_SC_1,CLS_GL_1,CLS_GL_2 (userPreference.DataView.MetadataIndicatorField)
        /// </summary>
        /// <param name="ICInfoFields"></param>
        public void SetICInfo(string ICInfoFields)
        {
            this.ICInfoFields = ICInfoFields;
            string[] ArrICFileds = this.ICInfoFields.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int FieldCount = 0; FieldCount < ArrICFileds.Length; FieldCount++)
            {
                if (this.DIDataView.IUSICInfo.Columns[ArrICFileds[FieldCount]].ColumnName.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.ICSector)) //ICSector
                {
                    if (this._Fields.All[ArrICFileds[FieldCount]] == null)
                    {
                        this.Fields.All.Add(new Field(ArrICFileds[FieldCount], this.DIDataView.IUSICInfo.Columns[ArrICFileds[FieldCount]].Caption));//, FieldType.ICSector));
                    }
                }
                else if (this.DIDataView.IUSICInfo.Columns[ArrICFileds[FieldCount]].ColumnName.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.ICSource)) //ICSource
                {
                    if (this._Fields.All[ArrICFileds[FieldCount]] == null)
                    {
                        this.Fields.All.Add(new Field(ArrICFileds[FieldCount], this.DIDataView.IUSICInfo.Columns[ArrICFileds[FieldCount]].Caption));//, FieldType.ICSource));
                    }
                }
                else if (this.DIDataView.IUSICInfo.Columns[ArrICFileds[FieldCount]].ColumnName.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.ICGoal)) //ICGoal
                {
                    if (this._Fields.All[ArrICFileds[FieldCount]] == null)
                    {
                        this.Fields.All.Add(new Field(ArrICFileds[FieldCount], this.DIDataView.IUSICInfo.Columns[ArrICFileds[FieldCount]].Caption));//, FieldType.ICGoal));
                    }
                }
                else if (this.DIDataView.IUSICInfo.Columns[ArrICFileds[FieldCount]].ColumnName.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.ICTheme)) //ICTheme
                {
                    if (this._Fields.All[ArrICFileds[FieldCount]] == null)
                    {
                        this.Fields.All.Add(new Field(ArrICFileds[FieldCount], this.DIDataView.IUSICInfo.Columns[ArrICFileds[FieldCount]].Caption));//, FieldType.ICTheame));
                    }
                }
                else if (this.DIDataView.IUSICInfo.Columns[ArrICFileds[FieldCount]].ColumnName.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.ICInstitution)) //ICInstitution
                {
                    if (this._Fields.All[ArrICFileds[FieldCount]] == null)
                    {
                        this.Fields.All.Add(new Field(ArrICFileds[FieldCount], this.DIDataView.IUSICInfo.Columns[ArrICFileds[FieldCount]].Caption));//, FieldType.ICInstitute));
                    }
                }
                else if (this.DIDataView.IUSICInfo.Columns[ArrICFileds[FieldCount]].ColumnName.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.ICConvention)) //ICConvention
                {
                    if (this._Fields.All[ArrICFileds[FieldCount]] == null)
                    {
                        this.Fields.All.Add(new Field(ArrICFileds[FieldCount], this.DIDataView.IUSICInfo.Columns[ArrICFileds[FieldCount]].Caption));//, FieldType.ICConvention));
                    }
                }

                if (this._Fields.Available[ArrICFileds[FieldCount]] == null && this._Fields.Rows[ArrICFileds[FieldCount]] == null && this._Fields.Columns[ArrICFileds[FieldCount]] == null)
                {
                    this.Fields.Available.Add(this.Fields.All[ArrICFileds[FieldCount]]);
                }
            }
        }

        #endregion

        #region "-- Pyramid --"

        /// <summary>
        /// Initialize the settings for the pyramid chart
        /// </summary>
        public void InitalizeSettingsForPyramid()
        {
            DataTable dtIndicators = this.GetIU(true);
            this.FilterPresentationDataForPyramid(this.ApplyFilterToDataView(true, false), false);
            this.GenFieldCollection(true);
            DataTable dtXAxisValue = this.GetXAxisValues(true);
            this.FilterPresentationDataForPyramid(this.ApplyFilterToDataView(true, true), false);
        }

        /// <summary>
        /// Get the distinct indicator and unit name and NIds
        /// </summary>
        /// <returns></returns>
        public DataTable GetIU(bool resetIndicatorSelection)
        {
            DataTable RetVal = new DataTable();
            try
            {
                string[] Indicators = new string[4];

                Indicators[0] = Indicator.IndicatorNId;
                Indicators[1] = Indicator.IndicatorName;
                Indicators[2] = Unit.UnitNId;
                Indicators[3] = Unit.UnitName;

                this.PresentationData.RowFilter = string.Empty;
                RetVal = this.PresentationData.ToTable(true, Indicators);

                if (resetIndicatorSelection)
                {
                    //-- Set the first as selected IU NId
                    this._SelectedIUNId = RetVal.Rows[0][Indicator.IndicatorNId].ToString() + "," + RetVal.Rows[0][Unit.UnitNId].ToString();
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        /// Get the distinct X axis value data table
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public DataTable GetXAxisValues(bool resetXAxisValue)
        {
            DataTable RetVal = new DataTable();
            try
            {
                if (this._Fields.Columns.Count > 0)
                {
                    this.FilterPresentationDataForPyramid(this.ApplyFilterToDataView(true, false), false);

                    string ColumnName = this.GetNIdstring();
                    string[] Param = new string[2];
                    DataView AreaView;
                    Param[0] = this._Fields.Columns[0].FieldID;
                    Param[1] = ColumnName;

                    RetVal = this.PresentationData.ToTable(true, Param);

                    foreach (DataRow Row in RetVal.Rows)
                    {
                        if (string.IsNullOrEmpty(Row[this._Fields.Columns[0].FieldID].ToString()))
                        {
                            Row.Delete();
                            break;
                        }
                    }

                    AreaView = RetVal.DefaultView;
                    AreaView.Sort = this._Fields.Columns[0].FieldID;
                    RetVal = AreaView.ToTable();

                    if (resetXAxisValue)
                    {
                        //-- Set the default X axis values
                        this._XAxisFilter.Clear();
                        this._XAxisFilter.Add(ColumnName);
                        this._XAxisFilter.Add(RetVal.Rows[0][ColumnName].ToString());
                        if (RetVal.Rows.Count > 1)
                        {
                            this._XAxisFilter.Add(RetVal.Rows[1][ColumnName].ToString());
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        /// Get the NID column of the X axis
        /// </summary>
        /// <returns></returns>
        private string GetNIdstring()
        {
            string RetVal = string.Empty;
            try
            {
                DataRow[] Rows = new DataRow[0];
                switch (this._Fields.Columns[0].FieldID)
                {
                    case Area.AreaName:
                        RetVal = Area.AreaNId;
                        break;
                    case Timeperiods.TimePeriod:
                        RetVal = Timeperiods.TimePeriodNId;
                        break;
                    default:
                        Rows = this.DIDataView.SubgroupInfo.Select(SubgroupInfoColumns.Name + " = '" + this._Fields.Columns[0].FieldID + "'");
                        if (Rows.Length > 0)
                        {
                            RetVal = Rows[0][SubgroupInfoColumns.NId].ToString();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
            }
            return RetVal;
        }

        public string ApplyFilterToDataView(bool Indicatorfilter, bool XaxisFilter)
        {
            string RetVal = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(this._SelectedIUNId) || this._XAxisFilter.Count > 0)
                {
                    string[] IUNId = new string[0];
                    int Index = 0;
                    string XAxisNid = string.Empty;

                    if (Indicatorfilter)
                    {
                        //'-- Filter for Indicator and Unit NId
                        IUNId = DICommon.SplitString(this._SelectedIUNId, ",");
                        RetVal = Indicator.IndicatorNId + " = " + IUNId[0] + " AND " + Unit.UnitNId + " = " + IUNId[1];
                    }

                    if (XaxisFilter)
                    {
                        //-- X axis value filter
                        foreach (string XAxis in this._XAxisFilter)
                        {
                            if (Index > 0)
                            {
                                if (!string.IsNullOrEmpty(XAxisNid))
                                {
                                    XAxisNid += ",";
                                }
                                XAxisNid += XAxis;
                            }
                            Index += 1;
                        }
                    }


                    if (!string.IsNullOrEmpty(XAxisNid))
                    {
                        if (!string.IsNullOrEmpty(RetVal))
                        {
                            RetVal += " AND ";
                        }
                        RetVal += this._XAxisFilter[0] + " IN (" + XAxisNid + ")";
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return RetVal;
        }

        /// <summary>
        /// Sort the column arrangement.
        /// </summary>
        public void SortColumnArrangementTable()
        {
            if (this._ChartType == 58)
            {
                if (this._Fields.Columns.Count > 1)
                {
                    int Index = 0;
                    DataView DvColumnArrangement = this._ColumnArrangementTable.DefaultView;
                    DvColumnArrangement.Sort = this._Fields.Columns[1].FieldID;
                    this._ColumnArrangementTable = DvColumnArrangement.ToTable();

                    foreach (DataRow Row in this._ColumnArrangementTable.Rows)
                    {
                        Row[RecordId] = Index;
                        Row[Order] = Index;
                        Index += 1;
                    }
                }
            }
        }

        /// <summary>
        /// Filter down the dataview based on the filter string passed by the hosting application
        /// </summary>
        /// <param name="FilterString">Filter String</param>
        public void FilterPresentationDataForPyramid(string FilterString, bool regenrateFields)
        {
            try
            {
                this.PresentationData.RowFilter = string.Empty;
                this.PresentationData.RowFilter = FilterString;

                if (this._Fields.Columns[Timeperiods.TimePeriod] == null && this._Fields.Rows[Timeperiods.TimePeriod] == null)
                {
                    this.ApplyMRD();
                }

                // Wizard STEP 2: Set the Title and Subtitle of table.
                this.SetTitleAndSubtitle();

                if (regenrateFields)
                {
                    this.GenFieldCollection(true);
                }

                // Wizard STEP 3: Column Arrangement.
                this._ColumnArrangementTable = this.SetColumnArrangement(true);

                //-- Sort the column arrangement table
                this.SortColumnArrangementTable();

            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Apply the MRD on the presentation data
        /// </summary>
        public void ApplyMRD()
        {
            bool TimePeriodExists = false;

            foreach (Field field in this._Fields.Rows)
            {
                if (field.FieldID == Timeperiods.TimePeriod)
                {
                    TimePeriodExists = true;
                    break;
                }
            }

            if (!TimePeriodExists)
            {
                foreach (Field field in this._Fields.Columns)
                {
                    if (field.FieldID == Timeperiods.TimePeriod)
                    {
                        TimePeriodExists = true;
                        break;
                    }
                }
            }
            if (!TimePeriodExists)
            {
                this.ApplyMRDFilter();
            }
        }

        private DataTable UpdateSubgroupDimensions()
        {
            DataTable RetVal = new DataTable();
            try
            {
                if (RetVal.Columns.Count == 0)
                {
                    RetVal.Columns.Add(SubgroupInfoColumns.NId, typeof(string));
                    RetVal.Columns.Add(SubgroupInfoColumns.Name, typeof(string));
                    RetVal.Columns.Add(SubgroupInfoColumns.GId, typeof(string));
                    RetVal.Columns.Add(SubgroupInfoColumns.Global, typeof(string));
                    RetVal.Columns.Add(SubgroupInfoColumns.Type, typeof(int));
                    RetVal.Columns.Add(SubgroupInfoColumns.Order, typeof(int));
                    RetVal.Columns.Add(SubgroupInfoColumns.Caption, typeof(string));
                    RetVal.Columns.Add(SubgroupInfoColumns.GIdValue, typeof(string));
                }


                // Add rows to SubgroupInfo Table 
                string SubgroupValNIds = string.Empty;
                StringBuilder sbSubgroupValNIds = new StringBuilder();
                foreach (DataRow dr in this.PresentationData.ToTable().Rows)
                {
                    sbSubgroupValNIds.Append("," + dr[SubgroupVals.SubgroupValNId].ToString());
                }

                if (sbSubgroupValNIds.Length > 0)
                {
                    string sSql = this.DIQueries.SubgroupTypes.GetSubgroupTypes(sbSubgroupValNIds.ToString().Substring(1));
                    DataTable dtSubgroupTypes = this.DIConnection.ExecuteDataTable(sSql);
                    string ValidColumnName = string.Empty;
                    foreach (DataRow dr in dtSubgroupTypes.Rows)
                    {
                        DataRow drSubgroupInfo = RetVal.NewRow();
                        //-- remove any spaces apostrophe or comma in field name 
                        ValidColumnName = GetValidColumnName(this.GetValidColumnName(dr[SubgroupTypes.SubgroupTypeName].ToString()));
                        drSubgroupInfo[SubgroupInfoColumns.NId] = "Subgroup_" + ValidColumnName + "_NId";
                        drSubgroupInfo[SubgroupInfoColumns.Name] = "Subgroup_" + ValidColumnName + "_Name";
                        drSubgroupInfo[SubgroupInfoColumns.GId] = "Subgroup_" + ValidColumnName + "_GId";
                        drSubgroupInfo[SubgroupInfoColumns.Global] = "Subgroup_" + ValidColumnName + "_Global";
                        drSubgroupInfo[SubgroupInfoColumns.Type] = dr[SubgroupTypes.SubgroupTypeNId];
                        drSubgroupInfo[SubgroupInfoColumns.Order] = dr[SubgroupTypes.SubgroupTypeOrder];
                        drSubgroupInfo[SubgroupInfoColumns.Caption] = dr[SubgroupTypes.SubgroupTypeName];
                        drSubgroupInfo[SubgroupInfoColumns.GIdValue] = dr[SubgroupTypes.SubgroupTypeGID];

                        RetVal.Rows.Add(drSubgroupInfo);
                    }
                }
            }
            catch (Exception)
            {
            }
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

        #region "-- STEP 6 --"

        /// <summary>
        /// generates the .XLS file to be shown as output on step 6.
        /// </summary>
        /// <param name="PresentationOutputFolder">FilePath for file to be saved.</param>
        /// <returns></returns>
        public string GeneratePresentation(string PresentationOutputFolder)
        {
            return GeneratePresentation(PresentationOutputFolder, string.Empty);
        }

        /// <summary>
        /// generates the data table for graph to be used in graph presentation as an input.
        /// </summary>
        /// <returns></returns>
        public DataTable GeneratePresentation()
        {
            DataTable Retval = new DataTable();
            try
            {
                this.GeneratePresentation(string.Empty, string.Empty);
                Retval = this._TableXLS;
            }
            catch (Exception ex)
            {
                Retval = this._TableXLS;
            }
            return Retval;
        }

        private void AddAreaInPresentationData(ArrayList ALANId, DataTable DTPresentationDataClone, DataRow DRVPresentationData, DataTable DTParentChildRelation, int MinAreaLevel)
        {
            string ParentChildCID = string.Empty;
            bool ParentExists = false;
            bool ChangedRootLevel = false;
            int TotalAreaOccurence = 1;
            int CurrentParentNId = -1;
            int CurrentAreaLevel = 1;
            DataRow NewRowPresentationData = DTPresentationDataClone.NewRow();
            string StrGetArea = DIQueries.Area.GetArea(FilterFieldType.NId, DRVPresentationData[Area.AreaParentNId].ToString());
            DataTable DTAreaTmp = DIConnection.ExecuteDataTable(StrGetArea);
            NewRowPresentationData[Area.AreaNId] = DTAreaTmp.Rows[0][Area.AreaNId];
            NewRowPresentationData[Area.AreaParentNId] = DTAreaTmp.Rows[0][Area.AreaParentNId];
            NewRowPresentationData[Area.AreaLevel] = DTAreaTmp.Rows[0][Area.AreaLevel];
            NewRowPresentationData[Area.AreaName] = DTAreaTmp.Rows[0][Area.AreaName];
            NewRowPresentationData[Area.AreaID] = DTAreaTmp.Rows[0][Area.AreaID];
            NewRowPresentationData[IndicatorClassifications.ICNId] = "-1";

            ALANId.Add(DRVPresentationData[Area.AreaParentNId].ToString());
            foreach (DataRow Drow in DTParentChildRelation.Select("ANID='" + NewRowPresentationData[Area.AreaParentNId] + "'"))
            {
                ParentChildCID = Drow["PAR"].ToString();
                break;
            }
            if (ParentChildCID == string.Empty && DTParentChildRelation.Rows.Count > 0)
            {
                foreach (DataRow Drow in DTParentChildRelation.Select("PNID='" + NewRowPresentationData[Area.AreaParentNId] + "'"))
                {
                    ParentExists = true;
                    ParentChildCID = Drow["PAR"].ToString();
                    break;
                }
                if (ParentExists == false)
                {
                    if (Convert.ToInt32(NewRowPresentationData[Area.AreaLevel]) == MinAreaLevel)
                    {
                        this.RootParentChildID = this.RootParentChildID.Substring(0, this.RootParentChildID.Length - 1) + this.RootParentChildIDCount.ToString();
                        ParentChildCID = this.RootParentChildID;
                        this.RootParentChildIDCount++;
                        ChangedRootLevel = true;
                    }
                    else
                    {
                        this.AddAreaInPresentationData(ALANId, DTPresentationDataClone, NewRowPresentationData, DTParentChildRelation, MinAreaLevel);
                        foreach (DataRow Drow in DTParentChildRelation.Select("ANID='" + NewRowPresentationData[Area.AreaParentNId] + "'"))
                        {
                            ParentChildCID = Drow["PAR"].ToString();
                            break;
                        }
                    }
                }
            }
            else if (ParentChildCID == string.Empty && DTParentChildRelation.Rows.Count == 0 && Convert.ToInt32(NewRowPresentationData[Area.AreaLevel]) > MinAreaLevel)
            {
                this.AddAreaInPresentationData(ALANId, DTPresentationDataClone, NewRowPresentationData, DTParentChildRelation, MinAreaLevel);
                foreach (DataRow Drow in DTParentChildRelation.Select("ANID='" + NewRowPresentationData[Area.AreaParentNId] + "'"))
                {
                    ParentChildCID = Drow["PAR"].ToString();
                    break;
                }
            }

            foreach (DataRow Drow in DTParentChildRelation.Select("PNID='" + NewRowPresentationData[Area.AreaParentNId] + "'"))
            {
                TotalAreaOccurence++;
            }
            DataRow NewRow = DTParentChildRelation.NewRow();
            if (ParentChildCID == string.Empty && DTParentChildRelation.Rows.Count == 0)
            {
                ParentChildCID = "0001";
                RootParentChildID = "0001";
                RootParentChildIDCount = 2;
                NewRow[2] = ParentChildCID;
                CurrentAreaLevel = Convert.ToInt32(NewRowPresentationData[Area.AreaLevel]);
                NewRowPresentationData["AL" + CurrentAreaLevel] = NewRowPresentationData[Area.AreaName];
                if (CurrentAreaLevel > MinAreaLevel)
                {
                    CurrentParentNId = Convert.ToInt32(NewRowPresentationData[Area.AreaParentNId]);
                    for (int i = CurrentAreaLevel - 1; i >= MinAreaLevel; i--)
                    {
                        foreach (DataRow DROW in DTParentChildRelation.Select("ANID='" + CurrentParentNId.ToString() + "'"))
                        {
                            NewRowPresentationData["AL" + i] = DROW["AName"];
                            CurrentParentNId = Convert.ToInt32(DROW["PNID"]);
                            break;
                        }
                    }
                }
                NewRowPresentationData["ParentChildID"] = ParentChildCID;
            }
            else if (ParentChildCID != string.Empty)
            {
                if (ChangedRootLevel)
                {
                    NewRowPresentationData["ParentChildID"] = ParentChildCID;
                    NewRow[2] = ParentChildCID;
                    CurrentAreaLevel = Convert.ToInt32(NewRowPresentationData[Area.AreaLevel]);
                    NewRowPresentationData["AL" + CurrentAreaLevel] = NewRowPresentationData[Area.AreaName];
                    if (CurrentAreaLevel > MinAreaLevel)
                    {
                        CurrentParentNId = Convert.ToInt32(NewRowPresentationData[Area.AreaParentNId]);
                        for (int i = CurrentAreaLevel - 1; i >= MinAreaLevel; i--)
                        {
                            foreach (DataRow DROW in DTParentChildRelation.Select("ANID='" + CurrentParentNId.ToString() + "'"))
                            {
                                NewRowPresentationData["AL" + i] = DROW["AName"];
                                CurrentParentNId = Convert.ToInt32(DROW["PNID"]);
                                break;
                            }
                        }
                    }
                }
                else if (ParentExists)
                {
                    if (TotalAreaOccurence > 9)
                    {
                        NewRowPresentationData["ParentChildID"] = ParentChildCID.Substring(0, ParentChildCID.Length - 2) + TotalAreaOccurence.ToString();
                        NewRow[2] = ParentChildCID.Substring(0, ParentChildCID.Length - 2) + TotalAreaOccurence.ToString();
                        CurrentAreaLevel = Convert.ToInt32(NewRowPresentationData[Area.AreaLevel]);
                        NewRowPresentationData["AL" + CurrentAreaLevel] = NewRowPresentationData[Area.AreaName];
                        if (CurrentAreaLevel > MinAreaLevel)
                        {
                            CurrentParentNId = Convert.ToInt32(NewRowPresentationData[Area.AreaParentNId]);
                            for (int i = CurrentAreaLevel - 1; i >= MinAreaLevel; i--)
                            {
                                foreach (DataRow DROW in DTParentChildRelation.Select("ANID='" + CurrentParentNId.ToString() + "'"))
                                {
                                    NewRowPresentationData["AL" + i] = DROW["AName"];
                                    CurrentParentNId = Convert.ToInt32(DROW["PNID"]);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        NewRowPresentationData["ParentChildID"] = ParentChildCID.Substring(0, ParentChildCID.Length - 1) + TotalAreaOccurence.ToString();
                        NewRow[2] = ParentChildCID.Substring(0, ParentChildCID.Length - 1) + TotalAreaOccurence.ToString();
                        CurrentAreaLevel = Convert.ToInt32(NewRowPresentationData[Area.AreaLevel]);
                        NewRowPresentationData["AL" + CurrentAreaLevel] = NewRowPresentationData[Area.AreaName];
                        if (CurrentAreaLevel > MinAreaLevel)
                        {
                            CurrentParentNId = Convert.ToInt32(NewRowPresentationData[Area.AreaParentNId]);
                            for (int i = CurrentAreaLevel - 1; i >= MinAreaLevel; i--)
                            {
                                foreach (DataRow DROW in DTParentChildRelation.Select("ANID='" + CurrentParentNId.ToString() + "'"))
                                {
                                    NewRowPresentationData["AL" + i] = DROW["AName"];
                                    CurrentParentNId = Convert.ToInt32(DROW["PNID"]);
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (TotalAreaOccurence > 9)
                    {
                        NewRow[2] = ParentChildCID + "00" + TotalAreaOccurence.ToString();
                        NewRowPresentationData["ParentChildID"] = ParentChildCID + "00" + TotalAreaOccurence.ToString();
                        CurrentAreaLevel = Convert.ToInt32(NewRowPresentationData[Area.AreaLevel]);
                        NewRowPresentationData["AL" + CurrentAreaLevel] = NewRowPresentationData[Area.AreaName];
                        if (CurrentAreaLevel > MinAreaLevel)
                        {
                            CurrentParentNId = Convert.ToInt32(NewRowPresentationData[Area.AreaParentNId]);
                            for (int i = CurrentAreaLevel - 1; i >= MinAreaLevel; i--)
                            {
                                foreach (DataRow DROW in DTParentChildRelation.Select("ANID='" + CurrentParentNId.ToString() + "'"))
                                {
                                    NewRowPresentationData["AL" + i] = DROW["AName"];
                                    CurrentParentNId = Convert.ToInt32(DROW["PNID"]);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        NewRow[2] = ParentChildCID + "000" + TotalAreaOccurence.ToString();
                        NewRowPresentationData["ParentChildID"] = ParentChildCID + "000" + TotalAreaOccurence.ToString();
                        CurrentAreaLevel = Convert.ToInt32(NewRowPresentationData[Area.AreaLevel]);
                        NewRowPresentationData["AL" + CurrentAreaLevel] = NewRowPresentationData[Area.AreaName];
                        if (CurrentAreaLevel > MinAreaLevel)
                        {
                            CurrentParentNId = Convert.ToInt32(NewRowPresentationData[Area.AreaParentNId]);
                            for (int i = CurrentAreaLevel - 1; i >= MinAreaLevel; i--)
                            {
                                foreach (DataRow DROW in DTParentChildRelation.Select("ANID='" + CurrentParentNId.ToString() + "'"))
                                {
                                    NewRowPresentationData["AL" + i] = DROW["AName"];
                                    CurrentParentNId = Convert.ToInt32(DROW["PNID"]);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            NewRow[0] = NewRowPresentationData[Area.AreaParentNId].ToString();
            NewRow[1] = NewRowPresentationData[Area.AreaNId].ToString();
            NewRow["AName"] = NewRowPresentationData[Area.AreaName];
            DTParentChildRelation.Rows.Add(NewRow);
            try
            {
                DTPresentationDataClone.Rows.Add(NewRowPresentationData);
            }
            catch (Exception ex)
            {

                Console.Write(ex.Message);
            }
            ParentExists = false;
        }

        private void AddLevelAreas(DataRowView DRVPresentationData, int MinAreaLevel, DataTable DTParentChildRelation)
        {
            int CurrentParentNId = 1;
            int CurrentAreaLevel = Convert.ToInt32(DRVPresentationData[Area.AreaLevel]);
            DRVPresentationData["AL" + CurrentAreaLevel] = DRVPresentationData[Area.AreaName];
            if (CurrentAreaLevel > MinAreaLevel)
            {
                CurrentParentNId = Convert.ToInt32(DRVPresentationData[Area.AreaParentNId]);
                for (int i = CurrentAreaLevel - 1; i >= MinAreaLevel; i--)
                {
                    foreach (DataRow DROW in DTParentChildRelation.Select("ANID='" + CurrentParentNId.ToString() + "'"))
                    {
                        DRVPresentationData["AL" + i] = DROW["AName"];
                        CurrentParentNId = Convert.ToInt32(DROW["PNID"]);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// generates the .XLS file to be shown as output on step 6. 
        /// </summary>
        /// <returns></returns>	
        public string GeneratePresentation(string PresentationOutputFolder, string presentationFileName)
        {

            //
            //
            //  Area	    | Source	    | IMR	            | U5MR      ] -> Header of TableXLS
            //  ----------------------------------------------------------
            //  India	    | UNICEF	    | 12	            | 11
            //  Pakistan	| UNICEF	    | 13	            | 12
            //
            //
            // 1. Above Tbl has 2 Columns for the 2 Row Fields (RF) (Area and Source) 
            // 2. Total Column Fields (CF) = 1 (Indicator)
            // 3. Total Columns using the Distinct Column Field Values (CFV) = 2 (IMR and U5MR)
            // 4. Total Columns (TC) = 2 CFV + 2 RF = 4

            this.RaiseEventProgressBar(10, 0);

            #region " -- Set Footnotes display style property -- "

            if (this._PresentationType == Presentation.PresentationType.Table || this._PresentationType == Presentation.PresentationType.FrequencyTable)
            {
                this.FootnoteInLine = this._TemplateStyle.Footnotes.FontTemplate.TableFootnoteInLine;
            }
            else if (this._PresentationType == Presentation.PresentationType.Graph)
            {
                this.FootnoteInLine = this._TemplateStyle.Footnotes.FontTemplate.GraphFootnoteInLine;
            }

            #endregion

            #region "-- Set variables and DTDataSheet structure --"

            string RetVal = string.Empty;
            this._TableXLS = new DataTable();
            DataTable DTICNames = new DataTable();				// DataTable that holds the IndicatorNid,IC_ParentNId,IC_Nid,IN_Name when IC Arrangement is on.
            DataTable DTFilterICNames = new DataTable();		// DataTable that holds the filtered record from DTICNames based on Level selected.
            DataRow TableXLSRow;								// Each Row in TableXLS
            DataRow RowDTRowsForIcons;

            bool ICAdded = false;								// set it When IC is added in TableXLS when there is no child for the root level IC. 
            ArrayList OrphandIndicators = new ArrayList();		//Array list that contains orphand Indicators.
            bool IsOrphand = true;
            string DataNIds = string.Empty;
            string FilterString = string.Empty;
            DataView DVColumnArrangement;
            int FieldCount = 0;

            //Prepare DataTable structure for Data WorkSheet
            if (this.DTDataSheet != null)
            {
                this.DTDataSheet.Dispose();
            }
            this.DTDataSheet = new DataTable();
            this.DTDataSheet.Columns.Add(Timeperiods.TimePeriod);
            this.DTDataSheet.Columns.Add(Area.AreaID);
            this.DTDataSheet.Columns.Add(Area.AreaName);
            this.DTDataSheet.Columns.Add(Indicator.IndicatorName);
            this.DTDataSheet.Columns.Add(Data.DataValue);
            this.DTDataSheet.Columns.Add(Unit.UnitName);
            this.DTDataSheet.Columns.Add(SubgroupVals.SubgroupVal);
            this.DTDataSheet.Columns.Add(IndicatorClassifications.ICName);
            this.DTDataSheet.Columns.Add(FootNotes.FootNote);

            //Add metadata columns
            if (this.MDIndicatorFields.Length > 0)
            {
                string[] ArrIndicatorFields = this.MDIndicatorFields.Split(',');
                for (FieldCount = 0; FieldCount < ArrIndicatorFields.Length; FieldCount++)
                {
                    this.DTDataSheet.Columns.Add(ArrIndicatorFields[FieldCount].ToString());
                }
            }
            if (this.MDAreaFields.Length > 0)
            {
                string[] ArrAreaFields = this.MDAreaFields.Split(',');
                for (FieldCount = 0; FieldCount < ArrAreaFields.Length; FieldCount++)
                {
                    this.DTDataSheet.Columns.Add(ArrAreaFields[FieldCount].ToString());
                }
            }
            if (this.MDSourceFields.Length > 0)
            {
                string[] ArrSourceFields = this.MDSourceFields.Split(',');
                for (FieldCount = 0; FieldCount < ArrSourceFields.Length; FieldCount++)
                {
                    this.DTDataSheet.Columns.Add(ArrSourceFields[FieldCount].ToString());
                }
            }

            //Add IC columns
            if (this.ICInfoFields.Length > 0)
            {
                string[] ArrICInfoFields = this.ICInfoFields.Split(',');
                for (FieldCount = 0; FieldCount < ArrICInfoFields.Length; FieldCount++)
                {
                    this.DTDataSheet.Columns.Add(ArrICInfoFields[FieldCount].ToString());
                }
            }

            #region "-- Move Area into Rows if it is present in the Available and Set the sorting order of Rows to first

            //Don't use this condition when user has selected single level and single area while saving report from table wizard
            //And User selects single area when generating report from this saved settings.			
            if (!string.IsNullOrEmpty(this._TableReport.AreaSelection.AreaLevels))
            {
                string[] areaNId = DICommon.SplitString(this._UserPreference.UserSelection.AreaNIds, ",");
                if (this._Fields.Available[Area.AreaName] != null && areaNId.Length > 1)
                {
                    this._Fields.Rows.Add(this._Fields.All[Area.AreaName]);
                    this._Fields.Rows.MoveToTop(this._Fields.Rows[Area.AreaName]);
                    if (this._Fields.Sort[Area.AreaName] == null)
                    {
                        this._Fields.Sort.Add(this._Fields.All[Area.AreaName]);
                    }
                    this._Fields.Sort.MoveToTop(this._Fields.Sort[Area.AreaName]);
                    this.RaiseUpdateSortEvent();
                    this.SetTitleAndSubtitle();
                }
                else if (this._Fields.Rows[Area.AreaName] != null)
                {
                    if (this._Fields.Rows[Area.AreaName].FieldIndex == 0)
                    {
                        if (this._Fields.Sort[Area.AreaName] == null)
                        {
                            this._Fields.Sort.Add(this._Fields.All[Area.AreaName]);
                        }
                        this._Fields.Sort.MoveToTop(this._Fields.Sort[Area.AreaName]);
                    }
                }
            }

            #endregion


            //Insert First Row containing Column Captions in the table DTDataSheet
            DataRow RowDTDataSheet = DTDataSheet.NewRow();
            RowDTDataSheet[Timeperiods.TimePeriod] = LanguageStrings.TimePeriod;
            RowDTDataSheet[Area.AreaID] = LanguageStrings.AreaID;
            RowDTDataSheet[Area.AreaName] = LanguageStrings.AreaName;
            RowDTDataSheet[Indicator.IndicatorName] = LanguageStrings.Indicator;
            RowDTDataSheet[Data.DataValue] = LanguageStrings.DataValue;
            RowDTDataSheet[Unit.UnitName] = LanguageStrings.Unit;
            RowDTDataSheet[SubgroupVals.SubgroupVal] = LanguageStrings.Subgroup;
            RowDTDataSheet[IndicatorClassifications.ICName] = LanguageStrings.Source;
            RowDTDataSheet[FootNotes.FootNote] = LanguageStrings.FootNotes;

            if (this.MDIndicatorFields.Length > 0)
            {
                string[] ArrIndicatorFileds = this.MDIndicatorFields.Split(',');
                for (FieldCount = 0; FieldCount < ArrIndicatorFileds.Length; FieldCount++)
                {
                    RowDTDataSheet[ArrIndicatorFileds[FieldCount]] = this.DIDataView.MetadataIndicator.Columns[ArrIndicatorFileds[FieldCount]].Caption;
                }
            }
            if (this.MDAreaFields.Length > 0)
            {
                string[] ArrAreaFileds = this.MDAreaFields.Split(',');
                for (FieldCount = 0; FieldCount < ArrAreaFileds.Length; FieldCount++)
                {
                    RowDTDataSheet[ArrAreaFileds[FieldCount]] = this.DIDataView.MetadataArea.Columns[ArrAreaFileds[FieldCount]].Caption;
                }
            }
            if (this.MDSourceFields.Length > 0)
            {
                string[] ArrSourceFileds = this.MDSourceFields.Split(',');
                for (FieldCount = 0; FieldCount < ArrSourceFileds.Length; FieldCount++)
                {
                    RowDTDataSheet[ArrSourceFileds[FieldCount]] = this.DIDataView.MetadataSource.Columns[ArrSourceFileds[FieldCount]].Caption;
                }
            }

            if (this.ICInfoFields.Length > 0)
            {
                string[] ArrICInfoFields = this.ICInfoFields.Split(',');
                for (FieldCount = 0; FieldCount < ArrICInfoFields.Length; FieldCount++)
                {
                    RowDTDataSheet[ArrICInfoFields[FieldCount]] = this.DIDataView.IUSICInfo.Columns[ArrICInfoFields[FieldCount]].Caption;
                }
            }

            DTDataSheet.Rows.Add(RowDTDataSheet);

            //set the order column of DTFieldNIds as records have to be sorted on this columns.
            if (DTImage != null)
            {

                //set the order column of DTFieldNIds as records have to be sorted on this columns.
                for (int row = 0; row < this.ColumnArrangementTable.Rows.Count; row++)
                {
                    this.DTImage.Rows[row][Order] = this._ColumnArrangementTable.Rows[row][Order];
                }

                //-------------------------------------------
                //OPT : Test for Sorting in DataTable.
                DVColumnArrangement = this._ColumnArrangementTable.DefaultView;
                DVColumnArrangement.Sort = Order;
                this._ColumnArrangementTable = DVColumnArrangement.ToTable();

                DVColumnArrangement = this.DTImage.DefaultView;
                DVColumnArrangement.Sort = Order;
                this.DTImage = DVColumnArrangement.ToTable();

                //--------------------------------------------    
            }
            //---------------------------------------------

            //Set the structure for DataTable DTRowsForIcons
            if (this.DTRowsForIcons != null)
            {
                this.DTRowsForIcons = null;
            }
            this.DTRowsForIcons = new DataTable();
            foreach (Field Field in this._Fields.Rows)
            {
                DTRowsForIcons.Columns.Add(Field.FieldID);
                if (Field.FieldID == Indicator.IndicatorName)
                {
                    DTRowsForIcons.Columns.Add(Indicator.IndicatorNId);
                }
                else if (Field.FieldID == Area.AreaName)
                {
                    DTRowsForIcons.Columns.Add(Area.AreaNId);
                }
                else if (Field.FieldID == Unit.UnitName)
                {
                    DTRowsForIcons.Columns.Add(Unit.UnitNId);
                }
                else if (Field.FieldID == IndicatorClassifications.ICName)
                {
                    DTRowsForIcons.Columns.Add(IndicatorClassifications.ICNId);
                }
                else if (Field.FieldID == SubgroupVals.SubgroupVal)
                {
                    DTRowsForIcons.Columns.Add(SubgroupVals.SubgroupValNId);
                }
                else
                {
                    DTRowsForIcons.Columns.Add(Field.FieldID + "_NId");
                }
            }


            //Total RFs (Row Fields)			
            int TotalRowFields = this._Fields.Rows.Count;

            //Total CFs (Column Fields) including NId fields
            int TotalColumnFields = this._ColumnArrangementTable.Columns.Count - 2;

            //Total CFVs (Column Field Vaues)
            int TotalColFieldVals = this._ColumnArrangementTable.Rows.Count;

            #endregion

            // Wizard STEP 1: Set the table structure of DTAreaParentNId
            if (this.DTAreaParentNId != null)
            {
                DTAreaParentNId.Dispose();
            }
            this.SetDTAreaParentNIdTableStructure();

            this.RaiseEventProgressBar(10, 100);

            #region " -- Loop on PresentationData for setting UniqueSources,UniqueDataNIds,UniqueIndicatorNIds -- "

            //Prepare comma delemited Indicator Nids string which will be used to fire a query on database to get ICNames when
            //IC Arrangement option is checked in STEP 3.Further ICNames will be used to group the indicators on STEP 6.
            //Also make a comma delimited string of DataNIds which will be used to get comments from the database.
            ArrayList UniqueIndicatorNIds = new ArrayList();
            ArrayList UniqueDataNIds = new ArrayList();
            ArrayList UniqueSources = new ArrayList();
            DataRow RowDTAreaParentNid;
            ArrayList UniqueUnitNIds = new ArrayList();
            ArrayList UniqueAreaNIds = new ArrayList();
            ArrayList UniqueICNIds = new ArrayList();
            ArrayList UniqueSubgroupValNIds = new ArrayList();


            string ParentChildCID = string.Empty;
            int TotalAreaOccurence = 1;
            bool ParentExists = false;
            ArrayList ALANId = new ArrayList();
            DataTable DTParentChildRelation = new DataTable();
            DTParentChildRelation.Columns.Add("PNID");
            DTParentChildRelation.Columns.Add("ANID");
            DTParentChildRelation.Columns.Add("PAR");
            DTParentChildRelation.Columns.Add("AName");
            DataRow NewRow;
            //DataTable DTPresentationData=null;
            if (!string.IsNullOrEmpty(this._TableReport.AreaSelection.AreaLevels))
            {
                this.PresentationData.Sort = Area.AreaLevel;
            }

            this.MaxAreaLevel = -1;
            this.MinAreaLevel = -1;
            int CurrentAreaLevel = 1;
            int CurrentParentNId = -1;
            string[] SelectedAreaLevels = null;
            //Make the clone of presentation data. 			
            DataTable DTPresentationDataClone = null;

            if (!string.IsNullOrEmpty(this._TableReport.AreaSelection.AreaLevels))
            {
                DataTable DTPresentationData = this.PresentationData.ToTable();
                if (!DTPresentationData.Columns.Contains("ParentChildID"))
                {
                    DTPresentationData.Columns.Add("ParentChildID");
                }
                this.PresentationData = DTPresentationData.DefaultView;
                DataTable DTLevelPresentationData = DTPresentationData.Clone();
                SelectedAreaLevels = DICommon.SplitString(this.TableReport.AreaSelection.AreaLevels, ",");
                for (int AreaLevelCount = 0; AreaLevelCount < SelectedAreaLevels.Length; AreaLevelCount++)
                {
                    if (this.MinAreaLevel == -1)
                    {
                        this.MinAreaLevel = Convert.ToInt32(SelectedAreaLevels[AreaLevelCount]);
                        this.MaxAreaLevel = Convert.ToInt32(SelectedAreaLevels[AreaLevelCount]);
                    }
                    else
                    {
                        if (this.MinAreaLevel > Convert.ToInt32(SelectedAreaLevels[AreaLevelCount]))
                        {
                            this.MinAreaLevel = Convert.ToInt32(SelectedAreaLevels[AreaLevelCount]);
                        }
                        else
                        {
                            this.MaxAreaLevel = Convert.ToInt32(SelectedAreaLevels[AreaLevelCount]);
                        }
                    }
                }
                this.MaxMinAreaLevelDiff = this.MaxAreaLevel - (this.MinAreaLevel - 1);
                DataView DVTempLevelPresentationData;
                for (int i = this.MinAreaLevel; i <= this.MaxAreaLevel; i++)
                {
                    //Timeperiods.TimePeriodNId + "=" + dVPresentationData[0][Timeperiods.TimePeriodNId].ToString();												
                    this.PresentationData.RowFilter = Area.AreaLevel + "=" + i.ToString();
                    DVTempLevelPresentationData = this.PresentationData;
                    DVTempLevelPresentationData.Sort = Area.AreaName;
                    foreach (DataRowView DRVLevel in DVTempLevelPresentationData)
                    {
                        DTLevelPresentationData.ImportRow(DRVLevel.Row);
                    }
                    this.PresentationData.RowFilter = string.Empty;
                }
                this.PresentationData = DTLevelPresentationData.DefaultView;

                //Add Extra Columns for AreaLevels.
                DTPresentationData = this.PresentationData.ToTable();
                if (!DTPresentationData.Columns.Contains("AreaParentName"))
                {
                    DTPresentationData.Columns.Add("AreaParentName");
                    for (int i = this.MinAreaLevel; i <= this.MaxAreaLevel; i++)
                    {
                        DTPresentationData.Columns.Add("AL" + i.ToString());
                    }
                }
                this.PresentationData = DTPresentationData.DefaultView;
                DTPresentationDataClone = DTPresentationData.Clone();
                foreach (DataColumn DCol in DTPresentationDataClone.Columns)
                {
                    DCol.AllowDBNull = true;
                }
            }





            foreach (DataRowView DRVPresentationData in this.PresentationData)
            {
                if (!string.IsNullOrEmpty(this._TableReport.AreaSelection.AreaLevels))
                {
                    if (ALANId.Contains(DRVPresentationData[Area.AreaNId].ToString()))
                    {
                        foreach (DataRow Drow in DTParentChildRelation.Select("ANID='" + DRVPresentationData[Area.AreaNId] + "'"))
                        {
                            DRVPresentationData["ParentChildID"] = Drow["PAR"].ToString();
                            this.AddLevelAreas(DRVPresentationData, this.MinAreaLevel, DTParentChildRelation);
                            break;
                        }
                    }
                    else
                    {
                        ALANId.Add(DRVPresentationData[Area.AreaNId].ToString());
                        foreach (DataRow Drow in DTParentChildRelation.Select("ANID='" + DRVPresentationData[Area.AreaParentNId] + "'"))
                        {
                            ParentChildCID = Drow["PAR"].ToString();
                            break;
                        }
                        if (ParentChildCID == string.Empty && DTParentChildRelation.Rows.Count > 0)
                        {
                            foreach (DataRow Drow in DTParentChildRelation.Select("PNID='" + DRVPresentationData[Area.AreaParentNId] + "'"))
                            {
                                ParentExists = true;
                                ParentChildCID = Drow["PAR"].ToString();
                                break;
                            }
                            if (ParentExists == false)
                            {
                                this.AddAreaInPresentationData(ALANId, DTPresentationDataClone, DRVPresentationData.Row, DTParentChildRelation, this.MinAreaLevel);
                                foreach (DataRow Drow in DTParentChildRelation.Select("ANID='" + DRVPresentationData[Area.AreaParentNId] + "'"))
                                {
                                    ParentChildCID = Drow["PAR"].ToString();
                                    break;
                                }
                            }
                        }
                        else if (ParentChildCID == string.Empty && DTParentChildRelation.Rows.Count == 0 && Convert.ToInt32(DRVPresentationData[Area.AreaLevel]) > this.MinAreaLevel)
                        {
                            this.AddAreaInPresentationData(ALANId, DTPresentationDataClone, DRVPresentationData.Row, DTParentChildRelation, this.MinAreaLevel);
                            foreach (DataRow Drow in DTParentChildRelation.Select("ANID='" + DRVPresentationData[Area.AreaParentNId] + "'"))
                            {
                                ParentChildCID = Drow["PAR"].ToString();
                                break;
                            }
                        }


                        foreach (DataRow Drow in DTParentChildRelation.Select("PNID='" + DRVPresentationData[Area.AreaParentNId] + "'"))
                        {
                            TotalAreaOccurence++;
                        }

                        NewRow = DTParentChildRelation.NewRow();
                        if (ParentChildCID == string.Empty && DTParentChildRelation.Rows.Count == 0)
                        {
                            ParentChildCID = "0001";
                            NewRow[2] = ParentChildCID;
                            DRVPresentationData["ParentChildID"] = ParentChildCID;
                            this.AddLevelAreas(DRVPresentationData, this.MinAreaLevel, DTParentChildRelation);
                        }
                        else if (ParentChildCID != string.Empty)
                        {
                            if (ParentExists)
                            {
                                if (TotalAreaOccurence > 9)
                                {
                                    DRVPresentationData["ParentChildID"] = ParentChildCID.Substring(0, ParentChildCID.Length - 2) + TotalAreaOccurence.ToString();
                                    this.AddLevelAreas(DRVPresentationData, this.MinAreaLevel, DTParentChildRelation);
                                    NewRow[2] = ParentChildCID.Substring(0, ParentChildCID.Length - 2) + TotalAreaOccurence.ToString();
                                }
                                else
                                {
                                    DRVPresentationData["ParentChildID"] = ParentChildCID.Substring(0, ParentChildCID.Length - 1) + TotalAreaOccurence.ToString();
                                    this.AddLevelAreas(DRVPresentationData, this.MinAreaLevel, DTParentChildRelation);
                                    NewRow[2] = ParentChildCID.Substring(0, ParentChildCID.Length - 1) + TotalAreaOccurence.ToString();
                                }
                            }
                            else
                            {
                                if (TotalAreaOccurence > 9)
                                {
                                    NewRow[2] = ParentChildCID + "00" + TotalAreaOccurence.ToString();
                                    this.AddLevelAreas(DRVPresentationData, this.MinAreaLevel, DTParentChildRelation);
                                    DRVPresentationData["ParentChildID"] = ParentChildCID + "00" + TotalAreaOccurence.ToString();
                                }
                                else
                                {
                                    NewRow[2] = ParentChildCID + "000" + TotalAreaOccurence.ToString();
                                    this.AddLevelAreas(DRVPresentationData, this.MinAreaLevel, DTParentChildRelation);
                                    DRVPresentationData["ParentChildID"] = ParentChildCID + "000" + TotalAreaOccurence.ToString();
                                }
                            }
                        }

                        NewRow[0] = DRVPresentationData[Area.AreaParentNId].ToString();
                        NewRow[1] = DRVPresentationData[Area.AreaNId].ToString();
                        NewRow["AName"] = DRVPresentationData[Area.AreaName];
                        DTParentChildRelation.Rows.Add(NewRow);
                    }
                    ParentChildCID = string.Empty;
                    ParentExists = false;
                    TotalAreaOccurence = 1;
                }
                if (!string.IsNullOrEmpty(this._TableReport.AreaSelection.AreaLevels))
                {

                    DTPresentationDataClone.ImportRow(DRVPresentationData.Row);
                }

                RowDTAreaParentNid = this.DTAreaParentNId.NewRow();
                RowDTAreaParentNid[Area.AreaParentNId] = DRVPresentationData[Area.AreaParentNId];
                RowDTAreaParentNid[Area.AreaName] = DRVPresentationData[Area.AreaName];
                this.DTAreaParentNId.Rows.Add(RowDTAreaParentNid);

                //Get the unique UnitNIds.
                if (!UniqueUnitNIds.Contains(DRVPresentationData[Unit.UnitNId].ToString()))
                {
                    if (DRVPresentationData[Unit.UnitNId].ToString() != string.Empty)
                    {
                        UniqueUnitNIds.Add(DRVPresentationData[Unit.UnitNId].ToString());
                    }
                }

                //Get the unique SubgroupValNIds
                if (!UniqueSubgroupValNIds.Contains(DRVPresentationData[SubgroupVals.SubgroupValNId].ToString()))
                {
                    if (DRVPresentationData[SubgroupVals.SubgroupValNId].ToString() != string.Empty)
                    {
                        UniqueSubgroupValNIds.Add(DRVPresentationData[SubgroupVals.SubgroupValNId].ToString());
                    }
                }

                //Get the unique AreaNIds
                if (!UniqueAreaNIds.Contains(DRVPresentationData[Area.AreaNId].ToString()))
                {
                    if (DRVPresentationData[Area.AreaNId].ToString() != string.Empty)
                    {
                        UniqueAreaNIds.Add(DRVPresentationData[Area.AreaNId].ToString());
                    }
                }

                //Get the unique IC_NIds
                if (!UniqueICNIds.Contains(DRVPresentationData[IndicatorClassifications.ICNId].ToString()))
                {
                    if (DRVPresentationData[IndicatorClassifications.ICNId].ToString() != string.Empty)
                    {
                        UniqueICNIds.Add(DRVPresentationData[IndicatorClassifications.ICNId].ToString());
                    }
                }

                //Get the unique sources.
                if (!UniqueSources.Contains(DRVPresentationData[IndicatorClassifications.ICName].ToString()))
                {
                    if (DRVPresentationData[IndicatorClassifications.ICName].ToString() != string.Empty)
                    {
                        UniqueSources.Add(DRVPresentationData[IndicatorClassifications.ICName].ToString());
                    }
                }

                UniqueDataNIds.Add(DRVPresentationData[Data.DataNId].ToString());

                if (!UniqueIndicatorNIds.Contains(DRVPresentationData[Indicator.IndicatorNId].ToString()))
                {
                    if (DRVPresentationData[Indicator.IndicatorNId].ToString() != string.Empty)
                    {
                        UniqueIndicatorNIds.Add(DRVPresentationData[Indicator.IndicatorNId].ToString());
                    }
                }
            }
            if (!string.IsNullOrEmpty(this._TableReport.AreaSelection.AreaLevels))
            {
                this.PresentationData = DTPresentationDataClone.DefaultView;
            }

            //Prepare a comma delimited string DataNIds.
            string[] UniqueDataArray = new string[UniqueDataNIds.Count];
            UniqueDataNIds.CopyTo(UniqueDataArray);
            DataNIds = string.Join(",", UniqueDataArray);

            //Prepare a comma delimited string of indicator NIds.
            string[] UniqueIndicatorArray = new string[UniqueIndicatorNIds.Count];
            UniqueIndicatorNIds.CopyTo(UniqueIndicatorArray);
            this.IndicatorNIds = string.Join(",", UniqueIndicatorArray);

            //Prepare a comma delimited string of Unit NIds.
            string[] UniqueUnitNIdArray = new string[UniqueUnitNIds.Count];
            UniqueUnitNIds.CopyTo(UniqueUnitNIdArray);
            this.UnitNIds = string.Join(",", UniqueUnitNIdArray);


            //Prepare a comma delimited string of IC NIds.
            string[] UniqueICArray = new string[UniqueICNIds.Count];
            UniqueICNIds.CopyTo(UniqueICArray);
            this.ICNIds = string.Join(",", UniqueICArray);


            //Prepare a comma delimited string of subgroupvalNids.
            string[] UniqueSubgroupNIds = new string[UniqueSubgroupValNIds.Count];
            UniqueSubgroupValNIds.CopyTo(UniqueSubgroupNIds);
            this.SubgroupNIds = string.Join(",", UniqueSubgroupNIds);

            //Prepare a comma delimited string of subgroupvalNids.
            string[] UniqueAreaNIdArray = new string[UniqueAreaNIds.Count];
            UniqueAreaNIds.CopyTo(UniqueAreaNIdArray);
            this.AreaNIds = string.Join(",", UniqueAreaNIdArray);

            //Set the distinct sources to the DataTable DTSources
            DataRow RowDTSources;
            //If there are sources available then insert them in datatable
            if (UniqueSources.Count > 0)
            {
                if (this.DTSources != null)
                {
                    this.DTSources.Dispose();
                }
                this.DTSources = new DataTable();
                DTSources.Columns.Add(IndicatorClassifications.ICName);
                for (int Counter = 0; Counter < UniqueSources.Count; Counter++)
                {
                    RowDTSources = this.DTSources.NewRow();
                    RowDTSources[IndicatorClassifications.ICName] = UniqueSources[Counter].ToString();
                    this.DTSources.Rows.Add(RowDTSources);
                }
            }

            #endregion

            #region "-- Set Footnote/Comments datatables --"

            //Get How many Columns to be generated for Notes and Footnotes in the TableXLS.
            if (this._TemplateStyle.Footnotes.Show || this._TemplateStyle.Comments.Show)
            {
                //clears the datatables if they contain any data.
                if (this.DTFootNote != null)
                {
                    this.DTFootNote = null;
                }
                if (this.DTMasterComments != null)
                {
                    this.DTMasterComments = null;
                }
                if (this.DTComments != null)
                {
                    this.DTComments = null;
                }

                //Set the Table Structure for FootNotes DataTable.
                this.SetFootNoteTableStructure();

                //Set the Table structure of master table of Notes containing relationship of DataNId and NotesNId.
                this.SetCommentsMasterTableStructure();

                //Set the Table structure for notes DataTable.
                this.SetCommentsTableStructure();
                FootnoteCommentColumnCount = FootnoteCommentColumnsCount(DataNIds);
                //TODO : Code set for Denominator
                if (this._TemplateStyle.Denominator.Show)
                {
                    FootnoteCommentColumnCount++;
                }
                //OLD : Nothing
            }
            else
            {
                //clears the datatables if they contain any data.
                if (this.DTFootNote != null)
                {
                    this.DTFootNote = null;
                }
                if (this.DTMasterComments != null)
                {
                    this.DTMasterComments = null;
                }
                if (this.DTComments != null)
                {
                    this.DTComments = null;
                }
                this.FootnoteCommentType = FootnoteCommentType.None;
                //TODO : Code set for Denominator
                if (this._TemplateStyle.Denominator.Show)
                {
                    FootnoteCommentColumnCount = 1;
                }
                else
                {
                    FootnoteCommentColumnCount = 0;
                }
            }

            #endregion

            #region "-- Generate XLS Header --"

            //Generate TableXLSHeader
            _TableXLS = this.GenerateTableXLSHeader(_TableXLS, TotalColFieldVals, TotalColumnFields, TotalRowFields);
            _TableXLS.AcceptChanges();       //TableXLS header is ready.

            this.RaiseEventProgressBar(20, 100);

            #endregion

            #region "-- Fill Data into TableXLS --"

            //Sort the Presentation data view on Rows collection.
            string SortExpression = GetSortExpression();
            this.PresentationData.Sort = SortExpression;

            //If IC Arrangement on STEP 3 is selected 
            if (this._SelectedIndicatorClassification)
            {

                this.GetParentNode();

                //Provides IC_Parent_Nid,IC_Nid,IC_Name,Indicator_Nid (from  IUS) irrespective of Nids.
                //Provides all levels of records.
                DTICNames = this.GetICNames();

                //Pass just as a parameter to function MakeRecursiveClassificationCalls as the function is recursive.
                //and Variable can't be declared there.
                ArrayList ALClassification = new ArrayList();
                DataTable DTClassifications;//= new DataTable();
                int TableXLSColumns = _TableXLS.Columns.Count;


                //Contains IC_Parent_Nid,IC_Nid,IC_Name,Indicator_Nid (from  IUS) based on levels selected.
                //Sorting on Indicator Classification Names of each level.
                DTFilterICNames = FilterICNames(DTICNames, this.SelectedICLevel);
                DataView DVFltrICNames = DTFilterICNames.DefaultView;
                DVFltrICNames.Sort = IndicatorClassifications.ICOrder + " Asc";
                DTFilterICNames = DVFltrICNames.ToTable();

                //To include ICNIds from  DTFilterICNames ,append ICNIds in this.ICNIds.
                foreach (DataRow DRowDTFilterICNames in DTFilterICNames.Rows)
                {
                    //Get the unique IC_NIds
                    if (!UniqueICNIds.Contains(DRowDTFilterICNames[IndicatorClassifications.ICNId].ToString()))
                    {
                        UniqueICNIds.Add(DRowDTFilterICNames[IndicatorClassifications.ICNId].ToString());
                    }
                }
                this.ICNIds = string.Empty;
                //Prepare a comma delimited string of IC NIds.
                UniqueICArray = new string[UniqueICNIds.Count];
                UniqueICNIds.CopyTo(UniqueICArray);
                this.ICNIds = string.Join(",", UniqueICArray);


                //Sort the datatable DTFilterICNames based on ICName.
                //Take the dataview as datatable doesn't preserve the output after filter.
                //DataView DVFilterICNames = new DataView();
                //DVFilterICNames = DTFilterICNames.DefaultView;
                //DVFilterICNames.Sort = IndicatorClassifications.ICName + " ASC";
                //DTFilterICNames = DVFilterICNames.ToTable();

                //Fill the DataTable DTClassifications which will hold the IC elements which are checked on STEP 3.
                //Also DTClassifications will contains the IC elements in the correct order of sort.
                DTClassifications = FillClassificationTable(DTFilterICNames);
                DataView DVFillClassification = DTClassifications.DefaultView;
                DVFillClassification.Sort = IndicatorClassifications.ICOrder + " ASC";
                DTClassifications = DVFillClassification.ToTable();

                if (DTClassifications != null && DTClassifications.Rows.Count > 0)
                {
                    //Iterate for Top Level (1) of IC Classification
                    foreach (DataRow DRowDTClassifications in DTClassifications.Rows)
                    {
                        //If level is 1 then add the GroupHeader as it will not going to be added in MakeRecursiveClassificationCalls for level 1.
                        if (this.SelectedICLevel == 1)
                        {
                            TableXLSRow = _TableXLS.NewRow();
                            RowDTRowsForIcons = this.DTRowsForIcons.NewRow();
                            RowDTRowsForIcons[0] = IconElementType.IndicatorClassification;
                            RowDTRowsForIcons[1] = DRowDTClassifications[IndicatorClassifications.ICNId].ToString();
                            TableXLSRow[0] = DRowDTClassifications["IC_Name"].ToString();
                            TableXLSRow[TablePresentation.ROWTYPE] = RowType.ICAggregate;
                            TableXLSRow[TablePresentation.AREALEVEL] = "-1";
                            TableXLSRow[TablePresentation.ROWINDEX] = _TableXLS.Rows.Count + 1;
                            this.DTRowsForIcons.Rows.Add(RowDTRowsForIcons);
                            _TableXLS.Rows.Add(TableXLSRow);
                        }

                        //When level selected is more than one but there is no child of root IC then insert the IC_Name into TableXLS.
                        if (this.SelectedICLevel != 1 && DTFilterICNames.Select(IndicatorClassifications.ICParent_NId + "='" + DRowDTClassifications[IndicatorClassifications.ICNId] + "'").Length == 0)
                        {
                            ICAdded = false;
                            foreach (DataRow DRowDTFilterICNames in DTFilterICNames.Select(IndicatorClassifications.ICNId + "='" + DRowDTClassifications[IndicatorClassifications.ICNId] + "'"))
                            {
                                //if the IC is not present in table XLS then Add it.
                                if (DRowDTFilterICNames[Indicator.IndicatorNId].ToString().Length > 0 && ICAdded == false)
                                {
                                    TableXLSRow = _TableXLS.NewRow();
                                    RowDTRowsForIcons = this.DTRowsForIcons.NewRow();
                                    RowDTRowsForIcons[0] = IconElementType.IndicatorClassification;
                                    RowDTRowsForIcons[1] = DRowDTClassifications[IndicatorClassifications.ICNId].ToString();
                                    TableXLSRow[0] = DRowDTClassifications["IC_Name"].ToString();
                                    TableXLSRow[TablePresentation.ROWTYPE] = RowType.ICAggregate;
                                    TableXLSRow[TablePresentation.AREALEVEL] = "-1";
                                    TableXLSRow[TablePresentation.ROWINDEX] = _TableXLS.Rows.Count + 1;
                                    this.DTRowsForIcons.Rows.Add(RowDTRowsForIcons);
                                    _TableXLS.Rows.Add(TableXLSRow);
                                    ICAdded = true;					//After inserting the IC, Mark it as Added.
                                }
                            }
                        }
                        MakeRecursiveClassificationCalls(DTFilterICNames, _TableXLS, DRowDTClassifications, TotalRowFields, TotalColFieldVals, TotalColumnFields, string.Empty, this.SelectedICLevel, 1, string.Empty, ALClassification, TableXLSColumns);
                    }

                    //Make a list of Orphand Indicators.
                    for (int Counter = 0; Counter < UniqueIndicatorNIds.Count; Counter++)
                    {
                        IsOrphand = true;
                        foreach (DataRow DRowDTFilterICNames in DTFilterICNames.Select(Indicator_Unit_Subgroup.IndicatorNId + "='" + UniqueIndicatorNIds[Counter] + "'"))
                        {
                            IsOrphand = false;
                        }
                        if (IsOrphand == true)
                        {
                            OrphandIndicators.Add(UniqueIndicatorNIds[Counter].ToString());
                        }
                    }
                }
                else
                {
                    FilterString = string.Empty;
                    this.FillTableXLS(FilterString, _TableXLS, TotalRowFields, TotalColFieldVals, TotalColumnFields);
                }
            }
            // If IC Arrangement on STEP 3 is NOT selected
            else
            {
                FilterString = string.Empty;
                this.FillTableXLS(FilterString, _TableXLS, TotalRowFields, TotalColFieldVals, TotalColumnFields);
            }

            #endregion

            #region "-- Apply aggregate and orphand Indicators --"

            this.RaiseEventProgressBar(40, 100);

            // Apply aggregates.			
            if (this._AggregateOn == true)
            {
                _TableXLS = ApplyAggregate(_TableXLS, TotalColFieldVals, TotalRowFields);
                this.RaiseEventProgressBar(60, 100);
            }

            //Fill TableXLS with the records of orphand indicators.
            for (int Counter = 0; Counter < OrphandIndicators.Count; Counter++)
            {
                //Add a blank row
                TableXLSRow = _TableXLS.NewRow();
                TableXLSRow[TablePresentation.ROWTYPE] = RowType.EmptyRow;
                TableXLSRow[TablePresentation.ROWINDEX] = _TableXLS.Rows.Count + 1;
                _TableXLS.Rows.Add(TableXLSRow);

                //Add records of orphand indicators.
                FilterString = Indicator.IndicatorNId + " = " + OrphandIndicators[Counter].ToString();
                this.FillTableXLS(FilterString, _TableXLS, TotalRowFields, TotalColFieldVals, TotalColumnFields);
                this.RaiseEventProgressBar(60, 100);
            }


            #endregion

            #region " --Fill Footnote/Notes value from database-- "

            //Get the FootNote from database if footnote is available in the PresentationData
            //Fill the FootNotes in DTFootNote which is containing FootnoteNId and blank Footnote column.
            if (this._TemplateStyle.Footnotes.Show)
            {
                if (this.DTFootNote.Rows.Count > 0)
                {
                    DataRow[] RowsDTFootNoteTemp;
                    DataTable DTFootNoteTemp;
                    string FootNoteNids = string.Empty;
                    foreach (DataRow DRowDTFootNote in this.DTFootNote.Rows)
                    {
                        if (FootNoteNids != string.Empty)
                        {
                            FootNoteNids = FootNoteNids + ",";
                        }
                        FootNoteNids = FootNoteNids + DRowDTFootNote[FootNotes.FootNoteNId].ToString();
                    }


                    string QueryGetFootNote = this.DIQueries.Footnote.GetFootnote(FilterFieldType.NId, FootNoteNids);
                    DTFootNoteTemp = new DataTable();

                    DTFootNoteTemp = this.DIConnection.ExecuteDataTable(QueryGetFootNote);
                    foreach (DataRow DRowDTFootNote in this.DTFootNote.Rows)
                    {
                        RowsDTFootNoteTemp = DTFootNoteTemp.Select(FootNotes.FootNoteNId + "='" + DRowDTFootNote[FootNotes.FootNoteNId] + "'");
                        foreach (DataRow DRowRowsDTFootNoteTemp in RowsDTFootNoteTemp)
                        {
                            DRowDTFootNote[FootNotes.FootNote] = DRowRowsDTFootNoteTemp[FootNotes.FootNote];
                        }
                    }
                }
                //Fill the Footnotes values in DTDataSheet
                DataRow[] RowsDTDataSheet;
                foreach (DataRow DRowDTFootNote in this.DTFootNote.Rows)
                {
                    RowsDTDataSheet = this.DTDataSheet.Select(FootNotes.FootNote + "='" + DRowDTFootNote[FootNotes.FootNoteNId] + "'");
                    foreach (DataRow DRow in RowsDTDataSheet)
                    {
                        DRow[FootNotes.FootNote] = DRowDTFootNote[FootNotes.FootNote];
                    }
                }
                //Emtry the columnvalue for footnote to empty where value is -1.
                foreach (DataRow DRow in this.DTDataSheet.Select(FootNotes.FootNote + "='-1'"))
                {
                    DRow[FootNotes.FootNote] = string.Empty;
                }
            }

            //Get the Notes from database if note is available in the PresentationData
            if (this._TemplateStyle.Comments.Show)
            {
                if (this.DTComments.Rows.Count > 0)
                {
                    string NoteNIds = string.Empty;
                    DataTable DTCommentsTemp;
                    DataRow[] RowsDTCommentsTemp;

                    foreach (DataRow DRowDTComments in this.DTComments.Rows)
                    {
                        if (NoteNIds != string.Empty)
                        {
                            NoteNIds += ",";
                        }
                        NoteNIds += DRowDTComments[Notes_Data.NotesNId].ToString();
                    }
                    string QueryGetNote = this.DIQueries.Notes.GetNotes(string.Empty, NoteNIds, string.Empty, string.Empty, CheckedStatus.True, FieldSelection.Light);
                    DTCommentsTemp = new DataTable();
                    DTCommentsTemp = this.DIConnection.ExecuteDataTable(QueryGetNote);
                    foreach (DataRow DRowDTComments in this.DTComments.Rows)
                    {
                        RowsDTCommentsTemp = DTCommentsTemp.Select(Notes.NotesNId + "='" + DRowDTComments[Notes.NotesNId] + "'");
                        foreach (DataRow DRowRowsDTCommentTemp in RowsDTCommentsTemp)
                        {
                            DRowDTComments[Notes.Note] = DRowRowsDTCommentTemp[Notes.Note];
                        }
                    }
                }
            }

            this.RaiseEventProgressBar(70, 100);
            #endregion

            //OPT : 
            //If Footnote is selected as InlineWithData then remove the footnote columns from the TableXLS.
            //Required in case of graph wizard as footnotes have to be displayeds in datacolumns.
            if (this._TemplateStyle.Footnotes.Show)
            {
                if (this.FootnoteInLine == FootNoteDisplayStyle.InlineWithData && this.DTFootNote.Rows.Count > 0)
                {
                    TempTableXLS = new DataTable();
                    TempTableXLS = _TableXLS.Copy();
                    TempTableXLS.AcceptChanges();
                    int RowFieldCount = this._Fields.Rows.Count;
                    if (!this._ShowExcel)
                    {
                        RowFieldCount = 1;
                    }
                    int FootNoteColumn = 0;
                    for (int Counter = 0; Counter < _TableXLS.Columns.Count - this.ExtraColumnCount; Counter++)
                    {
                        if (Counter > RowFieldCount)
                        {
                            if (FootNoteColumn % 2 == 0)
                            {
                                DataColumn Column = _TableXLS.Columns[Counter];
                                _TableXLS.Columns.Remove(Column);
                            }
                        }
                    }
                }
            }

            #region "-- Execute Aditional sorting on DataValues --"

            //Sorting Based on Data
            SortExpression = GetSortExpression();
            //Variable to check whether the datavalue sorting condition in the SortExpresssion is the last one or not.
            Boolean ExecuteAditonalSorting = false;
            if (SortExpression.Contains(DataExpressionColumns.DataType))
            {
                string[] ArrySortExpr = SortExpression.Split(',');
                if ((!ArrySortExpr[ArrySortExpr.Length - 1].ToString().Contains(DataExpressionColumns.TextualData)) || ArrySortExpr.Length == 3)
                {
                    ExecuteAditonalSorting = true;
                }
            }


            //Executes when SorExpression Contains the sorting condition for datavalue and also the sorting condition is not the last one.
            if (ExecuteAditonalSorting)
            {
                DataTable TempXLS = new DataTable();
                DataTable TempXLSFinal = new DataTable();
                TempXLS = TableXLS.Clone();
                TempXLS.Columns.Add(DataExpressionColumns.DataType, typeof(int));
                TempXLS.Columns.Add(DataExpressionColumns.NumericData, typeof(double));
                TempXLS.Columns.Add(DataExpressionColumns.TextualData);


                DataRow TempXLSRow;
                DataRow TempXLSFinalRow;
                //Make a clone of TableXLS
                TempXLSFinal = TableXLS.Clone();
                bool NewChunk = false;
                DataView DVTempXLS;

                int DataValueColumn = this._Fields.Rows.Count;

                //Loop through each row.
                //Pick a chunk of rows which are of DataRow type.
                //Apply sorting in the datatable whcih is containin this chunk.				
                //Export rows into clone of TableXLS which are not of DataRow type.
                //Export the rows of chunk to the same clone after applying sorting in the chunk.				
                foreach (DataRow Drow in TableXLS.Rows)
                {
                    if (Drow[TablePresentation.ROWTYPE].ToString() == RowType.DataRow.ToString())
                    {
                        TempXLSRow = TempXLS.NewRow();
                        for (int i = 0; i < TableXLS.Columns.Count; i++)
                        {
                            if (i == DataValueColumn)
                            {
                                if (DICommon.IsNumeric(Drow[i].ToString(), System.Globalization.CultureInfo.CurrentCulture))
                                {
                                    TempXLSRow[DataExpressionColumns.DataType] = 1;
                                    TempXLSRow[DataExpressionColumns.NumericData] = Drow[i];
                                }
                                else
                                {
                                    TempXLSRow[DataExpressionColumns.DataType] = 2;
                                    TempXLSRow[DataExpressionColumns.TextualData] = Drow[i].ToString();
                                }
                            }
                            TempXLSRow[i] = Drow[i];
                        }
                        TempXLS.Rows.Add(TempXLSRow);
                        NewChunk = true;
                    }
                    else
                    {
                        if (NewChunk)
                        {
                            //Import all rows from TempXLS to TempXLSFinal after sorting in TempXLS
                            DVTempXLS = new DataView();
                            DVTempXLS = TempXLS.DefaultView;
                            DVTempXLS.Sort = SortExpression;
                            TempXLS = DVTempXLS.ToTable();
                            foreach (DataRow row in TempXLS.Rows)
                            {
                                TempXLSFinalRow = TempXLSFinal.NewRow();
                                for (int i = 0; i < TempXLS.Columns.Count - (3 + this.TotalOrderColCount); i++)
                                {
                                    TempXLSFinalRow[i] = row[i];
                                }
                                TempXLSFinal.Rows.Add(TempXLSFinalRow);
                            }
                            NewChunk = false;
                            TempXLS.Clear();
                        }
                        TempXLSFinal.ImportRow(Drow);
                    }
                }
                if (NewChunk)
                {
                    DVTempXLS = new DataView();
                    DVTempXLS = TempXLS.DefaultView;
                    DVTempXLS.Sort = SortExpression;
                    TempXLS = DVTempXLS.ToTable();
                    foreach (DataRow row in TempXLS.Rows)
                    {
                        TempXLSFinalRow = TempXLSFinal.NewRow();
                        for (int i = 0; i < TempXLS.Columns.Count - (3 + this.TotalOrderColCount); i++)
                        {
                            TempXLSFinalRow[i] = row[i];
                        }
                        TempXLSFinal.Rows.Add(TempXLSFinalRow);
                    }
                }
                this._TableXLS = TempXLSFinal;
            }
            for (int i = 0; i < this.TotalOrderColCount; i++)
            {
                this._TableXLS.Columns.RemoveAt(this._TableXLS.Columns.Count - 1);
            }

            #endregion

            #region "-- Generate XLS Sheet --"

            try
            {
                // -- Total column is within the range of excel column limit i.e. 256
                // -- 250 column limit is taken to prevent the RTE in border line case. Few extra columns are generated at the time creation of TW.
                if (_TableXLS.Columns.Count <= 250 && this._Fields.Rows.Count > 0) // && this._PresentationType == Presentation.PresentationType.Table)
                {
                    //SW.WriteLine(DateTime.Now.ToString() + "Generate MHT starts");
                    //Fill XLS file of SpreadSheet gear.
                    this._TPMessage = TablePresentationMessages.None;
                    RetVal = GeneratesXLSFile(_TableXLS, this.DTFootNote, this.DTComments, PresentationOutputFolder, presentationFileName);
                    //SW.WriteLine(DateTime.Now.ToString() + "MHT created");
                    //SW.Close();
                }
                else
                {
                    RetVal = string.Empty;
                    this._TPMessage = TablePresentationMessages.ColumnsExceeded;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }

            this.RaiseEventProgressBar(100, 100);

            #endregion

            return RetVal;
        }

        /// <summary>
        /// 
        /// </summary>
        public void RotateView()
        {
            {
                // -- Move Row Field into the Column field and Column field into the Row
                // -- One at a time

                if (this.Fields.Rows.Count > 0)
                {
                    Field oPresentationFieldRow;
                    int RowIndex = this.Fields.Rows.Count - 1;

                    // -- Move non IC or metadata Last Field of Row to the Column
                    while (this._Fields.Rows[RowIndex].FieldID.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.Metadata) || this._Fields.Rows[RowIndex].FieldID.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.IC))
                    {
                        RowIndex -= 1;
                    }
                    oPresentationFieldRow = this.Fields.Rows[RowIndex];

                    if (this.Fields.Columns.Count > 0)
                    {
                        Field oPresentationFieldCol;
                        // -- Move First Field of Column to the Row
                        oPresentationFieldCol = this.Fields.Columns[0];

                        // -- Add to the first position of the Row
                        this.Fields.Rows.Add(oPresentationFieldCol);
                        this.Fields.Rows.MoveToTop(oPresentationFieldCol);
                        this.Fields.Columns.Remove(oPresentationFieldCol);

                        this.Fields.Columns.Add(oPresentationFieldRow);
                        this.Fields.Rows.Remove(oPresentationFieldRow);

                        // -- Reset the Sort Order
                        this.Fields.Sort.Clear();
                        for (int cc = 0; cc <= this.Fields.Rows.Count - 1; cc++)
                        {
                            this.Fields.Sort.Add(this.Fields.Rows[cc]);
                        }
                    }
                    else if (this.Fields.Rows.Count > 1)
                    {
                        // When there more than i items in the Rows and Columns are blank
                        // Move the last item of the Row into the Columns

                        // -- Add to the first position of the Columns
                        this.Fields.Columns.Add(oPresentationFieldRow);
                        this.Fields.Columns.MoveToTop(oPresentationFieldRow);
                        this.Fields.Rows.Remove(oPresentationFieldRow);

                        // -- Reset the Sort Order
                        this.Fields.Sort.Clear();
                        for (int cc = 0; cc <= this.Fields.Rows.Count - 1; cc++)
                        {
                            this.Fields.Sort.Add(this.Fields.Rows[cc]);
                        }
                    }
                }
                try
                {
                    this.SetMetadataInfo(this._UserPreference.DataView.MetadataIndicatorField, this._UserPreference.DataView.MetadataAreaField, this._UserPreference.DataView.MetadataSourceField);
                    this.SetICInfo(this._UserPreference.DataView.ICFields);
                }
                catch (Exception ex)
                {
                }

            }
        }

        public void RotateViewClockwise()
        {
            {
                // -- Move Row Field into the Column field and Column field into the Row
                // -- One at a time

                if (this.Fields.Columns.Count > 0)
                {
                    Field oPresentationFieldRow;
                    int ColumnIndex = this.Fields.Columns.Count - 1;

                    // -- Move the non ic and metadata Last Field of Row to the Column
                    while (this._Fields.Columns[ColumnIndex].FieldID.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.Metadata) || this._Fields.Columns[ColumnIndex].FieldID.StartsWith(DI_LibBAL.UI.UserPreference.UserPreference.DataviewPreference.IC))
                    {
                        ColumnIndex -= 1;
                    }
                    oPresentationFieldRow = this.Fields.Columns[ColumnIndex];

                    if (this.Fields.Rows.Count > 0)
                    {
                        Field oPresentationFieldCol;
                        // -- Move First Field of Column to the Row
                        oPresentationFieldCol = this.Fields.Rows[0];

                        // -- Add to the first position of the Row
                        this.Fields.Columns.Add(oPresentationFieldCol);
                        this.Fields.Columns.MoveToTop(oPresentationFieldCol);
                        this.Fields.Rows.Remove(oPresentationFieldCol);

                        this.Fields.Rows.Add(oPresentationFieldRow);
                        this.Fields.Columns.Remove(oPresentationFieldRow);

                        // -- Reset the Sort Order
                        this.Fields.Sort.Clear();
                        for (int cc = 0; cc <= this.Fields.Rows.Count - 1; cc++)
                        {
                            this.Fields.Sort.Add(this.Fields.Rows[cc]);
                        }
                    }
                    else if (this.Fields.Columns.Count > 1)
                    {
                        // When there more than i items in the Rows and Columns are blank
                        // Move the last item of the Row into the Columns

                        // -- Add to the first position of the Columns
                        this.Fields.Rows.Add(oPresentationFieldRow);
                        this.Fields.Rows.MoveToTop(oPresentationFieldRow);
                        this.Fields.Columns.Remove(oPresentationFieldRow);

                        // -- Reset the Sort Order
                        this.Fields.Sort.Clear();
                        for (int cc = 0; cc <= this.Fields.Rows.Count - 1; cc++)
                        {
                            this.Fields.Sort.Add(this.Fields.Rows[cc]);
                        }
                    }
                }
                try
                {
                    this.SetMetadataInfo(this._UserPreference.DataView.MetadataIndicatorField, this._UserPreference.DataView.MetadataAreaField, this._UserPreference.DataView.MetadataSourceField);
                    this.SetICInfo(this._UserPreference.DataView.ICFields);
                }
                catch (Exception ex)
                {
                }

            }
        }

        /// <summary>
        /// Generate the fields Row and column collection on the basis of isPyramidChart
        /// </summary>
        /// <param name="isPyramidChart"></param>
        public void GenFieldCollection(bool isPyramidChart)
        {
            if (this._Fields != null)
            {
                this.IsPyramidChart = isPyramidChart;
                this._Fields.All.Clear();
                this._Fields.Available.Clear();
                this._Fields.Rows.Clear();
                this._Fields.Columns.Clear();
                this._Fields.Sort.Clear();

                this.FillAllFields();
                if (isPyramidChart)
                {
                    ArrangeCollectionFieldsForPyramid(this.PresentationData);
                }
                else
                {
                    this.PresentationData.RowFilter = string.Empty;
                    this.ArrangeCollectionFields(this.PresentationData);

                    // Wizard STEP 3: Column Arrangement.
                    this._ColumnArrangementTable = this.SetColumnArrangement(true);

                    //-- Sort the column arrangement table
                    this.SortColumnArrangementTable();
                }
            }
        }

        #endregion

        #region " -- Step 2 --"

        /// <summary>
        /// Generate the theme datatable
        /// </summary>
        /// <param name="themeIndex"></param>
        /// <returns></returns>
        public DataTable GetLegendDataTable(int themeIndex)
        {
            DataTable RetVal = new DataTable();
            try
            {
                RetVal.Columns.Add(ColorLegendColumns.CAPTION, typeof(string));
                RetVal.Columns.Add(ColorLegendColumns.LEGEND, typeof(string));
                RetVal.Columns.Add(ColorLegendColumns.FROM, typeof(string));
                RetVal.Columns.Add(ColorLegendColumns.TO, typeof(string));
                RetVal.Columns.Add(ColorLegendColumns.COUNT, typeof(int));
                RetVal.Columns.Add(ColorLegendColumns.COLOR, typeof(Color));

                string FormatString = this.Themes[themeIndex].FormatNumber(this.Themes[themeIndex].Decimal);

                DataRow Row;

                for (int i = 0; i < this.Themes[themeIndex].BreakCount; i++)
                {
                    Row = RetVal.NewRow();


                    Row[0] = this.Themes[themeIndex].Legends[i].Caption;
                    Row[1] = this.Themes[themeIndex].Legends[i].Range;
                    Row[2] = this.Themes[themeIndex].Legends[i].RangeFrom.ToString(FormatString);
                    Row[3] = this.Themes[themeIndex].Legends[i].RangeTo.ToString(FormatString);
                    Row[4] = this.Themes[themeIndex].Legends[i].Count;
                    Row[5] = this.Themes[themeIndex].Legends[i].Color;
                    RetVal.Rows.Add(Row);
                }
                RetVal.AcceptChanges();
            }
            catch (Exception ex)
            {
                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        /// Set the theme for the indicators
        /// <para> It extract the distinct indicator and set the themes and legends against that indicator </para>
        /// </summary>
        /// <param name="DefaultColorArray">TODO</param>
        public void SetColorTheme(System.Drawing.Color[] DefaultColorArray)
        {
            double Min = 0;
            double Max = 0;
            int Decimal = 0;
            double DataValue = 0;
            double PreviousIndicatorCount;

            if (this.PresentationData != null)
            {
                // -- sort the data view on the basis of indicator, so that we can easily find out the min, max and decimal values.
                this.PresentationData.Sort = Indicator.IndicatorNId + " ASC";

                // -- add the data column indicator and Indicator_NId                
                this._DistinctIndicator = new DataTable("DistinctIndicator");
                this._DistinctIndicator.Columns.Add(Indicator.IndicatorName);
                this._DistinctIndicator.Columns.Add(Indicator.IndicatorNId);
                this._DistinctIndicator.Columns.Add(Indicator.IndicatorGId);

                // -- List to store the unique indicator and to check whether indicaot is already added in the list
                List<string> IndicatorList = new List<string>();

                // -- set the Initial indicator Count
                PreviousIndicatorCount = IndicatorList.Count;
                DataRowView UserRowView;
                // -- Loop to set the new theme
                for (int i = 0; i < this.PresentationData.Count; i++)
                {
                    UserRowView = this.PresentationData[i];
                    if (!IndicatorList.Contains(UserRowView[Indicator.IndicatorName].ToString()))
                    {
                        // -- New indicator comes i.e. Not in the IndicatorList
                        if (DICommon.IsNumeric(DICommon.SetDecimalSperator(UserRowView[DataExpressionColumns.NumericData].ToString())))
                        {
                            if (IndicatorList.Count > PreviousIndicatorCount)
                            {
                                // -- add the theme for the previous added indicator
                                // -- Set the color , if it is passed by hosting application
                                this.Themes.Add(new Theme(this._DistinctIndicator.Rows[this._DistinctIndicator.Rows.Count - 1][Indicator.IndicatorName].ToString(), this._DistinctIndicator.Rows[this._DistinctIndicator.Rows.Count - 1][Indicator.IndicatorNId].ToString(), Convert.ToDecimal(Min), Convert.ToDecimal(Max), Decimal, this.PresentationData, DefaultColorArray, this.Themes, this._DistinctIndicator.Rows.Count - 1, false));
                                PreviousIndicatorCount = IndicatorList.Count;
                            }

                            // -- set the default min, max and decimal value
                            Min = DICommon.ParseStringToDouble(DICommon.SetDecimalSperator(UserRowView[DataExpressionColumns.NumericData].ToString()), System.Globalization.CultureInfo.CurrentCulture);
                            Max = DICommon.ParseStringToDouble(DICommon.SetDecimalSperator(UserRowView[DataExpressionColumns.NumericData].ToString()), System.Globalization.CultureInfo.CurrentCulture);
                            DataValue = DICommon.ParseStringToDouble(DICommon.SetDecimalSperator(UserRowView[DataExpressionColumns.NumericData].ToString()), System.Globalization.CultureInfo.CurrentCulture);

                            if (DICommon.SetDecimalSperator(DataValue.ToString()).IndexOf(DICommon.DecimalSperator) == -1 && Decimal <= 0)
                            {
                                Decimal = 0;
                            }
                            else
                            {
                                Decimal = DataValue.ToString().Length - DICommon.SetDecimalSperator(DataValue.ToString()).IndexOf(DICommon.DecimalSperator) - 1;
                            }

                            // -- If the Data value is numeric
                            IndicatorList.Add(UserRowView[Indicator.IndicatorName].ToString());

                            DataRow Row;
                            Row = this._DistinctIndicator.NewRow();
                            // -- Indicator Name
                            Row[0] = UserRowView[Indicator.IndicatorName].ToString();
                            // -- Indicator GId
                            Row[1] = UserRowView[Indicator.IndicatorNId].ToString();
                            // -- Indicator GId
                            Row[2] = UserRowView[Indicator.IndicatorGId].ToString();
                            this._DistinctIndicator.Rows.Add(Row);
                        }
                    }
                    else
                    {
                        if (DICommon.IsNumeric(this.PresentationData[i][DataExpressionColumns.NumericData].ToString()))
                        {
                            // -- Datavalue is numeric
                            DataValue = Convert.ToDouble(this.PresentationData[i][DataExpressionColumns.NumericData].ToString().Replace(DICommon.DecimalSperator, DICommon.NumberDecimalSeparator));
                            // -- set the min & max value
                            Min = Math.Min(Min, DataValue);
                            Max = Math.Max(Max, DataValue);

                            if (DICommon.SetDecimalSperator(DataValue.ToString()).IndexOf(DICommon.DecimalSperator) > -1)
                            {
                                // -- if the decimal is less then new decimal count
                                Decimal = Math.Max(Decimal, DataValue.ToString().Length - DICommon.SetDecimalSperator(DataValue.ToString()).IndexOf(DICommon.DecimalSperator) - 1);
                            }
                        }
                    }
                }
                if (IndicatorList.Count > PreviousIndicatorCount)
                {
                    // -- add the theme for the last indicator
                    // -- Set the color , if it is passed by hosting application
                    this.Themes.Add(new Theme(this._DistinctIndicator.Rows[this._DistinctIndicator.Rows.Count - 1][Indicator.IndicatorName].ToString(), this._DistinctIndicator.Rows[this._DistinctIndicator.Rows.Count - 1][Indicator.IndicatorNId].ToString(), Convert.ToDecimal(Min), Convert.ToDecimal(Max), Decimal, this.PresentationData, DefaultColorArray, this.Themes, this._DistinctIndicator.Rows.Count - 1, false));
                    PreviousIndicatorCount = IndicatorList.Count;
                }
                this._DistinctIndicator.AcceptChanges();
                this.PresentationData.RowFilter = string.Empty;
            }
        }

        /// <summary>
        /// Set the theme for the indicators
        /// <para> It extract the distinct indicator and set the themes and legends against that indicator </para>
        /// </summary>
        /// <param name="DefaultColorArray">TODO</param>
        public void SetColorThemeForStats(System.Drawing.Color[] DefaultColorArray)
        {
            try
            {
                double Min = 0;
                double Max = 0;
                int Decimal = 0;
                double DataValue = 0;
                double PreviousIUSCount;
                DataRowView UserRowView;
                List<string> IUSList = new List<string>();// -- List to store the unique indicator and to check whether indicaot is already added in the list

                if (this.PresentationData != null)
                {
                    // -- sort the data view on the basis of indicator, so that we can easily find out the min, max and decimal values.
                    this.PresentationData.Sort = Indicator_Unit_Subgroup.IUSNId + " ASC";

                    // -- add the data column indicator and Indicator_NId                
                    this._DistinctIUS = new DataTable(DistinctThemeIUS);
                    this._DistinctIUS.Columns.Add(IUSName);
                    this._DistinctIUS.Columns.Add(Indicator_Unit_Subgroup.IUSNId);



                    // -- set the Initial indicator Count
                    PreviousIUSCount = IUSList.Count;

                    // -- Loop to set the new theme
                    for (int i = 0; i < this.PresentationData.Count; i++)
                    {
                        UserRowView = this.PresentationData[i];
                        if (!IUSList.Contains(UserRowView[Indicator.IndicatorName].ToString() + Seprator + UserRowView[Unit.UnitName].ToString() + Seprator + UserRowView[SubgroupVals.SubgroupVal].ToString()))
                        {
                            // -- New indicator comes i.e. Not in the IndicatorList
                            if (DICommon.IsNumeric(DICommon.SetDecimalSperator(UserRowView[DataExpressionColumns.NumericData].ToString())))
                            {
                                if (IUSList.Count > PreviousIUSCount)
                                {
                                    // -- add the theme for the previous added indicator
                                    // -- Set the color , if it is passed by hosting application
                                    this.FrequencyThemes.Add(new Theme(this._DistinctIUS.Rows[this._DistinctIUS.Rows.Count - 1][IUSName].ToString(), this._DistinctIUS.Rows[this._DistinctIUS.Rows.Count - 1][Indicator_Unit_Subgroup.IUSNId].ToString(), Convert.ToDecimal(Min), Convert.ToDecimal(Max), Decimal, this.PresentationData, DefaultColorArray, this.FrequencyThemes, this._DistinctIUS.Rows.Count - 1, true));
                                    PreviousIUSCount = IUSList.Count;
                                    Decimal = 0;
                                }

                                // -- set the default min, max and decimal value
                                Min = DICommon.ParseStringToDouble(DICommon.SetDecimalSperator(UserRowView[DataExpressionColumns.NumericData].ToString()), System.Globalization.CultureInfo.CurrentCulture);
                                Max = DICommon.ParseStringToDouble(DICommon.SetDecimalSperator(UserRowView[DataExpressionColumns.NumericData].ToString()), System.Globalization.CultureInfo.CurrentCulture);
                                DataValue = DICommon.ParseStringToDouble(DICommon.SetDecimalSperator(UserRowView[DataExpressionColumns.NumericData].ToString()), System.Globalization.CultureInfo.CurrentCulture);

                                if (DICommon.SetDecimalSperator(DataValue.ToString()).IndexOf(DICommon.DecimalSperator) == -1 && Decimal <= 0)
                                {
                                    Decimal = 0;
                                }
                                else
                                {
                                    Decimal = DataValue.ToString().Length - DICommon.SetDecimalSperator(DataValue.ToString()).IndexOf(DICommon.DecimalSperator) - 1;
                                }

                                // -- If the Data value is numeric
                                IUSList.Add(UserRowView[Indicator.IndicatorName].ToString() + Seprator + UserRowView[Unit.UnitName].ToString() + Seprator + UserRowView[SubgroupVals.SubgroupVal].ToString());

                                DataRow Row;
                                Row = this._DistinctIUS.NewRow();
                                // -- Indicator Name
                                Row[IUSName] = UserRowView[Indicator.IndicatorName].ToString() + Seprator + UserRowView[Unit.UnitName].ToString() + Seprator + UserRowView[SubgroupVals.SubgroupVal].ToString();
                                // -- Indicator GId
                                Row[Indicator_Unit_Subgroup.IUSNId] = UserRowView[Indicator_Unit_Subgroup.IUSNId].ToString();
                                // -- Indicator GId
                                this._DistinctIUS.Rows.Add(Row);
                            }
                        }
                        else
                        {
                            if (DICommon.IsNumeric(this.PresentationData[i][DataExpressionColumns.NumericData].ToString()))
                            {
                                // -- Datavalue is numeric
                                DataValue = Convert.ToDouble(this.PresentationData[i][DataExpressionColumns.NumericData].ToString().Replace(DICommon.DecimalSperator, DICommon.NumberDecimalSeparator));
                                // -- set the min & max value
                                Min = Math.Min(Min, DataValue);
                                Max = Math.Max(Max, DataValue);

                                if (DICommon.SetDecimalSperator(DataValue.ToString()).IndexOf(DICommon.DecimalSperator) > -1)
                                {
                                    // -- if the decimal is less then new decimal count
                                    Decimal = Math.Max(Decimal, DataValue.ToString().Length - DICommon.SetDecimalSperator(DataValue.ToString()).IndexOf(DICommon.DecimalSperator) - 1);
                                }
                            }
                        }
                    }
                    if (IUSList.Count > PreviousIUSCount)
                    {
                        // -- add the theme for the last indicator
                        // -- Set the color , if it is passed by hosting application
                        this.FrequencyThemes.Add(new Theme(this._DistinctIUS.Rows[this._DistinctIUS.Rows.Count - 1][IUSName].ToString(), this._DistinctIUS.Rows[this._DistinctIUS.Rows.Count - 1][Indicator_Unit_Subgroup.IUSNId].ToString(), Convert.ToDecimal(Min), Convert.ToDecimal(Max), Decimal, this.PresentationData, DefaultColorArray, this.FrequencyThemes, this._DistinctIUS.Rows.Count - 1, true));
                        PreviousIUSCount = IUSList.Count;
                    }
                    this._DistinctIUS.AcceptChanges();
                    this.PresentationData.RowFilter = string.Empty;
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region " -- Step 3 --"

        /// <summary>
        /// Generate the Classification values datatable. This Datatable contains the values against the selected IC.
        /// <remarks> Hosting application will set the order and selected crieteria. </remarks>
        /// </summary>
        public void GetIndicatorClassification(ICType icType)
        {
            DataTable ClassificationData = new DataTable("IndicatorClassification");
            DataRow Rows;
            int SortOrder = 0;

            // -- Create the structure of ClassificationData
            ClassificationData.Columns.Add(IndicatorClassifications.ICNId, typeof(int));
            ClassificationData.Columns.Add(IndicatorClassifications.ICGId);
            ClassificationData.Columns.Add(IndicatorClassifications.ICName);
            ClassificationData.Columns.Add("Selected", typeof(bool));
            ClassificationData.Columns.Add("Order", typeof(int));

            // -- Get the query to fetch the Ic value    
            string SqlQuery = DIQueries.IndicatorClassification.GetICTopParents(FilterFieldType.None, "", icType);

            IDataReader ClassificationReader;
            // -- Get the classification values
            ClassificationReader = DIConnection.ExecuteReader(SqlQuery);

            while (ClassificationReader.Read())
            {
                // -- fill the datatable with Ic values				
                Rows = ClassificationData.NewRow();
                Rows[IndicatorClassifications.ICNId] = ClassificationReader[IndicatorClassifications.ICNId];
                Rows[IndicatorClassifications.ICGId] = ClassificationReader[IndicatorClassifications.ICGId];
                Rows[IndicatorClassifications.ICName] = ClassificationReader[IndicatorClassifications.ICName];
                Rows["Selected"] = true;
                Rows["Order"] = SortOrder;
                ClassificationData.Rows.Add(Rows);
                SortOrder += 1;
            }
            this.ClassificationValuesTable = ClassificationData;
            ClassificationReader.Close();
            ClassificationReader.Dispose();
        }



        #endregion

        #region " -- Save / Load -- "

        /// <summary>
        /// Save the TablePresentation in form of XML file. Conversion of NId TO GId is covered under this method.
        /// </summary>
        /// <param name="fileNameWPath">File path of Serialized xml</param>
        /// <param name="updateGId">True, Conversion of NId to GId</param>
        public void Save(string fileNameWPath, bool updateGId)
        {
            if (updateGId)
            {
                if (DIConnection != null && DIQueries != null)
                {
                    this._UserPreference.UserSelection.UpdateGIdsFromNIds(DIConnection, DIQueries);
                }
            }
            XmlSerializer TableSerialize = new XmlSerializer(typeof(TablePresentation));
            StreamWriter TableWriter = new StreamWriter(fileNameWPath);
            TableSerialize.Serialize(TableWriter, this);
            TableWriter.Close();
        }

        /// <summary>
        /// Get the serialized text using MemoryStream.
        /// </summary>
        /// <param name="updateGId">True, Conversion of NId to GId</param>
        /// <remarks>http://www.dotnetjohn.com/PrintFriend.aspx?articleid=173</remarks>
        /// <returns>serialized string</returns>
        public string GetSerializedText(bool updateGId)
        {
            string RetVal = string.Empty;
            try
            {
                if (updateGId)
                {
                    this._UserPreference.UserSelection.UpdateGIdsFromNIds(DIConnection, DIQueries);
                }
                XmlSerializer TableSerialize = new XmlSerializer(typeof(TablePresentation));
                MemoryStream MemoryStream = new MemoryStream();
                //TODO UTF8 reason
                XmlTextWriter xmlTextWriter = new XmlTextWriter(MemoryStream, Encoding.UTF8);
                TableSerialize.Serialize(xmlTextWriter, this);
                MemoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                RetVal = UTF8ByteArrayToString(MemoryStream.ToArray());

                MemoryStream.Dispose();
            }
            catch (Exception ex)
            {
                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        /// Load the deserialize XML text
        /// </summary>
        /// <param name="SerializeText">Serialize Text</param>
        /// <returns>Object of Table Presentation</returns>
        /// <remarks>This overloaded method does not update NIds from GIds. 
        /// If you want to update NIds from GIds, use LoadFromSerializeText method with three parameter.</remarks>
        public static TablePresentation LoadFromSerializeText(string SerializeText)
        {
            TablePresentation RetVal;
            try
            {
                XmlSerializer TableSerialize = new XmlSerializer(typeof(TablePresentation));
                MemoryStream MemoryStream = new MemoryStream(StringToUTF8ByteArray(SerializeText));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(MemoryStream, Encoding.UTF8);
                RetVal = (TablePresentation)TableSerialize.Deserialize(MemoryStream);

                MemoryStream.Dispose();
            }
            catch (Exception ex)
            {
                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        /// Load the deserialize XML text
        /// </summary>
        /// <param name="SerializeText">Serialize Text</param>
        /// <param name="updateNId">True, Update NId from GId</param>
        /// <param name="DIConnection">DIConnection</param>
        /// <param name="DIQueries">DIQueries</param>
        /// <returns>Object of Table Presentation</returns>
        /// <remarks>This overloaded method implicetly update NIds from GIds. 
        /// If you do not want to update NIds from GIds, use LoadFromSerializeText method with one parameter.</remarks>
        public static TablePresentation LoadFromSerializeText(string SerializeText, DIConnection DIConnection, DIQueries DIQueries)
        {
            TablePresentation RetVal;
            try
            {
                XmlSerializer TableSerialize = new XmlSerializer(typeof(TablePresentation));
                MemoryStream MemoryStream = new MemoryStream(StringToUTF8ByteArray(SerializeText));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(MemoryStream, Encoding.UTF8);
                RetVal = (TablePresentation)TableSerialize.Deserialize(MemoryStream);
                RetVal.UserPreference.UserSelection.UpdateNIdsFromGIds(DIConnection, DIQueries);

                MemoryStream.Dispose();
            }
            catch (Exception ex)
            {
                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        /// Intialize the legends.
        /// </summary>
        /// <param name="LegendColor"></param>
        /// <remarks>This method must be called after calling the IntializeTablePresentation() method</remarks>
        public void IntializeLegends(Color[] LegendColor)
        {
            if (this._AddColor)
            {
                this._Themes = new Themes();
                this.SetColorTheme(LegendColor);
            }
        }

        /// <summary>
        /// Load the deserialize XML file
        /// </summary>
        /// <param name="fileNameWPath">File path of Serialized xml</param>
        /// <returns>Object of Table Presentation</returns>
        /// <remarks> This overloaded method does not update NIds from GIds. 
        /// If you want to update NIds from GIds, use Load method with three parameter.
        /// Call IntializeTablePresentstion after loading the report </remarks>
        public static TablePresentation Load(string fileNameWPath)
        {
            TablePresentation RetVal;
            try
            {
                XmlSerializer TableSerialize = new XmlSerializer(typeof(TablePresentation));
                TextReader TableReader = new StreamReader(fileNameWPath);
                RetVal = (TablePresentation)TableSerialize.Deserialize(TableReader);
                TableReader.Close();
            }
            catch (Exception ex)
            {
                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        /// Load the deserialize XML file
        /// </summary>
        /// <param name="fileNameWPath">File path of Serialized xml</param>
        /// <param name="updateNId">True, Update NId from GId</param>
        /// <param name="DIConnection">DIConnection</param>
        /// <param name="DIQueries">DIQueries</param>
        /// <returns>Object of Table Presentation</returns>
        /// <remarks> This overloaded method implicetly update NIds from GIds. 
        /// If you do not want to update NIds from GIds, use Load method with one parameter.
        /// Call IntializeTablePresentstion after loading the report.
        /// </remarks>        
        public static TablePresentation Load(string fileNameWPath, DIConnection DIConnection, DIQueries DIQueries)
        {
            TablePresentation RetVal;
            try
            {
                XmlSerializer TableSerialize = new XmlSerializer(typeof(TablePresentation));
                TextReader TableReader = new StreamReader(fileNameWPath);
                RetVal = (TablePresentation)TableSerialize.Deserialize(TableReader);
                RetVal.UserPreference.UserSelection.UpdateNIdsFromGIds(DIConnection, DIQueries);
                TableReader.Close();
            }
            catch (Exception ex)
            {
                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        /// Update the Applycolor(Step 2), Indicator Classification(Step 3), ColumnArrangement(Step 3)
        /// </summary>
        /// <param name="dIConnection">DIConnection</param>
        /// <param name="dIQueries">DIQueries</param>
        /// <param name="LanguageFilePath">Obsolete not in use. May set string.empty for time being</param>
        /// <param name="maskFilePath"> Mask folder path</param>
        /// <returns></returns>
        /// <remarks>This method is called after calling the load method.</remarks>
        public bool IntializeTablePresentation(DIConnection dIConnection, DIQueries dIQueries, string LanguageFilePath, string maskFilePath)
        {
            bool RetVal = false;
            try
            {
                // -- Set the connection properties.
                this.DIConnection = dIConnection;
                this.DIQueries = dIQueries;

                // -- Get presentation data based on the updated user selection.
                DIDataView DIDataView = new DIDataView(this._UserPreference, dIConnection, dIQueries, maskFilePath, "");
                DIDataView.GenerateAllPages();

                // -- Call overloaded method 
                RetVal = this.IntializeTablePresentation(dIConnection, dIQueries, string.Empty, DIDataView);
            }
            catch (Exception ex)
            {
                RetVal = false;
            }
            return RetVal;
        }

        /// <summary>
        /// Update the Applycolor(Step 2), Indicator Classification(Step 3), ColumnArrangement(Step 3)
        /// </summary>
        /// <param name="dIConnection">DIConnection</param>
        /// <param name="dIQueries">DIQueries</param>
        /// <param name="LanguageFilePath">Obsolete not in use. May set string.empty for time being</param>
        /// <param name="maskFilePath"> Mask folder path</param>
        /// <param name="preserveTheme"> Preserve the theme colelction</param>
        /// <returns></returns>
        /// <remarks>This method is called after calling the load method.</remarks>
        public bool IntializeTablePresentation(DIConnection dIConnection, DIQueries dIQueries, string LanguageFilePath, string maskFilePath, bool preserveTheme)
        {
            bool RetVal = false;
            try
            {
                // -- Set the connection properties.
                this.DIConnection = dIConnection;
                this.DIQueries = dIQueries;

                // -- Get presentation data based on the updated user selection.
                DIDataView DIDataView = new DIDataView(this._UserPreference, dIConnection, dIQueries, maskFilePath, "");
                DIDataView.GenerateAllPages();

                this.PreserveTheme = preserveTheme;

                // -- Call overloaded method 
                RetVal = this.IntializeTablePresentation(dIConnection, dIQueries, string.Empty, DIDataView);
            }
            catch (Exception ex)
            {
                RetVal = false;
            }
            return RetVal;
        }

        /// <summary>
        /// Set the connection, query and presentation data object and register the events.
        /// </summary>
        /// <param name="dIConnection">DIConnection</param>
        /// <param name="dIQueries">DIQueries</param>
        /// <param name="LanguageFilePath">Obsolete not in use. May set string.empty for time being</param>
        /// <param name="dIDataView">DIdataview instance</param>
        /// <remarks>This method is called after calling the load method, only if we have presnetation data.</remarks>
        public bool IntializeTablePresentation(DIConnection dIConnection, DIQueries dIQueries, string LanguageFilePath, DIDataView dIDataView)
        {
            bool RetVal = false;
            // -- Set the connection properties.
            this.DIConnection = dIConnection;
            this.DIQueries = dIQueries;

            //-- Set the instance of DIDataView class.            
            this.DIDataView = dIDataView;

            //-- Apply filters to set filter conditions
            this.DIDataView.ApplyFilters();

            // -- Get presentation data based on the updated user selection.
            this.PresentationData = dIDataView.GetPresentationData();

            if (this.PresentationData != null && this.PresentationData.Count > 0)
            {
                //-- Set the theme presentation data and theme object
                for (int ThemeIndex = 0; ThemeIndex < this.Themes.Count; ThemeIndex++)
                {
                    this.Themes.IntializeTheme(this.PresentationData, this.Themes, ThemeIndex);
                }

                if (this.FrequencyThemes != null)
                {
                    //-- Set the frequency theme presentation data and theme object
                    for (int ThemeIndex = 0; ThemeIndex < this.FrequencyThemes.Count; ThemeIndex++)
                    {
                        this.FrequencyThemes.IntializeTheme(this.PresentationData, this.FrequencyThemes, ThemeIndex);
                    }
                }

                this._PresentationDataRowCount = this.PresentationData.Count;
                // -- Register the event.
                this._Fields.ChangeAggregateFieldEvent += new ChangeAggregateFieldDelegate(_Fields_ChangeAggregateFieldEvent);
                this._Fields.UpdateColumnAggangementEvent += new UpdateColumnAggangementDelegate(_Fields_UpdateColumnAggangementEvent);

                // -- Update the distinct indicator datatable.
                this.UpdateApplyColorIndicator();

                // -- Update the distinct IUS datatable.
                this.UpdateApplyColorForIUS();

                // -- Intialize the IC level
                if (this._IndicatorClassificationLevel == -1)
                {
                    this.SelectedICLevel = 10;
                }
                else
                {
                    this.SelectedICLevel = this._IndicatorClassificationLevel;
                }

                this.CopyICSelectedNodes();

                // -- Update indicator classification
                this.UpdateIndicatorClassification(dIConnection, dIQueries);

                // -- Update Column arrangement.
                this.UpdateColumnArrangement();

                // -- Set the Language key values based on Language file selected.
                this.ApplyLanguageSettings();

                //-- Update the column arrangement table, This will prevent "no record found" message at the time of replication.
                if (this._ColumnArrangementTable.Rows.Count == 0)
                {
                    this._ColumnArrangementTable = this.SetColumnArrangement(false);
                }

                if (this._ColumnArrangementTable.Rows.Count > 0)
                {
                    RetVal = true;
                }
                else
                {
                    // -- If at any case, Column arrangement row count is 0.
                    RetVal = false;
                }

            }
            return RetVal;
        }

        /// <summary>
        /// Intialize the legends
        /// </summary>
        /// <returns></returns>
        public bool IntializeLegends()
        {
            bool Retval = true;
            try
            {
                if (this.PresentationData != null)
                {
                    //-- Set the theme presentation data and theme object
                    for (int ThemeIndex = 0; ThemeIndex < this.Themes.Count; ThemeIndex++)
                    {
                        this.Themes.IntializeTheme(this.PresentationData, this.Themes, ThemeIndex);
                    }
                }
                else
                {
                    Retval = false;
                }
            }
            catch (Exception)
            {
                Retval = false;
            }
            return Retval;
        }

        /// <summary>
        /// Intialize the frequency legends
        /// </summary>
        /// <returns></returns>
        public bool IntializeFrequencyLegends()
        {
            bool Retval = true;
            try
            {
                if (this.PresentationData != null)
                {
                    //-- Set the theme presentation data and theme object
                    for (int ThemeIndex = 0; ThemeIndex < this.FrequencyThemes.Count; ThemeIndex++)
                    {
                        this.FrequencyThemes.IntializeTheme(this.PresentationData, this.FrequencyThemes, ThemeIndex);
                    }
                }
                else
                {
                    Retval = false;
                }
            }
            catch (Exception)
            {
                Retval = false;
            }
            return Retval;
        }

        #endregion

        #region "-- Frequency Table Chart Image --"

        /// <summary>
        /// Get the chart image
        /// </summary>
        /// <returns></returns>
        public static Image GenerateHistogramImage(string fileNameWPath, int height, int width)
        {
            Image Retval = null;
            SpreadsheetGear.Windows.Forms.WorkbookView moSpreadsheet = new SpreadsheetGear.Windows.Forms.WorkbookView();
            try
            {

                moSpreadsheet.GetLock();
                moSpreadsheet.ActiveWorkbook = SpreadsheetGear.Factory.GetWorkbook(fileNameWPath, System.Globalization.CultureInfo.CurrentCulture);
                moSpreadsheet.ActiveWorkbook.DisplayDrawingObjects = SpreadsheetGear.DisplayDrawingObjects.DisplayShapes;


                SpreadsheetGear.Shapes.IShapes ExcelShapes = moSpreadsheet.ActiveWorksheet.Shapes;
                SpreadsheetGear.Drawing.Image SpreadsheetImage = new SpreadsheetGear.Drawing.Image(ExcelShapes[0]);

                Bitmap ExcelBitmap = SpreadsheetImage.GetBitmap(System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                //-- Set the aspect ratio of the image
                SetAspectRatio(ExcelBitmap, height, width);

                Size NewSize = DICommon.GetImageSizeByAspectRatio(Convert.ToInt32(ExcelShapes[0].Width), Convert.ToInt32(ExcelShapes[0].Height), width, height, true);

                //-- Resize the Image
                Retval = new Bitmap(NewSize.Width, NewSize.Height);
                //Retval = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(Retval);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage((Image)ExcelBitmap, 0, 0, NewSize.Width, NewSize.Height);
                g.Dispose();
            }
            catch (Exception)
            {
            }
            finally
            {
                moSpreadsheet.ReleaseLock();
            }
            return Retval;
        }

        /// <summary>
        /// Get the theme chart image
        /// </summary>
        /// <returns></returns>
        public static Image GenerateHistogramImage(string fileNameWPath, int height, int width, int themeIndex)
        {
            Image Retval = null;
            SpreadsheetGear.Windows.Forms.WorkbookView moSpreadsheet = new SpreadsheetGear.Windows.Forms.WorkbookView();
            try
            {

                moSpreadsheet.GetLock();
                moSpreadsheet.ActiveWorkbook = SpreadsheetGear.Factory.GetWorkbook(fileNameWPath, System.Globalization.CultureInfo.CurrentCulture);
                moSpreadsheet.ActiveWorkbook.DisplayDrawingObjects = SpreadsheetGear.DisplayDrawingObjects.DisplayShapes;


                SpreadsheetGear.Shapes.IShapes ExcelShapes = moSpreadsheet.ActiveWorksheet.Shapes;
                SpreadsheetGear.Drawing.Image SpreadsheetImage = new SpreadsheetGear.Drawing.Image(ExcelShapes[themeIndex]);

                Bitmap ExcelBitmap = SpreadsheetImage.GetBitmap(System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                //-- Set the aspect ratio of the image
                SetAspectRatio(ExcelBitmap, height, width);

                //-- Resize the Image
                Retval = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(Retval);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage((Image)ExcelBitmap, 0, 0, width, height);
                g.Dispose();
            }
            catch (Exception)
            {
            }
            finally
            {
                moSpreadsheet.ReleaseLock();
            }
            return Retval;
        }

        /// <summary>
        /// Set the aspect ratio of the image
        /// </summary>
        /// <param name="ExcelBitmap"></param>
        private static void SetAspectRatio(Bitmap ExcelBitmap, int height, int width)
        {
            //-- Set the aspect ratio.
            Single ImageAspectRatio = 1;

            if (height < ExcelBitmap.Height && width < ExcelBitmap.Width)
            {
                ImageAspectRatio = Convert.ToSingle(ExcelBitmap.Width) / Convert.ToSingle(ExcelBitmap.Height);
                if (ImageAspectRatio > 1)
                {
                    width = Convert.ToInt32(Convert.ToSingle(width) / ImageAspectRatio);
                }
                else
                {
                    height = Convert.ToInt32(Convert.ToSingle(height) * ImageAspectRatio);
                }
            }
            else
            {
                width = ExcelBitmap.Width;
                height = ExcelBitmap.Height;
            }
        }

        #endregion

        #endregion

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            TablePresentation RetVal;
            try
            {
                XmlSerializer XmlSerializer = new XmlSerializer(typeof(TablePresentation));
                MemoryStream MemoryStream = new MemoryStream();
                XmlSerializer.Serialize(MemoryStream, this);
                MemoryStream.Position = 0;
                RetVal = (TablePresentation)XmlSerializer.Deserialize(MemoryStream);
                MemoryStream.Close();
                MemoryStream.Dispose();
            }
            catch (Exception)
            {
                RetVal = null;
            }
            return RetVal;
        }

        #endregion

        #region " -- Object Comparision -- "

        /// <summary>
        /// GetHashCode is necessary to impement with equals method.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return 0;
        }

        /// <summary>
        /// Compare the TablePresentation object with the temp object.
        /// </summary>
        /// <param name="tempObj"></param>
        /// <returns></returns>
        public override bool Equals(System.Object tempObj)
        {
            try
            {
                TablePresentation TempTablePresentation = (TablePresentation)tempObj;

                int Index = 0;


                //-- Step 1 

                //-- Graph wizard
                if (this._ChartType != TempTablePresentation.ChartType)
                {
                    return false;
                }

                // -- Rows Comparision 
                if (this._Fields.Rows.Count != TempTablePresentation.Fields.Rows.Count)
                {
                    return false;
                }
                else
                {
                    for (Index = 0; Index < this._Fields.Rows.Count; Index++)
                    {
                        if (this._Fields.Rows[Index].FieldID != TempTablePresentation.Fields.Rows[Index].FieldID)
                        {
                            return false;
                        }
                    }
                }


                //-- Columns Comparision 
                if (this._Fields.Columns.Count != TempTablePresentation.Fields.Columns.Count)
                {
                    return false;
                }
                else
                {
                    for (Index = 0; Index < this._Fields.Columns.Count; Index++)
                    {
                        if (this._Fields.Columns[Index].FieldID != TempTablePresentation.Fields.Columns[Index].FieldID)
                        {
                            return false;
                        }
                    }
                }

                //-- Aggregation 
                if (this._AggregateOn != TempTablePresentation.AggregateOn)
                {
                    return false;
                }

                //-- Aggregate function
                if (this._LstAggregateFunction.Count != TempTablePresentation.LstAggregateFunction.Count)
                {
                    return false;
                }
                else
                {
                    if (this._LstAggregateFunction.Count > 0)
                    {
                        foreach (AggregateType AggType in this._LstAggregateFunction)
                        {
                            if (TempTablePresentation._LstAggregateFunction.Count > 0 && TempTablePresentation._LstAggregateFunction.IndexOf(AggType) == -1)
                            {
                                return false;
                            }
                        }
                    }
                }

                if (this._AggregateFunction != TempTablePresentation.AggregateFunction)
                {
                    return false;
                }

                if (this._AggregateFieldID != TempTablePresentation.AggregateFieldID)
                {
                    return false;
                }

                //-- supress duplicate 
                if (this._IsSuppressDuplicateColumns != TempTablePresentation.IsSuppressDuplicateColumns)
                {
                    return false;
                }

                if (this._IsSuppressDuplicateRows != TempTablePresentation.IsSuppressDuplicateRows)
                {
                    return false;
                }


                // -- STEP 2 

                //-- Title 
                if (this._Title.ToLower() != TempTablePresentation.Title.ToLower())
                {
                    return false;
                }

                //-- Sub title 
                if (this._Subtitle.ToLower() != TempTablePresentation.Subtitle.ToLower())
                {
                    return false;
                }

                //-- Footnotes 
                if (this._TemplateStyle.Footnotes.Show != TempTablePresentation.TemplateStyle.Footnotes.Show)
                {
                    return false;
                }

                if (this._TemplateStyle.Footnotes.FontTemplate.TableFootnoteInLine != TempTablePresentation.TemplateStyle.Footnotes.FontTemplate.TableFootnoteInLine)
                {
                    return false;
                }

                if (this._TemplateStyle.Footnotes.FontTemplate.GraphFootnoteInLine != TempTablePresentation.TemplateStyle.Footnotes.FontTemplate.GraphFootnoteInLine)
                {
                    return false;
                }

                //-- Comments 
                if (this._TemplateStyle.Comments.Show != TempTablePresentation.TemplateStyle.Comments.Show)
                {
                    return false;
                }

                if (this._TemplateStyle.Comments.FontTemplate.Inline != TempTablePresentation.TemplateStyle.Comments.FontTemplate.Inline)
                {
                    return false;
                }

                //-- Denominator 
                if (this.TemplateStyle.Denominator.Show != TempTablePresentation._TemplateStyle.Denominator.Show)
                {
                    return false;
                }

                //-- Apply color 
                if (this._AddColor != TempTablePresentation.AddColor)
                {
                    return false;
                }

                if (this._AddColor && TempTablePresentation.AddColor)
                {
                    if (this._DistinctIndicator.Rows.Count != TempTablePresentation.DistinctIndicator.Rows.Count)
                    {
                        return false;
                    }

                    for (Index = 0; Index < this._DistinctIndicator.Rows.Count; Index++)
                    {
                        //-- Break Count 
                        if (this._Themes[Index].BreakCount != TempTablePresentation.Themes[Index].BreakCount)
                        {
                            return false;
                        }

                        //-- Break Type 
                        if (this._Themes[Index].BreakType != TempTablePresentation.Themes[Index].BreakType)
                        {
                            return false;
                        }

                        //-- Minimum value 
                        if (this._Themes[Index].Minimum != TempTablePresentation.Themes[Index].Minimum)
                        {
                            return false;
                        }

                        //-- Maximum value 
                        if (this._Themes[Index].Maximum != TempTablePresentation.Themes[Index].Maximum)
                        {
                            return false;
                        }

                        //-- Decimal places 
                        if (this._Themes[Index].Decimal != TempTablePresentation.Themes[Index].Decimal)
                        {
                            return false;
                        }

                        //-- High is good 
                        if (this._Themes[Index].HighIsGood != TempTablePresentation.Themes[Index].HighIsGood)
                        {
                            return false;
                        }

                        //-- Legends 
                        for (int LegendIndex = 0; LegendIndex < this._Themes[Index].Legends.Count; LegendIndex++)
                        {
                            //-- Caption 
                            if (this._Themes[Index].Legends[LegendIndex].Caption != TempTablePresentation.Themes[Index].Legends[LegendIndex].Caption)
                            {
                                return false;
                            }

                            //-- From Value 
                            if (this._Themes[Index].Legends[LegendIndex].RangeFrom != TempTablePresentation.Themes[Index].Legends[LegendIndex].RangeFrom)
                            {
                                return false;
                            }

                            //-- to Value 
                            if (this._Themes[Index].Legends[LegendIndex].RangeTo != TempTablePresentation.Themes[Index].Legends[LegendIndex].RangeTo)
                            {
                                return false;
                            }

                            //-- Count 
                            if (this._Themes[Index].Legends[LegendIndex].Count != TempTablePresentation.Themes[Index].Legends[LegendIndex].Count)
                            {
                                return false;
                            }

                            //-- color 
                            if (this._Themes[Index].Legends[LegendIndex].Color != TempTablePresentation.Themes[Index].Legends[LegendIndex].Color)
                            {
                                return false;
                            }
                        }
                    }
                }

                //' -- STEP 3 (column Arranhement and IC) 

                //-- IC On / off 
                if (this._SelectedIndicatorClassification != TempTablePresentation.SelectedIndicatorClassification)
                {
                    return false;
                }

                if (this._SelectedIndicatorClassification && TempTablePresentation.SelectedIndicatorClassification)
                {
                    //-- IC Level 
                    if (this._IndicatorClassificationLevel != TempTablePresentation.IndicatorClassificationLevel)
                    {
                        return false;
                    }

                    //-- IC Type 
                    if (this._IndicatorClassification != TempTablePresentation.IndicatorClassification)
                    {
                        return false;
                    }

                    //-- Selected IC NODE count
                    if (this._SelectedNodes.Count != TempTablePresentation.SelectedNodes.Count)
                    {
                        return false;
                    }

                    //-- Selected IC NODE 
                    for (int i = 0; i < this._SelectedNodes.Count; i++)
                    {
                        if (this._SelectedNodes[i].NId != TempTablePresentation.SelectedNodes[i].NId)
                        {
                            return false;
                        }
                    }

                    //-- IC selected value count 
                    if (this._ClassificationValuesTable.Rows.Count != TempTablePresentation.ClassificationValuesTable.Rows.Count)
                    {
                        return false;
                    }

                    //-- IC table
                    for (Index = 0; Index < this._ClassificationValuesTable.Rows.Count; Index++)
                    {
                        //-- ICNId 
                        if (this._ClassificationValuesTable.Rows[Index][IndicatorClassifications.ICNId].ToString() != TempTablePresentation.ClassificationValuesTable.Rows[Index][IndicatorClassifications.ICNId].ToString())
                        {
                            return false;
                        }

                        //-- ICGId 
                        if (this._ClassificationValuesTable.Rows[Index][IndicatorClassifications.ICGId].ToString() != TempTablePresentation.ClassificationValuesTable.Rows[Index][IndicatorClassifications.ICGId].ToString())
                        {
                            return false;
                        }

                        //-- IC Name 
                        if (this._ClassificationValuesTable.Rows[Index][IndicatorClassifications.ICName].ToString() != TempTablePresentation.ClassificationValuesTable.Rows[Index][IndicatorClassifications.ICName].ToString())
                        {
                            return false;
                        }

                        //-- selected 
                        if (Convert.ToBoolean(this._ClassificationValuesTable.Rows[Index]["Selected"]) != Convert.ToBoolean(TempTablePresentation.ClassificationValuesTable.Rows[Index]["Selected"]))
                        {
                            return false;
                        }

                        //-- Order 
                        if (Convert.ToInt32(this._ClassificationValuesTable.Rows[Index]["Order"]) != Convert.ToInt32(TempTablePresentation.ClassificationValuesTable.Rows[Index]["Order"]))
                        {
                            return false;
                        }
                    }
                }

                if (this._SelectedNodes.Count != TempTablePresentation.SelectedNodes.Count)
                {
                    return false;
                }
                else
                {
                    for (int i = 0; i < this._SelectedNodes.Count; i++)
                    {
                        if (this._SelectedNodes[i].NId != TempTablePresentation.SelectedNodes[i].NId)
                        {
                            return false;
                            break;
                        }
                    }
                }

                //-- Column Arranegment 
                if (this._SelectedColumnArrangement != TempTablePresentation.SelectedColumnArrangement)
                {
                    return false;
                }

                if (this._SelectedColumnArrangement && TempTablePresentation.SelectedColumnArrangement)
                {
                    //-- Row count 
                    if (this._ColumnArrangementTable.Rows.Count != TempTablePresentation.ColumnArrangementTable.Rows.Count)
                    {
                        return false;
                    }

                    //-- Column count 
                    if (this._ColumnArrangementTable.Columns.Count != TempTablePresentation.ColumnArrangementTable.Columns.Count)
                    {
                        return false;
                    }

                    //-- Compare the value of column arrangement. 
                    for (Index = 0; Index < this._ColumnArrangementTable.Rows.Count; Index++)
                    {
                        for (int ColumnIndex = 0; ColumnIndex < this._ColumnArrangementTable.Columns.Count; ColumnIndex++)
                        {
                            if (this._ColumnArrangementTable.Rows[Index][ColumnIndex].ToString() != TempTablePresentation.ColumnArrangementTable.Rows[Index][ColumnIndex].ToString())
                            {
                                return false;
                            }
                        }
                    }
                }

                // -- STEP 4 (SORTING) 
                if (this._Fields.Sort.Count != TempTablePresentation.Fields.Sort.Count)
                {
                    return false;
                }

                for (Index = 0; Index < this._Fields.Sort.Count; Index++)
                {
                    //-- Field ID 
                    if (this._Fields.Sort[Index].FieldID != TempTablePresentation.Fields.Sort[Index].FieldID)
                    {
                        return false;
                    }

                    //-- sort Order. 
                    if (this._Fields.Sort[Index].SortType != TempTablePresentation.Fields.Sort[Index].SortType)
                    {
                        return false;
                    }
                }

                // -- STEP 5 (FORMAT) 

                //-- Template file name 
                if (this._TemplateFileName != TempTablePresentation.TemplateFileName)
                {
                    return false;
                }

                //-- Border Lines 
                if (this._TemplateStyle.ShowBorderLines != TempTablePresentation.TemplateStyle.ShowBorderLines)
                {
                    return false;
                }

                //-- Title Settings 
                if (this._TemplateStyle.TitleSetting.FontTemplate.FontName != TempTablePresentation.TemplateStyle.TitleSetting.FontTemplate.FontName)
                {
                    return false;
                }

                if (this._TemplateStyle.TitleSetting.FontTemplate.FontStyle != TempTablePresentation.TemplateStyle.TitleSetting.FontTemplate.FontStyle)
                {
                    return false;
                }

                if (this._TemplateStyle.TitleSetting.FontTemplate.FontSize != TempTablePresentation.TemplateStyle.TitleSetting.FontTemplate.FontSize)
                {
                    return false;
                }

                if (this._TemplateStyle.TitleSetting.FontTemplate.BackColor != TempTablePresentation.TemplateStyle.TitleSetting.FontTemplate.BackColor)
                {
                    return false;
                }

                if (this._TemplateStyle.TitleSetting.FontTemplate.ForeColor != TempTablePresentation.TemplateStyle.TitleSetting.FontTemplate.ForeColor)
                {
                    return false;
                }

                if (this._TemplateStyle.TitleSetting.FontTemplate.TextAlignment != TempTablePresentation.TemplateStyle.TitleSetting.FontTemplate.TextAlignment)
                {
                    return false;
                }

                //-- Sub Title Settings 
                if (this._TemplateStyle.SubTitleSetting.FontTemplate.FontName != TempTablePresentation.TemplateStyle.SubTitleSetting.FontTemplate.FontName)
                {
                    return false;
                }

                if (this._TemplateStyle.SubTitleSetting.FontTemplate.FontStyle != TempTablePresentation.TemplateStyle.SubTitleSetting.FontTemplate.FontStyle)
                {
                    return false;
                }

                if (this._TemplateStyle.SubTitleSetting.FontTemplate.FontSize != TempTablePresentation.TemplateStyle.SubTitleSetting.FontTemplate.FontSize)
                {
                    return false;
                }

                if (this._TemplateStyle.SubTitleSetting.FontTemplate.BackColor != TempTablePresentation.TemplateStyle.SubTitleSetting.FontTemplate.BackColor)
                {
                    return false;
                }

                if (this._TemplateStyle.SubTitleSetting.FontTemplate.ForeColor != TempTablePresentation.TemplateStyle.SubTitleSetting.FontTemplate.ForeColor)
                {
                    return false;
                }

                if (this._TemplateStyle.SubTitleSetting.FontTemplate.TextAlignment != TempTablePresentation.TemplateStyle.SubTitleSetting.FontTemplate.TextAlignment)
                {
                    return false;
                }


                //-- Column Settings 
                if (this._TemplateStyle.ColumnSetting.FontTemplate.FontName != TempTablePresentation.TemplateStyle.ColumnSetting.FontTemplate.FontName)
                {
                    return false;
                }

                if (this._TemplateStyle.ColumnSetting.FontTemplate.FontStyle != TempTablePresentation.TemplateStyle.ColumnSetting.FontTemplate.FontStyle)
                {
                    return false;
                }

                if (this._TemplateStyle.ColumnSetting.FontTemplate.FontSize != TempTablePresentation.TemplateStyle.ColumnSetting.FontTemplate.FontSize)
                {
                    return false;
                }

                if (this._TemplateStyle.ColumnSetting.FontTemplate.BackColor != TempTablePresentation.TemplateStyle.ColumnSetting.FontTemplate.BackColor)
                {
                    return false;
                }

                if (this._TemplateStyle.ColumnSetting.FontTemplate.ForeColor != TempTablePresentation.TemplateStyle.ColumnSetting.FontTemplate.ForeColor)
                {
                    return false;
                }

                if (this._TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth != TempTablePresentation.TemplateStyle.ColumnSetting.FontTemplate.ColumnWidth)
                {
                    return false;
                }


                //-- Row Settings 
                if (this._TemplateStyle.RowSetting.FontTemplate.FontName != TempTablePresentation.TemplateStyle.RowSetting.FontTemplate.FontName)
                {
                    return false;
                }

                if (this._TemplateStyle.RowSetting.FontTemplate.Show != TempTablePresentation.TemplateStyle.RowSetting.FontTemplate.Show)
                {
                    return false;
                }

                if (this._TemplateStyle.RowSetting.FontTemplate.FontStyle != TempTablePresentation.TemplateStyle.RowSetting.FontTemplate.FontStyle)
                {
                    return false;
                }

                if (this._TemplateStyle.RowSetting.FontTemplate.FontSize != TempTablePresentation.TemplateStyle.RowSetting.FontTemplate.FontSize)
                {
                    return false;
                }

                if (this._TemplateStyle.RowSetting.FontTemplate.ShowBackColor != TempTablePresentation.TemplateStyle.RowSetting.FontTemplate.ShowBackColor)
                {
                    return false;
                }

                if (this._TemplateStyle.RowSetting.FontTemplate.BackColor != TempTablePresentation.TemplateStyle.RowSetting.FontTemplate.BackColor)
                {
                    return false;
                }

                if (this._TemplateStyle.RowSetting.FontTemplate.ForeColor != TempTablePresentation.TemplateStyle.RowSetting.FontTemplate.ForeColor)
                {
                    return false;
                }

                if (this._TemplateStyle.RowSetting.FontTemplate.ShowAlternateColor != TempTablePresentation.TemplateStyle.RowSetting.FontTemplate.ShowAlternateColor)
                {
                    return false;
                }

                if (this._TemplateStyle.RowSetting.FontTemplate.AlternateBackColor1 != TempTablePresentation.TemplateStyle.RowSetting.FontTemplate.AlternateBackColor1)
                {
                    return false;
                }

                if (this._TemplateStyle.RowSetting.FontTemplate.AlternateBackColor2 != TempTablePresentation.TemplateStyle.RowSetting.FontTemplate.AlternateBackColor2)
                {
                    return false;
                }

                if (this._TemplateStyle.RowSetting.FontTemplate.TextAlignment != TempTablePresentation.TemplateStyle.RowSetting.FontTemplate.TextAlignment)
                {
                    return false;
                }

                if (this._TemplateStyle.RowSetting.FontTemplate.WordWrap != TempTablePresentation.TemplateStyle.RowSetting.FontTemplate.WordWrap)
                {
                    return false;
                }

                //-- Content Settings 
                if (this._TemplateStyle.ContentSetting.FontTemplate.FontName != TempTablePresentation.TemplateStyle.ContentSetting.FontTemplate.FontName)
                {
                    return false;
                }

                if (this._TemplateStyle.ContentSetting.FontTemplate.FontStyle != TempTablePresentation.TemplateStyle.ContentSetting.FontTemplate.FontStyle)
                {
                    return false;
                }

                if (this._TemplateStyle.ContentSetting.FontTemplate.FontSize != TempTablePresentation.TemplateStyle.ContentSetting.FontTemplate.FontSize)
                {
                    return false;
                }

                if (this._TemplateStyle.ContentSetting.FontTemplate.ShowBackColor != TempTablePresentation.TemplateStyle.ContentSetting.FontTemplate.ShowBackColor)
                {
                    return false;
                }

                if (this._TemplateStyle.ContentSetting.FontTemplate.BackColor != TempTablePresentation.TemplateStyle.ContentSetting.FontTemplate.BackColor)
                {
                    return false;
                }

                if (this._TemplateStyle.ContentSetting.FontTemplate.ForeColor != TempTablePresentation.TemplateStyle.ContentSetting.FontTemplate.ForeColor)
                {
                    return false;
                }

                if (this._TemplateStyle.ContentSetting.FontTemplate.TextAlignment != TempTablePresentation.TemplateStyle.ContentSetting.FontTemplate.TextAlignment)
                {
                    return false;
                }

                if (this._TemplateStyle.ContentSetting.FontTemplate.FormatDataValue != TempTablePresentation.TemplateStyle.ContentSetting.FontTemplate.FormatDataValue)
                {
                    return false;
                }

                if (this._TemplateStyle.ContentSetting.FontTemplate.RoundDataValues != TempTablePresentation.TemplateStyle.ContentSetting.FontTemplate.RoundDataValues)
                {
                    return false;
                }

                if (this._TemplateStyle.ContentSetting.FontTemplate.DecimalPlace != TempTablePresentation.TemplateStyle.ContentSetting.FontTemplate.DecimalPlace)
                {
                    return false;
                }

                //-- Sub Aggregate Settings 
                if (this._TemplateStyle.SubAggregateSetting.FontTemplate.FontName != TempTablePresentation.TemplateStyle.SubAggregateSetting.FontTemplate.FontName)
                {
                    return false;
                }

                if (this._TemplateStyle.SubAggregateSetting.FontTemplate.FontStyle != TempTablePresentation.TemplateStyle.SubAggregateSetting.FontTemplate.FontStyle)
                {
                    return false;
                }

                if (this._TemplateStyle.SubAggregateSetting.FontTemplate.FontSize != TempTablePresentation.TemplateStyle.SubAggregateSetting.FontTemplate.FontSize)
                {
                    return false;
                }

                if (this._TemplateStyle.SubAggregateSetting.FontTemplate.BackColor != TempTablePresentation.TemplateStyle.SubAggregateSetting.FontTemplate.BackColor)
                {
                    return false;
                }

                if (this._TemplateStyle.SubAggregateSetting.FontTemplate.ForeColor != TempTablePresentation.TemplateStyle.SubAggregateSetting.FontTemplate.ForeColor)
                {
                    return false;
                }

                if (this._TemplateStyle.SubAggregateSetting.AggregateAreaByParent != TempTablePresentation.TemplateStyle.SubAggregateSetting.AggregateAreaByParent)
                {
                    return false;
                }

                //-- Group Aggregate Settings 
                if (this._TemplateStyle.GroupAggregateSetting.FontTemplate.FontName != TempTablePresentation.TemplateStyle.GroupAggregateSetting.FontTemplate.FontName)
                {
                    return false;
                }

                if (this._TemplateStyle.GroupAggregateSetting.FontTemplate.FontStyle != TempTablePresentation.TemplateStyle.GroupAggregateSetting.FontTemplate.FontStyle)
                {
                    return false;
                }

                if (this._TemplateStyle.GroupAggregateSetting.FontTemplate.FontSize != TempTablePresentation.TemplateStyle.GroupAggregateSetting.FontTemplate.FontSize)
                {
                    return false;
                }

                if (this._TemplateStyle.GroupAggregateSetting.FontTemplate.BackColor != TempTablePresentation.TemplateStyle.GroupAggregateSetting.FontTemplate.BackColor)
                {
                    return false;
                }

                if (this._TemplateStyle.GroupAggregateSetting.FontTemplate.ForeColor != TempTablePresentation.TemplateStyle.GroupAggregateSetting.FontTemplate.ForeColor)
                {
                    return false;
                }

                //-- Group Header Settings 
                if (this._TemplateStyle.GroupHeaderSetting.FontTemplate.FontName != TempTablePresentation.TemplateStyle.GroupHeaderSetting.FontTemplate.FontName)
                {
                    return false;
                }

                if (this._TemplateStyle.GroupHeaderSetting.FontTemplate.FontStyle != TempTablePresentation.TemplateStyle.GroupHeaderSetting.FontTemplate.FontStyle)
                {
                    return false;
                }

                if (this._TemplateStyle.GroupHeaderSetting.FontTemplate.FontSize != TempTablePresentation.TemplateStyle.GroupHeaderSetting.FontTemplate.FontSize)
                {
                    return false;
                }

                if (this._TemplateStyle.GroupHeaderSetting.FontTemplate.BackColor != TempTablePresentation.TemplateStyle.GroupHeaderSetting.FontTemplate.BackColor)
                {
                    return false;
                }

                if (this._TemplateStyle.GroupHeaderSetting.FontTemplate.ForeColor != TempTablePresentation.TemplateStyle.GroupHeaderSetting.FontTemplate.ForeColor)
                {
                    return false;
                }

                //-- Legends 
                if (this._TemplateStyle.Legends.FontTemplate.FontName != TempTablePresentation.TemplateStyle.Legends.FontTemplate.FontName)
                {
                    return false;
                }

                if (this._TemplateStyle.Legends.FontTemplate.FontStyle != TempTablePresentation.TemplateStyle.Legends.FontTemplate.FontStyle)
                {
                    return false;
                }

                if (this._TemplateStyle.Legends.FontTemplate.FontSize != TempTablePresentation.TemplateStyle.Legends.FontTemplate.FontSize)
                {
                    return false;
                }

                if (this._TemplateStyle.Legends.FontTemplate.BackColor != TempTablePresentation.TemplateStyle.Legends.FontTemplate.BackColor)
                {
                    return false;
                }

                if (this._TemplateStyle.Legends.FontTemplate.ForeColor != TempTablePresentation.TemplateStyle.Legends.FontTemplate.ForeColor)
                {
                    return false;
                }

                if (this._TemplateStyle.Legends.ShowCaption != TempTablePresentation.TemplateStyle.Legends.ShowCaption)
                {
                    return false;
                }

                if (this._TemplateStyle.Legends.ShowRange != TempTablePresentation.TemplateStyle.Legends.ShowRange)
                {
                    return false;
                }

                //-- Footnotes 
                if (this._TemplateStyle.Footnotes.FontTemplate.FontName != TempTablePresentation.TemplateStyle.Footnotes.FontTemplate.FontName)
                {
                    return false;
                }

                if (this._TemplateStyle.Footnotes.FontTemplate.FontStyle != TempTablePresentation.TemplateStyle.Footnotes.FontTemplate.FontStyle)
                {
                    return false;
                }

                if (this._TemplateStyle.Footnotes.FontTemplate.FontSize != TempTablePresentation.TemplateStyle.Footnotes.FontTemplate.FontSize)
                {
                    return false;
                }

                if (this._TemplateStyle.Footnotes.FontTemplate.BackColor != TempTablePresentation.TemplateStyle.Footnotes.FontTemplate.BackColor)
                {
                    return false;
                }

                if (this._TemplateStyle.Footnotes.FontTemplate.ForeColor != TempTablePresentation.TemplateStyle.Footnotes.FontTemplate.ForeColor)
                {
                    return false;
                }

                //-- Comments 
                if (this._TemplateStyle.Comments.FontTemplate.FontName != TempTablePresentation.TemplateStyle.Comments.FontTemplate.FontName)
                {
                    return false;
                }

                if (this._TemplateStyle.Comments.FontTemplate.FontStyle != TempTablePresentation.TemplateStyle.Comments.FontTemplate.FontStyle)
                {
                    return false;
                }

                if (this._TemplateStyle.Comments.FontTemplate.FontSize != TempTablePresentation.TemplateStyle.Comments.FontTemplate.FontSize)
                {
                    return false;
                }

                if (this._TemplateStyle.Comments.FontTemplate.BackColor != TempTablePresentation.TemplateStyle.Comments.FontTemplate.BackColor)
                {
                    return false;
                }

                if (this._TemplateStyle.Comments.FontTemplate.ForeColor != TempTablePresentation.TemplateStyle.Comments.FontTemplate.ForeColor)
                {
                    return false;
                }

                //-- Denominator 
                if (this._TemplateStyle.Denominator.FontTemplate.FontName != TempTablePresentation.TemplateStyle.Denominator.FontTemplate.FontName)
                {
                    return false;
                }

                if (this._TemplateStyle.Denominator.FontTemplate.FontStyle != TempTablePresentation.TemplateStyle.Denominator.FontTemplate.FontStyle)
                {
                    return false;
                }

                if (this._TemplateStyle.Denominator.FontTemplate.FontSize != TempTablePresentation.TemplateStyle.Denominator.FontTemplate.FontSize)
                {
                    return false;
                }

                if (this._TemplateStyle.Denominator.FontTemplate.BackColor != TempTablePresentation.TemplateStyle.Denominator.FontTemplate.BackColor)
                {
                    return false;
                }

                if (this._TemplateStyle.Denominator.FontTemplate.ForeColor != TempTablePresentation.TemplateStyle.Denominator.FontTemplate.ForeColor)
                {
                    return false;
                }
            }

            catch (Exception ex)
            {

            }
            return true;

        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                //this.DIQueries.Dispose();
                //this.DIConnection.Dispose();                
                this._UserPreference.Dispose();
                this.DIDataView = null;
                this.TableXLS.Dispose();
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region ILanguage Members

        public void ApplyLanguageSettings()
        {
            string Grand = DILanguage.GetLanguageString("GRAND");
            LanguageStrings.Sum = DILanguage.GetLanguageString("SUM");
            LanguageStrings.Average = DILanguage.GetLanguageString("AVERAGE");
            LanguageStrings.Count = DILanguage.GetLanguageString("COUNT");
            LanguageStrings.GrandSum = Grand + " " + LanguageStrings.Sum;
            LanguageStrings.GrandAverage = Grand + " " + LanguageStrings.Average;
            LanguageStrings.GrandCount = Grand + " " + LanguageStrings.Count;
            LanguageStrings.DataValue = DILanguage.GetLanguageString("DATAVALUE");
            LanguageStrings.Table = DILanguage.GetLanguageString("TABLE");
            LanguageStrings.Data = DILanguage.GetLanguageString("DATA");
            LanguageStrings.Source = DILanguage.GetLanguageString("SOURCE");
            LanguageStrings.TimePeriod = DILanguage.GetLanguageString("TIMEPERIOD");
            LanguageStrings.AreaID = DILanguage.GetLanguageString("AREAID");
            LanguageStrings.AreaName = DILanguage.GetLanguageString("AREANAME");
            LanguageStrings.Indicator = DILanguage.GetLanguageString("INDICATOR");
            LanguageStrings.Unit = DILanguage.GetLanguageString("UNIT");
            LanguageStrings.Subgroup = DILanguage.GetLanguageString("SUBGROUP");
            LanguageStrings.FootNotes = DILanguage.GetLanguageString("FOOTNOTES");
            LanguageStrings.Comments = DILanguage.GetLanguageString("NOTES_NOTES");
            LanguageStrings.Minimum = DILanguage.GetLanguageString("MIN");
            LanguageStrings.Maximum = DILanguage.GetLanguageString("MAX");
            LanguageStrings.Chart = DILanguage.GetLanguageString("GRAPH");
            LanguageStrings.FrequencyTable = DILanguage.GetLanguageString("FREQ_TABLE");

        }

        #endregion

        #region " -- Table Workbook Image --"

        public static Image GetTableImage(string presentationPath)
        {
            Image Retval = null;
            try
            {
                DIExcel TableExcel = new DIExcel(presentationPath, System.Globalization.CultureInfo.CurrentCulture);
                SpreadsheetGear.IRange TableRange = TableExcel.GetUsedRange(0);

                SpreadsheetGear.Drawing.Image TableImage = new SpreadsheetGear.Drawing.Image(TableRange);

                Bitmap TableBitmap = TableImage.GetBitmap(System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Retval = (Image)TableBitmap;
                TableExcel.Close();
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        public static Image GetTableImage(string presentationPath, Size imageSize)
        {
            Image Retval = null;
            try
            {
                DIExcel TableExcel = new DIExcel(presentationPath, System.Globalization.CultureInfo.CurrentCulture);
                SpreadsheetGear.IRange TableRange = TableExcel.GetUsedRange(0);

                SpreadsheetGear.Drawing.Image TableImage = new SpreadsheetGear.Drawing.Image(TableRange);

                Bitmap TableBitmap = TableImage.GetBitmap(System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Size NewSize = DICommon.GetImageSizeByAspectRatio(TableBitmap.Width, TableBitmap.Height, imageSize.Width, imageSize.Height, true);
                Retval = TableBitmap.GetThumbnailImage(NewSize.Width, NewSize.Height, null, System.IntPtr.Zero);
                TableExcel.Close();
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        public static void GetSpecificRowsColumnTableImage(string presentationPath, string destinationPathWFileName, Size imageSize)
        {
            try
            {
                Image ExcelTableImage = null;
                int RowIndex = 10;
                int ColumnIndex = 5;

                DIExcel TableExcel = new DIExcel(presentationPath, System.Globalization.CultureInfo.CurrentCulture);
                SpreadsheetGear.IRange TableRange = TableExcel.GetUsedRange(0);

                if (TableRange.RowCount > 8)
                {
                    RowIndex = 8;
                }
                else
                {
                    RowIndex = TableRange.RowCount;
                }

                if (TableRange.ColumnCount > 2)
                {
                    ColumnIndex = 2;
                }
                else
                {
                    ColumnIndex = TableRange.ColumnCount;
                }

                TableRange = TableExcel.GetSelectedRange(0, 0, 0, RowIndex, ColumnIndex);
                SpreadsheetGear.Drawing.Image TableImage = new SpreadsheetGear.Drawing.Image(TableRange);

                Bitmap TableBitmap = TableImage.GetBitmap(System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Size NewSize = DICommon.GetImageSizeByAspectRatio(TableBitmap.Width, TableBitmap.Height, imageSize.Width, imageSize.Height, true);
                ExcelTableImage = TableBitmap.GetThumbnailImage(NewSize.Width, NewSize.Height, null, System.IntPtr.Zero);
                ExcelTableImage.Save(destinationPathWFileName);
                TableExcel.Close();
            }
            catch (Exception)
            {
            }
        }


        /// <summary>
        /// Save the table thumbnail image
        /// </summary>
        /// <param name="presentationPath">Presentation file name with path</param>
        /// <param name="destinationPathWFileName">Destination file name with path i.e image file name with path</param>
        /// <param name="imageSize">size of thumbnail image</param>
        public static void SaveTableImage(string presentationPath, string destinationPathWFileName, Size imageSize)
        {
            try
            {
                Image ExcelTableImage = null;
                DIExcel TableExcel = new DIExcel(presentationPath, System.Globalization.CultureInfo.CurrentCulture);
                SpreadsheetGear.IRange TableRange = TableExcel.GetUsedRange(0);
                SpreadsheetGear.Drawing.Image TableImage = new SpreadsheetGear.Drawing.Image(TableRange);

                Bitmap TableBitmap = TableImage.GetBitmap(System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Size NewSize = DICommon.GetImageSizeByAspectRatio(TableBitmap.Width, TableBitmap.Height, imageSize.Width, imageSize.Height, true);
                ExcelTableImage = TableBitmap.GetThumbnailImage(NewSize.Width, NewSize.Height, null, System.IntPtr.Zero);
                ExcelTableImage.Save(destinationPathWFileName);
                TableExcel.Close();
            }
            catch (Exception)
            {
            }
        }

        #endregion
    }
}
