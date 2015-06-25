using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;

namespace DevInfo.Lib.DI_LibBAL.Utility
{
    /// <summary>
    /// Class is used in DILiveUpdate client program. This class provides functions to check for DI live updates on server. 
    /// </summary>
    public class DILiveUpdate
    {
        #region "-- Private --"

        #region "-- Variables --"

        //FTP connection details required for FTP Login
        private string FTPServerName = string.Empty;      // ; "61.12.1.180"
        private string FTPUserName = string.Empty;     // ;"testftpdevgrp"
        private string FTPPassCode = string.Empty;        // "testftpdevgrp"  
        private const string FTPPort = "21";                  // -- Static port address of FTP :: Default port for FTP is 21.
        private string FTPDirectoryName = string.Empty;              // //@""; DevInfo Live Update Folder

        private string DILU_AppDirInfoXmlFileName = "DILU_AppDirInfo.xml";
                
        private string LocalAdaptationFolder = string.Empty;

        public static string DatabaseIncrementXmlFileName = "DatabaseIncrement.xml";
        public static string DatabaseIncrementFolderName = "Database Increment";
        public static string DatabaseIncrementFileType = "DatabaseIncrement";

        //---Calling Application's details
        private string ApplicationName = string.Empty;
        private string AppInstallationPath = string.Empty;
        private string Adaptation = string.Empty;

        private string TempFolder = string.Empty;
        

        private List<DIFileInfo> FTPFilesForUpdate = null;     //-- List of server Files which are different from local counterpart and are ready for LiveUpdate.

        private FTPClient DIFTPClient = new FTPClient();    //-- FTPClient object used through out the class.

        //- String that will contain the log event of all Live Update excecutions through out the process
        public static string LiveUpdateLog = string.Empty;     

        #region "-- Constant --"

        private const string FILE_NODE = "File";                // -- Name of Node (<File>) in Manifest.xml file

        //-- Desired format of Modified DateTime of server files .
        private const string DATETIME_FORMAT = "MM/dd/yyyy hh:mm:ss";      

        #endregion

        #endregion

        #region "-- Method --"

        #region "-- File Comparison --"

