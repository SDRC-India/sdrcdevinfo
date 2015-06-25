using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace DevInfo.Lib.DI_LibBAL.Utility
{
    /// <summary>
    /// Unzips the files and folders
    /// </summary>
    public static class UnZipFilesManager
    {
        #region "-- Public --"

        #region "-- Methods --"

        /// <summary>
        /// Extract ZipFile in same folder Here
        /// </summary>
        /// <param name="InputPathOfZipFile"></param>
        /// <returns></returns>
        public static bool UnZipFileHere(string InputPathOfZipFile)
        {
            bool ret = true;
            try
            {
                if (File.Exists(InputPathOfZipFile))
                {
                    string baseDirectory = Path.GetDirectoryName(InputPathOfZipFile);

                    using (ZipInputStream ZipStream = new

ZipInputStream(File.OpenRead(InputPathOfZipFile)))
                    {
                        ZipEntry theEntry;
                        while ((theEntry = ZipStream.GetNextEntry()) != null)
                        {
                            if (theEntry.IsFile)
                            {
                                if (theEntry.Name != "")
                                {
                                    string strNewFile = @"" + baseDirectory + @"\" +

theEntry.Name;
                                    if (File.Exists(strNewFile))
                                    {
                                        continue;
                                    }

                                    using (FileStream streamWriter = File.Create(strNewFile))
                                    {
                                        int size = 2048;
                                        byte[] data = new byte[2048];
                                        while (true)
                                        {
                                            size = ZipStream.Read(data, 0, data.Length);
                                            if (size > 0)
                                                streamWriter.Write(data, 0, size);
                                            else
                                                break;
                                        }
                                        streamWriter.Close();
                                    }
                                }
                            }
                            else if (theEntry.IsDirectory)
                            {
                                string strNewDirectory = @"" + baseDirectory + @"\" +

theEntry.Name;
                                if (!Directory.Exists(strNewDirectory))
                                {
                                    Directory.CreateDirectory(strNewDirectory);
                                }
                            }
                        }
                        ZipStream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ret = false;
            }
            return ret;
        }


        /// <summary>
        /// Unzip Files in Folder 
        /// </summary>
        /// <param name="zipPathAndFile"></param>
        /// <param name="outputFolder"></param>
        /// <param name="password"></param>
        /// <param name="deleteZipFile"></param>
        public static void UnZipFilesInFolder(string zipPathAndFile, string outputFolder, string password, bool deleteZipFile)
        {
            ZipEntry ZipEntryObj;
            ZipInputStream ZipStream;
            string TempEntry = String.Empty;

            ZipStream = new ZipInputStream(File.OpenRead(zipPathAndFile));

            if (password != null && password != String.Empty)
            {
                ZipStream.Password = password;
            }


            while ((ZipEntryObj = ZipStream.GetNextEntry()) != null)
            {
                string directoryName = outputFolder;
                string fileName = Path.GetFileName(ZipEntryObj.Name);

                // create directory 
                if (directoryName != "")
                {
                    Directory.CreateDirectory(directoryName);
                }
                if (fileName != String.Empty)
                {
                    if (ZipEntryObj.Name.IndexOf(".ini") < 0)
                    {
                        string fullPath = directoryName + "\\" + ZipEntryObj.Name;
                        fullPath = fullPath.Replace("\\ ", "\\");
                        string fullDirPath = Path.GetDirectoryName(fullPath);
                        if (!Directory.Exists(fullDirPath)) Directory.CreateDirectory(fullDirPath);
                        FileStream streamWriter = File.Create(fullPath);
                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = ZipStream.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                        streamWriter.Close();
                    }
                }
            }

            ZipStream.Close();

            if (deleteZipFile)
            {
                File.Delete(zipPathAndFile);
            }
        }

        #endregion

        #endregion
    }
}
