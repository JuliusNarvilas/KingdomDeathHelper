using Common.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using Common.IO;

namespace Game.Model
{
    [Serializable]
    public class NamedProperty : IXmlSerializable
    {
        protected string m_Name;
        public object Property;

        public string GetName()
        {
            return m_Name;
        }
        public virtual void SetName(string newName)
        {
            m_Name = newName;
        }

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
            m_Name = reader.ReadElementString("Name");
            string propTypeStr = reader.ReadElementString("PropertyType");

            Type propType = null;
            try
            {
                propType = Type.GetType(propTypeStr);
            }
            catch { }

            if (propType != null)
            {
                Property = XMLHelpers.Deserialise(reader, propType);
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Name", m_Name);
            writer.WriteElementString("PropertyType", Property.GetType().FullName);
            XMLHelpers.Serialize(Property, writer);
        }

        //public abstract IOrderedEnumerable<IPropertyCollectionGetter> ApplySort(IOrderedEnumerable<IPropertyCollectionGetter> collection, bool asc = true);
    }
    
}
