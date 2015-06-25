/************************************************/
/************     Library          ****************/
/************************************************/



// Byteforge FTP Library
// Copyright (C) 2003  Nick Ruisi
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Sockets;
//using ICSharpCode.SharpZipLib.Zip;

namespace DevInfo.Lib.DI_LibBAL.Utility
{
    /// <summary>
    /// A FTP Client Library
    /// </summary>
    public class FTPClient
    {
        #region " -- Private -- "

        #region " -- Variables -- "

        private string
            remoteHost, remotePath, mes, remoteUser, remotePass;
        private int remotePort, bytes;
        private Socket clientSocket;

        private int retValue;
        private Boolean debug;
        private Boolean logined;
        private string reply;
        private string Progress_caption = "";

        private static int BLOCK_SIZE = 512;

        Byte[] buffer = new Byte[BLOCK_SIZE];
        Encoding ASCII = Encoding.ASCII;

        #endregion

        #region " -- Methods -- "

        private Socket createDataSocket()
        {
            int AttempsCounts = 0;      //-- no. of attempts to create a socket when socket failed to be create.
            bool socketSuccess = false;

            sendCommand("PASV");

            if (retValue != 227)
            {
                throw new IOException(reply.Substring(4));
            }

            int index1 = reply.IndexOf('(');
            int index2 = reply.IndexOf(')');
            string ipData =
                reply.Substring(index1 + 1, index2 - index1 - 1);
            int[] parts = new int[6];

            int len = ipData.Length;
            int partCount = 0;
            string buf = "";

            for (int i = 0; i < len && partCount <= 6; i++)
            {

                char ch = Char.Parse(ipData.Substring(i, 1));
                if (Char.IsDigit(ch))
                    buf += ch;
                else if (ch != ',')
                {
                    throw new IOException("Malformed PASV reply: " +
                        reply);
                }

                if (ch == ',' || i + 1 == len)
                {

                    try
                    {
                        parts[partCount++] = Int32.Parse(buf);
                        buf = "";
                    }
                    catch (Exception)
                    {
                        throw new IOException("Malformed PASV reply: " +
                            reply);
                    }
                }
            }

            string ipAddress = parts[0] + "." + parts[1] + "." +
                parts[2] + "." + parts[3];

            int port = (parts[4] << 8) + parts[5];

            Socket s = new
                Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new
                IPEndPoint(Dns.Resolve(ipAddress).AddressList[0], port);

            while (socketSuccess == false)
            {
                //-- try to create socket within 3 attempts. 
                // (sometimes sockets does not create because of network failure)
                try
                {
                    s.ReceiveTimeout = this._SocketReceiveTimeout;
                    s.Connect(ep);                    
                    socketSuccess = true;
                }
                catch
                {
                    AttempsCounts += 1;

                    if (AttempsCounts >= 3)
                    {
                        throw new IOException("Can't connect to remote server");
                    }
                }
            }            
            return s;
        }

        public void cleanup()
        {
            if (clientSocket != null)
            {
                clientSocket.Close();
                clientSocket = null;
            }
            logined = false;

            if (this.FTPConnectionClosed != null)
            {
                this.FTPConnectionClosed(this, new EventArgs());
            }
        }

        private string readLine()
        {

            

            while (true)
            {
                bytes = clientSocket.Receive(buffer, buffer.Length, 0);
                mes += ASCII.GetString(buffer, 0, bytes);
                if (bytes < buffer.Length)
                {
                    break;
                }
            }

            char[] seperator = { '\n' };
            string[] mess = mes.Split(seperator);

            if (mes.Length > 2)
            {
                mes = mess[mess.Length - 2];
            }
            else
            {
                mes = mess[0];
            }

            if (!mes.Substring(3, 1).Equals(" "))
            {
                return readLine();
            }

            //if(debug)
            {
                for (int k = 0; k < mess.Length - 1; k++)
                {
                    //Console.WriteLine(mess[k]);
                }
            }
            return mes;
        }


        #endregion

        #region " -- Raise Event -- "

        private void RaiseUploadStatusEvent(long fileSize, long uploadSize)
        {
            if (this.UploadStatusEvent != null)
            {
                this.UploadStatusEvent(fileSize, uploadSize);
            }
        }

        #endregion

        #endregion

        #region " -- Public -- "

        #region " -- Delegates -- "

        public delegate void ProcessCompletedHandler();

