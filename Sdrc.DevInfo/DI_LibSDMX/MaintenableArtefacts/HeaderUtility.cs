using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using SDMXObjectModel.Message;
using SDMXObjectModel.Structure;
using SDMXObjectModel;
using System.Data;
using SDMXObjectModel.Data.Generic;
using SDMXObjectModel.Common;

namespace DevInfo.Lib.DI_LibSDMX
{
    internal class HeaderUtility: ArtefactUtility
    {
        #region "Properties"

        #region "Private"

        #endregion "Private"

        #region "Public"

        #endregion "Public"

        #endregion "Properties"

        #region "Constructors"

        #region "Private"

        #endregion "Private"

        #region "Public"

        internal HeaderUtility(string agencyId, string language, Header header, string outputFolder)
            : base(agencyId, language, header, outputFolder)
        {
            
        }

        #endregion "Public"

        #endregion "Constructors"

        #region "Methods"

        #region "Private"

        private ArtefactInfo Prepare_ArtefactInfo_From_Header(string FileName)
        {
            ArtefactInfo RetVal;
            SDMXObjectModel.Message.StructureType HeaderStructure;
            XmlDocument XmlContent;
            
            RetVal = null;
            XmlContent = null;

            try
            {
                HeaderStructure = this.Get_Structure_Object();
               
                XmlContent = Serializer.SerializeToXmlDocument(typeof(SDMXObjectModel.Message.StructureType), HeaderStructure);
                RetVal = new ArtefactInfo(string.Empty,string.Empty,string.Empty, string.Empty, ArtefactTypes.Header, FileName, XmlContent);
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

        private SDMXObjectModel.Message.StructureType Get_Structure_Object()
        {
            SDMXObjectModel.Message.StructureType RetVal;

            RetVal = new SDMXObjectModel.Message.StructureType();
            RetVal.Header = this.Get_Appropriate_Header();
            RetVal.Structures = new StructuresType(null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
            RetVal.Footer = null;

            return RetVal;
        }


        //private SDMXObjectModel.Message.StructureHeaderType Get_Structure_Object()
        //{
        //    SDMXObjectModel.Message.StructureHeaderType RetVal;

        //    RetVal = new SDMXObjectModel.Message.StructureHeaderType();
          

        //    return RetVal;
        //}

        #endregion "Private"

        #region "Public"

        public override List<ArtefactInfo> Generate_Artefact()
        {
            List<ArtefactInfo> RetVal;
            ArtefactInfo Artefact;

            RetVal = null;

            try
            {
                Artefact = this.Prepare_ArtefactInfo_From_Header(Constants.Header.FileName);
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

        #endregion "Public"

        #endregion "Methods"
    }
}
