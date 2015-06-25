using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.Controls.ImageTrayControl
{
    public class NormalImagesSource : ImageTraySource
    {

        internal NormalImagesSource()
        {
            this.FilterString = DICommon.FilterType.PIC_IMAGE_GIF + "|" +
                DICommon.FilterType.PIC_IMAGE_JPG + "|" +
                DICommon.FilterType.PIC_IMAGE_PNG + "|" + DICommon.FilterType.SHOCKWAVE_FILES +
                "|" + DICommon.FilterType.ADOBE_EXT_PDF +"|"+ DICommon.FilterType.MS_EXCEL_XLS;
        }
    }
}
