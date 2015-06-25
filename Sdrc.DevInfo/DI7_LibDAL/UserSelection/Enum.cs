using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.UserSelection
{
    /// <summary>
    /// Describes the expression operator used with SQL statements
    /// </summary>
    /// <remarks>
    /// Do not change the order as combobox items index in UI corresponds to these enums values
    /// </remarks>
    public enum OpertorType
    {
        /// <summary>
        /// No Operator 
        /// </summary>
        None = 0,

        /// <summary>
        /// BETWEEN
        /// </summary>
        Between = 1,

        /// <summary>
        /// >
        /// </summary>
        GreaterThan = 2,
          
        /// <summary>
        /// <
        /// </summary>
        LessThan = 3,


         /// <summary>
        /// =
        /// </summary>
        EqualTo = 4

    }
}
