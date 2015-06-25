using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.Controls.MappingControlsBAL
{
    public class Database
    {
        #region "-- Private --"

        #region "-- Variables --"

        #endregion

        #region "-- New/Dispose --"

        #endregion

        #region "-- Methods --"

        #endregion

        #endregion


        #region "-- Public --"

        #region "-- Variables/Properties --"

        private DIConnection _DBConnection;
        /// <summary>
        /// Gets or sets instance of DIConneciton
        /// </summary>
        public DIConnection DBConnection
        {
            get { return this._DBConnection; }
            set { this._DBConnection = value; }
        }


        private DIQueries _DBQueries;
        /// <summary>
        /// Gets or sets instance of DIQueries
        /// </summary>
        public DIQueries DBQueries
        {
            get { return this._DBQueries; }
            set { this._DBQueries = value; }
        }
	
	
        #endregion

        #region "-- New/Dispose --"

        public Database()
        {
            //do nothing. Added only for serialization purpose
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        public Database(DIConnection dbConnection, DIQueries dbQueries)
        {
            this._DBConnection = dbConnection;
            this.DBQueries = dbQueries;
        }


        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Returns indicators 
        /// </summary>
        /// <param name="indicatorName"></param>
        /// <returns></returns>
        public DataTable GetAllIndicators(string indicatorName)
        {
            DataTable RetVal;
            string SqlQuery = string.Empty;
            string FilterString = string.Empty;

            indicatorName = DICommon.RemoveQuotes(indicatorName);

            try
            {
                //get query
                if (!string.IsNullOrEmpty(indicatorName))
                {
                    FilterString = " " + Indicator.IndicatorName + " LIKE '%" + indicatorName + "%' ";
                    SqlQuery = this.DBQueries.Indicators.GetIndicator(FilterFieldType.Search, FilterString, FieldSelection.Light);
                }
                else
                {
                    SqlQuery = this.DBQueries.Indicators.GetIndicator(FilterFieldType.None, string.Empty, FieldSelection.Light);
                }


                //get datatable
                RetVal = this.DBConnection.ExecuteDataTable(SqlQuery);

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
            return RetVal;
        }

        /// <summary>
        /// Returns units 
        /// </summary>
        /// <param name="unitName"></param>
        /// <returns></returns>
        /// <remarks>Pass Empty String For All Records</remarks>
        public DataTable GetAllUnits(string unitName)
        {
            DataTable RetVal;
            string SqlQuery = string.Empty;
            string FilterString = string.Empty;

            unitName = DICommon.RemoveQuotes(unitName);

            try
            {
                //get query
                if (!string.IsNullOrEmpty(unitName))
                {
                    FilterString = " " + Unit.UnitName + " LIKE '%" + unitName + "%' ";
                    SqlQuery = this.DBQueries.Unit.GetUnit(FilterFieldType.Search, "" + FilterString + "");
                }
                else
                {
                    SqlQuery = this.DBQueries.Unit.GetUnit(FilterFieldType.None, string.Empty);
                }


                //get datatable
                RetVal = this.DBConnection.ExecuteDataTable(SqlQuery);

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
            return RetVal;
        }


        /// <summary>
        /// Returns Timeperiods 
        /// </summary>
        /// <param name="timePeriod"></param>
        /// <returns></returns>
        /// <remarks>Pass Empty String For All Records</remarks>
        public DataTable GetAllTimePeriods(string timePeriod)
        {
            DataTable RetVal;
            string SqlQuery = string.Empty;
            string FilterString = string.Empty;

            timePeriod = DICommon.RemoveQuotes(timePeriod);

            try
            {
                //get query
                if (!string.IsNullOrEmpty(timePeriod))
                {
                    FilterString = " " + Timeperiods.TimePeriod + " LIKE '%" + timePeriod + "%' ";
                    SqlQuery = this.DBQueries.Timeperiod.GetTimePeriod(FilterFieldType.Search, "" + FilterString + "");
                }
                else
                {
                    SqlQuery = this.DBQueries.Timeperiod.GetTimePeriod(FilterFieldType.None, string.Empty);
                }


                //get datatable
                RetVal = this.DBConnection.ExecuteDataTable(SqlQuery);

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
            return RetVal;
        }

        /// <summary>
        /// Returns units for the given indicator
        /// </summary>
        /// <param name="indicatorNId"></param>
        /// <returns></returns>
        public DataTable GetAllUnitsByIndicator(int indicatorNId)
        {
            DataTable RetVal;
            string SqlQuery = string.Empty;
            string FilterString = string.Empty;


            try
            {
                //get query
                SqlQuery = this.DBQueries.Unit.GetUnitByIndicator(indicatorNId);

                //get datatable
                RetVal = this.DBConnection.ExecuteDataTable(SqlQuery);

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
            return RetVal;
        }

        /// <summary>
        /// Returns subgroup vals for the given Subgroup Val
        /// </summary>
        /// <param name="subgroupVal"></param>
        /// <returns></returns>
        public DataTable GetAllSubgroupVals(string subgroupVal)
        {
            DataTable RetVal;
            string SqlQuery = string.Empty;
            string FilterString = string.Empty;

            try
            {
                // -- Get SubgroupVal all if paramaeter value is empty 
                if (!string.IsNullOrEmpty(subgroupVal))
                {
                    FilterString = " " + SubgroupVals.SubgroupVal + " LIKE '%" + subgroupVal + "%' ";
                    SqlQuery = this.DBQueries.SubgroupVals.GetSubgroupVals(FilterFieldType.Search, FilterString);
                }
                else
                {
                    SqlQuery = this.DBQueries.SubgroupVals.GetSubgroupVals(FilterFieldType.None, string.Empty);
                }
                
                //get datatable
                RetVal = this.DBConnection.ExecuteDataTable(SqlQuery);

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
            return RetVal;
        }

        /// <summary>
        /// Returns subgroup vals for the given indicator and unit
        /// </summary>
        /// <param name="unitNId"></param>
        /// <param name="indicatorNId"></param>
        /// <returns></returns>
        public DataTable GetSubgroupVals(int unitNId, int indicatorNId)
        {
            DataTable RetVal;
            string SqlQuery = string.Empty;
            string FilterString = string.Empty;

            try
            {
                //get query
                SqlQuery = this.DBQueries.SubgroupVals.GetSubgroupVal(unitNId, indicatorNId);

                //get datatable
                RetVal = this.DBConnection.ExecuteDataTable(SqlQuery);

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
            return RetVal;
        }

        /// <summary>
        /// Returns subgroup Dimension Values for the given indicator and unit
        /// </summary>
        /// <param name="subgroupNids"></param>
        /// <returns></returns>
        public DataTable GetSubgroupDimensions(string  subgroupNids)
        {
            DataTable RetVal;
            string SqlQuery = string.Empty;
            string FilterString = string.Empty;

            try
            {
              
                //-- step1: get records from subgroup table 
                SqlQuery = this.DBQueries.Subgroup.GetSubgroupInfoWithTypeNOrder(subgroupNids);

                RetVal = this.DBConnection.ExecuteDataTable(SqlQuery); 
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
            return RetVal;
        }

         /// <summary>
        /// Returns subgroup Dimension for the given indicator and unit
        /// </summary>
       /// <returns></returns>
        public DataTable GetAllSubgroupType()
        {
            DataTable RetVal;
            string SqlQuery = string.Empty;
            string FilterString = string.Empty;

            try
            {

                //-- step1: get records from subgroup type table 
                SqlQuery = this.DBQueries.SubgroupTypes.GetSubgroupTypes(FilterFieldType.None, string.Empty);

                RetVal = this.DBConnection.ExecuteDataTable(SqlQuery);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
            return RetVal;
        }
        /// <summary>
        /// Returns subgroup Dimension (Subgroup Type) for the given indicator and unit
        /// </summary>
        ///<param name="subgroupValNids"></param>
        /// <returns></returns>
        public DataTable GetSubgroupType(string subgroupValNids)
        {
            DataTable RetVal;
            string SqlQuery = string.Empty;
            string FilterString = string.Empty;

            try
            {

                //-- step1: get records from subgroup type table 
                SqlQuery = this.DBQueries.SubgroupTypes.GetSubgroupTypes(subgroupValNids);

                RetVal = this.DBConnection.ExecuteDataTable(SqlQuery);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
            return RetVal;
        }

        /// <summary>
        /// Returns areas for the given parent nid
        /// </summary>
        ///<param name="parentNId"></param>
        /// <returns></returns>
        public DataTable GetArea(int parentNId)
        {
            DataTable RetVal;
            string SqlQuery = string.Empty;
            string FilterString = string.Empty;

            try
            {
                //get query
                SqlQuery = this.DBQueries.Area.GetArea(FilterFieldType.ParentNId, parentNId.ToString());

                //get datatable
                RetVal = this.DBConnection.ExecuteDataTable(SqlQuery);

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
            return RetVal;
        }

        /// <summary>
        /// Returns areas 
        /// </summary>
        ///<param name="parentNId"></param>
        /// <returns></returns>
        public DataTable GetAllArea()
        {
            DataTable RetVal;
            string SqlQuery = string.Empty;
            string FilterString = string.Empty;

            try
            {
                //get query
                SqlQuery = this.DBQueries.Area.GetArea(FilterFieldType.None, string.Empty);

                //get datatable
                RetVal = this.DBConnection.ExecuteDataTable(SqlQuery);

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
            return RetVal;
        }

        /// <summary>
        /// Returns areas 
        /// </summary>
        ///<param name="name"></param>
        /// <returns></returns>
        public DataTable GetAreaByName(string name)
        {
            DataTable RetVal;
            string SqlQuery = string.Empty;
            string FilterString = string.Empty;

            name = DICommon.RemoveQuotes(name);

            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    SqlQuery = this.DBQueries.Area.GetArea(FilterFieldType.None, string.Empty);
                }
                else
                {
                    FilterString = " " + Area.AreaName + " LIKE '%" + name + "%' ";
                    SqlQuery = this.DBQueries.Area.GetArea(FilterFieldType.Search, FilterString);
                }

                //get datatable
                RetVal = this.DBConnection.ExecuteDataTable(SqlQuery);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
            return RetVal;
        }

        /// <summary>
        /// Returns Sources for the given Source
        /// </summary>
        /// <param name="subgroupVal"></param>
        /// <returns></returns>
        /// <remarks>Pass Empty String For All Records</remarks>
        public DataTable GetAllSources(string source)
        {
            DataTable RetVal;
            string SqlQuery = string.Empty;
            string FilterString = string.Empty;

            try
            {
                // -- Get SubgroupVal all if paramaeter value is empty 
                if (!string.IsNullOrEmpty(source))
                {
                    FilterString = " " + IndicatorClassifications.ICName + " LIKE '%" + source + "%' ";
                    SqlQuery = this.DBQueries.Source.GetSource(FilterFieldType.Search, FilterString,FieldSelection.Light,false);
                }
                else
                {
                    SqlQuery = this.DBQueries.IndicatorClassification.GetsSourcesWithoutPublishers(); // this.DBQueries.Source.GetSourceGetSource(FilterFieldType.None, string.Empty, FieldSelection.Light, false);
                }

                //get datatable
                RetVal = this.DBConnection.ExecuteDataTable(SqlQuery);

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
            return RetVal;
        }

        #endregion

        #endregion


    }
}
