using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace DevInfo.Lib.DI_LibSDMX
{
    /// <summary>
    /// This class represents an artefact that has been produced.
    /// </summary>
    public class ArtefactInfo
    {
        #region "Properties"

        #region "Private"

        private string _id;

        private string _agencyId;

        private string _version;

        private string _urn;

        private ArtefactTypes _type;

        private string _fileName;

        private XmlDocument _content;

        #endregion "Private"

        #region "Public"

        public string Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }

        public string AgencyId
        {
            get
            {
                return this._agencyId;
            }
            set
            {
                this._agencyId = value;
            }
        }

        public string Version
        {
            get
            {
                return this._version;
            }
            set
            {
                this._version = value;
            }
        }

        public string URN
        {
            get
            {
                return this._urn;
            }
            set
            {
                this._urn = value;
            }
        }

        public ArtefactTypes Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        public string FileName
        {
            get
            {
                return this._fileName;
            }
            set
            {
                this._fileName = value;
            }
        }

        public XmlDocument Content
        {
            get
            {
                return this._content;
            }
            set
            {
                this._content = value;
            }
        }

        #endregion "Public"

        #endregion "Properties"

        #region "Constructors"

        #region "Private"

        #endregion "Private"

        #region "Public"

        public ArtefactInfo(string id, string agencyId, string version, string urn, ArtefactTypes type, string fileName, XmlDocument content)
        {
            this._id = id;
            this._agencyId = agencyId;
            this._version = version;
            this._urn = urn;
            this._type = type;
            this._fileName = fileName;
            this._content = content;
        }

        public ArtefactInfo():this(string.Empty, string.Empty, string.Empty, string.Empty, ArtefactTypes.None, string.Empty, null) 
        {
        }

        #endregion "Public"

        #endregion "Constructors"
    }
}