        /// <summary>
        /// It compares each Server file's information with corresponding local file and generates the list of files which are required for updation.
        /// </summary>
        /// <param name="xmlFileList">List of files available in Server Xml file.</param>
        /// <param name="localApplicationPath">local application path.</param>
        /// <param name="applicationName">Hosting application Name</param>
        private void CompareAndGetFTPFilesListForUpdate(List<DIFileInfo> xmlFileList, string localApplicationPath, string applicationName)
        {
            DIFileInfo LocalFileInfo;
            string LocalFileName = string.Empty;
            DIFileInfo TempServerFileInfo;
            bool IsServerFileEqualsToLocal = false;

            foreach (DIFileInfo serverFileInfo in xmlFileList)
            {
                //-- Add application Name & local Path in server FileInfo.
                TempServerFileInfo = serverFileInfo;
                TempServerFileInfo.ApplicationPath = localApplicationPath;
                TempServerFileInfo.ApplicationName = applicationName;

                //- Get local counterpart information
                LocalFileName = Path.Combine(localApplicationPath + @"\", TempServerFileInfo.FilePath);
                LocalFileInfo = this.FetchFileInformation(LocalFileName);

                //-- Compare Server's FileInfo with local counterpart.
                IsServerFileEqualsToLocal = this.CompareServerFileWithLocal(TempServerFileInfo, LocalFileInfo);    // Return false if found different from local file.

                if (IsServerFileEqualsToLocal == false)
                {
                    //-- Add FileInfo in the list.
                    if (this.FTPFilesForUpdate == null)
                    {
                        this.FTPFilesForUpdate = new List<DIFileInfo>();
                    }

                    this.FTPFilesForUpdate.Add(TempServerFileInfo);
                }

                //-- Add ServerFileInfo & LocalFileInfo into list of Available FTP Files.
                ServerOfflineFileInfoPair ServerAndLocalFiles = new ServerOfflineFileInfoPair(TempServerFileInfo, LocalFileInfo);
                this._FTPAllAvailableFiles.Add(ServerAndLocalFiles, IsServerFileEqualsToLocal);
            }
        }

        /// <summary>
        /// Compares Server's file infomration with local counterpart.
        /// </summary>
        /// <param name="serverFileInfo">FileInfo variable of server file.</param>
        /// <returns>false, if server FileInfo is different from local one.</returns>
        public bool CompareServerFileWithLocal(DIFileInfo serverFileInfo, DIFileInfo localFileInfo)
        {
            bool RetVal = false;

            string FilePath = string.Empty;

            if (string.IsNullOrEmpty(serverFileInfo.FileName) == false)
            {
                if (Convert.ToBoolean(serverFileInfo.ForceUpdate))
                {
                    RetVal = false;
                }
                else
                {
                    if (string.IsNullOrEmpty(localFileInfo.FileName) == false)
                    {
                        //-- Compare two ServerFileInfo with localFileInfo
                        // -- Compare FileName, FileType, FileVersion, ModifiedDateTimeStamp
                        if ((serverFileInfo.FileName.ToLower() == localFileInfo.FileName.ToLower()))
                        {
                            //-- compare version
                            if (this.CheckVersionDiffrence(serverFileInfo.Version, localFileInfo.Version) == false)              //serverFileInfo.Version == localFileInfo.Version)
                            {
                                try
                                {
                                    DateTime ServerFileDateTime = Convert.ToDateTime(serverFileInfo.ModifiedDateTimeStamp);
                                    DateTime LocalFileDateTime = Convert.ToDateTime(localFileInfo.ModifiedDateTimeStamp);

                                    //-- Compare modified DateTimeStamp of two fileInfo.
                                    if (ServerFileDateTime.CompareTo(LocalFileDateTime) <= 0)
                                    {
                                        RetVal = true;
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                    }
                }
            }

            return RetVal;
        }

        /// <summary>
        /// Return true if Version1 is different from version 2, else false.
        /// </summary>
        /// <param name="version1">version1 string in fomrat x.x.x.x</param>
        /// <param name="version2">version2 string in fomrat x.x.x.x</param>
        /// <returns></returns>
        private bool CheckVersionDiffrence(string newVersion, string oldVersion)
        {
            bool RetVal = false;
            //decimal iVersion1 = 0;
            //decimal iVersion2 = 0;

            if (string.IsNullOrEmpty(newVersion) == false && string.IsNullOrEmpty(oldVersion) == false)
            {
                Version NewV1 = new Version(newVersion);
                Version OldV2 = new Version(oldVersion);

                if (NewV1.CompareTo(OldV2) > 0)
                {
                    RetVal = true;
                }

                //newVersion = newVersion.Replace(".", "");
                //oldVersion = oldVersion.Replace(".", "");
                //if (decimal.TryParse(newVersion, out iVersion1))
                //{
                //    if (decimal.TryParse(oldVersion, out iVersion2))
                //    {
                //        if (iVersion1 > iVersion2)
                //        {
                //            RetVal = true;
                //        }
                //    }
                //}
            }

            return RetVal;
        }


        /// <summary>
        /// Return FileType used in DILU_AppDirInfo.xml, on the basis of file extention.
        /// </summary>
        /// <param name="fileExtention">file extention.</param>
        /// <returns></returns>
        private string GetFileType(string fileNameWPath)
        {
            string RetVal = string.Empty;

            System.IO.FileInfo fileInfo = new System.IO.FileInfo(fileNameWPath);

            if (string.IsNullOrEmpty(fileInfo.Extension) == false)
            {
                switch (fileInfo.Extension.ToLower())
                {
                    case ".mdb":
                        RetVal = FileType.DATABASE;
                        break;
                    case ".pps":
                    case ".ppt":
                    case ".doc":
                    case ".docx":
                    case ".pdf":
                        RetVal = FileType.DOCUMENT;
                        break;
                    case ".xml":
                    default:
                        RetVal = FileType.APPLICATION_FILE;
                        break;
                }
            }

            // NOTE: Special handling of LanguageType
            // if .xml file is in LanguageFolder (..Language\")
            if (this.IsFileOfLanguageType(fileNameWPath))
            {
                RetVal = FileType.LANGUAGE;
            }

            // Special Handling for DA - EmergencyInfo Folder
            // if file is present in EmergencyInfo Folder
            else if (fileInfo.DirectoryName.Contains("\\" + DICommon.DAEmergencyInfoFolderName))
            {
                RetVal = FileType.DA_EMERGENCYINFO;
            }

            // Special Handling for DA - Exchange Folder
            // if file is present in DA - Exchange Folder
            else if (fileInfo.DirectoryName.Contains("\\" + DICommon.DAExchangeFolderName))
            {
                RetVal = FileType.DA_EXCHANGES;
            }

            // Special Handling for DA - Standards Folder
            // if file is present in DA - Standards Folder
            else if (fileInfo.DirectoryName.Contains("\\" + DICommon.DAStandardsFolderName))
            {
                RetVal = FileType.DA_STANDARDS;
            }

            return RetVal;
        }

        /// <summary>
        /// Gets the Assembly version of assembly file.
        /// </summary>
        /// <returns>string version number.</returns>
        private string GetVersion(string fileName)
        {
            string RetVal = string.Empty;
            //System.Diagnostics.FileVersionInfo finfo = null;
            try
            {

                //finfo = FileVersionInfo.GetVersionInfo(fileName);
                System.Reflection.AssemblyName an = AssemblyName.GetAssemblyName(fileName);
                RetVal = an.Version.ToString();

            }
            catch (Exception)
            {

                // Do nothing
            }
            //if (finfo != null && finfo.FileVersion != null)
            //{
            //    RetVal = finfo.FileVersion.ToString();
            //}

            return RetVal;
        }

        /// <summary>
        /// Fetches file information of specified fileName required for comparison with server file info..
        /// </summary>
        /// <param name="fileNameWPath">file Name with path.</param>
        /// <returns>FileInfo object.</returns>
        public DIFileInfo FetchFileInformation(string fileNameWPath)
        {
            DIFileInfo RetVal = new DIFileInfo();
            System.IO.FileInfo fileInfo;
            if (File.Exists(fileNameWPath))
            {
                fileInfo = new System.IO.FileInfo(fileNameWPath);

                // Filename
                RetVal.FileName = Path.GetFileName(fileNameWPath);

                // File Type
                // get fileType on the basis of file extention.
                // NOTE: Special handling of LanguageType if .xml file is in LanguageFolder (..Language\")
                RetVal.FileType = this.GetFileType(fileNameWPath);

                //- Modified Date-Time stamp.     
                //- Get Modified DateTime in desired format which is kept similar as Server's format.
                RetVal.ModifiedDateTimeStamp = fileInfo.LastWriteTime.ToString();

                // File Size
                RetVal.FileSize = fileInfo.Length.ToString();     

                // Version of file
                RetVal.Version = this.GetVersion(fileNameWPath);

                fileInfo = null;
            }

            return RetVal;
        }

        private bool IsFileOfLanguageType(string fileNameWPath)
        {
            bool RetVal = false;
            if (string.IsNullOrEmpty(fileNameWPath) == false)
            {
                string FileExtension = Path.GetExtension(fileNameWPath);

                if (FileExtension == ".xml")
                {
                    // if filePath is in Adaptation's Language Folder 
                    // i.e "..[Adaptation]\Language\"
                    if (Path.GetDirectoryName(fileNameWPath).EndsWith("\\Language"))
                    {
                        RetVal = true;
                    }
                }
            }

            return RetVal;
        }

        #endregion

        private void ClearFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                foreach (string subFolder in Directory.GetDirectories(folderPath))
                {
                    try
                    {
                        foreach (string file in Directory.GetFiles(subFolder))
                        {
                            try
                            {
                                File.Delete(file);
                            }
                            catch
                            {
                            }
                        }
                        //Delete Directory finally
                        Directory.Delete(subFolder);
                    }
                    catch
                    {
                    }
                }
            }
        }


        private string GetFTPFileReferencePath(string ftpFilePath)
        {
            string RetVal = string.Empty;

            if (string.IsNullOrEmpty(ftpFilePath) == false)
            {
                if (ftpFilePath.Contains("/"))
                {
                    RetVal = ftpFilePath.Substring(0, ftpFilePath.LastIndexOf('/'));
                }
                else if (ftpFilePath.Contains(@"\"))
                {
                    RetVal = ftpFilePath.Substring(0, ftpFilePath.LastIndexOf(@"\"));
                    RetVal = RetVal.Replace(@"\", "/");
                }
            }
            return RetVal;
        }

        /// <summary>
        /// Gets the list of FileInfo of Server Files. Each FileInfo object is for each file's information in DI_AppDirInfo.xml
        /// </summary>
        /// <param name="xmlFileName">DI_AppDirInfo.xml file path</param>
        /// <returns></returns>
        private List<DIFileInfo> GetFileListFromXML(string xmlFileName)
        {
            List<DIFileInfo> RetVal = new List<DIFileInfo>();
            XmlDocument ManifestXmlDoc = new XmlDocument();
            XmlNodeList FileNodesList = null;
            DIFileInfo ServerFileInfo;

            if (File.Exists(xmlFileName))
            {
                //-- Load Manifest xml file.
                ManifestXmlDoc.Load(xmlFileName);

                //-- Get all XmlNode <File> in  xml document.
                FileNodesList = ManifestXmlDoc.DocumentElement.SelectNodes(@"//" + DILiveUpdate.FILE_NODE);

                if (FileNodesList != null && FileNodesList.Count > 0)
                {
                    for (int i = 0; i < FileNodesList.Count; i++)
                    {
                        if (FileNodesList[i].HasChildNodes)
                        {
                            try
                            {
                                ServerFileInfo = new DIFileInfo();
                                //-- Get FileName, FilePath, FileVersion, FileType, ModifiedDateTimeStamp from xmlNode,
                                //-- and assign into FileInfo variable.

                                ServerFileInfo.FileName = FileNodesList[i].ChildNodes[0].InnerText;
                                ServerFileInfo.FilePath = FileNodesList[i].ChildNodes[1].InnerText;
                                ServerFileInfo.Version = FileNodesList[i].ChildNodes[2].InnerText;
                                ServerFileInfo.FileType = FileNodesList[i].ChildNodes[3].InnerText;
                                ServerFileInfo.ModifiedDateTimeStamp = FileNodesList[i].ChildNodes[4].InnerText;
                                
                                // Convert this Date Time to local System DateTime Format
                                // Server Date Time Format = mm/dd/yyyy hh:mm:ss
                                // STEP 1: Get Date Time Format according to the current Culture
                                // STEP 2: Convert the server Date Modified into the current cilture format
                                string sDate = FileNodesList[i].ChildNodes[4].InnerText.Split(' ')[0];
                                string sTime = FileNodesList[i].ChildNodes[4].InnerText.Split(' ')[1];
                                string sCurrentDateMM = sDate.Split('/')[0];
                                string sCurrentDateDD = sDate.Split('/')[1];
                                string sCurrentDateYY = sDate.Split('/')[2];
                                
                                string sCurrentDateHH = sTime.Split(':')[0];
                                string sCurrentDateMIN = sTime.Split(':')[1];
                                string sCurrentDateSEC = sTime.Split(':')[2];

                                DateTime dDateTime = new DateTime(Convert.ToInt32(sCurrentDateYY), Convert.ToInt32(sCurrentDateMM), Convert.ToInt32(sCurrentDateDD), Convert.ToInt32(sCurrentDateHH), Convert.ToInt32(sCurrentDateMIN), Convert.ToInt32(sCurrentDateSEC));

                                ServerFileInfo.ModifiedDateTimeStamp = dDateTime.ToString();

                                ServerFileInfo.FileSize = FileNodesList[i].ChildNodes[6].InnerText;
                                ServerFileInfo.ForceUpdate = FileNodesList[i].ChildNodes[7].InnerText;

                                RetVal.Add(ServerFileInfo);
                            }
                            catch (Exception)
                            {
                                //If XMl text is unreadable, then display message on interface. But Do not abort the process.
                                //TODO: handling of displaying message.
                            }
                        }
                    }
                }

            }
            return RetVal;

        }

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- New / Dispose --"

        public DILiveUpdate(string ftpServerName, string ftpUserName, string ftpPassCode, string ftpDirectoryName, string applicationName, string applicationPath, string adaptation, string tempFolder)
        {
            this.FTPServerName = ftpServerName;
            this.FTPUserName = ftpUserName;
            this.FTPPassCode = ftpPassCode;
            this.FTPDirectoryName = ftpDirectoryName;
            this.ApplicationName = applicationName;
            this.AppInstallationPath = applicationPath;
            this.Adaptation = adaptation;
            this.TempFolder = tempFolder;
            this.LocalAdaptationFolder = Path.Combine(this.AppInstallationPath, this.Adaptation);

            //--Clear Temp Folder & BackupFolder
            this.ClearFolder(this.TempFolder);

            if (this.DIFTPClient != null)
            {   
                this.DIFTPClient.DownlaodStatusEvent += new FTPClient.DownlaodStatusDelegate(DIFTPClient_DownlaodStatusEvent);
                this.DIFTPClient.DownloadCancelled += new EventHandler(DIFTPClient_DownloadCancelled);
                this.DIFTPClient.FTPLogined += new EventHandler(DIFTPClient_Logined);
                this.DIFTPClient.FTPConnectionClosed += new EventHandler(DIFTPClient_FTPConnectionClosed);
                this.DIFTPClient.FTPLoginedFailed += new FTPClient.FTPLoginFailedDelegate(DIFTPClient_LoginedFailed);
            }
        }

        #endregion

        #region "-- Properties --"

        private Dictionary<ServerOfflineFileInfoPair, bool> _FTPAllAvailableFiles = new Dictionary<ServerOfflineFileInfoPair, bool>();
        /// <summary>
        /// Gets the collection of available FTP Files present in Update Server.
        /// Structure of the Collection:
        /// Key: ServerFileType (APPLICATION_FILE, DATABASE, MAP, PowerPoint, WORD)
        /// Value: Dictionary[ServerFileInfo, LocalFileInfo]
        /// </summary>
        public Dictionary<ServerOfflineFileInfoPair, bool> FTPAllAvailableFiles
        {
            get
            {
                return this._FTPAllAvailableFiles;
            }
        }

        #endregion

        #region "-- Method --"

        public List<DIFileInfo> GetUpdatesAvailable()
        {
            List<DIFileInfo> RetVal = null;

            string XmlAppDirInfoTempFilePath = string.Empty;        // DILU_AppDirInfo.xml 

            string FtpDirectoryPath = string.Empty;
            List<DIFileInfo> XmlFileList = null;
            this._FTPAllAvailableFiles = new Dictionary<ServerOfflineFileInfoPair, bool>();

            DILiveUpdate.LiveUpdateLog += Environment.NewLine + "Start Time - " + DateTime.Now.ToString() + Environment.NewLine;

            //-- Set path for DILU_AppDirInfo.xml in Temp location.
            XmlAppDirInfoTempFilePath = this.TempFolder + @"\" + this.ApplicationName + @"\" + this.DILU_AppDirInfoXmlFileName;
            FtpDirectoryPath = this.FTPDirectoryName + "/" + this.ApplicationName;

            // -- Create Temp folder
            if (Directory.Exists(Path.GetDirectoryName(XmlAppDirInfoTempFilePath)) == false)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(XmlAppDirInfoTempFilePath));
            }

            // --download DILU_AppDirInfo.xml from FTPserver specific to calling application (DI, DA, DX)
            if (this.DownloadAppDirInfo(this.DILU_AppDirInfoXmlFileName, FtpDirectoryPath, XmlAppDirInfoTempFilePath))
            {
                // --Read XMLNode corresponding to calling Application. 
                // e.g: <DI6> for DevInfo, <DA6> for DataAdmin application
                XmlFileList = this.GetFileListFromXML(XmlAppDirInfoTempFilePath);

                //-- Compare file and Add in list
                this.CompareAndGetFTPFilesListForUpdate(XmlFileList, this.AppInstallationPath, this.ApplicationName);

            }
            else
            {
                // TODO:
                // Display message "error in Download";
            }


            //-- Check if any adaptation is set.
            if (string.IsNullOrEmpty(this.Adaptation) == false)
            {
                //--Set Paths for Adaptation.
                XmlAppDirInfoTempFilePath = this.TempFolder + @"\" + this.Adaptation + @"\" + this.DILU_AppDirInfoXmlFileName;
                FtpDirectoryPath = this.FTPDirectoryName + "/" + this.Adaptation;

                // --Create Temp folder
                if (Directory.Exists(Path.GetDirectoryName(XmlAppDirInfoTempFilePath)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(XmlAppDirInfoTempFilePath));
                }
                //-- Now download DILU_AppDirInfo.xml for adaptation 
                if (this.DownloadAppDirInfo(this.DILU_AppDirInfoXmlFileName, FtpDirectoryPath, XmlAppDirInfoTempFilePath))
                {
                    // --Read XMLNode corresponding to calling Application's adaptation. 
                    XmlFileList = this.GetFileListFromXML(XmlAppDirInfoTempFilePath);

                    //-- Compare file and Add in list
                    this.CompareAndGetFTPFilesListForUpdate(XmlFileList, this.LocalAdaptationFolder, this.Adaptation);
                }
            }

            RetVal = this.FTPFilesForUpdate;

            GC.Collect();

            return RetVal;
        }

        public bool DownloadFile(DIFileInfo fileInfo, string updateFolderPath)
        {
            bool RetVal = false;

            string FileToDownload = string.Empty;
            string TempFileName = string.Empty;
            string FtpDirectory = string.Empty;
            string LocalFileName = string.Empty;

            try
            {               

                FileToDownload = Path.GetFileName(fileInfo.FileName) + ".zip";


                //-- If this file belongs to current Adaptation, then handle Adaptation Folder name in downloaded path.
                if (fileInfo.ApplicationName.ToLower() == this.Adaptation.ToLower())
                {
                    TempFileName = Path.Combine(Path.Combine(updateFolderPath, fileInfo.ApplicationName), Path.GetDirectoryName(fileInfo.FilePath)) + "\\" + Path.GetFileNameWithoutExtension(fileInfo.FileName) + ".zip";
                }
                else
                {
                    if (fileInfo.FileName.ToLower() == "diLiveUpdate.exe".ToLower())
                    {
                        //-- NOTE: Special handling for "diLiveUpdate.exe" . 
                        // as "diLiveUpdate.exe" needs to be udpated directly into root application Path folder 
                        // instead of Temp Liveupdate Folder.
                        TempFileName = Path.Combine(this.AppInstallationPath, Path.GetFileNameWithoutExtension(fileInfo.FileName) + ".zip");
                    }
                    else
                    {
                        //-- normal files.
                        TempFileName = Path.Combine(updateFolderPath, Path.GetDirectoryName(fileInfo.FilePath)) + "\\" + Path.GetFileNameWithoutExtension(fileInfo.FileName) + ".zip";
                    }
                }

                // -- Concatenate Server root FTP path with file's parent folder path. 
                FtpDirectory = this.FTPDirectoryName + "/" + fileInfo.ApplicationName + "/" + this.GetFTPFileReferencePath(fileInfo.FilePath);

                // --Download it from location specified in the same xml element 
                // --and save in temp folder
                RetVal = this.DownloadFileThroughFTP(FileToDownload, FtpDirectory, TempFileName);


                //-- Change ModifiedDateTime of downloaded file to server's file Modified datatime.
                //-- because when file are unzipped, then creation time becomes current system time
                string downloadedFile = Path.Combine(Path.GetDirectoryName(TempFileName), fileInfo.FileName);
                File.SetLastWriteTime(downloadedFile, Convert.ToDateTime(fileInfo.ModifiedDateTimeStamp));

                //-- Unzip "Zipdi" files as Special handling
                if (Path.GetExtension(fileInfo.FileName).ToLower() == ".zipdi")
                {
                    //-- unzip file again (because .zipdi files were zipped twice at server
                    FTPClient oftp = new FTPClient();
                    TempFileName = Path.Combine(Path.GetDirectoryName(TempFileName), fileInfo.FileName);
                    oftp.ExtractArchive(TempFileName, Path.GetDirectoryName(TempFileName), false);
                }
            }
            catch (Exception ex)
            {
                RetVal = false;
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
                //MessageBoxControl.ShowMsg(DIMessage.CONNECTION_FAILED, Msg_IconType.INFORMATION, Msg_ButtonType.OK, true);
            }

            return RetVal;
        }

        /// <summary>
        /// Downloads the specified file from FTP server . FTP directory must be specified.
        /// </summary>
        /// <param name="ftpFileName">ftp file name to download.</param>
        /// <param name="ftpDirectory">ftp directpry path.</param>
        /// <param name="localFileName">desired local filename for downloaded file.</param>
        /// <returns>true, if succesfully downloaded.</returns>
        public bool DownloadFileThroughFTP(string ftpFileName, string ftpDirectory, string localFileName)
        {
            bool RetVal = false;
            if (this.DIFTPClient == null)
            {
                this.DIFTPClient = new FTPClient();
                this.DIFTPClient.DownlaodStatusEvent += new FTPClient.DownlaodStatusDelegate(DIFTPClient_DownlaodStatusEvent);
                this.DIFTPClient.DownloadCancelled += new EventHandler(DIFTPClient_DownloadCancelled);
            }
            this.DIFTPClient.CancelDownloadAsync = false;

            //- Delete local file if exists
            try
            {
                if (Directory.Exists(Path.GetDirectoryName(localFileName)) == false)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(localFileName)); 
                }
                else if (File.Exists(localFileName))
                {
                    File.Delete(localFileName);
                }
            }
            catch
            {
            }

                //--Download File from the ftp location. 
                try
                {
                    this.DIFTPClient.RemoteHost = this.FTPServerName;   // RemoteHost Server
                    this.DIFTPClient.UserName = this.FTPUserName;
                    this.DIFTPClient.Password = this.FTPPassCode;

                    //Check the existence of port number.

                    this.DIFTPClient.RemotePort = Convert.ToInt32(DILiveUpdate.FTPPort);

                    this.DIFTPClient.RemotePath = ftpDirectory;

                    this.DIFTPClient.Download(ftpFileName, localFileName);
                    //this.DIFTPClient.Close();
                    RetVal = true;
                }
                catch (Exception ex)
                {
                    this.DIFTPClient.cleanup();
                    this.DIFTPClient = null;

                    this.DeleteFile(localFileName);
                    throw new ApplicationException(ex.Message.ToString());
                }
            
            return RetVal;
        }

        private bool DownloadAppDirInfo(string appDirInfoXmlFileName, string ftpDirectoryPath, string xmlAppDirInfoTempFilePath)
        {
            bool RetVal = false;
            int DownloadAttempt = 0;
            int MaxAttempt = 5;
            bool DownloadCompleted = false;

            //- Keep Trying download file till 5 Attempt
            while (DownloadCompleted == false && DownloadAttempt < MaxAttempt)
            {
                DownloadAttempt += 1;

                try
                {
                    RetVal = this.DownloadFileThroughFTP(appDirInfoXmlFileName, ftpDirectoryPath, xmlAppDirInfoTempFilePath);

                    DILiveUpdate.LiveUpdateLog += Environment.NewLine + appDirInfoXmlFileName + " - Attempt <" + DownloadAttempt + "> - Success." + Environment.NewLine;

                    DownloadCompleted = RetVal;
                }
                catch (Exception ex)
                {
                    DILiveUpdate.LiveUpdateLog += Environment.NewLine + appDirInfoXmlFileName + " - Attempt <" + DownloadAttempt + "> - Failed." + Environment.NewLine;
                    DILiveUpdate.LiveUpdateLog += "--ERROR--: " + ex.Message.ToString() + Environment.NewLine + Environment.NewLine;

                    //- Try Downloading File till 5 attempts.
                    System.Threading.Thread.Sleep(1000);
                    if (DownloadAttempt > MaxAttempt)
                    {
                        //- Stop More download Attempts
                        DownloadCompleted = true;
                    }
                }
            }

            return RetVal;
        }

        private void DIFTPClient_DownlaodStatusEvent(long DownloadedBytesSize)
        {
            if (this.DownlaodStatusEvent != null)
            {
                this.DownlaodStatusEvent(DownloadedBytesSize);
            }
        }

        private void DIFTPClient_DownloadCancelled(object sender, EventArgs e)
        {
            if (this.DownlaodCancelled != null)
            {
                this.DIFTPClient.Close();
               
                this.DownlaodCancelled(this, new EventArgs());
            }
        }

        private void DIFTPClient_Logined(object sender, EventArgs e)
        {
            //- Record LOG
            DILiveUpdate.LiveUpdateLog += Environment.NewLine + "FTP Connection - Success" + Environment.NewLine + Environment.NewLine;
        }

        private void DIFTPClient_LoginedFailed(string errorMessage)
        {
            //- Record LOG
            DILiveUpdate.LiveUpdateLog += "FTP Connection - Failed" + Environment.NewLine;
            DILiveUpdate.LiveUpdateLog += "-- ERROR --" + Convert.ToString(errorMessage) + Environment.NewLine;
        }

        private void DIFTPClient_FTPConnectionClosed(object sender, EventArgs e)
        {
            //- Record LOG
            DILiveUpdate.LiveUpdateLog += "Closing FTP Connection - Success" + Environment.NewLine;
        }

        public void AbortDownload()
        {
            this.DIFTPClient.CancelDownloadAsync = true;
        }

        public void CloseFTP()
        {
            try
            {
                if (this.DIFTPClient != null)
                {
                    this.DIFTPClient.Close();
                    this.DIFTPClient = null;
                }
            }
            catch (Exception  ex)
            {
                DILiveUpdate.LiveUpdateLog += "Closing FTP Connection - Failed" + Environment.NewLine;
                DILiveUpdate.LiveUpdateLog += "--ERROR -- " + ex.Message.ToString() + Environment.NewLine;
            }
        }

        public void DeleteFile(string filepath)
        {
            try
            {
                File.Delete(filepath);
            }
            catch 
            {}
        }

        public static DIFTPInfo GetDIFTPInfo(string diFTPCode)
        {
            DIFTPInfo RetVal = null;
            
            try
            {
                DIMapServerWS.Utility WorldWideUtility = new DIMapServerWS.Utility();
                
                DevInfo.Lib.DI_LibBAL.DIMapServerWS.FTPReturnObject oDIFTPInfo = ((DevInfo.Lib.DI_LibBAL.DIMapServerWS.FTPReturnObject)(WorldWideUtility.GetDIFTPInfo(diFTPCode)));
                
                RetVal = new DIFTPInfo();
                RetVal.FTPHost = oDIFTPInfo.FTPHost;
                RetVal.FTPUserName = oDIFTPInfo.FTPUserName;
                RetVal.FTPPassword = oDIFTPInfo.FTPPassword;
                RetVal.FTPDirectory = oDIFTPInfo.FTPDirectory;
            }
            catch (Exception e)
            {
                
            }

            return RetVal;
        }

        #endregion

        #region "-- Events --"

        public event DevInfo.Lib.DI_LibBAL.Utility.FTPClient.DownlaodStatusDelegate DownlaodStatusEvent;
        public event EventHandler DownlaodCancelled;

        #endregion

        #endregion

        /// <summary>
        /// Static Class contaning distinct FileTypes constants used in Live Update.
        /// </summary>
        public static class FileType
        {
            public const string APPLICATION_FILE = "Application File";
            public const string DATABASE = "Database";
            public const string DOCUMENT = "Document";
            public const string LANGUAGE = "Language";
            public const string DA_EMERGENCYINFO = "EmergencyInfo";
            public const string DA_EXCHANGES = "Exchange";
            public const string DA_STANDARDS = "Standards";

        }
    }

    public class ServerOfflineFileInfoPair
    {
        public ServerOfflineFileInfoPair(DIFileInfo serverFileInfo, DIFileInfo offlineFileInfo)
        {
            this._ServerFileInfo = serverFileInfo;

            this._OfflineFileInfo = offlineFileInfo;
        }

        private DIFileInfo _ServerFileInfo;
        /// <summary>
        /// Gets or sets the Server FileInfo
        /// </summary>
        public DIFileInfo ServerFileInfo
        {
            get { return _ServerFileInfo; }
            set { _ServerFileInfo = value; }
        }


        private DIFileInfo _OfflineFileInfo;
        /// <summary>
        ///  Gets or sets the Offline FileInfo
        /// </summary>
        public DIFileInfo OfflineFileInfo
        {
            get { return _OfflineFileInfo; }
            set { _OfflineFileInfo = value; }
        }   

    }

    /// <summary>
    /// Structure used to store DI specific file's information (FileName, FilePath, Version, FileType)
    /// </summary>
    public struct DIFileInfo
    {

        public string FileName;
        public string FilePath;             // file path relative to Application's directory. (..\Data\xys.mdb)
        public string Version;
        public string FileType;
        public string ForceUpdate;
        public string ModifiedDateTimeStamp;
        public string FileSize;
        public string ApplicationPath;      // Application installation path . E.g: C:\DevInfo
        public string ApplicationName;      // Name of application to which this file belongs  (DI6, DA6)

        //public bool Adaptation;              // Adaptation Name if file belongs to adaptation folder.
    }


    public class DIFTPInfo
    {
        public string FTPHost = string.Empty;
        public string FTPUserName = string.Empty;
        public string FTPPassword = string.Empty;
        public string FTPDirectory = string.Empty;
    }
}
