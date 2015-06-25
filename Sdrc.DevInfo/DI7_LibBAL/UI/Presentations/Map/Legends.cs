using System;
using System.Collections;
//using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Map
{
    [Serializable()]
    [XmlInclude(typeof(Legend))]
    public class Legends : CollectionBase, ICloneable
    {

        public Legends()
            : base()
        {
        }

        public void Add(Legend p_Legend)
        {
            List.Add(p_Legend);
        }

        public void AddRange(Legend[] p_Legends)
        {

            foreach (Legend _Legend in p_Legends)
            {
                List.Add(_Legend);
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

        public Legend this[int p_Index]
        {
            get { return (Legend)List[p_Index]; }
        }


        public object Clone()
        {
            object RetVal = null;
            //*** Serialization is one way to do deep cloning. It works only if the objects and its references are serializable
            //BinaryFormatter oBinaryFormatter = new BinaryFormatter();
            XmlSerializer oXmlSerializer = new XmlSerializer(typeof(Legends));
            MemoryStream oMemStream = new MemoryStream();
            oXmlSerializer.Serialize(oMemStream, this);
            oMemStream.Position = 0;
            RetVal = (Legends)oXmlSerializer.Deserialize(oMemStream);
            oMemStream.Close();
            oMemStream.Dispose();
            oMemStream = null;
            return (Legends)RetVal;
            //return functionReturnValue;
        }
    }
}