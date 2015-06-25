using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibBAL.MergeTemplate;
using DevInfo.Lib.DI_LibBAL.Controls.TemplateMergeBAL;

namespace DevInfo.Lib.DI_LibBAL.Controls.TemplateMergeBAL
{
    public class TemplateSourceFactory
    {

        /// <summary>
        /// Returns the instance of TemplateMergingControlSource
        /// </summary>
        /// <param name="mergeControlType"></param>
        /// <param name="sourceAppliactionType"></param>
        /// <returns></returns>
        public static TemplateMergingControlSource GetInstance(TemplateMergeControlType mergeControlType, ApplicationType sourceAppliactionType)
        {
            TemplateMergingControlSource RetVal=null;
            
            switch (mergeControlType)
            {
                case TemplateMergeControlType.Indicator:
                    RetVal = new IndicatorSource();
                    break;
                case TemplateMergeControlType.Unit:
                    RetVal = new UnitSource();
                    break;
                case TemplateMergeControlType.Subgroups:
                    RetVal = new SubgroupValsSource();
                    break;
                case TemplateMergeControlType.SubgroupDimensions:
                    RetVal = new SubgroupDimensionsSource();
                    break;
                case TemplateMergeControlType.SubgroupDimensionsValue:
                    RetVal = new SubgroupsDimensionValuesSource();
                    break;
                case TemplateMergeControlType.IndicatorClassification:
                    
                    RetVal = new ICSource();
                    if (sourceAppliactionType == ApplicationType.MergeTemplateType)
                    {                        
                        RetVal._ISValidationReqOnMapClick = false;
                    }
                    else
                    {
                        RetVal._ISValidationReqOnMapClick = true;
                    }
                    break;

                case TemplateMergeControlType.Areas:

                    RetVal = new AreasSource();

                    if (sourceAppliactionType == ApplicationType.MergeTemplateType)
                    {
                        RetVal._IsAllowMapping = false;
                        RetVal._ISValidationReqOnMapClick = false;
                    }
                    else
                    {
                        RetVal._IsAllowMapping = true;
                        RetVal._ISValidationReqOnMapClick = true;
                    }
                    break;

                default:
                    break;
            }

            if (RetVal != null)
            {
                RetVal.SourceApplicationType = sourceAppliactionType;
                
            }

            return RetVal;
        
        }
        

    }

}
