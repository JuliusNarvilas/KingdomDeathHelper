using Common.Properties.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Game.Properties.Modifiers
{
    public class EnumReferencedNumericalPropertyModifier : KDMNumericalPropertyModifier
    {
        protected EnumProperty<string> m_EnumProperty;


        public EnumReferencedNumericalPropertyModifier() : base(0, 1)
        {
        }

        public EnumReferencedNumericalPropertyModifier(EnumProperty<string> i_ReferencedProperty, int i_ModifierValue) : base(i_ModifierValue, 1)
        {
            m_EnumProperty = i_ReferencedProperty;
        }

        public override string GetDescription()
        {
            return m_EnumProperty.Content;
        }

        public override string GetName()
        {
            return m_EnumProperty.GetValue();
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

            string factoryName = reader.ReadElementString("Factory");
            var factory = EnumProperty<string>.Generator.FindFactory(factoryName);
            string propertyName = reader.ReadElementString("Name");
            m_EnumProperty = factory.Find(propertyName);
        }

        public override void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Value", m_Value.ToString());
            writer.WriteElementString("Factory", m_EnumProperty.Factory.Name);
            writer.WriteElementString("Name", m_EnumProperty.GetValue());
        }
    }
}
