using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace DevInfo.Lib.DI_LibDAL.Resources
{
    /// <summary>
    /// Helps in getting DAL resources like blank database/template file.
    /// </summary>
    public static class Resource
    {
        /// <summary>
        /// Get blank devinfo database file.
        /// </summary>
        /// <param name="fileNameWPath">Database file path</param>
        /// <returns></returns>
        public static bool GetBlankDevInfoDBFile(string fileNameWPath)
        {
            bool RetVal = false;

            try
            {
                //delete the file if it is already exist.
                if (File.Exists(fileNameWPath))
                {
                    File.Delete(fileNameWPath);
                }

                //read file from resources and create new file 
                File.WriteAllBytes(fileNameWPath, Resource1.New_Template);
            }
            catch (Exception ex)
            {
                RetVal = false;
                new ApplicationException(ex.ToString());
            }


            return RetVal;
        }
    }

}
