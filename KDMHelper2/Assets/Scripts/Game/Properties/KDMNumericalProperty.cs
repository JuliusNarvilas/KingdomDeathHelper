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
using Common.IO;


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
    [Serializable]
    public class KDMNumericalProperty : ObservableNumericalProperty<int, KDMNumericalPropertyContext, KDMNumericalPropertyModifierReader>, IXmlSerializable
    {

        public KDMNumericalProperty() : base(new NumericalPropertyIntData(0))
        {
        }
        public KDMNumericalProperty(int i_Value) : base(new NumericalPropertyIntData(i_Value))
        {
        }
        public KDMNumericalProperty(INumericalPropertyData<int> i_Value) : base(i_Value)
        {
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

            reader.ReadStartElement("Modifiers");
            if (!reader.IsEmptyElement)
            {
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    reader.ReadStartElement("Modifier");
                    string typeString = reader.ReadElementString("Type");

                    Type modifierType = null;
                    try
                    {
                        modifierType = Type.GetType(typeString);
                    }
                    catch { }
                    
                    if (modifierType != null)
                    {
                        var newMod = XMLHelpers.Deserialise(reader, modifierType) as INumericalPropertyModifier<int, KDMNumericalPropertyContext, KDMNumericalPropertyModifierReader>;
                        if (newMod != null)
                        {
                            m_Modifiers.Add(newMod);
                        }
                    }
                    else
                    {
                        reader.Skip();
                    }

                    reader.ReadEndElement();
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
                writer.WriteStartElement("Modifier");

                var mod = m_Modifiers[i];
                writer.WriteElementString("Type", mod.GetType().AssemblyQualifiedName);

                XMLHelpers.Serialize(mod, writer);

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }
    }
}
