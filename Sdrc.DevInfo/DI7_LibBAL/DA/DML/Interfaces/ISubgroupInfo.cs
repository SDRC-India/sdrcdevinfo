using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using DevInfo.Lib.DI_LibDAL.Queries;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    public interface ISubgroupInfo :ICommonInfo
    {
        /// <summary>
        /// Get or set subgroup nid.
        /// </summary>
       new int Nid
        {
            get;
            set;
        }
        /// <summary>
        /// Get or set subgroup gid. 
        /// </summary>
       new string GID
        {
            get;
            set;
        }
        /// <summary>
        /// Get or set subgroup name.
        /// </summary>
       new string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set subgroup type.
        /// </summary>
        SubgroupType Type
        {
            get;
            set;
        }
        /// <summary>
        /// Gets or sets ture or false. True if subgroup is global otherwise false.Default is false.
        /// </summary>
        bool Global
        {
            get;
            set;
        }
    }
}
