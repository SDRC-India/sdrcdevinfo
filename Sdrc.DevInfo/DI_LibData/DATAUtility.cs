using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;


namespace DevInfo.Lib.DI_LibDATA
{
    public static class DATAUtility
    {
        public static string Get_Data(XmlDocument query, DataTypes format, DIConnection DIConnection, DIQueries DIQueries)
        {
            string RetVal;
            BaseDataUtility BaseDataUtility;

            RetVal = string.Empty;
            BaseDataUtility = null;

            try
            {
                switch (format)
                {
                    case DataTypes.JSON:
                        BaseDataUtility = new JsonDataUtility(DIConnection, DIQueries);
                        break;
                    case DataTypes.XML:
                        BaseDataUtility = new XmlDataUtility(DIConnection, DIQueries);
                        break;
                    default:
                        break;


                }

                RetVal = BaseDataUtility.Get_Data(query);
            }
            catch (Exception ex)
            {
                RetVal = string.Empty;
                throw ex;
            }
            finally
            {
            }

            return RetVal;
        }
    }
}
