
// **********************************************************************
// Program Name:[CustomFeature]
// Developed By: DG2
// Creation date: 2006-10-12
// Program Comments: Custom Feature Layer Structure
// **********************************************************************

// **********************Change history*********************************
// No. Mod:Date Mod:By Change Description
// c1 2006-10-17 DG2 Sample for future use
//
//
// **********************************************************************

using System.Drawing;
using System;
using System.IO;
using System.Collections;
using System.Drawing.Drawing2D;
using System.Xml.Serialization;
using System.Collections.Specialized;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Map
{
    [Serializable()]
    [XmlInclude(typeof(FeatureInfo))]
    public class CustomFeature
    {
        //*** For each CustomFeature class one serialized file shall be preserved
        //*** A CustomFeature shall hold multiple records of FeatureInfo in form of collection

        # region "-- Enumeration --"
        public enum FeatureField
        {
            Source = 0,
            Area = 1,
            Indicator = 2,
            Unit = 3,
            Subgroup = 4,
            TimePeriod = 5,
            IndicatorClassification = 6,
            Metadata = 7
            //MetadataSource = 7
            //MetadataIndicator = 8
            //MetadataArea = 9
        }

        # endregion

        # region "-- Variables --"
        private bool mbVisible;
        private string msName = "";
        private FeatureField meFeatureField;
        private string msMetadataField = "";
        //$$$ FeatureFieldValue (Key) - Value (FeatureInfo Structure)
        private Hashtable moFeatureCol = new Hashtable();

        //$$$ FeatureFieldValue Shall be GUID for Indicator, Unit, Subgroup and Indicator Classification && AREAID for Area && TEXT for Time Period, Source and Metadata

        //??? OffsetX & OffsetY

        //// This field is used for Xml serialization of  moFeatureCol.


        # endregion

        # region "-- Properties --"
        public string Name
        {
            get { return msName; }
            set { msName = value; }
        }

        public bool Visible
        {
            get { return mbVisible; }
            set { mbVisible = value; }
        }

        public FeatureField FeatureType
        {
            get { return meFeatureField; }
            set { meFeatureField = value; }
        }

        public string MetadataField
        {
            get { return msMetadataField; }
            set { msMetadataField = value; }
        }

        [XmlIgnore()]
        public Hashtable FeatureCol
        {
            get
            {

                return moFeatureCol;
            }
            set
            {
                moFeatureCol = value;

            }
        }
        //This property will serialize Hashtable object "FeatureCol" using customised class "HashtableSerializationProxy" which can be serialized.

        public HashtableSerializationProxy _XmlFeatureCol = new HashtableSerializationProxy();
        [XmlElement("FeatureCol")]
        public HashtableSerializationProxy XmlFeatureCol
        {
            //At the time of Serialization, this will convert original hashtable into HashtableSerializationProxy class object which can be easily serialized.
            get
            {
                _XmlFeatureCol = new HashtableSerializationProxy(moFeatureCol);
                return _XmlFeatureCol;
            }
            set
            {
                //At the time of Deserialization, HashtableSerializationProxy object's hashtable is restored into Original hashtable. 
                _XmlFeatureCol = value;
                moFeatureCol = value._hashTable;
            }
        }

        # endregion

        # region "--- Methods---"

        public CustomFeature()
        {
            // Constructor is made only for the class to be serialize
        }
        #endregion
    }


    [Serializable()]
    public class FeatureInfo
    {

    #region "-- Constructor --"

        public FeatureInfo()
        {
        }

        public FeatureInfo(string sCaption)
        {
            Caption = sCaption;
        }

    #endregion

    # region "-- Properties --"
        //GUID / Area Id shall form the key and caption will store the actual display String
        public string Caption = "";

        //*** Symbol
        public bool SymbolSet = false;

        [XmlIgnore()]
        public Color SymbolColor = Color.Red;

        //This property is used for SymbolColor variable to get xml serialized.
        [XmlElement("SymbolColor")]
        public int XmlSymbolColor
        {
            //At the time of serialization, SymbolColor variable gets serialized after color transaltion.
            get
            {
                return this.SymbolColor.ToArgb();
            }
            //At time of Deserialzation, value is converted back into SymbolColor
            set
            {
                this.SymbolColor = Color.FromArgb(value);
            }
        }

        [XmlIgnore()]
        public Font SymbolFont = new Font("Webdings", 10);

        //This below property is only used for Font property "SymbolFont" to get serialized.
        [XmlElement("SymbolFont")]
        public string XmlSymbolFont
        {
            //At the time of serialization, SymbolFont variable gets serialized into string (Font Name + Font Size)
            get
            {
                return SymbolFont.Name + "," + SymbolFont.Size.ToString() + "," + SymbolFont.Style.ToString();
            }
            //At time of Deserialzation, value is split into Font Name & Font Size and restored into SymbolFont
            set
            {
                string[] _FontSettings = new string[3];
                _FontSettings = value.Split(",".ToCharArray());
                if (_FontSettings.Length == 3 & _FontSettings[2] != "")
                {
                    this.SymbolFont = new Font(_FontSettings[0], float.Parse(_FontSettings[1]), (FontStyle)Map.GetFontStyleInteger(_FontSettings[2]));
                }
                else
                {
                    this.SymbolFont = new Font(_FontSettings[0], float.Parse(_FontSettings[1]));
                }
            }
        }

        public int SymbolSize = 10;
        public char SymbolChar = "H"[0];
        [XmlIgnore()]
        public Image SymbolImage;
        //public string XmlSymbolImage
        //{
        //    get
        //    {
        //        if (SymbolImage != null)
        //        {
        //            MemoryStream MS = new MemoryStream();
        //            SymbolImage.Save(MS, System.Drawing.Imaging.ImageFormat.Bmp);
        //            return new StreamReader(MS).ReadToEnd();
        //        }
        //        else
        //        {
        //            return string.Empty;
        //        }
        //    }
        //    set
        //    {
        //        if (value != String.Empty)
        //        {
        //            SymbolImage = Image.FromStream(new MemoryStream(Convert.FromBase64String(value)));
        //        }
        //    }
        //}


        //*** Label
        public bool LabelSet = false;
        [XmlIgnore()]
        public Color LabelColor = Color.Black;

        //This property is used for LabelColor variable to get xml serialized.
        [XmlElement("LabelColor")]
        public int XmlLabelColor
        {
            //At the time of serialization, LabelColor variable gets serialized in form of Color Name
            get
            {
                return this.LabelColor.ToArgb();
            }
            //At time of Deserialzation, value string is converted back into LabelColor
            set
            {
                this.LabelColor = Color.FromArgb(value);
            }
        }

        [XmlIgnore()]
        public Font LabelFont = new Font("Arial", 8);

        //This below property is only used for Font variable "LabelFont" to get serialized.
        [XmlElement("LabelFont")]
        public string XmlLabelFont
        {
            //At the time of serialization, LabelFont variable gets serialized into string (Font Name + Font Size)
            get
            {
                return LabelFont.Name + "," + LabelFont.Size.ToString() + "," + LabelFont.Style.ToString();
            }
            //At time of Deserialzation, value is split into Font Name & Font Size and restored into LabelFont
            set
            {
                string[] _FontSettings = new string[3];
                _FontSettings = value.Split(",".ToCharArray());
                if (_FontSettings.Length == 3 & _FontSettings[2] != "")
                {
                    this.LabelFont = new Font(_FontSettings[0], float.Parse(_FontSettings[1]), (FontStyle)Map.GetFontStyleInteger(_FontSettings[2]));
                }
                else
                {
                    this.LabelFont = new Font(_FontSettings[0], float.Parse(_FontSettings[1]));
                }
            }
        }

        //*** FillStyle (Color)
        public bool FillSet = false;

        [XmlIgnore()]
        public Color FillColor = Color.Yellow;

        //This property is used for FillColor variable to get xml serialized.
        [XmlElement("FillColor")]
        public int XmlFillColor
        {
            //At the time of serialization, FillColor variable gets serialized in form of Color Name.
            get
            {
                return this.FillColor.ToArgb();
            }
            //At time of Deserialzation, value string is converted back into FillColor
            set
            {
                this.FillColor = Color.FromArgb(value);
            }
        }

        //Polygon Custom Layer
        public FillStyle FillStyle = FillStyle.Percent10;

        [XmlIgnore()]
        public Color BorderColor = Color.LightGray;

        //This property is used for BorderColor variable to get xml serialized.
        [XmlElement("BorderColor")]
        public int XmlBorderColor
        {
            //At the time of serialization, BorderColor variable gets serialized in form of color Name
            get
            {
                return this.BorderColor.ToArgb();
            }
            //At time of Deserialzation, value is converted back into BorderColor
            set
            {
                this.BorderColor = Color.FromArgb(value);
            }
        }

        public DashStyle BorderStyle = DashStyle.Solid;
        public float BorderSize = 1;
    # endregion

    }

}