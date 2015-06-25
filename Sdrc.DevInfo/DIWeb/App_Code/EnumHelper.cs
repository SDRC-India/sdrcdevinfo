using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Enum
/// </summary>
public static class EnumHelper
{
    public enum PageName
    {
        News,
        Faq,
        Innovation
    };

    public enum MappingType
    {
        CodeList = 0,
        IUS = 1,
        Metadata = 2
    }
    public enum PublishDataFields
    {
        Indicator,
        Area,
        Time,
        Source
    }

    public enum FilterType
    {
        Indicator,
        Areas,
        Timeperiods,
        Source
    }
}