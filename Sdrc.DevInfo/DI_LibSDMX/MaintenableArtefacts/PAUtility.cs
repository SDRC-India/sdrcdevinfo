using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using SDMXObjectModel.Structure;
using SDMXObjectModel.Common;
using System.Xml;
using SDMXObjectModel;

namespace DevInfo.Lib.DI_LibSDMX
{
    internal class PAUtility : ArtefactUtility
    {
        #region "--Properties--"

        #region "--Private--"

        private bool _dfdMfdFlag;

        private string _dfdMfdId;

        private string _providerId;

        #endregion "--Private--"

        #region "--Public--"

        internal bool DfdMfdFlag
        {
            get
            {
                return this._dfdMfdFlag;
            }
            set
            {
                this._dfdMfdFlag = value;
            }
        }

        internal string DfdMfdId
        {
            get
            {
                return this._dfdMfdId;
            }
            set
            {
                this._dfdMfdId = value;
            }
        }

        internal string ProviderId
        {
            get
            {
                return this._providerId;
            }
            set
            {
                this._providerId = value;
            }
        }

        #endregion "--Public--"

        #endregion "--Properties--""

        #region "--Constructors--"

        #region "--Private--"

        #endregion "--Private--"

        #region "--Public--"

        internal PAUtility(bool dfdMfdFlag, string dfdMfdId, string providerId, string agencyId, Header header, string outputFolder)
            : base(agencyId, string.Empty, header, outputFolder)
        {
            this._dfdMfdFlag = dfdMfdFlag;
            this._dfdMfdId = dfdMfdId;
            this._providerId = providerId;
        }

        #endregion "--Public--"

        #endregion "--Constructors--""

        #region "--Methods--"

        #region "--Private--"

        private ArtefactInfo Prepare_ArtefactInfo_From_PA(SDMXObjectModel.Structure.ProvisionAgreementType ProvisionAgreement, string FileName)
        {
            ArtefactInfo RetVal;
            SDMXObjectModel.Message.StructureType Structure;
            XmlDocument XmlContent;

            RetVal = null;
            XmlContent = null;

            try
            {
                Structure = this.Get_Structure_Object(ProvisionAgreement);
                XmlContent = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.StructureType), Structure);
                RetVal = new ArtefactInfo(ProvisionAgreement.id, ProvisionAgreement.agencyID, ProvisionAgreement.version, string.Empty, ArtefactTypes.PA, FileName, XmlContent);
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

        private SDMXObjectModel.Message.StructureType Get_Structure_Object(SDMXObjectModel.Structure.ProvisionAgreementType ProvisionAgreement)
        {
            SDMXObjectModel.Message.StructureType RetVal;

            RetVal = new SDMXObjectModel.Message.StructureType();
            RetVal.Header = this.Get_Appropriate_Header();
            RetVal.Structures = new StructuresType(ProvisionAgreement, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
            RetVal.Footer = null;

            return RetVal;
        }

        #endregion "--Private--"

        #region "--Public--"

        public override List<ArtefactInfo> Generate_Artefact()
        {
            List<ArtefactInfo> RetVal;
            ArtefactInfo Artefact;
            SDMXObjectModel.Structure.ProvisionAgreementType ProvisionAgreement;
            string PAId, FileName;

            RetVal = null;
            PAId = string.Empty;
            FileName = string.Empty;

            try
            {
                PAId = this._providerId.Replace(Constants.DataProviderScheme.Prefix, Constants.PA.Prefix) + Constants.Underscore + this._dfdMfdId;
                ProvisionAgreement = new ProvisionAgreementType(PAId, this.AgencyId, Constants.PA.Version, Constants.PA.Name, Constants.PA.Description, Constants.DefaultLanguage, null);

                ProvisionAgreement.StructureUsage = new StructureUsageReferenceType();
                ProvisionAgreement.StructureUsage.Items.Add(new StructureUsageRefType(this._dfdMfdId, this.AgencyId, Constants.DFD.Version));

                ((StructureUsageRefType)ProvisionAgreement.StructureUsage.Items[0]).packageSpecified = true;
                ((StructureUsageRefType)ProvisionAgreement.StructureUsage.Items[0]).classSpecified = true;

                if (this._dfdMfdFlag == true)
                {
                    ((StructureUsageRefType)ProvisionAgreement.StructureUsage.Items[0]).package = PackageTypeCodelistType.datastructure;
                    ((StructureUsageRefType)ProvisionAgreement.StructureUsage.Items[0]).@class = ObjectTypeCodelistType.Dataflow;
                }
                else
                {
                    ((StructureUsageRefType)ProvisionAgreement.StructureUsage.Items[0]).package = PackageTypeCodelistType.metadatastructure;
                    ((StructureUsageRefType)ProvisionAgreement.StructureUsage.Items[0]).@class = ObjectTypeCodelistType.Metadataflow;
                }

                ProvisionAgreement.DataProvider = new DataProviderReferenceType();
                ProvisionAgreement.DataProvider.Items.Add(new DataProviderRefType(this._providerId, Constants.DataProviderScheme.AgencyId, Constants.DataProviderScheme.Id, Constants.DataProviderScheme.Version));

                FileName = PAId + Constants.XmlExtension;

                // Preparing Artefact and saving
                Artefact = this.Prepare_ArtefactInfo_From_PA(ProvisionAgreement, FileName);
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
