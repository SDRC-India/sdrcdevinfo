using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    [ComVisible(true)]
  public interface IDIDatabase
    {
        /// <summary>
        ///  Add datavalue into DevInfo database
        /// </summary>
        /// <param name="areaInfo">Instance of AreaInfo</param>
        /// <param name="iusInfo">Instance of Iusinfo</param>
        /// <param name="timeperiod">timeperiod</param>
        /// <param name="source">source name</param>
        /// <param name="dataValue">datavalue</param>
        /// <returns></returns>
        int AddDataPoint(AreaInfo areaInfo, IUSInfo iusInfo, string timeperiod, string source, string dataValue);
       
        /// <summary>
        ///  Add datavalue into DevInfo database
        /// </summary>
        /// <param name="areaInfo">Instance of Area Info</param>
        /// <param name="indicatorName">indicator name</param>
        /// <param name="unitName">unit name</param>
        /// <param name="subgroupValName">subgroup val name</param>
        /// <param name="timeperiod">timeperiod</param>
        /// <param name="source">source name</param>
        /// <param name="dataValue">data value</param>
        /// <returns></returns>
        int AddDataPoint(AreaInfo areaInfo, string indicatorName, string unitName, string subgroupValName, string timeperiod, string source, string dataValue);

        /// <summary>
        /// To open or create DevInfo Database
        /// </summary>
        /// <param name="fileNameWPath"></param>
      void OpenDatabase(string fileNameWPath);
        
        /// <summary>
        /// To open or create DevInfo Database
        /// </summary>
        /// <param name="fileNameWPath"></param>
        /// <param name="datasetPrefix"></param>
        void OpenDatabase(string fileNameWPath, string datasetPrefix);
        
        /// <summary>
        /// To open or create DevInfo database
        /// </summary>
        /// <param name="fileNameWPath"></param>
        /// <param name="datasetPrefix"></param>        
        /// <param name="langaugeCode"></param>
        void OpenDatabase(string fileNameWPath, string datasetPrefix, string langaugeCode);
        
     }
}
