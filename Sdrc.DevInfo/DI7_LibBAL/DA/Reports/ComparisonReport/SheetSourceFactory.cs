using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.ComparisonReport
{
    /// <summary>
    /// Used To Get Comparison Report Sheet Class Instance
    /// </summary>
    internal class SheetSourceFactory
    {
        #region "-- Internal --"

        /// <summary>
        /// Returns Instance of SheetSource for generating sheet
        /// </summary>
        /// <param name="sheetType">Sheet Type</param>
        /// <param name="dbConnection">Database Connection</param>
        /// <param name="dbQueries"></param>
        /// <returns></returns>
        internal static SheetSource CreateInstance(SheetType sheetType, DIConnection dbConnection, DIQueries dbQueries)
        {
            SheetSource RetVal = null;

            switch (sheetType)
            {
                case SheetType.INDICATOR:
                    RetVal = new IndicatorSheetSource();
                    break;
                case SheetType.UNIT:
                    RetVal = new UnitSheetSource();
                    break;
                case SheetType.SUBGROUP:
                    RetVal = new SubgroupSheetSource();
                    break;
                case SheetType.IUS:
                    RetVal = new IUSSheetSource();
                    break;
                case SheetType.TIMEPERIOD:
                    RetVal = new TimeperiodSheetSource();
                    break;
                case SheetType.AREA:
                    RetVal = new AreaSheetSource();
                    break;
                case SheetType.SECTOR:
                    RetVal = new ICSheetSource(ICType.Sector,DILanguage.GetLanguageString(Constants.SheetHeader.SECTOR));
                    break;
                case SheetType.GOAL:
                    RetVal = new ICSheetSource(ICType.Goal , DILanguage.GetLanguageString(Constants.SheetHeader.GOAL));
                    break;
                case SheetType.CF:
                    RetVal = new ICSheetSource(ICType.CF, DILanguage.GetLanguageString(Constants.SheetHeader.CF));
                    break;
                case SheetType.INSTITUTION:
                    RetVal = new ICSheetSource(ICType.Institution , DILanguage.GetLanguageString(Constants.SheetHeader.INSTITUTION));
                    break;
                case SheetType.THEME:
                    RetVal = new ICSheetSource(ICType.Theme, DILanguage.GetLanguageString(Constants.SheetHeader.THEME));
                    break;
                case SheetType.CONVENTION:
                    RetVal = new ICSheetSource(ICType.Convention, DILanguage.GetLanguageString(Constants.SheetHeader.CONVENTION));
                    break;
                case SheetType.SOURCE:
                    RetVal = new ICSheetSource(ICType.Source, DILanguage.GetLanguageString(Constants.SheetHeader.SOURCE));
                    break;
                case SheetType.DATA:
                    RetVal = new DataSheetSource();
                    break;
                default:
                    break;
            }
           
            // -- Set Connection
            if (RetVal != null)
            {
                RetVal.DBConnection = dbConnection ;
                RetVal.DBQueries = dbQueries;
            }

            return RetVal;
        }

        #endregion
    }
}
