using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// Provides common devinfo elements like name, Nid, GId,etc 
    /// </summary>
   public interface ICommonInfo
    {
       /// <summary>
       /// Get and set Nid.
       /// </summary>
      int Nid
       {
           get;
           set;
       }
       /// <summary>
       /// Get and set Gid.
       /// </summary>
       string GID
       {
           get;
           set;
       }
       /// <summary>
       /// Get and set Name.
       /// </summary>
       string Name
       {
           get;
           set;
       }

       /// <summary>
       /// Get and set true/false.
       /// </summary>
       bool Global
       {
           get;
           set;
       }
    }
}
