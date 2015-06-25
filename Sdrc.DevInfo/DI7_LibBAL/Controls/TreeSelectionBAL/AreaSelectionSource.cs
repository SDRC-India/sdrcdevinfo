using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.UI.AutoSelect;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.UI.UserPreference;

namespace DevInfo.Lib.DI_LibBAL.Controls.TreeSelectionBAL
{
    /// <summary>
    /// Helps in getting area records for TreeSelectionControl
    /// </summary>
    public class AreaSelectionSource : BaseSelectionSource
    {

        #region "-- Private --"

        #region "-- Methods --"

        private DataTable GetRecordsForAvailableAutoSelect(string availableItemsNid)
        {
            DataTable RetVal = new DataTable();
            int Requiredlevel = 0;
            AutoSelectArea AreaAutoSelect;
            UserPreference UserPreferenceInstance;

            try
            {
                //get required area level
                if (this._AreaRequiredLevel > 0)
                {
                    Requiredlevel = this._AreaRequiredLevel;
                }

                //check and set userpreferences for selection
                if (this.UserPrefences != null)
                {
                    UserPreferenceInstance = this.UserPrefences;
                }
                else
                {
                    UserPreferenceInstance = new DevInfo.Lib.DI_LibBAL.UI.UserPreference.UserPreference();
                    UserPreferenceInstance.UserSelection = new DevInfo.Lib.DI_LibDAL.UserSelection.UserSelection();
                    UserPreferenceInstance.UserSelection.TimePeriodNIds = this.UserPrefences.UserSelection.TimePeriodNIds;
                    UserPreferenceInstance.UserSelection.IndicatorNIds = this.UserPrefences.UserSelection.IndicatorNIds;
                    UserPreferenceInstance.UserSelection.ShowIUS = this.UserPrefences.UserSelection.ShowIUS;

                }

                //create AutoSelect class instance 
                    AreaAutoSelect = new AutoSelectArea(UserPreferenceInstance, this._DBConnection, this.SqlQueries, availableItemsNid);
                
                //get records
                RetVal = AreaAutoSelect.GetAutoSelectAreas(this.UseIndicator, this.UseTime).Table;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }


        private DataTable GetRecordsForAutoSelect(bool isForTree, string availableItemsNid)
        {
            DataTable RetVal = new DataTable();
            int Requiredlevel = 0;
            string SelectedIndicatorNIds = string.Empty;
            string SelectedTimeperiodNIds = this.UserPrefences.UserSelection.TimePeriodNIds;
            string SelectedSourceNIds = this.UserPrefences.UserSelection.SourceNIds;

            try
            {
                //get required area level
                if (this._AreaRequiredLevel > 0)
                {
                    Requiredlevel = this._AreaRequiredLevel;
                }

                SelectedIndicatorNIds = this.UserPrefences.UserSelection.IndicatorNIds;

                // -- AUTO SELECT the source nids on the basis of the selected  Indicator/IUS, Time & Area  
                if (!this._UseIndicator)
                {
                    SelectedIndicatorNIds = string.Empty;
                }

                if (!this._UseTime)
                    SelectedTimeperiodNIds = string.Empty;
                if (!this._UseSource)
                    SelectedSourceNIds = string.Empty;

                if (isForTree)
                {
                    RetVal = this._DBConnection.ExecuteDataTable(this.SqlQueries.Area.GetAutoSelectAreas(SelectedIndicatorNIds, this._UserPrefences.UserSelection.ShowIUS, SelectedTimeperiodNIds, SelectedSourceNIds, Requiredlevel));
                }
                else
                {

                    if (this.IsAutoFillForAvailableList && this._SelectedNid >0 && string.IsNullOrEmpty(availableItemsNid ))
                    {
                        // if ISAutoFillForAvailableList is true then get areanids where parent_nid = selected area node nid
                        DataTable TempTable = this._DBConnection.ExecuteDataTable(this.SqlQueries.Area.GetArea(FilterFieldType.ParentNId, this._SelectedNid.ToString()));
                        if (TempTable.Rows.Count > 0)
                        {
                            availableItemsNid = DevInfo.Lib.DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromDataTable(TempTable, Area.AreaNId);
                        }
                        else
                        {
                            availableItemsNid = "0";
                        }
                        
                    }

                    if (string.IsNullOrEmpty(availableItemsNid))
                    {
                        availableItemsNid = "-1";
                    }
                    RetVal = this._DBConnection.ExecuteDataTable(this.SqlQueries.Area.GetAutoSelectAreas(SelectedIndicatorNIds, this._UserPrefences.UserSelection.ShowIUS, SelectedTimeperiodNIds, SelectedSourceNIds, Requiredlevel, availableItemsNid));
                }


            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        #endregion

        #endregion

        #region "-- Protected --"

        #region "-- Methods --"

        protected override void SetColumnNames()
        {
            this._TagValueColumnName = Area.AreaNId;
            this._GlobalValueColumnName1 = Area.AreaGlobal;
            this._GlobalValueColumnName2 = Area.AreaGlobal;
            this._GlobalValueColumnName3 = Area.AreaGlobal;
            this._FirstColumnName = Area.AreaName;
            this._SecondColumnName = Area.AreaID;
            this._ThirdColumnName = Area.AreaLevel;
        }

        protected override void SetColumnHeaders()
        {
            //Get header string from language file and set column headers string 
            this._FirstColumnHeader = DILanguage.GetLanguageString("AREANAME");
            this._SecondColumnHeader = DILanguage.GetLanguageString("AREAID");
            this._ThirdColumnHeader = DILanguage.GetLanguageString("LEVEL");
        }

        protected override string GetAssocicatedRecordsQuery(int selectedNid, int selectedParentNid)
        {
            // "Select Area_Nid,Area_Name,Area_Id,Area_Level,Area_Parent_Nid,Area_Global from UT_Area_en where Area_Parent_Nid=" + selectedNid;
            string RetVal = string.Empty;
            DataTable Table;
            string AreaNIDsFrmBlock = string.Empty;

            // 1. get area_nids from area_block for the selected area
            RetVal = this.SqlQueries.Area.GetArea(FilterFieldType.NId, selectedNid.ToString());
            Table = this._DBConnection.ExecuteDataTable(RetVal);
            if (Table.Rows.Count > 0)
            {
                AreaNIDsFrmBlock = Convert.ToString(Table.Rows[0][Area.AreaBlock]).Trim();
            }

            // 2. get areas where parent nid is equal to the selected nid
            RetVal = this.SqlQueries.Area.GetArea(DevInfo.Lib.DI_LibDAL.Queries.FilterFieldType.ParentNId, selectedNid.ToString());

            // concatenate areablocks nids in where clause
            if (!string.IsNullOrEmpty(AreaNIDsFrmBlock))
            {
                RetVal += " OR " + Area.AreaNId + " IN(" + AreaNIDsFrmBlock + ") ";
            }

            // if showAreaGroupInLists is false then dont get group area(where areablock exists) 
            if (!this.ShowAreaGroupInLists)
            {
                //-- Change for MySql AS AreaBlock field is nText type
                if (this._DBConnection.ConnectionStringParameters.ServerType == DevInfo.Lib.DI_LibDAL.Connection.DIServerType.MySql)
                {
                    RetVal += "  AND (" + Area.AreaBlock + " IS null OR "+ Area.AreaBlock +" = '')";
                }
                else if (this._DBConnection.ConnectionStringParameters.ServerType != DevInfo.Lib.DI_LibDAL.Connection.DIServerType.MsAccess )
                {
                    RetVal += "  AND (" + Area.AreaBlock + " IS null OR CAST(" + Area.AreaBlock + " AS NVarChar(4000)) = '')";
                }
                else
                {
                    RetVal += "  AND (" + Area.AreaBlock + "='' or " + Area.AreaBlock + " IS null ) ";
                }
            }

            if (this.AreaTreeSortingType == AreaTreeSortType.ByAreaID)
            {
                RetVal += "ORDER BY " + Area.AreaID;
            }
            else
            {
                RetVal += "ORDER BY " + Area.AreaName;
            }

            return RetVal;
        }

        protected override string GetSelectAllQuery()
        {
            // "Select Area_Nid,Area_Name,Area_Id,Area_Level,Area_Parent_Nid,Area_Global from UT_Area_en ";
            string RetVal = string.Empty;
            if (this._AreaRequiredLevel > 0 || this._SelectedNid>0)
            {
                string AreaNIDsFrmBlock = string.Empty;
                DataTable Table;

                // 1. get areaNis from area block where area_nid = selectednid
                RetVal = this.SqlQueries.Area.GetArea(FilterFieldType.NId, this._SelectedNid.ToString());
                Table = this._DBConnection.ExecuteDataTable(RetVal);
                if (Table.Rows.Count > 0)
                {
                    AreaNIDsFrmBlock = Convert.ToString(Table.Rows[0][Area.AreaBlock]).Trim();
                }

                // 2. get areas by area NId and level
                RetVal = this.SqlQueries.Area.GetAreaByAreaNIds(this._SelectedNid, this._AreaSelectedLevel, this._AreaRequiredLevel, string.Empty);

                // 3. Add areaNids found from AreaBlock
                if (!string.IsNullOrEmpty(AreaNIDsFrmBlock))
                {
                    RetVal += " OR (A" + (this._AreaRequiredLevel - 2) +"."+ Area.AreaNId + " IN (" + AreaNIDsFrmBlock +")  AND A" + (this._AreaRequiredLevel - 2) +"."+ Area.AreaLevel +" = "+ this._AreaRequiredLevel +" )";
                }

                // if showAreaGroupInLists is false then dont get group area(where areablock exists) 
                if (!this.ShowAreaGroupInLists)
                {
                    //-- For SqlServer Database AreaBlock is of nText DataType and so not support "=''"
                    if (this._DBConnection.ConnectionStringParameters.ServerType != DevInfo.Lib.DI_LibDAL.Connection.DIServerType.MsAccess)
                    {
                        RetVal += "  AND (A0." + Area.AreaBlock + " IS null )";
                    }
                    else
                    {
                        RetVal += "  AND  (A0." + Area.AreaBlock + "='' or A0." + Area.AreaBlock + " IS null )";
                    }
                }
            }
            else
            {            
    
                    RetVal = this.SqlQueries.Area.GetArea(DevInfo.Lib.DI_LibDAL.Queries.FilterFieldType.None,
                        string.Empty);

                // if showAreaGroupInLists is false then dont get group area(where areablock exists) 
                    if (!this.ShowAreaGroupInLists)
                    {
                        if (this._DBConnection.ConnectionStringParameters.ServerType == DevInfo.Lib.DI_LibDAL.Connection.DIServerType.SqlServer || this._DBConnection.ConnectionStringParameters.ServerType == DevInfo.Lib.DI_LibDAL.Connection.DIServerType.SqlServerExpress )
                        {
                            RetVal += "  where  (CAST(" + Area.AreaBlock + " as NVarChar(4000))='' or " + Area.AreaBlock + " IS null )";
                        }
                        else
                        {
                            RetVal += "  where  (" + Area.AreaBlock + "='' or " + Area.AreaBlock + " IS null )";

                        }
                    }
            }
                   
            

            return RetVal;

        }

        protected override string GetRecordsForSelectedNids(string nids)
        {
            string RetVal = string.Empty;

            RetVal = this.SqlQueries.Area.GetArea(FilterFieldType.NId, nids);

            return RetVal;
        }

        protected override string GetAssocicatedRecordsQuery(string selectedNids)
        {
            //Do nothing
            return string.Empty;
        }

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Methods --"

        public override DataTable GetAutoSelectRecordsForAvailableList(string availableItemsNid)
        {
            //return this.GetRecordsForAvailableAutoSelect(availableItemsNid);
            return this.GetRecordsForAutoSelect(false, availableItemsNid);
        }

        /// <summary>
        /// Returns records for  auto select option of treeview.
        /// </summary>
        /// <returns></returns>
        public override DataTable GetAllRecordsForTreeAutoSelect()
        {
            DataTable RetVal=null;
            string SqlQuery;
            DataTable TempTable;
            string AvailableNIdsForAutoSelect = string.Empty;
            int RequiredLevel = -1;
            int SelectedLevel = -1;
            int MaxLevel = -1;
            bool IsAutoFillAvailableList = this.IsAutoFillForAvailableList;
           // if (this.IsAutoFillForAvailableList)
            //{

                if (this._SelectedNid == -1) // for root node
                {
                    RetVal = this.GetRecordsForAutoSelect(true, string.Empty);
                }
                else
                {
                    // Reset IsAutoFillForAvailableList to false  
                    this.IsAutoFillForAvailableList = false;

                                        
                    RequiredLevel = this._AreaRequiredLevel;
                    if (this._AreaRequiredLevel <= 0)
                    {
                        this._AreaRequiredLevel = this._AreaSelectedLevel+1;
                        MaxLevel = this.GetMaxAreaLevel();

                        if (this._AreaRequiredLevel > MaxLevel)
                        {
                            this._AreaRequiredLevel = MaxLevel;
                        }
                    }
                    else
                    {
                        MaxLevel = this._AreaRequiredLevel;
                    }


                    // get areas from area block 
                    if (this._AreaRequiredLevel <= MaxLevel)
                    { 
                        

                    }


                    for (int i = this._AreaRequiredLevel; i <= MaxLevel; i++)
                    {



                        // get all areas for the required level
                        this._AreaRequiredLevel = i;

                        SqlQuery = this.GetSelectAllQuery();                        
                        TempTable = this._DBConnection.ExecuteDataTable(SqlQuery);

                        // get nids
                        AvailableNIdsForAutoSelect = DevInfo.Lib.DI_LibDAL.Connection.DIConnection.GetDelimitedValuesFromDataTable(TempTable, Area.AreaNId);

                        if (string.IsNullOrEmpty(AvailableNIdsForAutoSelect))
                        {
                            AvailableNIdsForAutoSelect = "0";
                        }


                        // get only autofill area for the availablenids
                        TempTable=this.GetRecordsForAutoSelect(false, AvailableNIdsForAutoSelect);
                        if (RetVal == null)
                        {
                            RetVal = TempTable;
                        }
                        else
                        {
                            if (TempTable != null)
                            {
                                RetVal.Merge(TempTable);
                            }
                        }

                    }

                    //reset required level 
                    this._AreaRequiredLevel = RequiredLevel;

                      // Reset IsAutoFillForAvailableList to original value
                      this.IsAutoFillForAvailableList = IsAutoFillAvailableList;
                  
                  

                }
          
            return RetVal;
        }

        public override void UpdataDataTableBeforeCreatingListviewItem(DataTable iuTable)
        {
            //-- Do Nothing
        }

        public override List<string> GetIUSNIds(string iuNIds, bool checkUserSelection, bool selectSingleTon)
        {
            return new List<string>();
        }

        public override List<string> GetSubgroupDimensions(string iuNId, string IUSNIds)
        {
            return new List<string>();
        }

        public override List<string> GetSubgroupDimensionsWithIU(string iuNId, string IUSNIds)
        {
            return new List<string>();
        }

        public override void UpdateIndicatorSelectedDetails(int indicatorNId, int unitNId, string selectionDetails, bool addNewSelection)
        {
            //Do nothing
        }

        public override string GetIndicatorSelectionDetails(int indicatorNId, int unitNId)
        {
            return string.Empty;
        }

        #endregion

        #endregion

    }
}
