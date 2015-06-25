using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.UI.UserPreference;


namespace DevInfo.Lib.DI_LibBAL.Controls.TreeSelectionBAL
{
    /// <summary>
    /// Helps in getting indicator and ius records for TreeSelectionControl
    /// </summary>
    public class IndicatorSelectionSource : BaseSelectionSource
    {
        #region "-- Private --"

        #region "-- Methods --"

        private string GetAutoSelectQuery(int selectedICNId, string availableNIds)
        {
            string RetVal = string.Empty;
            string Sql = string.Empty;
            string IUSNIDs = string.Empty;
            string SelectedICNId = selectedICNId.ToString();
            bool IsIUSNIdsIsGiven = false;
            StringBuilder sICNIds = new StringBuilder();
            string[] DistinctColumns = new string[1];
            DataTable IUSTable = null;
            DataTable IUSFromICTypes = null;
            string[] IUNIds = new string[0];
            string[] IUNId = new string[0];
            string IndicatorNId_UnitNId = string.Empty;


            string SelectedTimeperiodNIds = this.UserPrefences.UserSelection.TimePeriodNIds;
            string SelectedAreaNIds = this.UserPrefences.UserSelection.AreaNIds;
            string SelectedSourceNIds = this.UserPrefences.UserSelection.SourceNIds;

            try
            {
                //-- Get all IUSNids for the selected IC Type 
                if (selectedICNId == -99)
                {
                    // -- When IC Element is slected 
                    Sql = this.SqlQueries.Indicators.GetIUSNIdFromICType(this._IndicatorClassificationType, string.Empty);
                    IUSFromICTypes = this._DBConnection.ExecuteDataTable(Sql);

                    // -- Get distinct IUS recrods. This will NOT return DISTINCT IUS under a given IC Type 
                    DistinctColumns[0] = IndicatorClassificationsIUS.IUSNId;
                    IUSFromICTypes = this.GetDistinctRecordsTable(IUSFromICTypes, DistinctColumns);
                }
                else if (!string.IsNullOrEmpty(availableNIds))
                {
                    if (this._UserPrefences.Indicator.ShowIUS)
                    {
                        IsIUSNIdsIsGiven = true;
                        IUSNIDs = availableNIds;
                    }
                    else
                    {
                        IUNIds = DICommon.SplitString(availableNIds, "~");
                        foreach (string AvailableIUNId in IUNIds)
                        {
                            IUNId = DICommon.SplitString(AvailableIUNId, ",");
                            IndicatorNId_UnitNId += ",'" + IUNId[0] + Delimiter.NUMERIC_SEPARATOR + IUNId[1] + "'";
                        }

                        if (!string.IsNullOrEmpty(IndicatorNId_UnitNId))
                        {
                            IndicatorNId_UnitNId = IndicatorNId_UnitNId.Substring(1);
                        }

                        // get IUS NIDs for the selected Indicator NID
                        Sql = this.SqlQueries.IUS.GetIUSFromIndicatorUnit(IndicatorNId_UnitNId, this._DBConnection.ConnectionStringParameters.ServerType);
                        //Sql = this.SqlQueries.IUS.GetIUSFromIndicator(availableNIds);
                        IUSFromICTypes = this._DBConnection.ExecuteDataTable(Sql);
                    }
                }
                //-- Get the IUS NIds on the basis of IC types 
                else if (selectedICNId > -1)
                {
                    // -- When IC Element is slected 
                    Sql = this.SqlQueries.Indicators.GetIUSNIdFromICType(this._IndicatorClassificationType, selectedICNId.ToString());
                    IUSFromICTypes = this._DBConnection.ExecuteDataTable(Sql);

                }

            // -- This will return Distinct IUS always under a given IC Element 
                else
                {
                    // -- When IC Element is NOT slected (IUS on the basis of the ICType) 
                    Sql = this.SqlQueries.Indicators.GetIUSNIdFromICType(this._IndicatorClassificationType, "");
                    IUSFromICTypes = this._DBConnection.ExecuteDataTable(Sql);

                    // -- Get distinct IUS recrods. This will NOT return DISTINCT IUS under a given IC Type 
                    DistinctColumns[0] = IndicatorClassificationsIUS.IUSNId;
                    IUSFromICTypes = this.GetDistinctRecordsTable(IUSFromICTypes, DistinctColumns);

                }


                // Get the comma seperated IUS NIDs  
                if (!IsIUSNIdsIsGiven)
                {
                    IUSNIDs = this.GetCommaSeparatedValues(IUSFromICTypes, IndicatorClassificationsIUS.IUSNId);
                }



                if (string.IsNullOrEmpty(IUSNIDs))
                {
                    IUSNIDs = "0";
                }

                if (selectedICNId == -99)
                {
                    // sIUSNIDs = "";
                }

                // -- AUTO SELECT the IUSNIDs on the basis of the selected Time, Area and Available IUS/I in the Available list 
                if (!this.UseTime)
                    SelectedTimeperiodNIds = string.Empty;
                if (!this.UseArea)
                    SelectedAreaNIds = string.Empty;
                if (!this.UseSource)
                    SelectedSourceNIds = string.Empty;


                Sql = this.SqlQueries.Indicators.GetAutoSelectByTimePeriodAreaSource(SelectedTimeperiodNIds, SelectedAreaNIds, IUSNIDs.ToString(), SelectedSourceNIds);
                IUSTable = this._DBConnection.ExecuteDataTable(Sql);


                //-- Get distinct IUSNIDs 
                DistinctColumns[0] = Data.IUSNId;
                IUSTable = this.GetDistinctRecordsTable(IUSTable, DistinctColumns);

                //get comma separated values
                IUSNIDs = this.GetCommaSeparatedValues(IUSTable, Indicator_Unit_Subgroup.IUSNId);

                if (this._UserPrefences.Indicator.ShowIUS)
                {
                    //Sql = this.SqlQueries.Indicators.GetIndicators(sIUSNIDs.ToString(), this.UserPrefences.Indicator.ShowIUS);
                    if (string.IsNullOrEmpty(IUSNIDs))
                    {
                        IUSNIDs = "-1";
                    }
                    if (selectedICNId < 0)
                    {
                        SelectedICNId = "-1";
                    }

                    if (SelectedICNId == "-1")
                    {
                        Sql = this.SqlQueries.IUS.GetIUSByParentOrder(FilterFieldType.NId, IUSNIDs, this.IndicatorClassificationType, FieldSelection.Light, SelectedICNId);
                    }
                    else
                    {
                        Sql = this.SqlQueries.IUS.GetIUSByOrder(FilterFieldType.NId, IUSNIDs, this.IndicatorClassificationType, FieldSelection.Light, SelectedICNId);
                    }
                }
                else
                {
                    Sql = this.SqlQueries.IUS.GetDistinctIndicatorUnit(IUSNIDs.ToString(), this.UserPrefences.Indicator.ShowIUS);
                }
                //Sql = this.SqlQueries.Indicators.GetIndicators(sIUSNIDs.ToString(), this.UserPrefences.Indicator.ShowIUS);

                RetVal = Sql;
            }
            catch (Exception ex)
            {
                RetVal = null;
            }

            return RetVal;
        }

