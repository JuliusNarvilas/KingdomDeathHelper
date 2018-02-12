using Common.Properties.Numerical;
using Common.Properties.Numerical.Specializations;
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Game.Properties.Modifiers
{
    public class KDMNumericalPropertyModifierReader : INumericalPropertyModifierReader<int>
    {
        public int Value;
        public string Name;
        public string Description;

        public int GetModifier()
        {
            return Value;
        }
    }

    public abstract class KDMNumericalPropertyModifier : INumericalPropertyModifier<int, KDMNumericalPropertyContext, KDMNumericalPropertyModifierReader>, IXmlSerializable
    {
        protected int m_Order;
        protected string m_Name;

        public string GetName() { return m_Name; }
        
        public KDMNumericalPropertyModifier(string i_Name, int i_Order = 1)
        {
            m_Order = i_Order;
            m_Name = i_Name;
        }

        public abstract KDMNumericalPropertyModifierReader GetReader(KDMNumericalPropertyContext i_Context);

        public abstract void Update(ref NumericalPropertyChangeEventStruct<int, KDMNumericalPropertyContext, KDMNumericalPropertyModifierReader> i_EventData);

        public int GetOrder()
        {
            return m_Order;
        }

        public virtual bool SerializeForProperty(KDMNumericalProperty i_Property)
        {
            return true;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }
        public virtual void ReadXml(XmlReader reader)
        {
            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
            {
                return;
            }

            m_Order = reader.ReadElementContentAsInt("Order", string.Empty);
            m_Name = reader.ReadElementString("Name");
        }
        public virtual void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Order", m_Order.ToString());
            writer.WriteElementString("Name", m_Name);
        }
    }
}
