using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.UI.UserPreference;

namespace DevInfo.Lib.DI_LibBAL.Controls.TreeFilterBAL
{
    public class TreeFilter
    {

        #region " -- Enum -- "

        /// <summary>
        /// Enum to specify the filter type.
        /// </summary>
        public enum FilterType
        {
            IUSFilter,
            SourceFilter
        }

        #endregion

        #region " -- Public -- "

        #region " -- New / Disposer -- "

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dIConnection"></param>
        /// <param name="dIQueries"></param>
        /// <param name="userPreference"></param>
        public TreeFilter(DIConnection dIConnection, DIQueries dIQueries, ref UserPreference userPreference)
        {
            this.DbConnection = dIConnection;
            this.DbQueries = dIQueries;
            this.Preference = userPreference;
        }

        #endregion

        #region " -- Properties -- "
   
        private FilterType _DataFilterType;

        public FilterType DataFilterType
        {
            get 
            {
                return this._DataFilterType; 
            }
            set 
            {
                this._DataFilterType = value; 
            }
        }

        private bool _RecommendedSources=true;
        /// <summary>
        /// Set the recommended source.
        /// </summary>
        public bool RecommendedSources
        {           
            set
            {
                this._RecommendedSources = value;
            }
        }
	

        #endregion

        #region " -- Methods -- "


        public string GetXML()
        {
            string Retval = string.Empty;
            try
            {
                switch (this._DataFilterType)
                {
                    case FilterType.IUSFilter:
                        Retval = this.IUSXMLFilterString();
                        break;
                    case FilterType.SourceFilter:
                        Retval = this.SourceXMLFilterString();
                        break;
                    default:
                        break;
                }
   
            }
            catch (Exception)
            {
                Retval = string.Empty;
            }
            return Retval;
        }

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
        /// User preference object
        /// </summary>
        UserPreference Preference;
    
        #endregion

        private const string SEPRATOR = "-";

        #region " -- Methods -- "

        private string SourceXMLFilterString()
        {
            string Retval = string.Empty;
            try
            {
                StringBuilder XMLString = new StringBuilder();
                StringBuilder SourceNode = new StringBuilder();
                string IUSName = string.Empty;
                bool ValidSourceFilter = false;
                bool IsRecommendedSource=false;
                int SourceCount = 0;

                // -- Get the Auto select Source, IUS data table
                DataTable IUSDt = this.DbConnection.ExecuteDataTable(this.DbQueries.Source.GetDataWAutoSelectedSourcesWithIUS(this.Preference.UserSelection.ShowIUS, this.Preference.UserSelection.IndicatorNIds, this.Preference.UserSelection.SourceNIds));

                // -- Parent Node
                XMLString.Append("<Source>");

                foreach (DataRow Row in IUSDt.Rows)
                {
                    if (this._RecommendedSources)
                    {
                        DataRow[] Rows = new DataRow[0];
                        Rows = IUSDt.Select(IndicatorClassificationsIUS.IUSNId + " = " + Convert.ToInt32(Row[IndicatorClassificationsIUS.IUSNId]) + " AND " + IndicatorClassificationsIUS.ICIUSOrder + " is not null");
                        if (Rows.Length > 0)
                        {
                            IsRecommendedSource = true;
                        }
                        else
                        {
                            IsRecommendedSource = false;
                        }
                    }
                    else
                    {
                        IsRecommendedSource = false;
                    }

                    if (!IsRecommendedSource)
                    {
                        string Temp = Row[Indicator.IndicatorName].ToString() + " " + SEPRATOR + " " + Row[Unit.UnitName].ToString() + " " + SEPRATOR + " " + Row[SubgroupVals.SubgroupVal].ToString();

                        if (string.Compare(Temp, IUSName, true) != 0)
                        {
                            if (!string.IsNullOrEmpty(IUSName) && SourceCount > 1)
                            {
                                XMLString.Append(SourceNode.ToString() + "</IUS>");
                                ValidSourceFilter = true;
                            }
                            SourceNode.Length = 0;
                            SourceCount = 0;

                            // -- Add the node, if found diff from the last node.
                            IUSName = Temp;
                            SourceNode.Append("<IUS IUSNId='" + string.Empty + "' IUSName='" + this.RemoveXMLSpecialCharacter(Row[Indicator.IndicatorName].ToString()) + " " + SEPRATOR + " " + this.RemoveXMLSpecialCharacter(Row[Unit.UnitName].ToString()) + " " + SEPRATOR + " " + this.RemoveXMLSpecialCharacter(Row[SubgroupVals.SubgroupVal].ToString()) + "'>");
                        }
                        SourceCount += 1;
                        // -- Add the source node.
                        SourceNode.Append("<IC ICNId='" + Row[Data.IUSNId].ToString() + "_" + Row[IndicatorClassifications.ICNId].ToString() + "' SourceName='" + this.RemoveXMLSpecialCharacter(Row[IndicatorClassifications.ICName].ToString()) + "'/>");
                    }
                }

                if (SourceCount > 1)
                {
                    XMLString.Append(SourceNode.ToString() + "</IUS>");
                    ValidSourceFilter = true;
                }
                if (ValidSourceFilter)
                {
                    XMLString.Append("</Source>");
                    Retval = XMLString.ToString();
                }
                else
                {
                    Retval = string.Empty;
                }
            }
            catch (Exception)
            {
                Retval = string.Empty;
            }
            return Retval;
        }

