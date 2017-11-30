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


/*

TargetType|Filter|Sort|Count

TargetType:
* Survivor
* Gear
* Resources

Filter (Key op value):
op:
* eq
* gt
* lt
* ge
* le

Sort (Key1; Key2 ...)

Count (number from the top or "ALL")

*/

namespace Game.Properties
{

    public enum EEffectType
    {
        ConstantNumerical,
        DepartureNumerical,
        ArrivalNumerical,
        Constant,
        Departure,
        Arrival
    }

    [Serializable]
    public struct PropertyEffectData
    {
        public string TargetId;
        public EEffectType EffectType;
        public string Value;
    }

    public class EnumPropertyData
    {
        public string Content;

        public List<PropertyEffectData> Effects;

        /*
        public SerializableNumericalPropertyModifier ApplyModifiers(SerializableNumericalProperty i_NumericalData)
        {
            int count = Effects.Count;
            for(int i = 0; i < count; ++i)
            {
                switch(Effects[i].EffectType)
                {
                    case EEffectType.ArrivalNumerical:
                        //TODO;
                        break;
                    case EEffectType.ConstantNumerical:
                        //TODO
                        break;
                    case EEffectType.DepartureNumerical:
                        break;
                }
            }
        }
        */

        public void ApplyModifiers()
        {

        }
    }

    /*
    public class KDMReferencingNumericalPropertyModifier : IKDMNumericalPropertyModifier
    {

    }

    */

    public class KDMNumericalProperty : NumericalProperty<int, int, KDMNumericalPropertyModifier>, IXmlSerializable
    {
        private static XmlSerializerNamespaces s_Namespaces;
        private static XmlSerializer s_CustomModSerializer;
        private static XmlSerializer s_EnumModSerializer;

        static KDMNumericalProperty()
        {
            s_Namespaces = new XmlSerializerNamespaces();
            s_Namespaces.Add(string.Empty, string.Empty);
            s_CustomModSerializer = new XmlSerializer(typeof(CustomNumericalPropertyModifier));
            s_EnumModSerializer = new XmlSerializer(typeof(EnumReferencedNumericalPropertyModifier));
        }

        public KDMNumericalProperty() : base(new NumericalPropertyIntData(0))
        {
        }
        public KDMNumericalProperty(int i_Value) : base(new NumericalPropertyIntData(i_Value))
        {
        }
        public KDMNumericalProperty(INumericalPropertyData<int> i_Value) : base(i_Value)
        {
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
            
            reader.ReadStartElement("BaseValue");
            m_BaseValue = reader.ReadContentAsInt();
            reader.ReadEndElement();

            reader.ReadStartElement("Modifiers");
            if (!reader.IsEmptyElement)
            {
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    if(reader.Name == typeof(CustomNumericalPropertyModifier).Name)
                    {
                        var newMod = s_CustomModSerializer.Deserialize(reader) as CustomNumericalPropertyModifier;
                        m_Modifiers.Add(newMod);
                    }
                    else if(reader.Name == typeof(EnumReferencedNumericalPropertyModifier).Name)
                    {
                        var newMod = s_EnumModSerializer.Deserialize(reader) as EnumReferencedNumericalPropertyModifier;
                        m_Modifiers.Add(newMod);
                    }
                    reader.MoveToContent();
                }
            }

            Update();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("BaseValue", m_BaseValue.ToString());

            writer.WriteStartElement("Modifiers");
            int count = m_Modifiers.Count;
            for(int i = 0; i < count; ++i)
            {
                var mod = m_Modifiers[i];
                if(mod is CustomNumericalPropertyModifier)
                {
                    s_CustomModSerializer.Serialize(writer, mod, s_Namespaces);
                }
            }

            writer.WriteEndElement();
        }
    }
}
