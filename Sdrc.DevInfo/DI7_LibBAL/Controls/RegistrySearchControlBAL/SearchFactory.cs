using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;

namespace DevInfo.Lib.DI_LibBAL.Controls.RegistrySearchControlBAL
{

    /// <summary>
    /// RegistrySearch Type
    /// </summary>
    public enum RegistrySearchType
    {
        Indicator,
        Unit,
        Subgroup
    }

    /// <summary>
    /// Factory to Get the Instance of Registry Search Type based class
    /// </summary>
    public class SearchFactory
    {
        /// <summary>
        /// Get Registry Search class Instance
        /// </summary>
        /// <param name="registrySearchType"></param>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        /// <returns></returns>
        public static SearchBase CreateInstance(RegistrySearchType registrySearchType, DIConnection dbConnection, DIQueries dbQueries)
        {
            SearchBase RetVal = null;

            switch (registrySearchType)
            {
                case RegistrySearchType.Indicator:
                    RetVal = new SearchIndicators(dbConnection, dbQueries);
                    break;
                case RegistrySearchType.Unit:
                    RetVal = new SearchUnits();
                    break;
                case RegistrySearchType.Subgroup:
                    RetVal = new SearchSubgroups();
                    break;
                default:
                    break;
            }

            if (RetVal != null)
            {
                RetVal.DBConnection = dbConnection;
                RetVal.DBQueries = dbQueries;
            }

            return RetVal;
        }

    }
}
