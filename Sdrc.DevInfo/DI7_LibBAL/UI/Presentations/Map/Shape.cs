using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
//using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Map
{
    [Serializable()]
    public class Shape
    {
        //*** Key
        private string m_AreaId = "";
        private string m_AreaName = "";
        private RectangleF m_Extent = new RectangleF();
        private PointF m_Centroid = new PointF();
        // PointF Array
        private System.Collections.ArrayList m_Parts = new ArrayList();

        public string AreaId
        {
            get
            {
                return m_AreaId;
            }
            set
            {
                m_AreaId = value;
            }
        }

        public string AreaName
        {
            get
            {
                return m_AreaName;
            }
            set
            {
                m_AreaName = value;
            }
        }


        public RectangleF Extent
        {
            get
            {
                return m_Extent;
            }
            set
            {
                m_Extent = value;
            }
        }

        public PointF Centroid
        {
            get
            {
                return m_Centroid;
            }
            set
            {
                m_Centroid = value;
            }
        }

        //[XmlIgnore()]
        [XmlArray("Parts")]
        [XmlArrayItem("Parts", typeof(PointF[]))]    //Xml serializer needs to know the type of object that Arraylist will be containing.
        public System.Collections.ArrayList Parts
        {
            get
            {
                return m_Parts;
            }

            set
            {
                m_Parts = value;
            }
        }


    }


}