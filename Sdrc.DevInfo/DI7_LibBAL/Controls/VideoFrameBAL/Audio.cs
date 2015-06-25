using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Controls.VideoFrameBAL
{
    public class Audio
    {
        #region " -- Public -- "

        #region " -- New / Dispose -- "

        public Audio(int audioIndex, string audioFileWpath, int startFrame, int endFrame)
        {
            this._AudioIndex = audioIndex;
            this._AudioFileWPath = audioFileWpath;
            this._StartFrameNo = startFrame;
            this._EndFrameNo = endFrame;
        }

        public Audio()
        { }

        #endregion

        #region " -- Variables / Properties -- "

        private int _AudioIndex = 0;
        /// <summary>
        /// Gets or sets the AudioIndex
        /// </summary>
        public int AudioIndex
        {
            get 
            {
                return this._AudioIndex; 
            }
            set 
            {
                this._AudioIndex = value; 
            }
        }
	

        private string _AudioFileWPath = string.Empty;
        /// <summary>
        /// Gets or sets the AudioFileWPath
        /// </summary>
        public string AudioFileWPath
        {
            get 
            {
                return this._AudioFileWPath; 
            }
            set 
            { 
                this._AudioFileWPath = value; 
            }
        }

        private int _StartFrameNo = 0;
        /// <summary>
        /// Gets or sets the StartFrameNo.
        /// </summary>
        public int StartFrameNo
        {
            get 
            {
                return this._StartFrameNo; 
            }
            set 
            {
                this._StartFrameNo = value; 
            }
        }

        private int _EndFrameNo = 0;
        /// <summary>
        /// Gets or sets the EndFrameNo
        /// </summary>
        public int EndFrameNo
        {
            get 
            {
                return this._EndFrameNo; 
            }
            set 
            {
                this._EndFrameNo = value; 
            }
        }
	
	
	

        #endregion

        #region " -- Methods -- "

        #endregion

        #endregion

        #region " -- Private -- "

        #region " -- Properties -- "

        #endregion

        #region " -- Methods -- "

        #endregion

        #endregion
    }
}