        /// <summary>
        /// Delegate to show the upload status.
        /// </summary>
        /// <param name="FileSize">FileSize</param>
        /// <param name="UploadSize">UploadSize</param>
        public delegate void UploadStatusDelegate(long FileSize, long UploadSize);

        public delegate void DownlaodStatusDelegate(long DownloadedBytesSize);

        public delegate void FTPLoginFailedDelegate(string errorMessage);
        
        #endregion

        #region " -- Events -- "

        public event ProcessCompletedHandler ProcessComplete;

        /// <summary>
        /// Event to show the upload status.
        /// </summary>
        public event UploadStatusDelegate UploadStatusEvent;

        public event DownlaodStatusDelegate DownlaodStatusEvent;

        public event EventHandler DownloadCancelled;        //-- Event for canelling downloading
        public event EventHandler FTPLogined;        //-- Event for FTP Login
        public event FTPLoginFailedDelegate FTPLoginedFailed;        //-- Event for FTP Login Process Failed
        public event EventHandler FTPConnectionClosed;        //-- Event for FTP Connection Closed

        #endregion

        #region " -- New / Dispose -- "

        public FTPClient()
        {
            remoteHost = "localhost";
            remotePath = ".";
            remoteUser = "anonymous";
            remotePass = "anonymous@localhost.localdomain";
            remotePort = 21;
            debug = false;
            logined = false;
        }

        #endregion

        #region " -- Properties -- "

        /// <summary>
        ///  Get the user's login status
        /// </summary>        
        public Boolean isLoggedin
        {
            get
            {
                return this.logined;
            }
        }

        /// <summary>
        /// Get or sets the remote host name 
        /// </summary>
        public string Progress_Caption
        {
            set
            {
                this.Progress_caption = value;
            }
        }

        /// <summary>
        /// Get or sets the remote host name
        /// </summary>
        public string RemoteHost
        {
            get
            {
                return remoteHost;
            }
            set
            {
                remoteHost = value;
            }
        }

        /// <summary>
        /// Get or set the remote port 
        /// </summary>
        public int RemotePort
        {
            get
            {
                return remotePort;
            }
            set
            {
                remotePort = value;
            }
        }

        /// <summary>
        /// Get or set the remote path 
        /// </summary>
        public string RemotePath
        {
            get
            {
                return remotePath;
            }
            set
            {
                if (value.StartsWith("/") == false)
                {
                    value = "/" + value;
                }
                if (value.EndsWith("/") == false)
                {
                    value += "/";
                }

                // if New FTP remote Path location is different from Current one ,then
                if (string.IsNullOrEmpty(this.remotePath) || this.remotePath != value)
                {
                    //- Change FTP Directory to new Directory
                    this.Chdir(value);                    
                }

                remotePath = value;
            }
        }

        /// <summary>
        /// Sets the username to log into the remote server with
        /// </summary>

        public string UserName
        {
            set
            {
                remoteUser = value;
            }
        }

        /// <summary>
        /// Sets the password for logging into the remote server
        /// </summary>
        public string Password
        {
            set
            {
                remotePass = value;
            }
        }

        /// <summary>
        /// Set debug mode. In debug mode, the entire FTP 
        /// session will be written to standard output
        /// </summary>
        public bool Debug
        {
            set
            {
                debug = value;
            }
            get
            {
                return debug;
            }
        }

        private bool _CancelDownloadAsync = false;
        /// <summary>
        /// Gets or sets the value indicating whether cancel downloading.
        /// </summary>
        public bool CancelDownloadAsync
        {
            get { return _CancelDownloadAsync; }
            set { _CancelDownloadAsync = value; }
        }

        private int _SocketReceiveTimeout = -1;
        /// <summary>
        /// Get or set the SocketReceiveTimeout
        /// </summary>
        public int SocketReceiveTimeout
        {
            get { return _SocketReceiveTimeout; }
            set { _SocketReceiveTimeout = value; }
        }
	

        #endregion

        #region " -- Methods -- "

        #region " -- Upload -- "

        /// <summary>
        /// Upload a file.
        /// </summary>
        /// <param name="fileName">Local file name</param>
        public void Upload(string fileName)
        {
            Upload(fileName, false);
        }

