using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Forms;

namespace DevInfo.Lib.DI_LibBAL.DatabaseIncrement
{
    [Serializable()]
    [System.Xml.Serialization.XmlRootAttribute("DBIncrement")]
    public class DatabaseIncrement : CollectionBase
    {
        public DatabaseIncrement()
            : base()
        {

        }

        public void Add(DatabaseInfo dbIncrementInfo)
        {
            if (!List.Contains(dbIncrementInfo))
            {
                List.Add(dbIncrementInfo);
            }
        }

        public void Insert(int index, DatabaseInfo dbIncrementInfo)
        {
            List.Insert(index, dbIncrementInfo);
        }

        public DatabaseInfo this[int index]
        {
            get
            {
                if (index < 0)
                    return null;

                return (DatabaseInfo)List[index];
            }
            set { List[index] = value; }
        }

        public DatabaseInfo this[string databaseName]
        {
            get
            {
                DatabaseInfo RetVal = null;

                foreach (DatabaseInfo _DBIncrementInfo in List)
                {
                    if (_DBIncrementInfo.DatabaseName.ToLower() == databaseName.ToLower())
                    {
                        RetVal = _DBIncrementInfo;
                        break;
                    }
                }
                return RetVal;
            }
        }

        public int ItemIndex(string databaseName)
        {
            int RetVal = -1;

            for (int i = 0; i < List.Count; i++)
            {
                if (((DatabaseInfo)List[i]).DatabaseName.ToLower() == databaseName.ToLower())
                {
                    RetVal = i;
                    break;
                }
            }
            return RetVal;
        }

        public void Remove(DatabaseInfo dbIncrementInfo)
        {
            List.Remove(dbIncrementInfo);
        }

        public void Remove(int index)
        {
            List.RemoveAt(index);
        }


        #region "Load and Save"

        public bool Serialize(string FileNameWPath)
        {
            bool RetVal = false;
            string directory = string.Empty;
            FileStream _IO = null;

            try
            {
                directory = Path.GetDirectoryName(FileNameWPath);

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                _IO = new FileStream(FileNameWPath, FileMode.Create);

                XmlSerializer serialize = new XmlSerializer(typeof(DatabaseIncrement));
                serialize.Serialize(_IO, this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_IO != null)
                {
                    _IO.Flush();
                    _IO.Close();
                }
            }

            return RetVal;
        }

        public static DatabaseIncrement Load(string FileNameWPath)
        {
            DatabaseIncrement RetVal = null;
            FileStream _IO = null;

            try
            {
                _IO = new FileStream(FileNameWPath, FileMode.Open);
                XmlSerializer _deSerializer = new XmlSerializer(typeof(DatabaseIncrement));
                RetVal = (DatabaseIncrement)_deSerializer.Deserialize(_IO);
            }
            catch (SerializationException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_IO != null)
                {
                    _IO.Flush();
                    _IO.Close();
                }
            }

            return RetVal;
        }

        #endregion
    }

    [Serializable()]
    public class DatabaseInfo
    {
        public DatabaseInfo()
        {
            //do nothing
        }

        public DatabaseInfo(string DBName, string DBDescription)
        {
            this._DatabaseName = DBName;
            this._DatabaseDescription = DBDescription;
        }

        private string _DatabaseName;

        [XmlElement(ElementName = "name")]
        public string DatabaseName
        {
            get { return _DatabaseName; }
            set { _DatabaseName = value; }
        }

        private string _DatabaseDescription;

        [XmlElement(ElementName = "desc")]
        public string DatabaseDescription
        {
            get { return _DatabaseDescription; }
            set { _DatabaseDescription = value; }
        }

    }

}
