using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using SDMXObjectModel.Structure;
using SDMXObjectModel.Common;
using System.Xml;
using SDMXObjectModel;
using System.IO;

namespace DevInfo.Lib.DI_LibSDMX
{
    internal class UsersUtility : ArtefactUtility
    {
        #region "--Properties--"

        #region "--Private--"

        private string _originalFileNameWPath;

        private UserTypes _userType;

        private string _id;

        private string _name;

        private bool _addRemoveFlag;

        #endregion "--Private--"

        #region "--Public--"

        internal string OriginalFileNameWPath
        {
            get
            {
                return this._originalFileNameWPath;
            }
            set
            {
                this._originalFileNameWPath = value;
            }
        }

        internal UserTypes UserType
        {
            get
            {
                return this._userType;
            }
            set
            {
                this._userType = value;
            }
        }

        internal string Id
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

        internal string Name
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

        internal bool AddRemoveFlag
        {
            get
            {
                return this._addRemoveFlag;
            }
            set
            {
                this._addRemoveFlag = value;
            }
        }

        #endregion "--Public--"

        #endregion "--Properties--""

        #region "--Constructors--"

        #region "--Private--"

        #endregion "--Private--"

        #region "--Public--"

        internal UsersUtility(string originalFileNameWPath, UserTypes userType, string id, string name, string language, string outputFolder, bool addRemoveFlag)
            : base(string.Empty, language, null, outputFolder)
        {
            this._originalFileNameWPath = originalFileNameWPath;
            this._userType = userType;
            this._id = id;
            this._name = name;
            this._addRemoveFlag = addRemoveFlag;
        }

        #endregion "--Public--"

        #endregion "--Constructors--""

        #region "--Methods--"

        #region "--Private--"

        private bool Is_Existing_User(OrganisationType OrganisationTobeAdded, OrganisationSchemeType OrganisationScheme)
        {
            bool RetVal;

            RetVal = false;

            foreach (OrganisationType Organisation in OrganisationScheme.Organisation)
            {
                if (Organisation.id == OrganisationTobeAdded.id)
                {
                    RetVal = true;
                    break;
                }
            }

            return RetVal;
        }