        private string IUSXMLFilterString()
        {
            string Retval = string.Empty;
            try
            {
                StringBuilder XMLString = new StringBuilder();
                StringBuilder IUSNode = new StringBuilder();
                bool ValidIUSFilter = false;
                string IndicatorUnitName = string.Empty;
                string TempIndicatorUnitName = string.Empty;
                string SubgroupName = string.Empty;
                int Subgroupcount = 0;

                // -- Get the IUS table
                DataTable IUSDt = this.DbConnection.ExecuteDataTable(this.DbQueries.IUS.GetDataWAutoSelectedIUS(this.Preference.UserSelection.ShowIUS, this.Preference.UserSelection.IndicatorNIds, this.Preference.UserSelection.AreaNIds, this.Preference.UserSelection.TimePeriodNIds, this.Preference.UserSelection.SourceNIds));
                // -- Add the parent node.
                XMLString.Append("<IUS>");

                foreach (DataRow Row in IUSDt.Rows)
                {
                    IndicatorUnitName = Row[Indicator.IndicatorName].ToString() + " - " + Row[Unit.UnitName].ToString();
                    if (string.Compare(IndicatorUnitName, TempIndicatorUnitName, true) != 0)
                    {
                        if (!string.IsNullOrEmpty(TempIndicatorUnitName) && Subgroupcount > 1)
                        {
                            XMLString.Append(IUSNode.ToString() + "</IndicatorUnit>");
                            ValidIUSFilter = true;
                        }
                        IUSNode.Length = 0;
                        Subgroupcount = 0;
                        TempIndicatorUnitName = Row[Indicator.IndicatorName].ToString() + " - " + Row[Unit.UnitName].ToString();
                        // -- Add the node, if found diff from the last node.                        
                        IUSNode.Append("<IndicatorUnit IndicatorNId='" + string.Empty + "' IndicatorName='" + this.RemoveXMLSpecialCharacter(Row[Indicator.IndicatorName].ToString() + " - " + Row[Unit.UnitName].ToString()) + "'>");
                    }

                    Subgroupcount += 1;
                
                    // -- Add the subgroup node.
                    IUSNode.Append("<Subgroup subgroupNId='" + Row[Data.IUSNId].ToString() + "' SubgroupName='" + this.RemoveXMLSpecialCharacter(Row[SubgroupVals.SubgroupVal].ToString()) + "'/>");
                }
                if (Subgroupcount > 1)
                {
                    XMLString.Append(IUSNode.ToString() + "</IndicatorUnit>");
                    ValidIUSFilter = true;
                }               
                
                if (ValidIUSFilter)
                {
                    XMLString.Append("</IUS>");
                    Retval = XMLString.ToString();
                }
                else
                {
                    Retval = string.Empty;
                }
            }
            catch (Exception)
            {
                Retval = string.Empty;
            }
            return Retval;
        }

        //private string IUSXMLFilterString()
        //{
        //    string Retval = string.Empty;
        //    try
        //    {
        //        StringBuilder XMLString = new StringBuilder();
        //        string IndicatorName = string.Empty;
        //        string UnitName = string.Empty;
        //        string SubgroupName = string.Empty;
        //        // -- Get the Ic table
        //        DataTable IUSDt = this.GetIUS();
        //        // -- Add the parent node.
        //        XMLString.Append("<IUS>");

        //        foreach (DataRow Row in IUSDt.Rows)
        //        {
        //            if (string.Compare(Row[Indicator.IndicatorName].ToString(), IndicatorName, true) != 0)
        //            {
        //                if (!string.IsNullOrEmpty(IndicatorName))
        //                {
        //                    UnitName = string.Empty;
        //                    XMLString.Append("</Unit>");
        //                    XMLString.Append("</Indicator>");
        //                }
        //                // -- Add the node, if found diff from the last node.
        //                IndicatorName = Row[Indicator.IndicatorName].ToString();
        //                XMLString.Append("<Indicator IndicatorNId='" + string.Empty + "' IndicatorName='" + this.RemoveXMLSpecialCharacter(Row[Indicator.IndicatorName].ToString()) + "'>");
        //            }

        //            if (string.Compare(Row[Unit.UnitName].ToString(), UnitName, true) != 0)
        //            {
        //                if (!string.IsNullOrEmpty(UnitName))
        //                {
        //                    XMLString.Append("</Unit>");
        //                }
        //                // -- Add the node, if found diff from the last node.
        //                UnitName = Row[Unit.UnitName].ToString();
        //                XMLString.Append("<Unit UnitNId='" + string.Empty + "' UnitName='" + this.RemoveXMLSpecialCharacter(Row[Unit.UnitName].ToString()) + "'>");
        //            }
        //            // -- Add the subgroup node.
        //            XMLString.Append("<Subgroup subgroupNId='" + Row[Data.IUSNId].ToString() + "' SubgroupName='" + this.RemoveXMLSpecialCharacter(Row[SubgroupVals.SubgroupVal].ToString()) + "'/>");
        //        }
        //        XMLString.Append("</Unit>");
        //        XMLString.Append("</Indicator>");
        //        XMLString.Append("</IUS>");
        //        Retval = XMLString.ToString();
        //    }
        //    catch (Exception)
        //    {
        //        Retval = string.Empty;
        //    }
        //    return Retval;
        //}



        /// <summary>
        /// Remove specail character from the XML
        /// </summary>
        /// <param name="icName"></param>
        /// <returns></returns>
        private string RemoveXMLSpecialCharacter(string xmlString)
        {
            string Retval = string.Empty;
            try
            {
                xmlString = xmlString.Replace("&", "&amp;");
                xmlString = xmlString.Replace("<", "&lt;");
                xmlString = xmlString.Replace(">", "&gt;");
                xmlString = xmlString.Replace("\"", "&quot;");
                xmlString = xmlString.Replace("'", "&#39;");
                Retval = xmlString;
            }
            catch (Exception)
            {
                Retval = xmlString;
            }
            return Retval;
        }  

        #endregion

        #endregion

    }
}