        /// <summary>
        /// Upload a file and set the resume flag.
        /// </summary>
        /// <param name="fileName">Local file name</param>
        /// <param name="resume">Resume flag</param>
        public void Upload(string fileName, Boolean resume)
        {

            if (!logined)
            {
                Login();
            }

            Socket cSocket = createDataSocket();
            long offset = 0;

            if (resume)
            {

                try
                {

                    SetBinaryMode(true);
                    offset = GetFileSize(fileName);

                }
                catch (Exception)
                {
                    offset = 0;
                }
            }

            if (offset > 0)
            {
                sendCommand("REST " + offset);
                if (retValue != 350)
                {
                    //throw new IOException(reply.Substring(4));
                    //Remote server may not support resuming.
                    offset = 0;
                }
            }

            sendCommand("STOR " + Path.GetFileName(fileName));

            if (!(retValue == 125 || retValue == 150))
            {
                throw new IOException(reply.Substring(4));
            }

            // open input stream to read source file
            FileStream input = new
                FileStream(fileName, FileMode.Open);

            if (offset != 0)
            {

                //if(debug)
                {
                    //Console.WriteLine("seeking to " + offset);
                }
                input.Seek(offset, SeekOrigin.Begin);
            }

            long UploadProgress = 0;

            while ((bytes = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                UploadProgress += Convert.ToInt64(bytes);
                cSocket.Send(buffer, bytes, 0);

                // -- raise the event to show the upload progress
                this.RaiseUploadStatusEvent(offset, UploadProgress);
            }
            input.Close();

            //Console.WriteLine("");

            if (cSocket.Connected)
            {
                cSocket.Close();
            }

            readReply();
            if (!(retValue == 226 || retValue == 250))
            {
                throw new IOException(reply.Substring(4));
            }
        }

        #endregion

        #region " -- Download -- "

        /// <summary>
        /// Download a file to the Assembly's local directory, keeping the same file name.
        /// </summary>
        /// <param name="remFileName">Remote file name</param>
        public void Download(string remFileName)
        {
            Download(remFileName, "", false);
        }


        /// <summary>
        /// Download a remote file to the Assembly's local directory, keeping the same file name, and set the resume flag.
        /// </summary>
        /// <param name="remFileName">Remote file name</param>
        /// <param name="resume">Resume</param>
        public void Download(string remFileName, Boolean resume)
        {
            Download(remFileName, "", resume);
        }

        /// <summary>
        /// Download a remote file to a local file name which can include
        /// a path. The local file name will be created or overwritten,
        /// but the path must exist.
        /// </summary>
        /// <param name="remFileName">Remote file name</param>
        /// <param name="locFileName">Local file name</param>
        public void Download(string remFileName, string locFileName)
        {
            Download(remFileName, locFileName, false);
        }

        /// <summary>
        /// Download a remote file to a local file name which can include
        /// a path, and set the resume flag. The local file name will be
        /// created or overwritten, but the path must exist.
        /// </summary>
        /// <param name="remFileName">Remote file name</param>
        /// <param name="locFileName">Local file name</param>
        /// <param name="resume">Resume flag</param>
        public void Download(string remFileName, string locFileName, Boolean resume)
        {
            bool isFileDownloaded = false;

            if (!logined)
            {
                Login();
            }

            SetBinaryMode(true);

            if (locFileName.Equals(""))
            {
                locFileName = remFileName;
            }

            if (File.Exists(locFileName))
            {
                File.Delete(locFileName);
            }

            if (!File.Exists(locFileName))
            {
                Stream st = File.Create(locFileName);
                st.Close();
            }

            FileStream output = new
                FileStream(locFileName, FileMode.Open);

            Socket cSocket = createDataSocket();

            long offset = 0;

            if (resume)
            {

                offset = output.Length;

                if (offset > 0)
                {
                    sendCommand("REST " + offset);
                    if (retValue != 350)
                    {
                        //throw new IOException(reply.Substring(4));
                        //Some servers may not support resuming.
                        offset = 0;
                    }
                }

                if (offset > 0)
                {
                    //if(debug)
                    {
                        //Console.WriteLine("seeking to " + offset);
                    }
                    long npos = output.Seek(offset, SeekOrigin.Begin);
                    //Console.WriteLine("new pos="+npos);
                }
            }

            sendCommand("RETR " + remFileName);

            if (!(retValue == 150 || retValue == 125))
            {
                throw new IOException(reply.Substring(4));
            }

            try
            {
                while (true)
                {                    
                    bytes = cSocket.Receive(buffer, buffer.Length, 0);
                    output.Write(buffer, 0, bytes);

                    if (bytes <= 0 || this._CancelDownloadAsync)
                    {
                        break;
                    }
                    else
                    {
                        //-- raise event of bytes downloaded.
                        if (this.DownlaodStatusEvent != null)
                        {
                            this.DownlaodStatusEvent(bytes);
                        }
                    }
                }
            }
            catch
            {
                if (output != null)
                {
                    output.Close();
                }
                if (cSocket.Connected)
                {
                    cSocket.Close();
                    DILiveUpdate.LiveUpdateLog += Environment.NewLine + "FILE: " + locFileName + " Socket Closed In Try Catch" + Environment.NewLine ;
                }
                throw;
            }

            DILiveUpdate.LiveUpdateLog += "FILE: " + locFileName + " Downloaded." + Environment.NewLine ;

            output.Close();
            
            if (cSocket.Connected)
            {
                cSocket.Close();
                DILiveUpdate.LiveUpdateLog += "FILE: " + locFileName + " Socket Closed." + Environment.NewLine ;
            }

            //- get File's Size Informations
            FileInfo fileInfo = new FileInfo(locFileName);
            long FileLength = fileInfo.Length;

            if (Path.GetExtension(locFileName) == ".zip" && this._CancelDownloadAsync==false)
            {
                DILiveUpdate.LiveUpdateLog += "FILE: " + locFileName + " Unzipping Started." + Environment.NewLine;

                Unzip(locFileName);
                
                DILiveUpdate.LiveUpdateLog += "FILE: " + locFileName + " Unizpped END." + Environment.NewLine ;

                isFileDownloaded = true;
            }

            //- Below Code " readReply(); " is Temperarily Commented 
            // because of occurence of hanging problem while downloading bigger files like .mdb
            if (!(Path.GetExtension(locFileName) == ".zip" && isFileDownloaded) || FileLength <= (8 * 1024 * 1024))
            {
                readReply();
            }
            else
            {
                //-Check if filesize > 8 MB
                // then Close FTP Session
                try
                {        
                    this.cleanup();
                }
                catch
                {                   
                }                
            }
            
            //Download process Cancelled
            if (this._CancelDownloadAsync)
            {
                // Throw Event - 
                if (this.DownloadCancelled != null)
                {
                    this.DownloadCancelled(this, new EventArgs());
                }
            }
            else 
            {
                if (!(Path.GetExtension(locFileName) == ".zip" && isFileDownloaded) || FileLength <= (8 * 1024 * 1024))
                {
                    if (!(retValue == 226 || retValue == 250))
                    {
                        throw new IOException(reply.Substring(4));
                    }
                }

                //frmPrg.Close();
                if (this.ProcessComplete != null)
                {
                    ProcessComplete();
                }
            }

        }

        #endregion

        /// <summary>
        /// Return a string array containing the remote directory's file list.
        /// </summary>
        /// <param name="mask">Search Pattern</param>
        /// <param name="longDirListing">Sends LIST insted of NLST</param>
        /// <returns>String array of files</returns>
        public string[] GetFileList(string mask, bool longDirListing)
        {
            if (!logined)
            {
                Login();
            }

            Socket cSocket = createDataSocket();

            if (!longDirListing)
                sendCommand("NLST " + mask);
            else
                sendCommand("LIST " + mask);

            if (!(retValue == 150 || retValue == 125))
            {
                throw new IOException(reply.Substring(4));
            }

            mes = "";

            while (true)
            {

                int bytes = cSocket.Receive(buffer, buffer.Length, 0);
                mes += ASCII.GetString(buffer, 0, bytes);

                if (bytes < buffer.Length)
                {
                    break;
                }
            }

            mes = mes.Replace("\r\n", "\n");
            char[] seperator = { '\n' };
            string[] mess = mes.Split(seperator);

            cSocket.Close();

            readReply();

            if (retValue != 226)
            {
                throw new IOException(reply.Substring(4));
            }
            return mess;

        }

        /// <summary>
        /// Return the size of a file.
        /// </summary>
        /// <param name="fileName">Name of remote file</param>
        /// <returns>Size of file in bytes</returns>
        public long GetFileSize(string fileName)
        {

            if (!logined)
            {
                Login();
            }

            sendCommand("SIZE " + fileName);
            long size = 0;

            if (retValue == 213)
            {
                size = Int64.Parse(reply.Substring(4));
            }
            else
            {
                throw new IOException(reply.Substring(4));
            }

            return size;

        }

        /// <summary>
        /// Login to the remote server.
        /// </summary>
        public void Login()
        {
            logined = false;
            clientSocket = new
                Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ep = new
                IPEndPoint(Dns.Resolve(remoteHost).AddressList[0], remotePort);

            try
            {
                clientSocket.ReceiveTimeout = this._SocketReceiveTimeout;
                clientSocket.Connect(ep);
            }
            catch (Exception)
            {
                if (this.FTPLoginedFailed != null)
                {
                    this.FTPLoginedFailed("Couldn't connect to remote server");
                }
                throw new IOException("Couldn't connect to remote server");
            }

            readReply();
            if (retValue != 220)
            {
                if (this.FTPLoginedFailed != null)
                {
                    this.FTPLoginedFailed("Login Failure - " + reply);
                }
                Close();
                throw new IOException(reply.Substring(4));
            }
            //if(debug)
            //Console.WriteLine("USER "+remoteUser);

            sendCommand("USER " + remoteUser);

            if (!(retValue == 331 || retValue == 230))
            {
                if (this.FTPLoginedFailed != null)
                {
                    this.FTPLoginedFailed("Login Failure - SendCommand('USER')");
                }
                cleanup();
                throw new IOException(reply.Substring(4));
            }

            if (retValue != 230)
            {
                //if(debug)
                //Console.WriteLine("PASS xxx");

                sendCommand("PASS " + remotePass);
                if (!(retValue == 230 || retValue == 202))
                {
                    if (this.FTPLoginedFailed != null)
                    {
                        this.FTPLoginedFailed("Login Failure - SendCommand('PASS')");
                    }

                    cleanup();
                    throw new IOException(reply.Substring(4));
                }
            }

            logined = true;

            if (this.FTPLogined != null)
            {
                this.FTPLogined(this, new EventArgs());
            }
            //Console.WriteLine("Connected to "+remoteHost);

            Chdir(remotePath);

        }

        /// <summary>
        /// Sets transfer type 
        /// </summary>
        /// <param name="mode">True for binary transfres, false otherwise</param>
        public void SetBinaryMode(Boolean mode)
        {

            if (mode)
            {
                sendCommand("TYPE I");
            }
            else
            {
                sendCommand("TYPE A");
            }
            if (retValue != 200)
            {
                throw new IOException(reply.Substring(4));
            }
        }


        public void Unzip(string locFileName)
        {
            ExtractArchive(locFileName, Path.GetDirectoryName(locFileName));
        }


        /// <summary>
        /// Delete a file from the remote FTP server.
        /// </summary>
        /// <param name="fileName">Remote file to delete</param>
        public void DeleteRemoteFile(string fileName)
        {

            if (!logined)
            {
                Login();
            }

            sendCommand("DELE " + fileName);

            if (retValue != 250)
            {
                throw new IOException(reply.Substring(4));
            }

        }

        /// <summary>
        /// Rename a file on the remote FTP server.
        /// </summary>
        /// <param name="oldFileName">Old file name</param>
        /// <param name="newFileName">New file name</param>
        public void RenameRemoteFile(string oldFileName, string
            newFileName)
        {

            if (!logined)
            {
                Login();
            }

            sendCommand("RNFR " + oldFileName);

            if (retValue != 350)
            {
                throw new IOException(reply.Substring(4));
            }

            //  known problem
            //  rnto will not take care of existing file.
            //  i.e. It will overwrite if newFileName exist
            sendCommand("RNTO " + newFileName);
            if (retValue != 250)
            {
                throw new IOException(reply.Substring(4));
            }

        }

        /// <summary>
        /// Create a directory on the remote FTP server.
        /// </summary>
        /// <param name="dirName">Directory name</param>
        public void Mkdir(string dirName)
        {

            if (!logined)
            {
                Login();
            }

            sendCommand("MKD " + dirName);

            if (retValue != 250)
            {
                throw new IOException(reply.Substring(4));
            }

        }

        /// <summary>
        /// Delete a directory on the remote FTP server.
        /// </summary>
        /// <param name="dirName">Directory name</param>
        public void Rmdir(string dirName)
        {

            if (!logined)
            {
                Login();
            }

            sendCommand("RMD " + dirName);

            if (retValue != 250)
            {
                throw new IOException(reply.Substring(4));
            }

        }

        /// <summary>
        /// Change the current working directory on the remote FTP server.
        /// </summary>
        /// <param name="dirName">Directory to change to</param>
        public void Chdir(string dirName)
        {

            if (dirName.Equals("."))
            {
                return;
            }

            if (!logined)
            {
                Login();
            }

            sendCommand("CWD " + dirName);

            if (retValue != 250)
            {
                throw new IOException(reply.Substring(4));
            }

            this.remotePath = dirName;

            //Console.WriteLine("Current directory is "+remotePath);

        }

        /// <summary>
        /// Close the FTP connection.
        /// </summary>
        public void Close()
        {

            if (clientSocket != null)
            {
                sendCommand("QUIT");
            }

            cleanup();
            //Console.WriteLine("Closing...");
        }

        /// <summary>
        /// Causes the client to read from the server.
        ///  This mehod is public to allow for derived classes.
        /// </summary>
        public void readReply()
        {
            mes = "";
            reply = readLine();
            retValue = Int32.Parse(reply.Substring(0, 3));
        }

        /// <summary>
        /// Sends a command to the FTP server. The commands sent to the server with this \
        /// method must be protocol-level commands (i.e. NLST,LIST,CWD), not FTP appplication commands.
        /// This method is public so that FTPClient can be derived from.
        /// </summary>
        /// <param name="command"></param>
        public void sendCommand(String command)
        {

            Byte[] cmdBytes =
                Encoding.ASCII.GetBytes((command + "\r\n").ToCharArray());
            clientSocket.Send(cmdBytes, cmdBytes.Length, 0);
            readReply();
        }

        /// <summary>
        /// The raw response from the server fronm the last command issued
        /// </summary>
        public string ServerResponse
        {
            get { return reply; }
        }

        /// <summary>
        /// The response code from the server in response to the last command issued 
        /// </summary>
        public int ResponseCode
        {
            get { return retValue; }
        }

        /// <summary>
        /// Extractor function 
        /// </summary>
        /// <param name="zipFilename">zipFilename</param>
        /// <param name="ExtractDir">ExtractDir</param>
        /// <param name="deleteZippedfile">if true, zipped file will be deleted after extraction.</param>
        public void ExtractArchive(string zipFilename, string ExtractDir)
        {

            this.ExtractArchive(zipFilename, ExtractDir, true);   //-- pass true. Delete zipped file also by Default..
        }

        /// <summary>
        /// Extractor function 
        /// </summary>
        /// <param name="zipFilename">zipFilename</param>
        /// <param name="ExtractDir">ExtractDir</param>
        /// <param name="deleteZippedfile">if true, zipped file will be deleted after extraction.</param>
        public void ExtractArchive(string zipFilename, string ExtractDir, bool deleteZippedfile)
        {
            //			int Redo = 1;
            ICSharpCode.SharpZipLib.Zip.ZipInputStream MyZipInputStream;
            FileStream MyFileStream;

            MyZipInputStream = new ICSharpCode.SharpZipLib.Zip.ZipInputStream(new FileStream(zipFilename, FileMode.Open, FileAccess.Read));
            ICSharpCode.SharpZipLib.Zip.ZipEntry MyZipEntry = MyZipInputStream.GetNextEntry();
            Directory.CreateDirectory(ExtractDir);

            while (!(MyZipEntry == null))
            {
                if (MyZipEntry.IsDirectory)
                {
                    Directory.CreateDirectory(ExtractDir + "\\" + MyZipEntry.Name);
                }
                else
                {
                    if (!Directory.Exists(ExtractDir + "\\" + Path.GetDirectoryName(MyZipEntry.Name)))
                    {
                        Directory.CreateDirectory(ExtractDir + "\\" + Path.GetDirectoryName(MyZipEntry.Name));
                    }

                    MyFileStream = new FileStream(ExtractDir + "\\" + MyZipEntry.Name, FileMode.OpenOrCreate, FileAccess.Write);
                    int count;
                    byte[] buffer = new byte[4096];
                    count = MyZipInputStream.Read(buffer, 0, 4096);
                    while (count > 0)
                    {
                        MyFileStream.Write(buffer, 0, count);
                        count = MyZipInputStream.Read(buffer, 0, 4096);
                    }
                    MyFileStream.Close();
                }

                try
                {
                    MyZipEntry = MyZipInputStream.GetNextEntry();
                }
                catch (Exception ex)
                {
                    MyZipEntry = null;
                }
            }

            //dispose active objects
            try
            {
                if (!(MyZipInputStream == null))
                    MyZipInputStream.Close();
            }
            catch (Exception ex)
            { }
            finally
            {
                if (deleteZippedfile)
                {
                    //delete the zip file
                    if (System.IO.File.Exists(zipFilename))
                        System.IO.File.Delete(zipFilename);
                }
            }

        }

        #endregion

        #endregion

    }
}