        private ArtefactInfo Prepare_ArtefactInfo_From_OrganisationScheme(SDMXObjectModel.Structure.OrganisationSchemeType OrganisationScheme, string FileName, ArtefactTypes artefactType)
        {
            ArtefactInfo RetVal;
            SDMXObjectModel.Message.StructureType Structure;
            XmlDocument XmlContent;

            RetVal = null;
            XmlContent = null;

            try
            {
                Structure = this.Get_Structure_Object(OrganisationScheme);
                XmlContent = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.StructureType), Structure);
                RetVal = new ArtefactInfo(OrganisationScheme.id, OrganisationScheme.agencyID, OrganisationScheme.version, string.Empty, artefactType, FileName, XmlContent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        private SDMXObjectModel.Message.StructureType Get_Structure_Object(SDMXObjectModel.Structure.OrganisationSchemeType OrganisationScheme)
        {
            SDMXObjectModel.Message.StructureType RetVal;

            RetVal = new SDMXObjectModel.Message.StructureType();
            RetVal.Header = this.Get_Appropriate_Header();
            RetVal.Structures = new StructuresType(null, null, null, null, null, null, null, null, null, null, null, null, null, null, OrganisationScheme);
            RetVal.Footer = null;

            return RetVal;
        }

        #endregion "--Private--"

        #region "--Public--"

        public override List<ArtefactInfo> Generate_Artefact()
        {
            List<ArtefactInfo> RetVal;
            ArtefactInfo Artefact;
            SDMXObjectModel.Structure.OrganisationSchemeType OrganisationScheme;
            OrganisationType OrganisationTobeAdded, OrganisationTobeRemoved;
            SDMXObjectModel.Message.StructureType LoadStructure;
            string FileName;
            bool IsExistingUser;
            ArtefactTypes ArtefactType;

            RetVal = null;
            Artefact = null;
            OrganisationScheme = null;
            OrganisationTobeAdded = null;
            OrganisationTobeRemoved = null;
            LoadStructure = null;
            FileName = string.Empty;
            ArtefactType = ArtefactTypes.MA;

            try
            {
                if (!string.IsNullOrEmpty(this._originalFileNameWPath))
                {
                    LoadStructure = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), this._originalFileNameWPath);
                    OrganisationScheme = LoadStructure.Structures.OrganisationSchemes[0];
                    FileName = Path.GetFileName(this._originalFileNameWPath);
                    this.OutputFolder = Path.GetDirectoryName(this._originalFileNameWPath);
                }

                if (this._addRemoveFlag == true)
                {
                    switch (this._userType)
                    {
                        case UserTypes.Agency:
                            if (OrganisationScheme == null)
                            {
                                OrganisationScheme = new AgencySchemeType(Constants.MaintenanceAgencyScheme.Id, Constants.MaintenanceAgencyScheme.AgencyId, Constants.MaintenanceAgencyScheme.Version, Constants.MaintenanceAgencyScheme.Name, Constants.MaintenanceAgencyScheme.Description, Constants.DefaultLanguage, null);
                                FileName = Constants.MaintenanceAgencyScheme.FileName;
                            }

                            OrganisationTobeAdded = new AgencyType(this._id, this._name, string.Empty, this.Language, null);
                            ArtefactType = ArtefactTypes.MA;
                            break;
                        case UserTypes.Consumer:
                            if (OrganisationScheme == null)
                            {
                                OrganisationScheme = new DataConsumerSchemeType(Constants.DataConsumerScheme.Id, Constants.DataConsumerScheme.AgencyId, Constants.DataConsumerScheme.Version, Constants.DataConsumerScheme.Name, Constants.DataConsumerScheme.Description, Constants.DefaultLanguage, null);
                                FileName = Constants.DataConsumerScheme.FileName;
                            }

                            OrganisationTobeAdded = new DataConsumerType(this._id, this._name, string.Empty, this.Language, null);
                            ArtefactType = ArtefactTypes.DC;
                            break;
                        case UserTypes.Provider:
                            if (OrganisationScheme == null)
                            {
                                OrganisationScheme = new DataProviderSchemeType(Constants.DataProviderScheme.Id, Constants.DataProviderScheme.AgencyId, Constants.DataProviderScheme.Version, Constants.DataProviderScheme.Name, Constants.DataProviderScheme.Description, Constants.DefaultLanguage, null);
                                FileName = Constants.DataProviderScheme.FileName;
                            }

                            OrganisationTobeAdded = new DataProviderType(this._id, this._name, string.Empty, this.Language, null);
                            ArtefactType = ArtefactTypes.DP;
                            break;
                        default:
                            break;
                    }

                    OrganisationScheme.Annotations = null;
                    OrganisationTobeAdded.Annotations = null;
                    IsExistingUser = Is_Existing_User(OrganisationTobeAdded, OrganisationScheme);

                    foreach (OrganisationType Organisation in OrganisationScheme.Organisation)
                    {
                        Organisation.Annotations = null;

                        if (IsExistingUser == true)
                        {
                            if (Organisation.id == this._id)
                            {
                                OrganisationTobeRemoved = Organisation;
                            }
                        }
                    }

                    if (OrganisationTobeRemoved != null)
                    {
                        OrganisationScheme.Organisation.Remove(OrganisationTobeRemoved);
                    }

                    OrganisationScheme.Organisation.Add(OrganisationTobeAdded);
                }
                else
                {
                    OrganisationScheme.Annotations = null;
                    foreach (OrganisationType Organisation in OrganisationScheme.Organisation)
                    {
                        Organisation.Annotations = null;

                        if (Organisation.id == this._id)
                        {
                            OrganisationTobeRemoved = Organisation;
                        }
                    }

                    OrganisationScheme.Organisation.Remove(OrganisationTobeRemoved);
                }

                Artefact = this.Prepare_ArtefactInfo_From_OrganisationScheme(OrganisationScheme, FileName, ArtefactType);
                this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }

        #endregion "--Public--"

        #endregion "--Methods--""
    }
}
