using System;
using System.Xml;
using Common.Properties.Numerical;

namespace Game.Properties.Modifiers
{
    public class SurvivorFrenzyMod : KDMNumericalPropertyModifier
    {
        protected string m_Description = "A brain trauma result. A survivor who suffers this is Frenzied until the end of the Showdown Phase. Gain +1 strength token, +1 speed token, and 1d5 insanity. Ignore the slow special rule on melee weapons. A frenzied survivor may not spend survival or use fighting arts, Weapon specialization, or weapon mastery. A survivor may be Frenzied multiple times.";

        public SurvivorFrenzyMod() : base("Frenzy")
        {
        }

        public override KDMNumericalPropertyModifierReader GetReader(KDMNumericalPropertyContext i_Context)
        {
            return new KDMNumericalPropertyModifierReader()
            {
                //Value = i_Context.Survivor.Frenzy.GetValue(),
                Value = 0,
                Description = m_Description,
                Name = m_Name
            };
        }

        public override void Update(ref NumericalPropertyChangeEventStruct<int, KDMNumericalPropertyContext, KDMNumericalPropertyModifierReader> i_EventData)
        {
            //i_EventData.NewModifier += i_EventData.Context.Survivor.Frenzy.GetValue();
        }

        public override bool SerializeForProperty(KDMNumericalProperty i_Property)
        {
            return false;
        }

        public override void ReadXml(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        public override void WriteXml(XmlWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