        private DataTable AutoSelectIndicatorOrIUS(int icNId, ICType icType)
        {
            DataTable RetVal = new DataTable();
            string Sql = string.Empty;
            DataTable IUSTable;
            string[] DistinctColumns = new string[1];
            StringBuilder StringBuilderIUSNIds = new StringBuilder();
            string IUSNIDs = string.Empty;
            string SelectedTimeperiodNIds = this.UserPrefences.UserSelection.TimePeriodNIds;
            string SelectedAreaNIds = this.UserPrefences.UserSelection.AreaNIds;
            string SelectedSourceNIds = this.UserPrefences.UserSelection.SourceNIds;
            IDataReader IUSDataReader;

            // -- Get All Comma seperated IUSNIDs for the selected IC Type from the Available list 
            DataTable IUSFromICTypes;

            try
            {
                //'-- Get the IUS NIds on the basis of IC types 
                if (icNId > -1)
                {
                    // -- When IC Element is slected 
                    Sql = this.SqlQueries.Indicators.GetIUSNIdFromICType(icType, icNId.ToString());
                    IUSFromICTypes = this._DBConnection.ExecuteDataTable(Sql);
                }
                // -- This will return Distinct IUS always under a given IC Element 
                else
                {
                    // -- When IC Element is NOT slected (IUS on the basis of the ICType) 
                    Sql = this.SqlQueries.Indicators.GetIUSNIdFromICType(icType, "");
                    IUSFromICTypes = this._DBConnection.ExecuteDataTable(Sql);

                    // -- This will NOT return DISTINCT IUS under a given IC Type 
                    // -- Get DISTINCT IUS 
                    RetVal = IUSFromICTypes;
                    DistinctColumns[0] = IndicatorClassificationsIUS.IUSNId;

                    //-- Distinct IUS NIds 
                    IUSFromICTypes = RetVal.DefaultView.ToTable(true, DistinctColumns);
                }

                // -- Loop and get the comma seperated IUS NIDs 
                foreach (DataRow IUSNId in IUSFromICTypes.Rows)
                {
                    StringBuilderIUSNIds.Append("," + IUSNId[IndicatorClassificationsIUS.IUSNId]);
                }
                // -- Remove extra Comma 
                if (StringBuilderIUSNIds.Length > 0)
                {
                    IUSNIDs = StringBuilderIUSNIds.ToString().Substring(1);
                }

                // -- AUTO SELECT the DISTINCT IUSNIDs on the basis of the selected Time, Area and Available IUS/I in the Available list 
                if (!this.UseArea)
                    SelectedAreaNIds = string.Empty;
                if (!this.UseTime)
                    SelectedTimeperiodNIds = string.Empty;
                if (!this.UseSource)
                    SelectedSourceNIds = string.Empty;

                if (string.IsNullOrEmpty(IUSNIDs))
                {
                    IUSNIDs = "0";
                }

                Sql = this.SqlQueries.Indicators.GetAutoSelectByTimePeriodAreaSource(SelectedTimeperiodNIds, SelectedAreaNIds, IUSNIDs.ToString(), SelectedSourceNIds);

                IUSDataReader = this._DBConnection.ExecuteReader(Sql);
                StringBuilderIUSNIds.Length = 0;
                while ((IUSDataReader.Read()))
                {
                    StringBuilderIUSNIds.Append(",");
                    StringBuilderIUSNIds.Append(IUSDataReader[Indicator_Unit_Subgroup.IUSNId]);
                }
                IUSNIDs = string.Empty;
                if (StringBuilderIUSNIds.Length > 0)
                {
                    IUSNIDs = StringBuilderIUSNIds.ToString().Substring(1);
                }
                IUSDataReader.Close();

                if (this.UserPrefences.Indicator.ShowIUS)
                {
                    Sql = this.SqlQueries.Indicators.GetIndicators(IUSNIDs.ToString(), this.UserPrefences.Indicator.ShowIUS);
                }
                else
                {
                    Sql = this.SqlQueries.IUS.GetDistinctIndicatorUnit(IUSNIDs.ToString(), this.UserPrefences.Indicator.ShowIUS);
                }

                //Sql = this.SqlQueries.Indicators.GetIndicators(sIUSNIDs.ToString(), this.UserPrefences.Indicator.ShowIUS);
                RetVal = this._DBConnection.ExecuteDataTable(Sql);
            }
            catch (Exception)
            {
                RetVal = null;
            }

            return RetVal;
        }

