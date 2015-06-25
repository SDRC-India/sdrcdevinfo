using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Collections;

namespace DevInfo.Lib.DI_LibBAL.Utility
{
    [Serializable()]
    [System.Xml.Serialization.XmlRootAttribute("HelpForms")]
    public class DIHelp : CollectionBase, ICloneable
    {
        #region "-- Private --"

        #region "-- Variable --"

        #endregion

        #region "-- Methods --"

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- New /Dispose --"

        public DIHelp()
        {
        }

        #endregion

        #region "-- Methods --"

        public void Add(FormSteps p_HelpData1)
        {
            List.Add(p_HelpData1);
        }

        public void AddRange(FormSteps[] p_Steps)
        {
            foreach (FormSteps _FormSteps1 in p_Steps)
            {
                List.Add(_FormSteps1);
            }
        }

        public void Remove(object p_Index)
        {
            List.Remove(p_Index);
        }

        public new int Count
        {
            get { return List.Count; }
        }

        public FormSteps this[int p_Index]
        {
            get { return (FormSteps)List[p_Index]; }
        }

        public FormSteps this[string p_Key]
        {
            get
            {
                FormSteps RetVal = null;
                foreach (FormSteps _helpData in List)
                {
                    if (_helpData.FormName.ToLower() == p_Key.ToLower())
                    {
                        RetVal = _helpData;
                        break;
                    }
                }
                return RetVal;
            }
        }

        public object Clone()
        {
            object RetVal = null;
            //*** Serialization is one way to do deep cloning. It works only if the objects and its references are serializable
            //BinaryFormatter oBinaryFormatter = new BinaryFormatter();
            XmlSerializer oXmlSerializer = new XmlSerializer(typeof(DIHelp));
            MemoryStream oMemStream = new MemoryStream();
            oXmlSerializer.Serialize(oMemStream, this);
            oMemStream.Position = 0;
            RetVal = (DIHelp)oXmlSerializer.Deserialize(oMemStream);
            oMemStream.Close();
            oMemStream.Dispose();
            oMemStream = null;
            return (DIHelp)RetVal;
            //return functionReturnValue;
        }

        #endregion

        #endregion

        #region "-- Load and Save --"

        /// <summary>
        /// Save the DIHelp in form of XML file.
        /// </summary>
        /// <param name="fileNameWPath"></param>
        public void Save(string fileNameWPath)
        {
            StreamWriter HelpWriter = null;
            XmlSerializer HelpSerialize = new XmlSerializer(typeof(DIHelp));

            try
            {
                HelpWriter = new StreamWriter(fileNameWPath);
                HelpSerialize.Serialize(HelpWriter, this);
                HelpWriter.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (HelpWriter != null)
                {
                    HelpWriter.Close();
                    HelpWriter.Dispose();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileNameWPath"></param>
        /// <returns></returns>
        public static DIHelp Load(string fileNameWPath)
        {
            DIHelp RetVal = new DIHelp();
            XmlSerializer HelpSerialize = new XmlSerializer(typeof(DIHelp));
            TextReader HelpReader = null;

            try
            {
                if (File.Exists(fileNameWPath))
                {
                    HelpReader = new StreamReader(fileNameWPath);
                    RetVal = (DIHelp)HelpSerialize.Deserialize(HelpReader);
                    HelpReader.Close();
                }
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw ex;
            }
            finally
            {
                if (HelpReader != null)
                {
                    HelpReader.Close();
                    HelpReader.Dispose();
                }
            }

            return RetVal;
        }

        #endregion
    }

    public class FormSteps
    {
        public FormSteps()
            : base()
        {
        }

        private string _FormName = "Form1";
        [XmlAttribute(AttributeName = "Name")]
        public string FormName
        {
            get { return _FormName; }
            set { _FormName = value; }
        }

        private List<Steps> _FormStep = new List<Steps>();
        [XmlElement(ElementName = "Steps")]
        public List<Steps> FormStep
        {
            get { return _FormStep; }
            set { _FormStep = value; }
        }

        public bool IsExists(int Number)
        {
            bool RetVal = false;

            RetVal = this._FormStep.Exists(delegate(Steps p) { return p.Number == Number; });

            return RetVal;
        }

        public Steps GetFormStep(int Number)
        {
            Steps RetVal = null;

            RetVal = this._FormStep.Find(delegate(Steps p) { return p.Number == Number; });

            return RetVal;
        }
    }

    public class Steps
    {
        private int _Number = 0;
        [XmlAttribute(AttributeName = "num")]
        public int Number
        {
            get { return _Number; }
            set { _Number = value; }
        }

        private List<string> _Content = new List<string>();
        [XmlElement(ElementName = "Content")]
        public List<string> Content
        {
            get { return _Content; }
            set { _Content = value; }
        }

        //private int _Number = 0;
        //[XmlElement(ElementName = "num")]
        //public int Number
        //{
        //    get { return _Number; }
        //    set { _Number = value; }
        //}   

        //private string _Content = "Form1 content";
        //[XmlElement(ElementName = "Content")]
        //public string Content
        //{
        //    get { return _Content; }
        //    set { _Content = value; }
        //}
    }
}
