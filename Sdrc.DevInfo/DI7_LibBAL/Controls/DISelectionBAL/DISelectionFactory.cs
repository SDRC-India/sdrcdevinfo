using System;
using System.Collections.Generic;
using System.Text;
//using DevInfo.Lib.DI_LibBAL.Controls.TreeSelectionBAL;
using DevInfo.Lib.DI_LibBAL.Controls.DISelectionBAL;

namespace DevInfo.Lib.DI_LibBAL.Controls.DISelectionBAL
{
    /// <summary>
    /// Class create and return Instance Of DISelection Controls(TimePeriod, Unit, Subgroup) Related Source class
    /// </summary>
    public static  class DISelectionFactory
    {
        /// <summary>
        /// Return Instance Of DISelection Controls Source class
        /// </summary>
        /// <param name="selectionType">Selection Type for </param>
        /// <returns></returns>
        public static BaseSelection CreateInstance(DISelectionType selectionType)
        {
            BaseSelection RetVal = null;

            switch (selectionType)
            {
                case DISelectionType.Timeperiod:
                    RetVal = new TimeperiodSelectionSource();
                    break;

                case DISelectionType.Unit:
                    RetVal = new UnitSelectionSource();
                    break;

                case DISelectionType.Subgroups:
                    RetVal = new SubgroupValSelectionSource();
                    break;
                case DISelectionType.IUS :
                    RetVal = new IUSSelectionSource();
                    break;
                default:
                    break;
            }
            return RetVal;
        }
    }
}
