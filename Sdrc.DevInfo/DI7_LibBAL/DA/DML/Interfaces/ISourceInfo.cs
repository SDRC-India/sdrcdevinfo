using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    public interface ISourceInfo : ICommonInfo
    {
        /// <summary>
        /// Get or set source nid.
        /// </summary>
        new int Nid
        {
            get;
            set;
        }
        /// <summary>
        /// Get or set source gid.
        /// </summary>
        new string GID
        {
            get;
            set;
        }
        /// <summary>
        /// Get or set source Name.
        /// </summary>
        new string Name
        {
            get;
            set;
        }
        /// <summary>
        /// Get or set source parent nid.
        /// </summary>
         int ParentNid
        {
            get ;
            set ;
        }
        /// <summary>
        /// Gets or sets ture or false. True if subgroup is global otherwise false.Default is false.
        /// </summary>
       bool Global
      {
          get;
          set;
      }
        /// <summary>
        /// Gets or sets source info.
        /// </summary>
        string Info
        {
            get;
            set;
        }
    }
}
