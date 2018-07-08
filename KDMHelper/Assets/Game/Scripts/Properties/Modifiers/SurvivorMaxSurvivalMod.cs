using System;
using System.Xml;
using Common.Properties.Numerical;

namespace Game.Properties.Modifiers
{
    public class SurvivorMaxSurvivalMod : KDMNumericalPropertyModifier
    {
        protected string m_Description = "Survival cannot exceed the settlement Max Survival.";

        public SurvivorMaxSurvivalMod() : base("Survival Limit")
        {
        }

        public override KDMNumericalPropertyModifierReader GetReader(KDMNumericalPropertyContext i_Context)
        {
            return new KDMNumericalPropertyModifierReader()
            {
                Value = 0,
                Description = m_Description,
                Name = m_Name
            };
        }

        public override void Update(ref NumericalPropertyChangeEventStruct<int, KDMNumericalPropertyContext, KDMNumericalPropertyModifierReader> i_EventData)
        {
            int currentValue = i_EventData.NumericalProperty.GetValue() + i_EventData.NewModifier;
            //int maxSurvival = i_EventData.Context.Settlement.MaxSurvival.GetValue();
            int maxSurvival = 1;
            if (currentValue > maxSurvival)
            {
                i_EventData.NumericalProperty.SetBaseValue(maxSurvival - i_EventData.NewModifier);
            }
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
