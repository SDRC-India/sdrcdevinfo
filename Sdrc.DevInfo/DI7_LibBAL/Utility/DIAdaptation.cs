using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Utility
{
    /// <summary>
    /// Class to store primary adaptaion information like adaptation name and adaptaion folder path 
    /// </summary>
    /// <remarks>
    /// For preserving detailed adaptaion information within adaptation folder use DevInfo.Lib.DI_LibBAL.UI.UserPreference.Adaptation class
    /// </remarks>
    public class DIAdaptation
    {

        #region " -- Constructor -- "

        /// <summary>
        /// Default constructor required for xml serialization
        /// </summary>
        public DIAdaptation()
        {
        }

        /// <summary>
        /// Parameterized constructor for DIAdaptation creation
        /// </summary>
        /// <param name="name"></param>
        /// <param name="adaptationPath"></param>
        public DIAdaptation(string adaptationName, string adaptationVersion, string adaptationFolder)
        {
            this._AdaptationName = adaptationName;
            this._AdaptationVersion = adaptationVersion;
            this._AdaptationFolder = adaptationFolder;
        }

        #endregion

        #region " -- Properties -- "

        private string _AdaptationName = string.Empty;
        /// <summary>
        /// Gets or sets unique adaptaion name
        /// </summary>
        /// <remarks>While adding adaptaion to collection DIUI application shall ensure the uniqueness of Adaptation Names</remarks>
        public string AdaptationName
        {
            get { return _AdaptationName; }
            set { _AdaptationName = value; }
        }

        private string _AdaptationVersion = string.Empty;
        /// <summary>
        /// Gets or sets adaptaion version number
        /// </summary>
        public string AdaptationVersion
        {
            get { return _AdaptationVersion; }
            set { _AdaptationVersion = value; }
        }

        private string _AdaptationFolder = string.Empty;
        /// <summary>
        /// Gets or sets AdaptationPath. 
        /// Normally this would be "C:\DevInfo\[AdptationFolderName]". It may be set to some other location too, in case of custom installation
        /// </summary>
        /// <remarks></remarks>
        public string AdaptationFolder
        {
            get { return _AdaptationFolder; }
            set { _AdaptationFolder = value; }
        }




        #endregion
    }
}
