using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

using System.Drawing;
using System.Drawing.Imaging;

namespace DevInfo.Lib.DI_LibBAL.Controls.VideoFrameBAL
{
    public class DIVideoSource
    {
        #region " -- Public -- "

        #region " -- New / Dispose -- "

        public DIVideoSource(string videoPath)
        {
            this.VideoFileWPath = videoPath;

            //-- Get the divideo folder path.
            if (!string.IsNullOrEmpty(this.VideoFileWPath))
            {
                string diVideo = Path.GetFileNameWithoutExtension(this.VideoFileWPath);

                this.VideoFolderPath = Path.Combine(Path.GetDirectoryName(this.VideoFileWPath), diVideo);                
            }
        }

        #endregion

        #region " -- Properties -- "

        #endregion

        #region " -- Methods -- "

        /// <summary>
        /// Get the first frame of di video
        /// </summary>
        /// <returns></returns>
        public string GetFirstVideoFrame()
        {
            string Retval = string.Empty;

            try
            {                
                if (Directory.Exists(this.VideoFolderPath))
                {
                    FileInfo[] VideoFiles = new FileInfo[0];

                    DirectoryInfo VideoDir = new DirectoryInfo(this.VideoFolderPath);
                    VideoFiles = VideoDir.GetFiles("*.xml");

                    //-- Iterate through each of the xml file and get the path of first frame.
                    foreach (FileInfo ImageFile in VideoFiles)
                    {
                        VideoFrameCollection VideoFrames = VideoFrameCollection.Load(ImageFile.FullName);
                        if (VideoFrames != null)
                        {
                            foreach (VideoFrame Frame in VideoFrames)
                            {
                                if (!string.IsNullOrEmpty(Frame.SelectedFilePathWithPath) && File.Exists(Frame.SelectedFilePathWithPath))
                                {
                                    Retval = Frame.SelectedFilePathWithPath;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        /// <summary>
        /// Check whether for the validity of DI Video
        /// </summary>
        /// <returns></returns>
        public bool ValiddiVideo()
        {
            bool Retval = false;
            try
            {
                if (Directory.Exists(this.VideoFolderPath))
                {
                    FileInfo[] VideoFiles = new FileInfo[0];

                    //-- Get the xml files
                    DirectoryInfo VideoDir = new DirectoryInfo(this.VideoFolderPath);
                    VideoFiles = VideoDir.GetFiles("*.xml");

                    foreach (FileInfo ImageFile in VideoFiles)
                    {
                        VideoFrameCollection VideoFrames = VideoFrameCollection.Load(ImageFile.FullName);
                        if (VideoFrames != null)
                        {
                            Retval = true;
                            break;                            
                        }
                    }
                }

            }
            catch (Exception)
            {
            }
            return Retval;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public VideoFrameCollection BuildVideoFrameCollection(string folderPath, string tempFolderPath)
        {
            VideoFrameCollection Retval = new VideoFrameCollection();
            Size FrameSize = new Size(350, 350);
            bool IsSizeReadFromImage = false;

            try
            {
                int FrameIndex=0;
                string TempFolderImage = string.Empty;
                FrameEffect FrameEffect = new FrameEffect(-1, 2, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

               

                if (Directory.Exists(folderPath))
                {                    
                    //-- Get the Image files
                    FileInfo[] VideoFiles = new FileInfo[0];
                    DirectoryInfo VideoDir = new DirectoryInfo(folderPath);
                    VideoFiles = VideoDir.GetFiles("*.jpg");

                    //-- Add the jpg images
                    foreach (FileInfo JpegFrame in VideoFiles)
                    {
                        //-- Read the image size
                        if (!IsSizeReadFromImage)
                        {
                            FrameSize = GetImageSize(JpegFrame.FullName);
                            IsSizeReadFromImage = true;
                        }

                        //////-- Copy the image in the temp folder.
                        ////TempFolderImage = Path.Combine(tempFolderPath, Path.GetFileName(JpegFrame.FullName));
                        ////File.Copy(JpegFrame.FullName, TempFolderImage, true);
                        Retval.Add(new VideoFrame(JpegFrame.FullName, string.Empty, string.Empty, FrameEffect, FrameIndex, 1, FrameSize, 3, string.Empty));
                    }

                    //-- Add the jpg images
                    VideoFiles = VideoDir.GetFiles("*.png");
                    foreach (FileInfo PngFrame in VideoFiles)
                    {
                        //-- Read the image size
                        if (!IsSizeReadFromImage)
                        {
                            FrameSize = GetImageSize(PngFrame.FullName);
                            IsSizeReadFromImage = true;
                        }

                        //-- Convert the image in to jpeg and save in the divideo temp folder.
                        string JpegImagePath = Path.Combine(tempFolderPath, Path.GetFileNameWithoutExtension(PngFrame.FullName) + ".jpg");
                        ConvertPngToJpeg(PngFrame.FullName, JpegImagePath);

                        Retval.Add(new VideoFrame(JpegImagePath, string.Empty, string.Empty, FrameEffect, FrameIndex, 1, FrameSize, 3, string.Empty));
                    }

                }
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public VideoFrameCollection BuildVideoFrameCollection(string folderPath, string tempFolderPath, Size imagesize, int tranisitionTime, string transition, string transitionFileName, string frameTransition)
        {
            VideoFrameCollection Retval = new VideoFrameCollection();

            try
            {
                int FrameIndex = 0;
                string TempFolderImage = string.Empty;
                FrameEffect FrameEffect = new FrameEffect(-1, 2, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);

                if (Directory.Exists(folderPath))
                {
                    //-- Get the Image files
                    FileInfo[] VideoFiles = new FileInfo[0];
                    DirectoryInfo VideoDir = new DirectoryInfo(folderPath);
                    VideoFiles = VideoDir.GetFiles("*.jpg");

                    //-- Add the jpg images
                    foreach (FileInfo JpegFrame in VideoFiles)
                    {
                        //////-- Copy the image in the temp folder.
                        Retval.Add(new VideoFrame(JpegFrame.FullName, frameTransition, transitionFileName, FrameEffect, FrameIndex, tranisitionTime, imagesize, 3, transition));
                    }

                    //-- Add the jpg images
                    VideoFiles = VideoDir.GetFiles("*.png");
                    foreach (FileInfo PngFrame in VideoFiles)
                    {
                        //-- Convert the image in to jpeg and save in the divideo temp folder.
                        string JpegImagePath = Path.Combine(tempFolderPath, Path.GetFileNameWithoutExtension(PngFrame.FullName) + ".jpg");
                        ConvertPngToJpeg(PngFrame.FullName, JpegImagePath);

                        Retval.Add(new VideoFrame(JpegImagePath, frameTransition, transitionFileName, FrameEffect, FrameIndex, tranisitionTime, imagesize, 3, transition));
                    }

                }
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        public static void ConvertPngToJpeg(string sourceFilePath, string destinationFilePath)
        {
            try
            {
                ImageCodecInfo[] ImageCodecs = new ImageCodecInfo[0];
                EncoderParameters Encoder;
                Bitmap Bmpmages = new Bitmap(sourceFilePath);
                ImageCodecs = ImageCodecInfo.GetImageEncoders();
                Encoder = new EncoderParameters(1);

                Encoder.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 100);

                Bmpmages.Save(destinationFilePath);

                Bmpmages.Dispose();
                Encoder.Dispose();
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #endregion

        #region " -- Private -- "

        #region " -- Variables -- "

        /// <summary>
        /// diVideo File with path
        /// </summary>
        private string VideoFileWPath = string.Empty;

        /// <summary>
        /// Contains the path of folder containing diVideo images and xml
        /// </summary>
        private string VideoFolderPath = string.Empty;

        #endregion

        #region " -- Methods -- "

        /// <summary>
        /// Get the image size for image
        /// </summary>
        /// <param name="imageFilePath"></param>
        /// <returns></returns>
        private Size GetImageSize(string imageFilePath)
        {
            Size Retval = new Size(350, 350);
            try
            {
                Image ThumbnailImage = null;
                FileStream ImageStream = new FileStream(imageFilePath, FileMode.Open);
                ThumbnailImage = new Bitmap(ImageStream);
                ImageStream.Close();                
                Retval = new Size(ThumbnailImage.Width, ThumbnailImage.Height);

            }
            catch (Exception)
            {
            }
            return Retval;
        }

        #endregion

        #endregion
    }
}
