using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace DevInfo.Lib.DI_LibDataCapture.Questions
{
    [Serializable()]
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Do not save area value using AreaName.DataValue and AreaID.DataValue property.
    /// </remarks>
    public class Area
    {
        #region "-- Private --"

        private string AreaXmlFileName=string.Empty;
        private Question AreaQuestion;

        #region "-- New/Dispose --"

        private Area()
        {
            //do nothing
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Read the area XML file and build the data table for that.
        /// </summary>
        /// <returns>Data table</returns>
        private DataTable GetAreaTable()
        {
            DataTable RetVal=null;
            DataSet TempDataset = new DataSet();
            try
            {
                RetVal = new DataTable();
                TempDataset.ReadXml(this.AreaXmlFileName);
                if (TempDataset.Tables.Count > 0)
                {
                    RetVal = TempDataset.Tables[0].Copy();
                }
                //RetVal.ReadXml(this.AreaXmlFileName);
            }
            catch (Exception)
            {
                RetVal = null;
                //throw;
            }
            return RetVal;
        }

      
        #endregion

        #endregion

        #region "-- Public/Internal --"

        #region "-- Variables / Properties --"

        
       // private Question _AreaID;
       // /// <summary>
       // /// Gets AreaID question
       // /// </summary>
       // public Question AreaID
       // {
       //     get
       //     {
       //         return this._AreaID;
       //     }
         
       // }

       // private Question _AreaName;
       // /// <summary>
       // /// Gets AreaName Question
       // /// </summary>
       // public Question AreaName
       // {
       //     get 
       //     {
       //         return this._AreaName; 
       //     }
       //}

        private string _AreaID=string.Empty;

        public string AreaID
        {
            get { return _AreaID; }
            set { _AreaID = value; }
        }


        private string _AreaName=string.Empty;

        public string AreaName
        {
            get { return _AreaName; }
            set { _AreaName = value; }
        }
	
        private DataTable _AreaTable;
        /// <summary>
        /// Gets the area table
        /// </summary>
        public DataTable AreaTable
        {
            get
            {
                if (this._AreaTable == null)
                {
                    this._AreaTable = this.GetAreaTable();
                }
               return this._AreaTable;
            }
        }

        private string _AreaGId=string.Empty;
        /// <summary>
        /// Gets or Sets Area GId
        /// </summary>
        public string AreaGId
        {
            get 
            {
                return this._AreaGId; 
            }
            set
            {
                this._AreaGId = value; 
            }
        }
               

        #endregion

        #region "-- New/Dispose --"

        internal Area(DataSet xmlDataSet, string areaXmlFileName)
        {
            string[] AreaValue ;

            this.AreaXmlFileName = areaXmlFileName;

            // get area table
            this._AreaTable=this.GetAreaTable();

            //get area question
            this.AreaQuestion = Questionnarie.GetQuestion(xmlDataSet, MandatoryQuestionsKey.Area);
           
            //update datavalue for AreaId and AreaName
            if (!string.IsNullOrEmpty(this.AreaQuestion.DataValue))
            {
                //AreaValue= this.AreaQuesiton.DataValue.Split(Constants.AreaValueSeparator.ToCharArray());
                AreaValue = Questionnarie.SplitString(this.AreaQuestion.DataValue.ToString(), Constants.AreaValueSeparator);
                if (AreaValue.Length > 0)
                {
                    this._AreaName = AreaValue[0];
                }
                if (AreaValue.Length > 1)
                {
                    this._AreaID = AreaValue[1];
                }
                if (AreaValue.Length > 2)
                {
                    this._AreaGId = AreaValue[2];
                }
            }
            else
            {
                this._AreaGId = string.Empty;
                this._AreaID = string.Empty;
                this._AreaName = string.Empty;
            }
        }


        #endregion

        #region "-- Methods --"

        internal void GetUpdatedValues(DataSet xmlDataSet)
        {
            string[] AreaValue ;
            
            this.AreaQuestion=Questionnarie.GetQuestion(xmlDataSet, MandatoryQuestionsKey.Area);

            if (!string.IsNullOrEmpty(AreaQuestion.DataValue))
            {
                AreaValue = Questionnarie.SplitString(AreaQuestion.DataValue.ToString(), Constants.AreaValueSeparator);
                if (AreaValue.Length == 3)
                {
                    this._AreaName = AreaValue[0];
                    this._AreaID = AreaValue[1];
                    this._AreaGId = AreaValue[2];
                }
                else
                {
                    this._AreaName = string.Empty;
                    this._AreaID = string.Empty;
                    this._AreaGId = string.Empty;
                }
            }
        }

        /// <summary>
        /// To save area value in the dataset
        /// </summary>
        /// <param name="areaName">Area Name</param>
        /// <param name="areaGId">Area GID</param>
        /// <param name="areaId">Area Id</param>
        public void SaveAreaValue(string areaName, string areaId, string areaGId)
        {
            if (string.IsNullOrEmpty(areaName) | string.IsNullOrEmpty(areaName) | string.IsNullOrEmpty(areaName))
            {
                this._AreaGId = string.Empty;
                this._AreaID = string.Empty;
                this._AreaName = string.Empty;
                this.AreaQuestion.DataValue = string.Empty;
            }
            else
            {
                this._AreaGId = areaGId;
                this._AreaID = AreaID;
                this._AreaName = areaName;
                // save in order : AreaName <delimiter> AreaId <delimiter> AreaGid
                string value = areaName + Constants.AreaValueSeparator + areaId + Constants.AreaValueSeparator + areaGId;
                this.AreaQuestion.DataValue = value;
            }
        }
        
        /// <summary>
        /// Validate the Area ID
        /// </summary>
        /// <param name="AreaId"></param>
        /// <returns></returns>
        public bool IsValidAreaId(string AreaId)
        {
            bool RetVal = false;
            DataRow[] Row;
            AreaId = Utility.RemoveQuotes(AreaId);
            Row = this.AreaTable.Select(ColumnNames.AreaXmlColumns.AreaId + "=" + "'" + AreaId + "'");
            if (Row.Length > 0)
            {
                RetVal = true;
            }
            else
            {
                RetVal = false;
            }
            return RetVal;
        }
        /// <summary>
        /// Validate the Area Name
        /// </summary>
        /// <param name="AreaName"></param>
        /// <returns></returns>
        public bool IsValidAreaName(string AreaName)
        {
            bool RetVal = false;
            DataRow[] Row;
            AreaName = Utility.RemoveQuotes(AreaName);
            Row = this.AreaTable.Select(ColumnNames.AreaXmlColumns.AreaName + "=" + "'" + AreaName + "'");
            if (Row.Length > 0)
            {
                RetVal = true;
            }
            else
            {
                RetVal = false;
            }
            return RetVal;
        }

        /// <summary>
        /// Get the area name on the basis of area ID
        /// </summary>
        /// <param name="AreaId">Area Id</param>
        /// <returns>Area Name</returns>
        public string GetAreaName(string AreaId)
        {
            string RetVal = string.Empty;
            try
            { 
                DataRow[] Row;
                AreaId = Utility.RemoveQuotes(AreaId);
                Row = this.AreaTable.Select(ColumnNames.AreaXmlColumns.AreaId + "=" + "'" + AreaId + "'");
                if (Row.Length > 0)
                {
                    RetVal = Row[0][ColumnNames.AreaXmlColumns.AreaName].ToString();
                }
                else
                {
                    RetVal = string.Empty;
                }
            }
            catch (Exception)
            {
                RetVal = string.Empty;
            }
            return RetVal;
        }

        /// <summary>
        /// Get the area Id on the basis of area name
        /// </summary>
        /// <param name="AreaName">Area Name</param>
        /// <returns>Area Id</returns>
        public string GetAreaID(string AreaName)
        {
            string RetVal = string.Empty;
            try
            { 
                DataRow[] Row;
                AreaName = Utility.RemoveQuotes(AreaName);
                Row = this.AreaTable.Select(ColumnNames.AreaXmlColumns.AreaName + "=" + "'" + AreaName + "'");
                if (Row.Length > 0)
                {
                    RetVal = Row[0][ColumnNames.AreaXmlColumns.AreaId].ToString();
                }
                else
                {
                    RetVal = string.Empty;
                }              
            }
            catch (Exception)
            {
                RetVal = string.Empty;
            }
            return RetVal;
        }

        /// <summary>
        /// Get the parent area name on the basis of area NId
        /// </summary>
        /// <param name="AreaName">Parent Area Name</param>
        /// <returns>Area Id</returns>
        public string GetParentAreaName(string areaID)
        {
            string RetVal = string.Empty;
            int AreaParentNid = 0;
            try
            {
                DataRow[] Row;
                areaID = Utility.RemoveQuotes(areaID);

                // get parent area nid by area id
                Row = this.AreaTable.Select(ColumnNames.AreaXmlColumns.AreaId + "='" + areaID + "'");
                if (Row.Length > 0)
                {
                    AreaParentNid =Convert.ToInt32(Row[0][ColumnNames.AreaXmlColumns.ParentNid]);

                    // get parent area row by nid
                    foreach (DataRow AreaRow in this.AreaTable.Select(ColumnNames.AreaXmlColumns.AreaNid + "=" + AreaParentNid ))
                    {
                        RetVal=Convert.ToString(AreaRow[ColumnNames.AreaXmlColumns.AreaName]);
                    }
                }
                else
                {
                    RetVal = string.Empty;
                }
            }
            catch (Exception)
            {
                RetVal = string.Empty;
            }
            return RetVal;
        }

        /// <summary>
        /// Get the area Gid on the basis of area id
        /// </summary>
        /// <param name="AreaID">Area Id</param>
        /// <returns>Area GId</returns>
        public string GetAreaGId(string AreaID)
        {
            string RetVal = string.Empty;
            try
            {
                DataRow[] Row;
                AreaID = Utility.RemoveQuotes(AreaID);
                Row = this.AreaTable.Select(ColumnNames.AreaXmlColumns.AreaId + "=" + "'" + AreaID + "'");
                if (Row.Length > 0)
                {
                    RetVal = Row[0][ColumnNames.AreaXmlColumns.AreaGid].ToString();
                }
                else
                {
                    RetVal = string.Empty;
                }
            }
            catch (Exception)
            {
                RetVal = string.Empty;
            }
            return RetVal;
        }

        #endregion

        #endregion
    }
}