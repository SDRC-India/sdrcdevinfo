using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.UserSelection;

namespace DevInfo.Lib.DI_LibBAL.Controls.RecommendedSourcesBAL
{
    public static class RecommendedSourceCntrlFactory
    {

        public static RecommendedSourceBase CreateInstance(RecommendedSourceViewType viewType,UserSelection userSelections)
        {
            RecommendedSourceBase RetVal=null;

            switch (viewType)
            {
                case RecommendedSourceViewType.Area:
                    RetVal = new RecommendedSourceArea();
                    break;

                case RecommendedSourceViewType.IUS:
                    RetVal = new RecommendedSourceIUS();
                    break;

                case RecommendedSourceViewType.Timeperiod:
                    RetVal = new RecommendedSourceTimeperiod();
                    break;

                default:
                    break;
            }
            
            RetVal.UserSelections = userSelections;
            
            return RetVal;
        }

    }
}
