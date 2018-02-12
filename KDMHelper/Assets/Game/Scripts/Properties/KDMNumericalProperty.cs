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

    public class KDMNumericalProperty : NumericalProperty<int, KDMNumericalPropertyContext, KDMNumericalPropertyModifierReader>, IXmlSerializable
    {
        private static XmlSerializerNamespaces s_Namespaces;
        private static List<Type> s_RegisteredModifierTypes = new List<Type>();

        static KDMNumericalProperty()
        {
            s_Namespaces = new XmlSerializerNamespaces();
            s_Namespaces.Add(string.Empty, string.Empty);
        }

        public static void RegisterModifierType(Type i_Type)
        {
            s_RegisteredModifierTypes.Add(i_Type);
        }


        public KDMNumericalProperty() : base(new NumericalPropertyIntData(0))
        {
            m_Name = string.Empty;
        }

        public KDMNumericalProperty(string i_Name) : base(new NumericalPropertyIntData(0))
        {
            m_Name = i_Name;
        }
        public KDMNumericalProperty(string i_Name, int i_Value) : base(new NumericalPropertyIntData(i_Value))
        {
            m_Name = i_Name;
        }
        public KDMNumericalProperty(string i_Name, INumericalPropertyData<int> i_Value) : base(i_Value)
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
            
            reader.ReadStartElement("BaseValue");
            m_BaseValue = reader.ReadContentAsInt();
            reader.ReadEndElement();

            reader.ReadStartElement("Modifiers");
            if (!reader.IsEmptyElement)
            {
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    Type modifierType = null;
                    string typeName = reader.Name;
                    int count = s_RegisteredModifierTypes.Count;
                    for(int i = 0; i < count; i++)
                    {
                        if(s_RegisteredModifierTypes[i].Name == typeName)
                        {
                            modifierType = s_RegisteredModifierTypes[i];
                            break;
                        }
                    }

                    if (modifierType != null)
                    {
                        XmlSerializer modSerializer = new XmlSerializer(modifierType);
                        var newMod = modSerializer.Deserialize(reader) as INumericalPropertyModifier<int, KDMNumericalPropertyContext, KDMNumericalPropertyModifierReader>;
                        if (newMod != null)
                        {
                            m_Modifiers.Add(newMod);
                        }
                    }
                    else
                    {
                        reader.Skip();
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
                XmlSerializer modSerializer = new XmlSerializer(mod.GetType());
                modSerializer.Serialize(writer, mod, s_Namespaces);
            }

            writer.WriteEndElement();
        }
    }
}
