using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibBAL.UI.UserPreference
{
    public class IndicatorClassificationSelections
    {
       
        #region " -- Public -- "

        #region " -- New / Dispose "

        public IndicatorClassificationSelections(DIConnection dIConnection,DIQueries dIQueries)
        {
            // -- DAL connection object
            this.DbConnection = dIConnection;
            // -- DAL query object
            this.DbQueries = dIQueries;
            // -- Intialize the string builder object
            this._XMLString = new StringBuilder();
        }

        #endregion

        #region " -- Properties -- "

        private ICType _ICFieldType;
        /// <summary>
        /// Gets or sets the IC field type.
        /// </summary>
        public ICType ICFieldType
        {
            get 
            {
                return this._ICFieldType; 
            }
            set 
            {
                this._ICFieldType = value;
            }
        }

        private int _ICLevel = -1;
        /// <summary>
        /// Gets or sets the IC level
        /// </summary>
        public int ICLevel
        {
            get 
            {
                return this._ICLevel; 
            }
            set 
            {
                this._ICLevel = value; 
            }
        }	

        private StringBuilder _XMLString;
        /// <summary>
        /// Get the IC XML string.
        /// </summary>
        public StringBuilder XMLString
        {
            get 
            {
                //-- Reset the string builder.
                this._XMLString.Length = 0;
                // -- Add the opening tag
                this._XMLString.Append("<IC ICGId=' ' IC_Name='" + this._ICFieldType.ToString() + "'>");
                // -- Generate the XML
                this.GetICXML(-1);
                // -- Add the clsoing tag
                this._XMLString.Append("</IC>");

                return this._XMLString;
            }
        }

        #endregion

        #region " -- Methods -- "
        
        #endregion

        #endregion

        #region " -- Private -- "

        #region " -- Variables -- "

        /// <summary>
        /// DAL Connection object
        /// </summary>      
        private DIConnection DbConnection;

        /// <summary>
        /// DAL query object
        /// </summary>
        private DIQueries DbQueries;

        /// <summary>
        /// Language specific parent node
        /// </summary>
        private string ParentNode=string.Empty;

        private int Level = 1;

        #endregion

        #region " -- Methods -- "

        /// <summary>
        /// Generates the XML of IC
        /// </summary>
        /// <param name="parent_NId"></param>
        private void GetICXML(int parent_NId)
        {
            DataTable ICTable;            

            // -- Get the IC table on the basis of ICType and parent NId
            ICTable = DbConnection.ExecuteDataTable(DbQueries.IndicatorClassification.GetICForParentNIdAndICType(this._ICFieldType, parent_NId));
			ICTable.Columns.Add("Level", typeof(System.Int32));
			foreach (DataRow Row in ICTable.Rows)
			{
				Row["Level"] = this.Level;
			}

            foreach (DataRow Row in ICTable.Rows)
            {
                //this._XMLString.Append("<ICNode" + DEPTH);
                this._XMLString.Append("<ICNode");

                if (this._ICLevel == -1)
                {
                    this._XMLString.Append(" ICGId='" + Row[IndicatorClassifications.ICGId] + "' ICParent_NId='" + Row[IndicatorClassifications.ICParent_NId] + "' IC_Name='" + this.RemoveXMLSpecialCharacter(Row[IndicatorClassifications.ICName].ToString()) + "'>");
                }
                else
                {
					this._XMLString.Append(" ICInfo='" + Row[IndicatorClassifications.ICNId].ToString() + Delimiter.TEXT_SEPARATOR + Row[IndicatorClassifications.ICGId] + Delimiter.TEXT_SEPARATOR + Row[IndicatorClassifications.ICParent_NId] + Delimiter.TEXT_SEPARATOR + Row["Level"].ToString() + "' IC_Name='" + this.RemoveXMLSpecialCharacter(Row[IndicatorClassifications.ICName].ToString()) + "'>");
                }

                if (this._ICLevel == -1)
                {
                    // -- Recursive loop to get the ICs
                    this.GetICXML(Convert.ToInt32(Row[IndicatorClassifications.ICNId]));
                }
                else if (Level < this._ICLevel)
                {
                    Level += 1;
                    // -- Recursive loop to get the ICs
                    this.GetICXML(Convert.ToInt32(Row[IndicatorClassifications.ICNId]));
					this.Level = Convert.ToInt32(Row["Level"]);
                }
                //this.Level = 1;

                //this._XMLString.Append("</ICNode" + DEPTH + ">");
                this._XMLString.Append("</ICNode>");

            }
        }




        /// <summary>
        /// Remove specail character from the XML
        /// </summary>
        /// <param name="icName"></param>
        /// <returns></returns>
        private string RemoveXMLSpecialCharacter(string icName)
        {
            string Retval = string.Empty;
            try
            {
                icName = icName.Replace("&", "&amp;");
                icName = icName.Replace("<", "&lt;");
                icName = icName.Replace(">", "&gt;");
                icName = icName.Replace("\"", "&quot;");
                icName = icName.Replace("'", "&#39;");
                Retval = icName;
            }
            catch (Exception)
            {
                Retval = icName;
            }
            return Retval;
        }

        private const string DEPTH = "1";
        
        #endregion

        #endregion
    }
}
