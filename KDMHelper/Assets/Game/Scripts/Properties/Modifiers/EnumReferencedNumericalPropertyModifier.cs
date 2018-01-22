using Common.Properties.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Common.Properties.Numerical;

namespace Game.Properties.Modifiers
{
    public class EnumReferencedNumericalPropertyModifier : KDMNumericalPropertyModifier
    {
        protected EnumProperty<string> m_EnumProperty;
        protected int m_Value;

        public string GetDescription() { return m_EnumProperty.Content; }

        public EnumReferencedNumericalPropertyModifier(EnumProperty<string> i_ReferencedProperty, int i_ModifierValue) : base(i_ReferencedProperty.GetValue())
        {
            m_EnumProperty = i_ReferencedProperty;
        }

        public override KDMNumericalPropertyModifierReader GetReader(KDMNumericalPropertyContext i_Context)
        {
            return new KDMNumericalPropertyModifierReader()
            {
                Value = m_Value,
                Name = m_Name,
                Description = m_EnumProperty.Content
            };
        }

        public override void ReadXml(XmlReader reader)
        {
            bool wasEmpty = reader.IsEmptyElement;
            reader.Read();

            if (wasEmpty)
            {
                return;
            }
            base.ReadXml(reader);

            reader.ReadStartElement("Value");
            m_Value = reader.ReadContentAsInt();
            reader.ReadEndElement();

            string factoryName = reader.ReadElementString("Factory");
            var factory = EnumProperty<string>.Generator.FindFactory(factoryName);
            m_EnumProperty = factory.Find(m_Name);
        }

        public override void Update(ref NumericalPropertyChangeEventStruct<int, KDMNumericalPropertyContext, KDMNumericalPropertyModifierReader> i_EventData)
        {
            throw new NotImplementedException();
        }

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);

            writer.WriteElementString("Value", m_Value.ToString());
            writer.WriteElementString("Factory", m_EnumProperty.Factory.Name);
        }
    }
}
