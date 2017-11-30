using Common.Properties.Numerical;
using Common.Properties.Numerical.Specializations;
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Game.Properties.Modifiers
{
    public abstract class KDMNumericalPropertyModifier : INumericalPropertyModifier<int, int, KDMNumericalPropertyModifier>, INumericalPropertyModifierReader<int>, IXmlSerializable
    {
        protected int m_Value;
        protected int m_Order;

        public KDMNumericalPropertyModifier()
        {
            m_Value = 0;
            m_Order = 1;
        }

        public KDMNumericalPropertyModifier(int i_Value, int i_Order = 1)
        {
            m_Value = i_Value;
            m_Order = i_Order;
        }

        public int GetModifier()
        {
            return m_Value;
        }

        public int GetOrder()
        {
            return m_Order;
        }

        public XmlSchema GetSchema()
        {
            return null;
        }
        
        public KDMNumericalPropertyModifier GetReader()
        {
            return this;
        }

        public void Update(ref NumericalPropertyChangeEventStruct<int, int, KDMNumericalPropertyModifier> i_EventData)
        {
            i_EventData.NewModifier += m_Value;
        }

        public abstract string GetName();
        public abstract string GetDescription();
        
        public abstract void ReadXml(XmlReader reader);
        public abstract void WriteXml(XmlWriter writer);
    }
}
