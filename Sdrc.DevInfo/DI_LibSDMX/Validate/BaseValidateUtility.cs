using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;

namespace DevInfo.Lib.DI_LibSDMX
{
    internal class BaseValidateUtility
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

        internal BaseValidateUtility()
        {
        }

        #endregion "Public"

        #endregion "Constructors"

        #region "Methods"

        #region "Private"


        #endregion "Private"

        #region "Public"

        internal virtual Dictionary<string, string> ValidateSdmxMlAgainstDSD(string dataFileNameWPath, string completeFileNameWPath)
        {
            return new Dictionary<string, string>();
        }

        internal virtual Dictionary<string, string> ValidateSdmxML(string dataFileNameWPath)
        {
            return new Dictionary<string, string>();
        }

        internal virtual Dictionary<string, string> ValidateMetadataReportAgainstMSD(string MetadataFileNameWPath, string completeFileNameWPath, string MFD_Id)
        {
            return new Dictionary<string, string>();
        }

        internal virtual Dictionary<string, string> ValidateDSDAgainstDevInfoDSD(string dsdFileNameWPath, string devinfodsdFileNameWPath)
        {
            return new Dictionary<string, string>();
        }

        internal virtual Dictionary<string, string> ValidateDSD(string dsdFileNameWPath)
        {
            return new Dictionary<string, string>();
        }




        #endregion "Public"

        #endregion "Methods"
    }
}
