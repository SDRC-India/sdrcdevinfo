using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    public interface ISubgroupValInfo : ICommonInfo
    {
        /// <summary>
        /// Get or set subgroupval nid. 
        /// </summary>
        new int Nid
        {
            get;
            set;
        }
        /// <summary>
        /// Get or set subgroupval gid.
        /// </summary>
        new string GID
        {
            get;
            set;
        }
        /// <summary>
        /// Get or set subgroupval name.
        /// </summary>
        new string Name
        {
            get;
            set;
        }
        /// <summary>
        /// Get or set instance of age type subgroup. 
        /// </summary>
      SubgroupInfo Age
      {
          get;
          set;
      }
        /// <summary>
        /// Get or set instance of sex type subgroup.
        /// </summary>
      SubgroupInfo Sex
      {
          get;
          set;
      }
      /// <summary>
      /// Get or set instance of location type subgroup.
      /// </summary>
      SubgroupInfo Location
      {
          get;
          set;
      }
      /// <summary>
      ///  Get or set instance of other type subgroup.
     /// </summary>
      SubgroupInfo Others
      {
          get;
          set;
      }

    }
}
