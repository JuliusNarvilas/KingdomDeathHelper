using System;
using System.Xml;

namespace Game.Properties.Modifiers
{
    public class KDMCustomNumericalPropertyModifier : KDMNumericalPropertyModifier
    {
        private string m_Name;
        private string m_Description;

        public KDMCustomNumericalPropertyModifier() : base(0)
        {
        }

        public KDMCustomNumericalPropertyModifier(int i_Value) : base(i_Value)
        {
        }
        public KDMCustomNumericalPropertyModifier(int i_Value, string i_Name, string i_Description) : base(i_Value)
        {
            m_Name = i_Name;
            m_Description = i_Description;
        }

        public override string GetDescription()
        {
            return m_Description;
        }

        public override string GetName()
        {
            return m_Name;
        }
        public override void ReadXml(XmlReader reader)
        {
            bool wasEmpty = reader.IsEmptyElement;

            reader.Read();

            if (wasEmpty)
            {
                return;
            }

            reader.ReadStartElement("Value");
            m_Value = reader.ReadContentAsInt();
            reader.ReadEndElement();

            m_Name = reader.ReadElementString("Name");
            m_Description = reader.ReadElementString("Description");
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Value", m_Value.ToString());
            writer.WriteElementString("Name", m_Name);
            writer.WriteElementString("Description", m_Description);
        }
    }
}
