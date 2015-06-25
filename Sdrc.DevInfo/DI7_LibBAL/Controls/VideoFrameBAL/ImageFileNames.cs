using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Controls.VideoFrameBAL
{
    public static class ImageFileNames
    {
        #region " -- Transitions -- "

        #region " -- Tranistions file Name -- "
        
        /// <summary>
        /// DIVideo
        /// </summary>
        public const string DIVIDEO_FOLDER = "DIVideo";

        private static string _REPLICATE_DEFAULT_IMAGE ="Transition.PNG";
        /// <summary>
        /// Gallery_Normal.png
        /// </summary>
        public static string REPLICATE_DEFAULT_IMAGE
        {
            get
            {
                return _REPLICATE_DEFAULT_IMAGE;
            }
        }

        private static string _PIXEL_TRANSITION = "Pixellate.jpg";
        /// <summary>
        ///Bars.PNG
        /// </summary>
        public static string PIXEL_TRANSITION
        {
            get
            {
                return _PIXEL_TRANSITION;
            }
        }

        private static string _CHECKBOARD_TRANSITION = "Checkboard.PNG";
        /// <summary>
        ///Checkboard.PNG
        /// </summary>
        public static string CHECKBOARD_TRANSITION
        {
            get
            {
                return _CHECKBOARD_TRANSITION;
            }
        }

        private static string _DISSOLVE_TRANSITION = "Dissolve.PNG";
        /// <summary>
        ///Dissolve.PNG
        /// </summary>
        public static string DISSOLVE_TRANSITION
        {
            get
            {
                return _DISSOLVE_TRANSITION;
            }
        }

        private static string _FADE_TRANSITION = "Fade.PNG";
        /// <summary>
        ///Fade.PNG
        /// </summary>
        public static string FADE_TRANSITION
        {
            get
            {
                return _FADE_TRANSITION;
            }
        }

        private static string _ZIG_ZAG_TRANSITION = "zig zag.jpg";
        /// <summary>
        /// Insert, Down Left.PNG
        /// </summary>
        public static string ZIG_ZAG_TRANSITION 
        {
            get
            {
                return _ZIG_ZAG_TRANSITION;
            }
        }

        private static string _SPIN_TRANSITION = "spin.jpg";
        /// <summary>
        /// Insert, Right Down.PNG
        /// </summary>
        public static string SPIN_TRANSITION
        {
            get
            {
                return _SPIN_TRANSITION;
            }
        }


        private static string _IRIS_TRANSITION = "Iris.PNG";
        /// <summary>
        /// Iris.PNG
        /// </summary>
        public static string IRIS_TRANSITION
        {
            get
            {
                return _IRIS_TRANSITION;
            }
        }

        private static string _ROTATE_TRANSITION ="Rotate.png";
        /// <summary>
        ///Rotate.png
        /// </summary>
        public static string ROTATE_TRANSITION
        {
            get
            {
                return _ROTATE_TRANSITION;
            }
        }

        #endregion

        #region " -- Transitions -- "

        /// <summary>
        /// Pixelate
        /// </summary>
        public const string Pixelate = "Pixelate";

        public const string ZigZag = "ZigZag";

        public const string Stretch = "Stretch";

        public const string CheckerBoard = "CheckerBoard";

        public const string RandomDissolve = "RandomDissolve";

        public const string Fade = "Fade";

        public const string Iris = "Iris";

        public const string RadialWipe = "RadialWipe";

        #endregion

        #region " -- Transitions Tool Tip -- "

        /// <summary>
        /// Zig Zag
        /// </summary>
        public const string ZigZag_ToolTip = "Zig Zag";

        /// <summary>
        /// CheckBoard
        /// </summary>
        public const string CheckerBoard_ToolTip = "CheckBoard";

        /// <summary>
        /// Dissolve
        /// </summary>
        public const string Dissolve_ToolTip = "Dissolve";

        /// <summary>
        /// Fade
        /// </summary>
        public const string Fade_ToolTip = "Fade";

        /// <summary>
        /// Pixellate
        /// </summary>
        public const string Pixelate_ToolTip = "Pixellate";

        /// <summary>
        /// Spin
        /// </summary>
        public const string Spin_ToolTip = "Spin";

        /// <summary>
        /// Rotate
        /// </summary>
        public const string Rotate_ToolTip = "Rotate";

        /// <summary>
        /// Iris
        /// </summary>
        public const string Iris_ToolTip = "Iris";

        #endregion

        #endregion

        #region " -- Effects -- "

        /// <summary>
        /// Blur.png
        /// </summary>
        public const string BLUR = "Blur.png";

        /// <summary>
        /// Brightness.PNG
        /// </summary>
        public const string BRIGHTNESS = "Brightness.PNG";

        /// <summary>
        /// FadeToBlack.PNG
        /// </summary>
        public const string FADETOBLACK = "FadeToBlack.PNG";

        /// <summary>
        /// Film, Old.PNG
        /// </summary>
        public const string FILM_OLD = "Film, Old.PNG";

        /// <summary>
        /// Mirror, Horizontal.PNG
        /// </summary>
        public const string MIRROR_HORIZONTAL = "Mirror, Horizontal.PNG";

        /// <summary>
        /// Pixellate.PNG
        /// </summary>
        public const string PIXELLATE = "Pixellate.PNG";

        /// <summary>
        /// Rotate 270.PNG
        /// </summary>
        public const string ROTATE_270 = "Rotate 270.PNG";

        /// <summary>
        /// Rotate 180.PNG
        /// </summary>
        public const string ROTATE_180 = "Rotate 180.PNG";

        /// <summary>
        /// Rotate 90.PNG
        /// </summary>
        public const string ROTATE_90 = "Rotate 90.PNG";

        #endregion

        /// <summary>
        /// diVideo.xml
        /// </summary>
        public const string VIDEO_FRAME_COLLECTION_XML = "diVideo.xml";

      
    }
}
