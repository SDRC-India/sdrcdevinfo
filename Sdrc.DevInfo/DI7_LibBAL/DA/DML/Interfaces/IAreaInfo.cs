using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    public interface IAreaInfo
    {
        /// <summary>
        /// Get or set area nid.
        /// </summary>
        int Nid
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set area id.
        /// </summary>
        string ID
        {
            get;
            set;
        }
        /// <summary>
        /// Get or set area Gid.
        /// </summary>
        string GID
        {
            get;
            set;
        }
        /// <summary>
        /// Get or set area name.
        /// </summary>
        string Name
        {
            get;
            set;
        }
        /// <summary>
        /// Get or set area level.
        /// </summary>
        int Level
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets area map
        /// </summary>
        string AreaMap
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets area block
        /// </summary>
        string AreaBlock
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets true/false
        /// </summary>
        bool IsGlobal
        {
            get;
            set;
        }


        /// <summary>
        /// Get or set  parent areaInfo. 
        /// </summary>
        AreaInfo Parent
        {
            get;
            set;
        }
        /// <summary>
        /// Return instance of AreaInfo.
        /// </summary>
        /// <param name="name">Area Name</param>
        /// <param name="gid">Area Gid</param>
        /// <param name="level">Area Level</param>
        /// <param name="parentName">Area parent name</param>
        /// <param name="parentGId">Parent Gid</param>
        /// <param name="parentLevel">Parent Level</param>
        void AddInfo(string name, string gid, int level, string parentName, string parentGId, int parentLevel);

        /// <summary>
        /// Create instance of AreaInfo.
        /// </summary>
        /// <param name="name">Area name</param>
        /// <param name="gid">Area Gid</param>
        void AddInfo(string name, string gid);

        /// <summary>
        /// Create instance of AreaInfo.
        /// </summary>
        /// <param name="name">Area name</param>
        /// <param name="gid">Area Gid</param>
        /// <param name="level">Area Level</param>
        void AddInfo(string name, string gid, int level);


        /// <summary>
        /// Create instance of AreaInfo.
        /// </summary>
        /// <param name="name">Area name</param>
        /// <param name="ID">Area Id</param>
        /// <param name="parentName">Area Parent Name</param>
        /// <param name="parentID">Parent Id</param>
        void AddInfo(string name, string ID, string parentName, string parentID);

    }
}
