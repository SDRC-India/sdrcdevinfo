using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Controls.MappingControlsBAL
{
    
    /// <summary>
    /// Determines the Mapping Type 
    /// </summary>
    public enum DIMappingType
    {
        Value,
        Reference,
        
    }
    /// <summary>
    /// Determine the Linking Window Mapping Type
    /// </summary>
    public enum LinkingWindowType
    { 
        NormalMapping,
        CrossTabMapping
    }

    /// <summary>
    /// Determine the Cell Mapping Type
    /// </summary>
    public enum CellMappingType
    {
        ColumnHeaderMapping,
        RowHeaderMapping,
        DataValueMapping,
        
        None


    }

    public enum SheetType
    { 
        CrossTabSheet,
        SimpleDataSheet,
        None
    }
}
