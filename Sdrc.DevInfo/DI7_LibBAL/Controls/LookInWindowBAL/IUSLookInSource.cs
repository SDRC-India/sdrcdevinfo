using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using System.Data;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;

namespace DevInfo.Lib.DI_LibBAL.Controls.LookInWindowBAL
{
    /// <summary>
    /// Helps in getting IUS for LookInWindow control
    /// </summary>
    public class IUSLookInSource : BaseLookInSource
    {
        #region "-- Private --"

        #region "-- Methods --"


        private DI6SubgroupValInfo GetSubgroupValInfo(DataRow row)
        {

            DI6SubgroupValInfo RetVal;
            DataTable TempTable;

            try
            {
                RetVal = new DI6SubgroupValInfo();
                RetVal.Name = DICommon.RemoveQuotes(row[SubgroupVals.SubgroupVal].ToString());
                RetVal.GID = row[SubgroupVals.SubgroupValGId].ToString();
                RetVal.Global = Convert.ToBoolean(row[SubgroupVals.SubgroupValGlobal]);
                RetVal.Nid = Convert.ToInt32(row[SubgroupVals.SubgroupValNId]);

                ////get nids of age,sex,others and location
                //TempTable = this.SourceDBConnection.ExecuteDataTable(this.SourceDBQueries.Subgroup.GetSubgroupVals(FilterFieldType.NId, RetVal.Nid.ToString()));

                //if (!Microsoft.VisualBasic.Information.IsDBNull(TempTable.Rows[0][SubgroupVals.SubgroupValAge]))
                //{
                //    RetVal.Age.Nid = Convert.ToInt32(TempTable.Rows[0][SubgroupVals.SubgroupValAge]);
                //}

                //if (!Microsoft.VisualBasic.Information.IsDBNull(TempTable.Rows[0][SubgroupVals.SubgroupValSex]))
                //{
                //    RetVal.Sex.Nid = Convert.ToInt32(TempTable.Rows[0][SubgroupVals.SubgroupValSex]);
                //}

                //if (!Microsoft.VisualBasic.Information.IsDBNull(TempTable.Rows[0][SubgroupVals.SubgroupValOthers]))
                //{
                //    RetVal.Others.Nid = Convert.ToInt32(TempTable.Rows[0][SubgroupVals.SubgroupValOthers]);
                //}

                //if (!Microsoft.VisualBasic.Information.IsDBNull(TempTable.Rows[0][SubgroupVals.SubgroupValLocation]))
                //{
                //    RetVal.Location.Nid = Convert.ToInt32(TempTable.Rows[0][SubgroupVals.SubgroupValLocation]);
                //}
            }
            catch (Exception ex)
            {
                RetVal = null;
                ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }


        private UnitInfo GetUnitInfo(DataRow row)
        {

            UnitInfo RetVal;

            try
            {
                //get unit from source table
                RetVal = new UnitInfo();
                RetVal.Name = DICommon.RemoveQuotes(row[Unit.UnitName].ToString());
                RetVal.GID = row[Unit.UnitGId].ToString();
                RetVal.Global = Convert.ToBoolean(row[Unit.UnitGlobal]);
                RetVal.Nid = Convert.ToInt32(row[Unit.UnitNId]);

            }
            catch (Exception ex)
            {
                RetVal = null;
                ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }


        private IndicatorInfo GetIndicatorInfo(DataRow row)
        {
            IndicatorInfo RetVal;

            try
            {
                //get unit from source table
                RetVal = new IndicatorInfo();
                RetVal.Name = DICommon.RemoveQuotes(row[Indicator.IndicatorName].ToString());
                RetVal.GID = row[Indicator.IndicatorGId].ToString();
                RetVal.Global = Convert.ToBoolean(row[Indicator.IndicatorGlobal]);
                RetVal.Info = DICommon.RemoveQuotes(Convert.ToString(row[Indicator.IndicatorInfo]));
                RetVal.Nid = Convert.ToInt32(row[Indicator.IndicatorNId]);
            }
            catch (Exception ex)
            {
                RetVal = null;
                ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }

        #endregion

        #endregion

        #region"--Protected--"

        #region"--Method--"



        protected override string GetSqlQuery(string searchString)
        {
            string RetVal = string.Empty;
            string FilterString = string.Empty;

            if (!string.IsNullOrEmpty(searchString))
            {
                FilterString = " And I." + Indicator.IndicatorName + " LIKE '%" + searchString + "%' ";
                RetVal = this.SourceDBQueries.IUS.GetIUS(FilterFieldType.Search, "" + FilterString + "", FieldSelection.Heavy);
            }
            else
            {
                RetVal = this.SourceDBQueries.IUS.GetIUS(FilterFieldType.None, string.Empty, FieldSelection.Heavy);
            }

            return RetVal;
        }

        protected override void ProcessDataTable(ref System.Data.DataTable table)
        {
            //Dont implement this
        }

        #endregion

        #endregion


        #region "-- public --"

        #region "-- Methods --"

        #region "-- Import  --"

        /// <summary>
        /// Imports records from source database to target database/template
        /// </summary>
        /// <param name="selectedNids"></param>
        /// <param name="allSelected">Set true to import all records</param>
        public override void ImportValues(List<string> selectedNids, bool allSelected)
        {
            DataRow Row;
            IUSInfo IUSInfoObject;
            IUSBuilder IUSBuilderObject;
            int ProgressBarValue = 0;

            foreach (string Nid in selectedNids)
            {
                try
                {
                    Row = this.SourceTable.Select(this.TagValueColumnName + "=" + Nid)[0];

                    IUSInfoObject = new IUSInfo();
                    IUSBuilderObject = new IUSBuilder(this._TargetDBConnection, this._TargetDBQueries);

                    IUSInfoObject.IndicatorInfo = this.GetIndicatorInfo(Row);
                    IUSInfoObject.UnitInfo = this.GetUnitInfo(Row);
                    IUSInfoObject.SubgroupValInfo = this.GetSubgroupValInfo(Row);

                    IUSBuilderObject.ImportIUS(IUSInfoObject, this.SourceDBQueries, this.SourceDBConnection);

                    IUSBuilderObject.UpdateISDefaultSubgroup(IUSInfoObject.IndicatorInfo.Nid, IUSInfoObject.UnitInfo.Nid);

                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.ToString());
                }
                this.RaiseIncrementProgessBarEvent(ProgressBarValue);
                ProgressBarValue++;
            }
        }

        /// <summary>
        /// Sets the columns info like name , global value column name,etc
        /// </summary>
        public override void SetColumnsInfo()
        {
            this.Columns.Clear();
            this.Columns.Add(Indicator.IndicatorName, DILanguage.GetLanguageString(Constants.LanguageKeys.Indicator));
            this.Columns.Add(Unit.UnitName, DILanguage.GetLanguageString(Constants.LanguageKeys.Unit));
            this.Columns.Add(SubgroupVals.SubgroupVal, DILanguage.GetLanguageString(Constants.LanguageKeys.Subgroup));
            this.TagValueColumnName = Indicator_Unit_Subgroup.IUSNId;
            this.GlobalValueColumnName1 = Indicator.IndicatorGlobal;
            this.GlobalValueColumnName2 = Unit.UnitGlobal;
            this.GlobalValueColumnName3 = SubgroupVals.SubgroupValGlobal;
            this._ShowIUS = true;
        }


        #endregion

        #endregion

        #endregion

    }
}
