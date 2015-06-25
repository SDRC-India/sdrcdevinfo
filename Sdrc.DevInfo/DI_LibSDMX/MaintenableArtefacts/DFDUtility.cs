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
    internal class DFDUtility : ArtefactUtility
    {
        #region "--Properties--"

        #region "--Private--"

        private string _dsdId;

        #endregion "--Private--"

        #region "--Public--"

        public string DSDId
        {
            get
            {
                return this._dsdId;
            }
            set
            {
                this._dsdId = value;
            }
        }

        #endregion "--Public--"

        #endregion "--Properties--""

        #region "--Constructors--"

        #region "--Private--"

        #endregion "--Private--"

        #region "--Public--"

        internal DFDUtility(string dsdId, string agencyId, Header header, string outputFolder)
            : base(agencyId, string.Empty, header, outputFolder)
        {
            this._dsdId = dsdId;
        }

        #endregion "--Public--"

        #endregion "--Constructors--""

        #region "--Methods--"

        #region "--Private--"

        private ArtefactInfo Prepare_ArtefactInfo_From_DFD(SDMXObjectModel.Structure.DataflowType Dataflow, string FileName)
        {
            ArtefactInfo RetVal;
            SDMXObjectModel.Message.StructureType Structure;
            XmlDocument XmlContent;

            RetVal = null;
            XmlContent = null;

            try
            {
                Structure = this.Get_Structure_Object(Dataflow);
                XmlContent = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.StructureType), Structure);
                RetVal = new ArtefactInfo(Dataflow.id, Dataflow.agencyID, Dataflow.version, string.Empty, ArtefactTypes.DFD, FileName, XmlContent);
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

        private SDMXObjectModel.Message.StructureType Get_Structure_Object(SDMXObjectModel.Structure.DataflowType Dataflow)
        {
            SDMXObjectModel.Message.StructureType RetVal;

            RetVal = new SDMXObjectModel.Message.StructureType();
            RetVal.Header = this.Get_Appropriate_Header();
            RetVal.Structures = new StructuresType(null, null, null, null, null, null, null, null, null, null, null, null, null, Dataflow, null);
            RetVal.Footer = null;

            return RetVal;
        }

        #endregion "--Private--"

        #region "--Public--"

        public override List<ArtefactInfo> Generate_Artefact()
        {
            List<ArtefactInfo> RetVal;
            ArtefactInfo Artefact;
            SDMXObjectModel.Structure.DataflowType Dataflow;

            RetVal = null;

            try
            {
                Dataflow = new SDMXObjectModel.Structure.DataflowType(Constants.DFD.Id, this.AgencyId, Constants.DFD.Version, Constants.DFD.Name, Constants.DFD.Description, Constants.DefaultLanguage, null);

                Dataflow.Structure = new DataStructureReferenceType();
                Dataflow.Structure.Items.Add(new DataStructureRefType(this._dsdId, this.AgencyId, Constants.DFD.Version));

                // Preparing Artefact and saving
                Artefact = this.Prepare_ArtefactInfo_From_DFD(Dataflow, Constants.DFD.FileName);
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
