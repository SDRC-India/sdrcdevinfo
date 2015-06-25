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
    /// Helps in getting Indicator classifications  for TreeSelectionControl
    /// </summary>
    public class ICSelectionSource : BaseSelectionSource
    {
        #region "-- Private --"

        #region "-- Methods --"



        #endregion

        #endregion

        #region "-- Protected --"

        #region "-- Methods --"

        protected override void SetColumnNames()
        {
            this._TagValueColumnName = IndicatorClassifications.ICNId;

            this._GlobalValueColumnName1 = IndicatorClassifications.ICGlobal;
            this._GlobalValueColumnName2 = string.Empty;
            this._GlobalValueColumnName3 = string.Empty;

            this._FirstColumnName = IndicatorClassifications.ICName;
            this._SecondColumnName = string.Empty;
            this._ThirdColumnName = string.Empty;

            if (this.ShowISBNNatureColumn)
            {
                this._SecondColumnName = IndicatorClassifications.ISBN;
                this._ThirdColumnName = IndicatorClassifications.Nature;
            }
        }

        protected override void SetColumnHeaders()
        {
            this._SecondColumnHeader = String.Empty;
            this._ThirdColumnHeader = String.Empty;

            //Get header string from language file and set column headers string 
            switch (this._IndicatorClassificationType)
            {
                case ICType.Sector:
                    this._FirstColumnHeader = DILanguage.GetLanguageString("SECTOR");
                    break;
                case ICType.Goal:
                    this._FirstColumnHeader = DILanguage.GetLanguageString("GOAL");
                    break;
                case ICType.CF:
                    this._FirstColumnHeader = DILanguage.GetLanguageString("FRAMEWORK");
                    break;
                case ICType.Theme:
                    this._FirstColumnHeader = DILanguage.GetLanguageString("THEME");
                    break;
                case ICType.Source:
                    this._FirstColumnHeader = DILanguage.GetLanguageString("SOURCE");

                    if (this.ShowISBNNatureColumn)
                    {
                        this._SecondColumnHeader = DILanguage.GetLanguageString("ISBN");
                        this._ThirdColumnHeader = DILanguage.GetLanguageString("NATURE");
                    }
                    break;
                case ICType.Institution:
                    this._FirstColumnHeader = DILanguage.GetLanguageString("INSTITUTION");
                    break;
                case ICType.Convention:
                    this._FirstColumnHeader = DILanguage.GetLanguageString("CONVENTION");
                    break;
                default:
                    break;
            }


        }

        protected override string GetSelectAllQuery()
        {
            string RetVal = string.Empty;
            string Nids = string.Empty;

            if (this._IndicatorClassificationType == ICType.Source)
            {
                RetVal = this.SqlQueries.IndicatorClassification.GetsSourcesWithoutPublishers();
            }
            else
            {
                RetVal = this.SqlQueries.IndicatorClassification.GetIC(DevInfo.Lib.DI_LibDAL.Queries.FilterFieldType.None, string.Empty, this.IndicatorClassificationType, FieldSelection.Light);
            }



            return RetVal;
        }

        protected override string GetAssocicatedRecordsQuery(int selectedNid, int selectedParentNid)
        {
            string RetVal = string.Empty;

            if (selectedNid == -1)
            {
                if (this._IndicatorClassificationType == ICType.Source)
                {
                    RetVal = this.SqlQueries.IndicatorClassification.GetsSourcesWithoutPublishers();
                }
                else
                {
                    RetVal = this.SqlQueries.IndicatorClassification.GetIC(DevInfo.Lib.DI_LibDAL.Queries.FilterFieldType.ParentNId, selectedNid.ToString(), this.IndicatorClassificationType, FieldSelection.Light);
                }
            }
            else
            {
                RetVal = this.SqlQueries.IndicatorClassification.GetIC(DevInfo.Lib.DI_LibDAL.Queries.FilterFieldType.ParentNId, selectedNid.ToString(), this.IndicatorClassificationType, FieldSelection.Light, true);
            }

            return RetVal;
        }

        protected override string GetRecordsForSelectedNids(string nids)
        {
            string RetVal = string.Empty;
            //get indicator classificaiton
            RetVal = this.SqlQueries.IndicatorClassification.GetIC(DevInfo.Lib.DI_LibDAL.Queries.FilterFieldType.NId, nids, this.IndicatorClassificationType, FieldSelection.Light,true);

            return RetVal;
        }

        protected override string GetAssocicatedRecordsQuery(string selectedNids)
        {
            //Do nothing
            return string.Empty;
        }

        public override void UpdataDataTableBeforeCreatingListviewItem(DataTable iuTable)
        {
            //-- Do Nothing
        }

        public override List<string> GetIUSNIds(string iuNIds, bool checkUserSelection, bool selectSingleTon)
        {
            //-- Do nothing
            return new List<string>();
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
            string SqlQuery = string.Empty;

            string SelectedIndicatorNIds = string.Empty;
            string SelectedTimeperiodNIds = this.UserPrefences.UserSelection.TimePeriodNIds;
            string SelectedAreaNIds = this.UserPrefences.UserSelection.AreaNIds;

            SelectedIndicatorNIds = this.UserPrefences.UserSelection.IndicatorNIds;

            // -- AUTO SELECT the source nids on the basis of the selected  Indicator/IUS, Time & Area in the Available list 
            if (!this._UseIndicator)
            {
                SelectedIndicatorNIds = string.Empty;
            }
            if (!this._UseTime)
                SelectedTimeperiodNIds = string.Empty;
            if (!this._UseArea)
                SelectedAreaNIds = string.Empty;


            SqlQuery = this.SqlQueries.Source.GetAutoSelectSource(SelectedIndicatorNIds, this._UserPrefences.UserSelection.ShowIUS, SelectedAreaNIds, SelectedTimeperiodNIds);

            RetVal = this._DBConnection.ExecuteDataTable(SqlQuery);

            return RetVal;
        }

        /// <summary>
        /// Returns records for  auto select option of available list.
        /// </summary>
        /// <returns></returns>
        public override DataTable GetAutoSelectRecordsForAvailableList(string availableItemsNid)
        {
            DataTable RetVal = new DataTable();
            string SqlQuery = string.Empty;
            string SelectedIndicatorNIds = string.Empty;
            string SelectedTimeperiodNIds = this.UserPrefences.UserSelection.TimePeriodNIds;
            string SelectedAreaNIds = this.UserPrefences.UserSelection.AreaNIds;


            SelectedIndicatorNIds = this.UserPrefences.UserSelection.IndicatorNIds;

            // -- AUTO SELECT the source nids on the basis of the selected  Indicator/IUS, Time & Area  
            if (!this._UseIndicator)
            {
                SelectedIndicatorNIds = string.Empty;
            }
            if (!this._UseTime)
                SelectedTimeperiodNIds = string.Empty;
            if (!this._UseArea)
                SelectedAreaNIds = string.Empty;


            if (this._SelectedNid > 0)
            {
                SqlQuery = this.SqlQueries.Source.GetAutoSelectSource(SelectedIndicatorNIds, this._UserPrefences.UserSelection.ShowIUS, SelectedAreaNIds, SelectedTimeperiodNIds, this._SelectedNid.ToString());
            }
            else
            {
                SqlQuery = this.SqlQueries.Source.GetAutoSelectSource(SelectedIndicatorNIds, this._UserPrefences.UserSelection.ShowIUS, SelectedAreaNIds, SelectedTimeperiodNIds);
            }

            RetVal = this._DBConnection.ExecuteDataTable(SqlQuery);
            return RetVal;
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
