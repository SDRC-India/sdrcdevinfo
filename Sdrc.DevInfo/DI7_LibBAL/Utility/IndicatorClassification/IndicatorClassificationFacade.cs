using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Queries;

namespace DevInfo.Lib.DI_LibBAL.Utility.IndicatorClassification
{
    ///</summary>
    /// Provides IC collection which can be bind with control using datasource property
    /// </summary>
    /// <example>
    /// <code>
    /// 
    ///  //code to bind with control
    ///  combo1.DisplayMember = "DisplayString";
    ///  combo1.ValueMember = "IndicatorClassificationType";
    ///  combo1.DataSource = IndicatorClassificationFacade.ICCollection;
    /// 
    ///  //code to get the mask value 
    ///  ICType SelectedICType = (ICType)combo1.SelectedValue;
    ///   
    /// </code>
    /// </example>    
public static   class IndicatorClassificationFacade
{
    #region "-- Private --"

    #region "-- Methods --"

    private static void BuildCollection()
    {
        ICCollection.Clear();
        ICCollection.Add(new IndicatorClassificationInfo( ICType.CF,DILanguage.GetLanguageString("CF"),"'CF'"));
        ICCollection.Add(new IndicatorClassificationInfo(ICType.Convention, DILanguage.GetLanguageString("CONVENTION"), "'CN'"));
        ICCollection.Add(new IndicatorClassificationInfo(ICType.Goal, DILanguage.GetLanguageString("GOAL"), "'GL'"));
        ICCollection.Add(new IndicatorClassificationInfo(ICType.Institution, DILanguage.GetLanguageString("INSTITUTION"), "'IT'"));
        ICCollection.Add(new IndicatorClassificationInfo(ICType.Sector, DILanguage.GetLanguageString("SECTOR"), "'SC'"));
        ICCollection.Add(new IndicatorClassificationInfo(ICType.Source, DILanguage.GetLanguageString("SOURCE"), "'SR'"));
        ICCollection.Add(new IndicatorClassificationInfo(ICType.Theme, DILanguage.GetLanguageString("THEME"), "'TH'"));
    }

    #endregion

    #endregion

    #region "-- Public --"

    #region "-- Variables --"

    /// <summary>
    /// Create instance of IndicatorClassificationCollection class
    /// </summary>
    public static IndicatorClassificationCollection ICCollection = new IndicatorClassificationCollection();

    #endregion

    #region "-- New/Dispose --"

    /// <summary>
    ///Add indicator classifications 
    /// </summary>
    static IndicatorClassificationFacade()
    {
        IndicatorClassificationFacade.BuildCollection();
    }


    #endregion

    #region "-- Methods --"

    /// <summary>
    /// After changing language file, always Invoke this method
    /// </summary>
    public static void ResetCollection()
    {
        IndicatorClassificationFacade.BuildCollection();
    }

    #endregion

    #endregion
    
    
    }
}
