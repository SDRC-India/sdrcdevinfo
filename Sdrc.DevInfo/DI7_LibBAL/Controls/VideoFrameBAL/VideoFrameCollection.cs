using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace DevInfo.Lib.DI_LibBAL.Controls.VideoFrameBAL
{
    public class VideoFrameCollection : CollectionBase
    {
        #region " -- Public -- "

        #region " -- Methods -- "

        /// <summary>
        /// Add the videoframe in the collection.
        /// </summary>
        /// <param name="videoFrame"></param>
        public void Add(VideoFrame videoFrame)
        {
            this.InnerList.Add(videoFrame);
        }	

        /// <summary>
        /// Get the video frame on the basis of selected image.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public VideoFrame this[string name]
        {
            get
            {
                VideoFrame Retval = null;
                try
                {
                    foreach (VideoFrame Video in InnerList)
                    {
                        if (Video.SelectedFilePathWithPath.ToLower() == name.ToLower())
                        {
                            Retval = Video;
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    Retval = null;
                }
                return Retval;
            }
        }

        /// <summary>
        /// Get the video frame on the basis of its index.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public VideoFrame this[int order]
        {
            get
            {
                VideoFrame Retval = null;
                try
                {
                    foreach (VideoFrame Video in InnerList)
                    {
                        if (Video.FrameIndex == order)
                        {
                            Retval = Video;
                            break;
                        }
                    }
                }
                catch (Exception)
                {
                    Retval = null;
                }
                return Retval;
            }
        }

        /// <summary>
        /// Swap the frames.
        /// </summary>
        /// <param name="selectedFrame"></param>
        /// <param name="newIndex"></param>
        /// <param name="oldIndex"></param>
        public void SwapFrame(int newIndex, int oldIndex)
        {
            try
            {
                // -- Swap the frames
                if (oldIndex < newIndex)
                {

                    this.InnerList.Insert(oldIndex, this[newIndex]);
                    this.InnerList.RemoveAt(newIndex + 1);
                    this.InnerList.Insert(newIndex + 1, this[oldIndex]);
                    this.InnerList.RemoveAt(oldIndex + 1);
                }
                else if (oldIndex > newIndex)
                {

                    this.InnerList.Insert(oldIndex, this[newIndex]);
                    this.InnerList.RemoveAt(newIndex);
                    this.InnerList.Insert(newIndex, this[oldIndex]);
                    this.InnerList.RemoveAt(oldIndex + 1);
                }
                // -- Set the order of the frame.
                ((VideoFrame)this.InnerList[newIndex]).FrameIndex = newIndex;
                ((VideoFrame)this.InnerList[oldIndex]).FrameIndex = oldIndex;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Remove the frame for the specified index.
        /// </summary>
        /// <param name="frameIndex"></param>
        public void RemoveAt(int frameIndex)
        {
            try
            {
                this.InnerList.RemoveAt(frameIndex);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Remove the range of frame
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        public void RemoveRange(int index, int count)
        {
            this.InnerList.RemoveRange(index, count);
        }

        /// <summary>
        /// Remove the frame object.
        /// </summary>
        /// <param name="videoFrame"></param>
        public void Remove(VideoFrame videoFrame)
        {
            try
            {
                this.InnerList.Remove(videoFrame);
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region " -- Save / Load -- "

        public void Save(string filenameWithPath)
        {
            try
            {
                XmlSerializer FrameSerialize = new XmlSerializer(typeof(VideoFrameCollection));
                StreamWriter FrameWriter = new StreamWriter(filenameWithPath);
                FrameSerialize.Serialize(FrameWriter, this);
                FrameWriter.Close();
            }
            catch (Exception)
            {
            }
        }

        public static VideoFrameCollection Load(string filenameWithPath)
        {
            VideoFrameCollection RetVal;
            try
            {
                XmlSerializer FrameSerialize = new XmlSerializer(typeof(VideoFrameCollection));
                TextReader FrameReader = new StreamReader(filenameWithPath);
                RetVal = (VideoFrameCollection)FrameSerialize.Deserialize(FrameReader);
                FrameReader.Close();
            }
            catch (Exception)
            {
                RetVal = null;
            }
            return RetVal;
        }

        #endregion

        #endregion
    }
}
