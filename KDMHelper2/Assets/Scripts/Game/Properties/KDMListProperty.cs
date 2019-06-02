using Common.Properties.Numerical;
using System;
using Common.Properties;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using Common.IO;

namespace Game.Properties
{

    [Serializable]
    public class PropertyType
    {
        public Type Type;
        public string FilterData;

        public PropertyType()
        { }

        public PropertyType(Type i_Type, string i_Data = null)
        {
            Type = i_Type;
            FilterData = i_Data;
        }
    }

    [Serializable]
    public class KDMListProperty : IXmlSerializable
    {
        public KDMListProperty()
        { }

        protected List<PropertyType> m_AllowedTypes = new List<PropertyType>();

        public List<PropertyType> AllowedTypes { get { return m_AllowedTypes; } }

        public List<object> Items = new List<object>();



        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();
            if (wasEmpty)
            {
                return;
            }

            reader.ReadStartElement("AllowedTypes");
            if (!reader.IsEmptyElement)
            {
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    reader.ReadStartElement("PropertyType");
                    PropertyType propType = new PropertyType();
                    try
                    {
                        propType.Type = Type.GetType(reader.ReadElementString("Type"));
                    }
                    catch
                    {
                        while (reader.NodeType != XmlNodeType.EndElement)
                        {
                            reader.Read();
                        }
                        continue;
                    }

                    if (reader.Name == "FilterData")
                    {
                        propType.FilterData = reader.ReadElementString("FilterData");
                    }

                    reader.ReadEndElement();
                    reader.MoveToContent();

                    m_AllowedTypes.Add(propType);

                }
            }

            reader.ReadStartElement("Items");
            if (!reader.IsEmptyElement)
            {
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    reader.ReadStartElement("Item");
                    Type propType = null;
                    try
                    {
                        propType = Type.GetType(reader.ReadElementString("Type"));
                    }
                    catch
                    {
                        while (reader.NodeType != XmlNodeType.EndElement)
                        {
                            reader.Read();
                        }
                        continue;
                    }

                    reader.ReadEndElement();
                    reader.MoveToContent();

                    object item = XMLHelpers.Deserialise(reader, propType);
                    if (item != null)
                    {
                        Items.Add(item);
                    }

                }
            }

        }

        public void WriteXml(XmlWriter writer)
        {
            int typeCount = m_AllowedTypes.Count;
            if (typeCount > 0)
            {
                writer.WriteStartElement("AllowedTypes");
                for (int i = 0; i < typeCount; i++)
                {
                    writer.WriteStartElement("PropertyType")
                        ;
                    writer.WriteElementString("Type", m_AllowedTypes[i].Type.FullName);
                    if(!string.IsNullOrEmpty(m_AllowedTypes[i].FilterData))
                    {
                        writer.WriteElementString("FilterData", m_AllowedTypes[i].FilterData);
                    }

                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }

            int itemCount = Items.Count;
            if (itemCount > 0)
            {
                writer.WriteStartElement("Items");
                for (int i = 0; i < itemCount; i++)
                {
                    writer.WriteStartElement("Item");

                    writer.WriteElementString("Type", Items[i].GetType().FullName);

                    XMLHelpers.Serialize(Items[i], writer);

                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
        }
    }
}