        private DataTable AutoSelectIndicatorOrIUS(bool isForTree, ICType icType)
        {
            DataTable RetVal = new DataTable();
            string Sql = string.Empty;
            string SelectedAreaNIds = this.UserPrefences.UserSelection.AreaNIds;
            string SelectedTimeperiodNIds = this.UserPrefences.UserSelection.TimePeriodNIds;
            string SelectedSourceNIds = this.UserPrefences.UserSelection.SourceNIds;

            try
            {
                // -- AUTO SELECT the source nids on the basis of the selected Time,Area & Source
                if (!this._UseArea)
                    SelectedAreaNIds = string.Empty;
                if (!this._UseTime)
                    SelectedTimeperiodNIds = string.Empty;
                if (!this._UseSource)
                    SelectedSourceNIds = string.Empty;

                if (isForTree)
                {

                    if (this._UserPrefences.Indicator.ShowIUS)
                    {

                        Sql = this.SqlQueries.IUS.GetAutoSelectIUS(SelectedTimeperiodNIds, SelectedAreaNIds, SelectedSourceNIds, 0, icType);
                    }
                    else
                    {
                        Sql = this.SqlQueries.Indicators.GetAutoSelectIndicator(SelectedTimeperiodNIds, SelectedAreaNIds, SelectedSourceNIds, 0, icType);
                    }
                }
                else
                {
                    if (this._UserPrefences.Indicator.ShowIUS)
                    {
                        Sql = this.SqlQueries.IUS.GetAutoSelectIUS(SelectedTimeperiodNIds, SelectedAreaNIds, SelectedSourceNIds, this._SelectedNid, icType);
                    }
                    else
                    {
                        Sql = this.SqlQueries.Indicators.GetAutoSelectIndicator(SelectedTimeperiodNIds, SelectedAreaNIds, SelectedSourceNIds, this._SelectedNid, icType);
                    }
                }

                RetVal = this._DBConnection.ExecuteDataTable(Sql);
            }
            catch (Exception)
            {
                RetVal = null;
            }

            return RetVal;
        }

