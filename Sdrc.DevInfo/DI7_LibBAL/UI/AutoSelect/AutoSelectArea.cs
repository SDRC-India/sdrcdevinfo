using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.IO;
using System.Xml;
using System.Data;
using DevInfo.Lib.DI_LibDAL;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.UI.UserPreference;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.UI.AutoSelect
{
    public class AutoSelectArea
    {
        #region -- Private --

        #region -- Variables --

        //Private variable to store UserPReference and connection details. Set while a call to GetDataView is made and then deferred usage in GetIUSSource
        private UserPreference.UserPreference mUserPreference;
        private DIQueries mdIQueries;
        private DIConnection mdIConnection;

        // Wil be built on every intermediate query for Autoselection
        // Will be used in the final step of Autoselection
        private DataTable mAvlAreaNIDs;

        int mparentNId;
        int mcurrentLevel;
        int mrequiredLevel;

        string mAreaNIDs = string.Empty;

        #endregion

        #region -- Methods --

        /// <summary>
        /// Uses mUserPreference and mAreaNIDs
        /// </summary>
        /// <returns>Returns the Autoselect query on tha basis of IUS, Time and Area selection</returns>
        private string GetAutoSelectQueryByIUSTimeArea(bool useIndicator, bool useTimePeriod)
        {
            string retVal = string.Empty;
            string sIUS = string.Empty;
            string sTimePeriods = string.Empty;

            // Check for Indicator
            if (useIndicator)
            {
                sIUS = this.mUserPreference.UserSelection.IndicatorNIds;
            }
            // Check for TimePeriod
            if (useTimePeriod)
            {
                sTimePeriods = this.mUserPreference.UserSelection.TimePeriodNIds;
            }

            // Get the Query
            if (this.mUserPreference.UserSelection.ShowIUS)
            {
                // Auto Select Area NIDS by IUS, Time and Area
                retVal = this.mdIQueries.Area.AutoSelectArea("", sIUS, sTimePeriods, this.mAreaNIDs);
            }
            else
            {
                // Auto Select Area NIDS by Indicators, Time and Area
                retVal = this.mdIQueries.Area.AutoSelectArea(sIUS, "", sTimePeriods, this.mAreaNIDs);
            }

            return retVal;
        }

        private string BuildCommaseperatedAreaNIDs(string _Query)
        {
            string retVal=string.Empty;
            StringBuilder sbAreaNIDs = new StringBuilder();
            string sAreaNIDs = string.Empty;

            // initialize mAvlAreaNIDs datatable
            InitializeAvlAreaNIDDataTable(_Query);
            // Build all Area NIDs
            sbAreaNIDs = new StringBuilder();
            for (int i = 0; i < this.mAvlAreaNIDs.Rows.Count; i++)
            {
                sbAreaNIDs.Append("," + this.mAvlAreaNIDs.Rows[i][Area.AreaNId]);
            }
            if (sbAreaNIDs.Length > 0)
            {
                retVal = sbAreaNIDs.ToString().Substring(1);
            }
            return retVal;
        }

        private void InitializeAvlAreaNIDDataTable(string _Query)
        {
            // initialize mAvlAreaNIDs datatable
            this.mAvlAreaNIDs = new DataTable("AvailableAreaNIDs");
            this.mAvlAreaNIDs = mdIConnection.ExecuteDataTable(_Query);
        }

        private string GetAreasFromWhichAutoSelectionToBeDone()
        {
            string retVal = string.Empty;
            string _Query = string.Empty;

            // --------------------------------------
            // STEP 1: Build Query to get all AreaNIDs from which the Autoselection needs to be done
            // --------------------------------------

            // STEP 1.1: Check if any Area selected in the Area Tree
            if (this.mparentNId == -1)
            {

                // STEP 1.1: -- NO AREA selected in the Tree --

                // set current level to 0
                this.mcurrentLevel = 0;
                // STEP 1.1.1: Check for the Level selected
                if (this.mrequiredLevel == 0)
                {
                    // -- NO LEVEL selected and no Parent is selected --
                    // STEP 1.1.1.1: GET ALL AREA NIDS
                    _Query = this.mdIQueries.Area.GetArea(FilterFieldType.None, "");
                }
                else
                {
                    // -- LEVEL SELECTED - but no Parent is selected --
                    // STEP 1.1.2.1: GET ALL AREA NIDS for a given LEVEL
                    _Query = this.mdIQueries.Area.GetAreaByAreaLevel(mrequiredLevel.ToString());
                }
            }
            else
            {
                // STEP 1.2: -- AREA selected in the Tree --
                // Current Level will have the Level of the selected Area in the Tree
                // mcurrentLevel = mcurrentLevel;

                // STEP 1.2.1: Check for the Level selected
                if (this.mrequiredLevel == 0)
                {
                    // -- NO LEVEL selected but Parent is selected --
                    // STEP 1.2.1.1: GET ALL Children under the selected Parent for all Levels
                    // -- NOTE: It is not possible to get all children under the selected Parent for all levels.
                    //          Currently this would require optimization in the database structure - so get all areas for all levels irrespective of the parent selected
                    // -- GET ALL AREA NIDS
                    _Query = this.mdIQueries.Area.GetArea(FilterFieldType.None, "");
                }
                else
                {
                    // -- LEVEL SELECTED and Parent is selected --
                    // STEP 1.2.2.1: GET ALL Children under the selected Parent for the selected level
                    _Query = this.mdIQueries.Area.GetAreaByAreaNIds(mparentNId, mcurrentLevel, mrequiredLevel, "");
                }
            }


            // --------------------------------------
            // STEP 2: Build all Area NIDs from which the Autoselection needs to be done
            // --------------------------------------
            retVal = BuildCommaseperatedAreaNIDs(_Query);

            return retVal;
        }

        #endregion

        #endregion

        #region -- Public --

        #region -- Constructor --

        /// <summary>
        /// Area Autoselection: From the TreeView by Selected Area, Target Area Level
        /// </summary>
        /// <param name="userPreference"></param>
        /// <param name="dIConnection"></param>
        /// <param name="dIQueries"></param>
        /// <param name="parentNId"></param>
        /// <param name="currentLevel"></param>
        /// <param name="requiredLevel"></param>
        public AutoSelectArea(UserPreference.UserPreference userPreference, DI_LibDAL.Connection.DIConnection dIConnection, DI_LibDAL.Queries.DIQueries dIQueries, int parentNId, int currentLevel, int requiredLevel)
        {
            // -- Connection details preserved
            this.mdIConnection = dIConnection;
            this.mdIQueries = dIQueries;
            this.mUserPreference = userPreference;

            // Autoselect Parameters
            this.mparentNId = parentNId;
            this.mcurrentLevel = currentLevel;
            this.mrequiredLevel = requiredLevel;
            this.mAreaNIDs = string.Empty;
        }

        /// <summary>
        /// Area Autoselection: From the Available list by a list of available Area NIDs
        /// </summary>
        /// <param name="userPreference"></param>
        /// <param name="dIConnection"></param>
        /// <param name="dIQueries"></param>
        /// <param name="sAvlAreaNIDs"></param>
        public AutoSelectArea(UserPreference.UserPreference userPreference, DI_LibDAL.Connection.DIConnection dIConnection, DI_LibDAL.Queries.DIQueries dIQueries, string avlAreaNIDs)
        {
            // -- Connection details preserved
            this.mdIConnection = dIConnection;
            this.mdIQueries = dIQueries;
            this.mUserPreference = userPreference;

            // Autoselect Parameters
            this.mparentNId = -1;
            this.mcurrentLevel = 0;
            this.mrequiredLevel = 0;
            this.mAreaNIDs = avlAreaNIDs;
        }

        #endregion

        #region -- Methods --

        public DataView GetAutoSelectAreas(bool useIndicator, bool useTimePeriod)
        {
            // Step 1: Get Areas from which Autoselection needs to be done
            // STEP 2: Build Area Auto Select Query
            // STEP 3: Run AutoSelect query to get the AutoSelected Areas

            DataView retVal = new DataView();
            string _AutoSelectQuery = string.Empty;

            // --------------------------------------
            // Step 1: Get Areas from which Autoselection needs to be done
            // --------------------------------------
            // Case 1: WHEN this.mAreaNIDs is empty - Build area NIDs from which which the Autoselection needs to be done
            // Case 2: WHEN this.mAreaNIDs is not empty - could already be set using the overloaded constructor. 
            if (this.mAreaNIDs.Length == 0)
            {
                // Case 1: WHEN this.mAreaNIDs is empty - Build area NIDs from which which the Autoselection needs to be done
                this.mAreaNIDs = GetAreasFromWhichAutoSelectionToBeDone();
            }
            else
            {
                // Case 2: WHEN this.mAreaNIDs is not empty - could already be set using the overloaded constructor. 

                // this.mAreaNIDs is already set using the Constructor - Client application has set
                
                // initialize mAvlAreaNIDs datatable
                //InitializeAvlAreaNIDDataTable(this.mdIQueries.Area.GetArea(FilterFieldType.None, ""));
                InitializeAvlAreaNIDDataTable(this.mdIQueries.Area.GetArea(FilterFieldType.NId, this.mAreaNIDs));

            }
            

            // --------------------------------------
            // STEP 2: Build Area Auto Select Query
            // --------------------------------------
            _AutoSelectQuery = GetAutoSelectQueryByIUSTimeArea(useIndicator, useTimePeriod);

            // --------------------------------------
            // STEP 3: Run AutoSelect query to get the AutoSelected Areas
            // --------------------------------------
            if (_AutoSelectQuery.Length > 0)
            {
                DataTable dtAutoSelectAreas;
                DataView dvAutoSelectAreas;
                // STEP 3.1: get the AreaNID dataview
                dtAutoSelectAreas = mdIConnection.ExecuteDataTable(_AutoSelectQuery);

                // STEP 3.2: Add dtAutoSelectAreas and this.mAvlAreaNIDs into a Dataset to set a relationship
                DataSet dsAreas = new DataSet();

                // STEP 3.2.1: Add this.mAvlAreaNIDs table into the dataset
                dsAreas.Tables.Add(this.mAvlAreaNIDs);

                // STEP 3.2.2: Add dtAutoSelectAreas table into the dataset
                dsAreas.Tables.Add(dtAutoSelectAreas);
                dsAreas.Tables[1].TableName = "Child";

                // STEP 3.2.3: Set Primaty Key in the AutoSelectAreas Data Table
                DataColumn[] PK = new DataColumn[1];
                PK[0] = dsAreas.Tables[1].Columns[Area.AreaNId];
                dsAreas.Tables[1].PrimaryKey = PK;

                // STEP 3.3: Add a selected column in the mAvlAreaNIDs table
                // IsAutoSelect field will be updated using the expression on the basis of the AutoSelected AreaNIDs in the final step of AutoSelection
                this.mAvlAreaNIDs.Columns.Add("IsAutoSelect");
                this.mAvlAreaNIDs.AcceptChanges();

                // STEP 3.4: Define relationship 
                dsAreas.Relations.Add("RelAreaNId", dsAreas.Tables[1].Columns[Area.AreaNId], dsAreas.Tables[0].Columns[Area.AreaNId], false);

                // STEP 3.5: Set the value for 
                dsAreas.Tables[0].Columns["IsAutoSelect"].Expression = "Parent(RelAreaNId)." + Area.AreaNId;

                // STEP 3.6: Create the DataView to be returned
                dvAutoSelectAreas = dsAreas.Tables[0].DefaultView;

                // STEP 3.7: Apply Filter on retVal to have only those rows which have some values in IsAutoSelect column
                dvAutoSelectAreas.RowFilter = "IsAutoSelect > 0 ";

                retVal = dvAutoSelectAreas.ToTable().DefaultView;
            }

            return retVal;
        }

        #endregion

        #endregion

    }
}
