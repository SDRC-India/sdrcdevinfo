using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using DevInfo.Lib.DI_LibBAL.UI.Presentations.Common;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Common
{
    public class AreaLevelFormat
    {
        #region " -- Public -- "

        #region " -- New / Constructor -- "

        /// <summary>
        /// Constructor only for serialization
        /// </summary>
        public AreaLevelFormat()
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fontSetting"></param>
        public AreaLevelFormat(string key, FontSetting fontSetting)
        {
            this._Key = key;
            this._FontSetting = fontSetting;
        }

        #endregion

        #region " -- Properties -- "

        private string _Key = string.Empty;
        /// <summary>
        /// Gets or sets the collection key (Area Level)
        /// </summary>
        public string Key
        {
            get
            {
                return this._Key;
            }
            set
            {
                this._Key = value;
            }
        }

        private FontSetting _FontSetting;
        /// <summary>
        /// Gets or sets the key template style.
        /// </summary>
        public FontSetting FontSetting
        {
            get
            {
                return this._FontSetting;
            }
            set
            {
                this._FontSetting = value;
            }
        }

        private bool _ShowDataValues = true;
        /// <summary>
        /// If Area level is selected just for grouping purpose and need not to display datavalues against them
        /// </summary>
        public bool ShowDataValues
        {
            get { return _ShowDataValues; }
            set { _ShowDataValues = value; }
        }


        #endregion

        #region " -- Methods -- "

        #endregion

        #endregion     
    }

    public class AreaLevelFormats : CollectionBase
    {
        #region " -- Public -- "

        #region " -- New / Constructor -- "
        
        public AreaLevelFormats()
        {

        }

        #endregion

        #region " -- Properties -- "

        #endregion

        #region " -- Methods -- "

        /// <summary>
        /// Get the level format on the basis of key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public AreaLevelFormat this[string key]
        {
            get
            {
                AreaLevelFormat Retval = null;
                try
                {
                    foreach (AreaLevelFormat levelFormat in this.InnerList)
                    {
                        if (levelFormat.Key == key)
                        {
                            Retval = levelFormat;
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                }
                return Retval;
            }
        }

        /// <summary>
        /// Get the level format on the basis of index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public AreaLevelFormat this[int index]
        {
            get
            {
                AreaLevelFormat Retval = null;
                try
                {
                    Retval = (AreaLevelFormat)this.InnerList[index];
                }
                catch (Exception ex)
                {
                    Retval = null;
                }
                return Retval;
            }
        }

        public bool Contains(string key)
        {
            bool RetVal = false;
            for (int i = 0; i < this.InnerList.Count; i++)
            {
                if (((AreaLevelFormat)this.InnerList[i]).Key == key)
                {
                    RetVal = true;
                    break;
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Add the level format
        /// </summary>
        /// <param name="levelFormat"></param>
        public void Add(AreaLevelFormat levelFormat)
        {
            this.InnerList.Add(levelFormat);
        }

        /// <summary>
        /// Remove the level format
        /// </summary>
        /// <param name="areaLevelFormat"></param>
        public void Remove(AreaLevelFormat areaLevelFormat)
        {
            foreach (AreaLevelFormat levelFormat in this.InnerList)
            {
                if (levelFormat.Key == areaLevelFormat.Key)
                {
                    this.InnerList.Remove(levelFormat);
                    break;
                }
            }
        }

        /// <summary>
        /// Remove the level format on the basis of index
        /// </summary>
        /// <param name="levelFormatIndex"></param>
        public void RemoveAt(int levelFormatIndex)
        {
            int Index = 0;
            foreach (AreaLevelFormat levelFormat in this.InnerList)
            {
                if (Index == levelFormatIndex)
                {
                    this.InnerList.RemoveAt(levelFormatIndex);
                    break;
                }
                Index += 1;
            }
        }

        #endregion

        #endregion     

    }
}
