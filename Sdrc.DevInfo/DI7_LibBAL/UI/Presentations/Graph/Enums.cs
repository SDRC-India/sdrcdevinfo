using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Graph
{

    /// <summary>
    /// Enum to define the chart mode.
    /// </summary>
    public enum ChartMode
    {
        /// <summary>
        /// Insert the new chart
        /// </summary>
        Insert,

        /// <summary>
        /// Edit the existing series
        /// </summary>
        Edit,

        /// <summary>
        /// Insert the chart in excel mode
        /// </summary>
        ExcelInsert,

        /// <summary>
        /// Edit the chart in excel mode
        /// </summary>
        ExcelEdit
    }
}
