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
    internal class MFDUtility : ArtefactUtility
    {
        #region "--Properties--"

        #region "--Private--"

        private MSDTypes _msdType;

        #endregion "--Private--"

        #region "--Public--"

        public MSDTypes MSDType
        {
            get
            {
                return this._msdType;
            }
            set
            {
                this._msdType = value;
            }
        }

        #endregion "--Public--"

        #endregion "--Properties--""

        #region "--Constructors--"

        #region "--Private--"

        #endregion "--Private--"

        #region "--Public--"

        internal MFDUtility(MSDTypes msdType, string agencyId, Header header, string outputFolder)
            : base(agencyId, string.Empty, header, outputFolder)
        {
            this._msdType = msdType;
        }

        #endregion "--Public--"

        #endregion "--Constructors--""

        #region "--Methods--"

        #region "--Private--"

        private ArtefactInfo Generate_MFD_Area()
        {
            ArtefactInfo RetVal;
            SDMXObjectModel.Structure.MetadataflowType Metadataflow;

            RetVal = null;
            Metadataflow = null;

            try
            {
                Metadataflow = new SDMXObjectModel.Structure.MetadataflowType(Constants.MFD.Area.Id, this.AgencyId, Constants.MFD.Area.Version, Constants.MFD.Area.Name, Constants.MFD.Area.Description, Constants.DefaultLanguage, null);

                Metadataflow.Structure = new MetadataStructureReferenceType();
                Metadataflow.Structure.Items.Add(new MetadataStructureRefType(Constants.MSD.Area.Id, this.AgencyId, Constants.MSD.Area.Version));

                RetVal = this.Prepare_ArtefactInfo_From_MFD(Metadataflow, Constants.MFD.Area.FileName);
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

        private ArtefactInfo Generate_MFD_Indicator()
        {
            ArtefactInfo RetVal;
            SDMXObjectModel.Structure.MetadataflowType Metadataflow;

            RetVal = null;
            Metadataflow = null;

            try
            {
                Metadataflow = new SDMXObjectModel.Structure.MetadataflowType(Constants.MFD.Indicator.Id, this.AgencyId, Constants.MFD.Indicator.Version, Constants.MFD.Indicator.Name, Constants.MFD.Indicator.Description, Constants.DefaultLanguage, null);

                Metadataflow.Structure = new MetadataStructureReferenceType();
                Metadataflow.Structure.Items.Add(new MetadataStructureRefType(Constants.MSD.Indicator.Id, this.AgencyId, Constants.MSD.Indicator.Version));

                RetVal = this.Prepare_ArtefactInfo_From_MFD(Metadataflow, Constants.MFD.Indicator.FileName);
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

        private ArtefactInfo Generate_MFD_Source()
        {
            ArtefactInfo RetVal;
            SDMXObjectModel.Structure.MetadataflowType Metadataflow;

            RetVal = null;
            Metadataflow = null;

            try
            {
                Metadataflow = new SDMXObjectModel.Structure.MetadataflowType(Constants.MFD.Source.Id, this.AgencyId, Constants.MFD.Source.Version, Constants.MFD.Source.Name, Constants.MFD.Source.Description, Constants.DefaultLanguage, null);

                Metadataflow.Structure = new MetadataStructureReferenceType();
                Metadataflow.Structure.Items.Add(new MetadataStructureRefType(Constants.MSD.Source.Id, this.AgencyId, Constants.MSD.Source.Version));

                RetVal = this.Prepare_ArtefactInfo_From_MFD(Metadataflow, Constants.MFD.Source.FileName);
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

        private ArtefactInfo Prepare_ArtefactInfo_From_MFD(SDMXObjectModel.Structure.MetadataflowType Metadataflow, string FileName)
        {
            ArtefactInfo RetVal;
            SDMXObjectModel.Message.StructureType Structure;
            XmlDocument XmlContent;

            RetVal = null;
            XmlContent = null;

            try
            {
                Structure = this.Get_Structure_Object(Metadataflow);
                XmlContent = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.StructureType), Structure);
                RetVal = new ArtefactInfo(Metadataflow.id, Metadataflow.agencyID, Metadataflow.version, string.Empty, ArtefactTypes.MFD, FileName, XmlContent);
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

        private SDMXObjectModel.Message.StructureType Get_Structure_Object(SDMXObjectModel.Structure.MetadataflowType Metadataflow)
        {
            SDMXObjectModel.Message.StructureType RetVal;

            RetVal = new SDMXObjectModel.Message.StructureType();
            RetVal.Header = this.Get_Appropriate_Header();
            RetVal.Structures = new StructuresType(null, null, null, null, null, null, null, null, null, null, null, null, Metadataflow, null, null);
            RetVal.Footer = null;

            return RetVal;
        }

        #endregion "--Private--"

        #region "--Public--"

        public override List<ArtefactInfo> Generate_Artefact()
        {
            List<ArtefactInfo> RetVal;
            ArtefactInfo Artefact;

            RetVal = null;

            try
            {
                if ((this._msdType & MSDTypes.ALL) == MSDTypes.ALL)
                {
                    Artefact = this.Generate_MFD_Area();
                    this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);

                    Artefact = this.Generate_MFD_Indicator();
                    this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);

                    Artefact = this.Generate_MFD_Source();
                    this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
                }
                else
                {
                    if ((this._msdType & MSDTypes.MSD_Area) == MSDTypes.MSD_Area)
                    {
                        Artefact = this.Generate_MFD_Area();
                        this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
                    }
                    if ((this._msdType & MSDTypes.MSD_Indicator) == MSDTypes.MSD_Indicator)
                    {
                        Artefact = this.Generate_MFD_Indicator();
                        this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
                    }
                    if ((this._msdType & MSDTypes.MSD_Source) == MSDTypes.MSD_Source)
                    {
                        Artefact = this.Generate_MFD_Source();
                        this.Add_ArtefactInfo_To_List(ref RetVal, Artefact);
                    }
                }
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
