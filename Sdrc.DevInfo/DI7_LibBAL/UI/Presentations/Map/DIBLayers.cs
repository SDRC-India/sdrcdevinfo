using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;
using DevInfo.Lib.DI_LibBAL.Utility;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using Microsoft.VisualBasic;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Map
{
    [Serializable()]
    public class DIBSettings
    {

        /// <summary>
        /// Default filename for DIB Settings
        /// </summary>
        public static string DIBSettingFileName = "DIBCurrent.xml";

        /// <summary>
        /// Default filename for DIB Settings
        /// </summary>
        public static string DIBDefaultFileName = "DIBDefault.xml";

        public DIBSettings()
        {

        }

        public static DIBSettings LoadDIBSettings(string XMLFileName)
        {
            DIBSettings RetVal = null;

            if (File.Exists(XMLFileName))
            {
                FileStream _IO = null;
                if (File.Exists(XMLFileName))
                {
                    try
                    {
                        _IO = new FileStream(XMLFileName, FileMode.Open);
                        XmlSerializer _SRZFrmt = new XmlSerializer(typeof(DIBSettings));
                        RetVal = (DIBSettings)_SRZFrmt.Deserialize(_IO);
                        _IO.Flush();
                        _IO.Close();
                    }
                    catch 
                    {
                        if (_IO != null)
                        {
                            _IO.Close();
                        }
                    }
                }

            }
            return RetVal;
        }

        public void Save(string XMLFileName)
        {
            this._DIBLayersSetting.Add(new DIBLayerSetting());

            //Save XMl Serialized
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(XMLFileName)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(XMLFileName));
                }

                FileStream _IO = new FileStream(XMLFileName, FileMode.Create);
                XmlSerializer _SRZFrmt = new XmlSerializer(typeof(DIBSettings));
                _SRZFrmt.Serialize(_IO, this);
                _IO.Flush();
                _IO.Close();
            }
            catch
            {
            }
        }

        private List<DIBLayerSetting> _DIBLayersSetting = null;
        /// <summary>
        /// gets or sets the collection of DIB layer's Settings object
        /// </summary>
        public List<DIBLayerSetting> DIBLayersSetting
        {
            get { return _DIBLayersSetting; }
            set { _DIBLayersSetting = value; }
        }

        public DIBLayerSetting GetDIBLayerSetting(string layerName)
        {
            DIBLayerSetting RetVal = null; //- Default

            if (this._DIBLayersSetting != null)
            {
                foreach (DIBLayerSetting dibLayerSetting in this._DIBLayersSetting)
                {
                    if (string.Compare(dibLayerSetting.LayerName, layerName, true) == 0)
                    {
                        RetVal = dibLayerSetting;
                        break;
                    }
                }
            }

            return RetVal;
        }
    }


    [Serializable()]
    public class DIBLayerSetting
    {
        public DIBLayerSetting()
        {

        }

        #region "Properties"

        private string _LayerName = string.Empty;
        /// <summary>
        /// Gets or sets the layerName associated with this setting object.
        /// </summary>
        public string LayerName
        {
            get { return _LayerName; }
            set { _LayerName = value; }
        }


        private FillStyle _FillStyle = FillStyle.Solid;
        /// <summary>
        /// 
        /// </summary>
        public FillStyle FillStyle
        {
            get { return _FillStyle; }
            set { _FillStyle = value; }
        }

        private Color _FillColor = Color.FromArgb((int)(VBMath.Rnd() * 255), (int)(VBMath.Rnd() * 255), (int)(VBMath.Rnd() * 255));
        [XmlIgnore()]
        public Color FillColor
        {
            get { return _FillColor; }
            set { _FillColor = value; }
        }

        private DashStyle _BorderStyle = DashStyle.Solid;
        /// <summary>
        /// Gets or sets bordr style
        /// </summary>
        public DashStyle BorderStyle
        {
            get { return _BorderStyle; }
            set { _BorderStyle = value; }
        }

        private float _BorderWidth = 0.01F;
        /// <summary>
        /// gets or sets the Border Width
        /// </summary>
        public float BorderWidth
        {
            get { return _BorderWidth; }
            set { _BorderWidth = value; }
        }

        private Color _BorderColor = Color.LightGray;
        /// <summary>
        /// Gets or sets the Border Color
        /// </summary>
        [XmlIgnore()]
        public Color BorderColor
        {
            get { return _BorderColor; }
            set { _BorderColor = value; }
        }

        //This property is used for _BorderColor variable to get xml serialized.
        [XmlElement("BorderColor")]
        public int XmlBorderColor
        {
            //At the time of serialization, _BorderColor variable gets serialized after in form of color name.
            get
            {
                return this._BorderColor.ToArgb();
            }
            //At time of Deserialzation, value is converted back into _BorderColor
            set
            {
                this._BorderColor = Color.FromArgb(value);
            }
        }

        //This property is used for _FillColor variable to get xml serialized.
        [XmlElement("FillColor")]
        public int XmlFillColor
        {
            //At the time of serialization, _FillColor variable gets serialized after in form of color name.
            get
            {
                return this._FillColor.ToArgb();
            }
            //At time of Deserialzation, value is converted back into _FillColor
            set
            {
                this._FillColor = Color.FromArgb(value);
            }
        }

        #endregion
    }
}
