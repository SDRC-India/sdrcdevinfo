using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.Controls.TreeSelectionBAL;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.Controls.DISelectionBAL
{
    /// <summary>
    /// Provides methods to show IUS in DISelection Control
    /// </summary>
    public class IUSSelectionSource : BaseSelection
    {

    #region "-- private/protected --"

    #region "-- Variables --"

    #endregion

    #region "-- Methods --"

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

        private DataTable AutoSelectIndicatorOrIUS(int icNId, ICType icType)
        {
            DataTable RetVal = new DataTable();
            try
            {
                string Sql = string.Empty;
                DataTable IUSDt;
                string[] DistinctColumns = new string[1];
                StringBuilder sbIUSNIds = new StringBuilder();
                string sIUSNIDs = string.Empty;
                string SelectedTimeperiodNIds = this.UserPrefences.UserSelection.TimePeriodNIds;
                string SelectedAreaNIds = this.UserPrefences.UserSelection.AreaNIds;
                string SelectedSourceNIds = this.UserPrefences.UserSelection.SourceNIds;

                // -- Get All Comma seperated IUSNIDs for the selected IC Type from the Available list 
                DataTable IUSFromICTypes;
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
                    sbIUSNIds.Append("," + IUSNId[IndicatorClassificationsIUS.IUSNId]);
                }
                // -- Remove extra Comma 
                if (sbIUSNIds.Length > 0)
                {
                    sIUSNIDs = sbIUSNIds.ToString().Substring(1);
                }

                // -- AUTO SELECT the DISTINCT IUSNIDs on the basis of the selected Time, Area and Available IUS/I in the Available list 
                if (!this.UseArea)
                    SelectedAreaNIds = string.Empty;
                if (!this.UseTime)
                    SelectedTimeperiodNIds = string.Empty;
                if (!this.UseSource)
                    SelectedSourceNIds = string.Empty;

                if (string.IsNullOrEmpty(sIUSNIDs))
                {
                    sIUSNIDs = "0";
                }

                Sql = this.SqlQueries.Indicators.GetAutoSelectByTimePeriodAreaSource(SelectedTimeperiodNIds, SelectedAreaNIds, sIUSNIDs.ToString(), SelectedSourceNIds);

                IDataReader rdIUS;
                rdIUS = this._DBConnection.ExecuteReader(Sql);
                sbIUSNIds.Length = 0;
                while ((rdIUS.Read()))
                {
                    sbIUSNIds.Append(",");
                    sbIUSNIds.Append(rdIUS[Indicator_Unit_Subgroup.IUSNId]);
                }
                sIUSNIDs = string.Empty;
                if (sbIUSNIds.Length > 0)
                {
                    sIUSNIDs = sbIUSNIds.ToString().Substring(1);
                }
                rdIUS.Close();

                if (this.UserPrefences.Indicator.ShowIUS)
                {
                    Sql = this.SqlQueries.Indicators.GetIndicators(sIUSNIDs.ToString(), this.UserPrefences.Indicator.ShowIUS);
                }
                else
                {
                    Sql = this.SqlQueries.IUS.GetDistinctIndicatorUnit(sIUSNIDs.ToString(), this.UserPrefences.Indicator.ShowIUS);
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

        protected internal override DataView processDataView(System.Data.DataView dv)
        {
           // throw new Exception("The method or operation is not implemented.");
            return dv;
        }
        
        protected override void SetColumnNames()
        {

            if (this.UserPrefences.Indicator.ShowIUS)
            {
                this._TagValueColumnName = Indicator_Unit_Subgroup.IUSNId;
            }
            else
            {
                this._TagValueColumnName = Indicator_Unit_Subgroup.IndicatorNId;
            }

            this._GlobalValueColumnName2 = Unit.UnitGlobal;
            this._SecondColumnName = Unit.UnitName;          

            this._GlobalValueColumnName1 = Indicator.IndicatorGlobal;
            this._GlobalValueColumnName3 = SubgroupVals.SubgroupValGlobal;
            this._FirstColumnName = Indicator.IndicatorName;
            this._ThirdColumnName = SubgroupVals.SubgroupVal;
        }

        protected override void SetColumnHeaders()
        {
            //Get header string from language file and set column headers string 
            this._FirstColumnHeader = DILanguage.GetLanguageString("INDICATOR");
            this._SecondColumnHeader = DILanguage.GetLanguageString("UNIT");
            this._ThirdColumnHeader = DILanguage.GetLanguageString("SUBGROUP");
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
                    if (this._IndicatorClassificationType == ICType.Source)
                    {
                        RetVal = this.SqlQueries.IUS.GetDistinctIUSByIC(this._IndicatorClassificationType, selectedNid, DevInfo.Lib.DI_LibDAL.Queries.FieldSelection.Light);
                    }
                    else
                    {
                        RetVal = this.SqlQueries.IUS.GetDistinctOrderedIUSByIC(this._IndicatorClassificationType, selectedNid.ToString(), DevInfo.Lib.DI_LibDAL.Queries.FieldSelection.Light);
                    }
                }
                else
                {//get indicators
                    RetVal = this.SqlQueries.Indicators.GetIndicatorUnitByIC(this._IndicatorClassificationType, selectedNid.ToString(), DevInfo.Lib.DI_LibDAL.Queries.FieldSelection.Light);
                }
            }

            return RetVal;
        }

        protected override string GetSelectAllQuery()
        {
            string RetVal = string.Empty;
            
            RetVal = this.SqlQueries.IUS.GetIUS(FilterFieldType.None,string.Empty, FieldSelection.Light);
            
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
                RetVal = this.SqlQueries.IUS.GetDistinctIndicatorUnit(nids, this._UserPrefences.Indicator.ShowIUS);
                //RetVal = this.SqlQueries.Indicators.GetIndicator(FilterFieldType.NId, nids, FieldSelection.Heavy);
            }
            return RetVal;
        }

        protected override string GetAssocicatedRecordsQuery(string selectedNids)
        {
            //Do nothing
            return string.Empty;
        }

    #endregion  
   
    #endregion

    #region "-- public --"

    #region "-- Variables --"

    #endregion

    #region "-- Methods --"

        /// <summary>
        /// return DataTable for all IUS
        /// </summary>
        /// <returns></returns>
        public override DataTable GetAllRecordsTable()
        {
            DataTable RetVal = null;
            try
            {
                RetVal = this._DBConnection.ExecuteDataTable(this.GetSelectAllQuery());                

            }
            catch (Exception)
            {
                // do nothing
            }
            return RetVal;
        }

        /// <summary>
        /// Returns records for  auto select option of available list.
        /// </summary>
        /// <returns></returns>
        public override DataTable GetAutoSelectRecordsForAvailableList(string availableItemsNid)
        {
            DataTable RetVal = null;

            try
            {
                RetVal = this._DBConnection.ExecuteDataTable(this.SqlQueries.IUS.GetAutoSelectByTimePeriodArea(this.UserPrefences.UserSelection.TimePeriodNIds , this.UserPrefences.UserSelection.AreaNIds ,-1,FieldSelection.Light));
               
            }
            catch { }

            return RetVal;
        }

        /// <summary>
        /// Returns records for  auto select option of treeview.
        /// </summary>
        /// <returns></returns>
        public override DataTable GetAllRecordsForTreeAutoSelect()
        {
            DataTable RetVal = new DataTable();

            RetVal = this.AutoSelectIndicatorOrIUS(-1, this.IndicatorClassificationType);
           

            return RetVal;
        }

        /// <summary>
        /// N/A
        /// </summary>
        /// <param name="iuTable"></param>
        public override void UpdataDataTableBeforeCreatingListviewItem(System.Data.DataTable iuTable)
        {
            //-- Do Nothing
        }

        /// <summary>
        /// Get the IUSNIds on the basis of IUNId
        /// </summary>
        /// <param name="iuNIds"></param>
        /// <returns></returns>
        public override List<string> GetIUSNIds(string iuNIds, bool checkUserSelection, bool selectSingleTon)
        {
            List<string> RetVal = new List<string>();
            try
            {
                IDataReader IUSReader;
                string[] IUNId = new string[0];
                string UserSelectionIUSNIds = this._UserPrefences.UserSelection.IndicatorNIds;
                bool IUSFound = false;

                UserSelectionIUSNIds = UserSelectionIUSNIds.Insert(0, ",");
                UserSelectionIUSNIds += ",";

                IUNId = DICommon.SplitString(iuNIds, ",");
                IUSReader = this._DBConnection.ExecuteReader(this.SqlQueries.IUS.GetIUSNIdByI_U_S(IUNId[0], IUNId[1], string.Empty));

                while (IUSReader.Read())
                {
                    if (checkUserSelection && UserSelectionIUSNIds.Contains("," + IUSReader[Indicator_Unit_Subgroup.IUSNId].ToString() + ","))
                    {
                        IUSFound = true;
                        break;
                    }
                    RetVal.Add(IUSReader[Indicator_Unit_Subgroup.IUSNId].ToString());
                }
                IUSReader.Close();

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

        /// <summary>
        /// Get subgroup dimension for Indicator and Unit or IUS
        /// </summary>
        /// <param name="iuNId"></param>
        /// <param name="IUSNIds"></param>
        /// <returns></returns>
        public override List<string> GetSubgroupDimensions(string iuNId, string IUSNIds)
        {
            List<string> RetVal = new List<string>();
            //-- NA
            return RetVal;
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
