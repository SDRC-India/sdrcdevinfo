using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Xml;

namespace SDMXObjectModel
{
    public static class Serializer
    {
        #region "--Public--"

        public static void SerializeToFile(System.Type ObjectType, Object Object, string FileNameWPath)
        {
            StreamWriter Writer;

            Writer = null;

            try
            {
                if (!string.IsNullOrEmpty(FileNameWPath))
                {
                    Writer = new StreamWriter(FileNameWPath);
                    Serialize(Writer, ObjectType, Object);
                    Writer.Flush();
                    Writer.Close();
                }
            }
            catch (Exception ex)
            {
                if (Writer != null)
                {
                    Writer.Flush();
                    Writer.Close();
                }
                throw ex;
            }
        }

        public static string SerializeToText(System.Type ObjectType, Object Object)
        {
            string RetVal;
            StreamWriter Writer;
            StreamReader Reader;
            MemoryStream Stream;

            RetVal = string.Empty;
            Stream = new MemoryStream();
            Reader = new StreamReader(Stream);
            Writer = new StreamWriter(Stream);

            try
            {
                if (Object != null && ObjectType != null)
                {
                    Serialize(Writer, ObjectType, Object);
                    Stream.Position = 0;
                    RetVal = Reader.ReadToEnd();

                    Writer.Flush();
                    Writer.Close();
                    Reader.Close();
                }
            }
            catch (Exception ex)
            {
                Writer.Flush();
                Writer.Close();
                Reader.Close();
                throw ex;
            }
            return RetVal;
        }

        public static XmlDocument SerializeToXmlDocument(System.Type ObjectType, Object Object)
        {
            XmlDocument RetVal;
            StreamWriter Writer;
            StreamReader Reader;
            MemoryStream Stream;

            RetVal = new XmlDocument();
            Stream = new MemoryStream();
            Reader = new StreamReader(Stream);
            Writer = new StreamWriter(Stream);

            try
            {
                if (Object != null && ObjectType != null)
                {
                    Serialize(Writer, ObjectType, Object);
                    Stream.Position = 0;
                    RetVal.LoadXml(Reader.ReadToEnd());

                    Writer.Flush();
                    Writer.Close();
                    Reader.Close();
                }
            }
            catch (Exception ex)
            {
                Writer.Flush();
                Writer.Close();
                Reader.Close();
                throw ex;
            }

            return RetVal;
        }

        #endregion "--Public--"

        #region "--Private--"

        private static void Serialize(StreamWriter Writer, Type ObjectType, Object Object)
        {
            XmlSerializer Serializer;
            XmlSerializerNamespaces SerializerNamespaces;

            Serializer = new XmlSerializer(ObjectType);
            SerializerNamespaces = new XmlSerializerNamespaces();

            SerializerNamespaces.Add(Constants.Namespaces.Prefixes.Common, Constants.Namespaces.URLs.Common);
            SerializerNamespaces.Add(Constants.Namespaces.Prefixes.Message, Constants.Namespaces.URLs.Message);
            SerializerNamespaces.Add(Constants.Namespaces.Prefixes.Query, Constants.Namespaces.URLs.Query);
            SerializerNamespaces.Add(Constants.Namespaces.Prefixes.Registry, Constants.Namespaces.URLs.Registry);
            SerializerNamespaces.Add(Constants.Namespaces.Prefixes.Structure, Constants.Namespaces.URLs.Structure);
            SerializerNamespaces.Add(Constants.Namespaces.Prefixes.GenericData, Constants.Namespaces.URLs.GenericData);
            SerializerNamespaces.Add(Constants.Namespaces.Prefixes.StructureSpecificData, Constants.Namespaces.URLs.StructureSpecificData);
            SerializerNamespaces.Add(Constants.Namespaces.Prefixes.XSI, Constants.Namespaces.URLs.XSI);

            Serializer.Serialize(Writer, Object, SerializerNamespaces);
        }

        #endregion "--Private--"

    }
}
