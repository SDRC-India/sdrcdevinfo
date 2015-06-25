using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibSDMX
{
    public class ArtefactRef
    {
        #region "--Variables--"

        #region "--Private--"

        private string _id;

        private string _agencyId;

        private string _version;

        private string _language;

        private string _name;

        private string _description;

        private ArtefactTypes _artefactType;

        #endregion "--Private--"

        #region "--Public--"

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

        public string Language
        {
            get
            {
                return this._language;
            }
            set
            {
                this._language = value;
            }
        }

        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                this._name = value;
            }
        }

        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        public ArtefactTypes ArtefactType
        {
            get
            {
                return this._artefactType;
            }
            set
            {
                this._artefactType = value;
            }
        }

        #endregion "--Public--"

        #endregion "--Variables--"

        #region "--Constructors--"

        #region "--Private--"

        #endregion "--Private--"

        #region "--Public--"

        public ArtefactRef(string id, string agencyId, string version, string language, string name, string description, ArtefactTypes artefactType)
        {
            this._id = id;
            this._agencyId = agencyId;
            this._version = version;
            this._language = language;
            this._name = name;
            this._description = description;
            this._artefactType = artefactType;
        }

        #endregion "--Public--"

        #endregion "--Constructors--"
    }
}
