using System; 
using System.Drawing;
using System.Collections;
//using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Map
{
    [Serializable()]
    public class ShapeInfo
    {
        private string m_ID;
        private int m_RecordCount;
        private ShapeType m_ShapeType;

        //Collection of Shape class/object
        private Hashtable m_Records = new Hashtable();
        private RectangleF m_Extent = new RectangleF();
        private SourceType m_SourceType;

        public string ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        public int RecordCount
        {
            get { return m_RecordCount; }
            set { m_RecordCount = value; }
        }

        public ShapeType ShapeType
        {
            get { return m_ShapeType; }
            set { m_ShapeType = value; }
        }

        [XmlIgnore()]
        public Hashtable Records
        {
            get { return m_Records; }
            set { m_Records = value; }
        }

        public RectangleF Extent
        {
            get { return m_Extent; }
            set { m_Extent = value; }
        }

        public SourceType SourceType
        {
            get { return m_SourceType; }
            set { m_SourceType = value; }
        }



        public static ShapeType SetLayerInfo(Layer p_Layer, string p_FileName, string p_LayerNid)
        {
            ShapeInfo _ShapeInfo;
            System.IO.FileStream _IO = new System.IO.FileStream(p_FileName, FileMode.Open);
           // XmlSerializer _SRZFrmt = new XmlSerializer();
            XmlSerializer _SRZFrmt = new XmlSerializer(typeof(ShapeInfo));                    
            _ShapeInfo = (ShapeInfo)_SRZFrmt.Deserialize(_IO);
            _IO.Flush();
            _IO.Close();
            {
                if (p_LayerNid.Length > 0)
                    p_Layer.ID = p_LayerNid;
                p_Layer.SourceType = _ShapeInfo.m_SourceType;
                p_Layer.Extent = _ShapeInfo.m_Extent;
                p_Layer.RecordCount = _ShapeInfo.m_RecordCount;
                //.LayerType = _ShapeInfo.m_ShapeType
                p_Layer.Records = _ShapeInfo.m_Records;
            }
            return _ShapeInfo.m_ShapeType;
        }

        public static ShapeType SetLayerInfo(Layer p_Layer, string p_FileName)
        {
            return SetLayerInfo(p_Layer, p_FileName, "");
        }
    }
}