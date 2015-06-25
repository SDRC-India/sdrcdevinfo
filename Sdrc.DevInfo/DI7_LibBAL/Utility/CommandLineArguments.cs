using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Utility
{
    /// <summary>
    /// This is class is used to Convert various arguments stores in properties into single command line arguments. 
    ///<para>'Adaptation
    ///    'ApplicationName
    ///    'AppInstallationPath
    ///    'FTPServerName
    ///    'FTPDirectoryName
    ///    'FTPUserName
    ///    'FTPPassCode
    ///    </para>
    /// </summary>
    /// <remarks>This class will be used by DevInfo desktop application (UI, DA, DX) to send information to DI Live Update Client program.</remarks>
    public class CommandLineArguments
    {

        #region "-- Public --"

        #region "-- New / Dispose --"
        /// <summary>
        /// Constructor to get individual argument values and set them into proeperties.
        /// </summary>
        /// <param name="adaptation">adaptation string . For eg: C:\Devinfo\DevInfo 6.0</param>
        /// <param name="applicationName">Calling application name like (UI_UserInterface, DXA)</param>
        /// <param name="appInstallationPath">Calling application installation path.</param>
        /// <param name="ftpHostName">FTP host name or IP address.</param>
        /// <param name="ftpUserName">FTP Username</param>
        /// <param name="ftpPassCode">FTP password</param>
        /// <param name="ftpDirectory">FTP Directory name to login.</param>
        public CommandLineArguments(string adaptation, string applicationName, string appInstallationPath, string ftpHostName, string ftpUserName, string ftpPassCode, string ftpDirectory)
        {
            // /a Adaptation  /App_Name ApplicationName /App_Path AppInstallationPath  /ftp_h FTPServerName /ftp_dir FTPDirectoryName /ftp_un FTPUserName /ftp_pwd FTPPassCode  ;

            //- Assign values to properties.
            if (!(string.IsNullOrEmpty(adaptation)))
            {
                this._Adaptation = adaptation;
            }

            if (!(string.IsNullOrEmpty(applicationName)))
            {
                this._ApplicationName = applicationName;
            }

            if (!(string.IsNullOrEmpty(appInstallationPath)))
            {
                this._AppInstallationPath = appInstallationPath;
            }

            if (!(string.IsNullOrEmpty(ftpHostName)))
            {
                this._FTPHostName = ftpHostName;
            }

            if (!(string.IsNullOrEmpty(ftpDirectory)))
            {
                this._FTPDirectoryName = ftpDirectory;
            }

            if (!(string.IsNullOrEmpty(ftpUserName)))
            {
                this._FTPUserName = ftpUserName;
            }

            if (!(string.IsNullOrEmpty(ftpPassCode)))
            {
                this._FTPPassCode = ftpPassCode;
            }
        }

        /// <summary>
        /// Constructor used when complete command Line arguement is to be parsed and to get individual values.
        /// </summary>
        /// <param name="commandLineArgument">Complete Command line argument string.</param>
        public CommandLineArguments(string commandLineArgument)
        {
            if (string.IsNullOrEmpty(commandLineArgument) == false)
            {
                this.SetArguments(commandLineArgument);
            }
        }

        #endregion

        #region "-- Properties --"


        private string _FTPHostName;
        /// <summary>
        /// Gets or sets FTP host name. (Can be an IP address)
        /// </summary>
        public string FTPHostName
        {
            get { return _FTPHostName; }
            set { _FTPHostName = value; }
        }

        private string _FTPUserName;
        /// <summary>
        /// Gets or sets FTP userName.
        /// </summary>
        public string FTPUserName
        {
            get { return _FTPUserName; }
            set { _FTPUserName = value; }
        }

        private string _FTPPassCode;
        /// <summary>
        /// Gets or sets FTP login password.
        /// </summary>
        public string FTPPassCode
        {
            get { return _FTPPassCode; }
            set { _FTPPassCode = value; }
        }

        private string _FTPDirectoryName;
        /// <summary>
        /// Gets or sets FTP Directory path..
        /// </summary>
        public string FTPDirectoryName
        {
            get { return _FTPDirectoryName; }
            set { _FTPDirectoryName = value; }
        }

        private string _AppInstallationPath;
        /// <summary>
        /// Gets or sets application Installation path .(StartUp path)
        /// </summary>
        public string AppInstallationPath
        {
            get { return _AppInstallationPath; }
            set { _AppInstallationPath = value; }
        }

        private string _ApplicationName;
        /// <summary>
        /// Gets or sets application Name.
        /// </summary>
        public string ApplicationName
        {
            get { return _ApplicationName; }
            set { _ApplicationName = value; }
        }

        private string _Adaptation;
        /// <summary>
        /// Gets or sets Adaptation Name.
        /// </summary>
        public string Adaptation
        {
            get { return _Adaptation; }
            set { _Adaptation = value; }
        }

        #endregion

        #region "-- Methods --"
        /// <summary>
        /// Gets the command line argument string  based on properties assigned.
        /// Format of string is:  /a Adaptation  /App_Type ApplicationName /App AppInstallationPath /ftp_h FTPServerName /ftp_dir FTPDirectoryName /ftp_un FTPUserName /ftp_pwd FTPPassCode
        /// </summary>
        /// <returns>Command line argument string.</returns>
        public string GetCommandLineArgument()
        {
            string Retval = string.Empty;

            // Few arguemnts are mandatory. 
            if (!(string.IsNullOrEmpty(this._FTPHostName)) && !(string.IsNullOrEmpty(this._FTPUserName)) && !(string.IsNullOrEmpty(this._ApplicationName)) && !(string.IsNullOrEmpty(this._FTPDirectoryName)) && !(string.IsNullOrEmpty(this._AppInstallationPath)))
            {

                // -- Adaptation (can be blank)
                Retval = SW_ADAPTATION + " ";
                if (string.IsNullOrEmpty(this._Adaptation) == false)
                {
                    Retval += this._Adaptation;
                }
 
               // Application name (for eg: DI6_App, DA6)
                if (string.IsNullOrEmpty(this._ApplicationName) == false)
                {
                    Retval += SW_APPNAME + " " + this._ApplicationName + " ";
                }

               //-- Application Installation path. For eg: C:\DevInfo
                if (string.IsNullOrEmpty(this._AppInstallationPath) == false)
                {
                    Retval += SW_APPPATH + " " + this._AppInstallationPath + " ";
                }

                //-- FTP host name
                if (string.IsNullOrEmpty(this._FTPHostName) == false)
                {
                    Retval += SW_FTP_HOST + " " + this._FTPHostName + " ";
                }

                // FTP Directory
                if (string.IsNullOrEmpty(this._FTPDirectoryName) == false)
                {
                    Retval += SW_FTP_DIR + " " + this._FTPDirectoryName + " ";
                }

                // -- FTP username
                if (string.IsNullOrEmpty(this._FTPUserName) == false)
                {
                    Retval += SW_FTP_UN + " " + this._FTPUserName + " ";
                }

                // -- FTP Pass
                //-- FTP password can be blank
                Retval += SW_FTP_PWD + " ";
                if (string.IsNullOrEmpty(this._FTPPassCode) == false)
                {
                    Retval += this._FTPPassCode + " ";
                }

            }
                return Retval;

        }

        #endregion

        #endregion

        #region "-- Private --"

        #region "-- Variables --"


            //-- Sample Command Line Argument
            //-- "/a C:\-- Projects --\DevInfo 5.0 - VS 2005\DI5-UserInterface-Desktop\bin\DevInfo 5.0 /t DXA "
            //-- switch /a defines Adaptation folder path
            //-- switch /t defines DX Application invoking UI (DXA=>Assistant, DXN=>Notes(Comments))


        //-- Constants below are used command line arguments switches.

        // /a Adaptation  /App_Name ApplicationName /App_Path AppInstallationPath  /ftp_h FTPServerName /ftp_un FTPUserName /ftp_pwd FTPPassCode /ftp_dir FTPDirectoryName  ;

        private const string SW_ADAPTATION = "/a";

        //-- This switch can be used as Calling DX application name, or Live update Server folder name For e.g:
        private const string SW_APPNAME = "/App_Name";
        private const string SW_APPPATH = "/App_Path";

        private const string SW_FTP_HOST = "/ftp_h";
        private const string SW_FTP_UN = "/ftp_un";
        private const string SW_FTP_PWD = "/ftp_pwd";
        private const string SW_FTP_DIR = "/ftp_dir";
        
        


        #endregion


        #region "-- Methods --"
        private void SetArguments(string commandLineArgument)
        {

           if (string.IsNullOrEmpty(commandLineArgument) == false)
            {
                //-- Command line is in format:
                // /a Adaptation  /App_Name ApplicationName /App_Path AppInstallationPath  /ftp_h FTPServerName /ftp_un FTPUserName /ftp_pwd FTPPassCode /ftp_dir FTPDirectoryName  ;

                //- Get Adaptation
                if (commandLineArgument.Contains(CommandLineArguments.SW_ADAPTATION))
                {
                    this._Adaptation = this.GetArgument(commandLineArgument, CommandLineArguments.SW_ADAPTATION, CommandLineArguments.SW_APPNAME);
                }

                if (commandLineArgument.Contains(SW_APPNAME))
                {
                    this._ApplicationName = this.GetArgument(commandLineArgument, CommandLineArguments.SW_APPNAME, CommandLineArguments.SW_APPPATH);
                }

                if (commandLineArgument.Contains(SW_APPPATH))
                {
                    this._AppInstallationPath = this.GetArgument(commandLineArgument, CommandLineArguments.SW_APPPATH, CommandLineArguments.SW_FTP_HOST);
                }

                if (commandLineArgument.Contains(SW_FTP_HOST))
                {
                    this._FTPHostName = this.GetArgument(commandLineArgument, CommandLineArguments.SW_FTP_HOST, CommandLineArguments.SW_FTP_DIR);
                }

                if (commandLineArgument.Contains(SW_FTP_DIR))
                {
                    this._FTPDirectoryName = this.GetArgument(commandLineArgument, CommandLineArguments.SW_FTP_DIR, CommandLineArguments.SW_FTP_UN);
                }

                if (commandLineArgument.Contains(SW_FTP_UN))
                {
                    this._FTPUserName = this.GetArgument(commandLineArgument, CommandLineArguments.SW_FTP_UN, CommandLineArguments.SW_FTP_PWD);
                }

                if (commandLineArgument.Contains(SW_FTP_PWD))
                {
                    this._FTPPassCode = this.GetArgument(commandLineArgument, CommandLineArguments.SW_FTP_PWD, string.Empty);
                }
 
            }
        }

        private string GetArgument(string commandLineString, string switchString1, string switchString2)
        {
            string RetVal = string.Empty;

            int StartPos = -1;
            int EndPos = -1;

            if (commandLineString.Contains(switchString1))
            {
                StartPos = commandLineString.IndexOf(switchString1) + switchString1.Length;
                if (string.IsNullOrEmpty(switchString2) == false)
                {
                    EndPos = commandLineString.IndexOf(switchString2);
                }

                if (EndPos != -1)
                {
                    RetVal = commandLineString.Substring(StartPos, EndPos - StartPos);
                }
                else
                {
                    RetVal = commandLineString.Substring(StartPos);
                }
            }
            return RetVal.Trim();
        }

        #endregion
        #endregion



    }
}
