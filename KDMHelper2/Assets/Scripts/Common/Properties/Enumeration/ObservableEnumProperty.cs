using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Common.Properties.Enumeration
{
    /// <summary>
    /// A simple property that represents a unique value (per generator used).
    /// </summary>
    [Serializable]
    public class ObservableEnumProperty : Property<EnumProperty>, IObservablePropertySimpleSubscription, IXmlSerializable
    {

        public event PropertyChangeHandler<EnumProperty, ObservableEnumProperty> ChangeSubscription;
        public event Action<object> SimpleChangeSubscription;

        public ObservableEnumProperty() : base(null)
        { }
        public ObservableEnumProperty(EnumProperty i_Value) : base(i_Value)
        {}

        public void SetValue(EnumProperty i_Value)
        {
            EnumProperty temp = m_Value;
            m_Value = i_Value;
            if (ChangeSubscription != null)
            {
                ChangeSubscription(temp, i_Value, this);
            }
            if (SimpleChangeSubscription != null)
            {
                SimpleChangeSubscription(this);
            }
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

            string factoryName = reader.ReadElementString("Factory");
            string key = reader.ReadElementString("Key");

            m_Value = EnumProperty.Find(factoryName, key);
        }

        public void WriteXml(XmlWriter writer)
        {
            if(m_Value != null)
            {
                writer.WriteElementString("Factory", m_Value.Factory.Name);
                writer.WriteElementString("Key", m_Value.GetValue().ToString());
            }
        }
    }
}
