using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Security.Cryptography;
using System.IO;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibBAL.Utility
{
    /// <summary>
    /// Summary description for DIMapServer
    /// </summary>
    public static class DIMapServer
    {
        private readonly static string EncryptionKey = "<\"}#$7#%";
        public static string MapServerName = string.Empty;

        public readonly static DIMapServerWS.Utility WebServiceInstance = DIMapServer.GetInstance();

        private static DIMapServerWS.Utility GetInstance()
        {
            DIMapServerWS.Utility RetVal = null;

            if (WebServiceInstance == null)
            {
                RetVal = new DIMapServerWS.Utility();
            }

            return RetVal;
        }

        public static DIConnectionDetails GetMapServerConnectionDetails()
        {
            //ServerType||ServerName||Database||User||Pasword

            DIConnectionDetails RetVal = new DIConnectionDetails();
            List<string> connectionDetailsWS;

            //GetConnection from web service
            connectionDetailsWS = new List<string>(DIMapServer.WebServiceInstance.GetMapServerConnection().Split("||".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));

            RetVal.ServerType = (DIServerType)Convert.ToInt32(connectionDetailsWS[0]);

            if (!string.IsNullOrEmpty(DIMapServer.MapServerName))
            {
                RetVal.ServerName = DIMapServer.MapServerName;
            }
            else
            {
                RetVal.ServerName = connectionDetailsWS[1];
            }

            RetVal.DbName = connectionDetailsWS[2];
            RetVal.UserName = connectionDetailsWS[3];
            RetVal.Password = DIMapServer.DecryptStringForMapServer(connectionDetailsWS[4]);

            //RetVal.ServerName = "dgps2";
            //RetVal.DbName = "DI7_MDG_r12";
            //RetVal.UserName = "sa";
            //RetVal.Password = "l9ce130";

            //RetVal.ServerName = "23.23.128.77";
            //RetVal.DbName = "DI7_ChildProtection";
            //RetVal.UserName = "sa";
            //RetVal.Password = "l9ce131";

            return RetVal;
        }

        public static DIQueries GetMapServerQueries(string DataPrefix_UI, string LanguageCode_UI, DIConnection connection)
        {
            //TO Do GetConnection from web service
            DIQueries RetVal = null;

            try
            {
                if (connection.DIDataSets().Select(DBAvailableDatabases.AvlDBPrefix + "='" + DataPrefix_UI.Trim('_') + "'").Length == 0)
                {
                    DataPrefix_UI = connection.DIDataSetDefault();
                }

                if (connection.DILanguages(DataPrefix_UI).Select(Language.LanguageCode + "='" + LanguageCode_UI + "'").Length == 0)
                {
                    LanguageCode_UI = connection.DILanguageCodeDefault(DataPrefix_UI);
                }

                RetVal = new DIQueries(DataPrefix_UI, LanguageCode_UI);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RetVal;
        }

        public static string EncryptStringForMapServer(string text)
        {
            string RetVal;
            DESCryptoServiceProvider CryptoProvider;
            MemoryStream MemoryStream;
            CryptoStream CryptoStream;
            StreamWriter Writer;
            byte[] Bytes;

            RetVal = string.Empty;

            if (!string.IsNullOrEmpty(text))
            {
                Bytes = ASCIIEncoding.ASCII.GetBytes(DIMapServer.EncryptionKey);
                CryptoProvider = new DESCryptoServiceProvider();
                MemoryStream = new MemoryStream(Bytes.Length);
                CryptoStream = new CryptoStream(MemoryStream, CryptoProvider.CreateEncryptor(Bytes, Bytes), CryptoStreamMode.Write);
                Writer = new StreamWriter(CryptoStream);
                Writer.Write(text);
                Writer.Flush();
                CryptoStream.FlushFinalBlock();
                Writer.Flush();

                RetVal = Convert.ToBase64String(MemoryStream.GetBuffer(), 0, (int)MemoryStream.Length);
            }

            return RetVal;
        }

        public static string DecryptStringForMapServer(string text)
        {
            string RetVal;
            DESCryptoServiceProvider CryptoProvider;
            MemoryStream MemoryStream;
            CryptoStream CryptoStream;
            StreamReader Reader;
            byte[] Bytes;

            RetVal = string.Empty;

            if (!string.IsNullOrEmpty(text))
            {
                Bytes = ASCIIEncoding.ASCII.GetBytes(DIMapServer.EncryptionKey);
                CryptoProvider = new DESCryptoServiceProvider();
                MemoryStream = new MemoryStream(Convert.FromBase64String(text));
                CryptoStream = new CryptoStream(MemoryStream, CryptoProvider.CreateDecryptor(Bytes, Bytes), CryptoStreamMode.Read);
                Reader = new StreamReader(CryptoStream);
                RetVal = Reader.ReadToEnd();
            }

            return RetVal;
        }

    }

}