        private string GetIUSNids(string selectedICNid)
        {
            string RetVal = string.Empty;
            DataTable Table;
            try
            {

                if (selectedICNid == "-1")
                {
                    selectedICNid = "";
                }

                Table = this._DBConnection.ExecuteDataTable(this.SqlQueries.IUS.GetDistinctIUSNidWithData(this._IndicatorClassificationType, selectedICNid));

                foreach (DataRow Row in Table.Rows)
                {
                    if (!string.IsNullOrEmpty(RetVal))
                    {
                        RetVal += ",";
                    }
                    RetVal += Row[IndicatorClassificationsIUS.IUSNId].ToString();
                }
                Table.Dispose();
            }
            catch (Exception ex)
            {
                RetVal = string.Empty;
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        private DataTable GetSubgroupTable(int indicatorNId, int unitNId, string subgroupValNId)
        {
            DataTable RetVal = null;
            DataTable SubgroupTable = null;
            bool Found = false;
            try
            {
                // Get Subgroupval based on IUS ON / OFF
                if (!string.IsNullOrEmpty(subgroupValNId))
                {
                    RetVal = this._DBConnection.ExecuteDataTable(this.SqlQueries.IUS.GetIUSNIdByI_U_S(indicatorNId.ToString(), unitNId.ToString(), subgroupValNId));
                }
                else
                {
                    RetVal = this._DBConnection.ExecuteDataTable(this.SqlQueries.IUS.GetSubgroupValByIU(indicatorNId, unitNId));
                }

                // Data Exist column is fetched from Data table based on IUS / IU (without consideration of Area / Timeperiod)


                //-- Update the Data exists field value, if ShowWhereDataExists = true and if value exists against area, timeperiod and sourceNId.
                if (this.UserPrefences.Indicator.ShowWhereDataExists && (!string.IsNullOrEmpty(this.UserPrefences.UserSelection.AreaNIds) || !string.IsNullOrEmpty(this.UserPrefences.UserSelection.TimePeriodNIds) || !string.IsNullOrEmpty(this.UserPrefences.UserSelection.SourceNIds)))
                {
                    //-- Get the auto selected IUS.
                    SubgroupTable = this._DBConnection.ExecuteDataTable(this.SqlQueries.IUS.GetAutoSelectIUS(indicatorNId.ToString(), unitNId.ToString(), this.UserPrefences.UserSelection.AreaNIds, this.UserPrefences.UserSelection.TimePeriodNIds, this.UserPrefences.UserSelection.SourceNIds));

                    //-- Loop to set the value of DataExists field.
                    foreach (DataRow SubgroupRow in RetVal.Rows)
                    {
                        Found = false;
                        foreach (DataRow Row in SubgroupTable.Rows)
                        {
                            if (Convert.ToInt32(Row[Indicator_Unit_Subgroup.SubgroupValNId]) == Convert.ToInt32(SubgroupRow[Indicator_Unit_Subgroup.SubgroupValNId]))
                            {
                                Found = true;
                                break;
                            }
                        }
                        if (Found)
                        {
                            SubgroupRow[Indicator_Unit_Subgroup.DataExist] = true;
                        }
                        else
                        {
                            SubgroupRow[Indicator_Unit_Subgroup.DataExist] = false;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        #endregion

        #endregion

        #region "-- Protected --"

        #region "-- Methods --"

        protected override void SetColumnNames()
        {
            this._GlobalValueColumnName1 = Indicator.IndicatorGlobal;
            this._FirstColumnName = Indicator.IndicatorName;

            if (this.UserPrefences.Indicator.ShowIUS)
            {
                this._TagValueColumnName = Indicator_Unit_Subgroup.IUSNId;
                this._GlobalValueColumnName2 = Unit.UnitGlobal;
                this._SecondColumnName = Unit.UnitName;

                this._GlobalValueColumnName3 = SubgroupVals.SubgroupValGlobal;
                this._ThirdColumnName = SubgroupVals.SubgroupVal;
            }
            else
            {
                this._TagValueColumnName = TreeSelectionConstant.IUNId;

                this._GlobalValueColumnName2 = TreeSelectionConstant.SubgroupCountGlobal;
                this._SecondColumnName = TreeSelectionConstant.SubgroupCount;

                this._GlobalValueColumnName3 = TreeSelectionConstant.SelectSubgroupGlobal;
                this._ThirdColumnName = TreeSelectionConstant.SelectSubgroup;
            }
        }

        protected override void SetColumnHeaders()
        {
            //Get header string from language file and set column headers string 
            this._FirstColumnHeader = DILanguage.GetLanguageString("INDICATOR");

            if (this.UserPrefences.Indicator.ShowIUS)
            {
                this._SecondColumnHeader = DILanguage.GetLanguageString("UNIT");
            }
            else
            {
                this._SecondColumnHeader = DILanguage.GetLanguageString("SUBGROUP");
            }

            if (this.UserPrefences.Indicator.ShowIUS)
            {
                this._ThirdColumnHeader = DILanguage.GetLanguageString("SUBGROUP");
            }
            else
            {
                this._ThirdColumnHeader = DILanguage.GetLanguageString("NUMBER_OF_SUBGROUPS");
            }
        }

        protected override string GetSelectAllQuery()
        {
            string RetVal = string.Empty;
            string Nids = string.Empty;


            if (UserPrefences.Indicator.ShowIndicatorWithData)
            {
                //get IUSNIDs
                Nids = this.GetIUSNids(string.Empty);

                //get IUS
                if (this.UserPrefences.Indicator.ShowIUS)
                {
                    RetVal = this.SqlQueries.IUS.GetDistinctIUSWithData(Nids, DevInfo.Lib.DI_LibDAL.Queries.FieldSelection.Light);
                }
                else
                {                    //get indicators
                    RetVal = this.SqlQueries.IUS.GetDistinctIndicatorUnit(Nids, this.UserPrefences.Indicator.ShowIUS);
                }
            }
            else
            {
                RetVal = this.SqlQueries.Indicators.IndicatorUnit_GetAvailable(-1, this._IndicatorClassificationType, this.UserPrefences.Indicator.ShowIUS);
            }

            return RetVal;
        }

        protected override string GetAssocicatedRecordsQuery(int selectedNid, int selectedParentNid)
        {
            string RetVal = string.Empty;
            string Nids = string.Empty;


            if (UserPrefences.Indicator.ShowIndicatorWithData)
            {
                //get IUSNIDs
                Nids = this.GetIUSNids(selectedNid.ToString());

                //get IUS
                if (this.UserPrefences.Indicator.ShowIUS)
                {
                    if (this._IndicatorClassificationType == ICType.Source)
                    {
                        RetVal = this.SqlQueries.IUS.GetDistinctIUSWithData(Nids, DevInfo.Lib.DI_LibDAL.Queries.FieldSelection.Light);
                    }
                    else
                    {
                        //RetVal = this.SqlQueries.IUS.GetDistinctIUSWithData(Nids, DevInfo.Lib.DI_LibDAL.Queries.FieldSelection.Light);
                        RetVal = this.SqlQueries.IUS.GetDistinctOrderedIUSWithData(selectedNid, Nids, DevInfo.Lib.DI_LibDAL.Queries.FieldSelection.Light);
                    }
                }
                else
                {
                    //get indicators
                    RetVal = this.SqlQueries.IUS.GetIndicatorUnitWithData(Nids, DevInfo.Lib.DI_LibDAL.Queries.FieldSelection.Light);
                }
            }
            else
            {
                //get IUS
                if (this.UserPrefences.Indicator.ShowIUS)
                {
                    //if (this._IndicatorClassificationType == ICType.Source)
                    //{
                    //    RetVal = this.SqlQueries.IUS.GetDistinctIUSByIC(this._IndicatorClassificationType, selectedNid, DevInfo.Lib.DI_LibDAL.Queries.FieldSelection.Light);
                    //}
                    //else
                    //{
                    if (selectedNid == -1)
                    {
                        RetVal = this.SqlQueries.IUS.GetOrderedDistinctAllIUS(this._IndicatorClassificationType, DevInfo.Lib.DI_LibDAL.Queries.FieldSelection.Light);
                    }
                    else
                    {
                        RetVal = this.SqlQueries.IUS.GetDistinctOrderedIUSByIC(this._IndicatorClassificationType, selectedNid.ToString(), DevInfo.Lib.DI_LibDAL.Queries.FieldSelection.Light);
                    }
                    //}                    
                }
                else
                {//get indicators
                    RetVal = this.SqlQueries.Indicators.GetIndicatorUnitByIC(this._IndicatorClassificationType, selectedNid.ToString(), DevInfo.Lib.DI_LibDAL.Queries.FieldSelection.Light);
                }
            }

            return RetVal;
        }

        protected override string GetRecordsForSelectedNids(string nids)
        {
            string RetVal = string.Empty;
            //get IUS
            if (this.UserPrefences.Indicator.ShowIUS)
            {
                RetVal = this.SqlQueries.IUS.GetIUS(FilterFieldType.NId, nids, FieldSelection.Heavy);
            }
            else
            {//get indicators                
                RetVal = this.SqlQueries.Indicators.GetDistinctIndicatorUnit(nids, this._UserPrefences.Indicator.ShowIUS);
                //RetVal = this.SqlQueries.Indicators.GetIndicator(FilterFieldType.NId, nids, FieldSelection.Heavy);
            }
            return RetVal;
        }

        protected override string GetAssocicatedRecordsQuery(string selectedNids)
        {
            string RetVal = string.Empty;
            try
            {
                //get IUS from indicator NId
                if (this.UserPrefences.Indicator.ShowIUS)
                {
                    RetVal = this.SqlQueries.IUS.GetOrderedDistinctIUS(this._IndicatorClassificationType, selectedNids, FieldSelection.Light);
                }
                else
                {
                    //get indicators from ISUNId
                    RetVal = this.SqlQueries.Indicators.GetDistinctIndicatorUnit(selectedNids, this.UserPrefences.Indicator.ShowIUS);
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Methods --"

        /// <summary>
        /// Returns records for  auto select option of treeview.
        /// </summary>
        /// <returns></returns>
        public override DataTable GetAllRecordsForTreeAutoSelect()
        {
            DataTable RetVal = new DataTable();

            RetVal = this.AutoSelectIndicatorOrIUS(-1, this.IndicatorClassificationType);

            //RetVal = this.AutoSelectIndicatorOrIUS(true, this.IndicatorClassificationType);

            return RetVal;
        }

        /// <summary>
        /// Returns records for  auto select option of available list.
        /// </summary>
        /// <returns></returns>
        public override DataTable GetAutoSelectRecordsForAvailableList(string availableItemsNid)
        {
            DataTable RetVal;

            RetVal = this._DBConnection.ExecuteDataTable(this.GetAutoSelectQuery(this._SelectedNid, availableItemsNid));

            //RetVal = this.AutoSelectIndicatorOrIUS(false, this.IndicatorClassificationType);

            return RetVal;
        }

        /// <summary>
        /// Update the data table with IUNID and select subgroup columns.
        /// </summary>
        /// <param name="iuTable"></param>
        public override void UpdataDataTableBeforeCreatingListviewItem(DataTable iuTable)
        {
            if (!this.UserPrefences.Indicator.ShowIUS)
            {
                iuTable.Columns.Add(TreeSelectionConstant.IUNId);

                //-- Add the fields for second column of the listview
                iuTable.Columns.Add(TreeSelectionConstant.SelectSubgroup);
                iuTable.Columns.Add(TreeSelectionConstant.SelectSubgroupGlobal, typeof(System.Boolean));

                //-- Add the fields for third column of the listview
                iuTable.Columns.Add(TreeSelectionConstant.SubgroupCount);
                iuTable.Columns.Add(TreeSelectionConstant.SubgroupCountGlobal, typeof(System.Boolean));

                iuTable.Columns[Indicator.IndicatorName].MaxLength = iuTable.Columns[Indicator.IndicatorName].MaxLength + iuTable.Columns[Unit.UnitName].MaxLength + 3;


                foreach (DataRow Row in iuTable.Rows)
                {
                    Row[Indicator.IndicatorName] = Row[Indicator.IndicatorName].ToString() + ", " + Row[Unit.UnitName].ToString();
                    Row[TreeSelectionConstant.IUNId] = Row[Indicator.IndicatorNId].ToString() + "," + Row[Unit.UnitNId].ToString() + ",1";

                    Row[TreeSelectionConstant.SelectSubgroup] = string.Empty;
                    Row[TreeSelectionConstant.SelectSubgroupGlobal] = false;

                    Row[TreeSelectionConstant.SubgroupCount] = string.Empty;
                    Row[TreeSelectionConstant.SubgroupCountGlobal] = false;
                }
            }
        }

        /// <summary>
        /// Get the IUSNIds on the basis of IUNId
        /// </summary>
        /// <param name="iuNIds"></param>
        /// <returns></returns>
        public override List<string> GetIUSNIds(string iuNIds, bool checkUserSelection, bool selectSingleTon)
        {
            List<string> RetVal = new List<string>();
            DataTable SubgroupTable;
            string[] IUNId = new string[0];
            string UserSelectionIUSNIds = this._UserPrefences.UserSelection.IndicatorNIds;
            bool IUSFound = false;

            try
            {
                UserSelectionIUSNIds = UserSelectionIUSNIds.Insert(0, ",");
                UserSelectionIUSNIds += ",";

                IUNId = DICommon.SplitString(iuNIds, ",");
                SubgroupTable = this.GetSubgroupTable(Convert.ToInt32(IUNId[0]), Convert.ToInt32(IUNId[1]), this._TotalNId);

                if (this.UserPrefences.Indicator.SpecialSubgroupHandling == false)
                {
                    selectSingleTon = false;
                }
                else
                {
                    if (this.UserPrefences.Indicator.SelectAllSubgroups)
                    {
                        selectSingleTon = false;
                    }
                }

                if (SubgroupTable.Rows.Count == 1)
                {
                    if (selectSingleTon && Convert.ToBoolean(SubgroupTable.Rows[0][Indicator_Unit_Subgroup.DataExist]))
                    {
                        RetVal.Add(SubgroupTable.Rows[0][Indicator_Unit_Subgroup.IUSNId].ToString());
                    }
                    else if (!selectSingleTon)
                    {
                        RetVal.Add(SubgroupTable.Rows[0][Indicator_Unit_Subgroup.IUSNId].ToString());
                    }
                }
                else
                {
                    foreach (DataRow Row in SubgroupTable.Rows)
                    {
                        if (checkUserSelection && UserSelectionIUSNIds.Contains("," + Row[Indicator_Unit_Subgroup.IUSNId].ToString() + ","))
                        {
                            IUSFound = true;
                            break;
                        }

                        if (selectSingleTon && DICommon.SplitString(Row[Indicator_Unit_Subgroup.SubgroupNids].ToString(), ",").Length == 1 && Convert.ToBoolean(Row[Indicator_Unit_Subgroup.DataExist]))
                        {
                            RetVal.Add(Row[Indicator_Unit_Subgroup.IUSNId].ToString());
                        }
                        else if (!selectSingleTon && (this.UserPrefences.Indicator.SelectAllSubgroups || !this.UserPrefences.Indicator.SpecialSubgroupHandling))
                        {
                            if (this.UserPrefences.Indicator.ShowWhereDataExists && Convert.ToBoolean(Row[Indicator_Unit_Subgroup.DataExist]))
                            {
                                RetVal.Add(Row[Indicator_Unit_Subgroup.IUSNId].ToString());
                            }
                            else
                            {
                                RetVal.Add(Row[Indicator_Unit_Subgroup.IUSNId].ToString());
                            }
                        }
                        else if (!selectSingleTon && !checkUserSelection)
                        {
                            RetVal.Add(Row[Indicator_Unit_Subgroup.IUSNId].ToString());
                        }
                    }
                }
                this._TotalNId = string.Empty;

                if (IUSFound)
                {
                    RetVal.Clear();
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        public override List<string> GetSubgroupDimensions(string iuNId, string IUSNIds)
        {
            List<string> RetVal = new List<string>();
            IDataReader SubgroupReader;
            DataTable SubgroupTable = null;
            DataTable SubgroupTableCount = null;
            DataTable IUTable = null;
            DataRow[] Rows = new DataRow[0];
            string[] IUNIds = new string[0];
            int SingleTonSubgroupNidsCount = 0;
            int SubgroupCount = 0;
            bool TotalFound = false;
            this._TotalNId = string.Empty;
            string SelectedSubgroupVals = string.Empty;

            try
            {
                //-- Get the Indicator and Unit NId
                IUNIds = DICommon.SplitString(iuNId, ",");

                //-- If IU is already exists in the selection and get the IUS for the selected IUS NIds.
                IUTable = this._DBConnection.ExecuteDataTable(this.SqlQueries.IUS.GetDistinctIndicatorUnit(IUSNIds, true));

                if (IUTable.Rows.Count > 0)
                {
                    Rows = IUTable.Select(Indicator.IndicatorNId + " =  " + Convert.ToInt32(IUNIds[0]) + " AND " + Unit.UnitNId + " = " + Convert.ToInt32(IUNIds[1]));

                    //-- Get the all IUS combinations
                    SubgroupTable = this._DBConnection.ExecuteDataTable(this.SqlQueries.IUS.GetSubgroupValByIU(Convert.ToInt32(IUNIds[0]), Convert.ToInt32(IUNIds[1])));

                    //-- If only 1 IUS was selected
                    if (Rows.Length == 1)
                    {
                        RetVal.Add(Rows[0][SubgroupVals.SubgroupVal].ToString());
                        RetVal.Add("1/" + SubgroupTable.Rows.Count.ToString() + " " + DILanguage.GetLanguageString("SELECTED"));
                        if (SubgroupTable.Rows.Count > 1)
                        {
                            //-- Enable the link and user can click on the text
                            RetVal.Add("1");
                        }
                        else
                        {
                            //-- disable the link and user will not be able to click on the text
                            RetVal.Add("0");
                        }
                    }
                    else if (Rows.Length > 0)
                    {

                        SubgroupTableCount = this._DBConnection.ExecuteDataTable(this.SqlQueries.IUS.GetSubgroupValByIU(Convert.ToInt32(IUNIds[0]), Convert.ToInt32(IUNIds[1])));
                        SubgroupTable = this._DBConnection.ExecuteDataTable(this.SqlQueries.IUS.GetIUS(FilterFieldType.NId, IUSNIds, FieldSelection.Light));
                        Rows = SubgroupTable.Select(Indicator.IndicatorNId + " =  " + Convert.ToInt32(IUNIds[0]) + " AND " + Unit.UnitNId + " = " + Convert.ToInt32(IUNIds[1]));

                        foreach (DataRow Row in Rows)
                        {
                            SelectedSubgroupVals += "," + Row[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.SubgroupVals.SubgroupVal].ToString();
                        }

                        if (SelectedSubgroupVals.Length > TreeSelectionConstant.SubgroupValLength)
                        {
                            RetVal.Add(SelectedSubgroupVals.Substring(1, TreeSelectionConstant.SubgroupValLength - 1) + "...");
                        }
                        else
                        {
                            RetVal.Add(SelectedSubgroupVals.Substring(1));
                        }

                        RetVal.Add(Rows.Length.ToString() + "/" + SubgroupTableCount.Rows.Count.ToString() + " " + DILanguage.GetLanguageString("SELECTED"));
                        RetVal.Add("1");
                    }
                }

                //-- If no previous selection was done by the user
                if (RetVal.Count == 0)
                {
                    if (this.UserPrefences.Indicator.SpecialSubgroupHandling == false)
                    {
                        //-- Get all SubgroupVal information of the IU.
                        SubgroupTable = this.GetSubgroupTable(Convert.ToInt32(IUNIds[0]), Convert.ToInt32(IUNIds[1]), string.Empty);

                        if (this.UserPrefences.Indicator.ShowWhereDataExists)
                        {
                            Rows = SubgroupTable.Select(Indicator_Unit_Subgroup.DataExist + " = true");
                        }
                        else
                        {
                            Rows = SubgroupTable.Select(Indicator_Unit_Subgroup.IUSNId + " > 0");
                        }

                        foreach (DataRow Row in Rows)
                        {
                            SelectedSubgroupVals += "," + Row[SubgroupVals.SubgroupVal].ToString();
                            SubgroupCount += 1;
                        }
                        if (!string.IsNullOrEmpty(SelectedSubgroupVals))
                        {
                            SelectedSubgroupVals = SelectedSubgroupVals.Substring(1);
                        }

                        RetVal.Add(SelectedSubgroupVals);
                        RetVal.Add(SubgroupCount.ToString() + "/" + SubgroupTable.Rows.Count.ToString() + " " + DILanguage.GetLanguageString("SELECTED"));
                        //-- Enable / Disable and user not able to click on the text
                        if (Rows.Length == 1 && SubgroupCount == 1)
                        {
                            RetVal.Add("0");
                        }
                        else
                        {
                            RetVal.Add("1");
                        }
                    }
                    else
                    {

                        if (this.UserPrefences.Indicator.SelectAllSubgroups)
                        {
                            //-- Get all SubgroupVal information of the IU.
                            SubgroupTable = this.GetSubgroupTable(Convert.ToInt32(IUNIds[0]), Convert.ToInt32(IUNIds[1]), string.Empty);

                            if (this.UserPrefences.Indicator.ShowWhereDataExists)
                            {
                                Rows = SubgroupTable.Select(Indicator_Unit_Subgroup.DataExist + " = true");
                            }
                            else
                            {
                                Rows = SubgroupTable.Select(Indicator_Unit_Subgroup.IUSNId + " > 0");
                            }

                            foreach (DataRow Row in Rows)
                            {
                                SelectedSubgroupVals += "," + Row[SubgroupVals.SubgroupVal].ToString();
                                SubgroupCount += 1;
                            }
                            if (!string.IsNullOrEmpty(SelectedSubgroupVals))
                            {
                                SelectedSubgroupVals = SelectedSubgroupVals.Substring(1);
                            }

                            RetVal.Add(SelectedSubgroupVals);
                            RetVal.Add(SubgroupCount.ToString() + "/" + SubgroupTable.Rows.Count.ToString() + " " + DILanguage.GetLanguageString("SELECTED"));
                            //-- Enable / Disable and user not able to click on the text
                            if (Rows.Length == 1 && SubgroupCount == 1)
                            {
                                RetVal.Add("0");
                            }
                            else
                            {
                                RetVal.Add("1");
                            }
                        }
                        else
                        {
                            //-- Get all SubgroupVal information of the IU.
                            SubgroupTable = this.GetSubgroupTable(Convert.ToInt32(IUNIds[0]), Convert.ToInt32(IUNIds[1]), string.Empty);
                            Rows = SubgroupTable.Select(Indicator_Unit_Subgroup.DataExist + " = true");

                            //-- Get the subgroupVal, if single subgroupVal exists against the IU.
                            if (SubgroupTable.Rows.Count == 1)
                            {
                                if (Convert.ToBoolean(SubgroupTable.Rows[0][Indicator_Unit_Subgroup.DataExist]))
                                {
                                    RetVal.Add(SubgroupTable.Rows[0][SubgroupVals.SubgroupVal].ToString());
                                    RetVal.Add("1/1 " + DILanguage.GetLanguageString("SELECTED"));
                                    //-- Disable and user not able to click on the text
                                    RetVal.Add("0");
                                }
                                else
                                {
                                    RetVal.Add(string.Empty);
                                    RetVal.Add("0/1 " + DILanguage.GetLanguageString("SELECTED"));
                                    //-- Disable and user not able to click on the text
                                    RetVal.Add("1");
                                }
                            }
                            else
                            {
                                //-- Get the TOTAL subgroup, if exists against the IU
                                foreach (DataRow Row in SubgroupTable.Rows)
                                {
                                    if (Row[SubgroupVals.SubgroupValGId].ToString().ToLower() == this._UserPrefences.General.TotalGId.ToLower() && Convert.ToBoolean(Row[Indicator_Unit_Subgroup.DataExist]))
                                    {
                                        this._TotalNId = Row[SubgroupVals.SubgroupValNId].ToString();
                                        RetVal.Add(Row[SubgroupVals.SubgroupVal].ToString());
                                        RetVal.Add("1/" + SubgroupTable.Rows.Count.ToString() + " " + DILanguage.GetLanguageString("SELECTED"));
                                        RetVal.Add("1");
                                        TotalFound = true;
                                        break;
                                    }
                                }

                                if (!TotalFound)
                                {
                                    //-- Get the Subgroup, if all the above condition fails.                           
                                    foreach (DataRow Row in Rows)
                                    {
                                        if (DICommon.SplitString(Row[Indicator_Unit_Subgroup.SubgroupNids].ToString(), ",").Length == 1)
                                        {
                                            SelectedSubgroupVals += "," + Row[SubgroupVals.SubgroupVal].ToString();
                                            SingleTonSubgroupNidsCount += 1;
                                        }
                                    }

                                    if (!string.IsNullOrEmpty(SelectedSubgroupVals))
                                    {
                                        SelectedSubgroupVals = SelectedSubgroupVals.Substring(1);
                                        if (SelectedSubgroupVals.Length > TreeSelectionConstant.SubgroupValLength)
                                        {
                                            RetVal.Add(SelectedSubgroupVals.Substring(0, TreeSelectionConstant.SubgroupValLength - 2) + "...");
                                        }
                                        else
                                        {
                                            RetVal.Add(SelectedSubgroupVals);
                                        }
                                        RetVal.Add(SingleTonSubgroupNidsCount.ToString() + "/" + SubgroupTable.Rows.Count.ToString() + " " + DILanguage.GetLanguageString("SELECTED"));
                                        RetVal.Add("1");
                                    }
                                    else
                                    {
                                        RetVal.Add("");
                                        RetVal.Add(SingleTonSubgroupNidsCount.ToString() + "/" + SubgroupTable.Rows.Count.ToString() + " " + DILanguage.GetLanguageString("SELECTED"));
                                        RetVal.Add("1");
                                    }
                                }
                            }
                        }
                    }
                }

                if (RetVal.Count == 0)
                {
                    RetVal.Add("");
                    RetVal.Add("0/0" + " " + DILanguage.GetLanguageString("SELECTED"));
                    RetVal.Add("0");
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        /// Get the subgroup information that need to be render on the IC list along with indicator and unit name.
        /// </summary>
        /// <param name="iuNId"></param>
        /// <param name="IUSNIds"></param>
        /// <returns></returns>
        public override List<string> GetSubgroupDimensionsWithIU(string iuNId, string IUSNIds)
        {
            List<string> RetVal = new List<string>();
            string[] IUNIds = new string[0];
            IDataReader ValueReader;
            try
            {
                IUNIds = DICommon.SplitString(iuNId, Delimiter.NUMERIC_DELIMITER);
                RetVal = this.GetSubgroupDimensions(iuNId, IUSNIds);

                if (IUNIds.Length >= 2)
                {
                    //-- Get the indicator name
                    ValueReader = this._DBConnection.ExecuteReader(this.SqlQueries.Indicators.GetIndicator(FilterFieldType.NId, IUNIds[0], FieldSelection.Light));
                    if (ValueReader.Read())
                    {
                        RetVal.Add(ValueReader[Indicator.IndicatorName].ToString());
                    }
                    ValueReader.Close();

                    //-- Get the unit name
                    ValueReader = this._DBConnection.ExecuteReader(this.SqlQueries.Unit.GetUnit(FilterFieldType.NId, IUNIds[1]));
                    if (ValueReader.Read())
                    {
                        RetVal.Add(ValueReader[Unit.UnitName].ToString());
                    }
                    ValueReader.Close();
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        public override void UpdateIndicatorSelectedDetails(int indicatorNId, int unitNId,  string selectionDetails, bool addNewSelection)
        {
            string[] IndicatorSelectionDetails = new string[0];
            string IndicatorSelectionDetail = this.GetIndicatorSelectionDetails(indicatorNId, unitNId);
            bool SelectionFound = false;

            IndicatorSelectionDetails = DICommon.SplitString(this.UserPrefences.UserSelection.IndicatorUnitSelectionDetails, Delimiter.TEXT_SEPARATOR);
            for (int Index = 0; Index < IndicatorSelectionDetails.Length; Index++)
            {
                if (addNewSelection && IndicatorSelectionDetails[Index] == IndicatorSelectionDetail)
                {
                    //-- Selection found 
                    SelectionFound = true;
                    break;
                }
                if (!addNewSelection && IndicatorSelectionDetails[Index] == IndicatorSelectionDetail)
                {
                    IndicatorSelectionDetails[Index] = string.Empty;
                    break;
                }
            }

            if (addNewSelection && !SelectionFound)
            {
                //-- Add the IU selection in the user selection
                if (!string.IsNullOrEmpty(this.UserPrefences.UserSelection.IndicatorUnitSelectionDetails))
                {
                    this.UserPrefences.UserSelection.IndicatorUnitSelectionDetails += Delimiter.TEXT_SEPARATOR;
                }
                this.UserPrefences.UserSelection.IndicatorUnitSelectionDetails += selectionDetails;
            }
            else if (!addNewSelection)
            {
                //-- Remove the IU selection in the user selection
                this.UserPrefences.UserSelection.IndicatorUnitSelectionDetails = string.Empty;
                foreach (string IndicatorUnitSelectionDetail in IndicatorSelectionDetails)
                {
                    if (!string.IsNullOrEmpty(IndicatorUnitSelectionDetail))
                    {
                        if (!string.IsNullOrEmpty(this.UserPrefences.UserSelection.IndicatorUnitSelectionDetails))
                        {
                            this.UserPrefences.UserSelection.IndicatorUnitSelectionDetails += Delimiter.TEXT_SEPARATOR;
                        }
                        this.UserPrefences.UserSelection.IndicatorUnitSelectionDetails += IndicatorUnitSelectionDetail;
                    }
                }

                if (!string.IsNullOrEmpty(selectionDetails.Trim()))
                {
                    if (!string.IsNullOrEmpty(this.UserPrefences.UserSelection.IndicatorUnitSelectionDetails))
                    {
                        this.UserPrefences.UserSelection.IndicatorUnitSelectionDetails += Delimiter.TEXT_SEPARATOR;
                    }
                    this.UserPrefences.UserSelection.IndicatorUnitSelectionDetails += selectionDetails;
                }
            }
        }

        public override string GetIndicatorSelectionDetails(int indicatorNId, int unitNId)
        {
            string RetVal = string.Empty;
            try
            {
                string[] Selections = new string[0];
                string[] IndicatorUnitSelectionDetails = new string[0];
                IndicatorUnitSelectionDetails = DICommon.SplitString(this.UserPrefences.UserSelection.IndicatorUnitSelectionDetails, Delimiter.TEXT_SEPARATOR);

                foreach (string IndicatorUnitSelectionDetail in IndicatorUnitSelectionDetails)
                {
                    Selections = DICommon.SplitString(IndicatorUnitSelectionDetail, Delimiter.NUMERIC_SEPARATOR);
                    Selections = DICommon.SplitString(Selections[0], Delimiter.NUMERIC_DELIMITER);
                    if (Convert.ToInt32(Selections[0]) == indicatorNId && Convert.ToInt32(Selections[1]) == unitNId)
                    {
                        RetVal = IndicatorUnitSelectionDetail;
                        break;
                    }
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        #endregion

        #endregion

    }
}