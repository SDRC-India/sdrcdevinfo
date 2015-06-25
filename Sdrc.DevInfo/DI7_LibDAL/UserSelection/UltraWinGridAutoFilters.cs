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
    public class UltraWinGridAutoFilters : System.Collections.CollectionBase, ICloneable
    {

        #region " -- Public / Friend -- "

        #region " -- Constructor -- "
        /// <summary>Internal Constructor</summary>
        /// <remarks>Allow Filter Class to instatntiate UltraWinGridFilters and expose as property </remarks>
        public UltraWinGridAutoFilters()
        {
        }

        #endregion

        #region " -- Indexer -- "

        /// <summary>
        /// Get the UltraWinGridFilter on the basis of index
        /// </summary>
        /// <param name="ThemeIndex">Index</param>
        /// <returns>IndicatorDataValueFilter</returns>
        public UltraWinGridAutoFilter this[int Index]
        {
            get
            {
                UltraWinGridAutoFilter RetVal;
                try
                {
                    if (Index < 0)
                    {
                        RetVal = null;  // -- For invalid IndicatorDataValueFilter Index
                    }
                    else
                    {
                        RetVal = (UltraWinGridAutoFilter)this.List[Index];
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
        /// Get the UltraWinGridFilter on the basis of indicator name
        /// </summary>
        /// <param name="IndicatorNId">IndicatorNId</param>
        /// <returns>IndicatorDataValueFilter</returns>
        public UltraWinGridAutoFilter this[string FilterColumn]
        {
            get
            {
                UltraWinGridAutoFilter RetVal = null;
                try
                {
                    foreach (UltraWinGridAutoFilter uwgFilter in this.List)
                    {
                        if (string.Compare(uwgFilter.FilterColumn, FilterColumn, true) == 0)
                        {
                            RetVal = uwgFilter;
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    RetVal = null;
                }

                return RetVal;
            }
        }

        #endregion

        #region " -- Methods -- "

        /// <summary>
        /// Add the IndicatorDataValueFilter in the list
        /// </summary>
        /// <param name="IndicatorDataValueFilter">IndicatorDataValueFilter</param>
        public void Add(UltraWinGridAutoFilter UltraWinGridAutoFilter)
        {
            this.List.Add(UltraWinGridAutoFilter);
        }

        /// <summary>
        /// Add the UltraWinGridFilter in the list
        /// </summary>
        /// <param name="filterColumn"></param>
        /// <param name="filterString"></param>
        public void Add(string filterColumn, string filterString)
        {
            UltraWinGridAutoFilter UltraWinGridFilter = new UltraWinGridAutoFilter(filterColumn, filterString);
            this.List.Add(UltraWinGridFilter);
        }
   
        public void Remove(UltraWinGridAutoFilter UltraWinGridFilter)
        {
            this.List.Remove(UltraWinGridFilter);

        }

        /// <summary>
        /// Remove the IndicatorDataValueFilter in the list
        /// </summary>
        /// <param name="Index">Index</param>
        public new void RemoveAt(int Index)
        {
            this.List.RemoveAt(Index);

        }

        public void Clear()
        {
            this.List.Clear();
        }

        public string GetUltraWinGridFilterString()
        {
            string RetVal = string.Empty;

            if (this.List.Count > 0)
            {
                foreach (UltraWinGridAutoFilter uwgFilter in this.List)
                {
                    if (! string.IsNullOrEmpty(uwgFilter.FilterString))
                    {
                        if (string.IsNullOrEmpty(RetVal))
                        {
                            RetVal += " (" + uwgFilter.FilterString + ")";
                        }
                        else
                        {
                            RetVal += " AND (" + uwgFilter.FilterString + ")";
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.Print("??");
                    }
                }
            }


            // -- close FilterString with ")"
            if (RetVal.Length > 0)
            {
                RetVal = "(" + RetVal + ")";
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
                XmlSerializer XmlSerializer = new XmlSerializer(typeof(UltraWinGridAutoFilters));
                MemoryStream MemoryStream = new MemoryStream();
                XmlSerializer.Serialize(MemoryStream, this);
                MemoryStream.Position = 0;
                RetVal = (UltraWinGridAutoFilters)XmlSerializer.Deserialize(MemoryStream);
                MemoryStream.Close();
                MemoryStream.Dispose();
            }
            catch (Exception)
            {
                RetVal = null;
            }
            return (UltraWinGridAutoFilters)RetVal;
        }

        #endregion
    }
}
