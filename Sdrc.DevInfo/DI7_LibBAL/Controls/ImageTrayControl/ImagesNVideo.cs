using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.Controls.ImageTrayControl
{
    public class ImagesNVideosource:ImageTraySource
    {
        internal ImagesNVideosource()
        {
            this.FilterString = DICommon.FilterType.PIC_IMAGE_GIF + "|" +
                DICommon.FilterType.PIC_IMAGE_JPG + "|" +
                DICommon.FilterType.PIC_IMAGE_PNG + "|" + DICommon.FilterType.MEDIA_FILES +
                "|" + DICommon.FilterType.WMV_FILES + "|" + DICommon.FilterType.AVI_FILES + "|" + DICommon.FilterType.MS_EXCEL_XLS;
        }
    }
}
