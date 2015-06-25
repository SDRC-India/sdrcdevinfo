using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace DevInfo.Lib.DI_LibBAL.Controls.VideoFrameBAL
{
    public class VideoFrame
    {
        #region " -- Public -- "

        #region " -- New / Dispose -- "

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="selectedFilename"></param>
        /// <param name="frameTransition"></param>
        /// <param name="frameEffect"></param>
        /// <param name="frameIndex"></param>
        /// <param name="transitionTime"></param>
        public VideoFrame(string selectedFilename, string frameTransition,string transitionFileName, FrameEffect frameEffect, int frameIndex, int transitionTime, Size frameSize, int videoTime,string transtionToolTip)
        {
            this._SelectedFilePathWithPath = selectedFilename;
            this._FrameTransition = frameTransition;
            this._TransitionFileName = transitionFileName;
            this._Effect = frameEffect;
            this._FrameIndex = frameIndex;
            this._TransitionTime = transitionTime;
            this._FrameSize = frameSize;
            this._VideoTime = videoTime;
            this._TransitionToolTip = transtionToolTip;
        }

        public VideoFrame(string selectedFilename, string frameTransition, string transitionFileName, FrameEffect frameEffect, int frameIndex, int transitionTime, Size frameSize, int videoTime, string transtionToolTip, AudioCollection audiocollection)
            : this(selectedFilename, frameTransition, transitionFileName, frameEffect, frameIndex, transitionTime, frameSize, videoTime, transtionToolTip)
        {
            this._BackgroundAudio = audiocollection;
        }

        /// <summary>
        ///  Condtructor only fro serialization purpose.
        /// </summary>
        public VideoFrame()
        { 
            //-- Do Nothing
        }
        
        #endregion

        #region  " -- Properties -- "

        private string _SelectedFilePathWithPath = string.Empty;
        /// <summary>
        /// Gets or sets the selected file path.
        /// </summary>
        public string SelectedFilePathWithPath
        {
            get 
            {
                return this._SelectedFilePathWithPath; 
            }
            set 
            {
                this._SelectedFilePathWithPath = value; 
            }
        }

        private string _FrameTransition = string.Empty;
        /// <summary>
        /// Gets or sets the frame transition.
        /// </summary>
        public string FrameTransition
        {
            get 
            {
                return this._FrameTransition; 
            }
            set 
            {
                this._FrameTransition = value; 
            }
        }

        private string _TransitionFileName = string.Empty;
        /// <summary>
        /// Gets or sets the transition file name.
        /// </summary>
        public string TransitionFileName
        {
            get 
            { 
                return this._TransitionFileName; 
            }
            set 
            {
                this._TransitionFileName = value; 
            }
        }

        private string _TransitionToolTip = string.Empty;
        /// <summary>
        /// Gets or sets the transition toop tip
        /// </summary>
        public string TransitionToolTip
        {
            get 
            {
                return this._TransitionToolTip; 
            }
            set 
            {
                this._TransitionToolTip = value; 
            }
        }	

        private FrameEffect _Effect;
        /// <summary>
        /// Gets or sets the frame effect.
        /// </summary>
        public FrameEffect Effect
        {
            get 
            {
                return this._Effect; 
            }
            set 
            {
                this._Effect = value; 
            }
        }

        private int _FrameIndex = 0;
        /// <summary>
        /// Gets or sets the frame order.
        /// </summary>
        public int FrameIndex
        {
            get 
            {
                return this._FrameIndex; 
            }
            set 
            {
                this._FrameIndex = value; 
            }
        }

        private int _TransitionTime = 2;
        /// <summary>
        /// Gets or sets the transition time.
        /// </summary>
        public int TransitionTime
        {
            get 
            { 
                return this._TransitionTime; 
            }
            set 
            {
                this._TransitionTime = value; 
            }
        }

        private Size _FrameSize = new Size(350, 350);
        /// <summary>
        /// Gets or sets the frame size.
        /// </summary>
        public Size FrameSize
        {
            get 
            {
                return this._FrameSize; 
            }
            set 
            {
                this._FrameSize = value; 
            }
        }

        private int _VideoTime = 3;
        /// <summary>
        /// Gets or sets the video time.
        /// </summary>
        public int VideoTime
        {
            get 
            {
                return this._VideoTime; 
            }
            set 
            {
                this._VideoTime = value; 
            }
        }

        private AudioCollection _BackgroundAudio = new AudioCollection();
        /// <summary>
        /// Gets or sets the BackgroundAudio
        /// </summary>
        public AudioCollection BackgroundAudio
        {
            get 
            {
                return this._BackgroundAudio; 
            }
            set 
            {
                this._BackgroundAudio = value; 
            }
        }
	
	
        #endregion     

        #endregion
    }    
}
