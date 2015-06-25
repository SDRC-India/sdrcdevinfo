using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    ///  Provides indicator classification information.
    /// </summary>
    public class IndicatorClassificationInfo : IIndicatorClassificationInfo
    {

        #region "-- public  --"

        #region "-- Variables / Properties --"

        private int _Nid = 0;
        /// <summary>
        /// Gets or sets area nid.
        /// </summary>
        public int Nid
        {
            get { return this._Nid; }
            set { this._Nid = value; }
        }

        private ICType _Type;
        /// <summary>
        /// Gets or sets indicator classification type
        /// </summary>
        public ICType Type
        {
            get { return this._Type; }
            set { this._Type = value; }
        }

        private string _GID = string.Empty;
        /// <summary>
        /// Gets or sets area gid.
        /// </summary>
        public string GID
        {
            get { return this._GID; }
            set { this._GID = value; }
        }

        private string _Name = string.Empty;
        /// <summary>
        /// Gets or sets area name
        /// </summary>
        public string Name
        {
            get { return this._Name; }
            set { this._Name = value; }
        }

        private bool _IsGlobal;
        /// <summary>
        /// Gets or sets true/false. 
        /// </summary>
        public bool IsGlobal
        {
            get { return this._IsGlobal; }
            set { this._IsGlobal = value; }
        }

        private string _ClassificationInfo = string.Empty;
        /// <summary>
        /// Gets or sets classifications info.
        /// </summary>
        public string ClassificationInfo
        {
            get { return this._ClassificationInfo; }
            set { this._ClassificationInfo = value; }
        }



        private IndicatorClassificationInfo _Parent;
        /// <summary>
        /// Get or Set  parent IndicatorClassificationInfo .
        /// </summary>
        public IndicatorClassificationInfo Parent
        {
            get { return this._Parent; }
            set { this._Parent = value; }
        }

        private string _ISBN = string.Empty;
        /// <summary>
        /// Gets or sets ISBN
        /// </summary>
        public string ISBN
        {
            get { return _ISBN; }
            set { _ISBN = value; }
        }

        private string _Nature = string.Empty;
        /// <summary>
        /// gets or set Nature
        /// </summary>
        public string Nature
        {
            get { return _Nature; }
            set { _Nature = value; }
        }

        #endregion

        #region "-- New/Dispose --"

        public IndicatorClassificationInfo()
        {
            // do not implement this
        }

        public IndicatorClassificationInfo(string name, ICType type, bool isGlobal, int parentNid)
        {
            this._Parent = new IndicatorClassificationInfo();
            this._Name = name;
            this._Type = type;
            this._IsGlobal = isGlobal;
            this._Parent.Nid = parentNid;
        }

        #endregion


        #endregion


    }
}
