using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    ///  Provides area information.
    /// </summary>
    public class AreaInfo : IAreaInfo
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


        private int _ParentNid = 0;
        /// <summary>
        /// Gets or sets area parent nid.
        /// </summary>
        public int ParentNid
        {
            get { return this._ParentNid; }
            set { this._ParentNid = value; }
        }

        private string _ID = string.Empty;
        /// <summary>
        /// Gets or sets area id
        /// </summary>
        public string ID
        {
            get { return this._ID; }
            set { this._ID = value; }
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

        private string _GID = string.Empty;
        /// <summary>
        /// Gets or sets area gid.
        /// </summary>
        public string GID
        {
            get { return this._GID; }
            set { this._GID = value; }
        }

        private int _Level = 0;
        /// <summary>
        /// Gets or sets area level
        /// </summary>
        public int Level
        {
            get { return this._Level; }
            set { this._Level = value; }
        }

        private string _AreaMap = string.Empty;
        /// <summary>
        /// Gets or sets area map
        /// </summary>
        public string AreaMap
        {
            get { return this._AreaMap; }
            set { this._AreaMap = value; }
        }

        private string _AreaBlock = string.Empty;
        /// <summary>
        /// Gets or sets area block
        /// </summary>
        public string AreaBlock
        {
            get { return this._AreaBlock; }
            set { this._AreaBlock = value; }
        }

        private bool _IsGlobal;
        /// <summary>
        /// Gets or sets true/false
        /// </summary>
        public bool IsGlobal
        {
            get { return this._IsGlobal; }
            set { this._IsGlobal = value; }
        }

        private AreaInfo _Parent;
        /// <summary>
        /// Get or Set  parent AreaInfo .
        /// </summary>
        public AreaInfo Parent
        {
            get { return this._Parent; }
            set { this._Parent = value; }
        }

        #endregion

        #region "-- New/Dispose --"

        public AreaInfo()
        {
            // do not implement this
        }


        public AreaInfo(string name, string gid)
        {
            this._Parent = new AreaInfo();
            this._Name = name;
            this._GID = gid;
        }

        public AreaInfo(string name, string gid, int level)
            : this(name, gid)
        {
            this._Level = level;
        }

        public AreaInfo(string name, string gid, int level, string parentName, string parentGId, int parentLevel)
            : this(name, gid, level)
        {
            this._Parent._Name = parentName;
            this._Parent._GID = parentGId;
            this._Parent._Level = parentLevel;
        }

        public AreaInfo(string name, string ID, string parentName, string parentID)
        {
            this._Parent = new AreaInfo();
            this._Name = name;
            this._ID = ID;
            this._Parent._Name = parentName;
            this._Parent._ID = parentID;
        }
        #endregion

        #region "-- Others --"

        /// <summary>
        /// To add Information of AreaInfo.
        /// </summary>
        /// <param name="name">Area name</param>
        /// <param name="gid">Area Gid</param>
        /// <param name="level">Area Level</param>
        /// <param name="parentName">Area Parent Name</param>
        /// <param name="parentGId">Parent Gid</param>
        /// <param name="parentLevel">Parent Level</param>
        public void AddInfo(string name, string gid, int level, string parentName, string parentGId, int parentLevel)
        {
            if (this._Parent == null)
            {
                this._Parent = new AreaInfo();
            }

            this._Name = name;
            this._GID = gid;
            this._Level = level;
            this._Parent.Name = parentName;
            this._Parent.GID = parentGId;
            this._Parent.Level = parentLevel;
        }

        /// <summary>
        /// To add Information of AreaInfo.
        /// </summary>
        /// <param name="name">Area name</param>
        /// <param name="gid">Area Gid</param>
        public void AddInfo(string name, string gid)
        {
            if (this._Parent == null)
            {
                this._Parent = new AreaInfo();
            }

            this._Name = name;
            this._GID = gid;
        }

        /// <summary>
        /// To add Information of AreaInfo.
        /// </summary>
        /// <param name="name">Area name</param>
        /// <param name="gid">Area Gid</param>
        /// <param name="level">Area Level</param>
        public void AddInfo(string name, string gid, int level)
        {
            if (this._Parent == null)
            {
                this._Parent = new AreaInfo();
            }

            this._Name = name;
            this._GID = gid;
            this._Level = level;
        }


        /// <summary>
        /// To add Information of AreaInfo.
        /// </summary>
        /// <param name="name">Area name</param>
        /// <param name="ID">Area Id</param>
        /// <param name="parentName">Area Parent Name</param>
        /// <param name="parentID">Parent Id</param>
        public void AddInfo(string name, string ID, string parentName, string parentID)
        {
            if (this._Parent == null)
            {
                this._Parent = new AreaInfo();
            }

            this._Name = name;
            this._ID = ID;
            this._Parent.Name = parentName;
            this._Parent.ID = parentID;
        }

        #endregion

        #endregion


    }
}
