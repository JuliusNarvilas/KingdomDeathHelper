using Common.Properties.Numerical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Properties.Numerical.Data;
using Common.Properties.Enumeration;
using Common;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using Common.Properties.Numerical.Specializations;
using UnityEngine;
using Game.Properties.Modifiers;


namespace Game.Properties
{
    
    public class KDMExhaustibleNumericalProperty : ExhaustibleNumericalProperty<int, KDMNumericalPropertyContext, KDMNumericalPropertyModifierReader>, IXmlSerializable
    {
        private static XmlSerializerNamespaces s_Namespaces;

        static KDMExhaustibleNumericalProperty()
        {
            s_Namespaces = new XmlSerializerNamespaces();
            s_Namespaces.Add(string.Empty, string.Empty);
        }

        public KDMExhaustibleNumericalProperty(string i_Name) : base(new NumericalPropertyIntData(0))
        {
            m_Name = i_Name;
        }
        public KDMExhaustibleNumericalProperty(string i_Name, int i_Value) : base(new NumericalPropertyIntData(i_Value))
        {
            m_Name = i_Name;
        }
        public KDMExhaustibleNumericalProperty(string i_Name, INumericalPropertyData<int> i_Value) : base(i_Value)
        {
            m_Name = i_Name;
        }

        protected string m_Name;

        public string GetName()
        {
            return m_Name;
        }


        public virtual KDMNumericalPropertyContext GetContext()
        {
            return new KDMNumericalPropertyContext()
            {
                Property = this,
                Survivor = null,
                Settlement = Model.Settlement.GetCurrent()
            };
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
            
            m_BaseValue = reader.ReadElementContentAsInt("BaseValue", string.Empty);
            m_Depletion = reader.ReadElementContentAsInt("Depletion", string.Empty);

            reader.ReadStartElement("Modifiers");
            if (!reader.IsEmptyElement)
            {
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    Type modifierType = Type.GetType(reader.Name);
                    if(modifierType != null)
                    {
                        XmlSerializer modSerializer = new XmlSerializer(modifierType);
                        if(typeof(KDMNumericalPropertyModifier).IsAssignableFrom(modifierType))
                        {
                            var newMod = modSerializer.Deserialize(reader) as KDMNumericalPropertyModifier;
                            m_Modifiers.Add(newMod);
                        }
                        else if(typeof(EnumReferencedNumericalPropertyModifier).IsAssignableFrom(modifierType))
                        {
                            var newMod = modSerializer.Deserialize(reader) as KDMNumericalPropertyModifier;
                            m_Modifiers.Add(newMod);
                        }
                    }
                    reader.MoveToContent();
                }
            }

            Update();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("BaseValue", m_BaseValue.ToString());
            writer.WriteElementString("Depletion", m_Depletion.ToString());

            writer.WriteStartElement("Modifiers");
            int count = m_Modifiers.Count;
            for(int i = 0; i < count; ++i)
            {
                var mod = m_Modifiers[i];
                XmlSerializer modSerializer = new XmlSerializer(mod.GetType());
                modSerializer.Serialize(writer, mod, s_Namespaces);
            }

            writer.WriteEndElement();
        }
    }
}
