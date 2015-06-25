using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Xml.Serialization;
using System.IO;

namespace DevInfo.Lib.DI_LibDAL.UserSelection
{
    /// <summary>
    /// Class contains the IndicatorDataValueFilter collections
    /// </summary>
    [Serializable]
    public class IndicatorDataValueFilters : System.Collections.CollectionBase, ICloneable
    {

        #region " -- Public / Friend -- "

        #region " -- Constructor -- "
        /// <summary>Internal Constructor</summary>
        /// <remarks>Allow Filter Class to instatntiate IndicatorDataValueFilters and expose as property </remarks>
        public IndicatorDataValueFilters()
        {
        }

        #endregion

        #region " -- Indexer -- "

        /// <summary>
        /// Get the IndicatorDataValueFilter on the basis of index
        /// </summary>
        /// <param name="ThemeIndex">Index</param>
        /// <returns>IndicatorDataValueFilter</returns>
        public IndicatorDataValueFilter this[int Index]
        {
            get
            {
                IndicatorDataValueFilter RetVal;
                try
                {
                    if (Index < 0)
                    {
                        RetVal = null;  // -- For invalid IndicatorDataValueFilter Index
                    }
                    else
                    {
                        RetVal = (IndicatorDataValueFilter)this.List[Index];
                    }
                }
                catch (Exception)
                {
                    RetVal = null;
                }
                return RetVal;
            }
        }

        /// <summary>
        /// Get the IndicatorDataValueFilter on the basis of indicator name
        /// </summary>
        /// <param name="IndicatorNId">IndicatorNId</param>
        /// <returns>IndicatorDataValueFilter</returns>
        public IndicatorDataValueFilter this[string IndicatorNId]
        {
            get
            {
                IndicatorDataValueFilter RetVal = null;
                int iIndicatorNId ;
                if (int.TryParse(IndicatorNId, out iIndicatorNId)) 
                {
                    try
                    {
                        foreach (IndicatorDataValueFilter IndicatorDataValueFilter in this.List)
                        {
                            if (IndicatorDataValueFilter.IndicatorNId == iIndicatorNId)
                            {
                                RetVal = IndicatorDataValueFilter;
                                break;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        RetVal = null;
                    }
                }
                return RetVal;
            }
        }

        #endregion

        #region " -- Properties -- "

        private bool _ShowIUS = true;
        /// <summary>
        /// Defines whether Datavalue filters are set against Indicator or IUS
        /// </summary>
        public bool ShowIUS
        {
            get { return _ShowIUS; }
            set { _ShowIUS = value; }
        }

        private bool _IncludeArea = false;
        /// <summary>
        /// Include Area defines whether only those area records qualify which satisfy all IndicatorDataValueFilters condition (rather than any of them)
        /// </summary>
        public bool IncludeArea
        {
            get { return _IncludeArea; }
            set { _IncludeArea = value; }
        }


#endregion

        #region " -- Methods -- "

        /// <summary>
        /// Add the IndicatorDataValueFilter in the list
        /// </summary>
        /// <param name="IndicatorDataValueFilter">IndicatorDataValueFilter</param>
        public void Add(IndicatorDataValueFilter IndicatorDataValueFilter)
        {
            this.List.Add(IndicatorDataValueFilter);
        }

        /// <summary>
        /// Add the IndicatorDataValueFilter in the list
        /// </summary>
        /// <param name="IndicatorDataValueFilter">IndicatorDataValueFilter</param>
        public void Add(int IndicatorNId, string IndicatorGId, OpertorType OpertorType, double FromDataValue, double ToDataValue)
        {
            IndicatorDataValueFilter IndicatorDataValueFilter = new IndicatorDataValueFilter(this._ShowIUS, IndicatorNId, IndicatorGId, OpertorType, FromDataValue, ToDataValue);
            this.List.Add(IndicatorDataValueFilter);
        }
        public void Remove(IndicatorDataValueFilter IndicatorDataValueFilter)
        {
            this.List.Remove(IndicatorDataValueFilter);

        }

        /// <summary>
        /// Remove the IndicatorDataValueFilter in the list
        /// </summary>
        /// <param name="Index">Index</param>
        public new void RemoveAt(int Index)
        {
            this.List.RemoveAt(Index);
            
        }

        public string SQL_GetIndicatorDataValueFilters(DIServerType DIServerType)
        {
            string RetVal = string.Empty;
            string IndicatorNIdsWithoutFilters = string.Empty;

            //( (IUS.Indicator_NId = 74 AND (D.Data_value BETWEEN 0 AND 100)) OR  (IUS.Indicator_NId = 95 AND (D.Data_value >= 50)) OR IUS.Indicator_NId IN (60,70,80))
            //( (IUS.IUSNId = 93 AND (D.Data_value BETWEEN 0 AND 100)) OR  (IUS.IUSNId = 48 AND (D.Data_value >= 50)) OR IUS.IUSNID IN (60,70,80))
            
            if (this.List.Count > 0 )
            {                
                foreach (IndicatorDataValueFilter IndicatorDataValueFilter in this.List)
                {
                    //-- Collect the Indicator_Nid having no data values
                    if (IndicatorDataValueFilter.OpertorType == OpertorType.None)                    
                    {
                        if (IndicatorNIdsWithoutFilters.Length == 0)
                        {
                            IndicatorNIdsWithoutFilters = IndicatorDataValueFilter.IndicatorNId.ToString();
                        }
                        else
                        {
                            IndicatorNIdsWithoutFilters += ","  + IndicatorDataValueFilter.IndicatorNId.ToString();
                        }
                    }
                    else
                    {
                        // -- Create the SQL statement having datavalue
                        if (RetVal.Length == 0)
                        {
                            RetVal = "(" + IndicatorDataValueFilter.SQL_GetDataValueFilter(DIServerType);
                        }
                        else
                        {
                            RetVal += " OR " + IndicatorDataValueFilter.SQL_GetDataValueFilter(DIServerType);
                        }
                    }
                }
                RetVal = " AND (" + RetVal + ")";
            }
            // -- Insert the Indicator_NId at the end of SQL statement. These Indicator_NId have no data value.
            if (IndicatorNIdsWithoutFilters.Length > 0)
            {
                if (RetVal.Length > 0)
                {
                    RetVal += " OR ";
                }
                if (this._ShowIUS)
                {
                    RetVal += "D." + Data.IUSNId + " IN (" + IndicatorNIdsWithoutFilters + ")";
                }
                else 
                {
                    RetVal += "D." + Data.IndicatorNId + " IN (" + IndicatorNIdsWithoutFilters + ")";
                }
            }
            // -- close the sql statement with ")"
            if (RetVal.Length > 0)
            {
                RetVal += ")";
            }
            return RetVal;
        } 

        #endregion

        #endregion


        #region ICloneable Members

        public object Clone()
        {
            object RetVal = null;
            try
            {
                XmlSerializer XmlSerializer = new XmlSerializer(typeof(IndicatorDataValueFilters));
                MemoryStream MemoryStream = new MemoryStream();
                XmlSerializer.Serialize(MemoryStream, this);
                MemoryStream.Position = 0;
                RetVal = (IndicatorDataValueFilters)XmlSerializer.Deserialize(MemoryStream);
                MemoryStream.Close();
                MemoryStream.Dispose();
            }
            catch (Exception)
            {
                RetVal = null;
            }
            return (IndicatorDataValueFilters)RetVal;
        }

        #endregion
    }
}
