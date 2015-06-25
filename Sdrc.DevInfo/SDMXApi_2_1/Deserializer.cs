using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Xml;
using SDMXObjectModel.Message;

namespace SDMXObjectModel
{
    public static class Deserializer
    {
        #region "--Public--"

        public static Object LoadFromFile(System.Type ObjectType, string FileNameWPath)
        {
            Object RetVal;
            StreamReader Reader;

            RetVal = null;
            Reader = null;

            try
            {
                if (!string.IsNullOrEmpty(FileNameWPath))
                {
                    Reader = new StreamReader(FileNameWPath);
                    RetVal = Deserialize(Reader, ObjectType);
                    Reader.Close();
                }
            }
            catch (Exception ex)
            {
                if (Reader != null)
                {
                    Reader.Close();
                }
                throw ex;
            }

            return RetVal;
        }

        public static Object LoadFromText(System.Type ObjectType, string XmlContent)
        {
            Object RetVal;
            MemoryStream Stream;
            StreamReader Reader;
            StreamWriter Writer;

            RetVal = null;
            Stream = new MemoryStream();
            Reader = new StreamReader(Stream);
            Writer = new StreamWriter(Stream);

            try
            {
                if (!string.IsNullOrEmpty(XmlContent))
                {
                    Writer.Write(XmlContent);
                    Writer.Flush();
                    Stream.Position = 0;
                    RetVal = Deserialize(Reader, ObjectType);

                    Writer.Close();
                    Reader.Close();
                }
            }
            catch (Exception ex)
            {
                Writer.Close();
                Reader.Close();
                throw ex;
            }

            return RetVal;
        }

        public static Object LoadFromXmlDocument(System.Type ObjectType, XmlDocument XmlDocument)
        {
            Object RetVal;
            MemoryStream Stream;
            StreamReader Reader;
            StreamWriter Writer;

            RetVal = null;
            Stream = new MemoryStream();
            Reader = new StreamReader(Stream);
            Writer = new StreamWriter(Stream);

            try
            {
                if (XmlDocument != null)
                {
                    Writer.Write(XmlDocument.InnerXml);
                    Writer.Flush();
                    Stream.Position = 0;
                    RetVal = Deserialize(Reader, ObjectType);

                    Writer.Close();
                    Reader.Close();
                }
            }
            catch (Exception ex)
            {
                Reader.Close();
                throw ex;
            }

            return RetVal;
        }

        #endregion "--Public--"

        #region "--Private--"

        private static Object Deserialize(StreamReader Reader, Type ObjectType)
        {
            Object RetVal;
            XmlSerializer Serializer;

            Serializer = new XmlSerializer(ObjectType);

            Serializer.UnknownElement += new XmlElementEventHandler(Serializer_UnknownElement);

            if (ObjectType != typeof(StructureSpecificTimeSeriesDataType))
            {
                Serializer.UnknownAttribute += new XmlAttributeEventHandler(Serializer_UnknownAttribute);
            }

            Serializer.UnreferencedObject += new UnreferencedObjectEventHandler(Serializer_UnreferencedObject);

            RetVal = Serializer.Deserialize(Reader);

            return RetVal;
        }

        private static void Handle_Unknown_SDMX_Event(XmlElementEventArgs eElementEventArgs,XmlAttributeEventArgs eAttributeEventArgs,UnreferencedObjectEventArgs eObjectEventArgs)
        {
            Exception ex;
            if (eElementEventArgs != null)
            {
                ex = new Exception(Constants.InvalidElement+ eElementEventArgs.Element.Name);
            }
            else if (eAttributeEventArgs != null)
            {
                ex = new Exception(Constants.InvalidAttribute + eAttributeEventArgs.Attr.Name);
            }
            else if (eObjectEventArgs != null)
            {
                ex = new Exception(Constants.LoadingException);
            }
            else
            {
                ex = new Exception(Constants.LoadingException);            
            }
           
            throw ex;
        }

        #endregion "--Private--"

        #region "--Events--"

        private static void Serializer_UnknownElement(object sender, XmlElementEventArgs e)
        {
            Handle_Unknown_SDMX_Event(e,null,null);
        }

        private static void Serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            if (e.Attr.Name.ToLower() != "xsi:type" && e.Attr.Name.ToLower() != "xsi:schemalocation")
            {
                Handle_Unknown_SDMX_Event(null, e, null);
            }
        }

        private static void Serializer_UnreferencedObject(object sender, UnreferencedObjectEventArgs e)
        {
            Handle_Unknown_SDMX_Event(null, null,e);
        }

        #endregion "--Events--"

    }
}
