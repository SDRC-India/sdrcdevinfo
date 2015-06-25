using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices ;
using DevInfo.Lib.DI_LibDAL.Queries;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    public interface IIndicatorClassificationInfo
    {
        /// <summary>
        /// Get or set  nid.
        /// </summary>
        int Nid
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set  indicator classification type.
        /// </summary>
        ICType Type
        {
            get;
            set;
        }
        /// <summary>
        /// Get or set  Gid.
        /// </summary>
        string GID
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set  classification info.
        /// </summary>
        string ClassificationInfo
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set  name.
        /// </summary>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set ture/false.
        /// </summary>
        bool IsGlobal
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set  parent Info. 
        /// </summary>
        IndicatorClassificationInfo Parent
        {
            get;
            set;
        }

        
     }
}
