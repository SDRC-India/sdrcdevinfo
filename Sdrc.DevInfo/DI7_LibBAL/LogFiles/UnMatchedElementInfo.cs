using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.LogFiles
{
    public class UnMatchedElementInfo
    {

        #region"--Public--"

        #region"--Variables/Properties--"
        public Dictionary<string, string> Indicators;
        public Dictionary<string, string> Units;
        public Dictionary<string, string> Subgroups;
        public Dictionary<string, string> Areas;
        #endregion

        #region"--New/Dispose--"
        public UnMatchedElementInfo()
        {
            Indicators = new Dictionary<string, string>();
            Units = new Dictionary<string, string>();
            Subgroups = new Dictionary<string, string>();
            Areas = new Dictionary<string, string>();
        }
        #endregion
        #endregion




    }
}
