using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DevInfo.Lib.DI_LibDAL.ExceptionHandler;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// Build timeperiod record  according to time period information and insert it into database. 
    /// </summary>
    public class TimeperiodBuilder
    {
        #region "-- Private --"

        #region "-- Variables --"

        private DIConnection DBConnection;
        private DIQueries DBQueries;

        #endregion

        #region "-- Methods --"
        /// <summary>
        /// Check existance of Timeperiod first in timeperiodcollection then in database.  
        /// </summary>
        /// <param name="timeperiodValue">Timeperiod Value</param>
        /// <returns>Timeperiod Nid</returns>
        private int CheckTimeperiodExists(string timeperiodValue)
        {
            int RetVal = 0;

            //Step 1: check timeperiod exists in Timeperiods collection
            RetVal = this.CheckTimeperiodInCollection(timeperiodValue);

            //Step 2: check timeperiod exists in database.
            if (RetVal <= 0)
            {
                RetVal = this.GetTimeperiodNid(timeperiodValue);
            }

            return RetVal;
        }
        /// <summary>
        /// Check  existance of timeperiod in timeperiodcollection  
        /// </summary>
        /// <param name="timeperiodValue">Timeperiod Value</param>
        /// <returns>Timeperiod Nid</returns>
        private int CheckTimeperiodInCollection(string timeperiodValue)
        {
            int RetVal = 0;
            try
            {
                if (this.TimeperiodCollection.ContainsKey(timeperiodValue))
                {
                    RetVal = this.TimeperiodCollection[timeperiodValue].Nid;
                }
            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }
       
        /// <summary>
        /// Add timeperiod record into timeperiodcollection.
        /// </summary>
        /// <param name="timeperiodRecord">object of TimeperiodInfo </param>
        private void AddTimeperiodIntoCollection(TimeperiodInfo timeperiodInfo)
        {
            if (!this.TimeperiodCollection.ContainsKey(timeperiodInfo.TimeperiodValue))
            {
                this.TimeperiodCollection.Add(timeperiodInfo.TimeperiodValue, timeperiodInfo);
            }

        }
        /// <summary>
        /// Insert timeperiod into database.
        /// </summary>
        /// <param name="timeperiodValue">Timeperiod Value</param>
        /// <returns>Ture/False. Return true after successful insertion otherwise false</returns>
        private bool InsertTimeperiod(string timeperiodValue)
        {
            bool RetVal = false;

            try
            {
                // create timeperiod
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Timeperiod.Insert.InsertTimeperiod(this.DBQueries.DataPrefix, timeperiodValue));
                RetVal = true;
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;
        }

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Variables & Properties --"
        /// <summary>
        /// Returns Timeperiod colleciton in key,pair format. Key is TimeperiodValue  and value is Object of imeperiodInfo.
        /// </summary>
        internal Dictionary<string, TimeperiodInfo> TimeperiodCollection = new Dictionary<string, TimeperiodInfo>();

        #endregion
        #endregion

        #region "-- Public --"

        #region "-- New/Dispose --"

        public TimeperiodBuilder(DIConnection connection, DIQueries queries)
        {
            this.DBConnection = connection;
            this.DBQueries = queries;
        }

        #endregion

        #region "-- Methods --"
        /// <summary>
        /// Get timeperiod Nid from timeperiod .
        /// </summary>
        /// <param name="timeperiodValue">Timeperiod Value</param>
        /// <returns>Timeperiod Nid</returns>
        public int GetTimeperiodNid(string timeperiodValue)
        {
            int RetVal = 0;
            DataTable TimeperiodTable;

            try
            {
                TimeperiodTable = this.DBConnection.ExecuteDataTable(this.DBQueries.Timeperiod.GetTimePeriod(FilterFieldType.Name, "'" + timeperiodValue + "'").ToString());
                if (TimeperiodTable.Rows.Count > 0)
                {
                    RetVal = Convert.ToInt32(TimeperiodTable.Rows[0][Timeperiods.TimePeriodNId]);
                }
            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }

        /// <summary>
        /// Returns timeperiod table
        /// </summary>
        /// <returns></returns>
        public DataTable GetTimeperiodTable()
        {

            DataTable RetVal=null ;

            try
            {
                RetVal = this.DBConnection.ExecuteDataTable(this.DBQueries.Timeperiod.GetTimePeriod(FilterFieldType.None, string.Empty));
                
            }
            catch (Exception)
            {
                RetVal = null;
            }

            return RetVal;
        }

        /// <summary>
        ///Check timeperiod into database if it is notfound then create it.  
        /// </summary>
        /// <param name="timeperiod">Timeperiod Value</param>
        /// <returns>Timeperiod Nid</returns>
        public int CheckNCreateTimeperiod(string timeperiod)
        {
            int RetVal = 0;
            TimeperiodInfo timeperiodInfo = new TimeperiodInfo();

            timeperiodInfo.TimeperiodValue = timeperiod;
            RetVal = this.CheckNCreateTimeperiod(timeperiodInfo);

            return RetVal;
        }
        /// <summary>
        /// Check existance of timeperiod first in timeperiodcollection then in collection.
        /// </summary>
        /// <param name="timeperiodRecord">object of Timeperiod</param>
        /// <returns>Timeperiod Nid</returns>
        public int CheckNCreateTimeperiod(TimeperiodInfo timeperiodInfo)
        {
            int RetVal = 0;

            //Step 1: check timeperiod exists or not
            RetVal = this.CheckTimeperiodExists(timeperiodInfo.TimeperiodValue);

            //Step 2: create timeperiod and get nid
            if (RetVal <= 0)
            {
                if (this.InsertTimeperiod(timeperiodInfo.TimeperiodValue))
                {
                    RetVal = this.GetTimeperiodNid(timeperiodInfo.TimeperiodValue);
                }
            }

            //Step 3: add timeperiod into collection
            if (RetVal > 0)
            {
                //update object of timeperiod info
                timeperiodInfo.Nid = RetVal;

                // add timeperiod into collection
                this.AddTimeperiodIntoCollection(timeperiodInfo);
            }


            return RetVal;
        }

        /// <summary>
        /// Update timeperiod 
        /// </summary>
        /// <param name="timeperiodRecord">object of Timeperiod</param>
        /// <param name="timeperiod"></param>   
        /// <returns>true/false</returns>
        public bool UpdateTimeperiod(int timeperiodNId,string timeperiod)
        {
            bool RetVal = false;
           
            //Step 1: Update timeperiod 
            if (timeperiodNId > 0)
            {
                // Update timeperiod
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Timeperiod.Update.UpdateTimeperiod(this.DBQueries.DataPrefix,timeperiodNId.ToString() ,timeperiod));
                RetVal = true;
            }
            
            return RetVal;
        }


        /// <summary>
        /// Deletes timeperiods 
        /// </summary>
        /// <param name="nids">Comma separated nids which may be blank</param>
        public void DeleteTimeperiods(string nids)
        {
            if (!string.IsNullOrEmpty(nids))
            {
                try
                {
                    // Step1: Delete records from timeperiod table
                    this.DBConnection.ExecuteNonQuery(this.DBQueries.Delete.Timeperiod.DeleteRecords(nids));
                    
                    // Step2: Delete records from Data table
                    new DIDatabase(this.DBConnection, this.DBQueries).DeleteDataValue(string.Empty, nids, string.Empty, string.Empty);

                }
                catch (Exception ex)
                {
                    throw new ApplicationException(ex.ToString());
                }
            }
        }

        #endregion

        #endregion

    }
}


