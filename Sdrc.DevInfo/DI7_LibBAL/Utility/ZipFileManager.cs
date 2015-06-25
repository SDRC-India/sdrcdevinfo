using System;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System.Collections;

namespace DevInfo.Lib.DI_LibBAL.Utility
{
    /// <summary>
    /// Helps in compressing the files and folders
    /// </summary>
    public static class ZipFileManager
    {

        #region "-- Private --"

        #region "-- Methods --"

        private static ArrayList GenerateFileList(string Dir)
        {
            ArrayList RetVal = new ArrayList();
            bool Empty = true;

            foreach (string FileName in Directory.GetFiles(Dir)) // add each file in directory
            {
                RetVal.Add(FileName);
                Empty = false;
            }

            if (Empty)
            {
                if (Directory.GetDirectories(Dir).Length == 0)
                // if directory is completely empty, add it
                {
                    RetVal.Add(Dir + @"/");
                }
            }

            foreach (string dirs in Directory.GetDirectories(Dir)) // recursive
            {
                foreach (object obj in GenerateFileList(dirs))
                {
                    RetVal.Add(obj);
                }
            }

            return RetVal; // return file list
        }

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Methods --"

        /// <summary>
        /// Zip Folder  
        /// </summary>
        /// <param name="inputFolderPath">Folder To Compress</param>
        /// <param name="outputPath"></param>
        /// <param name="password"></param>
        public static void ZipFolder(string inputFolderPath, string outputPath, string password)
        {
            ArrayList FileList = GenerateFileList(inputFolderPath); // generate file list
            string OutPath = string.Empty;
            ZipOutputStream ZipStream=null;
            ZipEntry ZipEntryObj=null;
            FileStream StreamObj=null;
            byte[] Buffer;

            try
            {
                int TrimLength = (Directory.GetParent(inputFolderPath)).ToString().Length;

                // find number of chars to remove     // from orginal file path
                TrimLength += 1; //remove '\'

                //string outPath = inputFolderPath + @"\" + outputPathAndFile;
                OutPath = outputPath;//  +".zip";

                ZipStream = new ZipOutputStream(File.Create(OutPath)); // create zip stream

                if (password != null && password != String.Empty)
                {
                    ZipStream.Password = password;
                }

                ZipStream.SetLevel(9); // maximum compression


                foreach (string Fil in FileList) // for each file, generate a zipentry
                {
                    ZipEntryObj = new ZipEntry(Fil.Remove(0, TrimLength));
                    ZipStream.PutNextEntry(ZipEntryObj);

                    if (!Fil.EndsWith(@"/")) // if a file ends with '/' its a directory
                    {
                        StreamObj = File.OpenRead(Fil);
                        Buffer = new byte[StreamObj.Length];
                        StreamObj.Read(Buffer, 0, Buffer.Length);
                        ZipStream.Write(Buffer, 0, Buffer.Length);

                        if (StreamObj != null)
                        {
                            StreamObj.Close();
                            StreamObj.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());
            }
            finally
            {
                if (ZipStream != null)
                {
                    // finish and close zip stream
                    ZipStream.Finish();
                    ZipStream.Close();
                }

                if (StreamObj != null)
                {
                    StreamObj.Close();
                    StreamObj.Dispose();
                }
            }

        }

        /// <summary>
        /// Zip Files Only
        /// </summary>
        /// <param name="fileNamesWPath"></param>
        /// <param name="outPutZipFileNameWPath"></param>
        /// <returns></returns>
        public static bool ZipFiles(List<string> fileNamesWPath, string outPutZipFileNameWPath)
        {
            bool RetVal = false;          

            // zip up the files
            try
            { 
            
                // Zip up the files - From SharpZipLib Demo Code
                using (ZipOutputStream ZipStream = new ZipOutputStream(File.Create(outPutZipFileNameWPath)))
                {
                    ZipStream.SetLevel(9); // 0-9, 9 being the highest level of compression
                    
                    byte[] Buffer = new byte[4096];

                    // Zip all files
                    foreach (string file in fileNamesWPath)
                    {

                        ZipEntry ZipEntryObj = new ZipEntry(Path.GetFileName(file));

                        //-- Support to unicode character specialy for khamer
                        ZipEntryObj.IsUnicodeText = true;

                        ZipEntryObj.DateTime = DateTime.Now;
                        ZipStream.PutNextEntry(ZipEntryObj);

                        // Open file and write bytes into Zip Stream
                        using (FileStream fs = File.OpenRead(file))
                        {
                            int SourceBytes;

                            do
                            {
                                SourceBytes = fs.Read(Buffer, 0, Buffer.Length);
                                ZipStream.Write(Buffer, 0, SourceBytes);

                            } while (SourceBytes > 0);
                        }
                    }

                    // Finish and close Zip Stream
                    ZipStream.Finish();
                    ZipStream.Close();
                }


                RetVal = true;
            }
            catch { RetVal = false; }
           
            return RetVal;
        }

        #endregion

        #endregion

    }
}
