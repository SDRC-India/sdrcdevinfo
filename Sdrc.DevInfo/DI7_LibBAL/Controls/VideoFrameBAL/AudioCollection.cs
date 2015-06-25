using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Controls.VideoFrameBAL
{
    public class AudioCollection : System.Collections.CollectionBase
    {
        #region " -- Public -- "

        #region " -- New / Dispose -- "

        #endregion

        #region " -- Properties -- "

        #endregion

        #region " -- Methods -- "

        /// <summary>
        /// Add the audio object in the collection.
        /// </summary>
        /// <param name="audio"></param>
        public void Add(Audio audio)
        {
            this.InnerList.Add(audio);
        }

        /// <summary>
        /// Get the Audio object.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Audio this[int index]
        {
            get
            {
                Audio RetVal = null;
                try
                {
                    RetVal = (Audio)this.InnerList[index];
                }
                catch (Exception)
                {
                }
                return RetVal;
            }
        }

        /// <summary>
        /// Remove the audio object from the collection.
        /// </summary>
        /// <param name="audio"></param>
        public void Remove(Audio audio)
        {
            this.InnerList.Remove(audio);
        }

        /// <summary>
        /// Remove the audio object from the collection on the basis of index.
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            if (this.InnerList.Count < index)
            {
                this.InnerList.RemoveAt(index);
            }
        }

        #endregion

        #endregion
    }
}
