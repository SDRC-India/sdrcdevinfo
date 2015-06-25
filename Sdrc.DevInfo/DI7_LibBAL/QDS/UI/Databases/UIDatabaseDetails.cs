using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace DevInfo.Lib.DI_LibBAL.QDS.UI.Databases
{
    [Serializable]
    public class UIDatabaseDetails
    {
        #region "-- Properties --"

        private List<UIDatabaseInfo> _DatabaseInfo = new List<UIDatabaseInfo>();
        public List<UIDatabaseInfo> DatabaseInfo
        {
            get { return _DatabaseInfo; }
            set { _DatabaseInfo = value; }
        }

        #endregion

        #region "-- Public --"

        #region "-- Methods --"

        /// <summary>
        /// Save serialize data into xml file
        /// </summary>
        /// <param name="xmlFileWithPath"></param>
        public void Save(string xmlFileWithPath)
        {
            FileStream FS = null;
            XmlSerializer SRZFrmt;

            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(xmlFileWithPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(xmlFileWithPath));
                }

                FS = new FileStream(xmlFileWithPath, FileMode.Create);
                SRZFrmt = new XmlSerializer(typeof(UIDatabaseDetails));
                SRZFrmt.Serialize(FS, this);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (FS != null)
                {
                    FS.Flush();
                    FS.Close();
                }
            }
        }

        /// <summary>
        /// Load xml file and deserialize data
        /// </summary>
        /// <param name="xmlFileWithPath"></param>
        /// <returns></returns>
        public UIDatabaseDetails Load(string xmlFileWithPath)
        {
            UIDatabaseDetails RetVal = new UIDatabaseDetails();
            string FilePath = string.Empty;
            FileStream FS = null;

            try
            {
                if (!File.Exists(xmlFileWithPath))
                {
                    FilePath = Path.GetDirectoryName(xmlFileWithPath);

                    if (!Directory.Exists(FilePath))
                    {
                        Directory.CreateDirectory(FilePath);
                    }

                    (new UIDatabaseDetails()).Save(xmlFileWithPath);
                }

                FS = new FileStream(xmlFileWithPath, FileMode.Open);
                XmlSerializer Serializer = new XmlSerializer(typeof(UIDatabaseDetails));
                RetVal = (UIDatabaseDetails)Serializer.Deserialize(FS);
            }
            catch (Exception)
            {
            }
            finally
            {
                if (FS != null)
                {
                    FS.Flush();
                    FS.Close();
                }
            }

            return RetVal;
        }

        #endregion

        #endregion
    }
}
