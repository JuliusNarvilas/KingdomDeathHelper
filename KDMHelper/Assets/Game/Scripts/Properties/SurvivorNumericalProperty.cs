using Game.Model.Character;

namespace Game.Properties
{
    public class SurvivorNumericalProperty : KDMNumericalProperty
    {
        private Survivor m_Survivor;
        
        public SurvivorNumericalProperty(Survivor i_Survivor, string i_Name) : base(i_Name)
        {
            m_Survivor = i_Survivor;
        }

        public SurvivorNumericalProperty(Survivor i_Survivor, string i_Name, int i_Value) : base(i_Name, i_Value)
        {
            m_Survivor = i_Survivor;
        }

        public override KDMNumericalPropertyContext GetContext()
        {
            var result = base.GetContext();
            result.Survivor = m_Survivor;
            return result;
        }
    }
}
