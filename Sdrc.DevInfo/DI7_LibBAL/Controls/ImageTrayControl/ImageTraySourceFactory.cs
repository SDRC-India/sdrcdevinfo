using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Controls.ImageTrayControl
{
    public static class ImageTraySourceFactory
    {
        public static ImageTraySource CreateInstance(ImageTrayType trayType)
        {
            ImageTraySource RetVal=null;

            switch (trayType)
            {
                case ImageTrayType.NormalImages:
                    RetVal = new NormalImagesSource();
                    break;

                case ImageTrayType.ImagesNVideo:
                    RetVal = new ImagesNVideosource();
                    break;

                default:
                    break;
            }

            return RetVal;
        }
    }
}
