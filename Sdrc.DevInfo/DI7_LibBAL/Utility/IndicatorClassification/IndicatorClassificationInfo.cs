using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;

namespace DevInfo.Lib.DI_LibBAL.Utility.IndicatorClassification
{
    /// <summary>
    /// Provides indicator classification information
    /// </summary>
   public class IndicatorClassificationInfo
   {
       #region "-- Public --"

       #region "-- Variables/Properties --"

       private string _DatabaseString;
       /// <summary>
       /// Gets or sets database string like SR,SC,etc
       /// </summary>
       public string DatabaseString
       {
           get { return this._DatabaseString; }
           set { this._DatabaseString = value; }
       }

       private ICType _IndicatorClassificationType;
       /// <summary>
       /// Gets or sets indicator classification type 
       /// </summary>
       public ICType IndicatorClassificationType
       {
           get { return this._IndicatorClassificationType; }
           set { this._IndicatorClassificationType = value; }
       }

       private string _DisplayString;
       /// <summary>
       /// Gets or sets display string based on selected language
       /// </summary>
       public string DisplayString
       {
           get { return this._DisplayString; }
           set { this._DisplayString = value; }
       }

       #endregion

       #region "-- New/Dispose --"

       /// <summary>
       /// Constructor
       /// </summary>
       /// <param name="indicatorClassificationType"></param>
       /// <param name="displayString"></param>
       /// <param name="databaseString"></param>
       public IndicatorClassificationInfo(ICType indicatorClassificationType, string displayString, string databaseString)
       {
           this._IndicatorClassificationType = indicatorClassificationType;
           this._DatabaseString = databaseString;
           this._DisplayString = displayString;
       }

       #endregion

       #endregion             		
    }

}
