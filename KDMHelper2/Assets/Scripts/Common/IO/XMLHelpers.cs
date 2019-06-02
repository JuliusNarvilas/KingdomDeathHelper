using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Common.IO
{
    public class XMLHelpers
    {
        private static XmlSerializerNamespaces s_namespaces;

        private static XmlSerializerNamespaces Namespaces
        {
            get
            {
                if(s_namespaces == null)
                {
                    s_namespaces = new XmlSerializerNamespaces();
                    s_namespaces.Add(string.Empty, string.Empty);
                }
                return s_namespaces;
            }
        }

        public static void Serialize(object item, Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(item.GetType());
            serializer.Serialize(stream, item, Namespaces);
        }

        public static void Serialize(object item, XmlWriter writer)
        {
            XmlSerializer serializer = new XmlSerializer(item.GetType());
            serializer.Serialize(writer, item, Namespaces);
        }

        public static T Deserialise<T>(Stream stream)
        {
            return (T)Deserialise(stream, typeof(T));
        }

        public static object Deserialise(Stream stream, Type type)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            return serializer.Deserialize(stream);
        }

        public static T Deserialise<T>(XmlReader reader)
        {
            return (T)Deserialise(reader, typeof(T));
        }

        public static object Deserialise(XmlReader reader, Type type)
        {
            XmlSerializer serializer = new XmlSerializer(type);
            return serializer.Deserialize(reader);
        }
    }
}
