﻿using System;
using System.Xml;
using Common.Properties.Numerical;

namespace Game.Properties.Modifiers
{
    public class CustomNumericalPropertyModifier : KDMNumericalPropertyModifier
    {
        protected string m_Description;
        protected int m_Value;

        public CustomNumericalPropertyModifier(string i_Name) : base(i_Name)
        {
        }

        public CustomNumericalPropertyModifier(string i_Name, int i_Value) : base(i_Name)
        {
            m_Value = i_Value;
        }
        public CustomNumericalPropertyModifier(int i_Value, string i_Name, string i_Description) : base(i_Name)
        {
            m_Description = i_Description;
        }

        public override KDMNumericalPropertyModifierReader GetReader(KDMNumericalPropertyContext i_Context)
        {
            return new KDMNumericalPropertyModifierReader()
            {
                Value = m_Value,
                Name = m_Name,
                Description = m_Description
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
            
            m_Description = reader.ReadElementString("Description");
        }

        public override void Update(ref NumericalPropertyChangeEventStruct<int, KDMNumericalPropertyContext, KDMNumericalPropertyModifierReader> i_EventData)
        {
            throw new NotImplementedException();
        }

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);
            writer.WriteElementString("Value", m_Value.ToString());
            writer.WriteElementString("Description", m_Description);
        }
    }
}
