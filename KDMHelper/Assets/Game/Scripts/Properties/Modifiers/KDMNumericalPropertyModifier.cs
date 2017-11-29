using Common.Properties.Numerical.Specializations;
using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Game.Properties.Modifiers
{
    public abstract class KDMNumericalPropertyModifier : NumericalPropertyIntModifier<int>, IXmlSerializable
    {
        public KDMNumericalPropertyModifier(int i_Value) : base(i_Value, 1)
        {
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public abstract string GetName();
        public abstract string GetDescription();
        public abstract void ReadXml(XmlReader reader);
        public abstract void WriteXml(XmlWriter writer);
    }
}
