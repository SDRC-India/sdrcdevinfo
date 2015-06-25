using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.QDS.UI.Databases
{
    [Serializable]
    public class UIDatabaseInfo
    {
        #region "-- Properties --"

        private string _DatabaseName = string.Empty;
        public string DatabaseName
        {
            get { return _DatabaseName; }
            set { _DatabaseName = value; }
        }

        private string _DatabaseFilename = string.Empty;
        public string DatabaseFilename
        {
            get { return _DatabaseFilename; }
            set { _DatabaseFilename = value; }
        }

        private string _LastModifiedDataNTime = string.Empty;
        public string LastModifiedDataNTime
        {
            get { return _LastModifiedDataNTime; }
            set { _LastModifiedDataNTime = value; }
        }

        private bool _IsXmlFilesGenerated = false;
        public bool IsXmlFilesGenerated
        {
            get { return _IsXmlFilesGenerated; }
            set { _IsXmlFilesGenerated = value; }
        }

        #endregion
    }
}
